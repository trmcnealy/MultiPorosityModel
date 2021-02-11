using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Engineering.UI;
using Engineering.UI.Controls;

using MultiPorosity.Presentation.Services;

using Prism.Mvvm;

namespace MultiPorosity.Presentation.Models
{    
    public sealed class FractureProperties : BindableBase
    {
        private int    _count;
        private double _width;
        private double _height;
        private double _halfLength;
        private double _porosity;
        private double _permeability;
        private double _skin;
        
        [PropertyOrder(0)]
        //[Category("Fracture Properties")]
        [DisplayName("Count")]
        [Description("")]
        [Editor(typeof(IntegerUpDown), typeof(IntegerUpDown))]
        public int Count
        {
            get { return _count; }
            set
            {
                if(SetProperty(ref _count, value))
                {
                }
            }
        }

        [PropertyOrder(1)]
        //[Category("Fracture Properties")]
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
        //[Category("Fracture Properties")]
        [DisplayName("Height")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double Height
        {
            get { return _height; }
            set
            {
                if(SetProperty(ref _height, value))
                {
                }
            }
        }

        [PropertyOrder(3)]
        //[Category("Fracture Properties")]
        [DisplayName("Half Length")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double HalfLength
        {
            get { return _halfLength; }
            set
            {
                if(SetProperty(ref _halfLength, value))
                {
                }
            }
        }

        [PropertyOrder(4)]
        //[Category("Fracture Properties")]
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

        [PropertyOrder(5)]
        //[Category("Fracture Properties")]
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

        [PropertyOrder(6)]
        //[Category("Fracture Properties")]
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

        public FractureProperties(MultiPorosity.Services.Models.FractureProperties fractureProperties)
        {
            
            _count              = fractureProperties.Count;
            _width              = fractureProperties.Width;
            _height             = fractureProperties.Height;
            _halfLength         = fractureProperties.HalfLength;
            _porosity           = fractureProperties.Porosity;
            _permeability       = fractureProperties.Permeability;
            _skin               = fractureProperties.Skin;
        }
        
        public static implicit operator MultiPorosity.Services.Models.FractureProperties(FractureProperties fractureProperties)
        {
            return new(fractureProperties._count,
                       fractureProperties._width,
                       fractureProperties._height,
                       fractureProperties._halfLength,
                       fractureProperties._porosity,
                       fractureProperties._permeability,
                       fractureProperties._skin);
        }
        
        public override string ToString()
        {
            return string.Empty;
        }
    }
}