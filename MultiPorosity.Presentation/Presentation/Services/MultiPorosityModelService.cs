using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Engineering.UI.Collections;

using Kokkos;

using MultiPorosity.Models;
using MultiPorosity.Services;
using MultiPorosity.Services.Models;

using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;

using Project = MultiPorosity.Presentation.Models.Project;
using TriplePorosityOptimizationResults = MultiPorosity.Models.TriplePorosityOptimizationResults;

namespace MultiPorosity.Presentation.Services
{
    public sealed class MultiPorosityModelService : BindableBase
    {
        private static readonly Devices devices = new();

        public static Devices GetDevices()
        {
            return devices;
        }

        private static ExecutionSpaceKind _executionSpaceType = GetExecutionSpaceKind();

        private readonly IEventAggregator   _eventAggregator;
        private readonly IContainerProvider _container;

        private bool _isRunnning;

        //public MultiPorosityData MultiPorosityData
        //{
        //    get
        //    {
        //        MultiPorosityData mpd = new (_executionSpaceType);
        //        mpd.ReservoirProperties       = _reservoirProperties;
        //        mpd.WellProperties            = _wellProperties;
        //        mpd.FractureProperties        = _fractureProperties;
        //        mpd.NaturalFractureProperties = _naturalFractureProperties;
        //        mpd.Pvt                       = _pvt;
        //        mpd.RelativePermeability      = _relativePermeabilities;
        //        return mpd;
        //    }
        //}

        private Project? _activeProject;

        //private readonly SourceList<ProductionHistoryModel> productionHistory = new SourceList<ProductionHistoryModel>();

        //public IObservableList<ProductionHistoryModel> ProductionHistory { get; set; }

        //public IObservable<IChangeSet<ProductionHistoryModel>> SelectedItems { get; }

        private string? _repositoryPath;

        public Project? ActiveProject
        {
            get { return _activeProject; }
            set
            {
                if(SetProperty(ref _activeProject, value))
                {
                    _eventAggregator.GetEvent<ProjectChangedEvent>().Publish(_activeProject.Name);
                }
            }
        }

        public Dictionary<double, DateTime> DaysToDateMap { get; set; } = new Dictionary<double, DateTime>();

        public string? RepositoryPath
        {
            get { return _repositoryPath; }
            set
            {
                if(SetProperty(ref _repositoryPath, value) && !string.IsNullOrEmpty(_repositoryPath))
                {
                    Application.Current.Properties["RepositoryPath"] = _repositoryPath;

                    if(!Directory.Exists(_repositoryPath))
                    {
                        Directory.CreateDirectory(_repositoryPath);
                    }
                }
            }
        }

        public ExecutionSpaceKind ExecutionSpace
        {
            get { return _executionSpaceType; }
            set
            {
                if(SetProperty(ref _executionSpaceType, value))
                {
                    Application.Current.Properties["ExecutionSpace"] = _executionSpaceType.ToString();
                }
            }
        }

        private List<MultiPorosity.Services.Models.TriplePorosityOptimizationResults> _triplePorosityOptimizationResults;

        public List<MultiPorosity.Services.Models.TriplePorosityOptimizationResults> TriplePorosityOptimizationResults
        {
            get { return _triplePorosityOptimizationResults; }
            set { SetProperty(ref _triplePorosityOptimizationResults, value); }
        }

        public MultiPorosityModelService(IEventAggregator   eventAggregator,
                                         IContainerProvider container)
        {
            _eventAggregator = eventAggregator;
            _container       = container;

            TriplePorosityOptimizationResults = new List<MultiPorosity.Services.Models.TriplePorosityOptimizationResults>();

            _eventAggregator.GetEvent<TriplePorosityOptimizationResultsEvent>().Subscribe(OnUpdateResults);
        }

        public void SetRepositoryPath(string? repositoryPath = null)
        {
            string? path = repositoryPath;

            if(path is null)
            {
                if(Application.Current.Properties.Contains("RepositoryPath"))
                {
                    path = Application.Current.Properties["RepositoryPath"]?.ToString();
                }
                else
                {
                    path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "MultiPorosity.Tool");
                }
            }

            RepositoryPath = path;
        }

        private static ExecutionSpaceKind GetExecutionSpaceKind()
        {
            ExecutionSpaceKind ExecutionSpace = ExecutionSpaceKind.OpenMP;

            try
            {
                int gpuCount = devices.Gpus.Count;

                if(gpuCount > 0)
                {
                    Debug.WriteLine($"Gpu Count: {gpuCount}");

                    int version = devices.Gpus.First().Architecture.Version;

                    Debug.WriteLine($"Gpu Arch: {devices.Gpus.First().Architecture.Name} {version}");

                    if(version >= 520)
                    {
                        ExecutionSpace = ExecutionSpaceKind.Cuda;
                    }
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return ExecutionSpace;
        }

        public void SetExecutionSpace(ExecutionSpaceKind? executionSpaceKind = null)
        {
            if(executionSpaceKind is null)
            {
                if(Application.Current.Properties.Contains("ExecutionSpace"))
                {
                    string? executionSpace = Application.Current.Properties["ExecutionSpace"]?.ToString();

                    if(!Enum.TryParse(executionSpace, out _executionSpaceType))
                    {
                        ExecutionSpace = ExecutionSpaceKind.OpenMP;
                    }
                }
                else
                {
                    ExecutionSpace = ExecutionSpaceKind.OpenMP;
                }
            }
            else
            {
                ExecutionSpace = executionSpaceKind.Value;
            }
        }

        public void CalcPvt()
        {
            ActiveProject.MultiPorosityProperties.Pvt.GasViscosity = PVT.PvtModels.GasViscosity(ActiveProject.MultiPorosityProperties.PvtModelProperties.GasViscosityType,
                                                                                                ActiveProject.MultiPorosityProperties.PvtModelProperties.GasPseudoCriticalType,
                                                                                                ActiveProject.MultiPorosityProperties.ReservoirProperties.InitialPressure,
                                                                                                ActiveProject.MultiPorosityProperties.ReservoirProperties.BottomholeTemperature,
                                                                                                ActiveProject.MultiPorosityProperties.Pvt.GasSpecificGravity);

            ActiveProject.MultiPorosityProperties.Pvt.GasCompressibilityFactor =
                PVT.PvtModels.GasCompressibilityFactor(ActiveProject.MultiPorosityProperties.PvtModelProperties.GasCompressibilityFactorType,
                                                       ActiveProject.MultiPorosityProperties.PvtModelProperties.GasPseudoCriticalType,
                                                       ActiveProject.MultiPorosityProperties.ReservoirProperties.InitialPressure,
                                                       ActiveProject.MultiPorosityProperties.ReservoirProperties.BottomholeTemperature,
                                                       ActiveProject.MultiPorosityProperties.Pvt.GasSpecificGravity);

            ActiveProject.MultiPorosityProperties.Pvt.GasFormationVolumeFactor = PVT.PvtModels.GasFormationVolumeFactor(ActiveProject.MultiPorosityProperties.PvtModelProperties.OilSolutionGasType,
                                                                                                                        ActiveProject.MultiPorosityProperties.ReservoirProperties.InitialPressure,
                                                                                                                        ActiveProject.MultiPorosityProperties.ReservoirProperties.BottomholeTemperature,
                                                                                                                        ActiveProject.MultiPorosityProperties.Pvt.GasCompressibilityFactor,
                                                                                                                        ActiveProject.MultiPorosityProperties.Pvt.GasSpecificGravity,
                                                                                                                        ActiveProject.MultiPorosityProperties.Pvt.OilApiGravity,
                                                                                                                        ActiveProject.MultiPorosityProperties.PvtModelProperties.SeparatorPressure,
                                                                                                                        ActiveProject.MultiPorosityProperties.PvtModelProperties.SeparatorTemperature);

            ActiveProject.MultiPorosityProperties.Pvt.GasCompressibility = PVT.PvtModels.GasCompressibility(ActiveProject.MultiPorosityProperties.PvtModelProperties.GasCompressibilityType,
                                                                                                            ActiveProject.MultiPorosityProperties.PvtModelProperties.GasPseudoCriticalType,
                                                                                                            ActiveProject.MultiPorosityProperties.ReservoirProperties.InitialPressure,
                                                                                                            ActiveProject.MultiPorosityProperties.ReservoirProperties.BottomholeTemperature,
                                                                                                            ActiveProject.MultiPorosityProperties.Pvt.GasCompressibilityFactor,
                                                                                                            ActiveProject.MultiPorosityProperties.Pvt.GasSpecificGravity);

            double Rs = PVT.PvtModels.OilSolutionGas(ActiveProject.MultiPorosityProperties.PvtModelProperties.OilSolutionGasType,
                                                     ActiveProject.MultiPorosityProperties.ReservoirProperties.InitialPressure,
                                                     ActiveProject.MultiPorosityProperties.ReservoirProperties.BottomholeTemperature,
                                                     ActiveProject.MultiPorosityProperties.Pvt.OilApiGravity,
                                                     ActiveProject.MultiPorosityProperties.Pvt.GasSpecificGravity,
                                                     ActiveProject.MultiPorosityProperties.PvtModelProperties.SeparatorPressure,
                                                     ActiveProject.MultiPorosityProperties.PvtModelProperties.SeparatorTemperature);

            double Pb = PVT.PvtModels.OilBubblePoint(ActiveProject.MultiPorosityProperties.PvtModelProperties.OilBubblePointType,
                                                     Rs,
                                                     ActiveProject.MultiPorosityProperties.ReservoirProperties.BottomholeTemperature,
                                                     ActiveProject.MultiPorosityProperties.Pvt.OilApiGravity,
                                                     ActiveProject.MultiPorosityProperties.Pvt.GasSpecificGravity,
                                                     ActiveProject.MultiPorosityProperties.PvtModelProperties.SeparatorPressure,
                                                     ActiveProject.MultiPorosityProperties.PvtModelProperties.SeparatorTemperature);

            ActiveProject.MultiPorosityProperties.Pvt.OilViscosity = PVT.PvtModels.OilViscosity(ActiveProject.MultiPorosityProperties.PvtModelProperties.DeadOilViscosityType,
                                                                                                ActiveProject.MultiPorosityProperties.PvtModelProperties.SaturatedOilViscosityType,
                                                                                                ActiveProject.MultiPorosityProperties.PvtModelProperties.UnderSaturatedOilViscosityType,
                                                                                                ActiveProject.MultiPorosityProperties.ReservoirProperties.InitialPressure,
                                                                                                ActiveProject.MultiPorosityProperties.ReservoirProperties.BottomholeTemperature,
                                                                                                Rs,
                                                                                                Pb,
                                                                                                ActiveProject.MultiPorosityProperties.Pvt.OilApiGravity);

            ActiveProject.MultiPorosityProperties.Pvt.OilFormationVolumeFactor =
                PVT.PvtModels.OilFormationVolumeFactor(ActiveProject.MultiPorosityProperties.PvtModelProperties.OilFormationVolumeFactorType,
                                                       Rs,
                                                       ActiveProject.MultiPorosityProperties.ReservoirProperties.BottomholeTemperature,
                                                       ActiveProject.MultiPorosityProperties.Pvt.OilApiGravity,
                                                       ActiveProject.MultiPorosityProperties.Pvt.GasSpecificGravity,
                                                       ActiveProject.MultiPorosityProperties.PvtModelProperties.SeparatorPressure,
                                                       ActiveProject.MultiPorosityProperties.PvtModelProperties.SeparatorTemperature);

            ActiveProject.MultiPorosityProperties.Pvt.OilCompressibility = PVT.PvtModels.OilCompressibility(ActiveProject.MultiPorosityProperties.PvtModelProperties.OilCompressibilityType,
                                                                                                            Rs,
                                                                                                            ActiveProject.MultiPorosityProperties.ReservoirProperties.InitialPressure,
                                                                                                            ActiveProject.MultiPorosityProperties.ReservoirProperties.BottomholeTemperature,
                                                                                                            ActiveProject.MultiPorosityProperties.Pvt.OilApiGravity,
                                                                                                            ActiveProject.MultiPorosityProperties.Pvt.GasSpecificGravity,
                                                                                                            ActiveProject.MultiPorosityProperties.PvtModelProperties.SeparatorPressure,
                                                                                                            ActiveProject.MultiPorosityProperties.PvtModelProperties.SeparatorTemperature);

            if(Math.Abs(ActiveProject.MultiPorosityProperties.Pvt.WaterSpecificGravity - 1.0) < double.Epsilon)
            {
                ActiveProject.MultiPorosityProperties.PvtModelProperties.WaterSalinity = 0.0;
            }
            else if(ActiveProject.MultiPorosityProperties.Pvt.WaterSpecificGravity == 0.0 && ActiveProject.MultiPorosityProperties.PvtModelProperties.WaterSalinity > 0.0)
            {
                ActiveProject.MultiPorosityProperties.Pvt.WaterSpecificGravity = PVT.PvtModels.WaterSpecificGravity(ActiveProject.MultiPorosityProperties.ReservoirProperties.InitialPressure,
                                                                                                                    ActiveProject.MultiPorosityProperties.ReservoirProperties.BottomholeTemperature,
                                                                                                                    ActiveProject.MultiPorosityProperties.PvtModelProperties.WaterSalinity);
            }
            else //if(ActiveProject.MultiPorosityProperties.Pvt.WaterSpecificGravity > 0.0 && ActiveProject.MultiPorosityProperties.PvtModelProperties.WaterSalinity == 0.0)
            {
                ActiveProject.MultiPorosityProperties.PvtModelProperties.WaterSalinity = PVT.PvtModels.WaterSalinity(ActiveProject.MultiPorosityProperties.ReservoirProperties.InitialPressure,
                                                                                                                     ActiveProject.MultiPorosityProperties.ReservoirProperties.BottomholeTemperature,
                                                                                                                     ActiveProject.MultiPorosityProperties.Pvt.WaterSpecificGravity);
            }

            ActiveProject.MultiPorosityProperties.Pvt.WaterViscosity = PVT.PvtModels.WaterViscosity(ActiveProject.MultiPorosityProperties.PvtModelProperties.WaterViscosityType,
                                                                                                    ActiveProject.MultiPorosityProperties.ReservoirProperties.InitialPressure,
                                                                                                    ActiveProject.MultiPorosityProperties.ReservoirProperties.BottomholeTemperature,
                                                                                                    ActiveProject.MultiPorosityProperties.PvtModelProperties.WaterSalinity);

            ActiveProject.MultiPorosityProperties.Pvt.WaterFormationVolumeFactor =
                PVT.PvtModels.WaterFormationVolumeFactor(ActiveProject.MultiPorosityProperties.PvtModelProperties.WaterFormationVolumeFactorType,
                                                         ActiveProject.MultiPorosityProperties.ReservoirProperties.InitialPressure,
                                                         ActiveProject.MultiPorosityProperties.ReservoirProperties.BottomholeTemperature,
                                                         ActiveProject.MultiPorosityProperties.PvtModelProperties.WaterSalinity);

            ActiveProject.MultiPorosityProperties.Pvt.WaterCompressibility = PVT.PvtModels.WaterCompressibility(ActiveProject.MultiPorosityProperties.PvtModelProperties.WaterCompressibilityType,
                                                                                                                ActiveProject.MultiPorosityProperties.ReservoirProperties.InitialPressure,
                                                                                                                ActiveProject.MultiPorosityProperties.ReservoirProperties.BottomholeTemperature,
                                                                                                                ActiveProject.MultiPorosityProperties.PvtModelProperties.WaterSalinity);
        }

        public void CalcRelPerm()
        {
            double saturation_gas   = ActiveProject.MultiPorosityProperties.Pvt.GasSaturation;
            double saturation_oil   = ActiveProject.MultiPorosityProperties.Pvt.OilSaturation;
            double saturation_water = ActiveProject.MultiPorosityProperties.Pvt.WaterSaturation;

            (double Krmg, double Krmo, double Krmw) = CalcRelPerm(saturation_gas, saturation_oil, saturation_water, ActiveProject.RelativePermeabilityProperties.Matrix);

            (double KrFg, double KrFo, double KrFw) = CalcRelPerm(saturation_gas, saturation_oil, saturation_water, ActiveProject.RelativePermeabilityProperties.HydraulicFracture);

            (double Krfg, double Krfo, double Krfw) = CalcRelPerm(saturation_gas, saturation_oil, saturation_water, ActiveProject.RelativePermeabilityProperties.NaturalFracture);

            ActiveProject.MultiPorosityProperties.RelativePermeabilities.MatrixGas   = Krmg;
            ActiveProject.MultiPorosityProperties.RelativePermeabilities.MatrixOil   = Krmo;
            ActiveProject.MultiPorosityProperties.RelativePermeabilities.MatrixWater = Krmw;

            ActiveProject.MultiPorosityProperties.RelativePermeabilities.FractureGas   = KrFg;
            ActiveProject.MultiPorosityProperties.RelativePermeabilities.FractureOil   = KrFo;
            ActiveProject.MultiPorosityProperties.RelativePermeabilities.FractureWater = KrFw;

            ActiveProject.MultiPorosityProperties.RelativePermeabilities.NaturalFractureGas   = Krfg;
            ActiveProject.MultiPorosityProperties.RelativePermeabilities.NaturalFractureOil   = Krfo;
            ActiveProject.MultiPorosityProperties.RelativePermeabilities.NaturalFractureWater = Krfw;
        }

        public void CalcParticlesInSwarm()
        {
            if(ActiveProject.MultiPorosityHistoryMatchParameters.Skin.Lower == 0.0 && ActiveProject.MultiPorosityHistoryMatchParameters.Skin.Upper == 0.0)
            {
                ActiveProject.ParticleSwarmOptimizationOptions.ParticlesInSwarm = ParticleSwarmOptimizationService.NumberOfParticlesPerSwarm(6);
            }
            else
            {
                ActiveProject.ParticleSwarmOptimizationOptions.ParticlesInSwarm = ParticleSwarmOptimizationService.NumberOfParticlesPerSwarm(7);
            }
        }

        private (double Krg, double Kro, double Krw) CalcRelPerm(double                            saturation_gas,
                                                                 double                            saturation_oil,
                                                                 double                            saturation_water,
                                                                 RelativePermeabilityPropertyModel model)
        {
            double saturation_water_connate                    = model.SaturationWaterConnate;
            double saturation_water_critical                   = model.SaturationWaterCritical;
            double saturation_oil_irreducible_water            = model.SaturationOilIrreducibleWater;
            double saturation_oil_residual_water               = model.SaturationOilResidualWater;
            double saturation_oil_irreducible_gas              = model.SaturationOilIrreducibleGas;
            double saturation_oil_residual_gas                 = model.SaturationOilResidualGas;
            double saturation_gas_connate                      = model.SaturationGasConnate;
            double saturation_gas_critical                     = model.SaturationGasCritical;
            double permeability_relative_water_oil_irreducible = model.PermeabilityRelativeWaterOilIrreducible;
            double permeability_relative_oil_water_connate     = model.PermeabilityRelativeOilWaterConnate;
            double permeability_relative_gas_liquid_connate    = model.PermeabilityRelativeGasLiquidConnate;
            double exponent_permeability_relative_water        = model.ExponentPermeabilityRelativeWater;
            double exponent_permeability_relative_oil_water    = model.ExponentPermeabilityRelativeOilWater;
            double exponent_permeability_relative_gas          = model.ExponentPermeabilityRelativeGas;
            double exponent_permeability_relative_oil_gas      = model.ExponentPermeabilityRelativeOilGas;

            return RelativePermeability.StoneII(saturation_gas,
                                                saturation_oil,
                                                saturation_water,
                                                saturation_water_connate,
                                                saturation_water_critical,
                                                saturation_oil_irreducible_water,
                                                saturation_oil_residual_water,
                                                saturation_oil_irreducible_gas,
                                                saturation_oil_residual_gas,
                                                saturation_gas_connate,
                                                saturation_gas_critical,
                                                permeability_relative_water_oil_irreducible,
                                                permeability_relative_oil_water_connate,
                                                permeability_relative_gas_liquid_connate,
                                                exponent_permeability_relative_water,
                                                exponent_permeability_relative_oil_water,
                                                exponent_permeability_relative_gas,
                                                exponent_permeability_relative_oil_gas);
        }

        public void RunModel()
        {
            if(!_isRunnning)
            {
                _isRunnning = true;

                Task.Run(() =>
                         {
                             _eventAggregator.GetEvent<ProjectIsBusyEvent>().Publish(true);

                             MultiPorosityModelResults results = TriplePorosityModel.Calculate(_eventAggregator, ActiveProject, ExecutionSpace);

                             ActiveProject.UpdateModel(results);

                             _eventAggregator.GetEvent<ProjectIsBusyEvent>().Publish(false);
                         }).Await();

                _isRunnning = false;
            }
        }

        public void HistoryMatch()
        {
            if(!_isRunnning)
            {
                _isRunnning = true;

                TriplePorosityOptimizationResults.Clear();

                Task.Run(() =>
                         {
                             _eventAggregator.GetEvent<ProjectIsBusyEvent>().Publish(true);

                             MultiPorosityModelResults results = TriplePorosityModel.HistoryMatch(_eventAggregator, ActiveProject, ExecutionSpace);

                             if(TriplePorosityOptimizationResults.Count > 0)
                             {
                                 results.TriplePorosityOptimizationResults = TriplePorosityOptimizationResults;
                             }

                             ActiveProject.UpdateModel(results);

                             _eventAggregator.GetEvent<SelectMultiPorosityModelViewEvent>().Publish(3);

                             _eventAggregator.GetEvent<ProjectIsBusyEvent>().Publish(false);
                         }).Await();

                _isRunnning = false;
            }
        }

        private void OnUpdateResults(Engineering.DataSource.Array<MultiPorosity.Services.Models.TriplePorosityOptimizationResults> results)
        {
            if(results.Count == 0)
            {
                return;
            }

            TriplePorosityOptimizationResults.Clear();
            TriplePorosityOptimizationResults.AddRange(results);
        }
    }
}