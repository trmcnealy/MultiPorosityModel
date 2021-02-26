using System.Collections.Specialized;
using System.ComponentModel;

using Engineering.UI.Controls;

using MultiPorosity.Presentation.Models;
using MultiPorosity.Presentation.Services;
using MultiPorosity.Services;

using Prism.Commands;
using Prism.Services.Dialogs;

namespace MultiPorosity.Presentation
{
    public class ConnectToDatabaseViewModel : DialogViewModel
    {
        public bool CanClose { get; set; }
        
        public DelegateCommand ConnectCommand { get; }

        public DelegateCommand GetWellListQueryCommand { get; }

        public DelegateCommand GetWellProductionQueryCommand { get; }

        public DelegateCommand ExportWellListCommand { get; }

        public DelegateCommand ExportWellProductionCommand { get; }

        private ConnectToDatabaseModel _model;

        public ConnectToDatabaseModel Model
        {
            get { return _model; }
            set
            {
                if(SetProperty(ref _model, value))
                {
                }
            }
        }

        private readonly DatabaseConnectionService _databaseConnectionService;

        public ConnectToDatabaseViewModel(MultiPorosityModelService? multiPorosityModelService)
            : base("Connect to a Database", 1500.0, 1000.0)
        {
            _databaseConnectionService = new(multiPorosityModelService);

            CanClose = true;

            _model = new ConnectToDatabaseModel
            {
                DatabaseDataSource = _databaseConnectionService.DatabaseDataSource
            };

            ConnectCommand                = new DelegateCommand(OnConnect);
            GetWellListQueryCommand       = new DelegateCommand(OnGetWellListQuery);
            GetWellProductionQueryCommand = new DelegateCommand(OnGetWellProductionQuery);
            ExportWellListCommand         = new DelegateCommand(OnExportWellList);
            ExportWellProductionCommand   = new DelegateCommand(OnExportWellProduction);



            void OnCollectionChanged(object?                          sender,
                                     NotifyCollectionChangedEventArgs args)
            {
                OnPropertyChanged(this, new PropertyChangedEventArgs("SelectedWells"));
            }

            _model.PropertyChanged -= OnPropertyChanged;
            _model.PropertyChanged += OnPropertyChanged;
            
            _model.SelectedWells.CollectionChanged -= OnCollectionChanged;
            _model.SelectedWells.CollectionChanged += OnCollectionChanged;
            _model.SelectedWellsCollectionChanged -= OnCollectionChanged;
            _model.SelectedWellsCollectionChanged += OnCollectionChanged;
        }

        private void OnPropertyChanged(object?                  sender,
                                       PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case "SelectedWells":
                {
                    OnGetSelectedWellListQuery();
                    break;
                }
                case "WellListDataFiltered":
                {
                    OnUpdateLocations();
                    break;
                }
            }
        }

        private void OnUpdateLocations()
        {
            Model.Locations = _databaseConnectionService.UpdateLocations(Model.WellListDataFiltered);
        }

        private void OnConnect()
        {
            (_databaseConnectionService.Connection, Model.SessionId) = DataSources.ConnectToDatabase(Model.DatabaseDataSource);
        }

        private void OnGetWellListQuery()
        {
            Model.WellListData = _databaseConnectionService.GetWellListQuery(Model.SessionId);
        }

        private void OnGetWellProductionQuery()
        {
            Model.WellProductionData = _databaseConnectionService.GetWellProductionQuery(Model.SessionId, Model.SelectedDataRow);
        }

        private void OnGetSelectedWellListQuery()
        {
            Model.WellListData = _databaseConnectionService.GetSelectedWellListQuery(Model.SessionId, Model.SelectedWells);
        }

        protected override void CloseDialog(string parameter)
        {
            if(parameter?.ToLower() == "true")
            {
                _databaseConnectionService.ImportTable(Model.WellProductionData);
            }

            base.CloseDialog(parameter);
        }
        
        private void OnExportWellList()
        {
            _databaseConnectionService.ExportWellList(Model.WellListDataFiltered);
        }

        private void OnExportWellProduction()
        {
            _databaseConnectionService.ExportWellProduction(Model.WellProductionData);
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
        }

        public override void OnDialogClosed()
        {
            _databaseConnectionService.DatabaseDataSource = Model.DatabaseDataSource;
        }
    }
}