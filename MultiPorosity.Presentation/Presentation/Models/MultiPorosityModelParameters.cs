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
    public class MultiPorosityModelParameters : BindableBase
    {
        private double _days;
        private double _matrixPermeability;
        private double _hydraulicFracturePermeability;
        private double _naturalFracturePermeability;
        private double _hydraulicFractureHalfLength;
        private double _hydraulicFractureSpacing;
        private double _naturalFractureSpacing;
        private double _skin;

        [PropertyOrder(0)]
        //[Category("Multi-Porosity Model")]
        [DisplayName("Days")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double Days
        {
            get { return _days; }
            set
            {
                if(SetProperty(ref _days, value))
                {
                }
            }
        }

        [PropertyOrder(1)]
        //[Category("Multi-Porosity Model")]
        [DisplayName("Matrix Permeability")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double MatrixPermeability
        {
            get { return _matrixPermeability; }
            set
            {
                if(SetProperty(ref _matrixPermeability, value))
                {
                }
            }
        }

        [PropertyOrder(2)]
        //[Category("Multi-Porosity Model")]
        [DisplayName("Hydraulic Fracture Permeability")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double HydraulicFracturePermeability
        {
            get { return _hydraulicFracturePermeability; }
            set
            {
                if(SetProperty(ref _hydraulicFracturePermeability, value))
                {
                }
            }
        }

        [PropertyOrder(3)]
        //[Category("Multi-Porosity Model")]
        [DisplayName("Natural Fracture Permeability")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double NaturalFracturePermeability
        {
            get { return _naturalFracturePermeability; }
            set
            {
                if(SetProperty(ref _naturalFracturePermeability, value))
                {
                }
            }
        }

        [PropertyOrder(4)]
        //[Category("Multi-Porosity Model")]
        [DisplayName("Hydraulic Fracture Half Length")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double HydraulicFractureHalfLength
        {
            get { return _hydraulicFractureHalfLength; }
            set
            {
                if(SetProperty(ref _hydraulicFractureHalfLength, value))
                {
                }
            }
        }

        [PropertyOrder(5)]
        //[Category("Multi-Porosity Model")]
        [DisplayName("Hydraulic Fracture Spacing")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double HydraulicFractureSpacing
        {
            get { return _hydraulicFractureSpacing; }
            set
            {
                if(SetProperty(ref _hydraulicFractureSpacing, value))
                {
                }
            }
        }

        [PropertyOrder(6)]
        //[Category("Multi-Porosity Model")]
        [DisplayName("Natural Fracture Spacing")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double NaturalFractureSpacing
        {
            get { return _naturalFractureSpacing; }
            set
            {
                if(SetProperty(ref _naturalFractureSpacing, value))
                {
                }
            }
        }

        [PropertyOrder(7)]
        //[Category("Multi-Porosity Model")]
        [DisplayName("Skin")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double Skin
        {
            get { return _skin; }
            set
            {
                if(SetProperty(ref _skin, value))
                {
                }
            }
        }

        public MultiPorosityModelParameters(MultiPorosity.Services.Models.MultiPorosityModelParameters multiPorosityModelParameters)
        {
            _days                         = multiPorosityModelParameters.Days;
            _matrixPermeability           = multiPorosityModelParameters.MatrixPermeability;
            _hydraulicFracturePermeability = multiPorosityModelParameters.HydraulicFracturePermeability;
            _naturalFracturePermeability  = multiPorosityModelParameters.NaturalFracturePermeability;
            _hydraulicFractureHalfLength   = multiPorosityModelParameters.HydraulicFractureHalfLength;
            _hydraulicFractureSpacing      = multiPorosityModelParameters.HydraulicFractureSpacing;
            _naturalFractureSpacing       = multiPorosityModelParameters.NaturalFractureSpacing;
            _skin                         = multiPorosityModelParameters.Skin;
        }

        public static implicit operator MultiPorosity.Services.Models.MultiPorosityModelParameters(MultiPorosityModelParameters multiPorosityModelParameters)
        {
            return new(multiPorosityModelParameters._days,
                       multiPorosityModelParameters._matrixPermeability,
                       multiPorosityModelParameters._hydraulicFracturePermeability,
                       multiPorosityModelParameters._naturalFracturePermeability,
                       multiPorosityModelParameters._hydraulicFractureHalfLength,
                       multiPorosityModelParameters._hydraulicFractureSpacing,
                       multiPorosityModelParameters._naturalFractureSpacing,
                       multiPorosityModelParameters._skin);
        }
        
        public override string ToString()
        {
            return string.Empty;
        }
    }
}