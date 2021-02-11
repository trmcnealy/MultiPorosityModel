
using System.Text.Json.Serialization;

namespace MultiPorosity.Services.Models
{
    public sealed class ParticleSwarmOptimizationOptions 
    {
        [JsonPropertyName(nameof(SwarmSize))]
        public long SwarmSize { get; set; }

        [JsonPropertyName(nameof(ParticlesInSwarm))]
        public long ParticlesInSwarm { get; set; }

        [JsonPropertyName(nameof(IterationMax))]
        public long IterationMax { get; set; }

        [JsonPropertyName(nameof(ErrorThreshold))]
        public double ErrorThreshold { get; set; }

        [JsonPropertyName(nameof(MinInertWeight))]
        public double MinInertWeight { get; set; }

        [JsonPropertyName(nameof(MaxInertWeight))]
        public double MaxInertWeight { get; set; }

        [JsonPropertyName(nameof(CacheResults))]
        public bool CacheResults { get; set; }

        public ParticleSwarmOptimizationOptions()
        {
            SwarmSize        = 1;
            ParticlesInSwarm = -1;
            IterationMax     = 40;
            ErrorThreshold   = 0.00;
            MinInertWeight   = 0.4;
            MaxInertWeight   = 0.9;
            CacheResults     = false;
        }

        public ParticleSwarmOptimizationOptions(long   swarmSize,
                                                long   particlesInSwarm,
                                                long   iterationMax,
                                                double errorThreshold,
                                                double minInertWeight,
                                                double maxInertWeight,
                                                bool   cacheResults)
        {
            SwarmSize        = swarmSize;
            ParticlesInSwarm = particlesInSwarm;
            IterationMax     = iterationMax;
            ErrorThreshold   = errorThreshold;
            MinInertWeight   = minInertWeight;
            MaxInertWeight   = maxInertWeight;
            CacheResults     = cacheResults;
        }
    }
}