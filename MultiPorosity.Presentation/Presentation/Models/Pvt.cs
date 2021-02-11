using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Engineering.UI;
using Engineering.UI.Controls;

using Prism.Mvvm;

namespace MultiPorosity.Presentation.Models
{
    public sealed class Pvt : BindableBase
    {
        private double _oilSaturation;
        private double _oilApiGravity;
        private double _oilViscosity;
        private double _oilFormationVolumeFactor;
        private double _oilCompressibility;
        private double _waterSaturation;
        private double _waterSpecificGravity;
        private double _waterViscosity;
        private double _waterFormationVolumeFactor;
        private double _waterCompressibility;
        private double _gasSaturation;
        private double _gasSpecificGravity;
        private double _gasViscosity;
        private double _gasFormationVolumeFactor;
        private double _gasCompressibilityFactor;
        private double _gasCompressibility;

        [PropertyOrder(2)]
        //[Category("Pvt")]
        [DisplayName("Gas Saturation")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double GasSaturation
        {
            get { return _gasSaturation; }
            set
            {
                if(SetProperty(ref _gasSaturation, value))
                {
                }
            }
        }

        [PropertyOrder(3)]
        //[Category("Pvt")]
        [DisplayName("Gas Specific Gravity")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double GasSpecificGravity
        {
            get { return _gasSpecificGravity; }
            set
            {
                if(SetProperty(ref _gasSpecificGravity, value))
                {
                }
            }
        }

        [PropertyOrder(4)]
        //[Category("Pvt")]
        [DisplayName("Gas Viscosity")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double GasViscosity
        {
            get { return _gasViscosity; }
            set
            {
                if(SetProperty(ref _gasViscosity, value))
                {
                }
            }
        }
        
        [PropertyOrder(6)]
        [Category("Pvt")]
        [DisplayName("Gas Formation Volume Factor")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double GasFormationVolumeFactor
        {
            get { return _gasFormationVolumeFactor; }
            set
            {
                if(SetProperty(ref _gasFormationVolumeFactor, value))
                {
                }
            }
        }

        [PropertyOrder(8)]
        //[Category("Pvt")]
        [DisplayName("Gas Compressibility Factor")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double GasCompressibilityFactor
        {
            get { return _gasCompressibilityFactor; }
            set
            {
                if(SetProperty(ref _gasCompressibilityFactor, value))
                {
                }
            }
        }

        [PropertyOrder(11)]
        //[Category("Pvt")]
        [DisplayName("Gas Compressibility")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double GasCompressibility
        {
            get { return _gasCompressibility; }
            set
            {
                if(SetProperty(ref _gasCompressibility, value))
                {
                }
            }
        }

        [PropertyOrder(15)]
        //[Category("Pvt")]
        [DisplayName("Oil Saturation")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double OilSaturation
        {
            get { return _oilSaturation; }
            set
            {
                if(SetProperty(ref _oilSaturation, value))
                {
                }
            }
        }

        [PropertyOrder(16)]
        //[Category("Pvt")]
        [DisplayName("Oil Api Gravity")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double OilApiGravity
        {
            get { return _oilApiGravity; }
            set
            {
                if(SetProperty(ref _oilApiGravity, value))
                {
                }
            }
        }

        [PropertyOrder(17)]
        //[Category("Pvt")]
        [DisplayName("Oil Viscosity")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double OilViscosity
        {
            get { return _oilViscosity; }
            set
            {
                if(SetProperty(ref _oilViscosity, value))
                {
                }
            }
        }

        [PropertyOrder(21)]
        //[Category("Pvt")]
        [DisplayName("Oil Formation Volume Factor")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double OilFormationVolumeFactor
        {
            get { return _oilFormationVolumeFactor; }
            set
            {
                if(SetProperty(ref _oilFormationVolumeFactor, value))
                {
                }
            }
        }

        [PropertyOrder(23)]
        //[Category("Pvt")]
        [DisplayName("Oil Compressibility")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double OilCompressibility
        {
            get { return _oilCompressibility; }
            set
            {
                if(SetProperty(ref _oilCompressibility, value))
                {
                }
            }
        }

        [PropertyOrder(25)]
        //[Category("Pvt")]
        [DisplayName("Water Saturation")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double WaterSaturation
        {
            get { return _waterSaturation; }
            set
            {
                if(SetProperty(ref _waterSaturation, value))
                {
                }
            }
        }

        [PropertyOrder(26)]
        //[Category("Pvt")]
        [DisplayName("Water Specific Gravity")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double WaterSpecificGravity
        {
            get { return _waterSpecificGravity; }
            set
            {
                if(SetProperty(ref _waterSpecificGravity, value))
                {
                }
            }
        }

        [PropertyOrder(28)]
        //[Category("Pvt")]
        [DisplayName("Water Viscosity")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double WaterViscosity
        {
            get { return _waterViscosity; }
            set
            {
                if(SetProperty(ref _waterViscosity, value))
                {
                }
            }
        }

        [PropertyOrder(30)]
        //[Category("Pvt")]
        [DisplayName("Water Formation Volume Factor")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double WaterFormationVolumeFactor
        {
            get { return _waterFormationVolumeFactor; }
            set
            {
                if(SetProperty(ref _waterFormationVolumeFactor, value))
                {
                }
            }
        }

        [PropertyOrder(32)]
        //[Category("Pvt")]
        [DisplayName("Water Compressibility")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double WaterCompressibility
        {
            get { return _waterCompressibility; }
            set
            {
                if(SetProperty(ref _waterCompressibility, value))
                {
                }
            }
        }

        public Pvt(double oilSaturation,
                   double oilApiGravity,
                   double oilViscosity,
                   double oilFormationVolumeFactor,
                   double oilCompressibility,
                   double waterSaturation,
                   double waterSpecificGravity,
                   double waterViscosity,
                   double waterFormationVolumeFactor,
                   double waterCompressibility,
                   double gasSaturation,
                   double gasSpecificGravity,
                   double gasViscosity,
                   double gasFormationVolumeFactor,
                   double gasCompressibilityFactor,
                   double gasCompressibility)
        {
            _oilSaturation              = oilSaturation;
            _oilApiGravity              = oilApiGravity;
            _oilViscosity               = oilViscosity;
            _oilFormationVolumeFactor   = oilFormationVolumeFactor;
            _oilCompressibility         = oilCompressibility;
            _waterSaturation            = waterSaturation;
            _waterSpecificGravity       = waterSpecificGravity;
            _waterViscosity             = waterViscosity;
            _waterFormationVolumeFactor = waterFormationVolumeFactor;
            _waterCompressibility       = waterCompressibility;
            _gasSaturation              = gasSaturation;
            _gasSpecificGravity         = gasSpecificGravity;
            _gasViscosity               = gasViscosity;
            _gasFormationVolumeFactor   = gasFormationVolumeFactor;
            _gasCompressibilityFactor   = gasCompressibilityFactor;
            _gasCompressibility         = gasCompressibility;
        }

        public Pvt(MultiPorosity.Services.Models.Pvt pvt)
        {
            _oilSaturation              = pvt.OilSaturation;
            _oilApiGravity              = pvt.OilApiGravity;
            _oilViscosity               = pvt.OilViscosity;
            _oilFormationVolumeFactor   = pvt.OilFormationVolumeFactor;
            _oilCompressibility         = pvt.OilCompressibility;
            _waterSaturation            = pvt.WaterSaturation;
            _waterSpecificGravity       = pvt.WaterSpecificGravity;
            _waterViscosity             = pvt.WaterViscosity;
            _waterFormationVolumeFactor = pvt.WaterFormationVolumeFactor;
            _waterCompressibility       = pvt.WaterCompressibility;
            _gasSaturation              = pvt.GasSaturation;
            _gasSpecificGravity         = pvt.GasSpecificGravity;
            _gasViscosity               = pvt.GasViscosity;
            _gasFormationVolumeFactor   = pvt.GasFormationVolumeFactor;
            _gasCompressibilityFactor   = pvt.GasCompressibilityFactor;
            _gasCompressibility         = pvt.GasCompressibility;
        }

        public static implicit operator MultiPorosity.Services.Models.Pvt(Pvt pvt)
        {
            return new(pvt._gasSaturation,
                       pvt._gasSpecificGravity,
                       pvt._gasViscosity,
                       pvt._gasFormationVolumeFactor,
                       pvt._gasCompressibilityFactor,
                       pvt._gasCompressibility,
                       pvt._oilSaturation,
                       pvt._oilApiGravity,
                       pvt._oilViscosity,
                       pvt._oilFormationVolumeFactor,
                       pvt._oilCompressibility,
                       pvt._waterSaturation,
                       pvt._waterSpecificGravity,
                       pvt._waterViscosity,
                       pvt._waterFormationVolumeFactor,
                       pvt._waterCompressibility);
        }
        
        public override string ToString()
        {
            return string.Empty;
        }
    }
}