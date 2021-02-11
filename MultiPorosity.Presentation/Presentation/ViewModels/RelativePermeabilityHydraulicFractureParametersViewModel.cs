using Prism.Mvvm;

using System.Collections.Generic;
using System.ComponentModel;

using Engineering.UI.Collections;

using Kokkos;

using MultiPorosity.Models;
using MultiPorosity.Presentation.Models;
using MultiPorosity.Presentation.Services;

namespace MultiPorosity.Presentation
{
    public class RelativePermeabilityHydraulicFractureParametersViewModel : BindableBase
    {
        public double SaturationWaterConnate
        {
            get { return relativePermeabilityProperties.HydraulicFracture.SaturationWaterConnate; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.HydraulicFracture.SaturationWaterConnate, value))
                {
                    UpdateModel();
                }
            }
        }

        public double SaturationWaterCritical
        {
            get { return relativePermeabilityProperties.HydraulicFracture.SaturationWaterCritical; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.HydraulicFracture.SaturationWaterCritical, value))
                {
                    UpdateModel();
                }
            }
        }

        public double SaturationOilIrreducibleWater
        {
            get { return relativePermeabilityProperties.HydraulicFracture.SaturationOilIrreducibleWater; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.HydraulicFracture.SaturationOilIrreducibleWater, value))
                {
                    UpdateModel();
                }
            }
        }

        public double SaturationOilResidualWater
        {
            get { return relativePermeabilityProperties.HydraulicFracture.SaturationOilResidualWater; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.HydraulicFracture.SaturationOilResidualWater, value))
                {
                    UpdateModel();
                }
            }
        }

        public double SaturationOilIrreducibleGas
        {
            get { return relativePermeabilityProperties.HydraulicFracture.SaturationOilIrreducibleGas; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.HydraulicFracture.SaturationOilIrreducibleGas, value))
                {
                    UpdateModel();
                }
            }
        }

        public double SaturationOilResidualGas
        {
            get { return relativePermeabilityProperties.HydraulicFracture.SaturationOilResidualGas; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.HydraulicFracture.SaturationOilResidualGas, value))
                {
                    UpdateModel();
                }
            }
        }

        public double SaturationGasConnate
        {
            get { return relativePermeabilityProperties.HydraulicFracture.SaturationGasConnate; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.HydraulicFracture.SaturationGasConnate, value))
                {
                    UpdateModel();
                }
            }
        }

        public double SaturationGasCritical
        {
            get { return relativePermeabilityProperties.HydraulicFracture.SaturationGasCritical; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.HydraulicFracture.SaturationGasCritical, value))
                {
                    UpdateModel();
                }
            }
        }

        public double PermeabilityRelativeWaterOilIrreducible
        {
            get { return relativePermeabilityProperties.HydraulicFracture.PermeabilityRelativeWaterOilIrreducible; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.HydraulicFracture.PermeabilityRelativeWaterOilIrreducible, value))
                {
                    UpdateModel();
                }
            }
        }

        public double PermeabilityRelativeOilWaterConnate
        {
            get { return relativePermeabilityProperties.HydraulicFracture.PermeabilityRelativeOilWaterConnate; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.HydraulicFracture.PermeabilityRelativeOilWaterConnate, value))
                {
                    UpdateModel();
                }
            }
        }

        public double PermeabilityRelativeGasLiquidConnate
        {
            get { return relativePermeabilityProperties.HydraulicFracture.PermeabilityRelativeGasLiquidConnate; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.HydraulicFracture.PermeabilityRelativeGasLiquidConnate, value))
                {
                    UpdateModel();
                }
            }
        }

        public double ExponentPermeabilityRelativeWater
        {
            get { return relativePermeabilityProperties.HydraulicFracture.ExponentPermeabilityRelativeWater; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.HydraulicFracture.ExponentPermeabilityRelativeWater, value))
                {
                    UpdateModel();
                }
            }
        }

        public double ExponentPermeabilityRelativeOilWater
        {
            get { return relativePermeabilityProperties.HydraulicFracture.ExponentPermeabilityRelativeOilWater; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.HydraulicFracture.ExponentPermeabilityRelativeOilWater, value))
                {
                    UpdateModel();
                }
            }
        }

        public double ExponentPermeabilityRelativeGas
        {
            get { return relativePermeabilityProperties.HydraulicFracture.ExponentPermeabilityRelativeGas; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.HydraulicFracture.ExponentPermeabilityRelativeGas, value))
                {
                    UpdateModel();
                }
            }
        }

        public double ExponentPermeabilityRelativeOilGas
        {
            get { return relativePermeabilityProperties.HydraulicFracture.ExponentPermeabilityRelativeOilGas; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.HydraulicFracture.ExponentPermeabilityRelativeOilGas, value))
                {
                    UpdateModel();
                }
            }
        }

        private readonly RelativePermeabilityService    _relativePermeabilityService;
        private readonly MultiPorosityModelService      _multiPorosityModelService;
        private          RelativePermeabilityProperties relativePermeabilityProperties;

        public RelativePermeabilityHydraulicFractureParametersViewModel(MultiPorosityModelService multiPorosityModelService)
        {
            _multiPorosityModelService     = multiPorosityModelService;
            relativePermeabilityProperties = multiPorosityModelService.ActiveProject.RelativePermeabilityProperties;
            _relativePermeabilityService   = new();
            
            _multiPorosityModelService.PropertyChanged -= OnPropertyChanged;
            _multiPorosityModelService.PropertyChanged += OnPropertyChanged;

            OnPropertyChanged(this, new PropertyChangedEventArgs("ActiveProject"));
        }
        
        private void OnPropertyChanged(object?                  sender,
                                       PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case "ActiveProject":
                {
                    _multiPorosityModelService.ActiveProject.PropertyChanged                                               -= OnPropertyChanged;
                    _multiPorosityModelService.ActiveProject.PropertyChanged                                               += OnPropertyChanged;
                    
                    UpdateModel();

                    break;
                }
                case "RelativePermeabilityProperties":
                {
                    UpdateModel();

                    break;
                }
            }
        }
        
        private void UpdateModel()
        {
            List<RelativePermeabilityModel> models;

            switch(_multiPorosityModelService.ExecutionSpace)
            {
                case ExecutionSpaceKind.OpenMP:
                {
                    models = _relativePermeabilityService.ExecuteOpenMP(SaturationWaterConnate,
                                                                        SaturationWaterCritical,
                                                                        SaturationOilIrreducibleWater,
                                                                        SaturationOilResidualWater,
                                                                        SaturationOilIrreducibleGas,
                                                                        SaturationOilResidualGas,
                                                                        SaturationGasConnate,
                                                                        SaturationGasCritical,
                                                                        PermeabilityRelativeWaterOilIrreducible,
                                                                        PermeabilityRelativeOilWaterConnate,
                                                                        PermeabilityRelativeGasLiquidConnate,
                                                                        ExponentPermeabilityRelativeWater,
                                                                        ExponentPermeabilityRelativeOilWater,
                                                                        ExponentPermeabilityRelativeGas,
                                                                        ExponentPermeabilityRelativeOilGas);

                    break;
                }
                case ExecutionSpaceKind.Cuda:
                {
                    models = _relativePermeabilityService.ExecuteCuda(SaturationWaterConnate,
                                                                      SaturationWaterCritical,
                                                                      SaturationOilIrreducibleWater,
                                                                      SaturationOilResidualWater,
                                                                      SaturationOilIrreducibleGas,
                                                                      SaturationOilResidualGas,
                                                                      SaturationGasConnate,
                                                                      SaturationGasCritical,
                                                                      PermeabilityRelativeWaterOilIrreducible,
                                                                      PermeabilityRelativeOilWaterConnate,
                                                                      PermeabilityRelativeGasLiquidConnate,
                                                                      ExponentPermeabilityRelativeWater,
                                                                      ExponentPermeabilityRelativeOilWater,
                                                                      ExponentPermeabilityRelativeGas,
                                                                      ExponentPermeabilityRelativeOilGas);

                    break;
                }
                case ExecutionSpaceKind.Serial:
                default:
                {
                    models = _relativePermeabilityService.Execute(SaturationWaterConnate,
                                                                  SaturationWaterCritical,
                                                                  SaturationOilIrreducibleWater,
                                                                  SaturationOilResidualWater,
                                                                  SaturationOilIrreducibleGas,
                                                                  SaturationOilResidualGas,
                                                                  SaturationGasConnate,
                                                                  SaturationGasCritical,
                                                                  PermeabilityRelativeWaterOilIrreducible,
                                                                  PermeabilityRelativeOilWaterConnate,
                                                                  PermeabilityRelativeGasLiquidConnate,
                                                                  ExponentPermeabilityRelativeWater,
                                                                  ExponentPermeabilityRelativeOilWater,
                                                                  ExponentPermeabilityRelativeGas,
                                                                  ExponentPermeabilityRelativeOilGas);

                    break;
                }
            }

            _multiPorosityModelService.ActiveProject.RelativePermeabilityHydraulicFractureModels = new BindableCollection<RelativePermeabilityModel>(models);
        }
    }
}