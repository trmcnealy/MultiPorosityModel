using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Engineering.UI;
using Engineering.UI.Controls;

using MultiPorosity.Presentation.Services;

using Prism.Mvvm;

namespace MultiPorosity.Presentation.Models
{
    //public sealed class ParticleSwarmOptimizationOptionsF : BindableBase
    //{
    //    public long SwarmSize
    //    {
    //        get { return _particleSwarmOptimizationOptions.SwarmSize; }
    //        set
    //        {
    //            if(_particleSwarmOptimizationOptions.SwarmSize != value)
    //            {
    //                _particleSwarmOptimizationOptions.SwarmSize = value;
    //                this.RaisePropertyChanged(nameof(SwarmSize));
    //            }
    //        }
    //    }

    //    public long ParticlesInSwarm
    //    {
    //        get { return _particleSwarmOptimizationOptions.ParticlesInSwarm; }
    //        set
    //        {
    //            if(_particleSwarmOptimizationOptions.ParticlesInSwarm != value)
    //            {
    //                _particleSwarmOptimizationOptions.ParticlesInSwarm = value;
    //                this.RaisePropertyChanged(nameof(ParticlesInSwarm));
    //            }
    //        }
    //    }

    //    public long IterationMax
    //    {
    //        get { return _particleSwarmOptimizationOptions.IterationMax; }
    //        set
    //        {
    //            if(_particleSwarmOptimizationOptions.IterationMax != value)
    //            {
    //                _particleSwarmOptimizationOptions.IterationMax = value;
    //                this.RaisePropertyChanged(nameof(IterationMax));
    //            }
    //        }
    //    }

    //    public double ErrorThreshold
    //    {
    //        get { return _particleSwarmOptimizationOptions.ErrorThreshold; }
    //        set
    //        {
    //            if(_particleSwarmOptimizationOptions.ErrorThreshold != value)
    //            {
    //                _particleSwarmOptimizationOptions.ErrorThreshold = value;
    //                this.RaisePropertyChanged(nameof(ErrorThreshold));
    //            }
    //        }
    //    }

    //    public double MinInertWeight
    //    {
    //        get { return _particleSwarmOptimizationOptions.MinInertWeight; }
    //        set
    //        {
    //            if(_particleSwarmOptimizationOptions.MinInertWeight != value)
    //            {
    //                _particleSwarmOptimizationOptions.MinInertWeight = value;
    //                this.RaisePropertyChanged(nameof(MinInertWeight));
    //            }
    //        }
    //    }

    //    public double MaxInertWeight
    //    {
    //        get { return _particleSwarmOptimizationOptions.MaxInertWeight; }
    //        set
    //        {
    //            if(_particleSwarmOptimizationOptions.MaxInertWeight != value)
    //            {
    //                _particleSwarmOptimizationOptions.MaxInertWeight = value;
    //                this.RaisePropertyChanged(nameof(MaxInertWeight));
    //            }
    //        }
    //    }

    //    public bool CacheResults
    //    {
    //        get { return _particleSwarmOptimizationOptions.CacheResults; }
    //        set
    //        {
    //            if(_particleSwarmOptimizationOptions.CacheResults != value)
    //            {
    //                _particleSwarmOptimizationOptions.CacheResults = value;
    //                this.RaisePropertyChanged(nameof(CacheResults));
    //            }
    //        }
    //    }

    //    private MultiPorosity.Models.ParticleSwarmOptimizationOptions _particleSwarmOptimizationOptions;

    //    public ParticleSwarmOptimizationOptionsF(MultiPorosity.Models.ParticleSwarmOptimizationOptionsF particleSwarmOptimizationOptions)
    //    {
    //        _particleSwarmOptimizationOptions = particleSwarmOptimizationOptions;
    //    }

    //    public static implicit operator MultiPorosity.Models.ParticleSwarmOptimizationOptionsF(ParticleSwarmOptimizationOptionsF particleSwarmOptimizationOptions)
    //    {
    //        return particleSwarmOptimizationOptions._particleSwarmOptimizationOptions;
    //    }
    //}

    public sealed class ParticleSwarmOptimizationOptions : BindableBase
    {
        private long   _swarmSize;
        private long   _particlesInSwarm;
        private long   _iterationMax;
        private double _errorThreshold;
        private double _minInertWeight;
        private double _maxInertWeight;
        private bool   _cacheResults;
        
        [Category(nameof(ParticleSwarmOptimizationOptions))]
        [DisplayName(nameof(SwarmSize))]
        [Description("")]
        [Editor(typeof(LongUpDown), typeof(LongUpDown))]
        public long SwarmSize
        {
            get { return _swarmSize; }
            set
            {
                if(SetProperty(ref _swarmSize, value))
                {
                }
            }
        }
        
        [Category(nameof(ParticleSwarmOptimizationOptions))]
        [DisplayName(nameof(ParticlesInSwarm))]
        [Description("")]
        [Editor(typeof(LongUpDown), typeof(LongUpDown))]
        public long ParticlesInSwarm
        {
            get { return _particlesInSwarm; }
            set
            {
                if(SetProperty(ref _particlesInSwarm, value))
                {
                }
            }
        }
        
        [Category(nameof(ParticleSwarmOptimizationOptions))]
        [DisplayName(nameof(IterationMax))]
        [Description("")]
        [Editor(typeof(LongUpDown), typeof(LongUpDown))]
        public long IterationMax
        {
            get { return _iterationMax; }
            set
            {
                if(SetProperty(ref _iterationMax, value))
                {
                }
            }
        }
        
        [Category(nameof(ParticleSwarmOptimizationOptions))]
        [DisplayName(nameof(ErrorThreshold))]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double ErrorThreshold
        {
            get { return _errorThreshold; }
            set
            {
                if(SetProperty(ref _errorThreshold, value))
                {
                }
            }
        }
        
        [Category(nameof(ParticleSwarmOptimizationOptions))]
        [DisplayName(nameof(MinInertWeight))]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double MinInertWeight
        {
            get { return _minInertWeight; }
            set
            {
                if(SetProperty(ref _minInertWeight, value))
                {
                }
            }
        }
        
        [Category(nameof(ParticleSwarmOptimizationOptions))]
        [DisplayName(nameof(MaxInertWeight))]
        [Description("")]
        [Editor(typeof(DoubleUpDown), typeof(DoubleUpDown))]
        public double MaxInertWeight
        {
            get { return _maxInertWeight; }
            set
            {
                if(SetProperty(ref _maxInertWeight, value))
                {
                }
            }
        }
        
        [Category(nameof(ParticleSwarmOptimizationOptions))]
        [DisplayName(nameof(CacheResults))]
        [Description("")]
        public bool CacheResults
        {
            get { return _cacheResults; }
            set
            {
                if(SetProperty(ref _cacheResults, value))
                {
                }
            }
        }
        
        public ParticleSwarmOptimizationOptions(MultiPorosity.Services.Models.ParticleSwarmOptimizationOptions particleSwarmOptimizationOptions)
        {
            _swarmSize        = particleSwarmOptimizationOptions.SwarmSize;
            _particlesInSwarm = particleSwarmOptimizationOptions.ParticlesInSwarm;
            _iterationMax     = particleSwarmOptimizationOptions.IterationMax;
            _errorThreshold   = particleSwarmOptimizationOptions.ErrorThreshold;
            _minInertWeight   = particleSwarmOptimizationOptions.MinInertWeight;
            _maxInertWeight   = particleSwarmOptimizationOptions.MaxInertWeight;
            _cacheResults     = particleSwarmOptimizationOptions.CacheResults;
        }
        
        public static implicit operator MultiPorosity.Services.Models.ParticleSwarmOptimizationOptions(ParticleSwarmOptimizationOptions particleSwarmOptimizationOptions)
        {
            return new(particleSwarmOptimizationOptions.SwarmSize,
                       particleSwarmOptimizationOptions.ParticlesInSwarm,
                       particleSwarmOptimizationOptions.IterationMax,
                       particleSwarmOptimizationOptions.ErrorThreshold,
                       particleSwarmOptimizationOptions.MinInertWeight,
                       particleSwarmOptimizationOptions.MaxInertWeight,
                       particleSwarmOptimizationOptions.CacheResults);
        }
        
        public override string ToString()
        {
            return string.Empty;
        }
    }
}