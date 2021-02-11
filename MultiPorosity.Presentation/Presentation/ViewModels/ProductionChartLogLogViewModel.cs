using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

using Engineering.UI.Collections;

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
    public class ProductionChartLogLogViewModel : BindableBase
    {
        #region Chart Properties

        private ObservableDictionary<string, (string type, object[] array)> dataSource = new ObservableDictionary<string, (string type, object[] array)>
        {
            {
                "Date", ("string", new object[0])
            },
            {
                "Days", ("float", new object[0])
            },
            {
                "Gas", ("float", new object[0])
            },
            {
                "Oil", ("float", new object[0])
            },
            {
                "Water", ("float", new object[0])
            },
            {
                "Weight", ("float", new object[0])
            }
        };

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
                    List<ProductionRecord> selectedProductionRecords = new(selected.Length);

                    for (int i = 0; i < selected.Length; ++i)
                    {
                        selectedProductionRecords.Add(_multiPorosityModelService.ActiveProject.ProductionRecords[selected[i].PointIndex]);
                    }

                    _multiPorosityModelService.ActiveProject.SelectedProductionRecords = new BindableCollection<ProductionRecord>(selectedProductionRecords);

                    //this.RaisePropertyChanged(nameof(SelectedProductionRecords));
                }
            }
        }

        #endregion

        private readonly MultiPorosityModelService _multiPorosityModelService;

        public ProductionChartLogLogViewModel(MultiPorosityModelService multiPorosityModelService)
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
                    Mode = ScatterGl.ModeFlag.Lines | ScatterGl.ModeFlag.Markers,
                    XSrc = "Days",
                    YSrc = "Gas",
                    Marker = new Marker()
                    {
                        Color   = "#CC0000",
                        Size    = 8,
                        SizeSrc = "Weight"
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
                    XSrc = "Days",
                    YSrc = "Oil",
                    Marker = new Marker()
                    {
                        Color   = "#00CC00",
                        Size    = 8,
                        SizeSrc = "Weight"
                    },
                    Line = new Line()
                    {
                        Color = "#00CC00",
                        Width = 1
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
                        Type = Plotly.Models.Layouts.XAxes.TypeEnum.Log,
                        Title = new Plotly.Models.Layouts.XAxes.Title
                        {
                            Text = "Log Days"
                        },
                        AutoRange = Plotly.Models.Layouts.XAxes.AutoRangeEnum.True
                    }
                },
                YAxis = new List<YAxis>
                {
                    new YAxis
                    {
                        Type = Plotly.Models.Layouts.YAxes.TypeEnum.Log,
                        Title = new Plotly.Models.Layouts.YAxes.Title
                        {
                            Text = "Log Rate"
                        },
                        AutoRange = Plotly.Models.Layouts.YAxes.AutoRangeEnum.True
                    }
                }
                //Shapes = new ObservableCollection<Shape>()
                //{
                //    new Shape()
                //    {
                //        Type = Plotly.Models.Layouts.Shapes.TypeEnum.Line,
                //        X0 = 1,
                //        Y0 = 10000,
                //        X1 = 100000,
                //        Y1 = 1,
                //        Line= new Plotly.Models.Layouts.Shapes.Line()
                //        {
                //            Color = "#000000",
                //            Width = 3
                //        }                
                //    }
                //}
            };
        }

        private void OnPropertyChanged(object? sender,
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
                    OnProductionRecordsChanged(sender, null);
                    break;
                }
                case "ProductionRecords":
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
                }
            };
        }
    }
}