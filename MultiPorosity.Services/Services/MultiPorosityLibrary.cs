// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local

using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;

using Kokkos;

using MultiPorosity.Models;
using MultiPorosity.Models.BoundConstraints;

using PlatformApi;
using PlatformApi.Win32;

namespace MultiPorosity.Services
{
    [NonVersionable]
    internal static class MultiPorosityLibrary
    {
        public const string LibraryName = "runtime.MultiPorosity";

        public static readonly string RuntimeLibraryName;

        public static nint Handle;

        public static MultiPorosityApi Api;

        //public static volatile bool Initialized;

        public static volatile bool IsLoaded;

        internal static GetApiDelegate GetApi;

        internal static CreateSolverSingleDelegate CreateSolverSingle;

        internal static CreateSolverDoubleDelegate CreateSolverDouble;

        internal static FreeSolverSingleDelegate FreeSolverSingle;

        internal static FreeSolverDoubleDelegate FreeSolverDouble;

        internal static SolverHistoryMatchSingleDelegate SolverHistoryMatchSingle;

        internal static SolverHistoryMatchDoubleDelegate SolverHistoryMatchDouble;

        internal static SolverGetResultsSingleDelegate SolverGetResultsSingle;

        internal static SolverGetResultsDoubleDelegate SolverGetResultsDouble;

        internal static SolverCalculateSingleDelegate SolverCalculateSingle;

        internal static SolverCalculateDoubleDelegate SolverCalculateDouble;

        internal static RelativePermeabilityStoneIISingleDelegate RelativePermeabilityStoneIISingle;

        internal static RelativePermeabilityStoneIIDoubleDelegate RelativePermeabilityStoneIIDouble;

        internal static unsafe delegate* unmanaged<nint, in ExecutionSpaceKind, in float, in float, in float, in float, in float, in float, in float, in float, in float, in float, in float, in float,
            in float, in float, in float, void> RelativePermeabilityTableStoneIISingle;

        internal static unsafe delegate* unmanaged<nint, in ExecutionSpaceKind, in double, in double, in double, in double, in double, in double, in double, in double, in double, in double, in double,
            in double, in double, in double, in double, void> RelativePermeabilityTableStoneIIDouble;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        static MultiPorosityLibrary()
        {
            string operatingSystem      = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "win" : "linux";
            string platformArchitecture = RuntimeInformation.ProcessArchitecture == Architecture.X64 ? "x64" : "x86";

            string libraryPath = GetLibraryPath() ?? throw new NullReferenceException("typeof(MultiPorosityLibrary).Assembly.Location is empty.");

#if DEBUG
            Console.WriteLine("libraryPath: " + libraryPath);
#endif

            string nativeLibraryPath = Path.Combine(libraryPath, $"runtimes\\{operatingSystem}-{platformArchitecture}\\native");

            nativeLibraryPath = Kernel32.GetShortPath(nativeLibraryPath);

            Kernel32.AddToPath(nativeLibraryPath);

            RuntimeLibraryName = LibraryName + (RuntimeInformation.ProcessArchitecture == Architecture.X64 ? ".x64" : ".x86");
        }

        internal static void Load()
        {
            if(!IsLoaded)
            {
                unsafe
                {
#if DEBUG
                    Console.WriteLine("Loading " + RuntimeLibraryName);
#endif
                    //typeof(View).Assembly, DllImportSearchPath.UseDllDirectoryForDependencies, 

                    if(!PlatformApi.NativeLibrary.TryLoad(RuntimeLibraryName, out Handle))
                    {
                        MultiPorosityLibraryException.Throw($"Error Load:{RuntimeLibraryName}");
                    }

                    if(PlatformApi.NativeLibrary.TryGetExport(Handle, "GetApi", out nint getApiHandle))
                    {
                        GetApi = Marshal.GetDelegateForFunctionPointer<GetApiDelegate>(getApiHandle);

                        Api = GetApi(1);

                        CreateSolverSingle = Marshal.GetDelegateForFunctionPointer<CreateSolverSingleDelegate>(Api.CreateSolverSinglePtr);

                        CreateSolverDouble = Marshal.GetDelegateForFunctionPointer<CreateSolverDoubleDelegate>(Api.CreateSolverDoublePtr);

                        FreeSolverSingle = Marshal.GetDelegateForFunctionPointer<FreeSolverSingleDelegate>(Api.FreeSolverSinglePtr);

                        FreeSolverDouble = Marshal.GetDelegateForFunctionPointer<FreeSolverDoubleDelegate>(Api.FreeSolverDoublePtr);

                        SolverHistoryMatchSingle = Marshal.GetDelegateForFunctionPointer<SolverHistoryMatchSingleDelegate>(Api.SolverHistoryMatchSinglePtr);

                        SolverHistoryMatchDouble = Marshal.GetDelegateForFunctionPointer<SolverHistoryMatchDoubleDelegate>(Api.SolverHistoryMatchDoublePtr);

                        SolverGetResultsSingle = Marshal.GetDelegateForFunctionPointer<SolverGetResultsSingleDelegate>(Api.SolverGetResultsSinglePtr);

                        SolverGetResultsDouble = Marshal.GetDelegateForFunctionPointer<SolverGetResultsDoubleDelegate>(Api.SolverGetResultsDoublePtr);

                        SolverCalculateSingle = Marshal.GetDelegateForFunctionPointer<SolverCalculateSingleDelegate>(Api.SolverCalculateSinglePtr);

                        SolverCalculateDouble = Marshal.GetDelegateForFunctionPointer<SolverCalculateDoubleDelegate>(Api.SolverCalculateDoublePtr);

                        //Console.WriteLine("hCreateSolverSingle " + $"@ 0x{CreateSolverSingle.Method.MethodHandle.Value.ToString("X")}");
                    }
                    else
                    {
                        MultiPorosityLibraryException.Throw("'runtime.MultiPorosity::GetApi' not found.");
                    }

                    RelativePermeabilityStoneIISingle =
                        Marshal.GetDelegateForFunctionPointer<RelativePermeabilityStoneIISingleDelegate>(PlatformApi.NativeLibrary.GetExport(Handle, "RelativePermeabilityStoneIISingle"));

                    RelativePermeabilityStoneIIDouble =
                        Marshal.GetDelegateForFunctionPointer<RelativePermeabilityStoneIIDoubleDelegate>(PlatformApi.NativeLibrary.GetExport(Handle, "RelativePermeabilityStoneIIDouble"));

                    RelativePermeabilityTableStoneIISingle =
                        (delegate* unmanaged<nint, in ExecutionSpaceKind, in float, in float, in float, in float, in float, in float, in float, in float, in float, in float, in float, in float, in
                            float, in float, in float, void>)(void*)PlatformApi.NativeLibrary.GetExport(Handle, "RelativePermeabilityTableStoneIISingle");

                    RelativePermeabilityTableStoneIIDouble =
                        (delegate* unmanaged<nint, in ExecutionSpaceKind, in double, in double, in double, in double, in double, in double, in double, in double, in double, in double, in double, in
                            double, in double, in double, in double, void>)(void*)PlatformApi.NativeLibrary.GetExport(Handle, "RelativePermeabilityTableStoneIIDouble");

#if DEBUG
                    Console.WriteLine("Loaded " + RuntimeLibraryName + $"@ 0x{Handle.ToString("X")}");
#endif
                }

                IsLoaded = true;
            }
        }

        internal static void Unload()
        {
            PlatformApi.NativeLibrary.Free(Handle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static string GetNativePackagePath(string nativePackagePath)
        {
            Version lastestVersion = new Version(0, 0, 0, 0);

            Version currentVersion;

            foreach(DirectoryInfo di in new DirectoryInfo(nativePackagePath).GetDirectories())
            {
                currentVersion = new Version(di.Name);

                if(lastestVersion < currentVersion)
                {
                    lastestVersion = currentVersion;
                }
            }

            return Path.Combine(nativePackagePath, lastestVersion.ToString());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static string? GetLibraryPath()
        {
            string fullPath = Assembly.GetExecutingAssembly().Location;

            if(!string.IsNullOrEmpty(fullPath) && !fullPath.Contains(".nuget"))
            {
                int lastIndex = fullPath.LastIndexOf("\\", StringComparison.Ordinal);

                return fullPath.Substring(0, lastIndex);
            }

            string? nugetPackagesEnvironmentVariable = Environment.GetEnvironmentVariable("NUGET_PACKAGES");

            if(!string.IsNullOrEmpty(nugetPackagesEnvironmentVariable))
            {
                string nativePackagePath = Path.Combine(nugetPackagesEnvironmentVariable, "native.MultiPorosity");

                return GetNativePackagePath(nativePackagePath);
            }

            //const string dotnetProfileDirectoryName = ".dotnet";
            //const string toolsShimFolderName        = "tools";

            string? userProfile = Environment.GetEnvironmentVariable(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "USERPROFILE" : "HOME");

            if(!string.IsNullOrEmpty(userProfile))
            {
                string nativePackagePath = Path.Combine(userProfile, ".nuget", "packages", "native.MultiPorosity");

                return GetNativePackagePath(nativePackagePath);
            }

            return null;
        }

        #region Delegates

        [SuppressUnmanagedCodeSecurity]
        internal delegate ref MultiPorosityApi GetApiDelegate(in uint version);

        [SuppressUnmanagedCodeSecurity]
        internal delegate nint CreateSolverSingleDelegate(in ExecutionSpaceKind        execution_space,
                                                          nint                         multi_porosity_data_ptr,
                                                          in InverseTransformPrecision inverseTransformPrecision = InverseTransformPrecision.High);

        [SuppressUnmanagedCodeSecurity]
        internal delegate nint CreateSolverDoubleDelegate(in ExecutionSpaceKind        execution_space,
                                                          nint                         multi_porosity_data_ptr,
                                                          in InverseTransformPrecision inverseTransformPrecision = InverseTransformPrecision.High);

        [SuppressUnmanagedCodeSecurity]
        internal delegate void FreeSolverSingleDelegate(nint                         instance,
                                                        in ExecutionSpaceKind        execution_space,
                                                        in InverseTransformPrecision inverseTransformPrecision);

        [SuppressUnmanagedCodeSecurity]
        internal delegate void FreeSolverDoubleDelegate(nint                         instance,
                                                        in ExecutionSpaceKind        execution_space,
                                                        in InverseTransformPrecision inverseTransformPrecision);

        [SuppressUnmanagedCodeSecurity]
        internal delegate nint SolverHistoryMatchSingleDelegate(in ParticleSwarmOptimizationOptionsF options,
                                                                nint                                 actual_data_rcp_view_ptr,
                                                                nint                                 actual_time_rcp_view_ptr,
                                                                nint                                 weights_rcp_view_ptr,
                                                                nint                                 limits_ptr,
                                                                nint                                 instance,
                                                                in ExecutionSpaceKind                executionSpace,
                                                                in InverseTransformPrecision         inverseTransformPrecision = InverseTransformPrecision.High);

        [SuppressUnmanagedCodeSecurity]
        internal delegate nint SolverHistoryMatchDoubleDelegate(in ParticleSwarmOptimizationOptions options,
                                                                nint                                actual_data_rcp_view_ptr,
                                                                nint                                actual_time_rcp_view_ptr,
                                                                nint                                weights_rcp_view_ptr,
                                                                nint                                limits_ptr,
                                                                nint                                instance,
                                                                in ExecutionSpaceKind               executionSpace,
                                                                in InverseTransformPrecision        inverseTransformPrecision = InverseTransformPrecision.High);

        [SuppressUnmanagedCodeSecurity]
        internal delegate void SolverGetResultsSingleDelegate(nint                   instance,
                                                              in  ExecutionSpaceKind executionSpace,
                                                              out nint               args,
                                                              out float              error,
                                                              out nint               cached_data);

        [SuppressUnmanagedCodeSecurity]
        internal delegate void SolverGetResultsDoubleDelegate(nint                   instance,
                                                              in  ExecutionSpaceKind executionSpace,
                                                              out nint               args,
                                                              out double             error,
                                                              out nint               cached_data);

        [SuppressUnmanagedCodeSecurity]
        internal delegate nint SolverCalculateSingleDelegate(nint                         time_rcp_view_ptr,
                                                             nint                         args_rcp_view_ptr,
                                                             nint                         instance,
                                                             in ExecutionSpaceKind        executionSpace,
                                                             in InverseTransformPrecision inverseTransformPrecision = InverseTransformPrecision.High);

        [SuppressUnmanagedCodeSecurity]
        internal delegate nint SolverCalculateDoubleDelegate(nint                         time_rcp_view_ptr,
                                                             nint                         args_rcp_view_ptr,
                                                             nint                         instance,
                                                             in ExecutionSpaceKind        executionSpace,
                                                             in InverseTransformPrecision inverseTransformPrecision = InverseTransformPrecision.High);

        [SuppressUnmanagedCodeSecurity]
        internal delegate void RelativePermeabilityStoneIISingleDelegate(in  float saturation_water,
                                                                         in  float saturation_oil,
                                                                         in  float saturation_gas,
                                                                         in  float saturation_water_connate,
                                                                         in  float saturation_water_critical,
                                                                         in  float saturation_oil_irreducible_water,
                                                                         in  float saturation_oil_residual_water,
                                                                         in  float saturation_oil_irreducible_gas,
                                                                         in  float saturation_oil_residual_gas,
                                                                         in  float saturation_gas_connate,
                                                                         in  float saturation_gas_critical,
                                                                         in  float permeability_relative_water_oil_irreducible,
                                                                         in  float permeability_relative_oil_water_connate,
                                                                         in  float permeability_relative_gas_liquid_connate,
                                                                         in  float exponent_permeability_relative_water,
                                                                         in  float exponent_permeability_relative_oil_water,
                                                                         in  float exponent_permeability_relative_gas,
                                                                         in  float exponent_permeability_relative_oil_gas,
                                                                         out float permeability_relative_water,
                                                                         out float permeability_relative_oil,
                                                                         out float permeability_relative_gas);

        [SuppressUnmanagedCodeSecurity]
        internal delegate void RelativePermeabilityStoneIIDoubleDelegate(in  double saturation_water,
                                                                         in  double saturation_oil,
                                                                         in  double saturation_gas,
                                                                         in  double saturation_water_connate,
                                                                         in  double saturation_water_critical,
                                                                         in  double saturation_oil_irreducible_water,
                                                                         in  double saturation_oil_residual_water,
                                                                         in  double saturation_oil_irreducible_gas,
                                                                         in  double saturation_oil_residual_gas,
                                                                         in  double saturation_gas_connate,
                                                                         in  double saturation_gas_critical,
                                                                         in  double permeability_relative_water_oil_irreducible,
                                                                         in  double permeability_relative_oil_water_connate,
                                                                         in  double permeability_relative_gas_liquid_connate,
                                                                         in  double exponent_permeability_relative_water,
                                                                         in  double exponent_permeability_relative_oil_water,
                                                                         in  double exponent_permeability_relative_gas,
                                                                         in  double exponent_permeability_relative_oil_gas,
                                                                         out double permeability_relative_water,
                                                                         out double permeability_relative_oil,
                                                                         out double permeability_relative_gas);

        #endregion
    }
}