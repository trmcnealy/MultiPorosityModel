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
    public class RelativePermeabilityNaturalFractureParametersViewModel : BindableBase
    {
        public double SaturationWaterConnate
        {
            get { return relativePermeabilityProperties.NaturalFracture.SaturationWaterConnate; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.NaturalFracture.SaturationWaterConnate, value))
                {
                    UpdateModel();
                }
            }
        }

        public double SaturationWaterCritical
        {
            get { return relativePermeabilityProperties.NaturalFracture.SaturationWaterCritical; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.NaturalFracture.SaturationWaterCritical, value))
                {
                    UpdateModel();
                }
            }
        }

        public double SaturationOilIrreducibleWater
        {
            get { return relativePermeabilityProperties.NaturalFracture.SaturationOilIrreducibleWater; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.NaturalFracture.SaturationOilIrreducibleWater, value))
                {
                    UpdateModel();
                }
            }
        }

        public double SaturationOilResidualWater
        {
            get { return relativePermeabilityProperties.NaturalFracture.SaturationOilResidualWater; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.NaturalFracture.SaturationOilResidualWater, value))
                {
                    UpdateModel();
                }
            }
        }

        public double SaturationOilIrreducibleGas
        {
            get { return relativePermeabilityProperties.NaturalFracture.SaturationOilIrreducibleGas; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.NaturalFracture.SaturationOilIrreducibleGas, value))
                {
                    UpdateModel();
                }
            }
        }

        public double SaturationOilResidualGas
        {
            get { return relativePermeabilityProperties.NaturalFracture.SaturationOilResidualGas; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.NaturalFracture.SaturationOilResidualGas, value))
                {
                    UpdateModel();
                }
            }
        }

        public double SaturationGasConnate
        {
            get { return relativePermeabilityProperties.NaturalFracture.SaturationGasConnate; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.NaturalFracture.SaturationGasConnate, value))
                {
                    UpdateModel();
                }
            }
        }

        public double SaturationGasCritical
        {
            get { return relativePermeabilityProperties.NaturalFracture.SaturationGasCritical; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.NaturalFracture.SaturationGasCritical, value))
                {
                    UpdateModel();
                }
            }
        }

        public double PermeabilityRelativeWaterOilIrreducible
        {
            get { return relativePermeabilityProperties.NaturalFracture.PermeabilityRelativeWaterOilIrreducible; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.NaturalFracture.PermeabilityRelativeWaterOilIrreducible, value))
                {
                    UpdateModel();
                }
            }
        }

        public double PermeabilityRelativeOilWaterConnate
        {
            get { return relativePermeabilityProperties.NaturalFracture.PermeabilityRelativeOilWaterConnate; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.NaturalFracture.PermeabilityRelativeOilWaterConnate, value))
                {
                    UpdateModel();
                }
            }
        }

        public double PermeabilityRelativeGasLiquidConnate
        {
            get { return relativePermeabilityProperties.NaturalFracture.PermeabilityRelativeGasLiquidConnate; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.NaturalFracture.PermeabilityRelativeGasLiquidConnate, value))
                {
                    UpdateModel();
                }
            }
        }

        public double ExponentPermeabilityRelativeWater
        {
            get { return relativePermeabilityProperties.NaturalFracture.ExponentPermeabilityRelativeWater; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.NaturalFracture.ExponentPermeabilityRelativeWater, value))
                {
                    UpdateModel();
                }
            }
        }

        public double ExponentPermeabilityRelativeOilWater
        {
            get { return relativePermeabilityProperties.NaturalFracture.ExponentPermeabilityRelativeOilWater; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.NaturalFracture.ExponentPermeabilityRelativeOilWater, value))
                {
                    UpdateModel();
                }
            }
        }

        public double ExponentPermeabilityRelativeGas
        {
            get { return relativePermeabilityProperties.NaturalFracture.ExponentPermeabilityRelativeGas; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.NaturalFracture.ExponentPermeabilityRelativeGas, value))
                {
                    UpdateModel();
                }
            }
        }

        public double ExponentPermeabilityRelativeOilGas
        {
            get { return relativePermeabilityProperties.NaturalFracture.ExponentPermeabilityRelativeOilGas; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.NaturalFracture.ExponentPermeabilityRelativeOilGas, value))
                {
                    UpdateModel();
                }
            }
        }

        private readonly RelativePermeabilityService    _relativePermeabilityService;
        private readonly MultiPorosityModelService      _multiPorosityModelService;
        private          RelativePermeabilityProperties relativePermeabilityProperties;

        public RelativePermeabilityNaturalFractureParametersViewModel(MultiPorosityModelService multiPorosityModelService)
        {
            _multiPorosityModelService     = multiPorosityModelService;
            relativePermeabilityProperties = multiPorosityModelService.ActiveProject.RelativePermeabilityProperties;

            _relativePermeabilityService = new();
            
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
                    _multiPorosityModelService.ActiveProject.PropertyChanged -= OnPropertyChanged;
                    _multiPorosityModelService.ActiveProject.PropertyChanged += OnPropertyChanged;
                    
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

            _multiPorosityModelService.ActiveProject.RelativePermeabilityNaturalFractureModels = new BindableCollection<RelativePermeabilityModel>(models);
        }
    }
}