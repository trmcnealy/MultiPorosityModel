using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MultiPorosity.Models
{
    public struct TriplePorosityOptimizationResults
    {
        [JsonProperty(nameof(Iteration),
                      NamingStrategyType = typeof(DefaultNamingStrategy))]
        public int Iteration { get; set; }

        [JsonProperty(nameof(SwarmIndex),
                      NamingStrategyType = typeof(DefaultNamingStrategy))]
        public int SwarmIndex { get; set; }

        [JsonProperty(nameof(ParticleIndex),
                      NamingStrategyType = typeof(DefaultNamingStrategy))]
        public int ParticleIndex { get; set; }

        [JsonProperty(nameof(km),
                      NamingStrategyType = typeof(DefaultNamingStrategy))]
        public float km { get; set; }

        [JsonProperty(nameof(kmVelocity),
                      NamingStrategyType = typeof(DefaultNamingStrategy))]
        public float kmVelocity { get; set; }

        [JsonProperty(nameof(kF),
                      NamingStrategyType = typeof(DefaultNamingStrategy))]
        public float kF { get; set; }

        [JsonProperty(nameof(kFVelocity),
                      NamingStrategyType = typeof(DefaultNamingStrategy))]
        public float kFVelocity { get; set; }

        [JsonProperty(nameof(kf),
                      NamingStrategyType = typeof(DefaultNamingStrategy))]
        public float kf { get; set; }

        [JsonProperty(nameof(kfVelocity),
                      NamingStrategyType = typeof(DefaultNamingStrategy))]
        public float kfVelocity { get; set; }

        [JsonProperty(nameof(ye),
                      NamingStrategyType = typeof(DefaultNamingStrategy))]
        public float ye { get; set; }

        [JsonProperty(nameof(yeVelocity),
                      NamingStrategyType = typeof(DefaultNamingStrategy))]
        public float yeVelocity { get; set; }

        [JsonProperty(nameof(LF),
                      NamingStrategyType = typeof(DefaultNamingStrategy))]
        public float LF { get; set; }

        [JsonProperty(nameof(LFVelocity),
                      NamingStrategyType = typeof(DefaultNamingStrategy))]
        public float LFVelocity { get; set; }

        [JsonProperty(nameof(Lf),
                      NamingStrategyType = typeof(DefaultNamingStrategy))]
        public float Lf { get; set; }

        [JsonProperty(nameof(LfVelocity),
                      NamingStrategyType = typeof(DefaultNamingStrategy))]
        public float LfVelocity { get; set; }

        [JsonProperty(nameof(sk),
                      NamingStrategyType = typeof(DefaultNamingStrategy))]
        public float sk { get; set; }

        [JsonProperty(nameof(skVelocity),
                      NamingStrategyType = typeof(DefaultNamingStrategy))]
        public float skVelocity { get; set; }

        public TriplePorosityOptimizationResults(double iteration,
                                                 double swarmIndex,
                                                 double particleIndex,
                                                 double km,
                                                 double kmVelocity,
                                                 double kF,
                                                 double kFVelocity,
                                                 double kf,
                                                 double kfVelocity,
                                                 double ye,
                                                 double yeVelocity,
                                                 double lF,
                                                 double lFVelocity,
                                                 double lf,
                                                 double lfVelocity,
                                                 double sk,
                                                 double skVelocity)
        {
            Iteration       = (int)iteration;
            SwarmIndex      = (int)swarmIndex;
            ParticleIndex   = (int)particleIndex;
            this.km         = (float)km;
            this.kmVelocity = (float)kmVelocity;
            this.kF         = (float)kF;
            this.kFVelocity = (float)kFVelocity;
            this.kf         = (float)kf;
            this.kfVelocity = (float)kfVelocity;
            this.ye         = (float)ye;
            this.yeVelocity = (float)yeVelocity;
            LF              = (float)lF;
            LFVelocity      = (float)lFVelocity;
            Lf              = (float)lf;
            LfVelocity      = (float)lfVelocity;
            this.sk         = (float)sk;
            this.skVelocity = (float)skVelocity;
        }

        public TriplePorosityOptimizationResults(double[] values)
        {
            int index = 0;
            Iteration     = (int)values[index++];
            SwarmIndex    = (int)values[index++];
            ParticleIndex = (int)values[index++];
            km            = (float)values[index++];
            kmVelocity    = (float)values[index++];
            kF            = (float)values[index++];
            kFVelocity    = (float)values[index++];
            kf            = (float)values[index++];
            kfVelocity    = (float)values[index++];
            ye            = (float)values[index++];
            yeVelocity    = (float)values[index++];
            LF            = (float)values[index++];
            LFVelocity    = (float)values[index++];
            Lf            = (float)values[index++];
            LfVelocity    = (float)values[index++];
            sk            = (float)values[index++];
            skVelocity    = (float)values[index];
        }
    }
}
