using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Engineering.UI;
using Engineering.UI.Controls;

using Prism.Mvvm;
using PVT;

namespace MultiPorosity.Presentation.Models
{
    public sealed class PvtModelProperties : BindableBase
    {
        private double _separatorPressure;

        private double _separatorTemperature;

        private double _waterSalinity;

        private GasViscosityType _gasViscosityType;

        private GasFormationVolumeFactorType _gasFormationVolumeFactorType;

        private GasCompressibilityFactorType _gasCompressibilityFactorType;

        private GasPseudoCriticalType _gasPseudoCriticalType;

        private GasCompressibilityType _gasCompressibilityType;

        private OilSolutionGasType _oilSolutionGasType;

        private OilBubblePointType _oilBubblePointType;

        private DeadOilViscosityType _deadOilViscosityType;

        private SaturatedOilViscosityType _saturatedOilViscosityType;

        private UnderSaturatedOilViscosityType _underSaturatedOilViscosityType;

        private OilFormationVolumeFactorType _oilFormationVolumeFactorType;

        private OilCompressibilityType _oilCompressibilityType;

        private WaterViscosityType _waterViscosityType;

        private WaterFormationVolumeFactorType _waterFormationVolumeFactorType;

        private WaterCompressibilityType _waterCompressibilityType;

        [PropertyOrder(0)]
        //[Category("Pvt")]
        [DisplayName("Separator Pressure")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double SeparatorPressure
        {
            get { return _separatorPressure; }
            set
            {
                if(SetProperty(ref _separatorPressure, value))
                {
                }
            }
        }

        [PropertyOrder(1)]
        //[Category("Pvt")]
        [DisplayName("Separator Temperature")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double SeparatorTemperature
        {
            get { return _separatorTemperature; }
            set
            {
                if(SetProperty(ref _separatorTemperature, value))
                {
                }
            }
        }


        [PropertyOrder(5)]
        //[Category("Pvt")]
        [DisplayName("Gas Viscosity Type")]
        [Description("")]
        [Editor(typeof(EnumComboBoxEditor), typeof(EnumComboBoxEditor))]
        public GasViscosityType GasViscosityType
        {
            get { return _gasViscosityType; }
            set
            {
                if(SetProperty(ref _gasViscosityType, value))
                {
                }
            }
        }



        [PropertyOrder(7)]
        //[Category("Pvt")]
        [DisplayName("Gas Formation Volume Factor Type")]
        [Description("")]
        [Editor(typeof(EnumComboBoxEditor), typeof(EnumComboBoxEditor))]
        public GasFormationVolumeFactorType GasFormationVolumeFactorType
        {
            get { return _gasFormationVolumeFactorType; }
            set
            {
                if(SetProperty(ref _gasFormationVolumeFactorType, value))
                {
                }
            }
        }

        

        [PropertyOrder(9)]
        //[Category("Pvt")]
        [DisplayName("Gas Compressibility Factor Type")]
        [Description("")]
        [Editor(typeof(EnumComboBoxEditor), typeof(EnumComboBoxEditor))]
        public GasCompressibilityFactorType GasCompressibilityFactorType
        {
            get { return _gasCompressibilityFactorType; }
            set
            {
                if(SetProperty(ref _gasCompressibilityFactorType, value))
                {
                }
            }
        }

        [PropertyOrder(10)]
        //[Category("Pvt")]
        [DisplayName("Gas Pseudo-Critical Type")]
        [Description("")]
        [Editor(typeof(EnumComboBoxEditor), typeof(EnumComboBoxEditor))]
        public GasPseudoCriticalType GasPseudoCriticalType
        {
            get { return _gasPseudoCriticalType; }
            set
            {
                if(SetProperty(ref _gasPseudoCriticalType, value))
                {
                }
            }
        }

        

        [PropertyOrder(12)]
        //[Category("Pvt")]
        [DisplayName("Gas Compressibility Type")]
        [Description("")]
        [Editor(typeof(EnumComboBoxEditor), typeof(EnumComboBoxEditor))]
        public GasCompressibilityType GasCompressibilityType
        {
            get { return _gasCompressibilityType; }
            set
            {
                if(SetProperty(ref _gasCompressibilityType, value))
                {
                }
            }
        }

        [PropertyOrder(13)]
        //[Category("Pvt")]
        [DisplayName("Oil Solution Gas Type")]
        [Description("")]
        [Editor(typeof(EnumComboBoxEditor), typeof(EnumComboBoxEditor))]
        public OilSolutionGasType OilSolutionGasType
        {
            get { return _oilSolutionGasType; }
            set
            {
                if(SetProperty(ref _oilSolutionGasType, value))
                {
                }
            }
        }

        [PropertyOrder(14)]
        //[Category("Pvt")]
        [DisplayName("Oil Bubble Point Type")]
        [Description("")]
        [Editor(typeof(EnumComboBoxEditor), typeof(EnumComboBoxEditor))]
        public OilBubblePointType OilBubblePointType
        {
            get { return _oilBubblePointType; }
            set
            {
                if(SetProperty(ref _oilBubblePointType, value))
                {
                }
            }
        }

        [PropertyOrder(18)]
        //[Category("Pvt")]
        [DisplayName("Dead-Oil Viscosity Type")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public DeadOilViscosityType DeadOilViscosityType
        {
            get { return _deadOilViscosityType; }
            set
            {
                if(SetProperty(ref _deadOilViscosityType, value))
                {
                }
            }
        }

        [PropertyOrder(19)]
        //[Category("Pvt")]
        [DisplayName("Saturated Oil Viscosity Type")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public SaturatedOilViscosityType SaturatedOilViscosityType
        {
            get { return _saturatedOilViscosityType; }
            set
            {
                if(SetProperty(ref _saturatedOilViscosityType, value))
                {
                }
            }
        }

        [PropertyOrder(20)]
        //[Category("Pvt")]
        [DisplayName("Under-Saturated Oil Viscosity Type")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public UnderSaturatedOilViscosityType UnderSaturatedOilViscosityType
        {
            get { return _underSaturatedOilViscosityType; }
            set
            {
                if(SetProperty(ref _underSaturatedOilViscosityType, value))
                {
                }
            }
        }

        [PropertyOrder(22)]
        //[Category("Pvt")]
        [DisplayName("Oil Formation Volume Factor Type")]
        [Description("")]
        [Editor(typeof(EnumComboBoxEditor), typeof(EnumComboBoxEditor))]
        public OilFormationVolumeFactorType OilFormationVolumeFactorType
        {
            get { return _oilFormationVolumeFactorType; }
            set
            {
                if(SetProperty(ref _oilFormationVolumeFactorType, value))
                {
                }
            }
        }

        [PropertyOrder(24)]
        //[Category("Pvt")]
        [DisplayName("Oil Compressibility Type")]
        [Description("")]
        [Editor(typeof(EnumComboBoxEditor), typeof(EnumComboBoxEditor))]
        public OilCompressibilityType OilCompressibilityType
        {
            get { return _oilCompressibilityType; }
            set
            {
                if(SetProperty(ref _oilCompressibilityType, value))
                {
                }
            }
        }

       

        [PropertyOrder(27)]
        //[Category("Pvt")]
        [DisplayName("Water Salinity")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double WaterSalinity
        {
            get { return _waterSalinity; }
            set
            {
                if(SetProperty(ref _waterSalinity, value))
                {
                }
            }
        }

        

        [PropertyOrder(29)]
        //[Category("Pvt")]
        [DisplayName("Water Viscosity Type")]
        [Description("")]
        [Editor(typeof(EnumComboBoxEditor), typeof(EnumComboBoxEditor))]
        public WaterViscosityType WaterViscosityType
        {
            get { return _waterViscosityType; }
            set
            {
                if(SetProperty(ref _waterViscosityType, value))
                {
                }
            }
        }

        

        [PropertyOrder(31)]
        //[Category("Pvt")]
        [DisplayName("Water Formation Volume Factor Type")]
        [Description("")]
        [Editor(typeof(EnumComboBoxEditor), typeof(EnumComboBoxEditor))]
        public WaterFormationVolumeFactorType WaterFormationVolumeFactorType
        {
            get { return _waterFormationVolumeFactorType; }
            set
            {
                if(SetProperty(ref _waterFormationVolumeFactorType, value))
                {
                }
            }
        }

        [PropertyOrder(33)]
        //[Category("Pvt")]
        [DisplayName("Water Compressibility Type")]
        [Description("")]
        [Editor(typeof(EnumComboBoxEditor), typeof(EnumComboBoxEditor))]
        public WaterCompressibilityType WaterCompressibilityType
        {
            get { return _waterCompressibilityType; }
            set
            {
                if(SetProperty(ref _waterCompressibilityType, value))
                {
                }
            }
        }

        public PvtModelProperties(MultiPorosity.Services.Models.PvtModelProperties pvtModelProperties)
        {
            _separatorPressure              = pvtModelProperties.SeparatorPressure;
            _separatorTemperature           = pvtModelProperties.SeparatorTemperature;
            _waterSalinity                  = pvtModelProperties.WaterSalinity;
            _gasViscosityType               = pvtModelProperties.GasViscosityType;
            _gasFormationVolumeFactorType   = pvtModelProperties.GasFormationVolumeFactorType;
            _gasCompressibilityFactorType   = pvtModelProperties.GasCompressibilityFactorType;
            _gasPseudoCriticalType          = pvtModelProperties.GasPseudoCriticalType;
            _gasCompressibilityType         = pvtModelProperties.GasCompressibilityType;
            _oilSolutionGasType             = pvtModelProperties.OilSolutionGasType;
            _oilBubblePointType             = pvtModelProperties.OilBubblePointType;
            _deadOilViscosityType           = pvtModelProperties.DeadOilViscosityType;
            _saturatedOilViscosityType      = pvtModelProperties.SaturatedOilViscosityType;
            _underSaturatedOilViscosityType = pvtModelProperties.UnderSaturatedOilViscosityType;
            _oilFormationVolumeFactorType   = pvtModelProperties.OilFormationVolumeFactorType;
            _oilCompressibilityType         = pvtModelProperties.OilCompressibilityType;
            _waterViscosityType             = pvtModelProperties.WaterViscosityType;
            _waterFormationVolumeFactorType = pvtModelProperties.WaterFormationVolumeFactorType;
            _waterCompressibilityType       = pvtModelProperties.WaterCompressibilityType;
        }
        
        public static implicit operator MultiPorosity.Services.Models.PvtModelProperties(PvtModelProperties pvtModelProperties)
        {
            return new(pvtModelProperties._separatorPressure,
                       pvtModelProperties._separatorTemperature,
                       pvtModelProperties._waterSalinity,
                       pvtModelProperties._gasViscosityType,
                       pvtModelProperties._gasFormationVolumeFactorType,
                       pvtModelProperties._gasCompressibilityFactorType,
                       pvtModelProperties._gasPseudoCriticalType,
                       pvtModelProperties._gasCompressibilityType,
                       pvtModelProperties._oilSolutionGasType,
                       pvtModelProperties._oilBubblePointType,
                       pvtModelProperties._deadOilViscosityType,
                       pvtModelProperties._saturatedOilViscosityType,
                       pvtModelProperties._underSaturatedOilViscosityType,
                       pvtModelProperties._oilFormationVolumeFactorType,
                       pvtModelProperties._oilCompressibilityType,
                       pvtModelProperties._waterViscosityType,
                       pvtModelProperties._waterFormationVolumeFactorType,
                       pvtModelProperties._waterCompressibilityType);
        }
        
        public override string ToString()
        {
            return string.Empty;
        }
    }
}