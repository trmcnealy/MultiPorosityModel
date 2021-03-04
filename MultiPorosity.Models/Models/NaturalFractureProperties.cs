

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Kokkos;

namespace MultiPorosity.Models
{
    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public sealed unsafe partial class NaturalFractureProperties<T> : IDisposable
        where T : unmanaged
    {
        private static readonly Type _T = typeof(T);
        public static readonly int ThisSize;
        
        private static readonly int _countOffset;
        private static readonly int _widthOffset;
        private static readonly int _porosityOffset;
        private static readonly int _permeabilityOffset;
        
        static NaturalFractureProperties()
        {
            _countOffset  = Marshal.OffsetOf<NaturalFractureProperties<T>>(nameof(_count)).ToInt32();
            _widthOffset  = Marshal.OffsetOf<NaturalFractureProperties<T>>(nameof(_width)).ToInt32();
            _porosityOffset  = Marshal.OffsetOf<NaturalFractureProperties<T>>(nameof(_porosity)).ToInt32();
            _permeabilityOffset  = Marshal.OffsetOf<NaturalFractureProperties<T>>(nameof(_permeability)).ToInt32();
            ThisSize = _permeabilityOffset + Unsafe.SizeOf<T>();
        }
        
        private int _count;
        private T _width;
        private T _porosity;
        private T _permeability;
        
        private readonly NativePointer pointer;
        
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(int*)(pointer.Data + _countOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(int*)(pointer.Data + _countOffset) = value; }
        }
        
        public T Width
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _widthOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _widthOffset) = value; }
        }
        
        public T Porosity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _porosityOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _porosityOffset) = value; }
        }
        
        public T Permeability
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _permeabilityOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _permeabilityOffset) = value; }
        }
        
        public NativePointer Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return pointer; }
        }
        
        public NaturalFractureProperties(ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda)
        {
            pointer = NativePointer.Allocate(ThisSize, executionSpace);
        }
        
        public void Dispose()
        {
            pointer.Dispose();
        }
        
        internal NaturalFractureProperties(IntPtr intPtr, ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda)
        {
            pointer = new NativePointer(intPtr, ThisSize, false, executionSpace);
        }
        
        internal NaturalFractureProperties(NaturalFractureProperties<T> copy, ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda)
        {
            pointer = new NativePointer(copy.Instance, executionSpace);
        }
        
        public static implicit operator NaturalFractureProperties<T>(IntPtr intPtr)
        {
            return new NaturalFractureProperties<T>(intPtr);
        }
    }
}