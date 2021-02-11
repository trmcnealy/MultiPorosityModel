using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Engineering.UI;
using Engineering.UI.Controls;

using Prism.Mvvm;

namespace MultiPorosity.Presentation.Models
{
    public sealed class ReservoirProperties : BindableBase
    {
        private double _length;
        private double _width;
        private double _thickness;
        private double _porosity;
        private double _permeability;
        private double _compressibility;
        private double _bottomholeTemperature;
        private double _initialPressure;


        [PropertyOrder(0)]
        //[Category("Reservoir Properties")]
        [DisplayName("Length")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double Length
        {
            get { return _length; }
            set
            {
                if(SetProperty(ref _length, value))
                {
                }
            }
        }

        [PropertyOrder(1)]
        //[Category("Reservoir Properties")]
        [DisplayName("Width")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double Width
        {
            get { return _width; }
            set
            {
                if(SetProperty(ref _width, value))
                {
                }
            }
        }

        [PropertyOrder(2)]
        //[Category("Reservoir Properties")]
        [DisplayName("Thickness")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double Thickness
        {
            get { return _thickness; }
            set
            {
                if(SetProperty(ref _thickness, value))
                {
                }
            }
        }

        [PropertyOrder(3)]
        //[Category("Reservoir Properties")]
        [DisplayName("Porosity")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double Porosity
        {
            get { return _porosity; }
            set
            {
                if(SetProperty(ref _porosity, value))
                {
                }
            }
        }

        [PropertyOrder(4)]
        //[Category("Reservoir Properties")]
        [DisplayName("Permeability")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double Permeability
        {
            get { return _permeability; }
            set
            {
                if(SetProperty(ref _permeability, value))
                {
                }
            }
        }

        [PropertyOrder(5)]
        //[Category("Reservoir Properties")]
        [DisplayName("Compressibility")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double Compressibility
        {
            get { return _compressibility; }
            set
            {
                if(SetProperty(ref _compressibility, value))
                {
                }
            }
        }

        [PropertyOrder(6)]
        //[Category("Reservoir Properties")]
        [DisplayName("Bottomhole Temperature")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double BottomholeTemperature
        {
            get { return _bottomholeTemperature; }
            set
            {
                if(SetProperty(ref _bottomholeTemperature, value))
                {
                }
            }
        }

        [PropertyOrder(7)]
        //[Category("Reservoir Properties")]
        [DisplayName("Initial Pressure")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double InitialPressure
        {
            get { return _initialPressure; }
            set
            {
                if(SetProperty(ref _initialPressure, value))
                {
                }
            }
        }

        public ReservoirProperties(double length,
                                   double width,
                                   double thickness,
                                   double porosity,
                                   double permeability,
                                   double compressibility,
                                   double bottomholeTemperature,
                                   double initialPressure)
        {
            _length                = length;
            _width                 = width;
            _thickness             = thickness;
            _porosity              = porosity;
            _permeability          = permeability;
            _compressibility       = compressibility;
            _bottomholeTemperature = bottomholeTemperature;
            _initialPressure       = initialPressure;
        }

        public ReservoirProperties(MultiPorosity.Services.Models.ReservoirProperties reservoirProperties)
        {
            _length                = reservoirProperties.Length;
            _width                 = reservoirProperties.Width;
            _thickness             = reservoirProperties.Thickness;
            _porosity              = reservoirProperties.Porosity;
            _permeability          = reservoirProperties.Permeability;
            _compressibility       = reservoirProperties.Compressibility;
            _bottomholeTemperature = reservoirProperties.BottomholeTemperature;
            _initialPressure       = reservoirProperties.InitialPressure;
        }

        public static implicit operator MultiPorosity.Services.Models.ReservoirProperties(ReservoirProperties reservoirProperties)
        {
            return new(reservoirProperties._length,
                       reservoirProperties._width,
                       reservoirProperties._thickness,
                       reservoirProperties._porosity,
                       reservoirProperties._permeability,
                       reservoirProperties._compressibility,
                       reservoirProperties._bottomholeTemperature,
                       reservoirProperties._initialPressure);
        }
        
        public override string ToString()
        {
            return string.Empty;
        }
    }
}