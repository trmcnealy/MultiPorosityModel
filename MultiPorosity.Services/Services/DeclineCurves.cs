using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Threading;
using System.Threading.Tasks;

using static System.Math;

namespace MultiPorosity.Services
{
    public static class DeclineCurves
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static double[] ExponentialRate(double[] xData,
                                               double   ip,
                                               double   de)
        {
            double[] outputModel = new double[xData.Length];

            double di = -Log(1 - de);

            for(int i = 0; i < xData.Length; i++)
            {
                outputModel[i] = ip * Exp(-di * xData[i]);
            }

            return outputModel;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static double[] HarmonicRate(double[] xData,
                                            double   ip,
                                            double   de)
        {
            double[] outputModel = new double[xData.Length];

            double di = de / (1 - de);

            for(int i = 0; i < xData.Length; i++)
            {
                outputModel[i] = ip / (1 + di * xData[i]);
            }

            return outputModel;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static double[] HorizontalLinearFlow(double[] xData,
                                                    double   dP,
                                                    double   bo,
                                                    double   mu_oil,
                                                    double   h,
                                                    double   xf,
                                                    double   fractureCount,
                                                    double   k,
                                                    double   phi,
                                                    double   ct)
        {
            double[] outputModel = new double[xData.Length];

            for(int i = 0; i < xData.Length; i++)
            {
                outputModel[i] = fractureCount * dP / (4.064 * bo * mu_oil * Sqrt(xData[i]) / (h * xf * Sqrt(k * phi * mu_oil * ct)));
            }

            return outputModel;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static double[] HyperbolicRate(double[] xData,
                                              double   ip,
                                              double   de,
                                              double   beta)
        {
            double[] outputModel = new double[xData.Length];

            double di = 1 /
                        beta *
                        (Pow(1 - de,
                             -beta) -
                         1);

            for(int i = 0; i < xData.Length; i++)
            {
                outputModel[i] = ip *
                                 Pow(1 + beta * di * xData[i],
                                     -1 / beta);
            }

            return outputModel;
        }
    }
}
