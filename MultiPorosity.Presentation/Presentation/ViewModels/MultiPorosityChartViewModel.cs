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
    public class MultiPorosityChartViewModel : BindableBase
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

        public MultiPorosityChartViewModel(MultiPorosityModelService multiPorosityModelService)
        {
            _multiPorosityModelService = multiPorosityModelService;
            
            _multiPorosityModelService.PropertyChanged                                   -= OnPropertyChanged;
            _multiPorosityModelService.PropertyChanged                                   += OnPropertyChanged;

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
                        Size    = 10,
                        SizeSrc = "Weight"
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
                        Color   = "#00CC00",
                        Size    = 10,
                        SizeSrc = "Weight"
                    }
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
                    Name      = "Model Oil",
                    Mode      = ScatterGl.ModeFlag.Lines,
                    XSrc      = "ModelDays",
                    YSrc      = "ModelOil",
                    Line = new Line()
                    {
                        Color = "#00CC00",
                        Width = 7
                    }
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
                        Type = Plotly.Models.Layouts.YAxes.TypeEnum.Linear,
                        Title = new Plotly.Models.Layouts.YAxes.Title
                        {
                            Text = "Rate"
                        }
                        //Range = new object[]()
                        //{
                        //    0.00,

                        //}
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
                    _multiPorosityModelService.ActiveProject.PropertyChanged                     -= OnPropertyChanged;
                    _multiPorosityModelService.ActiveProject.PropertyChanged                     += OnPropertyChanged;
                    
                    _multiPorosityModelService.ActiveProject.ProductionRecords.CollectionChanged -= OnProductionRecordsChanged;
                    _multiPorosityModelService.ActiveProject.ProductionRecords.CollectionChanged += OnProductionRecordsChanged;
                    
                    _multiPorosityModelService.ActiveProject.MultiPorosityModelProduction.CollectionChanged -= OnProductionRecordsChanged;
                    _multiPorosityModelService.ActiveProject.MultiPorosityModelProduction.CollectionChanged += OnProductionRecordsChanged;
                    
                    OnProductionRecordsChanged(sender, null);
                    break;
                }
                case "ProductionRecords":
                {
                    OnProductionRecordsChanged(sender, null);

                    break;
                }
                case "MultiPorosityModelProduction":
                {
                    OnProductionRecordsChanged(sender, null);

                    break;
                }
            }
        }

        private void OnProductionRecordsChanged(object?                          sender,
                                                NotifyCollectionChangedEventArgs? e)
        {
            ProductionRecord[]? productionRecordArray = _multiPorosityModelService.ActiveProject.ProductionRecords.ToArray();
            
            MultiPorosityModelProduction[]? multiPorosityModelProductionArray = _multiPorosityModelService.ActiveProject.MultiPorosityModelProduction.ToArray();

            DataSource = new ObservableDictionary<string, (string type, object[] array)>
            {
                {
                    "Date", ("string", new ProductionRecordColumn(1, productionRecordArray).ToArray())
                },
                {
                    "Days", ("float", new ProductionRecordColumn(2, productionRecordArray).ToArray())
                },
                {
                    "Gas", ("float", new ProductionRecordColumn(3, productionRecordArray).ToArray())
                },
                {
                    "Oil", ("float", new ProductionRecordColumn(4, productionRecordArray).ToArray())
                },
                {
                    "Water", ("float", new ProductionRecordColumn(5, productionRecordArray).ToArray())
                },
                {
                    "Weight", ("float", new ProductionRecordColumn(7, productionRecordArray).ToArray())
                },
                {
                    "ModelDays", ("float", new MultiPorosityModelProductionColumn(0, multiPorosityModelProductionArray).ToArray())
                },
                {
                    "ModelGas", ("float", new MultiPorosityModelProductionColumn(1, multiPorosityModelProductionArray).ToArray())
                },
                {
                    "ModelOil", ("float", new MultiPorosityModelProductionColumn(2, multiPorosityModelProductionArray).ToArray())
                },
                {
                    "ModelWater", ("float", new MultiPorosityModelProductionColumn(3, multiPorosityModelProductionArray).ToArray())
                }
            };
        }
    }
}