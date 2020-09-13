#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reactive;
using System.Windows;
using System.Windows.Controls;

using ReactiveUI;

using SciChart.Charting.Model.DataSeries;

namespace MultiPorosity.Tool
{
    public sealed class ProductionHistoryViewModel : ReactiveObject
    {
        #region Properties

        private bool isManualEditCommit;

        //private List<ProductionHistoryModel> productionHistory = new List<ProductionHistoryModel>();

        //public ObservableCollection<ProductionHistoryModel> ProductionHistory
        //{
        //    get
        //    {
        //        return new ObservableCollection<ProductionHistoryModel>(productionHistory);
        //    }
        //    set
        //    {
        //        this.RaiseAndSetIfChanged(ref productionHistory, value);

        //        if(productionHistory != null)
        //        {
        //            ProductionHistory.CollectionChanged -= (obj, evt) =>
        //                                                   {
        //                                                       this.RaisePropertyChanged(nameof(ProductionHistory));

        //                                                       foreach(var item in ProductionHistory)
        //                                                       {
        //                                                           item.PropertyChanged -= (o, e) => this.RaisePropertyChanged(nameof(ProductionHistory));
        //                                                           item.PropertyChanged += (o, e) => this.RaisePropertyChanged(nameof(ProductionHistory));
        //                                                       }
        //                                                   };

        //            ProductionHistory.CollectionChanged += (obj, evt) =>
        //                                                   {
        //                                                       this.RaisePropertyChanged(nameof(ProductionHistory));

        //                                                       foreach(var item in ProductionHistory)
        //                                                       {
        //                                                           item.PropertyChanged -= (o, e) => this.RaisePropertyChanged(nameof(ProductionHistory));
        //                                                           item.PropertyChanged += (o, e) => this.RaisePropertyChanged(nameof(ProductionHistory));
        //                                                       }
        //                                                   };

        //            foreach(var item in ProductionHistory)
        //            {
        //                item.PropertyChanged -= (o, e) => this.RaisePropertyChanged(nameof(ProductionHistory));
        //                item.PropertyChanged += (o, e) => this.RaisePropertyChanged(nameof(ProductionHistory));
        //            }
        //        }
        //    }
        //}

        private ProductionDataSet dataSet;

        public ProductionDataSet ProductionDataSet { get { return dataSet; } set { this.RaiseAndSetIfChanged(ref dataSet, value); } }

        public DataView ProductionHistory { get; set; }

        public class PointMetadata : ReactiveObject, IPointMetadata
        {
            private bool _IsSelected;

            public bool IsSelected { get { return _IsSelected; } set { this.RaiseAndSetIfChanged(ref _IsSelected, value); } }
        }

        private XyDataSeries<double, double> gasPoints = new XyDataSeries<double, double>
        {
            SeriesName = "Gas"
        };

        public ref XyDataSeries<double, double> GasPoints { get { return ref gasPoints; } }

        private XyDataSeries<double, double> oilPoints = new XyDataSeries<double, double>
        {
            SeriesName = "Oil"
        };

        public ref XyDataSeries<double, double> OilPoints { get { return ref oilPoints; } }

        public Dictionary<double, DateTime> DaysToDateMap { get; set; } = new Dictionary<double, DateTime>();

        private double maxGas;

        public double MaxGas { get { return maxGas; } set { this.RaiseAndSetIfChanged(ref maxGas, value); } }

        private double maxOil;

        public double MaxOil { get { return maxOil; } set { this.RaiseAndSetIfChanged(ref maxOil, value); } }

        //private readonly SourceList<ProductionHistoryModel> productionHistory = new SourceList<ProductionHistoryModel>();

        //public IObservableList<ProductionHistoryModel> ProductionHistory { get; set; }

        //public IObservable<IChangeSet<ProductionHistoryModel>> SelectedItems { get; }

        public int HistoryCount
        {
            get { return ProductionHistory.Count; }
            //set
            //{
            //    //if(productionHistory.Count < value)
            //    //{
            //    //    int num = productionHistory.Count;

            //    //    for(int i = 0; i < value - num; i++)
            //    //    {
            //    //        productionHistory.Add(new ProductionHistoryModel());
            //    //    }
            //    //}
            //    //else if(productionHistory.Count > value)
            //    //{
            //    //    int num = productionHistory.Count;

            //    //    for(int i = num - value; i > 0; i--)
            //    //    {
            //    //        productionHistory.RemoveAt(productionHistory.Count - i);
            //    //    }
            //    //}

            //    //if (ProductionHistory.Records != null)
            //    //{
            //    //    ProductionHistory.Records.ListChanged -= (o, e) => RaisePropertyChanged(nameof(HistoryModels));
            //    //    ProductionHistory.Records.ListChanged += (o, e) => RaisePropertyChanged(nameof(HistoryModels));
            //    //}

            //    UpdateIndex();
            //}
        }

        #region Commands

        public ReactiveCommand<DataGrid, Unit> KeyDownCommand { get; }

        public ReactiveCommand<IList<object>, Unit> KeyUpCommand { get; }

        public ReactiveCommand<DataGrid, Unit> DeleteRowsCommand { get; }

        public ReactiveCommand<DataGridCellEditEndingEventArgs, Unit> CellEditEndingCommand { get; }

        public ReactiveCommand<IList<DataGridCellInfo>, Unit> SelectedModelsCommand { get; }

        public ReactiveCommand<DataGrid, Unit> HistoryView_DataGridCommand { get; }

        #endregion

        public ObservableCollection<DataGridCellInfo> SelectedModels = new ObservableCollection<DataGridCellInfo>();

        public DataGrid? HistoryView_DataGrid;

        #endregion Properties

        #region Routines

        public ProductionHistoryViewModel()
        {
            KeyDownCommand = ReactiveCommand.Create<DataGrid>(DataGrid_KeyDown);

            KeyUpCommand = ReactiveCommand.Create<IList<object>>(DataGrid_KeyUp);

            DeleteRowsCommand = ReactiveCommand.Create<DataGrid>(DataGrid_DeleteRows);

            CellEditEndingCommand = ReactiveCommand.Create<DataGridCellEditEndingEventArgs>(DataGrid_CellEditEnding);

            SelectedModelsCommand = ReactiveCommand.Create<IList<DataGridCellInfo>>(DataGrid_SelectedCells);

            HistoryView_DataGridCommand = ReactiveCommand.Create<DataGrid>(SetDataGrid);

            dataSet           = new ProductionDataSet();
            ProductionHistory = dataSet.Monthly.DefaultView;

            ProductionHistory.ListChanged += OnDataViewChanged;

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

        private void OnDataViewChanged(object               sender,
                                       ListChangedEventArgs e)
        {
            GasPoints.Clear();
            OilPoints.Clear();
            DaysToDateMap.Clear();

            double days = 15;
            double gas;
            double oil;
            double weight;

            maxOil = double.MinValue;

            for(int i = 0; i < dataSet.Monthly.Count; ++i)
            {
                gas    = (double)dataSet.Monthly[i][1];
                oil    = (double)dataSet.Monthly[i][2];
                weight = (double)dataSet.Monthly[i][5];

                DaysToDateMap.Add(days, (DateTime)dataSet.Monthly[i][0]);

                GasPoints.Append(days,
                                 gas,
                                 new PointMetadata
                                 {
                                     IsSelected = false
                                 });

                OilPoints.Append(days,
                                 oil,
                                 new PointMetadata
                                 {
                                     IsSelected = false
                                 });

                if(maxGas < gas)
                {
                    maxGas = gas;
                }

                if(maxOil < oil)
                {
                    maxOil = oil;
                }

                days += 30;
            }

            MaxGas = Math.Round(maxGas / 100.0, 0) * 100.0;

            MaxOil = Math.Round(maxOil / 100.0, 0) * 100.0;

            this.RaisePropertyChanged(nameof(HistoryCount));
            this.RaisePropertyChanged(nameof(GasPoints));
            this.RaisePropertyChanged(nameof(OilPoints));
        }

        //public Task New()
        //{
        //    _well.ProductionHistory = new ProductionHistory();

        //    return Task.CompletedTask;
        //}

        //public Task Load(Well well)
        //{
        //    _well = well;
        //    _well.ProductionHistory = well.ProductionHistory;

        //    return Task.CompletedTask;
        //}

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

        private void SetDataGrid(DataGrid e)
        {
            //HistoryView_DataGrid = e;
        }

        #endregion Routines

        #region Event Methods

        private void DataGrid_SelectedCells(IList<DataGridCellInfo> e)
        {
            SelectedModels = new ObservableCollection<DataGridCellInfo>(e);
        }

        private void DataGrid_KeyUp(IList<object> e)
        {
            //foreach (object item in e)
            //{

            //}
            UpdateIndex();
        }

        private void DataGrid_DeleteRows(DataGrid e)
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

                HistoryView_DataGrid = e;

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
                                                                                                                   Select(cell => cell.Length > 0 && cell[cell.Length - 1] == '\r'
                                                                                                                              ? cell.Substring(0, cell.Length - 1)
                                                                                                                              : cell).ToArray()).Where(a => a.Any(b => b.Length > 0)).
                                                                                                 ToArray();

                        //if(ProductionHistory.Count == 0 && HistoryView_DataGrid.CurrentItem is DataRowView dataRowView && dataRowView.IsNew)
                        //{
                        //    startRow = 0;
                        //}
                        //else 
                        if(HistoryView_DataGrid.SelectedItems.Count > 0)
                        {
                            startRow =
                                HistoryView_DataGrid.ItemContainerGenerator.IndexFromContainer((DataGridRow)
                                                                                               HistoryView_DataGrid.ItemContainerGenerator.ContainerFromItem(HistoryView_DataGrid.
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
                                if(rowIndex > dataSet.Monthly.Count)
                                {
                                    dataSet.Monthly.AddMonthlyRow(_productionHistory[rowIndex].Date,
                                                                  _productionHistory[rowIndex].GasVolume,
                                                                  _productionHistory[rowIndex].OilVolume,
                                                                  _productionHistory[rowIndex].WaterVolume,
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
    }
}