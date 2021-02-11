
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Engineering.UI;
using Engineering.UI.Controls;

using Prism.Mvvm;

namespace MultiPorosity.Presentation.Models
{
    public sealed class WellProperties : BindableBase
    {
        private string _aPI;
        private double _lateralLength;
        private double _bottomholePressure;

        [PropertyOrder(0)]
        //[Category("Well Properties")]
        [DisplayName("API")]
        [Description("")]
        [Editor(typeof(APIMaskedTextBox), typeof(APIMaskedTextBox))]
        public string API
        {
            get { return _aPI; }
            set
            {
                if(SetProperty(ref _aPI, value))
                {
                }
            }
        }

        [PropertyOrder(1)]
        //[Category("Well Properties")]
        [DisplayName("Lateral Length")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double LateralLength
        {
            get { return _lateralLength; }
            set
            {
                if(SetProperty(ref _lateralLength, value))
                {
                }
            }
        }

        [PropertyOrder(2)]
        //[Category("Well Properties")]
        [DisplayName("Bottomhole Pressure")]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double BottomholePressure
        {
            get { return _bottomholePressure; }
            set
            {
                if(SetProperty(ref _bottomholePressure, value))
                {
                }
            }
        }

        public WellProperties(string aPi,
                              double lateralLength,
                              double bottomholePressure)
        {
            _aPI                = aPi;
            _lateralLength      = lateralLength;
            _bottomholePressure = bottomholePressure;
        }

        public WellProperties(MultiPorosity.Services.Models.WellProperties wellProperties)
        {
            _aPI                = wellProperties.API;
            _lateralLength      = wellProperties.LateralLength;
            _bottomholePressure = wellProperties.BottomholePressure;
        }

        public static implicit operator MultiPorosity.Services.Models.WellProperties(WellProperties wellProperties)
        {
            return new(wellProperties._aPI,
                       wellProperties._lateralLength,
                       wellProperties._bottomholePressure);
        }
        
        public override string ToString()
        {
            return string.Empty;
        }
    }
}