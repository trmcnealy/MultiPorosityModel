using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

using Engineering.UI.Collections;

using Microsoft.Win32;

using MultiPorosity.Models;
using MultiPorosity.Presentation.Services;
using MultiPorosity.Services;

using Prism.Mvvm;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

namespace MultiPorosity.Presentation
{
    public sealed class ProductionHistoryViewModel : BindableBase
    {
        #region Properties

        private bool isManualEditCommit;

        public DataView ProductionHistory
        {
            get { return _multiPorosityModelService.ActiveProject.ProductionHistory; }
            set
            {
                _multiPorosityModelService.ActiveProject.ProductionHistory = value;

                RaisePropertyChanged(nameof(ProductionHistory));
            }
        }

        #region Commands

        public DelegateCommand ConnectToDatabaseCommand { get; }

        public DelegateCommand ImportCommand { get; }

        public DelegateCommand ExportCommand { get; }

        public DelegateCommand ConvertCommand { get; }

        public DelegateCommand<DataGrid> KeyDownCommand { get; }

        public DelegateCommand<IList<object>> KeyUpCommand { get; }
        
        public DelegateCommand<IList<object>> DeleteRowsCommand { get; }

        public DelegateCommand<IList<object>> PreviewKeyDownCommand { get; }

        public DelegateCommand<DataGridCellEditEndingEventArgs> CellEditEndingCommand { get; }

        public DelegateCommand<IList<DataGridCellInfo>> SelectedModelsCommand { get; }

        public DelegateCommand<DataGrid> HistoryView_DataGridCommand { get; }

        #endregion

        public BindableCollection<DataGridCellInfo> SelectedModels = new BindableCollection<DataGridCellInfo>();

        public DataGrid? HistoryView_DataGrid { get; private set; }

        #endregion Properties

        #region Routines

        internal readonly MultiPorosityModelService _multiPorosityModelService;

        private readonly IEventAggregator _eventAggregator;

        private readonly IDialogService _dialogService;

        public ProductionHistoryViewModel(IEventAggregator          eventAggregator,
                                          IDialogService            dialogService,
                                          MultiPorosityModelService multiPorosityModelService)
        {
            _eventAggregator           = eventAggregator;
            _dialogService             = dialogService;
            _multiPorosityModelService = multiPorosityModelService;

            _eventAggregator.GetEvent<WeightsChangedEvent>().Subscribe(SetSelectedWeights);

            KeyDownCommand = new DelegateCommand<DataGrid>(DataGrid_KeyDown);

            KeyUpCommand = new DelegateCommand<IList<object>>(DataGrid_KeyUp);

            DeleteRowsCommand = new DelegateCommand<IList<object>>(DataGrid_DeleteRows);

            CellEditEndingCommand = new DelegateCommand<DataGridCellEditEndingEventArgs>(DataGrid_CellEditEnding);

            SelectedModelsCommand = new DelegateCommand<IList<DataGridCellInfo>>(DataGrid_SelectedCells);

            HistoryView_DataGridCommand = new DelegateCommand<DataGrid>(SetDataGrid);

            ConnectToDatabaseCommand = new DelegateCommand(ConnectToDatabase);

            ImportCommand = new DelegateCommand(Import);

            ExportCommand = new DelegateCommand(Export);

            ConvertCommand = new DelegateCommand(Convert);

            _multiPorosityModelService.PropertyChanged -= OnPropertyChanged;
            _multiPorosityModelService.PropertyChanged += OnPropertyChanged;

            OnPropertyChanged(this, new PropertyChangedEventArgs("ActiveProject"));

            //IConnectableObservable<IChangeSet<ProductionHistoryModel>> obs = productionHistory
            //                                                                .Connect()
            //                                                                .ObserveOn(RxApp.MainThreadScheduler)
            //                                                                .Transform(model => new
            //                                                                               ProductionHistoryModel(model))
            //                                                                .Publish();

            //ProductionHistory = obs;

            //SelectedItems = obs.Filter(x => x.IsSelected);

            //obs.Connect();

            //ProductionHistory = new SourceList<ProductionHistoryModel>();

            //ProductionHistory = productionHistory;

            //List<HistoryModel> models = new List<HistoryModel>(_well.ProductionHistory.Records.Count);

            //for (int i = 0; i < _well.ProductionHistory.Records.Count; i++)
            //{
            //    models.Add(new HistoryModel()
            //    {
            //        Date = _well.ProductionHistory.Records[i],
            //        Gas = _well.ProductionHistory.Gas[i],
            //        Oil = _well.ProductionHistory.Oil[i],
            //        Water = _well.ProductionHistory.Water[i],
            //        THP = _well.ProductionHistory.THP[i],
            //        BHP = _well.ProductionHistory.BHP[i]
            //    });
            //}

            //HistoryModels = new BindingList<HistoryModel>(models);

            //isUpdating = false;

            //#if DEBUG
            //            //if (!MainWindowViewModel.LoadingFromFile)
            //            //{
            //            HistoryModels = new BindingList<HistoryModel>()
            //                {
            //                    new HistoryModel() {Index = 1, Date = DateTime.Now, Oil=200, Gas=1000, BHP=6000},
            //                    new HistoryModel() {Index = 2, Date = DateTime.Now.AddDays(1), Oil=190, Gas=1000, BHP=5900},
            //                    new HistoryModel() {Index = 3, Date = DateTime.Now.AddDays(2), Oil=180, Gas=900, BHP=5800},
            //                    new HistoryModel() {Index = 4, Date = DateTime.Now.AddDays(3), Oil=170, Gas=800, BHP=5700},
            //                    new HistoryModel() {Index = 5, Date = DateTime.Now.AddDays(4), Oil=160, Gas=700, BHP=5600}
            //                };
            //            //}
            //#else
            //            this.HistoryModels = new BindingList<HistoryModel>();
            //#endif

            //IsDirty = false;
        }

        private void SetSelectedWeights(double value)
        {
            double       selectedWeightsValue = value;
            DataRowView? row;

            if(HistoryView_DataGrid != null)
            {
                for(int i = 0; i < HistoryView_DataGrid.SelectedItems.Count; ++i)
                {
                    row = HistoryView_DataGrid.SelectedItems[i] as DataRowView;

                    if(row != null)
                    {
                        row[7] = selectedWeightsValue;
                    }
                }
            }
        }

        private void OnPropertyChanged(object?                  sender,
                                       PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case "ActiveProject":
                {
                    _multiPorosityModelService.ActiveProject.PropertyChanged                                       -= OnPropertyChanged;
                    _multiPorosityModelService.ActiveProject.PropertyChanged                                       += OnPropertyChanged;
                    _multiPorosityModelService.ActiveProject.SelectedProductionRecords.CollectionChanged           -= SelectedProductionRecords_CollectionChanged;
                    _multiPorosityModelService.ActiveProject.SelectedProductionRecords.CollectionChanged           += SelectedProductionRecords_CollectionChanged;
                    _multiPorosityModelService.ActiveProject.SelectedCumulativeProductionRecords.CollectionChanged -= SelectedCumulativeProductionRecords_CollectionChanged;
                    _multiPorosityModelService.ActiveProject.SelectedCumulativeProductionRecords.CollectionChanged += SelectedCumulativeProductionRecords_CollectionChanged;
                    RaisePropertyChanged(nameof(ProductionHistory));

                    break;
                }
                case "ProductionHistory":
                {
                    RaisePropertyChanged(nameof(ProductionHistory));

                    break;
                }
                case "SelectedProductionRecords":
                {
                    HistoryView_DataGrid.SelectedIndex = -1;
                    HistoryView_DataGrid.SelectedItems.Clear();

                    DataRowView row;

                    foreach(ProductionRecord record in _multiPorosityModelService.ActiveProject.SelectedProductionRecords)
                    {
                        for(int i = 0; i < _multiPorosityModelService.ActiveProject.ProductionDataSet.Actual.Count; ++i)
                        {
                            if((DateTime)_multiPorosityModelService.ActiveProject.ProductionDataSet.Actual[i][1] == record.Date)
                            {
                                row = (DataRowView)HistoryView_DataGrid.Items[i];

                                HistoryView_DataGrid.SelectedItems.Add(row);

                                break;
                            }
                        }
                    }

                    if(HistoryView_DataGrid.SelectedItems.Count > 0)
                    {
                        HistoryView_DataGrid.SelectedItem = HistoryView_DataGrid.SelectedItems[0];
                        HistoryView_DataGrid.UpdateLayout();

                        if(HistoryView_DataGrid.SelectedItem != null)
                        {
                            HistoryView_DataGrid.ScrollIntoView(HistoryView_DataGrid.SelectedItem);
                        }
                    }

                    break;
                }
                case "SelectedCumulativeProductionRecords":
                {
                    HistoryView_DataGrid.SelectedIndex = -1;
                    HistoryView_DataGrid.SelectedItems.Clear();

                    DataRowView row;

                    foreach(CumulativeProductionRecord record in _multiPorosityModelService.ActiveProject.SelectedCumulativeProductionRecords)
                    {
                        for(int i = 0; i < _multiPorosityModelService.ActiveProject.ProductionDataSet.Actual.Count; ++i)
                        {
                            if((DateTime)_multiPorosityModelService.ActiveProject.ProductionDataSet.Actual[i][1] == record.Date)
                            {
                                row = (DataRowView)HistoryView_DataGrid.Items[i];

                                HistoryView_DataGrid.SelectedItems.Add(row);

                                break;
                            }
                        }
                    }
                    
                    if(HistoryView_DataGrid.SelectedItems.Count > 0)
                    {
                        HistoryView_DataGrid.SelectedItem = HistoryView_DataGrid.SelectedItems[0];
                        HistoryView_DataGrid.UpdateLayout();

                        if(HistoryView_DataGrid.SelectedItem != null)
                        {
                            HistoryView_DataGrid.ScrollIntoView(HistoryView_DataGrid.SelectedItem);
                        }
                    }

                    break;
                }
            }
        }

        private void SelectedProductionRecords_CollectionChanged(object?                          sender,
                                                                 NotifyCollectionChangedEventArgs e)
        {
            if(HistoryView_DataGrid is null)
            {
                return;
            }

            //if (sender is BindableCollection<DataPointInfo> dataPoints && dataPoints.Count == 0)
            //{
            //    HistoryView_DataGrid.SelectedItems.Clear();

            //    return;
            //}

            DataRowView row;

            switch(e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    if(e.NewItems != null)
                    {
                        //List<int> rowIndices = new (e.NewItems.Count);

                        foreach(ProductionRecord record in e.NewItems)
                        {
                            for(int i = 0; i < _multiPorosityModelService.ActiveProject.ProductionDataSet.Actual.Count; ++i)
                            {
                                if((DateTime)_multiPorosityModelService.ActiveProject.ProductionDataSet.Actual[i][1] == record.Date)
                                {
                                    row = (DataRowView)HistoryView_DataGrid.Items[i];

                                    //rowIndices.Add(i);

                                    //SelectRowByIndex(HistoryView_DataGrid, i);

                                    HistoryView_DataGrid.SelectedItems.Add(row);

                                    break;
                                }
                            }
                        }

                        //SelectRowByIndex(HistoryView_DataGrid, rowIndices);
                    }

                    break;
                }
                case NotifyCollectionChangedAction.Reset:
                {
                    //DataGridCell? headerCell;
                    //object?       rowItem;

                    //for(int i = 0; i < HistoryView_DataGrid.SelectedItems.Count; ++i)
                    //{
                    //    rowItem = HistoryView_DataGrid.SelectedItems[i];

                    //    if(rowItem is DataGridRow dataGridRow)
                    //    {
                    //        headerCell = GetCell(HistoryView_DataGrid, dataGridRow, 0);

                    //        if(headerCell != null && headerCell.IsSelected)
                    //        {
                    //            headerCell.IsSelected = false;
                    //        }
                    //    }
                    //    else if(rowItem is DataRowView dataRowView && dataRowView.Row is ProductionDataSet.ActualRow monthlyRow)
                    //    {
                    //        DataGridRow? _dataGridRow = HistoryView_DataGrid.ItemContainerGenerator.ContainerFromIndex(monthlyRow.Index) as DataGridRow;

                    //        if(_dataGridRow != null)
                    //        {
                    //            headerCell = GetCell(HistoryView_DataGrid, _dataGridRow, 0);

                    //            if(headerCell != null && headerCell.IsSelected)
                    //            {
                    //                headerCell.IsSelected = false;
                    //            }
                    //        }
                    //    }
                    //}

                    HistoryView_DataGrid.SelectedIndex = -1;
                    //HistoryView_DataGrid.UnselectAll();
                    //HistoryView_DataGrid.UnselectAllCells();
                    //HistoryView_DataGrid.SelectedCells.Clear();
                    HistoryView_DataGrid.SelectedItems.Clear();

                    break;
                }
                case NotifyCollectionChangedAction.Remove:
                {
                    if(e.OldItems != null)
                    {
                        foreach(ProductionRecord record in e.OldItems)
                        {
                            for(int i = 0; i < _multiPorosityModelService.ActiveProject.ProductionDataSet.Actual.Count; ++i)
                            {
                                if((DateTime)_multiPorosityModelService.ActiveProject.ProductionDataSet.Actual[i][1] == record.Date)
                                {
                                    row = (DataRowView)HistoryView_DataGrid.Items[i];

                                    HistoryView_DataGrid.SelectedItems.Remove(row);

                                    break;
                                }
                            }
                        }
                    }

                    break;
                }
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Move:
                default: throw new ApplicationException();
            }

            //if (sender is BindableCollection<DataPointInfo> dataPointInfos)
            //{
            //    foreach (DataPointInfo? dataPointInfo in dataPointInfos)
            //    {
            //        if (DaysToDateMap.TryGetValue((double)dataPointInfo.XValue, out DateTime dateTime))
            //        {
            //            for (int i = 0; i < ProductionDataSet.Actual.Count; ++i)
            //            {
            //                if ((DateTime)ProductionDataSet.Actual[i][1] == dateTime)
            //                {
            //                    System.Data.DataRowView row = (System.Data.DataRowView)HistoryView_DataGrid.Items[i];

            //                    HistoryView_DataGrid.SelectedItems.Add(row);

            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}

            //double gas;
            //double oil;
            //double weight;

            //for (int i = 0; i < ProductionDataSet.Actual.Count; ++i)
            //{
            //    //gas = (double)dataSet.Actual[i][1];
            //    //oil = (double)dataSet.Actual[i][2];
            //    //weight = (double)dataSet.Actual[i][5];

            //}
        }

        private void SelectedCumulativeProductionRecords_CollectionChanged(object?                          sender,
                                                                           NotifyCollectionChangedEventArgs e)
        {
            if(HistoryView_DataGrid is null)
            {
                return;
            }

            //if (sender is BindableCollection<DataPointInfo> dataPoints && dataPoints.Count == 0)
            //{
            //    HistoryView_DataGrid.SelectedItems.Clear();

            //    return;
            //}

            DataRowView row;

            switch(e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    if(e.NewItems != null)
                    {
                        //List<int> rowIndices = new (e.NewItems.Count);

                        foreach(CumulativeProductionRecord record in e.NewItems)
                        {
                            for(int i = 0; i < _multiPorosityModelService.ActiveProject.ProductionDataSet.Actual.Count; ++i)
                            {
                                if((DateTime)_multiPorosityModelService.ActiveProject.ProductionDataSet.Actual[i][1] == record.Date)
                                {
                                    row = (DataRowView)HistoryView_DataGrid.Items[i];

                                    //rowIndices.Add(i);

                                    //SelectRowByIndex(HistoryView_DataGrid, i);

                                    HistoryView_DataGrid.SelectedItems.Add(row);

                                    break;
                                }
                            }
                        }

                        //SelectRowByIndex(HistoryView_DataGrid, rowIndices);
                    }

                    break;
                }
                case NotifyCollectionChangedAction.Reset:
                {
                    //DataGridCell? headerCell;
                    //object?       rowItem;

                    //for(int i = 0; i < HistoryView_DataGrid.SelectedItems.Count; ++i)
                    //{
                    //    rowItem = HistoryView_DataGrid.SelectedItems[i];

                    //    if(rowItem is DataGridRow dataGridRow)
                    //    {
                    //        headerCell = GetCell(HistoryView_DataGrid, dataGridRow, 0);

                    //        if(headerCell != null && headerCell.IsSelected)
                    //        {
                    //            headerCell.IsSelected = false;
                    //        }
                    //    }
                    //    else if(rowItem is DataRowView dataRowView && dataRowView.Row is ProductionDataSet.ActualRow monthlyRow)
                    //    {
                    //        DataGridRow? _dataGridRow = HistoryView_DataGrid.ItemContainerGenerator.ContainerFromIndex(monthlyRow.Index) as DataGridRow;

                    //        if(_dataGridRow != null)
                    //        {
                    //            headerCell = GetCell(HistoryView_DataGrid, _dataGridRow, 0);

                    //            if(headerCell != null && headerCell.IsSelected)
                    //            {
                    //                headerCell.IsSelected = false;
                    //            }
                    //        }
                    //    }
                    //}

                    HistoryView_DataGrid.SelectedIndex = -1;
                    //HistoryView_DataGrid.UnselectAll();
                    //HistoryView_DataGrid.UnselectAllCells();
                    //HistoryView_DataGrid.SelectedCells.Clear();
                    HistoryView_DataGrid.SelectedItems.Clear();

                    break;
                }
                case NotifyCollectionChangedAction.Remove:
                {
                    if(e.OldItems != null)
                    {
                        foreach(CumulativeProductionRecord record in e.OldItems)
                        {
                            for(int i = 0; i < _multiPorosityModelService.ActiveProject.ProductionDataSet.Actual.Count; ++i)
                            {
                                if((DateTime)_multiPorosityModelService.ActiveProject.ProductionDataSet.Actual[i][1] == record.Date)
                                {
                                    row = (DataRowView)HistoryView_DataGrid.Items[i];

                                    HistoryView_DataGrid.SelectedItems.Remove(row);

                                    break;
                                }
                            }
                        }
                    }

                    break;
                }
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Move:
                default: throw new ApplicationException();
            }

            //if (sender is BindableCollection<DataPointInfo> dataPointInfos)
            //{
            //    foreach (DataPointInfo? dataPointInfo in dataPointInfos)
            //    {
            //        if (DaysToDateMap.TryGetValue((double)dataPointInfo.XValue, out DateTime dateTime))
            //        {
            //            for (int i = 0; i < ProductionDataSet.Actual.Count; ++i)
            //            {
            //                if ((DateTime)ProductionDataSet.Actual[i][1] == dateTime)
            //                {
            //                    System.Data.DataRowView row = (System.Data.DataRowView)HistoryView_DataGrid.Items[i];

            //                    HistoryView_DataGrid.SelectedItems.Add(row);

            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}

            //double gas;
            //double oil;
            //double weight;

            //for (int i = 0; i < ProductionDataSet.Actual.Count; ++i)
            //{
            //    //gas = (double)dataSet.Actual[i][1];
            //    //oil = (double)dataSet.Actual[i][2];
            //    //weight = (double)dataSet.Actual[i][5];

            //}
        }

        private void ConnectToDatabase()
        {
             DialogParameters parameters = new ();
            
            _dialogService.ShowDialog(RegionNames.ConnectToDatabase, parameters, ConnectToDatabaseResult);
        }

        private void ConnectToDatabaseResult(IDialogResult dialogResult)
        {
            if(dialogResult.Result == ButtonResult.OK)
            {

            }
        }

        private void Import()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect      = false;
            openFileDialog.Filter           = "Csv file (*.csv)|*.csv|Excel file (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = _multiPorosityModelService.RepositoryPath ?? Environment.GetFolderPath(Environment.SpecialFolder.Recent);

            if(openFileDialog.ShowDialog() == true && File.Exists(openFileDialog.FileName))
            {
                _multiPorosityModelService.ActiveProject.ProductionDataSet = DataSources.ImportCsv(openFileDialog.FileName);
            }
        }

        private void Export()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter           = "Csv file (*.csv)|*.csv|Excel file (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveFileDialog.InitialDirectory = _multiPorosityModelService.RepositoryPath ?? Environment.GetFolderPath(Environment.SpecialFolder.Recent);

            if(saveFileDialog.ShowDialog() == true)
            {
                switch(Path.GetExtension(saveFileDialog.FileName))
                {
                    case ".csv":
                    {
                        DataSources.ExportCsv(_multiPorosityModelService.ActiveProject.ProductionDataSet.Actual, saveFileDialog.FileName);
                        MessageBox.Show($"{saveFileDialog.FileName} saved.");

                        break;
                    }
                    case ".xlsx":
                    {
                        DataSources.ExporXlsx(_multiPorosityModelService.ActiveProject.ProductionDataSet.Actual, saveFileDialog.FileName);
                        MessageBox.Show($"{saveFileDialog.FileName} saved.");

                        break;
                    }
                    default:
                    {
                        MessageBox.Show("Invalid file format.");

                        break;
                    }
                }
            }
        }

        private void Convert()
        {
            _multiPorosityModelService.ActiveProject.UpdateProductionDataset(new(ProductionService.ConvertMonthlyToDaily(_multiPorosityModelService.ActiveProject.ProductionRecords.ToList())));
        }

        public void UpdateIndex()
        {
            //List<ProductionRecord> _productionHistory = new List<ProductionRecord>(productionHistory.Count);

            //try
            //{
            //    ProductionHistoryModel[] items = productionHistory.Items.ToArray();

            //    for(int i = 0; i < productionHistory.Count; i++)
            //    {
            //        _productionHistory.Add(new ProductionRecord(items[i])
            //        {
            //            Index = i
            //        });
            //    }
            //}
            //catch
            //{
            //    // ignored
            //}
            //finally
            //{
            //    productionHistory.Clear();

            //    foreach(ProductionRecord item in _productionHistory)
            //    {
            //        productionHistory.Add(new ProductionHistoryModel(item));
            //    }

            //    //IsDirty = true;

            //    this.RaisePropertyChanged(nameof(ProductionHistory));
            //}
        }

        public void SetDataGrid(DataGrid e)
        {
            HistoryView_DataGrid ??= e;
        }

        #endregion Routines

        #region Event Methods

        private void DataGrid_SelectedCells(IList<DataGridCellInfo> e)
        {
            SelectedModels = new BindableCollection<DataGridCellInfo>(e);
        }

        private void DataGrid_KeyUp(IList<object> e)
        {
            //foreach (object item in e)
            //{

            //}
            UpdateIndex();
        }

        private void DataGrid_DeleteRows(IList<object> e)
        {
            //for (int i = e.SelectedItems.Count - 1; i >= 0; --i)
            //{
            //    if(e.SelectedItems[i] is ProductionHistoryModel model)
            //    {
            //        productionHistory.Remove(model);
            //    }
            //}
            UpdateIndex();
        }

        private void DataGrid_KeyDown(DataGrid? e)
        {
            if(e != null && !isManualEditCommit) // && SelectedModels != null && SelectedModels.Count > 0)
            {
                isManualEditCommit = true;

                HistoryView_DataGrid ??= e;

                if(Clipboard.ContainsData(DataFormats.Text))
                {
                    //List<ProductionRecord> _productionHistory = new List<ProductionRecord>(productionHistory.Count);

                    //foreach (ProductionHistoryModel item in productionHistory.Items)
                    //{
                    //    _productionHistory.Add(new ProductionRecord(item));
                    //}

                    List<ProductionRecord>? _productionHistory = null;

                    int startRow            = 0;
                    int startCol            = 0;
                    int clipboardDataLength = 0;

                    try
                    {
                        // 2-dim array containing clipboard data
                        string[][] clipboardData = ((string)Clipboard.GetData(DataFormats.Text)).Split('\n').
                                                                                                 Select(row => row.Split('\t').
                                                                                                                   Select(cell => cell.Length > 0 && cell[^1] == '\r'
                                                                                                                                      ? cell.Substring(0, cell.Length - 1)
                                                                                                                                      : cell).ToArray()).Where(a => a.Any(b => b.Length > 0)).ToArray();

                        //if(ProductionHistory.Count == 0 && HistoryView_DataGrid.CurrentItem is DataRowView dataRowView && dataRowView.IsNew)
                        //{
                        //    startRow = 0;
                        //}
                        //else 
                        if(HistoryView_DataGrid.SelectedItems.Count > 0)
                        {
                            startRow =
                                HistoryView_DataGrid.ItemContainerGenerator.IndexFromContainer((DataGridRow)HistoryView_DataGrid.ItemContainerGenerator.ContainerFromItem(HistoryView_DataGrid.
                                                                                                   SelectedCells[0].Item));
                        }

                        startCol = HistoryView_DataGrid.SelectedCells[0].Column.DisplayIndex + 1;

                        clipboardDataLength = clipboardData.Length;

                        _productionHistory = new List<ProductionRecord>(clipboardDataLength);

                        for(int i = 0; i < clipboardDataLength; i++)
                        {
                            _productionHistory.Add(new ProductionRecord());
                        }

                        for(int rowIndex = 0; rowIndex < clipboardDataLength; rowIndex++)
                        {
                            string[] rowContent = clipboardData[rowIndex - startRow];
                            int      cols       = rowContent.Length;

                            for(int colIndex = startCol; colIndex < startCol + cols; colIndex++)
                            {
                                string cellContent = rowContent[colIndex - startCol];

                                if(cellContent.Length > 0)
                                {
                                    _productionHistory[rowIndex][colIndex] = cellContent;
                                }
                            }
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                    finally
                    {
                        if(_productionHistory != null)
                        {
                            for(int rowIndex = startRow; rowIndex < clipboardDataLength + startRow; rowIndex++)
                            {
                                if(rowIndex > _multiPorosityModelService.ActiveProject.ProductionDataSet.Actual.Count)
                                {
                                    _multiPorosityModelService.ActiveProject.ProductionDataSet.Actual.AddActualRow(rowIndex,
                                                                                                                   _productionHistory[rowIndex].Date,
                                                                                                                   _productionHistory[rowIndex].Days,
                                                                                                                   _productionHistory[rowIndex].Gas,
                                                                                                                   _productionHistory[rowIndex].Oil,
                                                                                                                   _productionHistory[rowIndex].Water,
                                                                                                                   _productionHistory[rowIndex].WellheadPressure,
                                                                                                                   _productionHistory[rowIndex].Weight);
                                }
                            }
                        }

                        //productionHistory.Clear();

                        //foreach (ProductionRecord item in _productionHistory)
                        //{
                        //    productionHistory.Add(new ProductionHistoryModel(item));
                        //}

                        //foreach(ProductionRecord item in _productionHistory)
                        //{
                        //    productionHistory.Add(new ProductionHistoryModel(item));
                        //}

                        //IsDirty = true;
                    }

                    UpdateIndex();
                }

                isManualEditCommit = false;
            }
        }

        private void DataGrid_CellEditEnding(DataGridCellEditEndingEventArgs e)
        {
            if(!isManualEditCommit && HistoryView_DataGrid != null)
            {
                isManualEditCommit = true;
                HistoryView_DataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                isManualEditCommit = false;
            }
        }

        #endregion Event Methods

        //private DataGridCell GetCellNearMouse()
        //{
        //    Panel itemsHost = InternalItemsHost;
        //    if (itemsHost != null)
        //    {
        //        Rect itemsHostBounds = new Rect(new Point(), itemsHost.RenderSize);
        //        double closestDistance = Double.PositiveInfinity;
        //        DataGridCell closestCell = null;
        //        bool isMouseInCorner = IsMouseInCorner(RelativeMousePosition);
        //        bool isGrouping = IsGrouping;

        //        // Iterate from the end to the beginning since it is more common
        //        // to drag toward the end.
        //        for (int i = (isGrouping ? Items.Count - 1 : itemsHost.Children.Count - 1); i >= 0; i--)
        //        {
        //            DataGridRow row = null;

        //            if (isGrouping)
        //            {
        //                // If Grouping is enabled, Children of itemsHost are not always DataGridRows
        //                row = ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
        //            }
        //            else
        //            {
        //                row = itemsHost.Children[i] as DataGridRow;
        //            }

        //            if (row != null)
        //            {
        //                DataGridCellsPresenter cellsPresenter = row.CellsPresenter;
        //                if (cellsPresenter != null)
        //                {
        //                    // Go through all of the instantiated cells and find the closest cell
        //                    ContainerTracking<DataGridCell> cellTracker = cellsPresenter.CellTrackingRoot;
        //                    while (cellTracker != null)
        //                    {
        //                        DataGridCell cell = cellTracker.Container;

        //                        double cellDistance;
        //                        if (CalculateCellDistance(cell, row, itemsHost, itemsHostBounds, isMouseInCorner, out cellDistance))
        //                        {
        //                            if ((closestCell == null) || (cellDistance < closestDistance))
        //                            {
        //                                // This cell's distance is less, so make it the closest cell
        //                                closestDistance = cellDistance;
        //                                closestCell = cell;
        //                            }
        //                        }

        //                        cellTracker = cellTracker.Next;
        //                    }

        //                    // Check if the header is close
        //                    DataGridRowHeader rowHeader = row.RowHeader;
        //                    if (rowHeader != null)
        //                    {
        //                        double cellDistance;
        //                        if (CalculateCellDistance(rowHeader, row, itemsHost, itemsHostBounds, isMouseInCorner, out cellDistance))
        //                        {
        //                            if ((closestCell == null) || (cellDistance < closestDistance))
        //                            {
        //                                // If the header is the closest, then use the first cell from the row
        //                                DataGridCell cell = row.TryGetCell(DisplayIndexMap[0]);
        //                                if (cell != null)
        //                                {
        //                                    closestDistance = cellDistance;
        //                                    closestCell = cell;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        return closestCell;
        //    }

        //    return null;
        //}

        #region Static Methods

        public static void SelectRowByIndex(DataGrid dataGrid,
                                            int      rowIndex)
        {
            //if(!dataGrid.SelectionUnit.Equals(DataGridSelectionUnit.FullRow))
            //{
            //    throw new ArgumentException("The SelectionUnit of the DataGrid must be set to FullRow.");
            //}

            if(rowIndex < 0 || rowIndex > dataGrid.Items.Count - 1)
            {
                throw new ArgumentException($"{rowIndex} is an invalid row index.");
            }

            //dataGrid.SelectedItems.Clear();

            /* set the SelectedItem property */
            object item = dataGrid.Items[rowIndex]; // = Product X
            //dataGrid.SelectedItem = item;

            DataGridRow? row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;

            if(row == null)
            {
                dataGrid.ScrollIntoView(item);
                row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            }

            if(row != null)
            {
                DataGridCell? cell = GetCell(dataGrid, row, 0);

                cell?.Focus();
            }
        }

        public static void SelectRowByIndex(DataGrid  dataGrid,
                                            List<int> rowIndices)
        {
            //if(!dataGrid.SelectionUnit.Equals(DataGridSelectionUnit.FullRow))
            //{
            //    throw new ArgumentException("The SelectionUnit of the DataGrid must be set to FullRow.");
            //}

            //dataGrid.SelectedItems.Clear();

            object        item;
            DataGridRow?  row;
            DataGridCell? cell;

            for(int i = 0; i < rowIndices.Count; ++i)
            {
                item = dataGrid.Items[rowIndices[i]];
                //dataGrid.SelectedItems.Add(item);

                row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndices[i]) as DataGridRow;

                if(row == null)
                {
                    dataGrid.ScrollIntoView(item);
                    row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndices[i]) as DataGridRow;
                }

                if(row != null)
                {
                    cell = GetCell(dataGrid, row, 0);

                    cell?.Focus();
                }
            }
        }

        public static DataGridCell? GetCell(DataGrid     dataGrid,
                                            DataGridRow? rowContainer,
                                            int          column)
        {
            if(rowContainer != null)
            {
                DataGridCellsPresenter? presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);

                if(presenter == null)
                {
                    rowContainer.ApplyTemplate();

                    presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
                }

                if(presenter != null)
                {
                    if(presenter.ItemContainerGenerator.ContainerFromIndex(column) is DataGridCell cell)
                    {
                        return cell;
                    }

                    dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[column]);

                    return presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                }
            }

            return null;
        }

        public static T? FindVisualChild<T>(DependencyObject obj)
            where T : DependencyObject
        {
            DependencyObject? child;
            T?                childOfChild;

            for(int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if(child is T o)
                {
                    return o;
                }

                childOfChild = FindVisualChild<T>(child);

                if(childOfChild != null)
                {
                    return childOfChild;
                }
            }

            return null;
        }

        #endregion
    }
}