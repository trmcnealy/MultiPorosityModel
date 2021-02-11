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
    public class RelativePermeabilityMatrixParametersViewModel : BindableBase
    {
        public double SaturationWaterConnate
        {
            get { return relativePermeabilityProperties.Matrix.SaturationWaterConnate; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.Matrix.SaturationWaterConnate, value))
                {
                    UpdateModel();
                }
            }
        }

        public double SaturationWaterCritical
        {
            get { return relativePermeabilityProperties.Matrix.SaturationWaterCritical; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.Matrix.SaturationWaterCritical, value))
                {
                    UpdateModel();
                }
            }
        }

        public double SaturationOilIrreducibleWater
        {
            get { return relativePermeabilityProperties.Matrix.SaturationOilIrreducibleWater; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.Matrix.SaturationOilIrreducibleWater, value))
                {
                    UpdateModel();
                }
            }
        }

        public double SaturationOilResidualWater
        {
            get { return relativePermeabilityProperties.Matrix.SaturationOilResidualWater; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.Matrix.SaturationOilResidualWater, value))
                {
                    UpdateModel();
                }
            }
        }

        public double SaturationOilIrreducibleGas
        {
            get { return relativePermeabilityProperties.Matrix.SaturationOilIrreducibleGas; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.Matrix.SaturationOilIrreducibleGas, value))
                {
                    UpdateModel();
                }
            }
        }

        public double SaturationOilResidualGas
        {
            get { return relativePermeabilityProperties.Matrix.SaturationOilResidualGas; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.Matrix.SaturationOilResidualGas, value))
                {
                    UpdateModel();
                }
            }
        }

        public double SaturationGasConnate
        {
            get { return relativePermeabilityProperties.Matrix.SaturationGasConnate; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.Matrix.SaturationGasConnate, value))
                {
                    UpdateModel();
                }
            }
        }

        public double SaturationGasCritical
        {
            get { return relativePermeabilityProperties.Matrix.SaturationGasCritical; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.Matrix.SaturationGasCritical, value))
                {
                    UpdateModel();
                }
            }
        }

        public double PermeabilityRelativeWaterOilIrreducible
        {
            get { return relativePermeabilityProperties.Matrix.PermeabilityRelativeWaterOilIrreducible; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.Matrix.PermeabilityRelativeWaterOilIrreducible, value))
                {
                    UpdateModel();
                }
            }
        }

        public double PermeabilityRelativeOilWaterConnate
        {
            get { return relativePermeabilityProperties.Matrix.PermeabilityRelativeOilWaterConnate; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.Matrix.PermeabilityRelativeOilWaterConnate, value))
                {
                    UpdateModel();
                }
            }
        }

        public double PermeabilityRelativeGasLiquidConnate
        {
            get { return relativePermeabilityProperties.Matrix.PermeabilityRelativeGasLiquidConnate; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.Matrix.PermeabilityRelativeGasLiquidConnate, value))
                {
                    UpdateModel();
                }
            }
        }

        public double ExponentPermeabilityRelativeWater
        {
            get { return relativePermeabilityProperties.Matrix.ExponentPermeabilityRelativeWater; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.Matrix.ExponentPermeabilityRelativeWater, value))
                {
                    UpdateModel();
                }
            }
        }

        public double ExponentPermeabilityRelativeOilWater
        {
            get { return relativePermeabilityProperties.Matrix.ExponentPermeabilityRelativeOilWater; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.Matrix.ExponentPermeabilityRelativeOilWater, value))
                {
                    UpdateModel();
                }
            }
        }

        public double ExponentPermeabilityRelativeGas
        {
            get { return relativePermeabilityProperties.Matrix.ExponentPermeabilityRelativeGas; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.Matrix.ExponentPermeabilityRelativeGas, value))
                {
                    UpdateModel();
                }
            }
        }

        public double ExponentPermeabilityRelativeOilGas
        {
            get { return relativePermeabilityProperties.Matrix.ExponentPermeabilityRelativeOilGas; }
            set
            {
                if(SetProperty(ref relativePermeabilityProperties.Matrix.ExponentPermeabilityRelativeOilGas, value))
                {
                    UpdateModel();
                }
            }
        }

        private readonly RelativePermeabilityService    _relativePermeabilityService;
        private readonly MultiPorosityModelService      _multiPorosityModelService;
        private          RelativePermeabilityProperties relativePermeabilityProperties;

        public RelativePermeabilityMatrixParametersViewModel(MultiPorosityModelService multiPorosityModelService)
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

            _multiPorosityModelService.ActiveProject.RelativePermeabilityMatrixModels = new BindableCollection<RelativePermeabilityModel>(models);
        }
    }
}