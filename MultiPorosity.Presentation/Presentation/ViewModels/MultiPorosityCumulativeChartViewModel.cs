
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

using OrientationEnum = Plotly.Models.Layouts.Legends.OrientationEnum;
using Title = Plotly.Models.Layouts.Title;

namespace MultiPorosity.Presentation
{
    public class MultiPorosityCumulativeChartViewModel : BindableBase
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
                }
            }
        }

        #endregion

        private readonly MultiPorosityModelService _multiPorosityModelService;

        public MultiPorosityCumulativeChartViewModel(MultiPorosityModelService multiPorosityModelService)
        {
            _multiPorosityModelService = multiPorosityModelService;
            
            _multiPorosityModelService.PropertyChanged                                             -= OnPropertyChanged;
            _multiPorosityModelService.PropertyChanged                                             += OnPropertyChanged;

            OnPropertyChanged(this, new PropertyChangedEventArgs("ActiveProject"));

            PlotData = new ObservableCollection<Plotly.Models.ITrace>
            {
                new ScatterGl
                {
                    Name = "Gas",
                    Mode = ScatterGl.ModeFlag.Markers,
                    XSrc = "Days",
                    YSrc = "Gas",
                    Marker = new Marker()
                    {
                        Color   = "#CC0000",
                        Size    = 10
                    }
                },
                new ScatterGl
                {
                    Name = "Oil",
                    Mode = ScatterGl.ModeFlag.Markers,
                    XSrc = "Days",
                    YSrc = "Oil",
                    Marker = new Marker()
                    {
                        Color = "#00CC00",
                        Size  = 10
                    },
                    YAxis = "y2"
                },
                new ScatterGl
                {
                    Name = "Water",
                    Mode = ScatterGl.ModeFlag.Markers,
                    XSrc = "Days",
                    YSrc = "Water",
                    Marker = new Marker()
                    {
                        Color = "#0000CC",
                        Size  = 10
                    },
                    YAxis = "y2"
                },
                new ScatterGl
                {
                    Name = "Model Gas",
                    Mode = ScatterGl.ModeFlag.Lines,
                    XSrc = "ModelDays",
                    YSrc = "ModelGas",
                    Line = new Line()
                    {
                        Color = "#CC0000",
                        Width = 7
                    }
                },
                new ScatterGl
                {
                    Name = "Model Oil",
                    Mode = ScatterGl.ModeFlag.Lines,
                    XSrc = "ModelDays",
                    YSrc = "ModelOil",
                    Line = new Line()
                    {
                        Color = "#00CC00",
                        Width = 7
                    },
                    YAxis = "y2"
                },
                new ScatterGl
                {
                    Name = "Model Water",
                    Mode = ScatterGl.ModeFlag.Lines,
                    XSrc = "ModelDays",
                    YSrc = "ModelWater",
                    Line = new Line()
                    {
                        Color = "#0000CC",
                        Width = 7
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
                        Title = new Plotly.Models.Layouts.XAxes.Title
                        {
                            Text = "Days"
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
        {
            switch(e.PropertyName)
            {
                case "ActiveProject":
                {
                    _multiPorosityModelService.ActiveProject.PropertyChanged                               -= OnPropertyChanged;
                    _multiPorosityModelService.ActiveProject.PropertyChanged                               += OnPropertyChanged;
                    
                    _multiPorosityModelService.ActiveProject.CumulativeProductionRecords.CollectionChanged -= OnCumulativeProductionRecordsChanged;
                    _multiPorosityModelService.ActiveProject.CumulativeProductionRecords.CollectionChanged += OnCumulativeProductionRecordsChanged;
                    
                    _multiPorosityModelService.ActiveProject.CumulativeMultiPorosityModelProduction.CollectionChanged           -= OnCumulativeProductionRecordsChanged;
                    _multiPorosityModelService.ActiveProject.CumulativeMultiPorosityModelProduction.CollectionChanged += OnCumulativeProductionRecordsChanged;
                    
                    OnCumulativeProductionRecordsChanged(sender, null);
                    break;
                }
                case "CumulativeProductionRecords":
                {
                    OnCumulativeProductionRecordsChanged(sender, null);

                    break;
                }
                case "CumulativeMultiPorosityModelProduction":
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
            
            CumulativeMultiPorosityModelProduction[]? cumulativeMultiPorosityModelProductionArray = _multiPorosityModelService.ActiveProject.CumulativeMultiPorosityModelProduction.ToArray();

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
                },
                {
                    "ModelDays", ("float", new CumulativeMultiPorosityModelProductionColumn(0, cumulativeMultiPorosityModelProductionArray).ToArray())
                },
                {
                    "ModelGas", ("float", new CumulativeMultiPorosityModelProductionColumn(1, cumulativeMultiPorosityModelProductionArray).ToArray())
                },
                {
                    "ModelOil", ("float", new CumulativeMultiPorosityModelProductionColumn(2, cumulativeMultiPorosityModelProductionArray).ToArray())
                },
                {
                    "ModelWater", ("float", new CumulativeMultiPorosityModelProductionColumn(3, cumulativeMultiPorosityModelProductionArray).ToArray())
                }
            };
        }
    }
}