using System;
using System.Data;
using System.Runtime.CompilerServices;

namespace MultiPorosity.Models
{
    public sealed class ProductionRecord
    {
        private int _index;

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        private DateTime _date;

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private double _days;

        public double Days
        {
            get { return _days; }
            set { _days = value; }
        }

        private double _gas;

        public double Gas
        {
            get { return _gas; }
            set { _gas = value; }
        }

        private double _oil;

        public double Oil
        {
            get { return _oil; }
            set { _oil = value; }
        }

        private double _water;

        public double Water
        {
            get { return _water; }
            set { _water = value; }
        }

        private double _wellheadPressure;

        public double WellheadPressure
        {
            get { return _wellheadPressure; }
            set { _wellheadPressure = value; }
        }

        private double _weight;

        public double Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        public ProductionRecord()
        {
        }

        public ProductionRecord(int      index,
                                DateTime date,
                                double   days,
                                double   gas,
                                double   oil,
                                double   water)
        {
            Index            = index;
            Date             = date;
            Days             = days;
            Gas              = gas;
            Oil              = oil;
            Water            = water;
            WellheadPressure = 0.0;
            Weight           = 1.0;
        }

        public ProductionRecord(int      index,
                                DateTime date,
                                double   days,
                                double   gas,
                                double   oil,
                                double   water,
                                double   wellheadPressure,
                                double   weight)
        {
            Index            = index;
            Date             = date;
            Days             = days;
            Gas              = gas;
            Oil              = oil;
            Water            = water;
            WellheadPressure = wellheadPressure;
            Weight           = weight;
        }

        public ProductionRecord(int                         index,
                                ProductionDataSet.ActualRow model)
        {
            Index            = index;
            Date             = model.Date;
            Days             = model.Days;
            Gas              = model.Gas;
            Oil              = model.Oil;
            Water            = model.Water;
            WellheadPressure = model.WellheadPressure;
            Weight           = model.Weight;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetDate()
        {
            return _date.ToString("yyyy-MM-dd");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref float GetFloat(int index)
        {
            switch(index)
            {
                case 1:
                {
                    return ref UnManaged.Unsafe.As<double, float>(ref _days);
                }
                case 2:
                {
                    return ref UnManaged.Unsafe.As<double, float>(ref _gas);
                }
                case 3:
                {
                    return ref UnManaged.Unsafe.As<double, float>(ref _oil);
                }
                case 4:
                {
                    return ref UnManaged.Unsafe.As<double, float>(ref _water);
                }
                case 5:
                {
                    return ref UnManaged.Unsafe.As<double, float>(ref _wellheadPressure);
                }
                case 6:
                {
                    return ref UnManaged.Unsafe.As<double, float>(ref _weight);
                }
            }

            return ref UnManaged.Unsafe.As<double, float>(ref _weight);
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

        #region Equality members

        public bool Equals(ProductionRecord? other)
        {
            if(ReferenceEquals(null, other))
            {
                return false;
            }

            if(ReferenceEquals(this, other))
            {
                return true;
            }

            return Date.Equals(other.Date)   &&
                   Days.Equals(other.Days)   &&
                   Gas.Equals(other.Gas)     &&
                   Oil.Equals(other.Oil)     &&
                   Water.Equals(other.Water) &&
                   WellheadPressure.Equals(other.WellheadPressure);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Date, Days, Gas, Oil, Water, WellheadPressure);
        }

        #endregion

        #region Overrides of Object

        public override string ToString()
        {
            return $"{Index} {Date:MM/dd/yyyy} {Days} {Gas} {Oil} {Water} {WellheadPressure} {Weight}";
        }

        #endregion
    }
}