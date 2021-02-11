
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Kokkos;

namespace MultiPorosity.Models
{
    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public sealed unsafe partial class ReservoirProperties<T> : IDisposable
        where T : unmanaged
    {
        private static readonly Type _T = typeof(T);
        public static readonly int ThisSize;
        
        private static readonly int _lengthOffset;
        private static readonly int _widthOffset;
        private static readonly int _thicknessOffset;
        private static readonly int _porosityOffset;
        private static readonly int _permeabilityOffset;
        private static readonly int _compressibilityOffset;
        private static readonly int _bottomholeTemperatureOffset;
        private static readonly int _initialPressureOffset;
        
        static ReservoirProperties()
        {
            _lengthOffset  = Marshal.OffsetOf<ReservoirProperties<T>>(nameof(_length)).ToInt32();
            _widthOffset  = Marshal.OffsetOf<ReservoirProperties<T>>(nameof(_width)).ToInt32();
            _thicknessOffset  = Marshal.OffsetOf<ReservoirProperties<T>>(nameof(_thickness)).ToInt32();
            _porosityOffset  = Marshal.OffsetOf<ReservoirProperties<T>>(nameof(_porosity)).ToInt32();
            _permeabilityOffset  = Marshal.OffsetOf<ReservoirProperties<T>>(nameof(_permeability)).ToInt32();
            _compressibilityOffset  = Marshal.OffsetOf<ReservoirProperties<T>>(nameof(_compressibility)).ToInt32();
            _bottomholeTemperatureOffset  = Marshal.OffsetOf<ReservoirProperties<T>>(nameof(_bottomholeTemperature)).ToInt32();
            _initialPressureOffset  = Marshal.OffsetOf<ReservoirProperties<T>>(nameof(_initialPressure)).ToInt32();
            ThisSize = _initialPressureOffset + Unsafe.SizeOf<T>();
        }
        
        private T _length;
        private T _width;
        private T _thickness;
        private T _porosity;
        private T _permeability;
        private T _compressibility;
        private T _bottomholeTemperature;
        private T _initialPressure;
        
        private readonly NativePointer pointer;
        
        public T Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _lengthOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _lengthOffset) = value; }
        }
        
        public T Width
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _widthOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _widthOffset) = value; }
        }
        
        public T Thickness
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _thicknessOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _thicknessOffset) = value; }
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
        
        public T Compressibility
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _compressibilityOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _compressibilityOffset) = value; }
        }
        
        public T BottomholeTemperature
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _bottomholeTemperatureOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _bottomholeTemperatureOffset) = value; }
        }
        
        public T InitialPressure
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _initialPressureOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _initialPressureOffset) = value; }
        }
        
        public NativePointer Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return pointer; }
        }
        
        public ReservoirProperties(ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda)
        {
            pointer = NativePointer.Allocate(ThisSize, executionSpace);
        }
        
        ~ReservoirProperties()
        {
        }
        public void Dispose()
        {
            pointer.Dispose();
            GC.SuppressFinalize(this);
        }
        
        internal ReservoirProperties(IntPtr intPtr, ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda)
        {
            pointer = new NativePointer(intPtr, ThisSize, false, executionSpace);
        }
        
        public static implicit operator ReservoirProperties<T>(IntPtr intPtr)
        {
            return new ReservoirProperties<T>(intPtr);
        }
    }
}