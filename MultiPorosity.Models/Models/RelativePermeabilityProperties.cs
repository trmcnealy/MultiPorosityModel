using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

using Kokkos;

#pragma warning disable IDE0044
namespace MultiPorosity.Models
{
    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public sealed unsafe partial class RelativePermeabilityProperties<T> : IDisposable
        where T : unmanaged
    {
        private static readonly Type _T = typeof(T);
        public static readonly int ThisSize;
        
        private static readonly int _saturationWaterConnateOffset;
        private static readonly int _saturationWaterCriticalOffset;
        private static readonly int _saturationOilIrreducibleWaterOffset;
        private static readonly int _saturationOilResidualWaterOffset;
        private static readonly int _saturationOilIrreducibleGasOffset;
        private static readonly int _saturationOilResidualGasOffset;
        private static readonly int _saturationGasConnateOffset;
        private static readonly int _saturationGasCriticalOffset;
        private static readonly int _permeabilityRelativeWaterOilIrreducibleOffset;
        private static readonly int _permeabilityRelativeOilWaterConnateOffset;
        private static readonly int _permeabilityRelativeGasLiquidConnateOffset;
        private static readonly int _exponentPermeabilityRelativeWaterOffset;
        private static readonly int _exponentPermeabilityRelativeOilWaterOffset;
        private static readonly int _exponentPermeabilityRelativeGasOffset;
        private static readonly int _exponentPermeabilityRelativeOilGasOffset;
        
        static RelativePermeabilityProperties()
        {
            _saturationWaterConnateOffset  = Marshal.OffsetOf<RelativePermeabilityProperties<T>>(nameof(_saturationWaterConnate)).ToInt32();
            _saturationWaterCriticalOffset  = Marshal.OffsetOf<RelativePermeabilityProperties<T>>(nameof(_saturationWaterCritical)).ToInt32();
            _saturationOilIrreducibleWaterOffset  = Marshal.OffsetOf<RelativePermeabilityProperties<T>>(nameof(_saturationOilIrreducibleWater)).ToInt32();
            _saturationOilResidualWaterOffset  = Marshal.OffsetOf<RelativePermeabilityProperties<T>>(nameof(_saturationOilResidualWater)).ToInt32();
            _saturationOilIrreducibleGasOffset  = Marshal.OffsetOf<RelativePermeabilityProperties<T>>(nameof(_saturationOilIrreducibleGas)).ToInt32();
            _saturationOilResidualGasOffset  = Marshal.OffsetOf<RelativePermeabilityProperties<T>>(nameof(_saturationOilResidualGas)).ToInt32();
            _saturationGasConnateOffset  = Marshal.OffsetOf<RelativePermeabilityProperties<T>>(nameof(_saturationGasConnate)).ToInt32();
            _saturationGasCriticalOffset  = Marshal.OffsetOf<RelativePermeabilityProperties<T>>(nameof(_saturationGasCritical)).ToInt32();
            _permeabilityRelativeWaterOilIrreducibleOffset  = Marshal.OffsetOf<RelativePermeabilityProperties<T>>(nameof(_permeabilityRelativeWaterOilIrreducible)).ToInt32();
            _permeabilityRelativeOilWaterConnateOffset  = Marshal.OffsetOf<RelativePermeabilityProperties<T>>(nameof(_permeabilityRelativeOilWaterConnate)).ToInt32();
            _permeabilityRelativeGasLiquidConnateOffset  = Marshal.OffsetOf<RelativePermeabilityProperties<T>>(nameof(_permeabilityRelativeGasLiquidConnate)).ToInt32();
            _exponentPermeabilityRelativeWaterOffset  = Marshal.OffsetOf<RelativePermeabilityProperties<T>>(nameof(_exponentPermeabilityRelativeWater)).ToInt32();
            _exponentPermeabilityRelativeOilWaterOffset  = Marshal.OffsetOf<RelativePermeabilityProperties<T>>(nameof(_exponentPermeabilityRelativeOilWater)).ToInt32();
            _exponentPermeabilityRelativeGasOffset  = Marshal.OffsetOf<RelativePermeabilityProperties<T>>(nameof(_exponentPermeabilityRelativeGas)).ToInt32();
            _exponentPermeabilityRelativeOilGasOffset  = Marshal.OffsetOf<RelativePermeabilityProperties<T>>(nameof(_exponentPermeabilityRelativeOilGas)).ToInt32();
            ThisSize = _exponentPermeabilityRelativeOilGasOffset + Unsafe.SizeOf<T>();
        }
        
        private T _saturationWaterConnate;
        private T _saturationWaterCritical;
        private T _saturationOilIrreducibleWater;
        private T _saturationOilResidualWater;
        private T _saturationOilIrreducibleGas;
        private T _saturationOilResidualGas;
        private T _saturationGasConnate;
        private T _saturationGasCritical;
        private T _permeabilityRelativeWaterOilIrreducible;
        private T _permeabilityRelativeOilWaterConnate;
        private T _permeabilityRelativeGasLiquidConnate;
        private T _exponentPermeabilityRelativeWater;
        private T _exponentPermeabilityRelativeOilWater;
        private T _exponentPermeabilityRelativeGas;
        private T _exponentPermeabilityRelativeOilGas;
        
        private readonly NativePointer pointer;
        
        public T SaturationWaterConnate
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _saturationWaterConnateOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _saturationWaterConnateOffset) = value; }
        }
        
        public T SaturationWaterCritical
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _saturationWaterCriticalOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _saturationWaterCriticalOffset) = value; }
        }
        
        public T SaturationOilIrreducibleWater
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _saturationOilIrreducibleWaterOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _saturationOilIrreducibleWaterOffset) = value; }
        }
        
        public T SaturationOilResidualWater
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _saturationOilResidualWaterOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _saturationOilResidualWaterOffset) = value; }
        }
        
        public T SaturationOilIrreducibleGas
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _saturationOilIrreducibleGasOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _saturationOilIrreducibleGasOffset) = value; }
        }
        
        public T SaturationOilResidualGas
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _saturationOilResidualGasOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _saturationOilResidualGasOffset) = value; }
        }
        
        public T SaturationGasConnate
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _saturationGasConnateOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _saturationGasConnateOffset) = value; }
        }
        
        public T SaturationGasCritical
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _saturationGasCriticalOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _saturationGasCriticalOffset) = value; }
        }
        
        public T PermeabilityRelativeWaterOilIrreducible
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _permeabilityRelativeWaterOilIrreducibleOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _permeabilityRelativeWaterOilIrreducibleOffset) = value; }
        }
        
        public T PermeabilityRelativeOilWaterConnate
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _permeabilityRelativeOilWaterConnateOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _permeabilityRelativeOilWaterConnateOffset) = value; }
        }
        
        public T PermeabilityRelativeGasLiquidConnate
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _permeabilityRelativeGasLiquidConnateOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _permeabilityRelativeGasLiquidConnateOffset) = value; }
        }
        
        public T ExponentPermeabilityRelativeWater
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _exponentPermeabilityRelativeWaterOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _exponentPermeabilityRelativeWaterOffset) = value; }
        }
        
        public T ExponentPermeabilityRelativeOilWater
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _exponentPermeabilityRelativeOilWaterOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _exponentPermeabilityRelativeOilWaterOffset) = value; }
        }
        
        public T ExponentPermeabilityRelativeGas
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _exponentPermeabilityRelativeGasOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _exponentPermeabilityRelativeGasOffset) = value; }
        }
        
        public T ExponentPermeabilityRelativeOilGas
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _exponentPermeabilityRelativeOilGasOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _exponentPermeabilityRelativeOilGasOffset) = value; }
        }
        
        public NativePointer Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return pointer; }
        }
        
        public RelativePermeabilityProperties(ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda)
        {
            pointer = NativePointer.Allocate(ThisSize, executionSpace);
        }
        
        public void Dispose()
        {
            pointer.Dispose();
        }
        
        internal RelativePermeabilityProperties(IntPtr intPtr, ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda)
        {
            pointer = new NativePointer(intPtr, ThisSize, false, executionSpace);
        }
        
        internal RelativePermeabilityProperties(RelativePermeabilityProperties<T> copy, ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda)
        {
            pointer = new NativePointer(copy.Instance, executionSpace);
        }
        
        public static implicit operator RelativePermeabilityProperties<T>(IntPtr intPtr)
        {
            return new RelativePermeabilityProperties<T>(intPtr);
        }
    }
}
#pragma warning restore IDE0044