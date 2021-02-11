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
    public sealed class RelativePermeabilities : BindableBase
    {
        private double _matrixOil;
        private double _matrixWater;
        private double _matrixGas;
        private double _fractureOil;
        private double _fractureWater;
        private double _fractureGas;
        private double _naturalFractureOil;
        private double _naturalFractureWater;
        private double _naturalFractureGas;

        [PropertyOrder(0)]
        //[Category("Relative Permeabilities")]
        [DisplayName("MatrixOil")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double MatrixOil
        {
            get { return _matrixOil; }
            set
            {
                if(SetProperty(ref _matrixOil, value))
                {
                }
            }
        }

        [PropertyOrder(1)]
        //[Category("Relative Permeabilities")]
        [DisplayName("MatrixWater")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double MatrixWater
        {
            get { return _matrixWater; }
            set
            {
                if(SetProperty(ref _matrixWater, value))
                {
                }
            }
        }

        [PropertyOrder(2)]
        //[Category("Relative Permeabilities")]
        [DisplayName("MatrixGas")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double MatrixGas
        {
            get { return _matrixGas; }
            set
            {
                if(SetProperty(ref _matrixGas, value))
                {
                }
            }
        }

        [PropertyOrder(3)]
        //[Category("Relative Permeabilities")]
        [DisplayName("FractureOil")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double FractureOil
        {
            get { return _fractureOil; }
            set
            {
                if(SetProperty(ref _fractureOil, value))
                {
                }
            }
        }

        [PropertyOrder(4)]
        //[Category("Relative Permeabilities")]
        [DisplayName("FractureWater")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double FractureWater
        {
            get { return _fractureWater; }
            set
            {
                if(SetProperty(ref _fractureWater, value))
                {
                }
            }
        }

        [PropertyOrder(5)]
        //[Category("Relative Permeabilities")]
        [DisplayName("FractureGas")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double FractureGas
        {
            get { return _fractureGas; }
            set
            {
                if(SetProperty(ref _fractureGas, value))
                {
                }
            }
        }

        [PropertyOrder(6)]
        //[Category("Relative Permeabilities")]
        [DisplayName("NaturalFractureOil")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double NaturalFractureOil
        {
            get { return _naturalFractureOil; }
            set
            {
                if(SetProperty(ref _naturalFractureOil, value))
                {
                }
            }
        }

        [PropertyOrder(7)]
        //[Category("Relative Permeabilities")]
        [DisplayName("NaturalFractureWater")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double NaturalFractureWater
        {
            get { return _naturalFractureWater; }
            set
            {
                if(SetProperty(ref _naturalFractureWater, value))
                {
                }
            }
        }

        [PropertyOrder(8)]
        //[Category("Relative Permeabilities")]
        [DisplayName("NaturalFractureGas")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double NaturalFractureGas
        {
            get { return _naturalFractureGas; }
            set
            {
                if(SetProperty(ref _naturalFractureGas, value))
                {
                }
            }
        }

        public RelativePermeabilities(MultiPorosity.Services.Models.RelativePermeabilities relativePermeabilities)
        {
            _matrixOil            = relativePermeabilities.MatrixOil;
            _matrixWater          = relativePermeabilities.MatrixWater;
            _matrixGas            = relativePermeabilities.MatrixGas;
            _fractureOil          = relativePermeabilities.FractureOil;
            _fractureWater        = relativePermeabilities.FractureWater;
            _fractureGas          = relativePermeabilities.FractureGas;
            _naturalFractureOil   = relativePermeabilities.NaturalFractureOil;
            _naturalFractureWater = relativePermeabilities.NaturalFractureWater;
            _naturalFractureGas   = relativePermeabilities.NaturalFractureGas;
        }

        public static implicit operator MultiPorosity.Services.Models.RelativePermeabilities(RelativePermeabilities relativePermeabilities)
        {
            return new(relativePermeabilities._matrixOil,
                       relativePermeabilities._matrixWater,
                       relativePermeabilities._matrixGas,
                       relativePermeabilities._fractureOil,
                       relativePermeabilities._fractureWater,
                       relativePermeabilities._fractureGas,
                       relativePermeabilities._naturalFractureOil,
                       relativePermeabilities._naturalFractureWater,
                       relativePermeabilities._naturalFractureGas);
        }
        
        public override string ToString()
        {
            return string.Empty;
        }
    }
}