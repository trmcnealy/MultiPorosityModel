using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Threading;

using Engineering.DataSource;

using MultiPorosity.Models;
using MultiPorosity.Presentation.Services;

using Plotly;
using Plotly.Models;
using Plotly.Models.Animations;
using Plotly.Models.Layouts;
using Plotly.Models.Layouts.Sliders;
using Plotly.Models.Layouts.Sliders.Steps;
using Plotly.Models.Layouts.UpdateMenus;
using Plotly.Models.Traces;
using Plotly.Models.Traces.Sploms;
using Plotly.Models.Traces.Sploms.Markers;

using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

using DirectionEnum = Plotly.Models.Layouts.UpdateMenus.DirectionEnum;
using OrientationEnum = Plotly.Models.Layouts.Legends.OrientationEnum;
using Title = Plotly.Models.Layouts.Title;
using TriplePorosityOptimizationResults = MultiPorosity.Presentation.Models.TriplePorosityOptimizationResults;
using XAnchorEnum = Plotly.Models.Layouts.Legends.XAnchorEnum;
using YAnchorEnum = Plotly.Models.Layouts.Legends.YAnchorEnum;

namespace MultiPorosity.Presentation
{
    public class MultiPorosityResultsChartViewModel : BindableBase
    {
        #region Chart Properties

        private ObservableDictionary<string, (string type, object[] array)> dataSource = new ObservableDictionary<string, (string type, object[] array)>
        {
            {
                "Iteration", ("long", Array.Empty<object>())
            },
            {
                "SwarmIndex", ("long", Array.Empty<object>())
            },
            {
                "ParticleIndex", ("long", Array.Empty<object>())
            },
            {
                "RMS", ("double", Array.Empty<object>())
            },
            {
                "MatrixPermeability", ("double", Array.Empty<object>())
            },
            {
                "HydraulicFracturePermeability", ("double", Array.Empty<object>())
            },
            {
                "NaturalFracturePermeability", ("double", Array.Empty<object>())
            },
            {
                "HydraulicFractureHalfLength", ("double", Array.Empty<object>())
            },
            {
                "HydraulicFractureSpacing", ("double", Array.Empty<object>())
            },
            {
                "NaturalFractureSpacing", ("double", Array.Empty<object>())
            },
            {
                "Skin", ("double", Array.Empty<object>())
            }
        };

        public ObservableDictionary<string, (string type, object[] array)> DataSource
        {
            get { return dataSource; }
            set { SetProperty(ref dataSource, value); }
        }

        private ObservableCollection<ITrace> plotData;

        public ObservableCollection<ITrace> PlotData
        {
            get { return plotData; }
            set { SetProperty(ref plotData, value); }
        }

        private Layout plotLayout;

        public Layout PlotLayout
        {
            get { return plotLayout; }
            set { SetProperty(ref plotLayout, value); }
        }

        private ObservableCollection<Frames> plotFrames;

        public ObservableCollection<Frames> PlotFrames
        {
            get { return plotFrames; }
            set { SetProperty(ref plotFrames, value); }
        }

        private SelectedData[] selected;

        public SelectedData[] SelectedRecords
        {
            get { return selected; }
            set
            {
                if(SetProperty(ref selected, value))
                {
                    //ObservableCollection<ProductionRecord> selectedProductionRecords = new(selected.Length);

                    //for (int i = 0; i < selected.Length; ++i)
                    //{
                    //    selectedProductionRecords.Add(_multiPorosityModelService.ActiveProject.ProductionRecords[selected[i].PointIndex]);
                    //}

                    //_multiPorosityModelService.ActiveProject.SelectedProductionRecords = new BindableCollection<ProductionRecord>(selectedProductionRecords);

                    //this.RaisePropertyChanged(nameof(SelectedProductionRecords));
                }
            }
        }

        #endregion

        private readonly MultiPorosityModelService _multiPorosityModelService;
        private readonly IEventAggregator          _eventAggregator;

        private int _MaxIteration;

        private int _CurrentIteration;

        private readonly Dictionary<long, ((string type, object[] array) MatrixPermeability,
                                           (string type, object[] array) HydraulicFracturePermeability,
                                           (string type, object[] array) NaturalFracturePermeability,
                                           (string type, object[] array) HydraulicFractureHalfLength,
                                           (string type, object[] array) HydraulicFractureSpacing,
                                           (string type, object[] array) NaturalFractureSpacing,
                                           (string type, object[] array) Skin,
                                           (string type, object[] array) RMS)> _triplePorosityOptimizationResultsRecords = new();

        public int MaxIteration
        {
            get { return _MaxIteration; }
            set { SetProperty(ref _MaxIteration, value); }
        }

        public int CurrentIteration
        {
            get { return _CurrentIteration; }
            set
            {
                if(SetProperty(ref _CurrentIteration, value))
                {
                    ((string type, object[] array) MatrixPermeability,
                     (string type, object[] array) HydraulicFracturePermeability,
                     (string type, object[] array) NaturalFracturePermeability,
                     (string type, object[] array) HydraulicFractureHalfLength,
                     (string type, object[] array) HydraulicFractureSpacing,
                     (string type, object[] array) NaturalFractureSpacing,
                     (string type, object[] array) Skin,
                     (string type, object[] array) RMS) = _triplePorosityOptimizationResultsRecords[CurrentIteration];

                    DataSource = new()
                    {
                        {
                            "MatrixPermeability", MatrixPermeability
                        },
                        {
                            "HydraulicFracturePermeability", HydraulicFracturePermeability
                        },
                        {
                            "NaturalFracturePermeability", NaturalFracturePermeability
                        },
                        {
                            "HydraulicFractureHalfLength", HydraulicFractureHalfLength
                        },
                        {
                            "HydraulicFractureSpacing", HydraulicFractureSpacing
                        },
                        {
                            "NaturalFractureSpacing", NaturalFractureSpacing
                        },
                        {
                            "Skin", Skin
                        },
                        {
                            "RMS", RMS
                        }
                    };
                }
            }
        }

        public DelegateCommand PlayCachedResultsCommand { get; }

        public DelegateCommand PauseCachedResultsCommand { get; }

        public MultiPorosityResultsChartViewModel(MultiPorosityModelService multiPorosityModelService,
                                                  IEventAggregator          eventAggregator)
        {
            _multiPorosityModelService = multiPorosityModelService;
            _eventAggregator           = eventAggregator;
            
            PlayCachedResultsCommand  = new DelegateCommand(OnPlayCachedResults);
            PauseCachedResultsCommand = new DelegateCommand(OnPauseCachedResults);

            //_eventAggregator.GetEvent<TriplePorosityOptimizationResultsEvent>().Subscribe(OnUpdateResults);

            _multiPorosityModelService.PropertyChanged -= OnPropertyChanged;
            _multiPorosityModelService.PropertyChanged += OnPropertyChanged;

            OnPropertyChanged(this, new PropertyChangedEventArgs("ActiveProject"));

            PlotData = new ObservableCollection<ITrace>
            {
                new Splom
                {
                    Name = "Scatterplot Matrix",
                    Dimensions = new List<Dimension>
                    {
                        new Dimension
                        {
                            Label = "km", ValuesSrc = "MatrixPermeability"
                        },
                        new Dimension
                        {
                            Label = "kF", ValuesSrc = "HydraulicFracturePermeability"
                        },
                        new Dimension
                        {
                            Label = "kf", ValuesSrc = "NaturalFracturePermeability"
                        },
                        new Dimension
                        {
                            Label = "ye", ValuesSrc = "HydraulicFractureHalfLength"
                        },
                        new Dimension
                        {
                            Label = "LF", ValuesSrc = "HydraulicFractureSpacing"
                        },
                        new Dimension
                        {
                            Label = "Lf", ValuesSrc = "NaturalFractureSpacing"
                        },
                        //new Dimension
                        //{
                        //    Label = "sk",
                        //    ValuesSrc = "Skin"
                        //}
                    },
                    Marker = new Marker
                    {
                        ShowScale  = true,
                        ColorBar   = new ColorBar(),
                        ColorSrc   = "RMS",
                        ColorScale = "Jet",
                        //ColorScale = Utilities.BuildColorscale(0, 10000.0, Colors.Green_1, Colors.Blue, Colors.Red_1),
                        Size       = 5
                    }
                }
            };

            PlotLayout = new Layout
            {
                //Title = new Title
                //{
                //    Text = "Scatterplot Matrix"
                //},
                ShowLegend = true,
                AutoSize   = true,
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
                        ShowLine  = false,
                        ZeroLine  = false,
                        GridColor = Colors.white
                        //ticklen:2,
                        //tickfont:{size:10},
                        //titlefont:{size:12}
                    },
                    new XAxis
                    {
                        ShowLine  = false,
                        ZeroLine  = false,
                        GridColor = Colors.white
                        //ticklen:2,
                        //tickfont:{size:10},
                        //titlefont:{size:12}
                    },
                    new XAxis
                    {
                        ShowLine  = false,
                        ZeroLine  = false,
                        GridColor = Colors.white
                        //ticklen:2,
                        //tickfont:{size:10},
                        //titlefont:{size:12}
                    },
                    new XAxis
                    {
                        ShowLine  = false,
                        ZeroLine  = false,
                        GridColor = Colors.white
                        //ticklen:2,
                        //tickfont:{size:10},
                        //titlefont:{size:12}
                    },
                    new XAxis
                    {
                        ShowLine  = false,
                        ZeroLine  = false,
                        GridColor = Colors.white
                        //ticklen:2,
                        //tickfont:{size:10},
                        //titlefont:{size:12}
                    },
                    new XAxis
                    {
                        ShowLine  = false,
                        ZeroLine  = false,
                        GridColor = Colors.white
                        //ticklen:2,
                        //tickfont:{size:10},
                        //titlefont:{size:12}
                    },
                    //new XAxis
                    //{
                    //    ShowLine  = false,
                    //    ZeroLine  = false,
                    //    GridColor = Colors.white
                    //    //ticklen:2,
                    //    //tickfont:{size:10},
                    //    //titlefont:{size:12}
                    //}
                },
                YAxis = new List<YAxis>
                {
                    new YAxis
                    {
                        ShowLine  = false,
                        ZeroLine  = false,
                        GridColor = Colors.white
                        //ticklen:2,
                        //tickfont:{size:10},
                        //titlefont:{size:12}
                    },
                    new YAxis
                    {
                        ShowLine  = false,
                        ZeroLine  = false,
                        GridColor = Colors.white
                        //ticklen:2,
                        //tickfont:{size:10},
                        //titlefont:{size:12}
                    },
                    new YAxis
                    {
                        ShowLine  = false,
                        ZeroLine  = false,
                        GridColor = Colors.white
                        //ticklen:2,
                        //tickfont:{size:10},
                        //titlefont:{size:12}
                    },
                    new YAxis
                    {
                        ShowLine  = false,
                        ZeroLine  = false,
                        GridColor = Colors.white
                        //ticklen:2,
                        //tickfont:{size:10},
                        //titlefont:{size:12}
                    },
                    new YAxis
                    {
                        ShowLine  = false,
                        ZeroLine  = false,
                        GridColor = Colors.white
                        //ticklen:2,
                        //tickfont:{size:10},
                        //titlefont:{size:12}
                    },
                    new YAxis
                    {
                        ShowLine  = false,
                        ZeroLine  = false,
                        GridColor = Colors.white
                        //ticklen:2,
                        //tickfont:{size:10},
                        //titlefont:{size:12}
                    },
                    //new YAxis
                    //{
                    //    ShowLine  = false,
                    //    ZeroLine  = false,
                    //    GridColor = Colors.white
                    //    //ticklen:2,
                    //    //tickfont:{size:10},
                    //    //titlefont:{size:12}
                    //}
                }
            };

            _timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 1000)
            };

            _timer.Tick += OnPlayTimer;
        }

        private readonly DispatcherTimer _timer;

        private void OnPlayTimer(object?   sender,
                            EventArgs e)
        {
            CurrentIteration = ((CurrentIteration + 1) % (MaxIteration + 1));
        }

        public void OnPlayCachedResults()
        {
            _timer.Start();
        }

        public void OnPauseCachedResults()
        {
            _timer.Stop();
        }

        private static Layout BuildLayout(List<Step> slidersSteps)
        {
            return new Layout
            {
                //Title = new Title
                //{
                //    Text = "Scatterplot Matrix"
                //},
                ShowLegend = true,
                AutoSize   = true,
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
                        ShowLine  = false,
                        ZeroLine  = false,
                        GridColor = Colors.white
                        //ticklen:2,
                        //tickfont:{size:10},
                        //titlefont:{size:12}
                    },
                    new XAxis
                    {
                        ShowLine  = false,
                        ZeroLine  = false,
                        GridColor = Colors.white
                        //ticklen:2,
                        //tickfont:{size:10},
                        //titlefont:{size:12}
                    },
                    new XAxis
                    {
                        ShowLine  = false,
                        ZeroLine  = false,
                        GridColor = Colors.white
                        //ticklen:2,
                        //tickfont:{size:10},
                        //titlefont:{size:12}
                    },
                    new XAxis
                    {
                        ShowLine  = false,
                        ZeroLine  = false,
                        GridColor = Colors.white
                        //ticklen:2,
                        //tickfont:{size:10},
                        //titlefont:{size:12}
                    },
                    new XAxis
                    {
                        ShowLine  = false,
                        ZeroLine  = false,
                        GridColor = Colors.white
                        //ticklen:2,
                        //tickfont:{size:10},
                        //titlefont:{size:12}
                    },
                    new XAxis
                    {
                        ShowLine  = false,
                        ZeroLine  = false,
                        GridColor = Colors.white
                        //ticklen:2,
                        //tickfont:{size:10},
                        //titlefont:{size:12}
                    },
                    //new XAxis
                    //{
                    //    ShowLine  = false,
                    //    ZeroLine  = false,
                    //    GridColor = Colors.white
                    //    //ticklen:2,
                    //    //tickfont:{size:10},
                    //    //titlefont:{size:12}
                    //}
                },
                YAxis = new List<YAxis>
                {
                    new YAxis
                    {
                        ShowLine  = false,
                        ZeroLine  = false,
                        GridColor = Colors.white
                        //ticklen:2,
                        //tickfont:{size:10},
                        //titlefont:{size:12}
                    },
                    new YAxis
                    {
                        ShowLine  = false,
                        ZeroLine  = false,
                        GridColor = Colors.white
                        //ticklen:2,
                        //tickfont:{size:10},
                        //titlefont:{size:12}
                    },
                    new YAxis
                    {
                        ShowLine  = false,
                        ZeroLine  = false,
                        GridColor = Colors.white
                        //ticklen:2,
                        //tickfont:{size:10},
                        //titlefont:{size:12}
                    },
                    new YAxis
                    {
                        ShowLine  = false,
                        ZeroLine  = false,
                        GridColor = Colors.white
                        //ticklen:2,
                        //tickfont:{size:10},
                        //titlefont:{size:12}
                    },
                    new YAxis
                    {
                        ShowLine  = false,
                        ZeroLine  = false,
                        GridColor = Colors.white
                        //ticklen:2,
                        //tickfont:{size:10},
                        //titlefont:{size:12}
                    },
                    new YAxis
                    {
                        ShowLine  = false,
                        ZeroLine  = false,
                        GridColor = Colors.white
                        //ticklen:2,
                        //tickfont:{size:10},
                        //titlefont:{size:12}
                    },
                    //new YAxis
                    //{
                    //    ShowLine  = false,
                    //    ZeroLine  = false,
                    //    GridColor = Colors.white
                    //    //ticklen:2,
                    //    //tickfont:{size:10},
                    //    //titlefont:{size:12}
                    //}
                },
                Sliders = new List<Slider>
                {
                    new Slider
                    {
                        Pad = new Plotly.Models.Layouts.Sliders.Pad
                        {
                            T = 30
                        },
                        X   = 0.05,
                        Len = 0.95,
                        CurrentValue = new CurrentValue
                        {
                            Visible = true,
                            Prefix  = "Iteration:",
                            XAnchor = Plotly.Models.Layouts.Sliders.CurrentValues.XAnchorEnum.Center
                        },
                        Steps = slidersSteps
                    }
                },
                UpdateMenus = new List<UpdateMenu>
                {
                    new UpdateMenu
                    {
                        Type       = TypeEnum.Buttons,
                        ShowActive = false,
                        X          = 0.05,
                        Y          = 0,
                        XAnchor    = Plotly.Models.Layouts.UpdateMenus.XAnchorEnum.Right,
                        YAnchor    = Plotly.Models.Layouts.UpdateMenus.YAnchorEnum.Top,
                        Direction  = DirectionEnum.Left,
                        Pad = new Plotly.Models.Layouts.UpdateMenus.Pad
                        {
                            T = 60, R = 20
                        },
                        Buttons = new List<Button>
                        {
                            new Button
                            {
                                Label  = "Play",
                                Method = Plotly.Models.Layouts.UpdateMenus.Buttons.MethodEnum.Animate,
                                Args = new List<object>
                                {
                                    null,
                                    new Animation
                                    {
                                        FromCurrent = true,
                                        Frame = new Frame
                                        {
                                            Redraw = false, Duration = 1000
                                        },
                                        Transition = new Plotly.Models.Animations.Transition
                                        {
                                            Duration = 500
                                        }
                                    }
                                }
                            },
                            new Button
                            {
                                Label  = "Pause",
                                Method = Plotly.Models.Layouts.UpdateMenus.Buttons.MethodEnum.Animate,
                                Args = new List<object>
                                {
                                    null,
                                    new Animation
                                    {
                                        Mode = ModeEnum.Immediate,
                                        Frame = new Frame
                                        {
                                            Redraw = false, Duration = 0
                                        }
                                    }
                                }
                            }
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
                    _multiPorosityModelService.ActiveProject.PropertyChanged                           -= OnPropertyChanged;
                    _multiPorosityModelService.ActiveProject.PropertyChanged                           += OnPropertyChanged;
                    _multiPorosityModelService.ActiveProject.MultiPorosityModelResults.PropertyChanged -= OnResultsChanged;
                    _multiPorosityModelService.ActiveProject.MultiPorosityModelResults.PropertyChanged += OnResultsChanged;
                    OnResultsChanged(sender, null);

                    break;
                }
                case "MultiPorosityModelResults":
                {
                    OnResultsChanged(sender, null);

                    break;
                }
            }
        }

        private void OnUpdateResults(Array<MultiPorosity.Models.TriplePorosityOptimizationResults> results)
        {
            if(results.Count == 0)
            {
                return;
            }

            long iteration = results.FirstOrDefault().Iteration;

            ObservableDictionary<string, (string type, object[] array)> dataSource = DataSource;

            object[] Iterations                     = new TriplePorosityOptimizationResultsColumn(0,  results).ToArray();
            object[] SwarmIndexs                    = new TriplePorosityOptimizationResultsColumn(1,  results).ToArray();
            object[] ParticleIndexs                 = new TriplePorosityOptimizationResultsColumn(2,  results).ToArray();
            object[] MatrixPermeabilitys            = new TriplePorosityOptimizationResultsColumn(3,  results).ToArray();
            object[] HydraulicFracturePermeabilitys = new TriplePorosityOptimizationResultsColumn(5,  results).ToArray();
            object[] NaturalFracturePermeabilitys   = new TriplePorosityOptimizationResultsColumn(7,  results).ToArray();
            object[] HydraulicFractureHalfLengths   = new TriplePorosityOptimizationResultsColumn(9,  results).ToArray();
            object[] HydraulicFractureSpacings      = new TriplePorosityOptimizationResultsColumn(11, results).ToArray();
            object[] NaturalFractureSpacings        = new TriplePorosityOptimizationResultsColumn(13, results).ToArray();
            object[] Skins                          = new TriplePorosityOptimizationResultsColumn(15, results).ToArray();
            object[] RMSs                           = new TriplePorosityOptimizationResultsColumn(17, results).ToArray();

            dataSource["Iteration"]          = (dataSource["Iteration"].type, dataSource["Iteration"].array.AddRange(Iterations));
            dataSource["SwarmIndex"]         = (dataSource["SwarmIndex"].type, dataSource["SwarmIndex"].array.AddRange(SwarmIndexs));
            dataSource["ParticleIndex"]      = (dataSource["ParticleIndex"].type, dataSource["ParticleIndex"].array.AddRange(ParticleIndexs));
            dataSource["MatrixPermeability"] = (dataSource["MatrixPermeability"].type, dataSource["MatrixPermeability"].array.AddRange(MatrixPermeabilitys));

            dataSource["HydraulicFracturePermeability"] =
                (dataSource["HydraulicFracturePermeability"].type, dataSource["HydraulicFracturePermeability"].array.AddRange(HydraulicFracturePermeabilitys));

            dataSource["NaturalFracturePermeability"] = (dataSource["NaturalFracturePermeability"].type, dataSource["NaturalFracturePermeability"].array.AddRange(NaturalFracturePermeabilitys));
            dataSource["HydraulicFractureHalfLength"] = (dataSource["HydraulicFractureHalfLength"].type, dataSource["HydraulicFractureHalfLength"].array.AddRange(HydraulicFractureHalfLengths));
            dataSource["HydraulicFractureSpacing"]    = (dataSource["HydraulicFractureSpacing"].type, dataSource["HydraulicFractureSpacing"].array.AddRange(HydraulicFractureSpacings));
            dataSource["NaturalFractureSpacing"]      = (dataSource["NaturalFractureSpacing"].type, dataSource["NaturalFractureSpacing"].array.AddRange(NaturalFractureSpacings));
            dataSource["Skin"]                        = (dataSource["Skin"].type, dataSource["Skin"].array.AddRange(Skins));
            dataSource["RMS"]                         = (dataSource["RMS"].type, dataSource["RMS"].array.AddRange(RMSs));

            dataSource[$"Iteration{iteration}_MatrixPermeability"]            = ("double", MatrixPermeabilitys);
            dataSource[$"Iteration{iteration}_HydraulicFracturePermeability"] = ("double", HydraulicFracturePermeabilitys);
            dataSource[$"Iteration{iteration}_NaturalFracturePermeability"]   = ("double", NaturalFracturePermeabilitys);
            dataSource[$"Iteration{iteration}_HydraulicFractureHalfLength"]   = ("double", HydraulicFractureHalfLengths);
            dataSource[$"Iteration{iteration}_HydraulicFractureSpacing"]      = ("double", HydraulicFractureSpacings);
            dataSource[$"Iteration{iteration}_NaturalFractureSpacing"]        = ("double", NaturalFractureSpacings);
            dataSource[$"Iteration{iteration}_Skin"]                          = ("double", Skins);
            dataSource[$"Iteration{iteration}_RMS"]                           = ("double", RMSs);

            Step step = new Step
            {
                Method = MethodEnum.Animate,
                Label  = $"{iteration}",
                Args = new List<object>
                {
                    new object[]
                    {
                        iteration
                    },
                    new Animation
                    {
                        Mode = ModeEnum.Immediate,
                        Transition = new Plotly.Models.Animations.Transition
                        {
                            Duration = 100
                        },
                        Frame = new Frame
                        {
                            Duration = 100, Redraw = false
                        }
                    }
                }
            };

            List<Step> _slidersSteps = PlotLayout.Sliders.First().Steps;

            _slidersSteps.Add(step);

            Frames frame = new Frames
            {
                Name = $"{iteration}",
                Data = new Splom
                {
                    Dimensions = new List<Dimension>
                    {
                        new Dimension
                        {
                            Label = "km", ValuesSrc = $"Iteration{iteration}_MatrixPermeability"
                        },
                        new Dimension
                        {
                            Label = "kF", ValuesSrc = $"Iteration{iteration}_HydraulicFracturePermeability"
                        },
                        new Dimension
                        {
                            Label = "kf", ValuesSrc = $"Iteration{iteration}_NaturalFracturePermeability"
                        },
                        new Dimension
                        {
                            Label = "ye", ValuesSrc = $"Iteration{iteration}_HydraulicFractureHalfLength"
                        },
                        new Dimension
                        {
                            Label = "LF", ValuesSrc = $"Iteration{iteration}_HydraulicFractureSpacing"
                        },
                        new Dimension
                        {
                            Label = "Lf", ValuesSrc = $"Iteration{iteration}_NaturalFractureSpacing"
                        },
                        new Dimension
                        {
                            Label = "sk", ValuesSrc = $"Iteration{iteration}_Skin"
                        }
                    },
                    Marker = new Marker
                    {
                        ColorSrc   = $"Iteration{iteration}_RMS",
                        ColorScale = Utilities.BuildColorscale(0, RMSs.Cast<double>().Max(), Colors.Green, Colors.Blue, Colors.Red_1),
                        Size       = 5
                    }
                }
            };

            PlotLayout = BuildLayout(_slidersSteps);

            DataSource = dataSource;

            PlotFrames.Add(frame);
        }

        private void OnResultsChanged(object?                   sender,
                                      PropertyChangedEventArgs? e)
        {
            _CurrentIteration = 0;

            List<MultiPorosity.Services.Models.TriplePorosityOptimizationResults> triplePorosityOptimizationResultsRecord =
                TriplePorosityOptimizationResults.Convert(_multiPorosityModelService.ActiveProject.MultiPorosityModelResults.TriplePorosityOptimizationResults.ToList());

            MultiPorosity.Models.TriplePorosityOptimizationResults[] triplePorosityOptimizationResultsRecordArray =
                MultiPorosity.Services.Models.TriplePorosityOptimizationResults.Convert(triplePorosityOptimizationResultsRecord).ToArray();

            if(triplePorosityOptimizationResultsRecordArray.Length == 0)
            {
                return;
            }

            object[] Iterations = new TriplePorosityOptimizationResultsColumn(0, triplePorosityOptimizationResultsRecordArray).ToArray();
            //object[] SwarmIndexs                    = new TriplePorosityOptimizationResultsColumn(1,  triplePorosityOptimizationResultsRecordArray).ToArray();
            //object[] ParticleIndexs                 = new TriplePorosityOptimizationResultsColumn(2,  triplePorosityOptimizationResultsRecordArray).ToArray();
            //object[] MatrixPermeabilitys            = new TriplePorosityOptimizationResultsColumn(3,  triplePorosityOptimizationResultsRecordArray).ToArray();
            //object[] HydraulicFracturePermeabilitys = new TriplePorosityOptimizationResultsColumn(5,  triplePorosityOptimizationResultsRecordArray).ToArray();
            //object[] NaturalFracturePermeabilitys   = new TriplePorosityOptimizationResultsColumn(7,  triplePorosityOptimizationResultsRecordArray).ToArray();
            //object[] HydraulicFractureHalfLengths   = new TriplePorosityOptimizationResultsColumn(9,  triplePorosityOptimizationResultsRecordArray).ToArray();
            //object[] HydraulicFractureSpacings      = new TriplePorosityOptimizationResultsColumn(11, triplePorosityOptimizationResultsRecordArray).ToArray();
            //object[] NaturalFractureSpacings        = new TriplePorosityOptimizationResultsColumn(13, triplePorosityOptimizationResultsRecordArray).ToArray();
            //object[] Skins                          = new TriplePorosityOptimizationResultsColumn(15, triplePorosityOptimizationResultsRecordArray).ToArray();
            //object[] RMSs                           = new TriplePorosityOptimizationResultsColumn(17, triplePorosityOptimizationResultsRecordArray).ToArray();

            long maxIteration = Iterations.Cast<long>().Max() + 1;

            (string type, object[] array) MatrixPermeabilityRecord;
            (string type, object[] array) HydraulicFracturePermeabilityRecord;
            (string type, object[] array) NaturalFracturePermeabilityRecord;
            (string type, object[] array) HydraulicFractureHalfLengthRecord;
            (string type, object[] array) HydraulicFractureSpacingRecord;
            (string type, object[] array) NaturalFractureSpacingRecord;
            (string type, object[] array) SkinRecord;
            (string type, object[] array) RMSRecord;

            _triplePorosityOptimizationResultsRecords.Clear();

            for(long iteration = 0; iteration < maxIteration; ++iteration)
            {
                RMSRecord = ("double", new TriplePorosityOptimizationResultsColumn(3, triplePorosityOptimizationResultsRecordArray.Where(rec => rec.Iteration == iteration).ToArray()).ToArray());

                MatrixPermeabilityRecord =
                    ("double", new TriplePorosityOptimizationResultsColumn(4, triplePorosityOptimizationResultsRecordArray.Where(rec => rec.Iteration == iteration).ToArray()).ToArray());

                HydraulicFracturePermeabilityRecord =
                    ("double", new TriplePorosityOptimizationResultsColumn(6, triplePorosityOptimizationResultsRecordArray.Where(rec => rec.Iteration == iteration).ToArray()).ToArray());

                NaturalFracturePermeabilityRecord =
                    ("double", new TriplePorosityOptimizationResultsColumn(8, triplePorosityOptimizationResultsRecordArray.Where(rec => rec.Iteration == iteration).ToArray()).ToArray());

                HydraulicFractureHalfLengthRecord =
                    ("double", new TriplePorosityOptimizationResultsColumn(10, triplePorosityOptimizationResultsRecordArray.Where(rec => rec.Iteration == iteration).ToArray()).ToArray());

                HydraulicFractureSpacingRecord =
                    ("double", new TriplePorosityOptimizationResultsColumn(12, triplePorosityOptimizationResultsRecordArray.Where(rec => rec.Iteration == iteration).ToArray()).ToArray());

                NaturalFractureSpacingRecord =
                    ("double", new TriplePorosityOptimizationResultsColumn(14, triplePorosityOptimizationResultsRecordArray.Where(rec => rec.Iteration == iteration).ToArray()).ToArray());

                SkinRecord = ("double", new TriplePorosityOptimizationResultsColumn(16, triplePorosityOptimizationResultsRecordArray.Where(rec => rec.Iteration == iteration).ToArray()).ToArray());

                _triplePorosityOptimizationResultsRecords.Add(iteration,
                                                              (MatrixPermeabilityRecord, HydraulicFracturePermeabilityRecord, NaturalFracturePermeabilityRecord, HydraulicFractureHalfLengthRecord,
                                                               HydraulicFractureSpacingRecord, NaturalFractureSpacingRecord, SkinRecord, RMSRecord));
            }

            ((string type, object[] array) MatrixPermeability, (string type, object[] array) HydraulicFracturePermeability, (string type, object[] array) NaturalFracturePermeability,
             (string type, object[] array) HydraulicFractureHalfLength, (string type, object[] array) HydraulicFractureSpacing, (string type, object[] array) NaturalFractureSpacing,
             (string type, object[] array) Skin, (string type, object[] array) RMS) = _triplePorosityOptimizationResultsRecords[CurrentIteration];

            MaxIteration = (int)maxIteration - 1;

            DataSource = new()
            {
                {
                    "MatrixPermeability", MatrixPermeability
                },
                {
                    "HydraulicFracturePermeability", HydraulicFracturePermeability
                },
                {
                    "NaturalFracturePermeability", NaturalFracturePermeability
                },
                {
                    "HydraulicFractureHalfLength", HydraulicFractureHalfLength
                },
                {
                    "HydraulicFractureSpacing", HydraulicFractureSpacing
                },
                {
                    "NaturalFractureSpacing", NaturalFractureSpacing
                },
                {
                    "Skin", Skin
                },
                {
                    "RMS", RMS
                }
            };
        }

    }
}
