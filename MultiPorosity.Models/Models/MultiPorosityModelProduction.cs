using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace MultiPorosity.Models
{
    public sealed class MultiPorosityModelProduction
    {
        public double Days { get; set; }
        
        public double Gas { get; set; }
        
        public double Oil { get; set; }
        
        public double Water { get; set; }

        public MultiPorosityModelProduction(double days,
                                            double gas,
                                            double oil,
                                            double water)
        {
            Days  = days;
            Gas   = gas;
            Oil   = oil;
            Water = water;
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
                        return Days;
                    }
                    case 1:
                    {
                        return Gas;
                    }
                    case 2:
                    {
                        return Oil;
                    }
                    case 3:
                    {
                        return Water;
                    }
                    default:
                    {
                        return Days;
                    }
                }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                switch(index)
                {
                    case 0:
                    {
                        Days = value;

                        break;
                    }
                    case 1:
                    {
                        Gas = value;

                        break;
                    }
                    case 2:
                    {
                        Oil = value;

                        break;
                    }
                    case 3:
                    {
                        Water = value;

                        break;
                    }
                }
            }
        }
    }
}