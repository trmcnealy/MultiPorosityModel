using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

using MultiPorosity.Models;

namespace MultiPorosity.Tool
{
    public class ThreePhaseParkerVanGenuchtenParams
    {
        public double vgAlpha { get; set; }

        public double vgM { get; set; }

        public double vgN { get; set; }

        public double Swr { get; set; }

        public double Snr { get; set; }

        public double Sgr { get; set; }

        public double Swrx { get; set; } // Swr + Snr

        public double betaNW { get; set; }

        public double betaGN { get; set; }

        public bool krRegardsSnr { get; set; }

        public ThreePhaseParkerVanGenuchtenParams()
        {
            betaNW = 1.0;
            betaGN = 1.0;
        }
    }

    public static class ThreePhaseParkerVanGenuchtenCapillaryPressure
    {
        public const int numPhases = 3;

        public static readonly int gasPhaseIdx        = 0; //gasPhaseIdx;
        public static readonly int wettingPhaseIdx    = 1; //wettingPhaseIdx;
        public static readonly int nonWettingPhaseIdx = 2; //nonWettingPhaseIdx;

        /// <summary>
        /// Leverett J-function
        /// https://en.wikipedia.org/wiki/Leverett_J-function
        /// </summary>
        /// <param name="p_c">is the capillary pressure (in pascal)</param>
        /// <param name="Sw">is the water saturation measured as a fraction</param>
        /// <param name="k">is the permeability (measured in m²)</param>
        /// <param name="phi">is the porosity (0-1)</param>
        /// <param name="gamma">is the surface tension (in N/m)</param>
        /// <param name="theta">is the contact angle</param>
        /// <returns></returns>
        public static double CapillaryPressureFunction(double Sw)
        {
            double k        = double.NaN;
            double phi      = double.NaN;
            double Pcgw     = double.NaN;
            double sigma_gw = double.NaN;

            return (Pcgw / sigma_gw) * Math.Sqrt(k / phi);
        }

        public static double LeverettJFunction(Func<double, double> p_c,
                                               double               Sw,
                                               double               k,
                                               double               phi,
                                               double               gamma,
                                               double               theta)
        {
            return (p_c(Sw) * Math.Sqrt(k / phi)) / (gamma * Math.Cos(theta));
        }

        /// <summary>
        /// Capillary pressure between the gas and the non-wetting liquid (i.e., oil) phase.
        /// p_{c,gn} = p_g - p_n
        /// </summary>
        /// <param name="params"></param>
        /// <param name="fluidState"></param>
        /// <returns></returns>
        public static double pcgn(ThreePhaseParkerVanGenuchtenParams @params,
                                  PoreSaturation                     fluidState)
        {
            double PC_VG_REG = 0.01;

            // sum of liquid saturations
            var St = fluidState.saturation(wettingPhaseIdx) + fluidState.saturation(nonWettingPhaseIdx);

            double Se = (St - @params.Swrx) / (1.0 - @params.Swrx);

            // regularization
            if(Se < 0.0)
            {
                Se = 0.0;
            }

            if(Se > 1.0)
            {
                Se = 1.0;
            }

            double x;

            if(Se > PC_VG_REG && Se < 1 - PC_VG_REG)
            {
                x = Math.Pow(Se, -1 / @params.vgM) - 1;

                return Math.Pow(x, 1.0 - @params.vgM) / @params.vgAlpha;
            }

            // value and derivative at regularization point
            double Se_regu = 0.0;

            if(Se <= PC_VG_REG)
            {
                Se_regu = PC_VG_REG;
            }
            else
            {
                Se_regu = 1 - PC_VG_REG;
            }

            x = Math.Pow(Se_regu, -1 / @params.vgM) - 1;
            double pc = Math.Pow(x, 1.0 / @params.vgN) / @params.vgAlpha;

            double pc_prime = Math.Pow(x, 1 / @params.vgN - 1) * Math.Pow(Se_regu, -1 / @params.vgM - 1) / (-@params.vgM) / @params.vgAlpha / (1 - @params.Sgr - @params.Swrx) / @params.vgN;

            // evaluate tangential
            return ((Se - Se_regu) * pc_prime + pc) / @params.betaGN;
        }

        /// <summary>
        /// Capillary pressure between the non-wetting liquid (i.e., oil) and the wetting liquid (i.e., water) phase.
        /// p_{c,nw} = p_n - p_w
        /// </summary>
        public static double pcnw(ThreePhaseParkerVanGenuchtenParams @params,
                                  PoreSaturation                     fluidState)
        {
            double Sw = fluidState.saturation(wettingPhaseIdx);
            double Se = (Sw - @params.Swr) / (1.0 - @params.Snr);

            double PC_VG_REG = 0.01;

            // regularization
            if(Se < 0.0)
            {
                Se = 0.0;
            }

            if(Se > 1.0)
            {
                Se = 1.0;
            }

            double x;

            if(Se > PC_VG_REG && Se < 1 - PC_VG_REG)
            {
                x = Math.Pow(Se, -1 / @params.vgM) - 1.0;
                x = Math.Pow(x, 1 - @params.vgM);

                return x / @params.vgAlpha;
            }

            // value and derivative at regularization point
            double Se_regu = 0.0;

            if(Se <= PC_VG_REG)
            {
                Se_regu = PC_VG_REG;
            }
            else
            {
                Se_regu = 1.0 - PC_VG_REG;
            }

            x = Math.Pow(Se_regu, -1 / @params.vgM) - 1;
            double pc = Math.Pow(x, 1 / @params.vgN) / @params.vgAlpha;

            double pc_prime = Math.Pow(x, 1 / @params.vgN - 1) * Math.Pow(Se_regu, -1.0 / @params.vgM - 1) / (-@params.vgM) / @params.vgAlpha / (1 - @params.Snr - @params.Swr) / @params.vgN;

            // evaluate tangential
            return ((Se - Se_regu) * pc_prime + pc) / @params.betaNW;
        }

        /// <summary>
        /// The relative permeability for the wetting phase of the medium implied by van Genuchten's parameterization.
        /// </summary>
        public static double krw(ThreePhaseParkerVanGenuchtenParams @params,
                                 PoreSaturation                     fluidState)
        {
            double Sw = fluidState.saturation(wettingPhaseIdx);
            // transformation to effective saturation
            double Se = (Sw - @params.Swr) / (1 - @params.Swr);

            // regularization
            if(Se > 1.0)
            {
                return 1.0;
            }

            if(Se < 0.0)
            {
                return 0.0;
            }

            double r = 1.0 - Math.Pow(1 - Math.Pow(Se, 1 / @params.vgM), @params.vgM);

            return Math.Sqrt(Se) * r * r;
        }

        /// <summary>
        /// The relative permeability for the non-wetting phase due to the model of Parker et al. (1987).
        /// </summary>
        public static double krn(ThreePhaseParkerVanGenuchtenParams @params,
                                 PoreSaturation                     fluidState)
        {
            double Sn  = fluidState.saturation(nonWettingPhaseIdx);
            double Sw  = fluidState.saturation(wettingPhaseIdx);
            double Swe = Math.Min((Sw      - @params.Swr) / (1 - @params.Swr), 1.0);
            double Ste = Math.Min((Sw + Sn - @params.Swr) / (1 - @params.Swr), 1.0);

            // regularization
            if(Swe <= 0.0)
            {
                Swe = 0.0;
            }

            if(Ste <= 0.0)
            {
                Ste = 0.0;
            }

            if(Ste - Swe <= 0.0)
            {
                return 0.0;
            }

            double krn_ = 0.0;
            krn_ =  Math.Pow(1 - Math.Pow(Swe, 1 / @params.vgM), @params.vgM);
            krn_ -= Math.Pow(1 - Math.Pow(Ste, 1 / @params.vgM), @params.vgM);
            krn_ *= krn_;

            if(@params.krRegardsSnr)
            {
                // regard Snr in the permeability of the non-wetting
                // phase, see Helmig1997
                double resIncluded = Math.Max(Math.Min(Sw - @params.Snr / (1 - @params.Swr), 1.0), 0.0);
                krn_ *= Math.Sqrt(resIncluded);
            }
            else
            {
                krn_ *= Math.Sqrt(Sn / (1 - @params.Swr));
            }

            return krn_;
        }

        /// <summary>
        /// The relative permeability for the non-wetting phase of the medium implied by van Genuchten's parameterization.
        /// </summary>
        public static double krg(ThreePhaseParkerVanGenuchtenParams @params,
                                 PoreSaturation                     fluidState)
        {
            double Sg = fluidState.saturation(gasPhaseIdx);
            double Se = Math.Min(((1 - Sg) - @params.Sgr) / (1 - @params.Sgr), 1.0);

            // regularization
            if(Se > 1.0)
            {
                return 0.0;
            }

            if(Se < 0.0)
            {
                return 1.0;
            }

            double scaleFactor = 1.0;

            if(Sg <= 0.1)
            {
                scaleFactor = (Sg - @params.Sgr) / (0.1 - @params.Sgr);

                if(scaleFactor < 0.0)
                {
                    return 0.0;
                }
            }

            return scaleFactor * Math.Pow(1 - Se, 1.0 / 3.0) * Math.Pow(1 - Math.Pow(Se, 1 / @params.vgM), 2 * @params.vgM);
        }
    }
}