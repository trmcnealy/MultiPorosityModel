using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

using Engineering.DataSource;

using Prism.Events;

namespace MultiPorosity.Services.Models
{
    public class TriplePorosityOptimizationResultsEvent : PubSubEvent<Array<TriplePorosityOptimizationResults>>
    {
    }


    public struct TriplePorosityOptimizationResults
    {
        [JsonPropertyName(nameof(Iteration))]
        public long Iteration { get; set; }

        [JsonPropertyName(nameof(SwarmIndex))]
        public long SwarmIndex { get; set; }

        [JsonPropertyName(nameof(ParticleIndex))]
        public long ParticleIndex { get; set; }

        [JsonPropertyName(nameof(Residual))]
        public double Residual { get; set; }

        [JsonPropertyName(nameof(km))]
        public double km { get; set; }

        [JsonPropertyName(nameof(kmVelocity))]
        public double kmVelocity { get; set; }

        [JsonPropertyName(nameof(kF))]
        public double kF { get; set; }

        [JsonPropertyName(nameof(kFVelocity))]
        public double kFVelocity { get; set; }

        [JsonPropertyName(nameof(kf))]
        public double kf { get; set; }

        [JsonPropertyName(nameof(kfVelocity))]
        public double kfVelocity { get; set; }

        [JsonPropertyName(nameof(ye))]
        public double ye { get; set; }

        [JsonPropertyName(nameof(yeVelocity))]
        public double yeVelocity { get; set; }

        [JsonPropertyName(nameof(LF))]
        public double LF { get; set; }

        [JsonPropertyName(nameof(LFVelocity))]
        public double LFVelocity { get; set; }

        [JsonPropertyName(nameof(Lf))]
        public double Lf { get; set; }

        [JsonPropertyName(nameof(LfVelocity))]
        public double LfVelocity { get; set; }

        [JsonPropertyName(nameof(sk))]
        public double sk { get; set; }

        [JsonPropertyName(nameof(skVelocity))]
        public double skVelocity { get; set; }

        public TriplePorosityOptimizationResults(long   iteration,
                                                 long   swarmIndex,
                                                 long   particleIndex,
                                                 double residual,
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
            Iteration       = iteration;
            SwarmIndex      = swarmIndex;
            ParticleIndex   = particleIndex;
            Residual        = residual;
            this.km         = km;
            this.kmVelocity = kmVelocity;
            this.kF         = kF;
            this.kFVelocity = kFVelocity;
            this.kf         = kf;
            this.kfVelocity = kfVelocity;
            this.ye         = ye;
            this.yeVelocity = yeVelocity;
            LF              = lF;
            LFVelocity      = lFVelocity;
            Lf              = lf;
            LfVelocity      = lfVelocity;
            this.sk         = sk;
            this.skVelocity = skVelocity;
        }

        public TriplePorosityOptimizationResults(double[] values)
        {
            int index = 0;
            Iteration     = (long)values[index++];
            SwarmIndex    = (long)values[index++];
            ParticleIndex = (long)values[index++];
            Residual      = values[index++];
            km            = values[index++];
            kmVelocity    = values[index++];
            kF            = values[index++];
            kFVelocity    = values[index++];
            kf            = values[index++];
            kfVelocity    = values[index++];
            ye            = values[index++];
            yeVelocity    = values[index++];
            LF            = values[index++];
            LFVelocity    = values[index++];
            Lf            = values[index++];
            LfVelocity    = values[index++];
            sk            = values[index++];
            skVelocity    = values[index];
        }

        public static implicit operator MultiPorosity.Models.TriplePorosityOptimizationResults(TriplePorosityOptimizationResults triplePorosityOptimizationResults)
        {
            return new (triplePorosityOptimizationResults.Iteration,
                        triplePorosityOptimizationResults.SwarmIndex,
                        triplePorosityOptimizationResults.ParticleIndex,
                        triplePorosityOptimizationResults.Residual,
                        triplePorosityOptimizationResults.km,
                        triplePorosityOptimizationResults.kmVelocity,
                        triplePorosityOptimizationResults.kF,
                        triplePorosityOptimizationResults.kFVelocity,
                        triplePorosityOptimizationResults.kf,
                        triplePorosityOptimizationResults.kfVelocity,
                        triplePorosityOptimizationResults.ye,
                        triplePorosityOptimizationResults.yeVelocity,
                        triplePorosityOptimizationResults.LF,
                        triplePorosityOptimizationResults.LFVelocity,
                        triplePorosityOptimizationResults.Lf,
                        triplePorosityOptimizationResults.LfVelocity,
                        triplePorosityOptimizationResults.sk,
                        triplePorosityOptimizationResults.skVelocity);
        }

        public static explicit operator TriplePorosityOptimizationResults(MultiPorosity.Models.TriplePorosityOptimizationResults triplePorosityOptimizationResults)
        {
            return new(triplePorosityOptimizationResults.Iteration,
                       triplePorosityOptimizationResults.SwarmIndex,
                       triplePorosityOptimizationResults.ParticleIndex,
                       triplePorosityOptimizationResults.Residual,
                       triplePorosityOptimizationResults.km,
                       triplePorosityOptimizationResults.kmVelocity,
                       triplePorosityOptimizationResults.kF,
                       triplePorosityOptimizationResults.kFVelocity,
                       triplePorosityOptimizationResults.kf,
                       triplePorosityOptimizationResults.kfVelocity,
                       triplePorosityOptimizationResults.ye,
                       triplePorosityOptimizationResults.yeVelocity,
                       triplePorosityOptimizationResults.LF,
                       triplePorosityOptimizationResults.LFVelocity,
                       triplePorosityOptimizationResults.Lf,
                       triplePorosityOptimizationResults.LfVelocity,
                       triplePorosityOptimizationResults.sk,
                       triplePorosityOptimizationResults.skVelocity);
        }

        public static List<TriplePorosityOptimizationResults> Convert(List<MultiPorosity.Models.TriplePorosityOptimizationResults> triplePorosityOptimizationResults)
        {
            List<TriplePorosityOptimizationResults> triplePorosityOptimization = new(triplePorosityOptimizationResults.Count);

            for(int i = 0; i < triplePorosityOptimizationResults.Count; ++i)
            {
                triplePorosityOptimization.Add((TriplePorosityOptimizationResults)triplePorosityOptimizationResults[i]);
            }

            return triplePorosityOptimization;
        }

        public static List<MultiPorosity.Models.TriplePorosityOptimizationResults> Convert(List<TriplePorosityOptimizationResults> triplePorosityOptimizationResults)
        {
            List<MultiPorosity.Models.TriplePorosityOptimizationResults> triplePorosityOptimization = new(triplePorosityOptimizationResults.Count);

            for(int i = 0; i < triplePorosityOptimizationResults.Count; ++i)
            {
                triplePorosityOptimization.Add(triplePorosityOptimizationResults[i]);
            }

            return triplePorosityOptimization;
        }
        


        private static readonly string[] names1 = new string[]{"Iteration","SwarmIndex","ParticleIndex","Residual","km","kmVelocity","kF","kFVelocity","kf","kfVelocity","ye","yeVelocity","LF","LFVelocity","Lf","LfVelocity"};

        private static ref readonly string[] Names1
        {
            get
            {
                return ref names1;
            }
        }

        private static readonly string[] names2 = new string[]{"Iteration","SwarmIndex","ParticleIndex","Residual","km","kmVelocity","kF","kFVelocity","kf","kfVelocity","ye","yeVelocity","LF","LFVelocity","Lf","LfVelocity","sk","skVelocity"};

        private static ref readonly string[] Names2
        {
            get
            {
                return ref names2;
            }
        }

        public string[] GetNames()
        {
            if(sk != 0.0)
            {
                return Names1;
            }
            return Names2;
        }

        public int GetLength()
        {
            if(sk == 0.0)
            {
                return 16;
            }
            return 18;
        }

        public object this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                switch(index)
                {
                    case 0:
                    {
                        return Iteration;
                    }
                    case 1:
                    {
                        return SwarmIndex;
                    }
                    case 2:
                    {
                        return ParticleIndex;
                    }
                    case 3:
                    {
                        return Residual;
                    }
                    case 4:
                    {
                        return km;
                    }
                    case 5:
                    {
                        return kmVelocity;
                    }
                    case 6:
                    {
                        return kF;
                    }
                    case 7:
                    {
                        return kFVelocity;
                    }
                    case 8:
                    {
                        return kf;
                    }
                    case 9:
                    {
                        return kfVelocity;
                    }
                    case 10:
                    {
                        return ye;
                    }
                    case 11:
                    {
                        return yeVelocity;
                    }
                    case 12:
                    {
                        return LF;
                    }
                    case 13:
                    {
                        return LFVelocity;
                    }
                    case 14:
                    {
                        return Lf;
                    }
                    case 15:
                    {
                        return LfVelocity;
                    }
                    case 16:
                    {
                        return sk;
                    }
                    case 17:
                    {
                        return skVelocity;
                    }
                    default:
                    {
                        return Iteration;
                    }
                }
            }
        }
        
        public override string ToString()
        {
            if(sk == 0.0)
            {
                return $"{Iteration},{SwarmIndex},{ParticleIndex},{Residual},{km},{kmVelocity},{kF},{kFVelocity},{kf},{kfVelocity},{ye},{yeVelocity},{LF},{LFVelocity},{Lf},{LfVelocity}";
            }
            return $"{Iteration},{SwarmIndex},{ParticleIndex},{Residual},{km},{kmVelocity},{kF},{kFVelocity},{kf},{kfVelocity},{ye},{yeVelocity},{LF},{LFVelocity},{Lf},{LfVelocity},{sk},{skVelocity}";
        }
    }
}