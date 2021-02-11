using System;
using System.Runtime.CompilerServices;

namespace MultiPorosity.Services
{
    public static class ParticleSwarmOptimizationService
    {
        private const long SWARM_SIZE_MAX = 1000;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static long NumberOfParticlesPerSwarm(long particlesInSwarm)
        {
            long result = (long)(10.0 + 2.0 * Math.Sqrt(2.0 * particlesInSwarm));

            return (result < SWARM_SIZE_MAX) ? result : SWARM_SIZE_MAX;
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //public static long NumberOfParticlesPerSwarm(long numberOfUnknownParameters, long particlesInSwarm)
        //{
        //    if (particlesInSwarm != -1)
        //    {
        //        return particlesInSwarm;
        //    }
            
        //    return NumberOfParticlesPerSwarm(numberOfUnknownParameters);
        //}
    }
}