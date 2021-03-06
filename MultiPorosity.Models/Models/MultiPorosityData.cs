﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Kokkos;

namespace MultiPorosity.Models
{
    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public sealed unsafe class MultiPorosityData<T>
        where T : unmanaged
    {
        private static readonly Type               _T = typeof(T);
        public static readonly  int                ThisSize;
        
        private static readonly int _ReservoirPropertiesOffset;
        
        private static readonly int _WellPropertiesOffset;
        
        private static readonly int _FracturePropertiesOffset;
        
        private static readonly int _NaturalFracturePropertiesOffset;
        
        private static readonly int _PvtOffset;
        
        private static readonly int _RelativePermeabilitiesOffset;

        static MultiPorosityData()
        {
            _ReservoirPropertiesOffset       = Marshal.OffsetOf<MultiPorosityData<T>>(nameof(_ReservoirProperties)).ToInt32();
            _WellPropertiesOffset            = Marshal.OffsetOf<MultiPorosityData<T>>(nameof(_WellProperties)).ToInt32();
            _FracturePropertiesOffset        = Marshal.OffsetOf<MultiPorosityData<T>>(nameof(_FractureProperties)).ToInt32();
            _NaturalFracturePropertiesOffset = Marshal.OffsetOf<MultiPorosityData<T>>(nameof(_NaturalFractureProperties)).ToInt32();
            _PvtOffset                       = Marshal.OffsetOf<MultiPorosityData<T>>(nameof(_Pvt)).ToInt32();
            _RelativePermeabilitiesOffset    = Marshal.OffsetOf<MultiPorosityData<T>>(nameof(_RelativePermeability)).ToInt32();
            ThisSize                         = _RelativePermeabilitiesOffset + Unsafe.SizeOf<IntPtr>();
        }
        
        private IntPtr _ReservoirProperties;
        
        private IntPtr _WellProperties;
        
        private IntPtr _FractureProperties;
        
        private IntPtr _NaturalFractureProperties;
        
        private IntPtr _Pvt;
        
        private IntPtr _RelativePermeability;
        
        private readonly NativePointer pointer;
        
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

        public RelativePermeabilities<T> RelativePermeability
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return new RelativePermeabilities<T>(*(IntPtr*)(pointer.Data + _RelativePermeabilitiesOffset)); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(IntPtr*)(pointer.Data + _RelativePermeabilitiesOffset) = value.Instance; }
        }
        
        public NativePointer Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return pointer; }
        }

        public MultiPorosityData(ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda)
        {
            pointer = NativePointer.Allocate(ThisSize, executionSpace);
        }

        public MultiPorosityData(ReservoirProperties <T >          reservoirProperties,
                                 WellProperties<T>                 wellProperties,
                                 FractureProperties<T>             fractureProperties,
                                 NaturalFractureProperties<T>      naturalFractureProperties,
                                 Pvt<T>                            pvt,
                                 RelativePermeabilities<T> relativePermeabilities,
                                 ExecutionSpaceKind                executionSpace = ExecutionSpaceKind.Cuda)
        {
            pointer = NativePointer.Allocate(ThisSize, executionSpace);

            ReservoirProperties       = reservoirProperties;
            WellProperties            = wellProperties;
            FractureProperties        = fractureProperties;
            NaturalFractureProperties = naturalFractureProperties;
            Pvt                       = pvt;
            RelativePermeability      = relativePermeabilities;
        }

        internal MultiPorosityData(NativePointer nativePointer)
        {
            pointer = nativePointer;
        }
    }
}