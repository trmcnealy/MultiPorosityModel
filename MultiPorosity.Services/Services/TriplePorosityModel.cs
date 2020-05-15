using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Kokkos;

using MultiPorosity.Models;
using MultiPorosity.Models.BoundConstraints;

using NumericalMethods.DataStorage;

namespace MultiPorosity.Services
{
    internal class TriplePorosityModelException : Exception
    {
        public TriplePorosityModelException()
        {
        }

        public TriplePorosityModelException(string message)
            : base(message)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        internal static void Throw()
        {
            throw new TriplePorosityModelException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        internal static void Throw(string message)
        {
            throw new TriplePorosityModelException(message);
        }
    }

    public sealed class TriplePorosityModel<T, TExecutionSpace> : IDisposable
        where T : unmanaged
        where TExecutionSpace : IExecutionSpace, new()
    {
        //private static readonly DataTypeKind       dataType;
        private static readonly ExecutionSpaceKind executionSpace;

        private readonly IntPtr _instance;

        private readonly InverseTransformPrecision _inverseTransformPrecision;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        static TriplePorosityModel()
        {
            //if(typeof(T) == typeof(float))
            //{
            //    dataType = DataTypeKind.Single;
            //}
            //else if(typeof(T) == typeof(double))
            //{
            //    dataType = DataTypeKind.Double;
            //}
            //else
            //{
            //    TriplePorosityModelException.Throw("T is not a supported type.");
            //}

            executionSpace = ExecutionSpace<TExecutionSpace>.GetKind();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public TriplePorosityModel(MultiPorosityData<T>         mpd,
                                   in InverseTransformPrecision inverseTransformPrecision = InverseTransformPrecision.High)
        {
            _inverseTransformPrecision = inverseTransformPrecision;

            if(typeof(T) == typeof(float))
            {
                switch(executionSpace)
                {
                    case ExecutionSpaceKind.Serial:
                    {
                        _instance = MultiPorosityLibrary.CreateSolverSingle(ExecutionSpaceKind.Serial, mpd.Instance, _inverseTransformPrecision);

                        break;
                    }
                    case ExecutionSpaceKind.OpenMP:
                    {
                        _instance = MultiPorosityLibrary.CreateSolverSingle(ExecutionSpaceKind.OpenMP, mpd.Instance, _inverseTransformPrecision);

                        break;
                    }
                    case ExecutionSpaceKind.Cuda:
                    {
                        _instance = MultiPorosityLibrary.CreateSolverSingle(ExecutionSpaceKind.Cuda, mpd.Instance, _inverseTransformPrecision);

                        break;
                    }
                    default: throw new ArgumentOutOfRangeException(nameof(executionSpace), executionSpace, null);
                }
            }
            else if(typeof(T) == typeof(double))
            {
                switch(executionSpace)
                {
                    case ExecutionSpaceKind.Serial:
                    {
                        _instance = MultiPorosityLibrary.CreateSolverDouble(ExecutionSpaceKind.Serial, mpd.Instance, _inverseTransformPrecision);

                        break;
                    }
                    case ExecutionSpaceKind.OpenMP:
                    {
                        _instance = MultiPorosityLibrary.CreateSolverDouble(ExecutionSpaceKind.OpenMP, mpd.Instance, _inverseTransformPrecision);

                        break;
                    }
                    case ExecutionSpaceKind.Cuda:
                    {
                        _instance = MultiPorosityLibrary.CreateSolverDouble(ExecutionSpaceKind.Cuda, mpd.Instance, _inverseTransformPrecision);

                        break;
                    }
                    default: throw new ArgumentOutOfRangeException(nameof(executionSpace), executionSpace, null);
                }
            }
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        private void ReleaseUnmanagedResources()
        {
            if(typeof(T) == typeof(float))
            {
                MultiPorosityLibrary.FreeSolverSingle(_instance, executionSpace, _inverseTransformPrecision);
            }
            else if(typeof(T) == typeof(double))
            {
                MultiPorosityLibrary.FreeSolverDouble(_instance, executionSpace, _inverseTransformPrecision);
            }
        }

        ~TriplePorosityModel()
        {
            ReleaseUnmanagedResources();
        }

        public MultiPorosityResult<float, TExecutionSpace> HistoryMatch(ParticleSwarmOptimizationOptionsF options,
                                                                        View<float, TExecutionSpace>      dataView,
                                                                        View<float, TExecutionSpace>      timeView,
                                                                        View<float, TExecutionSpace>      weightsView,
                                                                        BoundConstraints<float>[]         boundConstraints)
        {
            BoundConstraintsSingle[] constraints = Array.ConvertAll(boundConstraints, BoundConstraints<T>.To);

            IntPtr result = MultiPorosityLibrary.SolverHistoryMatchSingle(options,
                                                                          dataView.Pointer,
                                                                          timeView.Pointer,
                                                                          weightsView.Pointer,
                                                                          constraints,
                                                                          _instance,
                                                                          executionSpace,
                                                                          _inverseTransformPrecision);

            MultiPorosityLibrary.SolverGetResultsSingle(result, executionSpace, out IntPtr args, out float error, out IntPtr cachedData);

            NdArray argsNdArray = View<float, TExecutionSpace>.RcpConvert(args, 1);

            View<float, TExecutionSpace> argsView = new View<float, TExecutionSpace>(new NativePointer(args, sizeof(float) * argsNdArray.Extent(0)), argsNdArray);

            DataCache dataTable = Marshal.PtrToStructure<DataCache>(cachedData);

            MultiPorosityResult<float, TExecutionSpace> results = new MultiPorosityResult<float, TExecutionSpace>(argsView, error, dataTable);

            return results;
        }

        public MultiPorosityResult<double, TExecutionSpace> HistoryMatch(ParticleSwarmOptimizationOptions options,
                                                                         View<double, TExecutionSpace>    dataView,
                                                                         View<double, TExecutionSpace>    timeView,
                                                                         View<double, TExecutionSpace>    weightsView,
                                                                         BoundConstraints<double>[]       boundConstraints)
        {
            BoundConstraintsDouble[] constraints = Array.ConvertAll(boundConstraints, BoundConstraints<T>.To);

            IntPtr result = MultiPorosityLibrary.SolverHistoryMatchDouble(options,
                                                                          dataView.Pointer,
                                                                          timeView.Pointer,
                                                                          weightsView.Pointer,
                                                                          constraints,
                                                                          _instance,
                                                                          executionSpace,
                                                                          _inverseTransformPrecision);

            MultiPorosityLibrary.SolverGetResultsDouble(result, executionSpace, out IntPtr args, out double error, out IntPtr cachedData);

            NdArray argsNdArray = View<double, TExecutionSpace>.RcpConvert(args, 1);

            View<double, TExecutionSpace> argsView = new View<double, TExecutionSpace>(new NativePointer(args, sizeof(double) * argsNdArray.Extent(0)), argsNdArray);

            DataCache dataTable = Marshal.PtrToStructure<DataCache>(cachedData);

            MultiPorosityResult<double, TExecutionSpace> results = new MultiPorosityResult<double, TExecutionSpace>(argsView, error, dataTable);

            return results;
        }

        public View<float, TExecutionSpace> Calculate(View<float, TExecutionSpace> timeView,
                                                      View<float, TExecutionSpace> argsView)
        {
            IntPtr result = MultiPorosityLibrary.SolverCalculateSingle(timeView.Pointer, argsView.Pointer, _instance, executionSpace, _inverseTransformPrecision);

            NdArray ndArray = View<float, TExecutionSpace>.RcpConvert(result, 1);

            View<float, TExecutionSpace> qs = new View<float, TExecutionSpace>(new NativePointer(result, sizeof(float) * timeView.Size()), ndArray);

            return qs;
        }

        public View<double, TExecutionSpace> Calculate(View<double, TExecutionSpace> timeView,
                                                       View<double, TExecutionSpace> argsView)
        {
            IntPtr result = MultiPorosityLibrary.SolverCalculateDouble(timeView.Pointer, argsView.Pointer, _instance, executionSpace, _inverseTransformPrecision);

            NdArray ndArray = View<double, TExecutionSpace>.RcpConvert(result, 1);

            View<double, TExecutionSpace> qs = new View<double, TExecutionSpace>(new NativePointer(result, sizeof(double) * timeView.Size()), ndArray);

            return qs;
        }
    }
}
