using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace MultiPorosity.Presentation
{
    public class WeightsChangedEvent : PubSubEvent<double>
    {
    }
    
    public class DataSourceViewModel : BindableBase
    {
        public DelegateCommand? SetSelectedWeightsCommand { get; }

        private double _SelectedWeightsValue = 1.0;

        public double SelectedWeightsValue
        {
            get { return _SelectedWeightsValue; }
            set { SetProperty(ref _SelectedWeightsValue, value); }
        }
        
        private readonly IEventAggregator _eventAggregator;
        
        public DataSourceViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator          = eventAggregator;
            SetSelectedWeightsCommand = new DelegateCommand(SetSelectedWeights);
        }

        private void SetSelectedWeights()
        {
            _eventAggregator.GetEvent<WeightsChangedEvent>().Publish(SelectedWeightsValue);
        }
    }
}