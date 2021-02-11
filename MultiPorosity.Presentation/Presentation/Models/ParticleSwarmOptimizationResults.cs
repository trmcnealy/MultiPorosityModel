using System.Collections.Generic;

using Prism.Mvvm;

namespace MultiPorosity.Presentation.Models
{
    public class TriplePorosityOptimizationResults : BindableBase
    {
        private long   _Iteration;
        private long   _SwarmIndex;
        private long   _ParticleIndex;
        private double _residual;
        private double _km;
        private double _kmVelocity;
        private double _kF;
        private double _kFVelocity;
        private double _kf;
        private double _kfVelocity;
        private double _ye;
        private double _yeVelocity;
        private double _LF;
        private double _LFVelocity;
        private double _Lf;
        private double _LfVelocity;
        private double _sk;
        private double _skVelocity;

        public long Iteration
        {
            get { return _Iteration; }
            set
            {
                if(SetProperty(ref _Iteration, value))
                {
                }
            }
        }

        public long SwarmIndex
        {
            get { return _SwarmIndex; }
            set
            {
                if(SetProperty(ref _SwarmIndex, value))
                {
                }
            }
        }

        public long ParticleIndex
        {
            get { return _ParticleIndex; }
            set
            {
                if(SetProperty(ref _ParticleIndex, value))
                {
                }
            }
        }

        public double Residual
        {
            get { return _residual; }
            set
            {
                if(SetProperty(ref _residual, value))
                {
                }
            }
        }

        public double km
        {
            get { return _km; }
            set
            {
                if(SetProperty(ref _km, value))
                {
                }
            }
        }

        public double kmVelocity
        {
            get { return _kmVelocity; }
            set
            {
                if(SetProperty(ref _kmVelocity, value))
                {
                }
            }
        }

        public double kF
        {
            get { return _kF; }
            set
            {
                if(SetProperty(ref _kF, value))
                {
                }
            }
        }

        public double kFVelocity
        {
            get { return _kFVelocity; }
            set
            {
                if(SetProperty(ref _kFVelocity, value))
                {
                }
            }
        }

        public double kf
        {
            get { return _kf; }
            set
            {
                if(SetProperty(ref _kf, value))
                {
                }
            }
        }

        public double kfVelocity
        {
            get { return _kfVelocity; }
            set
            {
                if(SetProperty(ref _kfVelocity, value))
                {
                }
            }
        }

        public double ye
        {
            get { return _ye; }
            set
            {
                if(SetProperty(ref _ye, value))
                {
                }
            }
        }

        public double yeVelocity
        {
            get { return _yeVelocity; }
            set
            {
                if(SetProperty(ref _yeVelocity, value))
                {
                }
            }
        }

        public double LF
        {
            get { return _LF; }
            set
            {
                if(SetProperty(ref _LF, value))
                {
                }
            }
        }

        public double LFVelocity
        {
            get { return _LFVelocity; }
            set
            {
                if(SetProperty(ref _LFVelocity, value))
                {
                }
            }
        }

        public double Lf
        {
            get { return _Lf; }
            set
            {
                if(SetProperty(ref _Lf, value))
                {
                }
            }
        }

        public double LfVelocity
        {
            get { return _LfVelocity; }
            set
            {
                if(SetProperty(ref _LfVelocity, value))
                {
                }
            }
        }

        public double sk
        {
            get { return _sk; }
            set
            {
                if(SetProperty(ref _sk, value))
                {
                }
            }
        }

        public double skVelocity
        {
            get { return _skVelocity; }
            set
            {
                if(SetProperty(ref _skVelocity, value))
                {
                }
            }
        }

        public TriplePorosityOptimizationResults(double iteration,
                                                 double swarmIndex,
                                                 double particleIndex,
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
            Iteration       = (long)iteration;
            SwarmIndex      = (long)swarmIndex;
            ParticleIndex   = (long)particleIndex;
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

        public static implicit operator MultiPorosity.Services.Models.TriplePorosityOptimizationResults(TriplePorosityOptimizationResults triplePorosityOptimizationResults)
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

        public static explicit operator TriplePorosityOptimizationResults(MultiPorosity.Services.Models.TriplePorosityOptimizationResults triplePorosityOptimizationResults)
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


        public static List<TriplePorosityOptimizationResults> Convert(List<MultiPorosity.Services.Models.TriplePorosityOptimizationResults> triplePorosityOptimizationResults)
        {
            List<TriplePorosityOptimizationResults> triplePorosityOptimization = new(triplePorosityOptimizationResults.Count);

            for(int i = 0; i < triplePorosityOptimizationResults.Count; ++i)
            {
                triplePorosityOptimization.Add((TriplePorosityOptimizationResults)triplePorosityOptimizationResults[i]);
            }

            return triplePorosityOptimization;
        }

        public static List<MultiPorosity.Services.Models.TriplePorosityOptimizationResults> Convert(List<TriplePorosityOptimizationResults> triplePorosityOptimizationResults)
        {
            List<MultiPorosity.Services.Models.TriplePorosityOptimizationResults> triplePorosityOptimization = new(triplePorosityOptimizationResults.Count);

            for(int i = 0; i < triplePorosityOptimizationResults.Count; ++i)
            {
                triplePorosityOptimization.Add(triplePorosityOptimizationResults[i]);
            }

            return triplePorosityOptimization;
        }

    }
}