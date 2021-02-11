using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

using Engineering.UI.Collections;

using MultiPorosity.Presentation.Services;

using Plotly;
using Plotly.Models.Configs;
using Plotly.Models.Layouts;
using Plotly.Models.Layouts.MapBoxs;
using Plotly.Models.Traces;
using Plotly.Models.Traces.ScatterMapBoxs;

using Prism.Mvvm;

using Title = Plotly.Models.Layouts.Title;

namespace MultiPorosity.Presentation
{
    public class WellMapViewModel : BindableBase
    {
        private static readonly string AccessToken = "pk.eyJ1IjoidHJtY25lYWx5IiwiYSI6ImNrZDN3aGNvMzBxNjQycW16Zml2M2UwZmcifQ.aT8sIrXsA2pHPSjw_U-fUA";

        #region Chart Properties

        private ObservableDictionary<string, (string type, object[] array)> dataSource = new ObservableDictionary<string, (string type, object[] array)>
        {
            {
                "Api", ("string", new object[0])
            },
            {
                "Latitude", ("double", new object[0])
            },
            {
                "Longitude", ("double", new object[0])
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
        
        private Plotly.Models.Config plotConfig;

        public Plotly.Models.Config PlotConfig
        {
            get { return plotConfig; }
            set { SetProperty(ref plotConfig, value); }
        }

        private SelectedData[] selectedRecords;
        public SelectedData[] SelectedRecords
        {
            get { return selectedRecords; }
            set
            {
                if(SetProperty(ref selectedRecords, value))
                {
                    List<string> selected = new(selectedRecords.Length);

                    (string type, object[] array) = dataSource["Api"];

                    for (int i = 0; i < selectedRecords.Length; ++i)
                    {
                        selected.Add((string)array[selectedRecords[i].PointIndex]);
                    }

                    SelectedWells = new BindableCollection<string>(selected);
                }
            }
        }

        private BindableCollection<string> _SelectedWells = new();
        public BindableCollection<string> SelectedWells
        {
            get { return _SelectedWells; }
            set
            {
                if(SetProperty(ref _SelectedWells, value))
                {
                }
            }
        }

        #endregion

        private readonly MultiPorosityModelService _multiPorosityModelService;

        public WellMapViewModel(MultiPorosityModelService multiPorosityModelService)
        {
            _multiPorosityModelService = multiPorosityModelService;
            
            _multiPorosityModelService.PropertyChanged -= OnPropertyChanged;
            _multiPorosityModelService.PropertyChanged += OnPropertyChanged;

            OnPropertyChanged(this, new PropertyChangedEventArgs("ActiveProject"));

            PlotLayout = new Plotly.Models.Layout
            {
                Title = new Title
                {
                    Text = "Production Map"
                },
                //ShowLegend = true,
                //AutoSize =  true,
                //Legend = new Legend()
                //{
                //    Orientation = OrientationEnum.H,
                //    XAnchor = XAnchorEnum.Center,
                //    YAnchor = YAnchorEnum.Bottom,
                //    X=0.5,
                //    Y=1
                //},
                DragMode = DragModeEnum.Zoom,
                MapBox = new List<MapBox>
                {
                    new MapBox
                    {
                        Style = "mapbox://styles/mapbox/dark-v10",
                        Zoom = 6,
                        Center = new Center()
                        {
                            Lon = -97.5454313491453,
                            Lat = 28.8510236460433
                        }
                        //Layers = new List<Layer>()
                        //{
                        //    new Layer()
                        //    {
                        //        SourceType = SourceTypeEnum.GeoJson
                        //    }
                        //}
                    }
                },
                Margin = new Margin()
                {
                    R = 0,
                    T = 0,
                    B = 0,
                    L = 0
                }
            };

            PlotData = new ObservableCollection<Plotly.Models.ITrace>
            {
                new ScatterMapBox()
                {
                    Name = "Surface Locations",
                    LonSrc = "Longitude",
                    LatSrc = "Latitude",
                    TextSrc = "Api",
                    Mode = ModeFlag.Markers | ModeFlag.Text,
                    Marker = new Plotly.Models.Traces.ScatterMapBoxs.Marker()
                    {
                        Color   = "#CC0000",
                        Size    = 5
                    }
                }
            };

            PlotConfig = new()
            {
                ShowLink            = true,
                LinkText            = "https://trmcnealy.github.io",
                MapboxAccessToken   = "pk.eyJ1IjoidHJtY25lYWx5IiwiYSI6ImNrZDN3aGNvMzBxNjQycW16Zml2M2UwZmcifQ.aT8sIrXsA2pHPSjw_U-fUA",
                PlotGlPixelRatio    = 2,
                DisplayModeBar      = DisplayModeBarEnum.Hover,
                FrameMargins        = 0,
                DisplayLogo         = false,
                FillFrame           = true,
                Responsive          = true,
                ScrollZoom          = ScrollZoomFlag.MapBox,
                Edits               = new(),
                ModeBarButtons      = false,
                ModeBarButtonsToAdd = null,
                //ModeBarButtonsToAdd = new ModeBarButtons[]
                //{
                //    ModeBarButtons.ZoomInMapbox, ModeBarButtons.ZoomOutMapbox
                //},
                ModeBarButtonsToRemove = new ModeBarButtons[]
                {
                    ModeBarButtons.HoverClosestCartesian,
                    ModeBarButtons.HoverCompareCartesian
                },
                ToImageButtonOptions = new ImageButtonOptions
                {
                    Format = "svg",
                    Filename = "custom_image",
                    Scale = 1
                },
                Logging = 1,
                NotifyOnLogging = 2
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
                    //_multiPorosityModelService.ActiveProject.ProductionRecords.CollectionChanged -= OnProductionRecordsChanged;
                    //_multiPorosityModelService.ActiveProject.ProductionRecords.CollectionChanged += OnProductionRecordsChanged;
                    //OnProductionRecordsChanged(sender, null);
                    break;
                }
                //case "ProductionRecords":
                //{
                //    OnProductionRecordsChanged(sender, null);

                //    break;
                //}
            }
        }

        //private void OnLocationsChanged(object? sender,
        //                                        NotifyCollectionChangedEventArgs? e)
        //{
        //    //ProductionRecord[]? productionRecordArray = _multiPorosityModelService.ActiveProject.ProductionRecords.ToArray();

        //    DataSource = new ObservableDictionary<string, (string type, object[] array)>
        //    {
        //        {
        //            "Latitude", ("double", new object[0])
        //        },
        //        {
        //            "Longitude", ("double", new object[0])
        //        }


        //        //{
        //        //    "Date", ("string", new ProductionRecordColumn(1, productionRecordArray).ToArray())
        //        //},
        //        //{
        //        //    "Days", ("float", new ProductionRecordColumn(2, productionRecordArray).ToArray())
        //        //},
        //        //{
        //        //    "Gas", ("float", new ProductionRecordColumn(3, productionRecordArray).ToArray())
        //        //},
        //        //{
        //        //    "Oil", ("float", new ProductionRecordColumn(4, productionRecordArray).ToArray())
        //        //},
        //        //{
        //        //    "Water", ("float", new ProductionRecordColumn(5, productionRecordArray).ToArray())
        //        //},
        //        //{
        //        //    "Weight", ("float", new ProductionRecordColumn(7, productionRecordArray).ToArray())
        //        //}
        //    };
        //}
    }
}