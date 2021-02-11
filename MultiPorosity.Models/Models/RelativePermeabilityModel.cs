using System;
using System.Runtime.CompilerServices;

namespace MultiPorosity.Models
{
    public class RelativePermeabilityModel : IEquatable<RelativePermeabilityModel>
    {
        public double Sg { get; set; }
        public double So { get; set; }
        public double Sw { get; set; }

        public double Krg { get; set; }
        public double Kro { get; set; }
        public double Krw { get; set; }

        public RelativePermeabilityModel()
        {
        }

        public RelativePermeabilityModel(double sg,
                                         double so,
                                         double sw,
                                         double krg,
                                         double kro,
                                         double krw)
        {
            Sg  = sg;
            So  = so;
            Sw  = sw;
            Krg = krg;
            Kro = kro;
            Krw = krw;
        }

        public RelativePermeabilityModel(double sg,
                                         double so,
                                         double krg,
                                         double kro,
                                         double krw)
        {
            Sg  = sg;
            So  = so;
            Sw  = Math.Min(1.0, Math.Max(0.0, 1.0 - Sg - So));
            Krg = krg;
            Kro = kro;
            Krw = krw;
        }

        public double this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                switch(index)
                {
                    case 0:
                    {
                        return Sg;
                    }
                    case 1:
                    {
                        return So;
                    }
                    case 2:
                    {
                        return Sw;
                    }
                    case 3:
                    {
                        return Krg;
                    }
                    case 4:
                    {
                        return Kro;
                    }
                    case 5:
                    {
                        return Krw;
                    }
                    default:
                    {
                        return Sg;
                    }
                }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                switch (index)
                {
                    case 0:
                    {
                        Sg = value;
                        break;
                    }
                    case 1:
                    {
                        So = value;
                        break;
                    }
                    case 2:
                    {
                        Sw = value;
                        break;
                    }
                    case 3:
                    {
                        Krg = value;
                        break;
                    }
                    case 4:
                    {
                        Kro = value;
                        break;
                    }
                    case 5:
                    {
                        Krw = value;
                        break;
                    }
                }
            }
        }

        #region Equality members

        public bool Equals(RelativePermeabilityModel? other)
        {
            if(ReferenceEquals(null, other))
            {
                return false;
            }

            if(ReferenceEquals(this, other))
            {
                return true;
            }

            return So.Equals(other.So) && Sw.Equals(other.Sw) && Sg.Equals(other.Sg) && Kro.Equals(other.Kro) && Krw.Equals(other.Krw) && Krg.Equals(other.Krg);
        }

        public override bool Equals(object? obj)
        {
            if(ReferenceEquals(null, obj))
            {
                return false;
            }

            if(ReferenceEquals(this, obj))
            {
                return true;
            }

            if(obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((RelativePermeabilityModel)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(So, Sw, Sg, Kro, Krw, Krg);
        }

        public static bool operator ==(RelativePermeabilityModel? left,
                                       RelativePermeabilityModel? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RelativePermeabilityModel? left,
                                       RelativePermeabilityModel? right)
        {
            return !Equals(left, right);
        }

        #endregion

        #region Overrides of Object

        public override string ToString()
        {
            return $"{Sg} {So} {Sw} {Krg} {Kro} {Krw}";
        }

        #endregion
    }
}