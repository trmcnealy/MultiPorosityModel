
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Kokkos;

namespace MultiPorosity.Models
{
    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public sealed unsafe class Pvt<T>
        where T : unmanaged
    {
        private static readonly Type _T = typeof(T);
        public static readonly int ThisSize;
        
        private static readonly int _oilViscosityOffset;
        private static readonly int _oilFormationVolumeFactorOffset;
        private static readonly int _totalCompressibilityOffset;
        
        static Pvt()
        {
            _oilViscosityOffset  = Marshal.OffsetOf<Pvt<T>>(nameof(_oilViscosity)).ToInt32();
            _oilFormationVolumeFactorOffset  = Marshal.OffsetOf<Pvt<T>>(nameof(_oilFormationVolumeFactor)).ToInt32();
            _totalCompressibilityOffset  = Marshal.OffsetOf<Pvt<T>>(nameof(_totalCompressibility)).ToInt32();
            ThisSize = _totalCompressibilityOffset + Unsafe.SizeOf<T>();
        }
        
        private T _oilViscosity;
        private T _oilFormationVolumeFactor;
        private T _totalCompressibility;
        
        private readonly NativePointer pointer;
        
        public T OilViscosity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _oilViscosityOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _oilViscosityOffset) = value; }
        }
        
        public T OilFormationVolumeFactor
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _oilFormationVolumeFactorOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _oilFormationVolumeFactorOffset) = value; }
        }
        
        public T TotalCompressibility
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _totalCompressibilityOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _totalCompressibilityOffset) = value; }
        }
        
        public NativePointer Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return pointer; }
        }
        
        public Pvt(ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda)
        {
            pointer = NativePointer.Allocate(ThisSize, executionSpace);
        }
        
        internal Pvt(IntPtr intPtr, ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda)
        {
            pointer = new NativePointer(intPtr, ThisSize, false, executionSpace);
        }
        
        public static implicit operator Pvt<T>(IntPtr intPtr)
        {
            return new Pvt<T>(intPtr);
        }
    }
}