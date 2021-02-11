using System.ComponentModel;

using MultiPorosity.Presentation.Models;
using MultiPorosity.Presentation.Services;

using Prism.Commands;
using Prism.Mvvm;

namespace MultiPorosity.Presentation
{
    public class MultiPorosityHistoryMatchParametersViewModel : BindableBase
    {
        private readonly MultiPorosityModelService _multiPorosityModelService;

        public MultiPorosityHistoryMatchParameters MultiPorosityHistoryMatchParameters
        {
            get { return _multiPorosityModelService.ActiveProject.MultiPorosityHistoryMatchParameters; }
            set
            {
                _multiPorosityModelService.ActiveProject.MultiPorosityHistoryMatchParameters = value;

                RaisePropertyChanged(nameof(MultiPorosityHistoryMatchParameters));
            }
        }

        public long SwarmSize
        {
            get { return _multiPorosityModelService.ActiveProject.ParticleSwarmOptimizationOptions.SwarmSize; }
            set
            {
                _multiPorosityModelService.ActiveProject.ParticleSwarmOptimizationOptions.SwarmSize = value;
                RaisePropertyChanged(nameof(SwarmSize));
            }
        }

        private long _particlesInSwarm;
        public long ParticlesInSwarm
        {
            get { return _particlesInSwarm; }
            set
            {
                if(SetProperty(ref _particlesInSwarm, value))
                {
                    _multiPorosityModelService.ActiveProject.ParticleSwarmOptimizationOptions.ParticlesInSwarm = value;
                }
            }
        }
        
        public long IterationMax
        {
            get { return _multiPorosityModelService.ActiveProject.ParticleSwarmOptimizationOptions.IterationMax; }
            set
            {
                _multiPorosityModelService.ActiveProject.ParticleSwarmOptimizationOptions.IterationMax = value;
                RaisePropertyChanged(nameof(IterationMax));
            }
        }
        
        public double ErrorThreshold
        {
            get { return _multiPorosityModelService.ActiveProject.ParticleSwarmOptimizationOptions.ErrorThreshold; }
            set
            {
                _multiPorosityModelService.ActiveProject.ParticleSwarmOptimizationOptions.ErrorThreshold = value;
                RaisePropertyChanged(nameof(ErrorThreshold));
            }
        }
        
        public double MinInertWeight
        {
            get { return _multiPorosityModelService.ActiveProject.ParticleSwarmOptimizationOptions.MinInertWeight; }
            set
            {
                _multiPorosityModelService.ActiveProject.ParticleSwarmOptimizationOptions.MinInertWeight = value;
                RaisePropertyChanged(nameof(MinInertWeight));
            }
        }
        
        public double MaxInertWeight
        {
            get { return _multiPorosityModelService.ActiveProject.ParticleSwarmOptimizationOptions.MaxInertWeight; }
            set
            {
                _multiPorosityModelService.ActiveProject.ParticleSwarmOptimizationOptions.MaxInertWeight = value;
                RaisePropertyChanged(nameof(MaxInertWeight));
            }
        }
        
        public bool CacheResults
        {
            get { return _multiPorosityModelService.ActiveProject.ParticleSwarmOptimizationOptions.CacheResults; }
            set
            {
                _multiPorosityModelService.ActiveProject.ParticleSwarmOptimizationOptions.CacheResults = value;
                RaisePropertyChanged(nameof(CacheResults));
            }
        }

        public DelegateCommand ParticlesInSwarmCommand { get; }

        public MultiPorosityHistoryMatchParametersViewModel(MultiPorosityModelService multiPorosityModelService)
        {
            _multiPorosityModelService = multiPorosityModelService;

            _multiPorosityModelService.PropertyChanged -= OnPropertyChanged;
            _multiPorosityModelService.PropertyChanged += OnPropertyChanged;

            OnPropertyChanged(this, new PropertyChangedEventArgs("ActiveProject"));

            ParticlesInSwarmCommand = new DelegateCommand(_multiPorosityModelService.CalcParticlesInSwarm);

            _particlesInSwarm = _multiPorosityModelService.ActiveProject.ParticleSwarmOptimizationOptions.ParticlesInSwarm;
        }

        private void OnPropertyChanged(object?                   sender,
                                       PropertyChangedEventArgs? e)
        {
            switch(e.PropertyName)
            {
                case "ActiveProject":
                {
                    _multiPorosityModelService.ActiveProject.PropertyChanged -= OnPropertyChanged;
                    _multiPorosityModelService.ActiveProject.PropertyChanged += OnPropertyChanged;

                    //_multiPorosityModelService.ActiveProject.MultiPorosityHistoryMatchParameters.PropertyChanged -= OnPropertyChanged;
                    //_multiPorosityModelService.ActiveProject.MultiPorosityHistoryMatchParameters.PropertyChanged += OnPropertyChanged;

                    _multiPorosityModelService.ActiveProject.ParticleSwarmOptimizationOptions.PropertyChanged -= OnPropertyChanged;
                    _multiPorosityModelService.ActiveProject.ParticleSwarmOptimizationOptions.PropertyChanged += OnPropertyChanged;

                    RaisePropertyChanged(nameof(MultiPorosityHistoryMatchParameters));

                    break;
                }
                case "MultiPorosityHistoryMatchParameters":
                {
                    RaisePropertyChanged(nameof(MultiPorosityHistoryMatchParameters));

                    break;
                }
                case "SwarmSize":
                {
                    RaisePropertyChanged(nameof(SwarmSize));
                    break;
                }
                case "ParticlesInSwarm":
                {
                    ParticlesInSwarm = _multiPorosityModelService.ActiveProject.ParticleSwarmOptimizationOptions.ParticlesInSwarm;
                    break;
                }
                case "IterationMax":
                {
                    RaisePropertyChanged(nameof(IterationMax));
                    break;
                }
                case "ErrorThreshold":
                {
                    RaisePropertyChanged(nameof(ErrorThreshold));
                    break;
                }
                case "MinInertWeight":
                {
                    RaisePropertyChanged(nameof(MinInertWeight));
                    break;
                }
                case "MaxInertWeight":
                {
                    RaisePropertyChanged(nameof(MaxInertWeight));
                    break;
                }
                case "CacheResults":
                {
                    RaisePropertyChanged(nameof(CacheResults));
                    break;
                }
                //case "ParticleSwarmOptimizationOptions":
                //{
                //    RaisePropertyChanged(nameof(ParticleSwarmOptimizationOptions));
                //    break;
                //}
            }
        }
    }
}