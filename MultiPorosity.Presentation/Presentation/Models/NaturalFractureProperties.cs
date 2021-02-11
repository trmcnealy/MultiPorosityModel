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
    public sealed class NaturalFractureProperties : BindableBase
    {
        private int    _count;
        private double _width;
        private double _porosity;
        private double _permeability;
        
        [PropertyOrder(0)]
        //[Category("Natural Fracture Properties")]
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
        //[Category("Natural Fracture Properties")]
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
        //[Category("Natural Fracture Properties")]
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

        [PropertyOrder(3)]
        //[Category("Natural Fracture Properties")]
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

        public NaturalFractureProperties(MultiPorosity.Services.Models.NaturalFractureProperties naturalFractureProperties)
        {
            _count        = naturalFractureProperties.Count;
            _width        = naturalFractureProperties.Width;
            _porosity     = naturalFractureProperties.Porosity;
            _permeability = naturalFractureProperties.Permeability;
        }
        
        public static implicit operator MultiPorosity.Services.Models.NaturalFractureProperties(NaturalFractureProperties naturalFractureProperties)
        {
            return new(naturalFractureProperties._count,
                       naturalFractureProperties._width,
                       naturalFractureProperties._porosity,
                       naturalFractureProperties._permeability);
        }
        
        public override string ToString()
        {
            return string.Empty;
        }
    }
}