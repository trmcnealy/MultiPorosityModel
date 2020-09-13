#nullable enable
using ReactiveUI;

using Splat;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Reactive;
using System.Windows.Media;
using System.Windows.Shapes;

using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Utility;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core.Utility.Mouse;
using SciChart.Data.Model;
using SciChart.Drawing.Utility;

using Engineering.UI;

using SciChart.Charting.Numerics.CoordinateCalculators;
using SciChart.Charting.Visuals.Axes;
using SciChart.Drawing.Common;

namespace MultiPorosity.Tool
{
    public interface IMainViewModel
    {
        MultiPorositySettingsViewModel MultiPorositySettingsViewModel { get; set; }

        ProductionHistoryViewModel ProductionHistoryViewModel { get; set; }

        MultiPorosityResultsViewModel MultiPorosityResultsViewModel { get; set; }
    }

    public class MainViewModel : ReactiveObject, IMainViewModel
    {
        private static double[] LogLogMaxes =
        {
            0.001, 0.01, 0.1, 1.0, 10.0, 100.0, 1000.0, 10000.0, 100000.0
        };

        private ObservableCollection<DataPointInfo>? _SelectedPointMarkers;
        private ObservableCollection<DataPointInfo>? _SelectedLogLogPointMarkers;

        private string _Title = "Production";

        private MultiPorositySettingsViewModel _MultiPorositySettingsViewModel;

        private ProductionHistoryViewModel _ProductionHistoryViewModel;

        private MultiPorosityResultsViewModel _MultiPorosityResultsViewModel;

        private Range<double> _XVisibleRange = new DoubleRange(0.0, 100.0);

        private Range<double> _XLogLogVisibleRange = new DoubleRange(1.0, 100.0);

        private Range<double> _YVisibleRange       = new DoubleRange(0.0, 100.0);
        private Range<double> _YLogLogVisibleRange = new DoubleRange(1.0, 100.0);


        public string Title { get { return _Title; } set { this.RaiseAndSetIfChanged(ref _Title, value); } }

        public XyDataSeries<double, double> GasPoints { get { return ProductionHistoryViewModel.GasPoints; } }

        public XyDataSeries<double, double> OilPoints { get { return ProductionHistoryViewModel.OilPoints; } }
        
        public Range<double> XVisibleRange { get { return _XVisibleRange; } set { this.RaiseAndSetIfChanged(ref _XVisibleRange, value); } }
        public Range<double> XLogLogVisibleRange { get { return _XLogLogVisibleRange; } set { this.RaiseAndSetIfChanged(ref _XLogLogVisibleRange, value); } }

        
        public Range<double> YVisibleRange { get { return _YVisibleRange; } set { this.RaiseAndSetIfChanged(ref _YVisibleRange, value); } }

        
        public Range<double> YLogLogVisibleRange { get { return _YLogLogVisibleRange; } set { this.RaiseAndSetIfChanged(ref _YLogLogVisibleRange, value); } }

        public ObservableCollection<DataPointInfo> SelectedPointMarkers
        {
            get { return _SelectedPointMarkers!; }
            set
            {
                _SelectedPointMarkers = value;
                //_SelectedPointMarkers.CollectionChanged += Data_CollectionChanged;
            }
        }

        public ObservableCollection<DataPointInfo> SelectedLogLogPointMarkers
        {
            get { return _SelectedLogLogPointMarkers!; }
            set
            {
                _SelectedLogLogPointMarkers = value;
                //_SelectedLogLogPointMarkers.CollectionChanged += Data_CollectionChanged;
            }
        }
        
        private double _SelectedWeightsValue = 1.0;

        public double SelectedWeightsValue { get { return _SelectedWeightsValue; } set { this.RaiseAndSetIfChanged(ref _SelectedWeightsValue, value); } }

        public ReactiveCommand<Unit, Unit> SetSelectedWeightsCommand { get; }

        //public PlotModel ChartData
        //{
        //    get
        //    {
        //        PlotModel model = new PlotModel();

        //        SelectableLineSeries gasSeries = new SelectableLineSeries
        //        {
        //            Title           = "Gas Rate",
        //            SelectionMode   = SelectionMode.All,
        //            Color           = OxyColors.Red,
        //            StrokeThickness = 1,
        //            MarkerSize      = 3,
        //            LineStyle       = LineStyle.Solid,
        //            MarkerFill      =OxyColors.Transparent,
        //            MarkerStroke    = OxyColors.Red,
        //            MarkerType      = MarkerType.Circle
        //        };

        //        gasSeries.Points.AddRange(GasPoints);

        //        model.Series.Add(gasSeries);

        //        SelectableLineSeries oilSeries = new SelectableLineSeries
        //        {
        //            Title           = "Oil Rate",
        //            SelectionMode   = SelectionMode.All,
        //            Color           = OxyColors.Green,
        //            StrokeThickness = 1,
        //            LineStyle       = LineStyle.Solid,
        //            MarkerSize      = 3,
        //            MarkerFill      =OxyColors.Transparent,
        //            MarkerStroke    = OxyColors.Green,
        //            MarkerType      = MarkerType.Circle
        //        };

        //        //oilSeries.MouseDown += (s, e) =>
        //        //                       {
        //        //                           oilSeries.Select();

        //        //                           int index = (int)e.HitTestResult.Index;

        //        //                           if (oilSeries.IsItemSelected(index))
        //        //                           {
        //        //                               oilSeries.Unselect();
        //        //                           }
        //        //                           else
        //        //                           {
        //        //                               oilSeries.SelectItem(index);
        //        //                           }

        //        //                           model.InvalidatePlot(false);

        //        //                           e.Handled = false;
        //        //                       };

        //        oilSeries.Points.AddRange(OilPoints);

        //        model.Series.Add(oilSeries);

        //        model.SelectionColor = OxyColors.Red;

        //        return model;
        //    }
        //}

        //public PlotModel ChartDataLogLog
        //{
        //    get
        //    {
        //        PlotModel model = new PlotModel();

        //        model.Axes.Add(new LogarithmicAxis
        //        {
        //            Position = AxisPosition.Bottom
        //        });

        //        model.Axes.Add(new LogarithmicAxis
        //        {
        //            Position = AxisPosition.Left
        //        });

        //        LineSeries gasSeries = new LineSeries
        //        {
        //            Title           = "Gas Rate",
        //            SelectionMode   = SelectionMode.All,
        //            Color           = OxyColors.Red,
        //            StrokeThickness = 1,
        //            LineStyle       = LineStyle.Solid,
        //            MarkerSize      = 3,
        //            MarkerFill      =OxyColors.Transparent,
        //            MarkerStroke    = OxyColors.Red,
        //            MarkerType      = MarkerType.Circle
        //        };

        //        gasSeries.Points.AddRange(GasPoints);

        //        model.Series.Add(gasSeries);

        //        LineSeries oilSeries = new LineSeries
        //        {
        //            Title         = "Oil Rate",
        //            SelectionMode = SelectionMode.All,
        //            Color           = OxyColors.Green,
        //            StrokeThickness = 1,
        //            LineStyle       = LineStyle.Solid,
        //            MarkerSize      = 3,
        //            MarkerFill      =OxyColors.Transparent,
        //            MarkerStroke    = OxyColors.Green,
        //            MarkerType      = MarkerType.Circle
        //        };

        //        oilSeries.Points.AddRange(OilPoints);

        //        model.Series.Add(oilSeries);

        //        model.SelectionColor = OxyColors.Red;

        //        return model;
        //    }
        //}

        public MainViewModel(IMutableDependencyResolver? dependencyResolver = null)
        {
            dependencyResolver ??= Locator.CurrentMutable;

            SetSelectedWeightsCommand = ReactiveCommand.Create(SetSelectedWeights);

            _MultiPorositySettingsViewModel = new MultiPorositySettingsViewModel();
            _ProductionHistoryViewModel     = new ProductionHistoryViewModel();
            _MultiPorosityResultsViewModel  = new MultiPorosityResultsViewModel();

            _ProductionHistoryViewModel.SelectedModels.CollectionChanged += SelectedModels_CollectionChanged;

            _ProductionHistoryViewModel.PropertyChanged += ProductionHistoryViewModel_PropertyChanged;

            RegisterParts(dependencyResolver);

            DefaultSettings();
        }

        public MultiPorositySettingsViewModel MultiPorositySettingsViewModel
        {
            get { return _MultiPorositySettingsViewModel; }
            set { this.RaiseAndSetIfChanged(ref _MultiPorositySettingsViewModel, value); }
        }

        public ProductionHistoryViewModel ProductionHistoryViewModel
        {
            get { return _ProductionHistoryViewModel; }
            set { this.RaiseAndSetIfChanged(ref _ProductionHistoryViewModel, value); }
        }

        public MultiPorosityResultsViewModel MultiPorosityResultsViewModel
        {
            get { return _MultiPorosityResultsViewModel; }
            set { this.RaiseAndSetIfChanged(ref _MultiPorosityResultsViewModel, value); }
        }


        private void SetSelectedWeights()
        {
            DataRowView? row;

            if(ProductionHistoryViewModel.HistoryView_DataGrid != null)
            {
                for(int i = 0; i < ProductionHistoryViewModel.HistoryView_DataGrid.SelectedItems.Count; ++i)
                {
                    row = ProductionHistoryViewModel.HistoryView_DataGrid.SelectedItems[i] as DataRowView;

                    if(row != null)
                    {
                        row[5] = SelectedWeightsValue;
                    }
                }
            }
        }
        //public ObservableDictionary<IRenderableSeries, ObservableCollection<DataPoint>> SelectedDataPoints
        //{
        //    get { return _selectedDataPoints; }
        //    set
        //    {
        //        if(value == null)
        //        {
        //            return;
        //        }

        //        this.RaiseAndSetIfChanged(ref _selectedDataPoints, value);

        //        //_selectedDataPoints.CollectionChanged += Data_CollectionChanged;
        //    }
        //}

        #region Static Methods
        public static void SelectRowByIndex(DataGrid dataGrid,
                                    int rowIndex)
        {
            if (!dataGrid.SelectionUnit.Equals(DataGridSelectionUnit.FullRow))
            {
                throw new ArgumentException("The SelectionUnit of the DataGrid must be set to FullRow.");
            }

            if (rowIndex < 0 || rowIndex > dataGrid.Items.Count - 1)
            {
                throw new ArgumentException($"{rowIndex} is an invalid row index.");
            }

            dataGrid.SelectedItems.Clear();

            /* set the SelectedItem property */
            object item = dataGrid.Items[rowIndex]; // = Product X
            dataGrid.SelectedItem = item;

            DataGridRow? row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;

            if (row == null)
            {
                dataGrid.ScrollIntoView(item);
                row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            }

            if (row != null)
            {
                DataGridCell? cell = GetCell(dataGrid, row, 0);

                cell?.Focus();
            }
        }

        public static void SelectRowByIndex(DataGrid dataGrid,
                                            List<int> rowIndices)
        {
            //if(!dataGrid.SelectionUnit.Equals(DataGridSelectionUnit.FullRow))
            //{
            //    throw new ArgumentException("The SelectionUnit of the DataGrid must be set to FullRow.");
            //}

            dataGrid.SelectedItems.Clear();

            object item;
            DataGridRow? row;
            DataGridCell? cell;

            for (int i = 0; i < rowIndices.Count; ++i)
            {
                item = dataGrid.Items[rowIndices[i]];
                dataGrid.SelectedItems.Add(item);

                row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndices[i]) as DataGridRow;

                if (row == null)
                {
                    dataGrid.ScrollIntoView(item);
                    row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndices[i]) as DataGridRow;
                }

                if (row != null)
                {
                    cell = GetCell(dataGrid, row, 0);

                    cell?.Focus();
                }
            }
        }

        public static DataGridCell? GetCell(DataGrid dataGrid,
                                            DataGridRow? rowContainer,
                                            int column)
        {
            if (rowContainer != null)
            {
                DataGridCellsPresenter? presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);

                if (presenter == null)
                {
                    rowContainer.ApplyTemplate();

                    presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
                }

                if (presenter != null)
                {
                    if (presenter.ItemContainerGenerator.ContainerFromIndex(column) is DataGridCell cell)
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
            T? childOfChild;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T o)
                {
                    return o;
                }

                childOfChild = FindVisualChild<T>(child);

                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }

            return null;
        } 
        #endregion

        public void PointMarkersSelectionModifier_SelectionChanged(object    sender,
                                                                   EventArgs e)
        {
            if(sender is DataPointSelectionModifier dataPointSelectionModifier)
            {
                HashSet<int> indices = new HashSet<int>(dataPointSelectionModifier.SelectedPointMarkers.Count);

                foreach(DataPointInfo dataPointInfo in dataPointSelectionModifier.SelectedPointMarkers)
                {
                    if(ProductionHistoryViewModel.DaysToDateMap.TryGetValue((double)dataPointInfo.XValue, out DateTime dateTime))
                    {
                        for(int i = 0; i < ProductionHistoryViewModel.ProductionDataSet.Monthly.Count; ++i)
                        {
                            if((DateTime)ProductionHistoryViewModel.ProductionDataSet.Monthly[i][0] == dateTime && !indices.Contains(i))
                            {
                                indices.Add(i);

                                break;
                            }
                        }
                    }
                }

                if(ProductionHistoryViewModel.HistoryView_DataGrid != null)
                {
                    SelectRowByIndex(ProductionHistoryViewModel.HistoryView_DataGrid, indices.ToList());
                }

                //ProductionHistoryViewModel.HistoryView_DataGrid.UnselectAll();
                ////ProductionHistoryViewModel.HistoryView_DataGrid.SelectedItems.Clear();
                //DataRowView? row;

                //foreach(DataPointInfo dataPointInfo in dataPointSelectionModifier.SelectedPointMarkers)
                //{
                //    if(ProductionHistoryViewModel.DaysToDateMap.TryGetValue((double)dataPointInfo.XValue, out DateTime dateTime))
                //    {
                //        for(int i = 0; i < ProductionHistoryViewModel.ProductionDataSet.Monthly.Count; ++i)
                //        {
                //            if((DateTime)ProductionHistoryViewModel.ProductionDataSet.Monthly[i][0] == dateTime)
                //            {
                //                row = ProductionHistoryViewModel.HistoryView_DataGrid.Items[i] as DataRowView;

                //                if(row != null)
                //                {
                //                    ProductionHistoryViewModel.HistoryView_DataGrid.SelectedItems.Add(row.Row);
                //                }

                //                break;
                //            }
                //        }
                //    }
                //}
            }
        }
        private void ProductionHistoryViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case "HistoryCount":
                {

                    if(ProductionHistoryViewModel.DaysToDateMap.Count > 0)
                    {
                        double maxDays = ProductionHistoryViewModel.DaysToDateMap.Keys.Max();

                        XVisibleRange = new DoubleRange(0.0, Math.Round(maxDays / 100.0, 0) * 100.0 + 30);

                        XLogLogVisibleRange = new DoubleRange(0.1, Math.Round(maxDays / 10.0, 0) * 10.0);
                    }


                    break;
                }
                case "MaxGas":
                {
                    YVisibleRange       = new DoubleRange(0.0, Math.Max(YVisibleRange.Max,       Math.Round(ProductionHistoryViewModel.MaxGas / 50.0, 0) * 50.0));
                    YLogLogVisibleRange = new DoubleRange(1.0, Math.Max(YLogLogVisibleRange.Max, ProductionHistoryViewModel.MaxGas));

                    for (int i = 0; i < LogLogMaxes.Length; ++i)
                    {
                        if(YLogLogVisibleRange.Min >= LogLogMaxes[i])
                        {
                            YLogLogVisibleRange.Min = LogLogMaxes[i];

                            break;
                        }
                    }

                    for (int i = 1; i < LogLogMaxes.Length; ++i)
                    {
                        if(YLogLogVisibleRange.Max < LogLogMaxes[i] && YLogLogVisibleRange.Max > LogLogMaxes[i - 1])
                        {
                            YLogLogVisibleRange.Max = LogLogMaxes[i];

                            break;
                        }
                    }

                    
                    break;
                }
                case "MaxOil":
                {
                    YVisibleRange       = new DoubleRange(0.0, Math.Max(YVisibleRange.Max,       Math.Round(ProductionHistoryViewModel.MaxOil / 50.0, 0) * 50.0));
                    YLogLogVisibleRange = new DoubleRange(1.0, Math.Max(YLogLogVisibleRange.Max, ProductionHistoryViewModel.MaxOil));

                    for (int i = 0; i < LogLogMaxes.Length; ++i)
                    {
                        if(YLogLogVisibleRange.Min >= LogLogMaxes[i])
                        {
                            YLogLogVisibleRange.Min = LogLogMaxes[i];

                            break;
                        }
                    }

                    for (int i = 1; i < LogLogMaxes.Length; ++i)
                    {
                        if(YLogLogVisibleRange.Max < LogLogMaxes[i] && YLogLogVisibleRange.Max > LogLogMaxes[i - 1])
                        {
                            YLogLogVisibleRange.Max = LogLogMaxes[i];

                            break;
                        }
                    }
                    break;
                }
            }
        }

        private void SelectedModels_CollectionChanged(object                           sender,
                                                      NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    //foreach(DataGridCellInfo dataGridCellInfo in e.NewItems.Cast<DataGridCellInfo>())
                    //{
                    //    SelectedPointMarkers.Add(GasPoints.);
                    //}

                    break;
                }
                case NotifyCollectionChangedAction.Reset:
                {
                    break;
                }
                case NotifyCollectionChangedAction.Remove:
                {
                    //foreach(DataGridCellInfo dataGridCellInfo in e.OldItems.Cast<DataGridCellInfo>())
                    //{

                    //}

                    break;
                }
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Move:
                default: throw new ApplicationException();
            }
        }

        private void Data_CollectionChanged(object                           sender,
                                            NotifyCollectionChangedEventArgs e)
        {

            if(ProductionHistoryViewModel.HistoryView_DataGrid is null)
            {
                return;
            }


            if(sender is ObservableCollection<DataPointInfo> dataPoints && dataPoints.Count == 0)
            {
                ProductionHistoryViewModel.HistoryView_DataGrid.SelectedItems.Clear();

                return;
            }

            DataRowView row;

            switch(e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    foreach(DataPointInfo dataPointInfo in e.NewItems.Cast<DataPointInfo>())
                    {
                        if(ProductionHistoryViewModel.DaysToDateMap.TryGetValue((double)dataPointInfo.XValue, out DateTime dateTime))
                        {
                            for(int i = 0; i < ProductionHistoryViewModel.ProductionDataSet.Monthly.Count; ++i)
                            {
                                if((DateTime)ProductionHistoryViewModel.ProductionDataSet.Monthly[i][0] == dateTime)
                                {
                                    row = (DataRowView)ProductionHistoryViewModel.HistoryView_DataGrid.Items[i];

                                    ProductionHistoryViewModel.HistoryView_DataGrid.SelectedItems.Add(row);

                                    break;
                                }
                            }
                        }
                    }

                    break;
                }
                case NotifyCollectionChangedAction.Reset:
                {
                    break;
                }
                case NotifyCollectionChangedAction.Remove:
                {
                    foreach(DataPointInfo dataPointInfo in e.OldItems.Cast<DataPointInfo>())
                    {
                        if(ProductionHistoryViewModel.DaysToDateMap.TryGetValue((double)dataPointInfo.XValue, out DateTime dateTime))
                        {
                            for(int i = 0; i < ProductionHistoryViewModel.ProductionDataSet.Monthly.Count; ++i)
                            {
                                if((DateTime)ProductionHistoryViewModel.ProductionDataSet.Monthly[i][0] == dateTime)
                                {
                                    row = (DataRowView)ProductionHistoryViewModel.HistoryView_DataGrid.Items[i];

                                    ProductionHistoryViewModel.HistoryView_DataGrid.SelectedItems.Remove(row);

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


            //if(sender is ObservableCollection<DataPointInfo> dataPointInfos)
            //{
            //    foreach(DataPointInfo? dataPointInfo in dataPointInfos)
            //    {
            //        if(ProductionHistoryViewModel.DaysToDateMap.TryGetValue((double)dataPointInfo.XValue, out DateTime dateTime))
            //        {
            //            for(int i = 0; i < ProductionHistoryViewModel.ProductionDataSet.Monthly.Count; ++i)
            //            {
            //                if((DateTime)ProductionHistoryViewModel.ProductionDataSet.Monthly[i][0] == dateTime)
            //                {
            //                    System.Data.DataRowView row = (System.Data.DataRowView)ProductionHistoryViewModel.HistoryView_DataGrid.Items[i];

            //                    ProductionHistoryViewModel.HistoryView_DataGrid.SelectedItems.Add(row);

            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}

            //double gas;
            //double oil;
            //double weight;

            //for (int i = 0; i < ProductionHistoryViewModel.ProductionDataSet.Monthly.Count; ++i)
            //{
            //    //gas = (double)dataSet.Monthly[i][1];
            //    //oil = (double)dataSet.Monthly[i][2];
            //    //weight = (double)dataSet.Monthly[i][5];

            //}
        }

        private void RegisterParts(IMutableDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterConstant(this, typeof(IMainViewModel));

            dependencyResolver.RegisterConstant(MultiPorositySettingsViewModel, typeof(MultiPorositySettingsViewModel));

            dependencyResolver.RegisterConstant(ProductionHistoryViewModel, typeof(ProductionHistoryViewModel));

            dependencyResolver.RegisterConstant(MultiPorosityResultsViewModel, typeof(MultiPorosityResultsViewModel));

            //dependencyResolver.Register(() => new MultiPorositySettingsView(), typeof(IViewFor<MultiPorositySettingsViewModel>));
            //dependencyResolver.Register(() => new ProductionHistoryView(),     typeof(IViewFor<ProductionHistoryViewModel>));
            //dependencyResolver.Register(() => new MultiPorosityResultsView(),  typeof(IViewFor<MultiPorosityResultsViewModel>));
        }

        private void DefaultSettings()
        {
            /*km*/
            //arg_limits[0] = new BoundConstraints<double>(0.0001, 0.01);
            MultiPorositySettingsViewModel.MatrixPermLower = 0.0001;
            MultiPorositySettingsViewModel.MatrixPermUpper = 0.01;

            /*kF*/
            //arg_limits[1] = new BoundConstraints<double>(100.0, 10000.0);
            MultiPorositySettingsViewModel.HydralicFracturePermLower = 100.0;
            MultiPorositySettingsViewModel.HydralicFracturePermUpper = 10000.0;

            /*kf*/
            //arg_limits[2] = new BoundConstraints<double>(0.01, 100.0);
            MultiPorositySettingsViewModel.NaturalFracturePermLower = 0.01;
            MultiPorositySettingsViewModel.NaturalFracturePermUpper = 100.0;

            /*ye*/
            //arg_limits[3] = new BoundConstraints<double>(1.0, 500.0);
            MultiPorositySettingsViewModel.HydralicFractureHalfLengthLower = 1.0;
            MultiPorositySettingsViewModel.HydralicFractureHalfLengthUpper = 500.0;

            /*LF*/
            //arg_limits[4] = new BoundConstraints<double>(50.0, 250.0);
            MultiPorositySettingsViewModel.HydralicFractureSpacingLower = 50.0;
            MultiPorositySettingsViewModel.HydralicFractureSpacingUpper = 250.0;

            /*Lf*/
            MultiPorositySettingsViewModel.NaturalFractureSpacingLower = 10.0;
            MultiPorositySettingsViewModel.NaturalFractureSpacingUpper = 150.0;
            //arg_limits[5] = new BoundConstraints<double>(10.0, 150.0);

            /*sk*/
            //arg_limits[6] = new BoundConstraints<double>(0.0, 0.0);

            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("11/1/2012"), 270.7299,           476.1036,           0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("12/1/2012"), 190.127903225806,   303.849,            0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("1/1/2013"),  164.749064516129,   282.59564516129,    0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("2/1/2013"),  125.19675,          229.90275,          0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("3/1/2013"),  177.836806451613,   263.390806451613,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("4/1/2013"),  126.7434,           213.5322,           0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("5/1/2013"),  122.07164516129,    173.611741935484,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("6/1/2013"),  74.4408,            133.4466,           0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("7/1/2013"),  68.5683870967742,   112.127806451613,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("8/1/2013"),  75.2829677419355,   125.898387096774,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("9/1/2013"),  63.0483,            110.2794,           0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("10/1/2013"), 45.5794838709677,   90.1204838709677,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("11/1/2013"), 43.6443,            84.9807,            0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("12/1/2013"), 57.9986129032258,   105.356322580645,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("1/1/2014"),  53.8162258064516,   86.9339032258064,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("2/1/2014"),  59.39325,           94.20075,           0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("3/1/2014"),  61.3132258064516,   88.3422580645161,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("4/1/2014"),  52.9788,            81.8496,            0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("5/1/2014"),  11.0392258064516,   21.1110967741935,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("6/1/2014"),  18.6837,            36.0003,            0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("7/1/2014"),  18.1379032258065,   38.0113548387097,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("8/1/2014"),  31.4105806451613,   50.530064516129,    0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("9/1/2014"),  20.5947,            43.4091,            0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("10/1/2014"), 22.491,             61.74,              0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("11/1/2014"), 22.7115,            56.2275,            0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("12/1/2014"), 37.4423225806452,   57.4153548387097,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("1/1/2015"),  15.4207741935484,   34.582935483871,    0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("2/1/2015"),  9.18225,            43.92675,           0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("3/1/2015"),  11.4517741935484,   27.4842580645161,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("4/1/2015"),  9.0846,             26.0337,            0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("5/1/2015"),  23.6575161290323,   45.679064516129,    0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("6/1/2015"),  32.4135,            58.7118,            0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("7/1/2015"),  25.4357419354839,   43.7443548387097,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("8/1/2015"),  8.2651935483871,    17.6826774193548,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("9/1/2015"),  4.1454,             8.5848,             0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("10/1/2015"), 6.81416129032258,   15.0651290322581,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("11/1/2015"), 6.174,              13.7739,            0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("12/1/2015"), 7.58235483870968,   17.824935483871,    0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("1/1/2016"),  7.8241935483871,    15.1220322580645,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("2/1/2016"),  4.82058620689655,   17.2142068965517,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("3/1/2016"),  1.26609677419355,   12.7036451612903,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("4/1/2016"),  1.6464,             4.3512,             0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("5/1/2016"),  11.0676774193548,   26.3319677419355,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("6/1/2016"),  7.8204,             22.2558,            0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("7/1/2016"),  9.41748387096774,   23.188064516129,    0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("8/1/2016"),  7.07022580645161,   18.0240967741935,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("9/1/2016"),  9.8784,             18.081,             0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("10/1/2016"), 13.927064516129,    22.1922580645161,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("11/1/2016"), 4.9833,             8.5701,             0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("12/1/2016"), 0.01,              0.01,              0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("1/1/2017"),  12.1346129032258,   22.2633870967742,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("2/1/2017"),  13.3245,            27.57825,           0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("3/1/2017"),  11.6224838709677,   18.1805806451613,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("4/1/2017"),  1.3671,             3.2193,             0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("5/1/2017"),  6.9421935483871,    18.4224193548387,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("6/1/2017"),  13.6269,            19.698,             0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("7/1/2017"),  11.3664193548387,   21.3102580645161,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("8/1/2017"),  9.01916129032258,   16.0467096774194,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("9/1/2017"),  10.2165,            16.0083,            0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("10/1/2017"), 9.3748064516129,    16.1889677419355,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("11/1/2017"), 8.9229,             16.0965,            0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("12/1/2017"), 7.78151612903226,   14.4107419354839,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("1/1/2018"),  7.15558064516129,   14.3822903225806,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("2/1/2018"),  10.08,              13.986,             0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("3/1/2018"),  6.615,              14.1689032258065,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("4/1/2018"),  5.1597,             8.7024,             0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("5/1/2018"),  0.0142258064516129, 1.42258064516129,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("6/1/2018"),  0.01,              6.3651,             0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("7/1/2018"),  0.298741935483871,  12.4049032258065,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("8/1/2018"),  4.5238064516129,    19.6885161290323,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("9/1/2018"),  5.1891,             8.6142,             0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("10/1/2018"), 0.01,              0.0284516129032258, 0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("11/1/2018"), 0.0588,             1.7493,             0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("12/1/2018"), 8.80577419354839,   22.8750967741936,   0.0, 1500.0, 1.0);
            ProductionHistoryViewModel.ProductionDataSet.Monthly.AddMonthlyRow(DateTime.Parse("1/1/2019"),  5.69032258064516,   13.8843870967742,   0.0, 1500.0, 1.0);
        }
    }
}