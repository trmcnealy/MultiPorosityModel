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
    public class ProductionSmootherChartViewModel : BindableBase
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

       
        private readonly ProductionSmootherService _productionSmootherService;

        public ProductionSmootherChartViewModel(ProductionSmootherService? productionSmootherService)
        {
            _productionSmootherService = productionSmootherService;
            
            _productionSmootherService.PropertyChanged -= OnPropertyChanged;
            _productionSmootherService.PropertyChanged += OnPropertyChanged;

            OnPropertyChanged(this, new PropertyChangedEventArgs("Model"));

            PlotData = new ObservableCollection<Plotly.Models.ITrace>
            {
                new ScatterGl
                {
                    Name = "Gas",
                    Mode = ScatterGl.ModeFlag.Markers | ScatterGl.ModeFlag.Lines,
                    XSrc = "Days",
                    YSrc = "Gas",
                    Line = new Line()
                    {
                        Color = "#CC0000",
                        Width = 2
                    },
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
                    Mode = ScatterGl.ModeFlag.Markers | ScatterGl.ModeFlag.Lines,
                    XSrc = "Days",
                    YSrc = "Oil",
                    Line = new Line()
                    {
                        Color = "#00CC00",
                        Width = 2
                    },
                    Marker = new Marker()
                    {
                        Color   = "#00CC00",
                        Size    = 10,
                        SizeSrc = "Weight"
                    }
                },
                new ScatterGl
                {
                    Name = "Water",
                    Mode = ScatterGl.ModeFlag.Markers | ScatterGl.ModeFlag.Lines,
                    XSrc = "Days",
                    YSrc = "Water",
                    Line = new Line()
                    {
                        Color = "#0000CC",
                        Width = 2
                    },
                    Marker = new Marker()
                    {
                        Color   = "#0000CC",
                        Size    = 10,
                        SizeSrc = "Weight"
                    }
                },
                new ScatterGl
                {
                    Name = "Smooth Gas",
                    Mode = ScatterGl.ModeFlag.Lines,
                    XSrc = "SmoothDays",
                    YSrc = "SmoothGas",
                    Line = new Line()
                    {
                        Color = "#CC0000",
                        Width = 7
                    }
                },
                new ScatterGl
                {
                    Name = "Smooth Oil",
                    Mode = ScatterGl.ModeFlag.Lines,
                    XSrc = "SmoothDays",
                    YSrc = "SmoothOil",
                    Line = new Line()
                    {
                        Color = "#00CC00",
                        Width = 7
                    }
                },
                new ScatterGl
                {
                    Name = "Smooth Water",
                    Mode = ScatterGl.ModeFlag.Lines,
                    XSrc = "SmoothDays",
                    YSrc = "SmoothWater",
                    Line = new Line()
                    {
                        Color = "#0000CC",
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
                case "Model":
                {
                    _productionSmootherService.PropertyChanged -= OnPropertyChanged;
                    _productionSmootherService.PropertyChanged += OnPropertyChanged;

                    _productionSmootherService.Model.PropertyChanged -= OnPropertyChanged;
                    _productionSmootherService.Model.PropertyChanged += OnPropertyChanged;
                    
                    _productionSmootherService.Model.ProductionRecords.CollectionChanged -= OnProductionRecordsChanged;
                    _productionSmootherService.Model.ProductionRecords.CollectionChanged += OnProductionRecordsChanged;
                    
                    _productionSmootherService.Model.SmoothedProductionRecords.CollectionChanged -= OnProductionRecordsChanged;
                    _productionSmootherService.Model.SmoothedProductionRecords.CollectionChanged += OnProductionRecordsChanged;
                    
                    OnProductionRecordsChanged(sender, null);
                    break;
                }
                case "ProductionRecords":
                {
                    OnProductionRecordsChanged(sender, null);

                    break;
                }
                case "SmoothedProductionRecords":
                {
                    OnProductionRecordsChanged(sender, null);

                    break;
                }
            }
        }

        private void OnProductionRecordsChanged(object?                          sender,
                                                NotifyCollectionChangedEventArgs? e)
        {
            ProductionRecord[]? productionRecordArray = _productionSmootherService.Model.ProductionRecords.ToArray();
            
            ProductionRecord[]? smoothedProductionRecordArray = _productionSmootherService.Model.SmoothedProductionRecords.ToArray();

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
                    "SmoothDays", ("float", new ProductionRecordColumn(2, smoothedProductionRecordArray).ToArray())
                },
                {
                    "SmoothGas", ("float", new ProductionRecordColumn(3, smoothedProductionRecordArray).ToArray())
                },
                {
                    "SmoothOil", ("float", new ProductionRecordColumn(4, smoothedProductionRecordArray).ToArray())
                },
                {
                    "SmoothWater", ("float", new ProductionRecordColumn(5, smoothedProductionRecordArray).ToArray())
                }
            };
        }
    }
}