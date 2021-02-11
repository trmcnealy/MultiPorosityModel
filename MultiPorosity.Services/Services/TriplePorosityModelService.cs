using System;
using System.Runtime.CompilerServices;

using MultiPorosity.Models;

using RuntimeGeneration;

namespace MultiPorosity.Services
{
//    internal delegate IntPtr CalculateDelegate<in T>(IntPtr multiPorositySolver,
//                                                     IntPtr time,
//                                                     T[]    args)
//        where T : unmanaged;

//    internal delegate IntPtr ExecuteDelegate<T>(IntPtr                              multiPorositySolver,
//                                                IntPtr                              weights,
//                                                BoundConstraints<T>[]               limits,
//                                                in ParticleSwarmOptimizationOptions options)
//        where T : unmanaged;

//    internal delegate IntPtr SetupDelegate(IntPtr mpd);

//    public static class TriplePorosityModelCudaSingle
//    {
//#if X86
//        internal const string DllName = "runtime.MultiPorosity.x86";
//#else
//        internal const string DllName = "runtime.MultiPorosity.x64";
//#endif

//        /// <summary>Calculate</summary>

//        #region Calculate

//        [NativeCall(DllName, "_ZN3EAS18MultiPorosityModel19MultiPorositySolverIfLj3ENS0_19LinearFlowrateModelELj26EN6Kokkos4CudaEE9CalculateEPS5_PN6System16Interoperability5ArrayIfEEPKf")]
//        internal static CalculateDelegate<float> Calculate;

//        #endregion

//        /// <summary>Execute</summary>

//        #region Execute

//        [NativeCall(DllName,
//                    "_ZN3EAS18MultiPorosityModel19MultiPorositySolverIfLj3ENS0_19LinearFlowrateModelELj26EN6Kokkos4CudaEE7ExecuteEPS5_PN6System16Interoperability5ArrayIfEEPKNS7_5TupleILj2EJfEEERKN16NumericalMethods10Algorithms32ParticleSwarmOptimizationOptionsIfEE")]
//        internal static ExecuteDelegate<float> Execute;

//        #endregion

//        /// <summary>Setup</summary>

//        #region Setup

//        [NativeCall(DllName, "_ZN3EAS18MultiPorosityModel19MultiPorositySolverIfLj3ENS0_19LinearFlowrateModelELj26EN6Kokkos4CudaEE5SetupEPNS0_17MultiPorosityDataIfEE")]
//        internal static SetupDelegate Setup;

//        #endregion
//    }

//    public static class TriplePorosityModelCudaDouble
//    {
//#if X86
//        internal const string DllName = "runtime.MultiPorosity.x86";
//#else
//        internal const string DllName = "runtime.MultiPorosity.x64";
//#endif

//        /// <summary>Calculate</summary>

//        #region Calculate

//        [NativeCall(DllName, "_ZN3EAS18MultiPorosityModel19MultiPorositySolverIdLj3ENS0_19LinearFlowrateModelELj26EN6Kokkos4CudaEE9CalculateEPS5_PN6System16Interoperability5ArrayIdEEPKd")]
//        internal static CalculateDelegate<double> Calculate;

//        #endregion

//        /// <summary>Execute</summary>

//        #region Execute

//        [NativeCall(DllName,
//                    "_ZN3EAS18MultiPorosityModel19MultiPorositySolverIdLj3ENS0_19LinearFlowrateModelELj26EN6Kokkos4CudaEE7ExecuteEPS5_PN6System16Interoperability5ArrayIdEEPKNS7_5TupleILj2EJdEEERKN16NumericalMethods10Algorithms32ParticleSwarmOptimizationOptionsIdEE")]
//        internal static ExecuteDelegate<double> Execute;

//        #endregion

//        /// <summary>Setup</summary>

//        #region Setup

//        [NativeCall(DllName, "_ZN3EAS18MultiPorosityModel19MultiPorositySolverIdLj3ENS0_19LinearFlowrateModelELj26EN6Kokkos4CudaEE5SetupEPNS0_17MultiPorosityDataIdEE")]
//        internal static SetupDelegate Setup;

//        #endregion
//    }

//    public static class TriplePorosityModelSerialSingle
//    {
//#if X86
//        internal const string DllName = "runtime.MultiPorosity.x86";
//#else
//        internal const string DllName = "runtime.MultiPorosity.x64";
//#endif

//        /// <summary>Calculate</summary>

//        #region Calculate

//        [NativeCall(DllName, "_ZN3EAS18MultiPorosityModel19MultiPorositySolverIfLj3ENS0_19LinearFlowrateModelELj26EN6Kokkos6SerialEE9CalculateEPS5_PN6System16Interoperability5ArrayIfEEPKf")]
//        internal static CalculateDelegate<float> Calculate;

//        #endregion

//        /// <summary>Execute</summary>

//        #region Execute

//        [NativeCall(DllName,
//                    "_ZN3EAS18MultiPorosityModel19MultiPorositySolverIfLj3ENS0_19LinearFlowrateModelELj26EN6Kokkos6SerialEE7ExecuteEPS5_PN6System16Interoperability5ArrayIfEEPKNS7_5TupleILj2EJfEEERKN16NumericalMethods10Algorithms32ParticleSwarmOptimizationOptionsIfEE")]
//        internal static ExecuteDelegate<float> Execute;

//        #endregion

//        /// <summary>Setup</summary>

//        #region Setup

//        [NativeCall(DllName, "_ZN3EAS18MultiPorosityModel19MultiPorositySolverIfLj3ENS0_19LinearFlowrateModelELj26EN6Kokkos6SerialEE5SetupEPNS0_17MultiPorosityDataIfEE")]
//        internal static SetupDelegate Setup;

//        #endregion
//    }

//    public static class TriplePorosityModelSerialDouble
//    {
//#if X86
//        internal const string DllName = "runtime.MultiPorosity.x86";
//#else
//        internal const string DllName = "runtime.MultiPorosity.x64";
//#endif

//        /// <summary>Calculate</summary>

//        #region Calculate

//        [NativeCall(DllName, "_ZN3EAS18MultiPorosityModel19MultiPorositySolverIdLj3ENS0_19LinearFlowrateModelELj26EN6Kokkos6SerialEE9CalculateEPS5_PN6System16Interoperability5ArrayIdEEPKd")]
//        internal static CalculateDelegate<double> Calculate;

//        #endregion

//        /// <summary>Execute</summary>

//        #region Execute

//        [NativeCall(DllName,
//                    "_ZN3EAS18MultiPorosityModel19MultiPorositySolverIdLj3ENS0_19LinearFlowrateModelELj26EN6Kokkos6SerialEE7ExecuteEPS5_PN6System16Interoperability5ArrayIdEEPKNS8_5TupleILj2EJdEEERKN16NumericalMethods10Algorithms32ParticleSwarmOptimizationOptionsIdEE")]
//        internal static ExecuteDelegate<double> Execute;

//        #endregion

//        /// <summary>Setup</summary>

//        #region Setup

//        [NativeCall(DllName, "_ZN3EAS18MultiPorosityModel19MultiPorositySolverIdLj3ENS0_19LinearFlowrateModelELj26EN6Kokkos6SerialEE5SetupEPNS0_17MultiPorosityDataIdEE")]
//        internal static SetupDelegate Setup;

//        #endregion
//    }

//    public static class TriplePorosityModelThreadsSingle
//    {
//#if X86
//        internal const string DllName = "runtime.MultiPorosity.x86";
//#else
//        internal const string DllName = "runtime.MultiPorosity.x64";
//#endif

//        /// <summary>Calculate</summary>

//        #region Calculate

//        [NativeCall(DllName, "_ZN3EAS18MultiPorosityModel19MultiPorositySolverIfLj3ENS0_19LinearFlowrateModelELj26EN6Kokkos7ThreadsEE9CalculateEPS5_PN6System16Interoperability5ArrayIfEEPKf")]
//        internal static CalculateDelegate<float> Calculate;

//        #endregion

//        /// <summary>Execute</summary>

//        #region Execute

//        [NativeCall(DllName,
//                    "_ZN3EAS18MultiPorosityModel19MultiPorositySolverIfLj3ENS0_19LinearFlowrateModelELj26EN6Kokkos7ThreadsEE7ExecuteEPS5_PN6System16Interoperability5ArrayIfEEPKNS8_5TupleILj2EJfEEERKN16NumericalMethods10Algorithms32ParticleSwarmOptimizationOptionsIfEE")]
//        internal static ExecuteDelegate<float> Execute;

//        #endregion

//        /// <summary>Setup</summary>

//        #region Setup

//        [NativeCall(DllName, "_ZN3EAS18MultiPorosityModel19MultiPorositySolverIfLj3ENS0_19LinearFlowrateModelELj26EN6Kokkos7ThreadsEE5SetupEPNS0_17MultiPorosityDataIfEE")]
//        internal static SetupDelegate Setup;

//        #endregion
//    }

//    public static class TriplePorosityModelThreadsDouble
//    {
//#if X86
//        internal const string DllName = "runtime.MultiPorosity.x86";
//#else
//        internal const string DllName = "runtime.MultiPorosity.x64";
//#endif

//        /// <summary>Calculate</summary>

//        #region Calculate

//        [NativeCall(DllName, "_ZN3EAS18MultiPorosityModel19MultiPorositySolverIdLj3ENS0_19LinearFlowrateModelELj26EN6Kokkos7ThreadsEE9CalculateEPS5_PN6System16Interoperability5ArrayIdEEPKd")]
//        internal static CalculateDelegate<double> Calculate;

//        #endregion

//        /// <summary>Execute</summary>

//        #region Execute

//        [NativeCall(DllName,
//                    "_ZN3EAS18MultiPorosityModel19MultiPorositySolverIdLj3ENS0_19LinearFlowrateModelELj26EN6Kokkos7ThreadsEE7ExecuteEPS5_PN6System16Interoperability5ArrayIdEEPKNS8_5TupleILj2EJdEEERKN16NumericalMethods10Algorithms32ParticleSwarmOptimizationOptionsIdEE")]
//        internal static ExecuteDelegate<double> Execute;

//        #endregion

//        /// <summary>Setup</summary>

//        #region Setup

//        [NativeCall(DllName, "_ZN3EAS18MultiPorosityModel19MultiPorositySolverIdLj3ENS0_19LinearFlowrateModelELj26EN6Kokkos7ThreadsEE5SetupEPNS0_17MultiPorosityDataIdEE")]
//        internal static SetupDelegate Setup;

//        #endregion
//    }

    //public abstract class TriplePorosityModel
    //{
    //    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    //    static TriplePorosityModel()
    //    {
    //        RuntimeCil.Generate(typeof(TriplePorosityModel).Assembly);
    //    }
    //}
}