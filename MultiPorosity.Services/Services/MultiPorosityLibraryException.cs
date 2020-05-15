using System;
using System.Runtime.CompilerServices;

namespace MultiPorosity.Services
{
    internal class MultiPorosityLibraryException : Exception
    {
        public MultiPorosityLibraryException()
        {
        }

        public MultiPorosityLibraryException(string message)
            : base(message)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        internal static void Throw()
        {
            throw new MultiPorosityLibraryException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        internal static void Throw(string message)
        {
            throw new MultiPorosityLibraryException(message);
        }
    }
}