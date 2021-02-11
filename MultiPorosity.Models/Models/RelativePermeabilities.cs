
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Kokkos;

namespace MultiPorosity.Models
{
    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public sealed unsafe partial class RelativePermeabilities<T> : IDisposable
        where T : unmanaged
    {
        private static readonly Type _T = typeof(T);
        public static readonly int ThisSize;
        
        private static readonly int _matrixOilOffset;
        private static readonly int _matrixWaterOffset;
        private static readonly int _matrixGasOffset;
        private static readonly int _fractureOilOffset;
        private static readonly int _fractureWaterOffset;
        private static readonly int _fractureGasOffset;
        private static readonly int _naturalFractureOilOffset;
        private static readonly int _naturalFractureWaterOffset;
        private static readonly int _naturalFractureGasOffset;
        
        static RelativePermeabilities()
        {
            _matrixOilOffset  = Marshal.OffsetOf<RelativePermeabilities<T>>(nameof(_matrixOil)).ToInt32();
            _matrixWaterOffset  = Marshal.OffsetOf<RelativePermeabilities<T>>(nameof(_matrixWater)).ToInt32();
            _matrixGasOffset  = Marshal.OffsetOf<RelativePermeabilities<T>>(nameof(_matrixGas)).ToInt32();
            _fractureOilOffset  = Marshal.OffsetOf<RelativePermeabilities<T>>(nameof(_fractureOil)).ToInt32();
            _fractureWaterOffset  = Marshal.OffsetOf<RelativePermeabilities<T>>(nameof(_fractureWater)).ToInt32();
            _fractureGasOffset  = Marshal.OffsetOf<RelativePermeabilities<T>>(nameof(_fractureGas)).ToInt32();
            _naturalFractureOilOffset  = Marshal.OffsetOf<RelativePermeabilities<T>>(nameof(_naturalFractureOil)).ToInt32();
            _naturalFractureWaterOffset  = Marshal.OffsetOf<RelativePermeabilities<T>>(nameof(_naturalFractureWater)).ToInt32();
            _naturalFractureGasOffset  = Marshal.OffsetOf<RelativePermeabilities<T>>(nameof(_naturalFractureGas)).ToInt32();
            ThisSize = _naturalFractureGasOffset + Unsafe.SizeOf<T>();
        }
        
        private T _matrixOil;
        private T _matrixWater;
        private T _matrixGas;
        private T _fractureOil;
        private T _fractureWater;
        private T _fractureGas;
        private T _naturalFractureOil;
        private T _naturalFractureWater;
        private T _naturalFractureGas;
        
        private readonly NativePointer pointer;
        
        public T MatrixOil
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _matrixOilOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _matrixOilOffset) = value; }
        }
        
        public T MatrixWater
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _matrixWaterOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _matrixWaterOffset) = value; }
        }
        
        public T MatrixGas
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _matrixGasOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _matrixGasOffset) = value; }
        }
        
        public T FractureOil
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _fractureOilOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _fractureOilOffset) = value; }
        }
        
        public T FractureWater
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _fractureWaterOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _fractureWaterOffset) = value; }
        }
        
        public T FractureGas
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _fractureGasOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _fractureGasOffset) = value; }
        }
        
        public T NaturalFractureOil
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _naturalFractureOilOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _naturalFractureOilOffset) = value; }
        }
        
        public T NaturalFractureWater
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _naturalFractureWaterOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _naturalFractureWaterOffset) = value; }
        }
        
        public T NaturalFractureGas
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _naturalFractureGasOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _naturalFractureGasOffset) = value; }
        }
        
        public NativePointer Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return pointer; }
        }
        
        public RelativePermeabilities(ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda)
        {
            pointer = NativePointer.Allocate(ThisSize, executionSpace);
        }
        
        ~RelativePermeabilities()
        {
        }
        public void Dispose()
        {
            pointer.Dispose();
            GC.SuppressFinalize(this);
        }
        
        internal RelativePermeabilities(IntPtr intPtr, ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda)
        {
            pointer = new NativePointer(intPtr, ThisSize, false, executionSpace);
        }
        
        public static implicit operator RelativePermeabilities<T>(IntPtr intPtr)
        {
            return new RelativePermeabilities<T>(intPtr);
        }
    }
}