using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MultiPorosity.Services
{
    public static class Derivatives
    {
        public delegate double[] GetFunctorArgs(int equation_index);

        public delegate double FunctorDelegate(int             equation_index,
                                               double[]        args,
                                               params double[] additional_args);

        public delegate double JacobianFunctorDelegate(int             equation_index,
                                                       int             arg_index,
                                                       double[]        args,
                                                       params double[] additional_args);

        public delegate double HessianFunctorDelegate(int             equation_index,
                                                      int             arg_index,
                                                      double[]        args,
                                                      params double[] additional_args);

        private const double EpsilonPowOneOverThree = 0.00000605545445239334;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static double step_size()
        {
            return EpsilonPowOneOverThree;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static double increment_dx(in double value)
        {
            if(Math.Abs(value) <= double.Epsilon)
            {
                return step_size();
            }

            return value * step_size();
        }

        /// <summary>
        /// f ′(x i ) =f (x i+1 ) − f (x i−1 )/2h
        /// </summary>
        /// <param name="equation_index"></param>
        /// <param name="arg_index"></param>
        /// <param name="functor"></param>
        /// <param name="args"></param>
        /// <param name="additional_args"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static double CenteredFiniteDifferenceFirstDerivative(int             equation_index,
                                                                     int             arg_index,
                                                                     FunctorDelegate functor,
                                                                     double[]        args,
                                                                     params double[] additional_args)
        {
            int n_args = args.Length;
            
            double[] high_args = new double[n_args];
            double[] low_args  = new double[n_args];

            for(int i0 = 0; i0 < n_args; ++i0)
            {
                high_args[i0] = args[i0];
                low_args[i0]  = args[i0];
            }

            double increment = increment_dx(args[arg_index]);

            high_args[arg_index] += increment;
            low_args[arg_index]  -= increment;

            double high = functor(equation_index, high_args, additional_args);
            double low  = functor(equation_index, low_args,  additional_args);

            return (high - low) / (2.0 * increment);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static List<double> CenteredFiniteDifferenceFirstDerivative(int             equation_index, 
                                                                           FunctorDelegate functor,
                                                                           double[]        args,
                                                                           params double[] additional_args)
        {
            int n_args = args.Length;

            List<double> jacobian = new(n_args);

            for(int arg_index = 0; arg_index < n_args; ++arg_index)
            {
                jacobian.Insert(arg_index, CenteredFiniteDifferenceFirstDerivative(equation_index, arg_index, functor, args, additional_args));
            }

            return jacobian;
        }

        /// <summary>
        /// f ″(x i ) =f (x i+1 ) − 2 f (x i ) + f (x i−1 )/h^2
        /// </summary>
        /// <param name="equation_index"></param>
        /// <param name="arg_index"></param>
        /// <param name="functor"></param>
        /// <param name="args"></param>
        /// <param name="additional_args"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static double CenteredFiniteDifferenceSecondDerivative(int             equation_index,
                                                                      int             arg_index,
                                                                      FunctorDelegate functor,
                                                                      double[]        args,
                                                                      params double[] additional_args)
        {
            int n_args = args.Length;

            double[] high_args = new double[n_args];
            double[] low_args  = new double[n_args];

            for(int i0 = 0; i0 < n_args; ++i0)
            {
                high_args[i0] = args[i0];
                low_args[i0]  = args[i0];
            }

            double increment = increment_dx(args[arg_index]);

            high_args[arg_index] += increment;
            low_args[arg_index]  -= increment;

            double high   = functor(equation_index, high_args, additional_args);
            double center = functor(equation_index, args,      additional_args);
            double low    = functor(equation_index, low_args,  additional_args);

            return (high - (2.0 * center) + low) / (increment * increment);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static List<double> CenteredFiniteDifferenceSecondDerivative(int             equation_index,
                                                                            FunctorDelegate functor,
                                                                            double[]        args,
                                                                            params double[] additional_args)
        {
            int n_args = args.Length;

            List<double> jacobian = new(n_args);

            for(int arg_index = 0; arg_index < n_args; ++arg_index)
            {
                jacobian.Insert(arg_index, CenteredFiniteDifferenceSecondDerivative(equation_index, arg_index, functor, args, additional_args));
            }

            return jacobian;
        }
    }
}