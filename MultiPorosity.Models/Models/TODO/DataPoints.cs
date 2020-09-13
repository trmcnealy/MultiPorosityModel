using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace MultiPorosity.Models
{
    namespace DataPoints
    {
        [StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit,
                      Size = 16)]
        public struct Internal
        {
            [FieldOffset(0)]
            internal IntPtr Time;

            [FieldOffset(8)]
            internal IntPtr Weights;

            [SuppressUnmanagedCodeSecurity, MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            [SuppressGCTransition, SuppressUnmanagedCodeSecurity, DllImport(NativeModule.DllName,
                       CallingConvention = CallingConvention.Cdecl,
                       EntryPoint        = "_ZN3EAS18MultiPorosityModel10DataPointsIfEC2EPN6System5ArrayIfEES6_")]
            internal static extern void ctorc__N_EAS_N_MultiPorosityModel_S_DataPoints__f(IntPtr instance,
                                                                                          IntPtr time,
                                                                                          IntPtr weights);

            [SuppressUnmanagedCodeSecurity, MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            [SuppressGCTransition, SuppressUnmanagedCodeSecurity, DllImport(NativeModule.DllName,
                       CallingConvention = CallingConvention.Cdecl,
                       EntryPoint        = "_ZN3EAS18MultiPorosityModel10DataPointsIfEC2ERKS2_")]
            internal static extern void cctorc__N_EAS_N_MultiPorosityModel_S_DataPoints__f(IntPtr instance,
                                                                                           IntPtr other);

            [SuppressUnmanagedCodeSecurity, MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            [SuppressGCTransition, SuppressUnmanagedCodeSecurity, DllImport(NativeModule.DllName,
                       CallingConvention = CallingConvention.Cdecl,
                       EntryPoint        = "_ZN3EAS18MultiPorosityModel10DataPointsIfE6CreateEPPS2_")]
            internal static extern void Createc__N_EAS_N_MultiPorosityModel_S_DataPoints__f(out IntPtr instance);

            [SuppressUnmanagedCodeSecurity, MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            [SuppressGCTransition, SuppressUnmanagedCodeSecurity, DllImport(NativeModule.DllName,
                       CallingConvention = CallingConvention.Cdecl,
                       EntryPoint        = "_ZN3EAS18MultiPorosityModel10DataPointsIfE6CreateEPvRKS2_")]
            internal static extern void Createc__N_EAS_N_MultiPorosityModel_S_DataPoints__f(IntPtr instance,
                                                                                            IntPtr args);

            [SuppressUnmanagedCodeSecurity, MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            [SuppressGCTransition, SuppressUnmanagedCodeSecurity, DllImport(NativeModule.DllName,
                       CallingConvention = CallingConvention.Cdecl,
                       EntryPoint        = "_ZN3EAS18MultiPorosityModel10DataPointsIfE7DestroyEPS2_")]
            internal static extern void Destroyc__N_EAS_N_MultiPorosityModel_S_DataPoints__f(IntPtr instance);

            [SuppressUnmanagedCodeSecurity, MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            [SuppressGCTransition, SuppressUnmanagedCodeSecurity, DllImport(NativeModule.DllName,
                       CallingConvention = CallingConvention.Cdecl,
                       EntryPoint        = "_ZN3EAS18MultiPorosityModel10DataPointsIfE6CreateEPPS2_PN6System5ArrayIfEES8_")]
            internal static extern void Createc__N_EAS_N_MultiPorosityModel_S_DataPoints__f(IntPtr instance,
                                                                                            IntPtr time,
                                                                                            IntPtr weights);

            [SuppressUnmanagedCodeSecurity, MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            [SuppressGCTransition, SuppressUnmanagedCodeSecurity, DllImport(NativeModule.DllName,
                       CallingConvention = CallingConvention.Cdecl,
                       EntryPoint        = "_ZN3EAS18MultiPorosityModel10DataPointsIdEC2EPN6System5ArrayIdEES6_")]
            internal static extern void ctorc__N_EAS_N_MultiPorosityModel_S_DataPoints__d(IntPtr instance,
                                                                                          IntPtr time,
                                                                                          IntPtr weights);

            [SuppressUnmanagedCodeSecurity, MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            [SuppressGCTransition, SuppressUnmanagedCodeSecurity, DllImport(NativeModule.DllName,
                       CallingConvention = CallingConvention.Cdecl,
                       EntryPoint        = "_ZN3EAS18MultiPorosityModel10DataPointsIdEC2ERKS2_")]
            internal static extern void cctorc__N_EAS_N_MultiPorosityModel_S_DataPoints__d(IntPtr instance,
                                                                                           IntPtr other);

            [SuppressUnmanagedCodeSecurity, MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            [SuppressGCTransition, SuppressUnmanagedCodeSecurity, DllImport(NativeModule.DllName,
                       CallingConvention = CallingConvention.Cdecl,
                       EntryPoint        = "_ZN3EAS18MultiPorosityModel10DataPointsIdE6CreateEPPS2_")]
            internal static extern void Createc__N_EAS_N_MultiPorosityModel_S_DataPoints__d(out IntPtr instance);

            [SuppressUnmanagedCodeSecurity, MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            [SuppressGCTransition, SuppressUnmanagedCodeSecurity, DllImport(NativeModule.DllName,
                       CallingConvention = CallingConvention.Cdecl,
                       EntryPoint        = "_ZN3EAS18MultiPorosityModel10DataPointsIdE6CreateEPvRKS2_")]
            internal static extern void Createc__N_EAS_N_MultiPorosityModel_S_DataPoints__d(IntPtr instance,
                                                                                            IntPtr args);

            [SuppressUnmanagedCodeSecurity, MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            [SuppressGCTransition, SuppressUnmanagedCodeSecurity, DllImport(NativeModule.DllName,
                       CallingConvention = CallingConvention.Cdecl,
                       EntryPoint        = "_ZN3EAS18MultiPorosityModel10DataPointsIdE7DestroyEPS2_")]
            internal static extern void Destroyc__N_EAS_N_MultiPorosityModel_S_DataPoints__d(IntPtr instance);

            [SuppressUnmanagedCodeSecurity, MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            [SuppressGCTransition, SuppressUnmanagedCodeSecurity, DllImport(NativeModule.DllName,
                       CallingConvention = CallingConvention.Cdecl,
                       EntryPoint        = "_ZN3EAS18MultiPorosityModel10DataPointsIdE6CreateEPPS2_PN6System5ArrayIdEES8_")]
            internal static extern void Createc__N_EAS_N_MultiPorosityModel_S_DataPoints__d(IntPtr instance,
                                                                                            IntPtr time,
                                                                                            IntPtr weights);
        }
    }

    public unsafe class DataPoints<T> : IDisposable
        where T : unmanaged
    {
        private static readonly Type t = typeof(T);

        internal static readonly ConcurrentDictionary<IntPtr, DataPoints<T>> NativeToManagedMap = new ConcurrentDictionary<IntPtr, DataPoints<T>>();

        protected bool OwnsNativeInstance;

        public IntPtr Instance { get; protected set; }

        public NativeArray<T> Time
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get
            {
                if(((DataPoints.Internal*)Instance)->Time == IntPtr.Zero)
                {
                    return null;
                }

                return new NativeArray<T>(((DataPoints.Internal*)Instance)->Time);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { ((DataPoints.Internal*)Instance)->Time = value?.Instance ?? IntPtr.Zero; }
        }

        public NativeArray<T> Weights
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get
            {
                if(((DataPoints.Internal*)Instance)->Weights == IntPtr.Zero)
                {
                    return null;
                }

                return new NativeArray<T>(((DataPoints.Internal*)Instance)->Weights);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { ((DataPoints.Internal*)Instance)->Weights = value?.Instance ?? IntPtr.Zero; }
        }

        private DataPoints(DataPoints.Internal native,
                           bool                skipVTables = false)
            : this(CopyValue(native),
                   skipVTables)
        {
            OwnsNativeInstance           = true;
            NativeToManagedMap[Instance] = this;
        }

        protected DataPoints(void* native,
                             bool  skipVTables = false)
        {
            if(native == null)
            {
                return;
            }

            Instance = new IntPtr(native);
        }

        public DataPoints(NativeArray<T> time,
                          NativeArray<T> weights)
        {
            if(t == typeof(float))
            {
                Instance                     = Marshal.AllocHGlobal(sizeof(DataPoints.Internal));
                OwnsNativeInstance           = true;
                NativeToManagedMap[Instance] = this;
                IntPtr arg0 = time?.Instance    ?? IntPtr.Zero;
                IntPtr arg1 = weights?.Instance ?? IntPtr.Zero;

                DataPoints.Internal.ctorc__N_EAS_N_MultiPorosityModel_S_DataPoints__f(Instance,
                                                                                      arg0,
                                                                                      arg1);

                return;
            }

            if(t == typeof(double))
            {
                Instance                     = Marshal.AllocHGlobal(sizeof(DataPoints.Internal));
                OwnsNativeInstance           = true;
                NativeToManagedMap[Instance] = this;
                IntPtr arg0 = time?.Instance    ?? IntPtr.Zero;
                IntPtr arg1 = weights?.Instance ?? IntPtr.Zero;

                DataPoints.Internal.ctorc__N_EAS_N_MultiPorosityModel_S_DataPoints__d(Instance,
                                                                                      arg0,
                                                                                      arg1);

                return;
            }

            throw new ArgumentOutOfRangeException(nameof(T),
                                                  string.Join(", ",
                                                              typeof(T).FullName),
                                                  "DataPoints<T> maps a C++ template class and therefore it only supports a limited set of types and their subclasses: <float>, <double>.");
        }

        public DataPoints()
        {
            if(t == typeof(float))
            {
                Instance                     = Marshal.AllocHGlobal(sizeof(DataPoints.Internal));
                OwnsNativeInstance           = true;
                NativeToManagedMap[Instance] = this;

                return;
            }

            if(t == typeof(double))
            {
                Instance                     = Marshal.AllocHGlobal(sizeof(DataPoints.Internal));
                OwnsNativeInstance           = true;
                NativeToManagedMap[Instance] = this;

                return;
            }

            throw new ArgumentOutOfRangeException(nameof(T),
                                                  string.Join(", ",
                                                              typeof(T).FullName),
                                                  "DataPoints<T> maps a C++ template class and therefore it only supports a limited set of types and their subclasses: <float>, <double>.");
        }

        public DataPoints(DataPoints<T> other)
        {
            if(t == typeof(float))
            {
                Instance                        = Marshal.AllocHGlobal(sizeof(DataPoints.Internal));
                OwnsNativeInstance              = true;
                NativeToManagedMap[Instance]    = this;
                *(DataPoints.Internal*)Instance = *(DataPoints.Internal*)other.Instance;

                return;
            }

            if(t == typeof(double))
            {
                Instance                        = Marshal.AllocHGlobal(sizeof(DataPoints.Internal));
                OwnsNativeInstance              = true;
                NativeToManagedMap[Instance]    = this;
                *(DataPoints.Internal*)Instance = *(DataPoints.Internal*)other.Instance;

                return;
            }

            throw new ArgumentOutOfRangeException(nameof(T),
                                                  string.Join(", ",
                                                              typeof(T).FullName),
                                                  "DataPoints<T> maps a C++ template class and therefore it only supports a limited set of types and their subclasses: <float>, <double>.");
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private static void* CopyValue(DataPoints.Internal native)
        {
            IntPtr ret = Marshal.AllocHGlobal(sizeof(DataPoints.Internal));
            *(DataPoints.Internal*)ret = native;

            return ret.ToPointer();
        }

        //public static void Create(DataPoints<T> instance)
        //{
        //    if(t == typeof(float))
        //    {
        //        IntPtr arg0 = instance?.Instance ?? IntPtr.Zero;
        //        DataPoints.Internal.Createc__N_EAS_N_MultiPorosityModel_S_DataPoints__f(out arg0);

        //        return;
        //    }

        //    if(t == typeof(double))
        //    {
        //        IntPtr arg0 = instance?.Instance ?? IntPtr.Zero;
        //        DataPoints.Internal.Createc__N_EAS_N_MultiPorosityModel_S_DataPoints__d(arg0);

        //        return;
        //    }

        //    throw new ArgumentOutOfRangeException(nameof(T),
        //                                          string.Join(", ",
        //                                                      typeof(T).FullName),
        //                                          "DataPoints<T> maps a C++ template class and therefore it only supports a limited set of types and their subclasses: <float>, <double>.");
        //}

        //public static void Create(IntPtr        instance,
        //                          DataPoints<T> args)
        //{
        //    if(t == typeof(float))
        //    {
        //        if(ReferenceEquals(args,
        //                           null))
        //        {
        //            throw new ArgumentNullException(nameof(args),
        //                                            "Cannot be null because it is a C++ reference (&).");
        //        }

        //        IntPtr arg1 = args.Instance;

        //        DataPoints.Internal.Createc__N_EAS_N_MultiPorosityModel_S_DataPoints__f(instance,
        //                                                                                arg1);

        //        return;
        //    }

        //    if(t == typeof(double))
        //    {
        //        if(ReferenceEquals(args,
        //                           null))
        //        {
        //            throw new ArgumentNullException(nameof(args),
        //                                            "Cannot be null because it is a C++ reference (&).");
        //        }

        //        IntPtr arg1 = args.Instance;

        //        DataPoints.Internal.Createc__N_EAS_N_MultiPorosityModel_S_DataPoints__d(instance,
        //                                                                                arg1);

        //        return;
        //    }

        //    throw new ArgumentOutOfRangeException(nameof(T),
        //                                          string.Join(", ",
        //                                                      typeof(T).FullName),
        //                                          "DataPoints<T> maps a C++ template class and therefore it only supports a limited set of types and their subclasses: <float>, <double>.");
        //}

        public static void Create(DataPoints<T>  instance,
                                  NativeArray<T> time,
                                  NativeArray<T> weights)
        {
            if(t == typeof(float))
            {
                IntPtr arg0 = instance?.Instance ?? IntPtr.Zero;
                IntPtr arg1 = time?.Instance     ?? IntPtr.Zero;
                IntPtr arg2 = weights?.Instance  ?? IntPtr.Zero;

                DataPoints.Internal.Createc__N_EAS_N_MultiPorosityModel_S_DataPoints__f(arg0,
                                                                                        arg1,
                                                                                        arg2);

                return;
            }

            if(t == typeof(double))
            {
                IntPtr arg0 = instance?.Instance ?? IntPtr.Zero;
                IntPtr arg1 = time?.Instance     ?? IntPtr.Zero;
                IntPtr arg2 = weights?.Instance  ?? IntPtr.Zero;

                DataPoints.Internal.Createc__N_EAS_N_MultiPorosityModel_S_DataPoints__d(arg0,
                                                                                        arg1,
                                                                                        arg2);

                return;
            }

            throw new ArgumentOutOfRangeException(nameof(T),
                                                  string.Join(", ",
                                                              typeof(T).FullName),
                                                  "DataPoints<T> maps a C++ template class and therefore it only supports a limited set of types and their subclasses: <float>, <double>.");
        }

        internal static DataPoints<T> CreateInstance(IntPtr native,
                                                     bool   skipVTables = false)
        {
            return new DataPoints<T>(native.ToPointer(),
                                     skipVTables);
        }

        internal static DataPoints<T> CreateInstance(DataPoints.Internal native,
                                                     bool                skipVTables = false)
        {
            return new DataPoints<T>(native,
                                     skipVTables);
        }

        public static void Destroy(DataPoints<T> instance)
        {
            if(t == typeof(float))
            {
                IntPtr arg0 = instance?.Instance ?? IntPtr.Zero;
                DataPoints.Internal.Destroyc__N_EAS_N_MultiPorosityModel_S_DataPoints__f(arg0);

                return;
            }

            if(t == typeof(double))
            {
                IntPtr arg0 = instance?.Instance ?? IntPtr.Zero;
                DataPoints.Internal.Destroyc__N_EAS_N_MultiPorosityModel_S_DataPoints__d(arg0);

                return;
            }

            throw new ArgumentOutOfRangeException(nameof(T),
                                                  string.Join(", ",
                                                              typeof(T).FullName),
                                                  "DataPoints<T> maps a C++ template class and therefore it only supports a limited set of types and their subclasses: <float>, <double>.");
        }

        public void Dispose(bool disposing)
        {
            if(Instance == IntPtr.Zero)
            {
                return;
            }

            NativeToManagedMap.TryRemove(Instance,
                                         out DataPoints<T> _);

            if(disposing)
            {
                if(t == typeof(float))
                {
                    DataPoints.Internal.Destroyc__N_EAS_N_MultiPorosityModel_S_DataPoints__f(Instance);

                    return;
                }

                if(t == typeof(double))
                {
                    DataPoints.Internal.Destroyc__N_EAS_N_MultiPorosityModel_S_DataPoints__d(Instance);

                    return;
                }

                throw new ArgumentOutOfRangeException(nameof(T),
                                                      string.Join(", ",
                                                                  typeof(T).FullName),
                                                      "DataPoints<T> maps a C++ template class and therefore it only supports a limited set of types and their subclasses: <float>, <double>.");
            }

            if(OwnsNativeInstance)
            {
                Marshal.FreeHGlobal(Instance);
            }

            Instance = IntPtr.Zero;
        }
    }

    public unsafe class DataPointsF : DataPoints<float>
    {
        private DataPointsF(DataPoints.Internal native,
                            bool                skipVTables = false)
            : this(CopyValue(native),
                   skipVTables)
        {
            OwnsNativeInstance           = true;
            NativeToManagedMap[Instance] = this;
        }

        protected DataPointsF(void* native,
                              bool  skipVTables = false)
            : base((void*)null)
        {
            if(native == null)
            {
                return;
            }

            Instance = new IntPtr(native);
        }

        public DataPointsF(DataPointsF dataPoints)
            : this((void*)null)
        {
            Instance                        = Marshal.AllocHGlobal(sizeof(DataPoints.Internal));
            OwnsNativeInstance              = true;
            NativeToManagedMap[Instance]    = this;
            *(DataPoints.Internal*)Instance = *(DataPoints.Internal*)dataPoints.Instance;
        }

        public DataPointsF()
            : this((void*)null)
        {
            Instance                     = Marshal.AllocHGlobal(sizeof(DataPoints.Internal));
            OwnsNativeInstance           = true;
            NativeToManagedMap[Instance] = this;
        }

        private static void* CopyValue(DataPoints.Internal native)
        {
            IntPtr ret = Marshal.AllocHGlobal(sizeof(DataPoints.Internal));
            *(DataPoints.Internal*)ret = native;

            return ret.ToPointer();
        }

        //internal static DataPointsF CreateInstance(IntPtr native,
        //                                             bool   skipVTables = false)
        //{
        //    return new DataPointsF(native.ToPointer(),
        //                           skipVTables);
        //}

        //internal static DataPointsF CreateInstance(DataPoints.Internal native,
        //                                             bool                skipVTables = false)
        //{
        //    return new DataPointsF(native,
        //                           skipVTables);
        //}
    }

    public unsafe class DataPointsD : DataPoints<double>
    {
        private DataPointsD(DataPoints.Internal native,
                            bool                skipVTables = false)
            : this(CopyValue(native),
                   skipVTables)
        {
            OwnsNativeInstance           = true;
            NativeToManagedMap[Instance] = this;
        }

        protected DataPointsD(void* native,
                              bool  skipVTables = false)
            : base((void*)null)
        {
            if(native == null)
            {
                return;
            }

            Instance = new IntPtr(native);
        }

        public DataPointsD(DataPointsD dataPoints)
            : this((void*)null)
        {
            Instance                        = Marshal.AllocHGlobal(sizeof(DataPoints.Internal));
            OwnsNativeInstance              = true;
            NativeToManagedMap[Instance]    = this;
            *(DataPoints.Internal*)Instance = *(DataPoints.Internal*)dataPoints.Instance;
        }

        public DataPointsD()
            : this((void*)null)
        {
            Instance                     = Marshal.AllocHGlobal(sizeof(DataPoints.Internal));
            OwnsNativeInstance           = true;
            NativeToManagedMap[Instance] = this;
        }

        private static void* CopyValue(DataPoints.Internal native)
        {
            IntPtr ret = Marshal.AllocHGlobal(sizeof(DataPoints.Internal));
            *(DataPoints.Internal*)ret = native;

            return ret.ToPointer();
        }

        //internal static DataPointsD CreateInstance(IntPtr native,
        //                                             bool   skipVTables = false)
        //{
        //    return new DataPointsD(native.ToPointer(),
        //                           skipVTables);
        //}

        //internal static DataPointsD CreateInstance(DataPoints.Internal native,
        //                                             bool                skipVTables = false)
        //{
        //    return new DataPointsD(native,
        //                           skipVTables);
        //}
    }
}