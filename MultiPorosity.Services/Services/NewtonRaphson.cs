using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiPorosity.Services
{
    public static class NewtonRaphson
    {
        public static int MaxSolverIterations = 250;

        private static double RMS(double[] errors)
        {
            int n_args = errors.Length;

            double error = 0.0;

            for(int i0 = 0; i0 < n_args; ++i0)
            {
                if(double.IsNaN(errors[i0]) || double.IsInfinity(errors[i0]))
                {
                    continue;
                }
                error += Math.Pow(errors[i0], 2);
            }

            return Math.Sqrt(error);
        }

        private static double RMS(List<double> errors)
        {
            int n_args = errors.Count;

            double error = 0.0;

            for(int i0 = 0; i0 < n_args; ++i0)
            {
                if(double.IsNaN(errors[i0]) || double.IsInfinity(errors[i0]))
                {
                    continue;
                }
                error += Math.Pow(errors[i0], 2);
            }

            return Math.Sqrt(error);
        }

        /// <summary>
        /// // x(i+1) = x(i) - f'(x) / f''(x)
        /// </summary>
        /// <param name="numberOfEquations"></param>
        /// <param name="get_functor_args"></param>
        /// <param name="functor"></param>
        /// <param name="firstDerivativeFunctor"></param>
        /// <param name="secondDerivativeFunctor"></param>
        /// <param name="constantArgFunctor"></param>
        /// <param name="additional_args"></param>
        /// <returns></returns>
        private static double[][] solve(int                                   numberOfEquations,
                                          Derivatives.GetFunctorArgs          get_functor_args,
                                          Derivatives.FunctorDelegate         functor,
                                          Derivatives.JacobianFunctorDelegate firstDerivativeFunctor,
                                          Derivatives.HessianFunctorDelegate  secondDerivativeFunctor,
                                          Func<int, int, bool>                constantArgFunctor,
                                          params double[]                     additional_args)
        {
            const double error_target   = 1E-09;
            
            double[][] error_iterations = new double[MaxSolverIterations][];
            
            int n_equations = numberOfEquations;
            int n_args      = get_functor_args(0).Length;
            
            double[][] new_equation_args = new double[n_equations][];
            double[] errors            = new double[n_equations];

            
            double rms_error;
            //double delta_arg;
            double functor_result;
            //double firstDerivativeFunctor_result;
            //double secondDerivativeFunctor_result;
            int    iterations = 0;

            do
            {
                error_iterations[iterations] = new double[n_equations];
                    
                for(int i0 = 0; i0 < n_equations; ++i0)
                {
                    new_equation_args[i0] = get_functor_args(i0);
                    
                    functor_result        = functor(i0, new_equation_args[i0], additional_args);

                    if(Math.Abs(functor_result) <= double.Epsilon)
                    {
                        continue;
                    }

                    for(int i1 = 0; i1 < n_args; ++i1)
                    {
                        if(!constantArgFunctor(i0, i1))
                        {
                            //firstDerivativeFunctor_result  = firstDerivativeFunctor(i0, i1, new_equation_args[i0], additional_args);
                            //secondDerivativeFunctor_result = secondDerivativeFunctor(i0, i1, new_equation_args[i0], additional_args);

                            //delta_arg = firstDerivativeFunctor_result;// / secondDerivativeFunctor_result;

                            //if(double.IsNaN(delta_arg) || double.IsInfinity(delta_arg))
                            //{
                            //    continue;
                            //}

                            new_equation_args[i0][i1] += functor_result / 10000.0;

                            if(new_equation_args[i0][i1] < double.Epsilon)
                            {
                                new_equation_args[i0][i1] = 0.0;
                            }
                        }
                    }
                    
                    error_iterations[iterations][i0] = errors[i0] = functor(i0, new_equation_args[i0], additional_args);
                }
                
                rms_error = RMS(errors);
                
                //error_iterations[iterations] = rms_error;
                
                ++iterations;
                
                if(iterations >= MaxSolverIterations)
                {
                    break;
                }
            } while(rms_error >= error_target);

            for(int i0 = 0; i0 < n_equations; ++i0)
            {
                for(int i1 = 0; i1 < n_args; ++i1)
                {
                    if(Math.Abs(new_equation_args[i0][i1]) <= double.Epsilon                 ||
                       Compare.AreEqual(new_equation_args[i0][i1], double.MaxValue) ||
                       double.IsNaN(new_equation_args[i0][i1])                      ||
                       double.IsInfinity(new_equation_args[i0][i1]))
                    {
                        new_equation_args[i0][i1] = 0.0;
                    }
                }
            }

            return new_equation_args;
        }

        public static (List<double> errors, double[][] new_args) Solve(int                         numberOfEquations,
                                                                       Derivatives.GetFunctorArgs  get_functor_args,
                                                                       Derivatives.FunctorDelegate functor,
                                                                       Func<int, int, bool>        constantArgFunctor,
                                                                       params double[]             additional_args)
        {
            double FirstDerivative(int             equation_index,
                                   int             arg_index,
                                   double[]        args,
                                   params double[] _additional_args)
            {
                return Derivatives.CenteredFiniteDifferenceFirstDerivative(equation_index, arg_index, functor, args, _additional_args);
            }

            double SecondDerivative(int             equation_index,
                                    int             arg_index,
                                    double[]        args,
                                    params double[] _additional_args)
            {
                return Derivatives.CenteredFiniteDifferenceSecondDerivative(equation_index, arg_index, functor, args, _additional_args);
            }

            double[][] new_args = solve(numberOfEquations, get_functor_args, functor, FirstDerivative, SecondDerivative, constantArgFunctor, additional_args);

            List<double> errors = new (new_args.Length);
            
            
            

            return (errors, new_args);
        }
    }
}