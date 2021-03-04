using System.Collections.Specialized;
using System.ComponentModel;

using Engineering.DataSource;
using Engineering.UI.Controls;

using MultiPorosity.Presentation.Models;
using MultiPorosity.Presentation.Services;
using MultiPorosity.Services;

using Prism.Commands;
using Prism.Services.Dialogs;

namespace MultiPorosity.Presentation
{
    public class ProductionSmootherViewModel : DialogViewModel
    {
        public bool CanClose { get; set; }
        
        public DelegateCommand SmoothCommand { get; }
        


        
        private ProductionSmootherService _productionSmootherService;
        public ProductionSmootherService Service
        {
            get { return _productionSmootherService; }
            set
            {

                if(SetProperty(ref _productionSmootherService, value))
                {

                //    void OnModelPropertyChanged(object?                  sender,
                //                                   PropertyChangedEventArgs e)
                //    {
                //        RaisePropertyChanged(nameof(Model));
                //    }
                    
                //    _productionSmootherService._model.PropertyChanged -= OnModelPropertyChanged;
                //    _productionSmootherService._model.PropertyChanged += OnModelPropertyChanged;
                }
            }
        }




        public ProductionSmootherViewModel(ProductionSmootherService? productionSmootherService)
            : base("Production Data Smoother", 1500.0, 1000.0)
        {
            _productionSmootherService = productionSmootherService.IsNull();

            CanClose = true;

            SmoothCommand = new DelegateCommand(OnSmooth);
        }

        //private void OnPropertyChanged(object? sender,
        //                               PropertyChangedEventArgs e)
        //{
        //    switch (e.PropertyName)
        //    {
        //        case "Model":
        //        {
        //            RaisePropertyChanged(nameof(Model));
        //            break;
        //        }
        //    }
        //}

        private void OnSmooth()
        {
            _productionSmootherService.SmoothProduction();
        }

        protected override void CloseDialog(string parameter)
        {
            if(parameter?.ToLower() == "true")
            {
                _productionSmootherService.ImportTable();
            }

            base.CloseDialog(parameter);
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
        }

        public override void OnDialogClosed()
        {
            
        }
    }
}