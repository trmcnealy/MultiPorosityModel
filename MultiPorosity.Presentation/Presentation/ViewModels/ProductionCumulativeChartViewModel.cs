
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

using MultiPorosity.Models;
using MultiPorosity.Presentation.Services;

using Plotly;
using Plotly.Models.Layouts;
using Plotly.Models.Layouts.Legends;
using Plotly.Models.Traces;
using Plotly.Models.Traces.ScatterGls;

using Prism.Mvvm;

using Line = Plotly.Models.Traces.ScatterGls.Line;
using OrientationEnum = Plotly.Models.Layouts.Legends.OrientationEnum;
using Title = Plotly.Models.Layouts.Title;

namespace MultiPorosity.Presentation
{
    public class ProductionCumulativeChartViewModel : BindableBase
    {
        #region Chart Properties

        private ObservableDictionary<string, (string type, object[] array)> dataSource = new ();

        public ObservableDictionary<string, (string type, object[] array)> DataSource
        {
            get { return dataSource; }
            set { SetProperty(ref dataSource, value); }
        }

        private ObservableCollection<Plotly.Models.ITrace> plotData;

        public ObservableCollection<Plotly.Models.ITrace> PlotData
        {
            get { return plotData; }
            set { SetProperty(ref plotData, value); }
        }

        private Plotly.Models.Layout plotLayout;

        public Plotly.Models.Layout PlotLayout
        {
            get { return plotLayout; }
            set { SetProperty(ref plotLayout, value); }
        }

        private SelectedData[] selected;

        public SelectedData[] SelectedRecords
        {
            get { return selected; }
            set
            {
                if(SetProperty(ref selected, value))
                {                    
                    List<CumulativeProductionRecord> selectedCumulativeProductionRecords = new(selected.Length);

                    for (int i = 0; i < selected.Length; ++i)
                    {
                        selectedCumulativeProductionRecords.Add(_multiPorosityModelService.ActiveProject.CumulativeProductionRecords[selected[i].PointIndex]);
                    }

                    _multiPorosityModelService.ActiveProject.SelectedCumulativeProductionRecords = new (selectedCumulativeProductionRecords);

                    //this.RaisePropertyChanged(nameof(SelectedProductionRecords));
                }
            }
        }

        #endregion

        private readonly MultiPorosityModelService _multiPorosityModelService;

        public ProductionCumulativeChartViewModel(MultiPorosityModelService multiPorosityModelService)
        {
            _multiPorosityModelService = multiPorosityModelService;
            
            _multiPorosityModelService.PropertyChanged -= OnPropertyChanged;
            _multiPorosityModelService.PropertyChanged += OnPropertyChanged;

            OnPropertyChanged(this, new PropertyChangedEventArgs("ActiveProject"));

            PlotData = new ObservableCollection<Plotly.Models.ITrace>
            {
                new ScatterGl
                {
                    Name = "Gas",
                    Mode = ScatterGl.ModeFlag.Lines | ScatterGl.ModeFlag.Markers,
                    XSrc = "Date",
                    YSrc = "Gas",
                    Marker = new Marker()
                    {
                        Color   = "#CC0000",
                        Size    = 8
                    },
                    Line = new Line()
                    {
                        Color = "#CC0000",
                        Width = 1
                    }
                },
                new ScatterGl
                {
                    Name = "Oil",
                    Mode = ScatterGl.ModeFlag.Lines | ScatterGl.ModeFlag.Markers,
                    XSrc = "Date",
                    YSrc = "Oil",
                    Marker = new Marker()
                    {
                        Color = "#00CC00",
                        Size  = 8
                    },
                    Line = new Line()
                    {
                        Color = "#00CC00",
                        Width = 1
                    },
                    YAxis = "y2"
                },
                new ScatterGl
                {
                    Name = "Water",
                    Mode = ScatterGl.ModeFlag.Lines | ScatterGl.ModeFlag.Markers,
                    XSrc = "Date",
                    YSrc = "Water",
                    Marker = new Marker()
                    {
                        Color = "#0000CC",
                        Size  = 8
                    },
                    Line = new Line()
                    {
                        Color = "#0000CC",
                        Width = 1
                    },
                    YAxis = "y2"
                }
            };

            PlotLayout = new Plotly.Models.Layout
            {
                Title = new Title
                {
                    Text = "Production"
                },
                ShowLegend = true,
                Legend = new Legend()
                {
                    Orientation = OrientationEnum.H,
                    XAnchor = XAnchorEnum.Center,
                    YAnchor = YAnchorEnum.Bottom,
                    X=0.5,
                    Y=1
                },
                XAxis = new List<XAxis>
                {
                    new XAxis
                    {
                        Type = Plotly.Models.Layouts.XAxes.TypeEnum.Date,
                        Title = new Plotly.Models.Layouts.XAxes.Title
                        {
                            Text = "Date"
                        }
                    }
                },
                YAxis = new List<YAxis>
                {
                    new YAxis
                    {
                        Side = Plotly.Models.Layouts.YAxes.SideEnum.Left,
                        Type = Plotly.Models.Layouts.YAxes.TypeEnum.Linear,
                        Title = new Plotly.Models.Layouts.YAxes.Title
                        {
                            Text = "Mscf"
                        }
                    },
                    new YAxis
                    {
                        Side = Plotly.Models.Layouts.YAxes.SideEnum.Right,
                        Type = Plotly.Models.Layouts.YAxes.TypeEnum.Linear,
                        Title = new Plotly.Models.Layouts.YAxes.Title
                        {
                            Text = "BBL"
                        },
                        Overlaying = "y"
                    }
                }
            };
        }
        

        private void OnPropertyChanged(object?                  sender,
                                       PropertyChangedEventArgs e)
        {            switch(e.PropertyName)
            {
                case "ActiveProject":
                {
                    _multiPorosityModelService.ActiveProject.PropertyChanged                               -= OnPropertyChanged;
                    _multiPorosityModelService.ActiveProject.PropertyChanged                               += OnPropertyChanged;
                    _multiPorosityModelService.ActiveProject.CumulativeProductionRecords.CollectionChanged -= OnCumulativeProductionRecordsChanged;
                    _multiPorosityModelService.ActiveProject.CumulativeProductionRecords.CollectionChanged += OnCumulativeProductionRecordsChanged;
                    OnCumulativeProductionRecordsChanged(sender, null);
                    break;
                }
                case "CumulativeProductionRecords":
                {
                    OnCumulativeProductionRecordsChanged(sender, null);

                    break;
                }
            }
        }
        private void OnCumulativeProductionRecordsChanged(object?                          sender,
                                                          NotifyCollectionChangedEventArgs? e)
        {
            CumulativeProductionRecord[]? cumulativeProductionRecordsArray = _multiPorosityModelService.ActiveProject.CumulativeProductionRecords.ToArray();

            DataSource = new ObservableDictionary<string, (string type, object[] array)>
            {
                {
                    "Date", ("string", new CumulativeProductionRecordColumn(1, cumulativeProductionRecordsArray).ToArray())
                },
                {
                    "Days", ("float", new CumulativeProductionRecordColumn(2, cumulativeProductionRecordsArray).ToArray())
                },
                {
                    "Gas", ("float", new CumulativeProductionRecordColumn(3, cumulativeProductionRecordsArray).ToArray())
                },
                {
                    "Oil", ("float", new CumulativeProductionRecordColumn(4, cumulativeProductionRecordsArray).ToArray())
                },
                {
                    "Water", ("float", new CumulativeProductionRecordColumn(5, cumulativeProductionRecordsArray).ToArray())
                }
            };
        }
    }
}