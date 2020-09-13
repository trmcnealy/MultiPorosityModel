// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local

using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Text;

using Kokkos;

using MultiPorosity.Models;
using MultiPorosity.Models.BoundConstraints;

using PlatformApi.Win32;

namespace MultiPorosity.Services
{
//    [NonVersionable]
//    internal static class Kernel32
//    {
//        [Flags]
//        public enum LoadLibraryFlags : uint
//        {
//            None                                = 0,
//            DONT_RESOLVE_DLL_REFERENCES         = 0x00000001,
//            LOAD_IGNORE_CODE_AUTHZ_LEVEL        = 0x00000010,
//            LOAD_LIBRARY_AS_DATAFILE            = 0x00000002,
//            LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE  = 0x00000040,
//            LOAD_LIBRARY_AS_IMAGE_RESOURCE      = 0x00000020,
//            LOAD_LIBRARY_SEARCH_APPLICATION_DIR = 0x00000200,
//            LOAD_LIBRARY_SEARCH_DEFAULT_DIRS    = 0x00001000,
//            LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR    = 0x00000100,
//            LOAD_LIBRARY_SEARCH_SYSTEM32        = 0x00000800,
//            LOAD_LIBRARY_SEARCH_USER_DIRS       = 0x00000400,
//            LOAD_WITH_ALTERED_SEARCH_PATH       = 0x00000008
//        }

//        public const int MaxPath = 255;

//        [SuppressUnmanagedCodeSecurity]
//        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
//        [DllImport("kernel32", EntryPoint = "LoadLibraryA", CharSet = CharSet.Ansi, SetLastError = true)]
//        private static extern IntPtr LoadLibrary([In] [MarshalAs(UnmanagedType.LPStr)] string libName);

//        [SuppressUnmanagedCodeSecurity]
//        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
//        [DllImport("kernel32", EntryPoint = "LoadLibraryW", CharSet = CharSet.Unicode, SetLastError = true)]
//        private static extern IntPtr LoadLibraryW([In] [MarshalAs(UnmanagedType.LPWStr)] string libName);

//        [SuppressUnmanagedCodeSecurity]
//        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
//        [DllImport("kernel32", EntryPoint = "LoadLibraryExA", CharSet = CharSet.Ansi, SetLastError = true)]
//        private static extern IntPtr LoadLibraryEx([In] [MarshalAs(UnmanagedType.LPStr)] string lpFileName,
//                                                   IntPtr                                       hReservedNull,
//                                                   uint                                         dwFlags);

//        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
//        [SuppressUnmanagedCodeSecurity]
//        [DllImport("kernel32", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
//        private static extern IntPtr LoadLibraryExW([In] [MarshalAs(UnmanagedType.LPWStr)] string lpwLibFileName,
//                                                    [In]                                   IntPtr hFile,
//                                                    [In]                                   uint   dwFlags);

//        [SuppressUnmanagedCodeSecurity]
//        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
//        [DllImport("kernel32", EntryPoint = "GetShortPathNameW", CharSet = CharSet.Unicode, SetLastError = true)]
//        public static extern int GetShortPathName([MarshalAs(UnmanagedType.LPWStr)] string        path,
//                                                  [MarshalAs(UnmanagedType.LPWStr)] StringBuilder shortPath,
//                                                  uint                                            shortPathLength);

//        [SuppressUnmanagedCodeSecurity]
//        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
//        [DllImport("kernel32", EntryPoint = "AddDllDirectory", CharSet = CharSet.Unicode, SetLastError = true)]
//        public static extern IntPtr AddDllDirectory([In] [MarshalAs(UnmanagedType.LPWStr)] string newDirectory);

//        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
//        public static string GetShortPath(string path)
//        {
//            StringBuilder shortPath = new StringBuilder(MaxPath);

//            if(path.EndsWith("\\"))
//            {
//                path = path.Substring(0, path.Length - 1);
//            }

//            GetShortPathName(path, shortPath, MaxPath);

//            return shortPath.ToString();
//        }

//        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
//        public static IntPtr AddDllDirectory(string        newDirectory,
//                                             out ErrorCode errorCode)
//        {
//            IntPtr result = AddDllDirectory(newDirectory);
//            errorCode = ((ErrorCode)Marshal.GetLastWin32Error()).IfErrorThrow();

//            return result;
//        }

//        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
//        //public static IntPtr LoadLibrary(string libName, out ErrorCode errorCode)
//        //{
//        //    IntPtr result = LoadLibrary(libName);
//        //    errorCode = ((ErrorCode)Marshal.GetLastWin32Error()).IfErrorThrow();
//        //    return result;
//        //}

//        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
//        public static IntPtr LoadLibraryEx(string           lpFileName,
//                                           LoadLibraryFlags dwFlags,
//                                           out ErrorCode    errorCode)
//        {
//            //LoadLibraryFlags.LOAD_LIBRARY_AS_DATAFILE | LoadLibraryFlags.LOAD_LIBRARY_AS_IMAGE_RESOURCE | LoadLibraryFlags.LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR
//            IntPtr result = LoadLibraryEx(lpFileName, IntPtr.Zero, (uint)dwFlags); // & 0xFFFFFF00

//            errorCode = ((ErrorCode)Marshal.GetLastWin32Error()).IfErrorThrow();

//            return result;
//        }

//        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
//        public static bool AddToPath(string dirToAdd)
//        {
//            if(string.IsNullOrEmpty(dirToAdd))
//            {
//                return false;
//            }

//            if(!Directory.Exists(dirToAdd))
//            {
//                return false;
//            }

//            string text = Environment.GetEnvironmentVariable("PATH");

//            if(text == null)
//            {
//                return false;
//            }

//            //string[] array = text.Split(Path.PathSeparator);

//            text += Path.PathSeparator;

//            text = text.Replace(dirToAdd + Path.PathSeparator, "");

//            if(text[^1] == Path.PathSeparator)
//            {
//                text = text.Substring(0, text.Length - 1);
//            }

//            string value = dirToAdd + Path.PathSeparator + text;

//            Environment.SetEnvironmentVariable("PATH", value);
//#if DEBUG
//            string PATH = Environment.GetEnvironmentVariable("PATH");
//#endif
//            return true;
//        }
//    }

    [NonVersionable]
    internal static class MultiPorosityLibrary
    {
        public const string LibraryName = "runtime.MultiPorosity";

        public static readonly string RuntimeLibraryName;

        public static IntPtr Handle;

        public static MultiPorosityApi Api;

        //public static volatile bool Initialized;

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

            KokkosLibrary.Loaded   += OnKokkosLibraryLoaded;
            KokkosLibrary.Unloaded += OnKokkosLibraryUnloaded;

            if(KokkosLibrary.IsLoaded())
            {
                Load();
            }
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
        private static string GetLibraryPath()
        {
            string fullPath = Assembly.GetExecutingAssembly().Location;

            if(!string.IsNullOrEmpty(fullPath) && !fullPath.Contains(".nuget"))
            {
                int lastIndex = fullPath.LastIndexOf("\\", StringComparison.Ordinal);

                return fullPath.Substring(0, lastIndex);
            }

            string nugetPackagesEnvironmentVariable = Environment.GetEnvironmentVariable("NUGET_PACKAGES");

            if(!string.IsNullOrEmpty(nugetPackagesEnvironmentVariable))
            {
                string nativePackagePath = Path.Combine(nugetPackagesEnvironmentVariable, "native.MultiPorosity");

                return GetNativePackagePath(nativePackagePath);
            }

            //const string dotnetProfileDirectoryName = ".dotnet";
            //const string toolsShimFolderName        = "tools";

            string userProfile = Environment.GetEnvironmentVariable(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "USERPROFILE" : "HOME");

            if(!string.IsNullOrEmpty(userProfile))
            {
                string nativePackagePath = Path.Combine(userProfile, ".nuget", "packages", "native.MultiPorosity");

                return GetNativePackagePath(nativePackagePath);
            }

            return null;
        }

        internal static void OnKokkosLibraryLoaded(object                 sender,
                                                   KokkosLibraryEventArgs e)
        {
            Console.WriteLine("MultiPorosityLibrary: KokkosLibrary Loaded.");
            Load();
        }

        internal static void OnKokkosLibraryUnloaded(object                 sender,
                                                     KokkosLibraryEventArgs e)
        {
            Console.WriteLine("MultiPorosityLibrary: KokkosLibrary Unloaded.");
            Unload();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool IsLoaded()
        {
            return Handle != IntPtr.Zero;
        }

        internal static void Load()
        {
#if DEBUG
            Console.WriteLine("Loading " + RuntimeLibraryName);
#endif
            //typeof(View).Assembly, DllImportSearchPath.UseDllDirectoryForDependencies, 

            if(!PlatformApi.NativeLibrary.TryLoad(RuntimeLibraryName, out Handle, out ulong errorCode))
            {
                MultiPorosityLibraryException.Throw($"Error Code:{errorCode}");
            }

            if(PlatformApi.NativeLibrary.TryGetExport(Handle, "GetApi", out IntPtr getApiHandle, out errorCode))
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

#if DEBUG
            Console.WriteLine("Loaded " + RuntimeLibraryName + $"@ 0x{Handle.ToString("X")}");
#endif
        }

        internal static void Unload()
        {
            PlatformApi.NativeLibrary.Free(Handle, out ulong errorCode);
        }

        #region Delegates
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate ref MultiPorosityApi GetApiDelegate(in uint version);
        
        [SuppressUnmanagedCodeSecurity]
        internal delegate IntPtr CreateSolverSingleDelegate(in ExecutionSpaceKind        execution_space,
                                                            IntPtr                       multi_porosity_data_ptr,
                                                            in InverseTransformPrecision inverseTransformPrecision = InverseTransformPrecision.High);

        [SuppressUnmanagedCodeSecurity]
        internal delegate IntPtr CreateSolverDoubleDelegate(in ExecutionSpaceKind        execution_space,
                                                            IntPtr                       multi_porosity_data_ptr,
                                                            in InverseTransformPrecision inverseTransformPrecision = InverseTransformPrecision.High);

        [SuppressUnmanagedCodeSecurity]
        internal delegate void FreeSolverSingleDelegate(IntPtr                       instance,
                                                        in ExecutionSpaceKind        execution_space,
                                                        in InverseTransformPrecision inverseTransformPrecision);

        [SuppressUnmanagedCodeSecurity]
        internal delegate void FreeSolverDoubleDelegate(IntPtr                       instance,
                                                        in ExecutionSpaceKind        execution_space,
                                                        in InverseTransformPrecision inverseTransformPrecision);

        [SuppressUnmanagedCodeSecurity]
        internal delegate IntPtr SolverHistoryMatchSingleDelegate(in ParticleSwarmOptimizationOptionsF options,
                                                                  IntPtr                               actual_data_rcp_view_ptr,
                                                                  IntPtr                               actual_time_rcp_view_ptr,
                                                                  IntPtr                               weights_rcp_view_ptr,
                                                                  BoundConstraintsSingle[]             limits,
                                                                  IntPtr                               instance,
                                                                  in ExecutionSpaceKind                executionSpace,
                                                                  in InverseTransformPrecision         inverseTransformPrecision = InverseTransformPrecision.High);

        [SuppressUnmanagedCodeSecurity]
        internal delegate IntPtr SolverHistoryMatchDoubleDelegate(in ParticleSwarmOptimizationOptions options,
                                                                  IntPtr                              actual_data_rcp_view_ptr,
                                                                  IntPtr                              actual_time_rcp_view_ptr,
                                                                  IntPtr                              weights_rcp_view_ptr,
                                                                  BoundConstraintsDouble[]            limits,
                                                                  IntPtr                              instance,
                                                                  in ExecutionSpaceKind               executionSpace,
                                                                  in InverseTransformPrecision        inverseTransformPrecision = InverseTransformPrecision.High);

        [SuppressUnmanagedCodeSecurity]
        internal delegate void SolverGetResultsSingleDelegate(IntPtr                 instance,
                                                              in  ExecutionSpaceKind executionSpace,
                                                              out IntPtr             args,
                                                              out float              error,
                                                              out IntPtr             cached_data);

        [SuppressUnmanagedCodeSecurity]
        internal delegate void SolverGetResultsDoubleDelegate(IntPtr                 instance,
                                                              in  ExecutionSpaceKind executionSpace,
                                                              out IntPtr             args,
                                                              out double             error,
                                                              out IntPtr             cached_data);

        [SuppressUnmanagedCodeSecurity]
        internal delegate IntPtr SolverCalculateSingleDelegate(IntPtr                       time_rcp_view_ptr,
                                                               IntPtr                       args_rcp_view_ptr,
                                                               IntPtr                       instance,
                                                               in ExecutionSpaceKind        executionSpace,
                                                               in InverseTransformPrecision inverseTransformPrecision = InverseTransformPrecision.High);

        [SuppressUnmanagedCodeSecurity]
        internal delegate IntPtr SolverCalculateDoubleDelegate(IntPtr                       time_rcp_view_ptr,
                                                               IntPtr                       args_rcp_view_ptr,
                                                               IntPtr                       instance,
                                                               in ExecutionSpaceKind        executionSpace,
                                                               in InverseTransformPrecision inverseTransformPrecision = InverseTransformPrecision.High);

        #endregion
    }
}
