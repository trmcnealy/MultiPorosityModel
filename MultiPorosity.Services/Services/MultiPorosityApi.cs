using System;
using System.Runtime.InteropServices;

namespace MultiPorosity.Services
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MultiPorosityApi
    {
        public IntPtr CreateSolverSinglePtr;
        public IntPtr CreateSolverDoublePtr;
        public IntPtr FreeSolverSinglePtr;
        public IntPtr FreeSolverDoublePtr;
        public IntPtr SolverHistoryMatchSinglePtr;
        public IntPtr SolverHistoryMatchDoublePtr;
        public IntPtr SolverGetResultsSinglePtr;
        public IntPtr SolverGetResultsDoublePtr;
        public IntPtr SolverCalculateSinglePtr;
        public IntPtr SolverCalculateDoublePtr;
    }
}