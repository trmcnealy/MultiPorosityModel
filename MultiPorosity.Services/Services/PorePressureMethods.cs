using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MultiPorosity.Services
{
    public static class PorePressureMethods
    {
        //Rn =R0 exp(bZ)
        //DTn =DTm+(DTml-DTm)exp(-cZ)

        //P_p = S - (S-P_hyd)*(R_log/R_n)^1.2
        //P_p = S - (S-P_hyd)*(ΔT_n/ΔT_log)^3.0

        //sigma = sigma_0*exp(-beta*sigma*v)
        //P_p = S_v-(1/(beta*ln(phi_o/phi)) where phi = 1-(Δt_ma/Δt)^1/f

        //Nomenclature
        //e	= base of a natural logarithm, e = 2.718281828 …
        //f	= acoustic formation factor, used in Eq. 5
        //Pa	= Pore pressure at deptha
        //Phyd	= hydrostatic pressure, MPa, psi, lbm/gal
        //Pp = pore pressure, MPa, psi, lbm/gal
        //Pz = Pore pressure at depth z, MPa, psi, lbm/gal
        //Rlog = measured value of resistivity, ohm-m
        //Rn = normal value of resistivity, ohm-m
        //S = total stress, MPa, psi
        //Sa = stress at depth a, MPa, psi, lbm/gal
        //Sz = axial stress along a wellbore, MPa, psi
        //Δtma	= matrix transit time, μs/ft
        //ΔT = temperature difference between the fluid in a well and the adjacent rock
        //ΔTlog	= measured value of sonic transit-time at a given depth, μs/ft
        //ΔTn = normal value of sonic transit-time at a given depth, μs/ft
        //σ = Terzaghi effective stress, MPa, psi

        //Superscripts
        //β = coefficient multiplying the effective vertical stress in Athy's relationship, Eq. 4

        //σv	= effective vertical stress in Athy’s relationship, Eq. 4

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CalculateTVD(float depth, float gradient, float datum)
        {
            return (depth * gradient);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CalculateTVDSS(float depth, float gradient, float datum)
        {
            return (depth * gradient) + (datum * gradient);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] CalculateTVDSS(float[] depth, float gradient, float datum)
        {
            float[] pressure = new float[depth.Length];

            for(int i = 0; i < pressure.Length; i++)
            {
                pressure[i] = CalculateTVDSS(depth[i], gradient, datum);
            }

            return pressure;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<float> CalculateTVDSS(IEnumerable<float> depths, float gradient, float datum)
        {
            foreach(var depth in depths)
            {
                yield return CalculateTVDSS(depth, gradient, datum);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double CalculateTVD(double depth, double gradient, double datum)
        {
            return (depth * gradient);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double CalculateTVDSS(double depth, double gradient, double datum)
        {
            return (depth * gradient) + (datum * gradient);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[] CalculateTVDSS(double[] depth, double gradient, double datum)
        {
            double[] pressure = new double[depth.Length];

            for(int i = 0; i < pressure.Length; i++)
            {
                pressure[i] = CalculateTVDSS(depth[i], gradient, datum);
            }

            return pressure;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<double> CalculateTVDSS(IEnumerable<double> depths, double gradient, double datum)
        {
            foreach(var depth in depths)
            {
                yield return CalculateTVDSS(depth, gradient, datum);
            }
        }
    }
}