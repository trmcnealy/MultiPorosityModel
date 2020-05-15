using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Kokkos;

namespace MultiPorosity.Models
{
    [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public sealed unsafe class ProductionData<T>
        where T : unmanaged
    {
        private static readonly Type _T = typeof(T);
        public static readonly  int  ThisSize;

        private static readonly int _countOffset;
        private static readonly int _recordsOffset;

        static ProductionData()
        {
            _countOffset   = Marshal.OffsetOf<ProductionData<T>>(nameof(_count)).ToInt32();
            _recordsOffset = Marshal.OffsetOf<ProductionData<T>>(nameof(_records)).ToInt32();
            ThisSize       = _recordsOffset + Unsafe.SizeOf<IntPtr>();
        }

        private int                _count;
        private IntPtr             _records;
        private ExecutionSpaceKind _executionSpace;

        private readonly NativePointer _pointer;
        private readonly NativePointer Records;

        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return *(int*)(_pointer.Data + _countOffset); }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            private set { *(int*)(_pointer.Data + _countOffset) = value; }
        }

        //private IntPtr Records
        //{
        //    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //    get { return *(IntPtr*)(_pointer.Data + _recordsOffset); }
        //    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //    set { *(IntPtr*)(_pointer.Data + _recordsOffset) = value; }
        //}

        public NativePointer Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return _pointer; }
        }

        public ProductionData(int                size,
                              ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda)
        {
            _executionSpace = executionSpace;

            _pointer = NativePointer.Allocate(ThisSize,
                                              executionSpace);

            Count = size;

            Records = NativePointer.Allocate(ProductionDataRecord<T>.ThisSize * size,
                                             executionSpace);
        }

        internal ProductionData(IntPtr             intPtr,
                                ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda)
        {
            _pointer = new NativePointer(intPtr,
                                         ThisSize,
                                         false,
                                         executionSpace);
        }

        public ProductionDataRecord<T> this[int index]
        {
            get
            {
                return new ProductionDataRecord<T>(Records.Data + ProductionDataRecord<T>.ThisSize * index,
                                                   _executionSpace);
            }
            set
            {
                ProductionDataRecord<T> productionDataRecord = new ProductionDataRecord<T>(Records.Data + ProductionDataRecord<T>.ThisSize * index,
                                                                                           _executionSpace)
                {
                    Time  = value.Time,
                    Qo    = value.Qo,
                    Qw    = value.Qw,
                    Qg    = value.Qg,
                    QgBoe = value.QgBoe,
                    Qt    = value.Qt
                };
            }
        }

        public ProductionDataRecord<T> this[uint index]
        {
            get
            {
                return new ProductionDataRecord<T>(Records.Data + ProductionDataRecord<T>.ThisSize * (int)index,
                                                   _executionSpace);
            }
            set
            {
                ProductionDataRecord<T> productionDataRecord = new ProductionDataRecord<T>(Records.Data + ProductionDataRecord<T>.ThisSize * (int)index,
                                                                                           _executionSpace)
                {
                    Time  = value.Time,
                    Qo    = value.Qo,
                    Qw    = value.Qw,
                    Qg    = value.Qg,
                    QgBoe = value.QgBoe,
                    Qt    = value.Qt
                };
            }
        }

        public ProductionDataRecord<T> this[long index]
        {
            get
            {
                return new ProductionDataRecord<T>(Records.Data + ProductionDataRecord<T>.ThisSize * (int)index,
                                                   _executionSpace);
            }
            set
            {
                ProductionDataRecord<T> productionDataRecord = new ProductionDataRecord<T>(Records.Data + ProductionDataRecord<T>.ThisSize * (int)index,
                                                                                           _executionSpace)
                {
                    Time  = value.Time,
                    Qo    = value.Qo,
                    Qw    = value.Qw,
                    Qg    = value.Qg,
                    QgBoe = value.QgBoe,
                    Qt    = value.Qt
                };
            }
        }

        public ProductionDataRecord<T> this[ulong index]
        {
            get
            {
                return new ProductionDataRecord<T>(Records.Data + ProductionDataRecord<T>.ThisSize * (int)index,
                                                   _executionSpace);
            }
            set
            {
                ProductionDataRecord<T> productionDataRecord = new ProductionDataRecord<T>(Records.Data + ProductionDataRecord<T>.ThisSize * (int)index,
                                                                                           _executionSpace)
                {
                    Time  = value.Time,
                    Qo    = value.Qo,
                    Qw    = value.Qw,
                    Qg    = value.Qg,
                    QgBoe = value.QgBoe,
                    Qt    = value.Qt
                };
            }
        }
    }
}