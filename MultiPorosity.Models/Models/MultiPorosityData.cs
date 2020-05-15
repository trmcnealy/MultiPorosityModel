using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Kokkos;

namespace MultiPorosity.Models
{
    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public sealed unsafe class MultiPorosityData<T>
        where T : unmanaged
    {
        private static readonly Type _T = typeof(T);
        public static readonly int ThisSize;
        
        private static readonly int _ProductionDataOffset;
        
        private static readonly int _ReservoirPropertiesOffset;
        
        private static readonly int _WellPropertiesOffset;
        
        private static readonly int _FracturePropertiesOffset;
        
        private static readonly int _NaturalFracturePropertiesOffset;
        
        private static readonly int _PvtOffset;

        static MultiPorosityData()
        {
            _ProductionDataOffset = Marshal.OffsetOf<MultiPorosityData<T>>(nameof(_ProductionData)).ToInt32();
            _ReservoirPropertiesOffset = Marshal.OffsetOf<MultiPorosityData<T>>(nameof(_ReservoirProperties)).ToInt32();
            _WellPropertiesOffset = Marshal.OffsetOf<MultiPorosityData<T>>(nameof(_WellProperties)).ToInt32();
            _FracturePropertiesOffset = Marshal.OffsetOf<MultiPorosityData<T>>(nameof(_FractureProperties)).ToInt32();
            _NaturalFracturePropertiesOffset = Marshal.OffsetOf<MultiPorosityData<T>>(nameof(_NaturalFractureProperties)).ToInt32();
            _PvtOffset = Marshal.OffsetOf<MultiPorosityData<T>>(nameof(_Pvt)).ToInt32();
            ThisSize = _PvtOffset + Unsafe.SizeOf<IntPtr>();
        }
        
        private IntPtr _ProductionData;
        
        private IntPtr _ReservoirProperties;
        
        private IntPtr _WellProperties;
        
        private IntPtr _FractureProperties;
        
        private IntPtr _NaturalFractureProperties;
        
        private IntPtr _Pvt;
        
        private readonly NativePointer pointer;

        public ProductionData<T> ProductionData
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return new ProductionData<T>(*(IntPtr*)(pointer.Data + _ProductionDataOffset)); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(IntPtr*)(pointer.Data + _ProductionDataOffset) = value.Instance.Data; }
        }
        
        public ReservoirProperties<T> ReservoirProperties
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return new ReservoirProperties<T>(*(IntPtr*)(pointer.Data + _ReservoirPropertiesOffset)); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(IntPtr*)(pointer.Data + _ReservoirPropertiesOffset) = value.Instance; }
        }

        public WellProperties<T> WellProperties
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return new WellProperties<T>(*(IntPtr*)(pointer.Data + _WellPropertiesOffset)); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(IntPtr*)(pointer.Data + _WellPropertiesOffset) = value.Instance; }
        }

        public FractureProperties<T> FractureProperties
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return new FractureProperties<T>(*(IntPtr*)(pointer.Data + _FracturePropertiesOffset)); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(IntPtr*)(pointer.Data + _FracturePropertiesOffset) = value.Instance; }
        }

        public NaturalFractureProperties<T> NaturalFractureProperties
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return new NaturalFractureProperties<T>(*(IntPtr*)(pointer.Data + _NaturalFracturePropertiesOffset)); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(IntPtr*)(pointer.Data + _NaturalFracturePropertiesOffset) = value.Instance; }
        }

        public Pvt<T> Pvt
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return new Pvt<T>(*(IntPtr*)(pointer.Data + _PvtOffset)); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(IntPtr*)(pointer.Data + _PvtOffset) = value.Instance; }
        }
        
        public NativePointer Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return pointer; }
        }

        public MultiPorosityData()
        {
            pointer = NativePointer.Allocate(ThisSize, ExecutionSpaceKind.Cuda);
        }

        public MultiPorosityData(ProductionData<T> productionData,
                                 ReservoirProperties<T> reservoirProperties,
                                 WellProperties<T> wellProperties,
                                 FractureProperties<T> fractureProperties,
                                 NaturalFractureProperties<T> naturalFractureProperties,
                                 Pvt<T> pvt)
        {
            pointer = NativePointer.Allocate(ThisSize, ExecutionSpaceKind.Cuda);

            ProductionData = productionData;
            ReservoirProperties = reservoirProperties;
            WellProperties = wellProperties;
            FractureProperties = fractureProperties;
            NaturalFractureProperties = naturalFractureProperties;
            Pvt = pvt;
        }

        internal MultiPorosityData(NativePointer nativePointer)
        {
            pointer = nativePointer;
        }
    }
}