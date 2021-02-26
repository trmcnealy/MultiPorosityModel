#nullable enable

using System;
using System.Runtime.CompilerServices;

using MultiPorosity.Models;

using Prism.Mvvm;

namespace MultiPorosity.Presentation.Models
{
    public sealed class ProductionHistoryModel : BindableBase
    {
        private int      _index;
        private DateTime _date;
        private double   _days;
        private double   _gas;
        private double   _oil;
        private double   _water;
        private double   _wellheadPressure;
        private double   _weight;

        public int Index
        {
            get { return _index; }
            set { this.SetProperty(ref _index, value); }
        }

        public DateTime Date
        {
            get { return _date; }
            set { this.SetProperty(ref _date, value); }
        }

        public double Days
        {
            get { return _days; }
            set { this.SetProperty(ref _days, value); }
        }

        public double Gas
        {
            get { return _gas; }
            set { this.SetProperty(ref _gas, value); }
        }

        public double Oil
        {
            get { return _oil; }
            set { this.SetProperty(ref _oil, value); }
        }

        public double Water
        {
            get { return _water; }
            set { this.SetProperty(ref _water, value); }
        }

        public double WellheadPressure
        {
            get { return _wellheadPressure; }
            set { this.SetProperty(ref _wellheadPressure, value); }
        }

        public double Weight
        {
            get { return _weight; }
            set { this.SetProperty(ref _weight, value); }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { this.SetProperty(ref _isSelected, value); }
        }

        public ProductionHistoryModel()
        {
        }

        public ProductionHistoryModel(int      index,
                                      DateTime date,
                                      double   days,
                                      double   gas,
                                      double   oil,
                                      double   water,
                                      double   wellheadPressure,
                                      double   weight)
        {
            _index            = index;
            _date             = date;
            _days             = days;
            _gas              = gas;
            _oil              = oil;
            _water            = water;
            _wellheadPressure = wellheadPressure;
            _weight           = weight;
        }

        public ProductionHistoryModel(ProductionRecord model)
        {
            Index            = model.Index;
            Date             = model.Date;
            Days             = model.Days;
            Gas              = model.Gas;
            Oil              = model.Oil;
            Water            = model.Water;
            WellheadPressure = model.WellheadPressure;
            Weight           = model.Weight;
        }

        public object this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                switch(index)
                {
                    case 1:
                    {
                        return Date;
                    }
                    case 2:
                    {
                        return Days;
                    }
                    case 3:
                    {
                        return Gas;
                    }
                    case 4:
                    {
                        return Oil;
                    }
                    case 5:
                    {
                        return Water;
                    }
                    case 6:
                    {
                        return WellheadPressure;
                    }
                    case 7:
                    {
                        return Weight;
                    }
                    default:
                    {
                        return Index;
                    }
                }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                switch(index)
                {
                    case 1:
                    {
                        if(value is string stringValue)
                        {
                            if(DateTime.TryParse(stringValue, out DateTime newValue))
                            {
                                Date = newValue;
                            }
                        }
                        else if(value is DateTime newValue)
                        {
                            Date = newValue;
                        }

                        break;
                    }
                    case 2:
                    {
                        if(value is string stringValue)
                        {
                            if(double.TryParse(stringValue, out double newValue))
                            {
                                Days = newValue;
                            }
                        }
                        else if(value is double newValue)
                        {
                            Days = newValue;
                        }

                        break;
                    }
                    case 3:
                    {
                        if(value is string stringValue)
                        {
                            if(double.TryParse(stringValue, out double newValue))
                            {
                                Gas = newValue;
                            }
                        }
                        else if(value is double newValue)
                        {
                            Gas = newValue;
                        }

                        break;
                    }
                    case 4:
                    {
                        if(value is string stringValue)
                        {
                            if(double.TryParse(stringValue, out double newValue))
                            {
                                Oil = newValue;
                            }
                        }
                        else if(value is double newValue)
                        {
                            Oil = newValue;
                        }

                        break;
                    }
                    case 5:
                    {
                        if(value is string stringValue)
                        {
                            if(double.TryParse(stringValue, out double newValue))
                            {
                                Water = newValue;
                            }
                        }
                        else if(value is double newValue)
                        {
                            Water = newValue;
                        }

                        break;
                    }
                    case 6:
                    {
                        if(value is string stringValue)
                        {
                            if(double.TryParse(stringValue, out double newValue))
                            {
                                WellheadPressure = newValue;
                            }
                        }
                        else if(value is double newValue)
                        {
                            WellheadPressure = newValue;
                        }

                        break;
                    }
                    case 7:
                    {
                        if(value is string stringValue)
                        {
                            if(double.TryParse(stringValue, out double newValue))
                            {
                                Weight = newValue;
                            }
                        }
                        else if(value is double newValue)
                        {
                            Weight = newValue;
                        }

                        break;
                    }
                }
            }
        }
    }
}