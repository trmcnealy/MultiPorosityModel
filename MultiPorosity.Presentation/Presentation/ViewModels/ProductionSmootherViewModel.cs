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
        
        public ProductionSmootherModel Model
        {
            get { return _productionSmootherService._model; }
            set
            {
                if(SetProperty(ref _productionSmootherService._model, value))
                {
                }
            }
        }

        private readonly ProductionSmootherService _productionSmootherService;

        public ProductionSmootherViewModel(MultiPorosityModelService? multiPorosityModelService)
            : base("Production Data Smoother", 1500.0, 1000.0)
        {
            _productionSmootherService = new(multiPorosityModelService.IsNull());

            CanClose = true;

            SmoothCommand = new DelegateCommand(OnSmooth);
        }

        //private void OnPropertyChanged(object?                  sender,
        //                               PropertyChangedEventArgs e)
        //{
        //    switch(e.PropertyName)
        //    {
        //        case "WellListDataFiltered":
        //        {
        //            OnUpdateLocations();
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