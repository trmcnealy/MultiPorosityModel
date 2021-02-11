
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Kokkos;

namespace MultiPorosity.Models
{
    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public sealed unsafe partial class Pvt<T> : IDisposable
        where T : unmanaged
    {
        private static readonly Type _T = typeof(T);
        public static readonly int ThisSize;
        
        private static readonly int _oilSaturationOffset;
        private static readonly int _oilApiGravityOffset;
        private static readonly int _oilViscosityOffset;
        private static readonly int _oilFormationVolumeFactorOffset;
        private static readonly int _oilCompressibilityOffset;
        private static readonly int _waterSaturationOffset;
        private static readonly int _waterSpecificGravityOffset;
        private static readonly int _waterViscosityOffset;
        private static readonly int _waterFormationVolumeFactorOffset;
        private static readonly int _waterCompressibilityOffset;
        private static readonly int _gasSaturationOffset;
        private static readonly int _gasSpecificGravityOffset;
        private static readonly int _gasViscosityOffset;
        private static readonly int _gasFormationVolumeFactorOffset;
        private static readonly int _gasCompressibilityFactorOffset;
        private static readonly int _gasCompressibilityOffset;
        
        static Pvt()
        {
            _oilSaturationOffset  = Marshal.OffsetOf<Pvt<T>>(nameof(_oilSaturation)).ToInt32();
            _oilApiGravityOffset  = Marshal.OffsetOf<Pvt<T>>(nameof(_oilApiGravity)).ToInt32();
            _oilViscosityOffset  = Marshal.OffsetOf<Pvt<T>>(nameof(_oilViscosity)).ToInt32();
            _oilFormationVolumeFactorOffset  = Marshal.OffsetOf<Pvt<T>>(nameof(_oilFormationVolumeFactor)).ToInt32();
            _oilCompressibilityOffset  = Marshal.OffsetOf<Pvt<T>>(nameof(_oilCompressibility)).ToInt32();
            _waterSaturationOffset  = Marshal.OffsetOf<Pvt<T>>(nameof(_waterSaturation)).ToInt32();
            _waterSpecificGravityOffset  = Marshal.OffsetOf<Pvt<T>>(nameof(_waterSpecificGravity)).ToInt32();
            _waterViscosityOffset  = Marshal.OffsetOf<Pvt<T>>(nameof(_waterViscosity)).ToInt32();
            _waterFormationVolumeFactorOffset  = Marshal.OffsetOf<Pvt<T>>(nameof(_waterFormationVolumeFactor)).ToInt32();
            _waterCompressibilityOffset  = Marshal.OffsetOf<Pvt<T>>(nameof(_waterCompressibility)).ToInt32();
            _gasSaturationOffset  = Marshal.OffsetOf<Pvt<T>>(nameof(_gasSaturation)).ToInt32();
            _gasSpecificGravityOffset  = Marshal.OffsetOf<Pvt<T>>(nameof(_gasSpecificGravity)).ToInt32();
            _gasViscosityOffset  = Marshal.OffsetOf<Pvt<T>>(nameof(_gasViscosity)).ToInt32();
            _gasFormationVolumeFactorOffset  = Marshal.OffsetOf<Pvt<T>>(nameof(_gasFormationVolumeFactor)).ToInt32();
            _gasCompressibilityFactorOffset  = Marshal.OffsetOf<Pvt<T>>(nameof(_gasCompressibilityFactor)).ToInt32();
            _gasCompressibilityOffset  = Marshal.OffsetOf<Pvt<T>>(nameof(_gasCompressibility)).ToInt32();
            ThisSize = _gasCompressibilityOffset + Unsafe.SizeOf<T>();
        }
        
        private T _oilSaturation;
        private T _oilApiGravity;
        private T _oilViscosity;
        private T _oilFormationVolumeFactor;
        private T _oilCompressibility;
        private T _waterSaturation;
        private T _waterSpecificGravity;
        private T _waterViscosity;
        private T _waterFormationVolumeFactor;
        private T _waterCompressibility;
        private T _gasSaturation;
        private T _gasSpecificGravity;
        private T _gasViscosity;
        private T _gasFormationVolumeFactor;
        private T _gasCompressibilityFactor;
        private T _gasCompressibility;
        
        private readonly NativePointer pointer;
        
        public T OilSaturation
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _oilSaturationOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _oilSaturationOffset) = value; }
        }
        
        public T OilApiGravity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _oilApiGravityOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _oilApiGravityOffset) = value; }
        }
        
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
        
        public T OilCompressibility
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _oilCompressibilityOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _oilCompressibilityOffset) = value; }
        }
        
        public T WaterSaturation
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _waterSaturationOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _waterSaturationOffset) = value; }
        }
        
        public T WaterSpecificGravity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _waterSpecificGravityOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _waterSpecificGravityOffset) = value; }
        }
        
        public T WaterViscosity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _waterViscosityOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _waterViscosityOffset) = value; }
        }
        
        public T WaterFormationVolumeFactor
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _waterFormationVolumeFactorOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _waterFormationVolumeFactorOffset) = value; }
        }
        
        public T WaterCompressibility
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _waterCompressibilityOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _waterCompressibilityOffset) = value; }
        }
        
        public T GasSaturation
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _gasSaturationOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _gasSaturationOffset) = value; }
        }
        
        public T GasSpecificGravity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _gasSpecificGravityOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _gasSpecificGravityOffset) = value; }
        }
        
        public T GasViscosity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _gasViscosityOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _gasViscosityOffset) = value; }
        }
        
        public T GasFormationVolumeFactor
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _gasFormationVolumeFactorOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _gasFormationVolumeFactorOffset) = value; }
        }
        
        public T GasCompressibilityFactor
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _gasCompressibilityFactorOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _gasCompressibilityFactorOffset) = value; }
        }
        
        public T GasCompressibility
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _gasCompressibilityOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _gasCompressibilityOffset) = value; }
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
        
        ~Pvt()
        {
        }
        public void Dispose()
        {
            pointer.Dispose();
            GC.SuppressFinalize(this);
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