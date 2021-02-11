using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace MultiPorosity.Models
{

    [StructLayout(LayoutKind.Explicit, Pack = sizeof(double), Size = sizeof(double) * 3)]
    public struct PoreSaturation
    {
        [DataMember]
        [FieldOffset(0)]
        public double SaturationGas;

        [DataMember]
        [FieldOffset(sizeof(double))]
        public double SaturationOil;

        [DataMember]
        [FieldOffset(sizeof(double) * 2)]
        public double SaturationWater;

        [FieldOffset(0)]
        public unsafe fixed double Saturations[3];

        public PoreSaturation(double saturationGas,
                          double saturationOil,
                          double saturationWater)
            : this()
        {
            SaturationGas   = saturationGas;
            SaturationOil   = saturationOil;
            SaturationWater = saturationWater;
        }

        public ref double this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get
            {
                unsafe
                {
                    return ref Saturations[index];
                }
            }
        }

        public double saturation(int index)
        {
            unsafe
            {
                return Saturations[index];
            }
        }
    }

}