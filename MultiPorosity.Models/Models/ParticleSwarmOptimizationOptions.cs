using System;
using System.Runtime.InteropServices;

namespace MultiPorosity.Models
{
    [StructLayout(LayoutKind.Explicit)] //, Size = 3 * sizeof(int) + 3 * sizeof(float) + sizeof(bool))]
    public unsafe struct ParticleSwarmOptimizationOptionsF
    {
        [field: FieldOffset(0)]
        public int SwarmSize { get; set; }

        [field: FieldOffset(sizeof(int))]
        public int ParticlesInSwarm { get; set; }

        [field: FieldOffset(2 * sizeof(int))]
        public int IterationMax { get; set; }

        [field: FieldOffset(3 * sizeof(int))]
        public float ErrorThreshold { get; set; }

        [field: FieldOffset(3 * sizeof(int) + sizeof(float))]
        public float MinInertWeight { get; set; }

        [field: FieldOffset(3 * sizeof(int) + 2 * sizeof(float))]
        public float MaxInertWeight { get; set; }

        [field: FieldOffset(3 * sizeof(int) + 3 * sizeof(float))]
        public bool CacheResults { get; set; }

        [field: FieldOffset(3 * sizeof(long) + 3 * sizeof(float) + sizeof(bool))]
        public delegate* unmanaged<long, long, float*, void> ResultCallback;

        public static readonly ParticleSwarmOptimizationOptionsF Default = new ParticleSwarmOptimizationOptionsF(20,
                                                                                                                 20,
                                                                                                                 100,
                                                                                                                 0.0f,
                                                                                                                 0.4f,
                                                                                                                 0.9f,
                                                                                                                 true,
                                                                                                                 null);

        public ParticleSwarmOptimizationOptionsF(int                                           swarm_size,
                                                 int                                           particles_in_swarm,
                                                 int                                           iteration_max,
                                                 float                                         error_threshold,
                                                 float                                         min_inert_weight,
                                                 float                                         max_inert_weight,
                                                 bool                                          cache_results,
                                                 delegate* unmanaged<long, long, float*, void> resultCallback = null)

        {
            SwarmSize        = swarm_size;
            ParticlesInSwarm = particles_in_swarm;
            IterationMax     = iteration_max;
            ErrorThreshold   = error_threshold;
            MinInertWeight   = min_inert_weight;
            MaxInertWeight   = max_inert_weight;
            CacheResults     = cache_results;
            ResultCallback   = resultCallback;
        }
    }

    [StructLayout(LayoutKind.Sequential)]//, Size = 3 * sizeof(long) + 3 * sizeof(double) + sizeof(bool) + sizeof(nint))]
    public unsafe struct ParticleSwarmOptimizationOptions
    {
        //[field: FieldOffset(0)]
        public long SwarmSize { get; set; }

        //[field: FieldOffset(sizeof(long))]
        public long ParticlesInSwarm { get; set; }

        //[field: FieldOffset(2 * sizeof(long))]
        public long IterationMax { get; set; }

        //[field: FieldOffset(3 * sizeof(long))]
        public double ErrorThreshold { get; set; }

        //[field: FieldOffset(3 * sizeof(long) + sizeof(double))]
        public double MinInertWeight { get; set; }

        //[field: FieldOffset(3 * sizeof(long) + 2 * sizeof(double))]
        public double MaxInertWeight { get; set; }

        //[field: FieldOffset(3 * sizeof(long) + 3 * sizeof(double))]
        public delegate*<long, long, double*, void> ResultCallback;

        //[field: FieldOffset(3 * sizeof(long) + 3 * sizeof(double) + sizeof(bool))]
        public bool CacheResults { get; set; }

        public static readonly ParticleSwarmOptimizationOptions Default = new ParticleSwarmOptimizationOptions(20,
                                                                                                               16,
                                                                                                               100,
                                                                                                               0.0,
                                                                                                               0.4,
                                                                                                               0.9,
                                                                                                               null,
                                                                                                               false);

        public ParticleSwarmOptimizationOptions(long                                 swarm_size,
                                                long                                 particles_in_swarm,
                                                long                                 iteration_max,
                                                double                               error_threshold,
                                                double                               min_inert_weight,
                                                double                               max_inert_weight,
                                                bool                                 cache_results)

        {
            SwarmSize        = swarm_size;
            ParticlesInSwarm = particles_in_swarm;
            IterationMax     = iteration_max;
            ErrorThreshold   = error_threshold;
            MinInertWeight   = min_inert_weight;
            MaxInertWeight   = max_inert_weight;
            ResultCallback   = null;
            CacheResults     = cache_results;
        }

        public ParticleSwarmOptimizationOptions(long                                           swarm_size,
                                                long                                           particles_in_swarm,
                                                long                                           iteration_max,
                                                double                                         error_threshold,
                                                double                                         min_inert_weight,
                                                double                                         max_inert_weight,
                                                delegate*<long, long, double*, void>           resultCallback,
                                                bool                                           cache_results)

        {
            SwarmSize        = swarm_size;
            ParticlesInSwarm = particles_in_swarm;
            IterationMax     = iteration_max;
            ErrorThreshold   = error_threshold;
            MinInertWeight   = min_inert_weight;
            MaxInertWeight   = max_inert_weight;
            ResultCallback   = resultCallback;
            CacheResults     = cache_results;
        }

        public static uint EstimateSwarmSize(uint number_of_unknowns)
        {
            uint SWARM_SIZE_MAX = 1000;

            uint result = (uint)(10.0 + 2.0 * Math.Sqrt(2.0 * number_of_unknowns));

            return result < SWARM_SIZE_MAX ? result : SWARM_SIZE_MAX;
        }
    }
}
