
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Kokkos;

namespace MultiPorosity.Models
{
    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public sealed unsafe partial class ProductionDataRecord<T> : IDisposable
        where T : unmanaged
    {
        private static readonly Type _T = typeof(T);
        public static readonly int ThisSize;
        
        private static readonly int _timeOffset;
        private static readonly int _qoOffset;
        private static readonly int _qwOffset;
        private static readonly int _qGOffset;
        private static readonly int _qgBoeOffset;
        private static readonly int _qtOffset;
        
        static ProductionDataRecord()
        {
            _timeOffset  = Marshal.OffsetOf<ProductionDataRecord<T>>(nameof(_time)).ToInt32();
            _qoOffset  = Marshal.OffsetOf<ProductionDataRecord<T>>(nameof(_qo)).ToInt32();
            _qwOffset  = Marshal.OffsetOf<ProductionDataRecord<T>>(nameof(_qw)).ToInt32();
            _qGOffset  = Marshal.OffsetOf<ProductionDataRecord<T>>(nameof(_qG)).ToInt32();
            _qgBoeOffset  = Marshal.OffsetOf<ProductionDataRecord<T>>(nameof(_qgBoe)).ToInt32();
            _qtOffset  = Marshal.OffsetOf<ProductionDataRecord<T>>(nameof(_qt)).ToInt32();
            ThisSize = _qtOffset + Unsafe.SizeOf<T>();
        }
        
        private T _time;
        private T _qo;
        private T _qw;
        private T _qG;
        private T _qgBoe;
        private T _qt;
        
        private readonly NativePointer pointer;
        
        public T Time
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _timeOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _timeOffset) = value; }
        }
        
        public T Qo
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _qoOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _qoOffset) = value; }
        }
        
        public T Qw
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _qwOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _qwOffset) = value; }
        }
        
        public T Qg
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _qGOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _qGOffset) = value; }
        }
        
        public T QgBoe
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _qgBoeOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _qgBoeOffset) = value; }
        }
        
        public T Qt
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(T*)(pointer.Data + _qtOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { *(T*)(pointer.Data + _qtOffset) = value; }
        }
        
        public NativePointer Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return pointer; }
        }
        
        public ProductionDataRecord(ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda)
        {
            pointer = NativePointer.Allocate(ThisSize, executionSpace);
        }
        
        ~ProductionDataRecord()
        {
        }
        public void Dispose()
        {
            pointer.Dispose();
            GC.SuppressFinalize(this);
        }
        
        internal ProductionDataRecord(IntPtr intPtr, ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda)
        {
            pointer = new NativePointer(intPtr, ThisSize, false, executionSpace);
        }
        
        public static implicit operator ProductionDataRecord<T>(IntPtr intPtr)
        {
            return new ProductionDataRecord<T>(intPtr);
        }
    }
}