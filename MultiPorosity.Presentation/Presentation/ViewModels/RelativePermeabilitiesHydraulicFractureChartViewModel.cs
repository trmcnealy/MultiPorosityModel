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

using Prism.Mvvm;

using Title = Plotly.Models.Layouts.Title;

namespace MultiPorosity.Presentation
{
    public class RelativePermeabilitiesHydraulicFractureChartViewModel : BindableBase
    {
        #region Chart Properties

        private ObservableDictionary<string, (string type, object[] array)> dataSource = new();

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
                    _multiPorosityModelService.ActiveProject.SelectedRelativePermeabilityHydraulicFractureModels.Clear();

                    for(int i = 0; i < selected.Length; ++i)
                    {
                        _multiPorosityModelService.ActiveProject.SelectedRelativePermeabilityHydraulicFractureModels.Add(_multiPorosityModelService.ActiveProject.
                                                                                                                             RelativePermeabilityHydraulicFractureModels
                                                                                                                                 [selected[i].PointIndex]);
                    }

                    //this.RaisePropertyChanged(nameof(SelectedRelativePermeabilityModels));
                }
            }
        }

        #endregion
        
        private readonly MultiPorosityModelService _multiPorosityModelService;

        public RelativePermeabilitiesHydraulicFractureChartViewModel(MultiPorosityModelService multiPorosityModelService)
        {
            _multiPorosityModelService = multiPorosityModelService;

            _multiPorosityModelService.PropertyChanged -= OnPropertyChanged;
            _multiPorosityModelService.PropertyChanged += OnPropertyChanged;

            OnPropertyChanged(this, new PropertyChangedEventArgs("ActiveProject"));

            PlotData = new ObservableCollection<Plotly.Models.ITrace>
            {
                new ScatterGl
                {
                    Name = "Kro",
                    Mode = ScatterGl.ModeFlag.Lines,
                    XSrc = "Sw",
                    YSrc = "Kro",
                    Marker = new Plotly.Models.Traces.ScatterGls.Marker
                    {
                        Color = "#00CC00",
                    },
                    XAxis = "x1",
                    YAxis = "y1"
                },
                new ScatterGl
                {
                    Name = "Krw",
                    Mode = ScatterGl.ModeFlag.Lines,
                    XSrc = "Sw",
                    YSrc = "Krw",
                    Marker = new Plotly.Models.Traces.ScatterGls.Marker
                    {
                        Color = "#0000CC",
                    },
                    XAxis = "x1",
                    YAxis = "y1"
                },
                new ScatterGl
                {
                    Name = "Krg",
                    Mode = ScatterGl.ModeFlag.Lines,
                    XSrc = "Sw",
                    YSrc = "Krg",
                    Marker = new Plotly.Models.Traces.ScatterGls.Marker
                    {
                        Color = "#CC0000",
                    },
                    XAxis = "x1",
                    YAxis = "y2"
                },
                new ScatterGl
                {
                    Name = "Krw",
                    Mode = ScatterGl.ModeFlag.Lines,
                    XSrc = "Sw",
                    YSrc = "Krw",
                    Marker = new Plotly.Models.Traces.ScatterGls.Marker
                    {
                        Color = "#0000CC",
                    },
                    XAxis = "x1",
                    YAxis = "y2"
                }
            };

            PlotLayout = new Plotly.Models.Layout
            {
                Title = new Title
                {
                    Text = "Relative Permeabilities"
                },
                ShowLegend = true,
                Legend = new Legend
                {
                    Orientation = OrientationEnum.H,
                    XAnchor     = XAnchorEnum.Center,
                    YAnchor     = YAnchorEnum.Bottom,
                    X           = 0.5,
                    Y           = 1
                },
                XAxis = new List<XAxis>
                {
                    new XAxis
                    {
                        Type = Plotly.Models.Layouts.XAxes.TypeEnum.Linear,
                        Title = new Plotly.Models.Layouts.XAxes.Title
                        {
                            Text = "Sw"
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
                            Text = "Kro"
                        },
                        Domain = new List<object>
                        {
                            0, 0.5
                        }
                    },
                    new YAxis
                    {
                        Type = Plotly.Models.Layouts.YAxes.TypeEnum.Linear,
                        Title = new Plotly.Models.Layouts.YAxes.Title
                        {
                            Text = "Krg"
                        },
                        Domain = new List<object>
                        {
                            0.5, 1.0
                        }
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
                    _multiPorosityModelService.ActiveProject.PropertyChanged                                               -= OnPropertyChanged;
                    _multiPorosityModelService.ActiveProject.PropertyChanged                                               += OnPropertyChanged;
                    _multiPorosityModelService.ActiveProject.RelativePermeabilityHydraulicFractureModels.CollectionChanged -= OnRelativePermeabilityModelsChanged;
                    _multiPorosityModelService.ActiveProject.RelativePermeabilityHydraulicFractureModels.CollectionChanged += OnRelativePermeabilityModelsChanged;
                    OnRelativePermeabilityModelsChanged(sender, null);

                    break;
                }
                case "RelativePermeabilityHydraulicFractureModels":
                {
                    OnRelativePermeabilityModelsChanged(sender, null);

                    break;
                }
            }
        }

        private void OnRelativePermeabilityModelsChanged(object?                           sender,
                                                         NotifyCollectionChangedEventArgs? e)
        {
            RelativePermeabilityModel[]? relativePermeabilityModelsSoArray =
                _multiPorosityModelService.ActiveProject.RelativePermeabilityHydraulicFractureModels.Where(m => m.Sg == 0.0).ToArray();

            RelativePermeabilityModel[]? relativePermeabilityModelsSgArray =
                _multiPorosityModelService.ActiveProject.RelativePermeabilityHydraulicFractureModels.Where(m => m.So == 0.0).ToArray();

            DataSource = new ObservableDictionary<string, (string type, object[] array)>
            {
                //{
                //    "Sg", ("float", new RelativePermeabilityColumn(0, _relativePermeabilityModels.Where(m => m.So == 0.0).ToArray()).ToArray())
                //},
                //{
                //    "So", ("float", new RelativePermeabilityColumn(1, _relativePermeabilityModels.Where(m => m.Sg == 0.0).ToArray()).ToArray())
                //},
                {
                    "Sw", ("float", new RelativePermeabilityColumn(2, relativePermeabilityModelsSoArray).ToArray())
                },
                {
                    "Krg", ("float", new RelativePermeabilityColumn(3, relativePermeabilityModelsSgArray).ToArray())
                },
                {
                    "Kro", ("float", new RelativePermeabilityColumn(4, relativePermeabilityModelsSoArray).ToArray())
                },
                {
                    "Krw", ("float", new RelativePermeabilityColumn(5, relativePermeabilityModelsSgArray).ToArray())
                }
            };
        }

        
    }
}