using System.Runtime.InteropServices;

using Kokkos;

using LayoutKind = System.Runtime.InteropServices.LayoutKind;

using NumericalMethods.DataStorage;

namespace MultiPorosity.Services
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct MultiPorosityResult<TDataType, TExecutionSpace>
        where TDataType : struct
        where TExecutionSpace : IExecutionSpace, new()
    {
        public readonly View<TDataType, TExecutionSpace> Args;

        public readonly TDataType Error;

        public readonly DataCache CachedData;

        internal MultiPorosityResult(View<TDataType, TExecutionSpace> args,
                                     TDataType                        error,
                                     DataCache                        cached_data)
        {
            Args       = args;
            Error      = error;
            CachedData = cached_data;
        }
    }
}