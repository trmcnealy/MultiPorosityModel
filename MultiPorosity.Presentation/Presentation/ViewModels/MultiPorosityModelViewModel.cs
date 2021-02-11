using System.ComponentModel;

using MultiPorosity.Presentation.Models;
using MultiPorosity.Presentation.Services;

using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace MultiPorosity.Presentation
{
    public class SelectMultiPorosityModelViewEvent : PubSubEvent<int>
    {
    }

    public class MultiPorosityModelViewModel : BindableBase
    {
        
        public MultiPorosityProperties MultiPorosityProperties
        {
            get { return _multiPorosityModelService.ActiveProject.MultiPorosityProperties; }
            set
            {
                _multiPorosityModelService.ActiveProject.MultiPorosityProperties = value;

                RaisePropertyChanged(nameof(MultiPorosityProperties));
            }
        }

        private int _SelectedIndex;
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                if(SetProperty(ref _SelectedIndex, value))
                {

                }
            }
        }


        public DelegateCommand CalcPvtCommand { get; }
        
        public DelegateCommand CalcRelPermCommand { get; }
        
        public DelegateCommand RunModelCommand { get; }

        public DelegateCommand HistoryMatchCommand { get; }
        
        private readonly MultiPorosityModelService _multiPorosityModelService;
        private readonly IEventAggregator          _eventAggregator;

        public MultiPorosityModelViewModel(MultiPorosityModelService multiPorosityModelService,
                                           IEventAggregator          eventAggregator)
        {
            _multiPorosityModelService = multiPorosityModelService;
            _eventAggregator           = eventAggregator;

            _eventAggregator.GetEvent<SelectMultiPorosityModelViewEvent>().Subscribe(OnSelectMultiPorosityModelViewEvent);
            
            OnPropertyChanged(this, new PropertyChangedEventArgs("ActiveProject"));
            
            CalcPvtCommand      = new DelegateCommand(_multiPorosityModelService.CalcPvt);
            CalcRelPermCommand  = new DelegateCommand(_multiPorosityModelService.CalcRelPerm);
            RunModelCommand     = new DelegateCommand(_multiPorosityModelService.RunModel);
            HistoryMatchCommand = new DelegateCommand(_multiPorosityModelService.HistoryMatch);
            
            _multiPorosityModelService.PropertyChanged               -= OnPropertyChanged;
            _multiPorosityModelService.PropertyChanged               += OnPropertyChanged;
        }

        private void OnSelectMultiPorosityModelViewEvent(int viewIndex)
        {
            SelectedIndex = viewIndex;
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
                    
                    RaisePropertyChanged(nameof(MultiPorosityProperties));
                    break;
                }
                case "MultiPorosityProperties":
                {
                    RaisePropertyChanged(nameof(MultiPorosityProperties));

                    break;
                }
            }
        }
    }
}