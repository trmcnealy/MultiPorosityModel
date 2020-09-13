using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

using ReactiveUI;

namespace MultiPorosity.Tool
{
    public sealed class ProductionRecord : IEquatable<ProductionRecord>
    {
        public int Index { get; set; }

        public DateTime Date { get; set; }

        public double GasVolume { get; set; }

        public double OilVolume { get; set; }

        public double WaterVolume { get; set; }

        public double WellheadPressure { get; set; }

        public double Weight { get; set; }

        public ProductionRecord()
        {
        }

        public ProductionRecord(int      index,
                                DateTime date,
                                double   gasVolume,
                                double   oilVolume,
                                double   waterVolume,
                                double   wellheadPressure,
                                double   weight)
        {
            Index            = index;
            Date             = date;
            GasVolume        = gasVolume;
            OilVolume        = oilVolume;
            WaterVolume      = waterVolume;
            WellheadPressure = wellheadPressure;
            Weight           = weight;
        }

        public ProductionRecord(ProductionHistoryModel model)
        {
            Index            = model.Index;
            Date             = model.Date;
            GasVolume        = model.GasVolume;
            OilVolume        = model.OilVolume;
            WaterVolume      = model.WaterVolume;
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
                        return GasVolume;
                    }
                    case 3:
                    {
                        return OilVolume;
                    }
                    case 4:
                    {
                        return WaterVolume;
                    }
                    case 5:
                    {
                        return WellheadPressure;
                    }
                    case 6:
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
                                GasVolume = newValue;
                            }
                        }
                        else if(value is double newValue)
                        {
                            GasVolume = newValue;
                        }

                        break;
                    }
                    case 3:
                    {
                        if(value is string stringValue)
                        {
                            if(double.TryParse(stringValue, out double newValue))
                            {
                                OilVolume = newValue;
                            }
                        }
                        else if(value is double newValue)
                        {
                            OilVolume = newValue;
                        }

                        break;
                    }
                    case 4:
                    {
                        if(value is string stringValue)
                        {
                            if(double.TryParse(stringValue, out double newValue))
                            {
                                WaterVolume = newValue;
                            }
                        }
                        else if(value is double newValue)
                        {
                            WaterVolume = newValue;
                        }

                        break;
                    }
                    case 5:
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
                    case 6:
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

        public bool Equals(ProductionRecord other)
        {
            if(ReferenceEquals(null, other))
            {
                return false;
            }

            if(ReferenceEquals(this, other))
            {
                return true;
            }

            return Date.Equals(other.Date) && GasVolume.Equals(other.GasVolume) && OilVolume.Equals(other.OilVolume) && WaterVolume.Equals(other.WaterVolume) && WellheadPressure.Equals(other.WellheadPressure) && Weight.Equals(other.Weight);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is ProductionRecord other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Date, GasVolume, OilVolume, WaterVolume, WellheadPressure, Weight);
        }

        public static bool operator ==(ProductionRecord left,
                                       ProductionRecord right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ProductionRecord left,
                                       ProductionRecord right)
        {
            return !Equals(left, right);
        }
    }

    public sealed class ProductionHistoryModel : ReactiveObject
    {
        private int      _Index;
        private DateTime _Date;
        private double   _GasVolume;
        private double   _OilVolume;
        private double   _WaterVolume;
        private double   _WellheadPressure;
        private double   _Weight;

        public int Index { get { return _Index; } set { this.RaiseAndSetIfChanged(ref _Index, value); } }

        public DateTime Date { get { return _Date; } set { this.RaiseAndSetIfChanged(ref _Date, value); } }

        public double GasVolume { get { return _GasVolume; } set { this.RaiseAndSetIfChanged(ref _GasVolume, value); } }

        public double OilVolume { get { return _OilVolume; } set { this.RaiseAndSetIfChanged(ref _OilVolume, value); } }

        public double WaterVolume { get { return _WaterVolume; } set { this.RaiseAndSetIfChanged(ref _WaterVolume, value); } }

        public double WellheadPressure { get { return _WellheadPressure; } set { this.RaiseAndSetIfChanged(ref _WellheadPressure, value); } }

        public double Weight { get { return _Weight; } set { this.RaiseAndSetIfChanged(ref _Weight, value); } }

        
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { this.RaiseAndSetIfChanged(ref _isSelected, value); }
        }

        public ProductionHistoryModel()
        {
        }

        public ProductionHistoryModel(int      index,
                                      DateTime date,
                                      double   gasVolume,
                                      double   oilVolume,
                                      double   waterVolume,
                                      double   wellheadPressure,
                                      double   weight)
        {
            _Index            = index;
            _Date             = date;
            _GasVolume        = gasVolume;
            _OilVolume        = oilVolume;
            _WaterVolume      = waterVolume;
            _WellheadPressure = wellheadPressure;
            _Weight           = weight;
        }

        public ProductionHistoryModel(ProductionRecord model)
        {
            Index            = model.Index;
            Date             = model.Date;
            GasVolume        = model.GasVolume;
            OilVolume        = model.OilVolume;
            WaterVolume      = model.WaterVolume;
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
                        return GasVolume;
                    }
                    case 3:
                    {
                        return OilVolume;
                    }
                    case 4:
                    {
                        return WaterVolume;
                    }
                    case 5:
                    {
                        return WellheadPressure;
                    }
                    case 6:
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
                                GasVolume = newValue;
                            }
                        }
                        else if(value is double newValue)
                        {
                            GasVolume = newValue;
                        }

                        break;
                    }
                    case 3:
                    {
                        if(value is string stringValue)
                        {
                            if(double.TryParse(stringValue, out double newValue))
                            {
                                OilVolume = newValue;
                            }
                        }
                        else if(value is double newValue)
                        {
                            OilVolume = newValue;
                        }

                        break;
                    }
                    case 4:
                    {
                        if(value is string stringValue)
                        {
                            if(double.TryParse(stringValue, out double newValue))
                            {
                                WaterVolume = newValue;
                            }
                        }
                        else if(value is double newValue)
                        {
                            WaterVolume = newValue;
                        }

                        break;
                    }
                    case 5:
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
                    case 6:
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