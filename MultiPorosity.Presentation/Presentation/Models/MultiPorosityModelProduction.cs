using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Reflection;


using MultiPorosity.Models;

using Prism.Mvvm;

namespace MultiPorosity.Presentation.Models
{

    public sealed class MultiPorosityModelProduction : BindableBase
    {
        private double _days;
        private double _gas;
        private double _oil;
        private double _water;

        public double Days
        {
            get { return _days; }
            set { SetProperty(ref _days, value); }
        }

        public double Gas
        {
            get { return _gas; }
            set { SetProperty(ref _gas, value); }
        }

        public double Oil
        {
            get { return _oil; }
            set { SetProperty(ref _oil, value); }
        }

        public double Water
        {
            get { return _water; }
            set { SetProperty(ref _water, value); }
        }

        public MultiPorosityModelProduction()
        {
        }

        public MultiPorosityModelProduction(double days,
                                            double gas,
                                            double oil,
                                            double water)
        {
            _days  = days;
            _gas   = gas;
            _oil   = oil;
            _water = water;
        }
        
        public static explicit operator MultiPorosityModelProduction(MultiPorosity.Services.Models.MultiPorosityModelProduction multiPorosityModelProduction)
        {
            return new(multiPorosityModelProduction.Days, multiPorosityModelProduction.Gas, multiPorosityModelProduction.Oil, multiPorosityModelProduction.Water);
        }

        public static implicit operator MultiPorosity.Services.Models.MultiPorosityModelProduction(MultiPorosityModelProduction multiPorosityModelProduction)
        {
            return new(multiPorosityModelProduction.Days, multiPorosityModelProduction.Gas, multiPorosityModelProduction.Oil, multiPorosityModelProduction.Water);
        }

        public static List<MultiPorosityModelProduction> Convert(List<MultiPorosity.Services.Models.MultiPorosityModelProduction> multiPorosityModelProduction)
        {
            List<MultiPorosityModelProduction> multiPorosityModelProductions = new(multiPorosityModelProduction.Count);

            for (int i = 0; i < multiPorosityModelProduction.Count; ++i)
            {
                multiPorosityModelProductions.Add((MultiPorosityModelProduction)multiPorosityModelProduction[i]);
            }

            return multiPorosityModelProductions;
        }

        public static List<MultiPorosity.Services.Models.MultiPorosityModelProduction> Convert(List<MultiPorosityModelProduction> multiPorosityModelProduction)
        {
            List<MultiPorosity.Services.Models.MultiPorosityModelProduction> multiPorosityModelProductions = new(multiPorosityModelProduction.Count);

            for (int i = 0; i < multiPorosityModelProduction.Count; ++i)
            {
                multiPorosityModelProductions.Add(multiPorosityModelProduction[i]);
            }

            return multiPorosityModelProductions;
        }
    }
}