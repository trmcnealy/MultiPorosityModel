
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using LayoutKind = System.Runtime.InteropServices.LayoutKind;
using ValueType = System.ValueType;

using Kokkos;

namespace MultiPorosity.Models
{
    namespace BoundConstraints
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct BoundConstraintsSingle
        {
            [FieldOffset(0)]
            public float Lower;

            [FieldOffset(sizeof(float))]
            public float Upper;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct BoundConstraintsDouble
        {
            [FieldOffset(0)]
            public double Lower;

            [FieldOffset(sizeof(double))]
            public double Upper;
        }
    }

    public sealed class BoundConstraints<T>
        where T : unmanaged
    {
        //private static readonly Type _T = typeof(T);
        //public static readonly int ThisSize;

        //private static readonly int _lowerOffset;
        //private static readonly int _upperOffset;

        //static BoundConstraints()
        //{
        //    _lowerOffset  = Marshal.OffsetOf<BoundConstraints<T>>(nameof(_lower)).ToInt32();
        //    _upperOffset  = Marshal.OffsetOf<BoundConstraints<T>>(nameof(_upper)).ToInt32();
        //    ThisSize = _upperOffset + Unsafe.SizeOf<T>();
        //}

        public T Lower
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get;
        }

        public T Upper
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get;
        }

        public BoundConstraints(T lower,
                                T upper)
        {
            Lower = lower;
            Upper = upper;
        }

        public BoundConstraints(BoundConstraints.BoundConstraintsSingle boundConstraints)
        {
            Lower = (T)(ValueType)boundConstraints.Lower;
            Upper = (T)(ValueType)boundConstraints.Upper;
        }

        public BoundConstraints(BoundConstraints.BoundConstraintsDouble boundConstraints)
        {
            Lower = (T)(ValueType)boundConstraints.Lower;
            Upper = (T)(ValueType)boundConstraints.Upper;
        }

        public static implicit operator BoundConstraints.BoundConstraintsSingle(BoundConstraints<T> boundConstraints)
        {
            return new BoundConstraints.BoundConstraintsSingle
            {
                Lower = (float)(ValueType)boundConstraints.Lower, Upper = (float)(ValueType)boundConstraints.Upper
            };
        }

        public static implicit operator BoundConstraints.BoundConstraintsDouble(BoundConstraints<T> boundConstraints)
        {
            return new BoundConstraints.BoundConstraintsDouble
            {
                Lower = (double)(ValueType)boundConstraints.Lower, Upper = (double)(ValueType)boundConstraints.Upper
            };
        }

        public static BoundConstraints<float> From(BoundConstraints.BoundConstraintsSingle value)
        {
            return new BoundConstraints<float>(value.Lower,
                                               value.Upper);
        }

        public static BoundConstraints<double> From(BoundConstraints.BoundConstraintsDouble value)
        {
            return new BoundConstraints<double>(value.Lower,
                                                value.Upper);
        }

        public static BoundConstraints.BoundConstraintsSingle To(BoundConstraints<float> value)
        {
            return new BoundConstraints.BoundConstraintsSingle
            {
                Lower = value.Lower, Upper = value.Upper
            };
        }

        public static BoundConstraints.BoundConstraintsDouble To(BoundConstraints<double> value)
        {
            return new BoundConstraints.BoundConstraintsDouble
            {
                Lower = value.Lower, Upper = value.Upper
            };
        }
    }
}