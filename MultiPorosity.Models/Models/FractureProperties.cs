
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Kokkos;

namespace MultiPorosity.Models
{
    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public sealed unsafe class FractureProperties<T>
        where T : unmanaged
    {
        private static readonly Type _T = typeof(T);
        public static readonly int ThisSize;
        
        private static readonly int _countOffset;
        private static readonly int _widthOffset;
        private static readonly int _heightOffset;
        private static readonly int _halfLengthOffset;
        private static readonly int _porosityOffset;
        private static readonly int _permeabilityOffset;
        private static readonly int _skinOffset;
        
        static FractureProperties()
        {
            _countOffset  = Marshal.OffsetOf<FractureProperties<T>>(nameof(_count)).ToInt32();
            _widthOffset  = Marshal.OffsetOf<FractureProperties<T>>(nameof(_width)).ToInt32();
            _heightOffset  = Marshal.OffsetOf<FractureProperties<T>>(nameof(_height)).ToInt32();
            _halfLengthOffset  = Marshal.OffsetOf<FractureProperties<T>>(nameof(_halfLength)).ToInt32();
            _porosityOffset  = Marshal.OffsetOf<FractureProperties<T>>(nameof(_porosity)).ToInt32();
            _permeabilityOffset  = Marshal.OffsetOf<FractureProperties<T>>(nameof(_permeability)).ToInt32();
            _skinOffset  = Marshal.OffsetOf<FractureProperties<T>>(nameof(_skin)).ToInt32();
            ThisSize = _skinOffset + Unsafe.SizeOf<T>();
        }
        
        private int _count;
        private T _width;
        private T _height;
        private T _halfLength;
        private T _porosity;
        private T _permeability;
        private T _skin;
        
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
        
        public T Height
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _heightOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _heightOffset) = value; }
        }
        
        public T HalfLength
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _halfLengthOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _halfLengthOffset) = value; }
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
        
        public T Skin
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _skinOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _skinOffset) = value; }
        }
        
        public NativePointer Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return pointer; }
        }
        
        public FractureProperties(ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda)
        {
            pointer = NativePointer.Allocate(ThisSize, executionSpace);
        }
        
        internal FractureProperties(IntPtr intPtr, ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda)
        {
            pointer = new NativePointer(intPtr, ThisSize, false, executionSpace);
        }
        
        public static implicit operator FractureProperties<T>(IntPtr intPtr)
        {
            return new FractureProperties<T>(intPtr);
        }
    }
}