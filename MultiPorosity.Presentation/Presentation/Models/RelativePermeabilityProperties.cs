using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Engineering.UI;
using Engineering.UI.Controls;

using Prism.Mvvm;

#pragma warning disable IDE0044
namespace MultiPorosity.Presentation.Models
{
    public sealed class RelativePermeabilityProperties : BindableBase
    {
        private RelativePermeabilityPropertyModel _matrix;

        private RelativePermeabilityPropertyModel _hydraulicFracture;

        private RelativePermeabilityPropertyModel _naturalFracture;

        public RelativePermeabilityPropertyModel Matrix
        {
            get { return _matrix; }
            set
            {
                if(SetProperty(ref _matrix, value))
                {
                }
            }
        }

        public RelativePermeabilityPropertyModel HydraulicFracture
        {
            get { return _hydraulicFracture; }
            set
            {
                if(SetProperty(ref _hydraulicFracture, value))
                {
                }
            }
        }

        public RelativePermeabilityPropertyModel NaturalFracture
        {
            get { return _naturalFracture; }
            set
            {
                if(SetProperty(ref _naturalFracture, value))
                {
                }
            }
        }

        public RelativePermeabilityProperties(MultiPorosity.Services.Models.RelativePermeabilityProperties relativePermeabilityProperties)
        {
            _matrix            = new RelativePermeabilityPropertyModel(relativePermeabilityProperties.Matrix);
            _hydraulicFracture = new RelativePermeabilityPropertyModel(relativePermeabilityProperties.HydraulicFracture);
            _naturalFracture   = new RelativePermeabilityPropertyModel(relativePermeabilityProperties.NaturalFracture);
        }

        public static implicit operator MultiPorosity.Services.Models.RelativePermeabilityProperties(RelativePermeabilityProperties relativePermeabilityProperties)
        {
            return new(relativePermeabilityProperties.Matrix, relativePermeabilityProperties.HydraulicFracture, relativePermeabilityProperties.NaturalFracture);
        }
    }

    public sealed class RelativePermeabilityPropertyModel : BindableBase
    {
        private double _saturationWaterConnate;
        private double _saturationWaterCritical;
        private double _saturationOilIrreducibleWater;
        private double _saturationOilResidualWater;
        private double _saturationOilIrreducibleGas;
        private double _saturationOilResidualGas;
        private double _saturationGasConnate;
        private double _saturationGasCritical;
        private double _permeabilityRelativeWaterOilIrreducible;
        private double _permeabilityRelativeOilWaterConnate;
        private double _permeabilityRelativeGasLiquidConnate;
        private double _exponentPermeabilityRelativeWater;
        private double _exponentPermeabilityRelativeOilWater;
        private double _exponentPermeabilityRelativeGas;
        private double _exponentPermeabilityRelativeOilGas;

        public ref double SaturationWaterConnate
        {
            get { return ref _saturationWaterConnate; }
        }

        public ref double SaturationWaterCritical
        {
            get { return ref _saturationWaterCritical; }
        }

        public ref double SaturationOilIrreducibleWater
        {
            get { return ref _saturationOilIrreducibleWater; }
        }

        public ref double SaturationOilResidualWater
        {
            get { return ref _saturationOilResidualWater; }
        }

        public ref double SaturationOilIrreducibleGas
        {
            get { return ref _saturationOilIrreducibleGas; }
        }

        public ref double SaturationOilResidualGas
        {
            get { return ref _saturationOilResidualGas; }
        }

        public ref double SaturationGasConnate
        {
            get { return ref _saturationGasConnate; }
        }

        public ref double SaturationGasCritical
        {
            get { return ref _saturationGasCritical; }
        }

        public ref double PermeabilityRelativeWaterOilIrreducible
        {
            get { return ref _permeabilityRelativeWaterOilIrreducible; }
        }

        public ref double PermeabilityRelativeOilWaterConnate
        {
            get { return ref _permeabilityRelativeOilWaterConnate; }
        }

        public ref double PermeabilityRelativeGasLiquidConnate
        {
            get { return ref _permeabilityRelativeGasLiquidConnate; }
        }

        public ref double ExponentPermeabilityRelativeWater
        {
            get { return ref _exponentPermeabilityRelativeWater; }
        }

        public ref double ExponentPermeabilityRelativeOilWater
        {
            get { return ref _exponentPermeabilityRelativeOilWater; }
        }

        public ref double ExponentPermeabilityRelativeGas
        {
            get { return ref _exponentPermeabilityRelativeGas; }
        }

        public ref double ExponentPermeabilityRelativeOilGas
        {
            get { return ref _exponentPermeabilityRelativeOilGas; }
        }

        public RelativePermeabilityPropertyModel(double saturationWaterConnate,
                                                 double saturationWaterCritical,
                                                 double saturationOilIrreducibleWater,
                                                 double saturationOilResidualWater,
                                                 double saturationOilIrreducibleGas,
                                                 double saturationOilResidualGas,
                                                 double saturationGasConnate,
                                                 double saturationGasCritical,
                                                 double permeabilityRelativeWaterOilIrreducible,
                                                 double permeabilityRelativeOilWaterConnate,
                                                 double permeabilityRelativeGasLiquidConnate,
                                                 double exponentPermeabilityRelativeWater,
                                                 double exponentPermeabilityRelativeOilWater,
                                                 double exponentPermeabilityRelativeGas,
                                                 double exponentPermeabilityRelativeOilGas)
        {
            _saturationWaterConnate                  = saturationWaterConnate;
            _saturationWaterCritical                 = saturationWaterCritical;
            _saturationOilIrreducibleWater           = saturationOilIrreducibleWater;
            _saturationOilResidualWater              = saturationOilResidualWater;
            _saturationOilIrreducibleGas             = saturationOilIrreducibleGas;
            _saturationOilResidualGas                = saturationOilResidualGas;
            _saturationGasConnate                    = saturationGasConnate;
            _saturationGasCritical                   = saturationGasCritical;
            _permeabilityRelativeWaterOilIrreducible = permeabilityRelativeWaterOilIrreducible;
            _permeabilityRelativeOilWaterConnate     = permeabilityRelativeOilWaterConnate;
            _permeabilityRelativeGasLiquidConnate    = permeabilityRelativeGasLiquidConnate;
            _exponentPermeabilityRelativeWater       = exponentPermeabilityRelativeWater;
            _exponentPermeabilityRelativeOilWater    = exponentPermeabilityRelativeOilWater;
            _exponentPermeabilityRelativeGas         = exponentPermeabilityRelativeGas;
            _exponentPermeabilityRelativeOilGas      = exponentPermeabilityRelativeOilGas;
        }

        public RelativePermeabilityPropertyModel(MultiPorosity.Services.Models.RelativePermeabilityPropertyModel relativePermeabilityModel)
        {
            _saturationWaterConnate                  = relativePermeabilityModel.SaturationWaterConnate;
            _saturationWaterCritical                 = relativePermeabilityModel.SaturationWaterCritical;
            _saturationOilIrreducibleWater           = relativePermeabilityModel.SaturationOilIrreducibleWater;
            _saturationOilResidualWater              = relativePermeabilityModel.SaturationOilResidualWater;
            _saturationOilIrreducibleGas             = relativePermeabilityModel.SaturationOilIrreducibleGas;
            _saturationOilResidualGas                = relativePermeabilityModel.SaturationOilResidualGas;
            _saturationGasConnate                    = relativePermeabilityModel.SaturationGasConnate;
            _saturationGasCritical                   = relativePermeabilityModel.SaturationGasCritical;
            _permeabilityRelativeWaterOilIrreducible = relativePermeabilityModel.PermeabilityRelativeWaterOilIrreducible;
            _permeabilityRelativeOilWaterConnate     = relativePermeabilityModel.PermeabilityRelativeOilWaterConnate;
            _permeabilityRelativeGasLiquidConnate    = relativePermeabilityModel.PermeabilityRelativeGasLiquidConnate;
            _exponentPermeabilityRelativeWater       = relativePermeabilityModel.ExponentPermeabilityRelativeWater;
            _exponentPermeabilityRelativeOilWater    = relativePermeabilityModel.ExponentPermeabilityRelativeOilWater;
            _exponentPermeabilityRelativeGas         = relativePermeabilityModel.ExponentPermeabilityRelativeGas;
            _exponentPermeabilityRelativeOilGas      = relativePermeabilityModel.ExponentPermeabilityRelativeOilGas;
        }

        public static implicit operator MultiPorosity.Services.Models.RelativePermeabilityPropertyModel(RelativePermeabilityPropertyModel relativePermeabilityModel)
        {
            return new(relativePermeabilityModel._saturationWaterConnate,
                       relativePermeabilityModel._saturationWaterCritical,
                       relativePermeabilityModel._saturationOilIrreducibleWater,
                       relativePermeabilityModel._saturationOilResidualWater,
                       relativePermeabilityModel._saturationOilIrreducibleGas,
                       relativePermeabilityModel._saturationOilResidualGas,
                       relativePermeabilityModel._saturationGasConnate,
                       relativePermeabilityModel._saturationGasCritical,
                       relativePermeabilityModel._permeabilityRelativeWaterOilIrreducible,
                       relativePermeabilityModel._permeabilityRelativeOilWaterConnate,
                       relativePermeabilityModel._permeabilityRelativeGasLiquidConnate,
                       relativePermeabilityModel._exponentPermeabilityRelativeWater,
                       relativePermeabilityModel._exponentPermeabilityRelativeOilWater,
                       relativePermeabilityModel._exponentPermeabilityRelativeGas,
                       relativePermeabilityModel._exponentPermeabilityRelativeOilGas);
        }
    }
}
#pragma warning restore IDE0044