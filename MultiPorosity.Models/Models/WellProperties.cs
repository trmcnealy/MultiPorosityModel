using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Kokkos;

namespace MultiPorosity.Models
{
    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public sealed unsafe partial class WellProperties<T> : IDisposable
        where T : unmanaged
    {
        private static readonly Type _T = typeof(T);
        public static readonly int ThisSize;
        
        private static readonly int _aPIOffset;
        private static readonly int _lateralLengthOffset;
        private static readonly int _bottomholePressureOffset;
        
        static WellProperties()
        {
            _aPIOffset  = Marshal.OffsetOf<WellProperties<T>>(nameof(_aPI)).ToInt32();
            _lateralLengthOffset  = Marshal.OffsetOf<WellProperties<T>>(nameof(_lateralLength)).ToInt32();
            _bottomholePressureOffset  = Marshal.OffsetOf<WellProperties<T>>(nameof(_bottomholePressure)).ToInt32();
            ThisSize = _bottomholePressureOffset + Unsafe.SizeOf<T>();
        }
        
        private ulong _aPI;
        private T _lateralLength;
        private T _bottomholePressure;
        
        private readonly NativePointer pointer;
        
        public ulong API
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(ulong*)(pointer.Data + _aPIOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(ulong*)(pointer.Data + _aPIOffset) = value; }
        }
        
        public T LateralLength
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _lateralLengthOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _lateralLengthOffset) = value; }
        }
        
        public T BottomholePressure
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _bottomholePressureOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _bottomholePressureOffset) = value; }
        }
        
        public NativePointer Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return pointer; }
        }
        
        public WellProperties(ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda)
        {
            pointer = NativePointer.Allocate(ThisSize, executionSpace);
        }
        
        public void Dispose()
        {
            pointer.Dispose();
        }
        
        internal WellProperties(IntPtr intPtr, ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda)
        {
            pointer = new NativePointer(intPtr, ThisSize, false, executionSpace);
        }
        
        internal WellProperties(WellProperties<T> copy, ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda)
        {
            pointer = new NativePointer(copy.Instance, executionSpace);
        }
        
        public static implicit operator WellProperties<T>(IntPtr intPtr)
        {
            return new WellProperties<T>(intPtr);
        }
    }
}
