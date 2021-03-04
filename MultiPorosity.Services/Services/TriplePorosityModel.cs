using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Kokkos;

using MultiPorosity.DataStorage;
using MultiPorosity.Models;
using MultiPorosity.Models.BoundConstraints;
using MultiPorosity.Services.Models;

using NumericalMethods.DataStorage;

using PlatformApi;

using Prism.Events;
using Prism.Ioc;

using ParticleSwarmOptimizationOptions = MultiPorosity.Models.ParticleSwarmOptimizationOptions;
using TriplePorosityOptimizationResults = MultiPorosity.Services.Models.TriplePorosityOptimizationResults;

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
        private static          IEventAggregator?  _eventAggregator;

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
            _eventAggregator           = Prism.Ioc.ContainerLocator.Container?.Resolve<IEventAggregator>();

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

        public View<float, TExecutionSpace> Calculate(View<float, TExecutionSpace> timeView,
                                                      View<float, TExecutionSpace> argsView)
        {
            IntPtr result = MultiPorosityLibrary.SolverCalculateSingle(timeView.Pointer, argsView.Pointer, _instance, executionSpace, _inverseTransformPrecision);

            NdArray ndArray = View<float, TExecutionSpace>.RcpConvert(result, 2);

            View<float, TExecutionSpace> qs = new(new NativePointer(result, sizeof(float) * timeView.Size()), ndArray);

            return qs;
        }

        public View<double, TExecutionSpace> Calculate(View<double, TExecutionSpace> timeView,
                                                       View<double, TExecutionSpace> argsView)
        {
            IntPtr result = MultiPorosityLibrary.SolverCalculateDouble(timeView.Pointer, argsView.Pointer, _instance, executionSpace, _inverseTransformPrecision);

            NdArray ndArray = View<double, TExecutionSpace>.RcpConvert(result, 2);

            View<double, TExecutionSpace> qs = new(new NativePointer(result, sizeof(double) * timeView.Size()), ndArray);

            return qs;
        }

        //public MultiPorosityResult<float, TExecutionSpace> HistoryMatch(ParticleSwarmOptimizationOptionsF options,
        //                                                                View<float, TExecutionSpace>      dataView,
        //                                                                View<float, TExecutionSpace>      timeView,
        //                                                                View<float, TExecutionSpace>      weightsView,
        //                                                                BoundConstraints<float>[]         boundConstraints)
        //{
        //    BoundConstraintsSingle[] constraints = Array.ConvertAll(boundConstraints, BoundConstraints<T>.To);
        //
        //    IntPtr result = MultiPorosityLibrary.SolverHistoryMatchSingle(options,
        //                                                                  dataView.Pointer,
        //                                                                  timeView.Pointer,
        //                                                                  weightsView.Pointer,
        //                                                                  constraints,
        //                                                                  _instance,
        //                                                                  executionSpace,
        //                                                                  _inverseTransformPrecision);
        //
        //    MultiPorosityLibrary.SolverGetResultsSingle(result, executionSpace, out IntPtr args, out float error, out IntPtr cachedData);
        //
        //    NdArray argsNdArray = View<float, TExecutionSpace>.RcpConvert(args, 1);
        //
        //    View<float, TExecutionSpace> argsView = new View<float, TExecutionSpace>(new NativePointer(args, sizeof(float) * argsNdArray.Extent(0)), argsNdArray);
        //
        //    DataCache dataTable = Marshal.PtrToStructure<DataCache>(cachedData);
        //
        //    MultiPorosityResult<float, TExecutionSpace> results = new MultiPorosityResult<float, TExecutionSpace>(argsView, error, dataTable);
        //
        //    return results;
        //}

        public MultiPorosityResult<double, TExecutionSpace> HistoryMatch(ParticleSwarmOptimizationOptions options,
                                                                         View<double, TExecutionSpace>    dataView,
                                                                         View<double, TExecutionSpace>    timeView,
                                                                         View<double, TExecutionSpace>    weightsView,
                                                                         BoundConstraints<double>[]       boundConstraints)
        {
            unsafe
            {
                NativeArray<BoundConstraintsDouble> constraints = Array.ConvertAll(boundConstraints, BoundConstraints<double>.To);

                nint constraintsPtr = (nint)Unsafe.AsPointer(ref constraints.Instance);

                options.ResultCallback = &ResultCallback;

                nint result = MultiPorosityLibrary.SolverHistoryMatchDouble(options,
                                                                            dataView.Pointer,
                                                                            timeView.Pointer,
                                                                            weightsView.Pointer,
                                                                            constraintsPtr,
                                                                            _instance,
                                                                            executionSpace,
                                                                            _inverseTransformPrecision);

                MultiPorosityLibrary.SolverGetResultsDouble(result, executionSpace, out IntPtr args, out double error, out IntPtr cachedData);

                NdArray argsNdArray = View<double, TExecutionSpace>.RcpConvert(args, 1);

                View<double, TExecutionSpace> argsView = new View<double, TExecutionSpace>(new NativePointer(args, sizeof(double) * argsNdArray.Extent(0)), argsNdArray);

                DataCache dataTable = cachedData == IntPtr.Zero ? new DataCache() : Marshal.PtrToStructure<DataCache>(cachedData);

                MultiPorosityResult<double, TExecutionSpace> results = new MultiPorosityResult<double, TExecutionSpace>(argsView, error, dataTable);

                return results;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static long ResultCallbackOffset(long i0,
                                                 long i1,
                                                 long N1)
        {
            return i1 + (N1 * i0);
        }

        public static unsafe void ResultCallback(long    nRows,
                                                 long    nColumns,
                                                 double* data)
        {
            long   iteration;
            long   swarmIndex;
            long   particleIndex;
            double residual;
            double km;
            double kmVelocity;
            double kF;
            double kFVelocity;
            double kf;
            double kfVelocity;
            double ye;
            double yeVelocity;
            double lF;
            double lFVelocity;
            double lf;
            double lfVelocity;
            double sk;
            double skVelocity;

            Engineering.DataSource.Array<MultiPorosity.Services.Models.TriplePorosityOptimizationResults> results = new(nRows);

            int index;

            if(nColumns == 16)
            {
                for(int row = 0; row < nRows; ++row)
                {
                    index = 0;

                    iteration     = (long)data[ResultCallbackOffset(row, index++, nColumns)];
                    swarmIndex    = (long)data[ResultCallbackOffset(row, index++, nColumns)];
                    particleIndex = (long)data[ResultCallbackOffset(row, index++, nColumns)];
                    residual      = data[ResultCallbackOffset(row, index++, nColumns)];
                    km            = data[ResultCallbackOffset(row, index++, nColumns)];
                    kmVelocity    = data[ResultCallbackOffset(row, index++, nColumns)];
                    kF            = data[ResultCallbackOffset(row, index++, nColumns)];
                    kFVelocity    = data[ResultCallbackOffset(row, index++, nColumns)];
                    kf            = data[ResultCallbackOffset(row, index++, nColumns)];
                    kfVelocity    = data[ResultCallbackOffset(row, index++, nColumns)];
                    ye            = data[ResultCallbackOffset(row, index++, nColumns)];
                    yeVelocity    = data[ResultCallbackOffset(row, index++, nColumns)];
                    lF            = data[ResultCallbackOffset(row, index++, nColumns)];
                    lFVelocity    = data[ResultCallbackOffset(row, index++, nColumns)];
                    lf            = data[ResultCallbackOffset(row, index++, nColumns)];
                    lfVelocity    = data[ResultCallbackOffset(row, index,   nColumns)];

                    results.Add(new MultiPorosity.Services.Models.TriplePorosityOptimizationResults(iteration,
                                                                                                    swarmIndex,
                                                                                                    particleIndex,
                                                                                                    residual,
                                                                                                    km,
                                                                                                    kmVelocity,
                                                                                                    kF,
                                                                                                    kFVelocity,
                                                                                                    kf,
                                                                                                    kfVelocity,
                                                                                                    ye,
                                                                                                    yeVelocity,
                                                                                                    lF,
                                                                                                    lFVelocity,
                                                                                                    lf,
                                                                                                    lfVelocity,
                                                                                                    0.0,
                                                                                                    0.0));
                }
            }
            else
            {
                for(int row = 0; row < nRows; ++row)
                {
                    index = 0;

                    iteration     = (long)data[ResultCallbackOffset(row, index++, nColumns)];
                    swarmIndex    = (long)data[ResultCallbackOffset(row, index++, nColumns)];
                    particleIndex = (long)data[ResultCallbackOffset(row, index++, nColumns)];
                    residual      = data[ResultCallbackOffset(row, index++, nColumns)];
                    km            = data[ResultCallbackOffset(row, index++, nColumns)];
                    kmVelocity    = data[ResultCallbackOffset(row, index++, nColumns)];
                    kF            = data[ResultCallbackOffset(row, index++, nColumns)];
                    kFVelocity    = data[ResultCallbackOffset(row, index++, nColumns)];
                    kf            = data[ResultCallbackOffset(row, index++, nColumns)];
                    kfVelocity    = data[ResultCallbackOffset(row, index++, nColumns)];
                    ye            = data[ResultCallbackOffset(row, index++, nColumns)];
                    yeVelocity    = data[ResultCallbackOffset(row, index++, nColumns)];
                    lF            = data[ResultCallbackOffset(row, index++, nColumns)];
                    lFVelocity    = data[ResultCallbackOffset(row, index++, nColumns)];
                    lf            = data[ResultCallbackOffset(row, index++, nColumns)];
                    lfVelocity    = data[ResultCallbackOffset(row, index++, nColumns)];
                    sk            = data[ResultCallbackOffset(row, index++, nColumns)];
                    skVelocity    = data[ResultCallbackOffset(row, index,   nColumns)];

                    results.Add(new MultiPorosity.Services.Models.TriplePorosityOptimizationResults(iteration,
                                                                                                    swarmIndex,
                                                                                                    particleIndex,
                                                                                                    residual,
                                                                                                    km,
                                                                                                    kmVelocity,
                                                                                                    kF,
                                                                                                    kFVelocity,
                                                                                                    kf,
                                                                                                    kfVelocity,
                                                                                                    ye,
                                                                                                    yeVelocity,
                                                                                                    lF,
                                                                                                    lFVelocity,
                                                                                                    lf,
                                                                                                    lfVelocity,
                                                                                                    sk,
                                                                                                    skVelocity));
                }
            }

            _eventAggregator?.GetEvent<TriplePorosityOptimizationResultsEvent>().Publish(results);
        }
    }

    public static class TriplePorosityModel
    {
        private static volatile bool isRunning;

        public static MultiPorosityModelResults Calculate(Project            project,
                                                          ExecutionSpaceKind ExecutionSpace)
        {
            MultiPorosityModelResults results = new();

            if(!isRunning)
            {
                isRunning = true;

                InitArguments arguments = new(OpenMP.NumberOfThreads, -1, Cuda.Id, OpenMP.DisableWarnings)
                {
                    ndevices = Cuda.NumberOfDevices, skip_device = Cuda.SkipDevice
                };

                using(ScopeGuard.Get(arguments))
                {
                    using ReservoirProperties<double> reservoir = new(ExecutionSpace);
                    reservoir.Length                = project.MultiPorosityProperties.ReservoirProperties.Length;
                    reservoir.Width                 = project.MultiPorosityProperties.ReservoirProperties.Width;
                    reservoir.Thickness             = project.MultiPorosityProperties.ReservoirProperties.Thickness;
                    reservoir.Porosity              = project.MultiPorosityProperties.ReservoirProperties.Porosity;
                    reservoir.Permeability          = project.MultiPorosityProperties.ReservoirProperties.Permeability;
                    reservoir.Compressibility       = project.MultiPorosityProperties.ReservoirProperties.Compressibility;
                    reservoir.BottomholeTemperature = project.MultiPorosityProperties.ReservoirProperties.BottomholeTemperature;
                    reservoir.InitialPressure       = project.MultiPorosityProperties.ReservoirProperties.InitialPressure;

                    using WellProperties<double> wellProperties = new(ExecutionSpace);
                    wellProperties.LateralLength      = project.MultiPorosityProperties.WellProperties.LateralLength;
                    wellProperties.BottomholePressure = project.MultiPorosityProperties.WellProperties.BottomholePressure;

                    using FractureProperties<double> fracture = new(ExecutionSpace);
                    fracture.Count        = project.MultiPorosityProperties.FractureProperties.Count;
                    fracture.Width        = project.MultiPorosityProperties.FractureProperties.Width;
                    fracture.Height       = project.MultiPorosityProperties.FractureProperties.Height;
                    fracture.HalfLength   = project.MultiPorosityProperties.FractureProperties.HalfLength;
                    fracture.Porosity     = project.MultiPorosityProperties.FractureProperties.Porosity;
                    fracture.Permeability = project.MultiPorosityProperties.FractureProperties.Permeability;
                    fracture.Skin         = project.MultiPorosityProperties.FractureProperties.Skin;

                    using NaturalFractureProperties<double> natural_fracture = new(ExecutionSpace);
                    natural_fracture.Count        = project.MultiPorosityProperties.NaturalFractureProperties.Count;
                    natural_fracture.Width        = project.MultiPorosityProperties.NaturalFractureProperties.Width;
                    natural_fracture.Porosity     = project.MultiPorosityProperties.NaturalFractureProperties.Porosity;
                    natural_fracture.Permeability = project.MultiPorosityProperties.NaturalFractureProperties.Permeability;

                    using Pvt<double> pvt = new();
                    pvt.OilSaturation              = project.MultiPorosityProperties.Pvt.OilSaturation;
                    pvt.OilApiGravity              = project.MultiPorosityProperties.Pvt.OilApiGravity;
                    pvt.OilViscosity               = project.MultiPorosityProperties.Pvt.OilViscosity;
                    pvt.OilFormationVolumeFactor   = project.MultiPorosityProperties.Pvt.OilFormationVolumeFactor;
                    pvt.OilCompressibility         = project.MultiPorosityProperties.Pvt.OilCompressibility;
                    pvt.WaterSaturation            = project.MultiPorosityProperties.Pvt.WaterSaturation;
                    pvt.WaterSpecificGravity       = project.MultiPorosityProperties.Pvt.WaterSpecificGravity;
                    pvt.WaterViscosity             = project.MultiPorosityProperties.Pvt.WaterViscosity;
                    pvt.WaterFormationVolumeFactor = project.MultiPorosityProperties.Pvt.WaterFormationVolumeFactor;
                    pvt.WaterCompressibility       = project.MultiPorosityProperties.Pvt.WaterCompressibility;
                    pvt.GasSaturation              = project.MultiPorosityProperties.Pvt.GasSaturation;
                    pvt.GasSpecificGravity         = project.MultiPorosityProperties.Pvt.GasSpecificGravity;
                    pvt.GasViscosity               = project.MultiPorosityProperties.Pvt.GasViscosity;
                    pvt.GasFormationVolumeFactor   = project.MultiPorosityProperties.Pvt.GasFormationVolumeFactor;
                    pvt.GasCompressibilityFactor   = project.MultiPorosityProperties.Pvt.GasCompressibilityFactor;
                    pvt.GasCompressibility         = project.MultiPorosityProperties.Pvt.GasCompressibility;

                    using RelativePermeabilities<double> relativePermeabilities = new();
                    relativePermeabilities.MatrixOil            = project.MultiPorosityProperties.RelativePermeabilities.MatrixOil;
                    relativePermeabilities.MatrixWater          = project.MultiPorosityProperties.RelativePermeabilities.MatrixWater;
                    relativePermeabilities.MatrixGas            = project.MultiPorosityProperties.RelativePermeabilities.MatrixGas;
                    relativePermeabilities.FractureOil          = project.MultiPorosityProperties.RelativePermeabilities.FractureOil;
                    relativePermeabilities.FractureWater        = project.MultiPorosityProperties.RelativePermeabilities.FractureWater;
                    relativePermeabilities.FractureGas          = project.MultiPorosityProperties.RelativePermeabilities.FractureGas;
                    relativePermeabilities.NaturalFractureOil   = project.MultiPorosityProperties.RelativePermeabilities.NaturalFractureOil;
                    relativePermeabilities.NaturalFractureWater = project.MultiPorosityProperties.RelativePermeabilities.NaturalFractureWater;
                    relativePermeabilities.NaturalFractureGas   = project.MultiPorosityProperties.RelativePermeabilities.NaturalFractureGas;

                    MultiPorosityData<double> mpd = new(ExecutionSpace);
                    mpd.ReservoirProperties       = reservoir;
                    mpd.WellProperties            = wellProperties;
                    mpd.FractureProperties        = fracture;
                    mpd.NaturalFractureProperties = natural_fracture;
                    mpd.Pvt                       = pvt;
                    mpd.RelativePermeability      = relativePermeabilities;

                    switch(ExecutionSpace)
                    {
                        case ExecutionSpaceKind.OpenMP:
                        {
                            using(TriplePorosityModel<double, OpenMP> tpm = new(mpd))
                            {
                                using View<double, OpenMP> timeView = new("timeView", (int)project.MultiPorosityModelParameters.Days);

                                for(ulong i0 = 0; i0 < timeView.Extent(0); ++i0)
                                {
                                    timeView[i0] = i0 + 1.0;
                                }

                                using View<double, OpenMP> argsView = new("argsView", 7);

                                /*km*/
                                argsView[0] = project.MultiPorosityModelParameters.MatrixPermeability;
                                /*kF*/
                                argsView[1] = project.MultiPorosityModelParameters.HydraulicFracturePermeability;
                                /*kf*/
                                argsView[2] = project.MultiPorosityModelParameters.NaturalFracturePermeability;
                                /*ye*/
                                argsView[3] = project.MultiPorosityModelParameters.HydraulicFractureHalfLength;
                                /*LF*/
                                argsView[4] = project.MultiPorosityModelParameters.HydraulicFractureSpacing;
                                /*Lf*/
                                argsView[5] = project.MultiPorosityModelParameters.NaturalFractureSpacing;
                                /*sk*/
                                argsView[6] = project.MultiPorosityModelParameters.Skin;

                                using View<double, OpenMP> resultsView = tpm.Calculate(timeView, argsView);

                                results.MatrixPermeability            = project.MultiPorosityModelParameters.MatrixPermeability;
                                results.HydraulicFracturePermeability = project.MultiPorosityModelParameters.HydraulicFracturePermeability;
                                results.NaturalFracturePermeability   = project.MultiPorosityModelParameters.NaturalFracturePermeability;
                                results.HydraulicFractureHalfLength   = project.MultiPorosityModelParameters.HydraulicFractureHalfLength;
                                results.HydraulicFractureSpacing      = project.MultiPorosityModelParameters.HydraulicFractureSpacing;
                                results.NaturalFractureSpacing        = project.MultiPorosityModelParameters.NaturalFractureSpacing;
                                results.Skin                          = project.MultiPorosityModelParameters.Skin;

                                List<Models.MultiPorosityModelProduction> production = new((int)project.MultiPorosityModelParameters.Days);

                                for(ulong i0 = 0; i0 < resultsView.Extent(0); ++i0)
                                {
                                    production.Add(new Models.MultiPorosityModelProduction(timeView[i0], resultsView[i0, 0], resultsView[i0, 1], resultsView[i0, 2]));
                                }

                                results.Production = new(production);
                            }

                            break;
                        }
                        case ExecutionSpaceKind.Cuda:
                        {
                            using(TriplePorosityModel<double, Cuda> tpm = new(mpd))
                            {
                                using View<double, Cuda> timeView = new("timeView", (int)project.MultiPorosityModelParameters.Days);

                                for(ulong i0 = 0; i0 < timeView.Extent(0); ++i0)
                                {
                                    timeView[i0] = i0 + 1.0;
                                }

                                using View<double, Cuda> argsView = new("argsView", 7);

                                /*km*/
                                argsView[0] = project.MultiPorosityModelParameters.MatrixPermeability;
                                /*kF*/
                                argsView[1] = project.MultiPorosityModelParameters.HydraulicFracturePermeability;
                                /*kf*/
                                argsView[2] = project.MultiPorosityModelParameters.NaturalFracturePermeability;
                                /*ye*/
                                argsView[3] = project.MultiPorosityModelParameters.HydraulicFractureHalfLength;
                                /*LF*/
                                argsView[4] = project.MultiPorosityModelParameters.HydraulicFractureSpacing;
                                /*Lf*/
                                argsView[5] = project.MultiPorosityModelParameters.NaturalFractureSpacing;
                                /*sk*/
                                argsView[6] = project.MultiPorosityModelParameters.Skin;

                                using View<double, Cuda> resultsView = tpm.Calculate(timeView, argsView);

                                results.MatrixPermeability            = project.MultiPorosityModelParameters.MatrixPermeability;
                                results.HydraulicFracturePermeability = project.MultiPorosityModelParameters.HydraulicFracturePermeability;
                                results.NaturalFracturePermeability   = project.MultiPorosityModelParameters.NaturalFracturePermeability;
                                results.HydraulicFractureHalfLength   = project.MultiPorosityModelParameters.HydraulicFractureHalfLength;
                                results.HydraulicFractureSpacing      = project.MultiPorosityModelParameters.HydraulicFractureSpacing;
                                results.NaturalFractureSpacing        = project.MultiPorosityModelParameters.NaturalFractureSpacing;
                                results.Skin                          = project.MultiPorosityModelParameters.Skin;

                                List<Models.MultiPorosityModelProduction> production = new((int)project.MultiPorosityModelParameters.Days);

                                for(ulong i0 = 0; i0 < resultsView.Extent(0); ++i0)
                                {
                                    production.Add(new Models.MultiPorosityModelProduction(timeView[i0], resultsView[i0, 0], resultsView[i0, 1], resultsView[i0, 2]));
                                }

                                results.Production = new(production);
                            }

                            break;
                        }
                        case ExecutionSpaceKind.Serial:
                        default:
                        {
                            using(TriplePorosityModel<double, Serial> tpm = new(mpd))
                            {
                                using View<double, Serial> timeView = new("timeView", (int)project.MultiPorosityModelParameters.Days);

                                for(ulong i0 = 0; i0 < timeView.Extent(0); ++i0)
                                {
                                    timeView[i0] = i0 + 1.0;
                                }

                                using View<double, Serial> argsView = new("argsView", 7);

                                /*km*/
                                argsView[0] = project.MultiPorosityModelParameters.MatrixPermeability;
                                /*kF*/
                                argsView[1] = project.MultiPorosityModelParameters.HydraulicFracturePermeability;
                                /*kf*/
                                argsView[2] = project.MultiPorosityModelParameters.NaturalFracturePermeability;
                                /*ye*/
                                argsView[3] = project.MultiPorosityModelParameters.HydraulicFractureHalfLength;
                                /*LF*/
                                argsView[4] = project.MultiPorosityModelParameters.HydraulicFractureSpacing;
                                /*Lf*/
                                argsView[5] = project.MultiPorosityModelParameters.NaturalFractureSpacing;
                                /*sk*/
                                argsView[6] = project.MultiPorosityModelParameters.Skin;

                                using View<double, Serial> resultsView = tpm.Calculate(timeView, argsView);

                                results.MatrixPermeability            = project.MultiPorosityModelParameters.MatrixPermeability;
                                results.HydraulicFracturePermeability = project.MultiPorosityModelParameters.HydraulicFracturePermeability;
                                results.NaturalFracturePermeability   = project.MultiPorosityModelParameters.NaturalFracturePermeability;
                                results.HydraulicFractureHalfLength   = project.MultiPorosityModelParameters.HydraulicFractureHalfLength;
                                results.HydraulicFractureSpacing      = project.MultiPorosityModelParameters.HydraulicFractureSpacing;
                                results.NaturalFractureSpacing        = project.MultiPorosityModelParameters.NaturalFractureSpacing;
                                results.Skin                          = project.MultiPorosityModelParameters.Skin;

                                List<Models.MultiPorosityModelProduction> production = new((int)project.MultiPorosityModelParameters.Days);

                                for(ulong i0 = 0; i0 < resultsView.Extent(0); ++i0)
                                {
                                    production.Add(new Models.MultiPorosityModelProduction(timeView[i0], resultsView[i0, 0], resultsView[i0, 1], resultsView[i0, 2]));
                                }

                                results.Production = new(production);
                            }

                            break;
                        }
                    }
                }

                isRunning = false;
            }

            return results;
        }

        public static MultiPorosityModelResults HistoryMatch(Project            project,
                                                             ExecutionSpaceKind ExecutionSpace)
        {
            MultiPorosityModelResults results = new();

            if(!isRunning)
            {
                isRunning = true;

                InitArguments arguments = new(OpenMP.NumberOfThreads, -1, Cuda.Id, OpenMP.DisableWarnings)
                {
                    ndevices = Cuda.NumberOfDevices, skip_device = Cuda.SkipDevice
                };

                using(ScopeGuard.Get(arguments))
                {
                    using ReservoirProperties<double> reservoir = new(ExecutionSpace);
                    reservoir.Length                = project.MultiPorosityProperties.ReservoirProperties.Length;
                    reservoir.Width                 = project.MultiPorosityProperties.ReservoirProperties.Width;
                    reservoir.Thickness             = project.MultiPorosityProperties.ReservoirProperties.Thickness;
                    reservoir.Porosity              = project.MultiPorosityProperties.ReservoirProperties.Porosity;
                    reservoir.Permeability          = project.MultiPorosityProperties.ReservoirProperties.Permeability;
                    reservoir.Compressibility       = project.MultiPorosityProperties.ReservoirProperties.Compressibility;
                    reservoir.BottomholeTemperature = project.MultiPorosityProperties.ReservoirProperties.BottomholeTemperature;
                    reservoir.InitialPressure       = project.MultiPorosityProperties.ReservoirProperties.InitialPressure;

                    using WellProperties<double> wellProperties = new(ExecutionSpace);
                    wellProperties.LateralLength      = project.MultiPorosityProperties.WellProperties.LateralLength;
                    wellProperties.BottomholePressure = project.MultiPorosityProperties.WellProperties.BottomholePressure;

                    using FractureProperties<double> fracture = new(ExecutionSpace);
                    fracture.Count        = project.MultiPorosityProperties.FractureProperties.Count;
                    fracture.Width        = project.MultiPorosityProperties.FractureProperties.Width;
                    fracture.Height       = project.MultiPorosityProperties.FractureProperties.Height;
                    fracture.HalfLength   = project.MultiPorosityProperties.FractureProperties.HalfLength;
                    fracture.Porosity     = project.MultiPorosityProperties.FractureProperties.Porosity;
                    fracture.Permeability = project.MultiPorosityProperties.FractureProperties.Permeability;
                    fracture.Skin         = project.MultiPorosityProperties.FractureProperties.Skin;

                    using NaturalFractureProperties<double> natural_fracture = new(ExecutionSpace);
                    natural_fracture.Count        = project.MultiPorosityProperties.NaturalFractureProperties.Count;
                    natural_fracture.Width        = project.MultiPorosityProperties.NaturalFractureProperties.Width;
                    natural_fracture.Porosity     = project.MultiPorosityProperties.NaturalFractureProperties.Porosity;
                    natural_fracture.Permeability = project.MultiPorosityProperties.NaturalFractureProperties.Permeability;

                    using Pvt<double> pvt = new();
                    pvt.OilSaturation              = project.MultiPorosityProperties.Pvt.OilSaturation;
                    pvt.OilApiGravity              = project.MultiPorosityProperties.Pvt.OilApiGravity;
                    pvt.OilViscosity               = project.MultiPorosityProperties.Pvt.OilViscosity;
                    pvt.OilFormationVolumeFactor   = project.MultiPorosityProperties.Pvt.OilFormationVolumeFactor;
                    pvt.OilCompressibility         = project.MultiPorosityProperties.Pvt.OilCompressibility;
                    pvt.WaterSaturation            = project.MultiPorosityProperties.Pvt.WaterSaturation;
                    pvt.WaterSpecificGravity       = project.MultiPorosityProperties.Pvt.WaterSpecificGravity;
                    pvt.WaterViscosity             = project.MultiPorosityProperties.Pvt.WaterViscosity;
                    pvt.WaterFormationVolumeFactor = project.MultiPorosityProperties.Pvt.WaterFormationVolumeFactor;
                    pvt.WaterCompressibility       = project.MultiPorosityProperties.Pvt.WaterCompressibility;
                    pvt.GasSaturation              = project.MultiPorosityProperties.Pvt.GasSaturation;
                    pvt.GasSpecificGravity         = project.MultiPorosityProperties.Pvt.GasSpecificGravity;
                    pvt.GasViscosity               = project.MultiPorosityProperties.Pvt.GasViscosity;
                    pvt.GasFormationVolumeFactor   = project.MultiPorosityProperties.Pvt.GasFormationVolumeFactor;
                    pvt.GasCompressibilityFactor   = project.MultiPorosityProperties.Pvt.GasCompressibilityFactor;
                    pvt.GasCompressibility         = project.MultiPorosityProperties.Pvt.GasCompressibility;

                    using RelativePermeabilities<double> relativePermeabilities = new();
                    relativePermeabilities.MatrixOil            = project.MultiPorosityProperties.RelativePermeabilities.MatrixOil;
                    relativePermeabilities.MatrixWater          = project.MultiPorosityProperties.RelativePermeabilities.MatrixWater;
                    relativePermeabilities.MatrixGas            = project.MultiPorosityProperties.RelativePermeabilities.MatrixGas;
                    relativePermeabilities.FractureOil          = project.MultiPorosityProperties.RelativePermeabilities.FractureOil;
                    relativePermeabilities.FractureWater        = project.MultiPorosityProperties.RelativePermeabilities.FractureWater;
                    relativePermeabilities.FractureGas          = project.MultiPorosityProperties.RelativePermeabilities.FractureGas;
                    relativePermeabilities.NaturalFractureOil   = project.MultiPorosityProperties.RelativePermeabilities.NaturalFractureOil;
                    relativePermeabilities.NaturalFractureWater = project.MultiPorosityProperties.RelativePermeabilities.NaturalFractureWater;
                    relativePermeabilities.NaturalFractureGas   = project.MultiPorosityProperties.RelativePermeabilities.NaturalFractureGas;

                    MultiPorosityData<double> mpd = new(ExecutionSpace);
                    mpd.ReservoirProperties       = reservoir;
                    mpd.WellProperties            = wellProperties;
                    mpd.FractureProperties        = fracture;
                    mpd.NaturalFractureProperties = natural_fracture;
                    mpd.Pvt                       = pvt;
                    mpd.RelativePermeability      = relativePermeabilities;

                    ParticleSwarmOptimizationOptions particleSwarmOptimizationOptions = new();
                    particleSwarmOptimizationOptions.SwarmSize        = project.ParticleSwarmOptimizationOptions.SwarmSize;
                    particleSwarmOptimizationOptions.ParticlesInSwarm = project.ParticleSwarmOptimizationOptions.ParticlesInSwarm;
                    particleSwarmOptimizationOptions.IterationMax     = project.ParticleSwarmOptimizationOptions.IterationMax;
                    particleSwarmOptimizationOptions.ErrorThreshold   = project.ParticleSwarmOptimizationOptions.ErrorThreshold;
                    particleSwarmOptimizationOptions.MinInertWeight   = project.ParticleSwarmOptimizationOptions.MinInertWeight;
                    particleSwarmOptimizationOptions.MaxInertWeight   = project.ParticleSwarmOptimizationOptions.MaxInertWeight;
                    particleSwarmOptimizationOptions.CacheResults     = project.ParticleSwarmOptimizationOptions.CacheResults;

                    switch(ExecutionSpace)
                    {
                        case ExecutionSpaceKind.OpenMP:
                        {
                            using(TriplePorosityModel<double, OpenMP> tpm = new(mpd))
                            {
                                ulong days = (ulong)project.ProductionHistory.Count;

                                using View<double, OpenMP> dataView    = new("dataView", days, 3);
                                using View<double, OpenMP> timeView    = new("timeView", days);
                                using View<double, OpenMP> weightsView = new("weightsView", days);

                                for(int i0 = 0; i0 < (int)days; ++i0)
                                {
                                    dataView[i0, 0] = project.ProductionHistory[i0].Gas;
                                    dataView[i0, 1] = project.ProductionHistory[i0].Oil;
                                    dataView[i0, 2] = project.ProductionHistory[i0].Water;
                                    timeView[i0]    = project.ProductionHistory[i0].Days;
                                    weightsView[i0] = project.ProductionHistory[i0].Weight;
                                }

                                BoundConstraints<double>[] arg_limits;

                                if(!project.MultiPorosityHistoryMatchParameters.Skin.IsConstant())
                                {
                                    arg_limits = new BoundConstraints<double>[7];

                                    /*km*/
                                    arg_limits[0] = project.MultiPorosityHistoryMatchParameters.MatrixPermeability;
                                    /*kF*/
                                    arg_limits[1] = project.MultiPorosityHistoryMatchParameters.HydraulicFracturePermeability;
                                    /*kf*/
                                    arg_limits[2] = project.MultiPorosityHistoryMatchParameters.NaturalFracturePermeability;
                                    /*ye*/
                                    arg_limits[3] = project.MultiPorosityHistoryMatchParameters.HydraulicFractureHalfLength;
                                    /*LF*/
                                    arg_limits[4] = project.MultiPorosityHistoryMatchParameters.HydraulicFractureSpacing;
                                    /*Lf*/
                                    arg_limits[5] = project.MultiPorosityHistoryMatchParameters.NaturalFractureSpacing;
                                    /*sk*/
                                    arg_limits[6] = project.MultiPorosityHistoryMatchParameters.Skin;
                                }
                                else
                                {
                                    arg_limits = new BoundConstraints<double>[6];

                                    /*km*/
                                    arg_limits[0] = project.MultiPorosityHistoryMatchParameters.MatrixPermeability;
                                    /*kF*/
                                    arg_limits[1] = project.MultiPorosityHistoryMatchParameters.HydraulicFracturePermeability;
                                    /*kf*/
                                    arg_limits[2] = project.MultiPorosityHistoryMatchParameters.NaturalFracturePermeability;
                                    /*ye*/
                                    arg_limits[3] = project.MultiPorosityHistoryMatchParameters.HydraulicFractureHalfLength;
                                    /*LF*/
                                    arg_limits[4] = project.MultiPorosityHistoryMatchParameters.HydraulicFractureSpacing;
                                    /*Lf*/
                                    arg_limits[5] = project.MultiPorosityHistoryMatchParameters.NaturalFractureSpacing;
                                }

                                MultiPorosityResult<double, OpenMP> resultsView = tpm.HistoryMatch(particleSwarmOptimizationOptions, dataView, timeView, weightsView, arg_limits);

                                results.MatrixPermeability = resultsView.Args[0];
                                results.HydraulicFracturePermeability = resultsView.Args[1];
                                results.NaturalFracturePermeability = resultsView.Args[2];
                                results.HydraulicFractureHalfLength = resultsView.Args[3];
                                results.HydraulicFractureSpacing = resultsView.Args[4];
                                results.NaturalFractureSpacing = resultsView.Args[5];
                                results.Skin = project.MultiPorosityHistoryMatchParameters.Skin.IsConstant() ? project.MultiPorosityHistoryMatchParameters.Skin.Lower : resultsView.Args[6];

                                if(resultsView.CachedData.RowCount > 0)
                                {
                                    ResultCallback(resultsView.CachedData);
                                }

                                using View<double, OpenMP> productionView = tpm.Calculate(timeView, resultsView.Args);

                                List<Models.MultiPorosityModelProduction> production = new((int)project.MultiPorosityModelParameters.Days);

                                for(ulong i0 = 0; i0 < productionView.Extent(0); ++i0)
                                {
                                    production.Add(new Models.MultiPorosityModelProduction(timeView[i0], productionView[i0, 0], productionView[i0, 1], productionView[i0, 2]));
                                }

                                results.Production = new(production);
                            }

                            break;
                        }
                        case ExecutionSpaceKind.Cuda:
                        {
                            using(TriplePorosityModel<double, Cuda> tpm = new(mpd))
                            {
                                ulong days = (ulong)project.ProductionHistory.Count;

                                using View<double, Cuda> dataView    = new("dataView", days, 3);
                                using View<double, Cuda> timeView    = new("timeView", days);
                                using View<double, Cuda> weightsView = new("weightsView", days);

                                for(int i0 = 0; i0 < (int)days; ++i0)
                                {
                                    dataView[i0, 0] = project.ProductionHistory[i0].Gas;
                                    dataView[i0, 1] = project.ProductionHistory[i0].Oil;
                                    dataView[i0, 2] = project.ProductionHistory[i0].Water;
                                    timeView[i0]    = project.ProductionHistory[i0].Days;
                                    weightsView[i0] = project.ProductionHistory[i0].Weight;
                                }

                                BoundConstraints<double>[] arg_limits;

                                if(!project.MultiPorosityHistoryMatchParameters.Skin.IsConstant())
                                {
                                    arg_limits = new BoundConstraints<double>[7];

                                    /*km*/
                                    arg_limits[0] = project.MultiPorosityHistoryMatchParameters.MatrixPermeability;
                                    /*kF*/
                                    arg_limits[1] = project.MultiPorosityHistoryMatchParameters.HydraulicFracturePermeability;
                                    /*kf*/
                                    arg_limits[2] = project.MultiPorosityHistoryMatchParameters.NaturalFracturePermeability;
                                    /*ye*/
                                    arg_limits[3] = project.MultiPorosityHistoryMatchParameters.HydraulicFractureHalfLength;
                                    /*LF*/
                                    arg_limits[4] = project.MultiPorosityHistoryMatchParameters.HydraulicFractureSpacing;
                                    /*Lf*/
                                    arg_limits[5] = project.MultiPorosityHistoryMatchParameters.NaturalFractureSpacing;
                                    /*sk*/
                                    arg_limits[6] = project.MultiPorosityHistoryMatchParameters.Skin;
                                }
                                else
                                {
                                    arg_limits = new BoundConstraints<double>[6];

                                    /*km*/
                                    arg_limits[0] = project.MultiPorosityHistoryMatchParameters.MatrixPermeability;
                                    /*kF*/
                                    arg_limits[1] = project.MultiPorosityHistoryMatchParameters.HydraulicFracturePermeability;
                                    /*kf*/
                                    arg_limits[2] = project.MultiPorosityHistoryMatchParameters.NaturalFracturePermeability;
                                    /*ye*/
                                    arg_limits[3] = project.MultiPorosityHistoryMatchParameters.HydraulicFractureHalfLength;
                                    /*LF*/
                                    arg_limits[4] = project.MultiPorosityHistoryMatchParameters.HydraulicFractureSpacing;
                                    /*Lf*/
                                    arg_limits[5] = project.MultiPorosityHistoryMatchParameters.NaturalFractureSpacing;
                                }

                                MultiPorosityResult<double, Cuda> resultsView = tpm.HistoryMatch(particleSwarmOptimizationOptions, dataView, timeView, weightsView, arg_limits);

                                results.MatrixPermeability = resultsView.Args[0];
                                results.HydraulicFracturePermeability = resultsView.Args[1];
                                results.NaturalFracturePermeability = resultsView.Args[2];
                                results.HydraulicFractureHalfLength = resultsView.Args[3];
                                results.HydraulicFractureSpacing = resultsView.Args[4];
                                results.NaturalFractureSpacing = resultsView.Args[5];
                                results.Skin = project.MultiPorosityHistoryMatchParameters.Skin.IsConstant() ? project.MultiPorosityHistoryMatchParameters.Skin.Lower : resultsView.Args[6];

                                if(resultsView.CachedData.RowCount > 0)
                                {
                                    ResultCallback(resultsView.CachedData);
                                }

                                using View<double, Cuda> productionView = tpm.Calculate(timeView, resultsView.Args);

                                List<Models.MultiPorosityModelProduction> production = new((int)project.MultiPorosityModelParameters.Days);

                                for(ulong i0 = 0; i0 < productionView.Extent(0); ++i0)
                                {
                                    production.Add(new Models.MultiPorosityModelProduction(timeView[i0], productionView[i0, 0], productionView[i0, 1], productionView[i0, 2]));
                                }

                                results.Production = new(production);
                            }

                            break;
                        }
                        case ExecutionSpaceKind.Serial:
                        default:
                        {
                            using(TriplePorosityModel<double, Serial> tpm = new(mpd))
                            {
                                ulong days = (ulong)project.ProductionHistory.Count;

                                using View<double, Serial> dataView    = new("dataView", days, 3);
                                using View<double, Serial> timeView    = new("timeView", days);
                                using View<double, Serial> weightsView = new("weightsView", days);

                                for(int i0 = 0; i0 < (int)days; ++i0)
                                {
                                    dataView[i0, 0] = project.ProductionHistory[i0].Gas;
                                    dataView[i0, 1] = project.ProductionHistory[i0].Oil;
                                    dataView[i0, 2] = project.ProductionHistory[i0].Water;
                                    timeView[i0]    = project.ProductionHistory[i0].Days;
                                    weightsView[i0] = project.ProductionHistory[i0].Weight;
                                }

                                BoundConstraints<double>[] arg_limits;

                                if(!project.MultiPorosityHistoryMatchParameters.Skin.IsConstant())
                                {
                                    arg_limits = new BoundConstraints<double>[7];

                                    /*km*/
                                    arg_limits[0] = project.MultiPorosityHistoryMatchParameters.MatrixPermeability;
                                    /*kF*/
                                    arg_limits[1] = project.MultiPorosityHistoryMatchParameters.HydraulicFracturePermeability;
                                    /*kf*/
                                    arg_limits[2] = project.MultiPorosityHistoryMatchParameters.NaturalFracturePermeability;
                                    /*ye*/
                                    arg_limits[3] = project.MultiPorosityHistoryMatchParameters.HydraulicFractureHalfLength;
                                    /*LF*/
                                    arg_limits[4] = project.MultiPorosityHistoryMatchParameters.HydraulicFractureSpacing;
                                    /*Lf*/
                                    arg_limits[5] = project.MultiPorosityHistoryMatchParameters.NaturalFractureSpacing;
                                    /*sk*/
                                    arg_limits[6] = project.MultiPorosityHistoryMatchParameters.Skin;
                                }
                                else
                                {
                                    arg_limits = new BoundConstraints<double>[6];

                                    /*km*/
                                    arg_limits[0] = project.MultiPorosityHistoryMatchParameters.MatrixPermeability;
                                    /*kF*/
                                    arg_limits[1] = project.MultiPorosityHistoryMatchParameters.HydraulicFracturePermeability;
                                    /*kf*/
                                    arg_limits[2] = project.MultiPorosityHistoryMatchParameters.NaturalFracturePermeability;
                                    /*ye*/
                                    arg_limits[3] = project.MultiPorosityHistoryMatchParameters.HydraulicFractureHalfLength;
                                    /*LF*/
                                    arg_limits[4] = project.MultiPorosityHistoryMatchParameters.HydraulicFractureSpacing;
                                    /*Lf*/
                                    arg_limits[5] = project.MultiPorosityHistoryMatchParameters.NaturalFractureSpacing;
                                }

                                MultiPorosityResult<double, Serial> resultsView = tpm.HistoryMatch(particleSwarmOptimizationOptions, dataView, timeView, weightsView, arg_limits);

                                results.MatrixPermeability = resultsView.Args[0];
                                results.HydraulicFracturePermeability = resultsView.Args[1];
                                results.NaturalFracturePermeability = resultsView.Args[2];
                                results.HydraulicFractureHalfLength = resultsView.Args[3];
                                results.HydraulicFractureSpacing = resultsView.Args[4];
                                results.NaturalFractureSpacing = resultsView.Args[5];
                                results.Skin = project.MultiPorosityHistoryMatchParameters.Skin.IsConstant() ? project.MultiPorosityHistoryMatchParameters.Skin.Lower : resultsView.Args[6];

                                if(resultsView.CachedData.RowCount > 0)
                                {
                                    ResultCallback(resultsView.CachedData);
                                }

                                using View<double, Serial> productionView = tpm.Calculate(timeView, resultsView.Args);

                                List<Models.MultiPorosityModelProduction> production = new((int)project.MultiPorosityModelParameters.Days);

                                for(ulong i0 = 0; i0 < productionView.Extent(0); ++i0)
                                {
                                    production.Add(new Models.MultiPorosityModelProduction(timeView[i0], productionView[i0, 0], productionView[i0, 1], productionView[i0, 2]));
                                }

                                results.Production = new(production);
                            }

                            break;
                        }
                    }
                }

                isRunning = false;
            }

            return results;
        }

        public static void ResultCallback(DataCache cache)
        {
            IEventAggregator eventAggregator = Prism.Ioc.ContainerLocator.Container.Resolve<IEventAggregator>();

            ulong nRows    = cache.RowCount;
            ulong nColumns = cache.ColumnCount;

            long   iteration;
            long   swarmIndex;
            long   particleIndex;
            double residual;
            double km;
            double kmVelocity;
            double kF;
            double kFVelocity;
            double kf;
            double kfVelocity;
            double ye;
            double yeVelocity;
            double lF;
            double lFVelocity;
            double lf;
            double lfVelocity;
            double sk;
            double skVelocity;

            Engineering.DataSource.Array<MultiPorosity.Services.Models.TriplePorosityOptimizationResults> results = new((int)nRows);

            ulong column;

            if(nColumns == 16)
            {
                for(ulong row = 0; row < nRows; ++row)
                {
                    column = 0;

                    iteration     = (long)cache[row, column++];
                    swarmIndex    = (long)cache[row, column++];
                    particleIndex = (long)cache[row, column++];
                    residual      = cache[row, column++];
                    km            = cache[row, column++];
                    kmVelocity    = cache[row, column++];
                    kF            = cache[row, column++];
                    kFVelocity    = cache[row, column++];
                    kf            = cache[row, column++];
                    kfVelocity    = cache[row, column++];
                    ye            = cache[row, column++];
                    yeVelocity    = cache[row, column++];
                    lF            = cache[row, column++];
                    lFVelocity    = cache[row, column++];
                    lf            = cache[row, column++];
                    lfVelocity    = cache[row, column];

                    results.Add(new MultiPorosity.Services.Models.TriplePorosityOptimizationResults(iteration,
                                                                                                    swarmIndex,
                                                                                                    particleIndex,
                                                                                                    residual,
                                                                                                    km,
                                                                                                    kmVelocity,
                                                                                                    kF,
                                                                                                    kFVelocity,
                                                                                                    kf,
                                                                                                    kfVelocity,
                                                                                                    ye,
                                                                                                    yeVelocity,
                                                                                                    lF,
                                                                                                    lFVelocity,
                                                                                                    lf,
                                                                                                    lfVelocity,
                                                                                                    0.0,
                                                                                                    0.0));
                }
            }
            else
            {
                for(ulong row = 0; row < nRows; ++row)
                {
                    column = 0;

                    iteration     = (long)cache[row, column++];
                    swarmIndex    = (long)cache[row, column++];
                    particleIndex = (long)cache[row, column++];
                    residual      = cache[row, column++];
                    km            = cache[row, column++];
                    kmVelocity    = cache[row, column++];
                    kF            = cache[row, column++];
                    kFVelocity    = cache[row, column++];
                    kf            = cache[row, column++];
                    kfVelocity    = cache[row, column++];
                    ye            = cache[row, column++];
                    yeVelocity    = cache[row, column++];
                    lF            = cache[row, column++];
                    lFVelocity    = cache[row, column++];
                    lf            = cache[row, column++];
                    lfVelocity    = cache[row, column++];
                    sk            = cache[row, column++];
                    skVelocity    = cache[row, column];

                    results.Add(new MultiPorosity.Services.Models.TriplePorosityOptimizationResults(iteration,
                                                                                                    swarmIndex,
                                                                                                    particleIndex,
                                                                                                    residual,
                                                                                                    km,
                                                                                                    kmVelocity,
                                                                                                    kF,
                                                                                                    kFVelocity,
                                                                                                    kf,
                                                                                                    kfVelocity,
                                                                                                    ye,
                                                                                                    yeVelocity,
                                                                                                    lF,
                                                                                                    lFVelocity,
                                                                                                    lf,
                                                                                                    lfVelocity,
                                                                                                    sk,
                                                                                                    skVelocity));
                }
            }

            eventAggregator?.GetEvent<TriplePorosityOptimizationResultsEvent>().Publish(results);
        }

        public static (double F, double f, double m) CalculateLambdaOil(MultiPorosityProperties parameters,
                                                                        in double               km,
                                                                        in double               kF,
                                                                        in double               kf,
                                                                        in double               ye,
                                                                        in double               LF,
                                                                        in double               Lf,
                                                                        in double               skin = 0.0)
        {

            double Acw = 2.0 * parameters.ReservoirProperties.Thickness * parameters.ReservoirProperties.Length;
            
            (double F, double f, double m) kr   = (parameters.RelativePermeabilities.FractureOil, parameters.RelativePermeabilities.NaturalFractureOil, parameters.RelativePermeabilities.MatrixOil);
            (double F, double f, double m) k    = (kF * kr.F, kf * kr.f, km * kr.m);
            (double F, double f, double m) phio = (parameters.FractureProperties.Porosity, parameters.NaturalFractureProperties.Porosity, parameters.ReservoirProperties.Porosity);
            (double F, double f, double m) phi  = (parameters.Pvt.OilSaturation * phio.F, parameters.Pvt.OilSaturation * phio.f, parameters.Pvt.OilSaturation * phio.m);

            (double F, double f, double m) W = (parameters.FractureProperties.Width, parameters.NaturalFractureProperties.Width, 1.0);
            (double F, double f, double m) L = (LF, Lf, 1.0);
            (double F, double f, double m) K = (k.F * (W.F / L.F), k.f * (W.f / L.f), k.m * (W.m / L.m));

            double Ac_Ff = (12.0 / Math.Pow(LF, 2.0)) * (K.f / K.F) * Acw;
            double Ac_fm = (12.0 / Math.Pow(Lf, 2.0)) * (K.m / K.F) * Acw;

            return (3.0, Ac_Ff, Ac_fm);
        }

        public static (double F, double f, double m) CalculateOmegaOil(MultiPorosityProperties parameters,
                                                                       in double               km,
                                                                       in double               kF,
                                                                       in double               kf,
                                                                       in double               ye,
                                                                       in double               LF,
                                                                       in double               Lf,
                                                                       in double               skin = 0.0)
        {

            double Acw = 2.0 * parameters.ReservoirProperties.Thickness * parameters.ReservoirProperties.Length;
            
            (double F, double f, double m) kr   = (parameters.RelativePermeabilities.FractureOil, parameters.RelativePermeabilities.NaturalFractureOil, parameters.RelativePermeabilities.MatrixOil);
            (double F, double f, double m) k    = (kF * kr.F, kf * kr.f, km * kr.m);
            (double F, double f, double m) phio = (parameters.FractureProperties.Porosity, parameters.NaturalFractureProperties.Porosity, parameters.ReservoirProperties.Porosity);
            (double F, double f, double m) phi  = (parameters.Pvt.OilSaturation * phio.F, parameters.Pvt.OilSaturation * phio.f, parameters.Pvt.OilSaturation * phio.m);

            (double F, double f, double m) W = (parameters.FractureProperties.Width, parameters.NaturalFractureProperties.Width, 1.0);
            (double F, double f, double m) L = (LF, Lf, 1.0);
            (double F, double f, double m) K = (k.F * (W.F / L.F), k.f * (W.f / L.f), k.m * (W.m / L.m));

            double                         Vt =  2 * parameters.ReservoirProperties.Length * ye;
            double                         VF = (Vt * (W.F / L.F));
            double                         Vf = (Vt * (W.f / L.f));
            (double F, double f, double m) V  = (VF, Vf, Vt - (VF + Vf));

            (double F, double f, double m) PhiV  = (phi.F * V.F, phi.f * V.f, phi.m * V.m);
            double                         phiVt = (PhiV.F + PhiV.f + PhiV.m);
            double                         phit  = (phiVt / Vt);

            return (PhiV.F / phiVt, PhiV.f / phiVt, PhiV.m / phiVt);
        }
    }
}
