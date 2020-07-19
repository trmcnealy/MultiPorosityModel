using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Engineering.DataSource.OilGas;

using Kokkos;

using MultiPorosity.Models;
using MultiPorosity.Services;

using NumericalMethods.DataStorage;

using OilGas.Data;
using OilGas.Data.RRC.Texas;

using Engineering.DataSource.Tools;

namespace MultiPorosity.Driver
{
    internal class Program
    {
        private readonly Random _rand = new Random();

        [STAThread]
        private static void Main(string[] args)
        {
            Test();

#if DEBUG
            Console.WriteLine("press any key to exit.");
            Console.ReadKey();
#endif
        }

        private static void Test()
        {
            ExecutionSpaceKind executionSpace = ExecutionSpaceKind.Cuda;

            RrcTexasDataAdapter adapter        = new RrcTexasDataAdapter();

            List<Well> wells = adapter.GetWellsByCounty("Karnes").Where(w => w.MonthlyProduction != null).ToList();

            InitArguments arguments = new InitArguments(8, -1, 0, true);

            using(ScopeGuard.Get(arguments))
            {
                foreach(Well well in wells)
                {
                    Console.WriteLine($"{well.Api}");                    

                    ReservoirProperties<double> reservoir = new ReservoirProperties<double>(executionSpace);
                    reservoir.Length = 6500.0;
                    reservoir.Width  = 348.0;
                    // reservoir.Area                        = (reservoir.Length * reservoir.Width) / 43560;
                    reservoir.Thickness       = 50.0;
                    reservoir.Porosity        = 0.06;
                    reservoir.Permeability    = 0.002;
                    reservoir.Temperature     = 275.0;
                    reservoir.InitialPressure = 7000.0;

                    WellProperties<double> wellProperties = new WellProperties<double>(executionSpace);
                    wellProperties.LateralLength      = 6500.0;
                    wellProperties.BottomholePressure = 3500.0;

                    FractureProperties<double> fracture = new FractureProperties<double>(executionSpace);
                    fracture.Count        = 60;
                    fracture.Width        = 0.1;
                    fracture.Height       = 50.0;
                    fracture.HalfLength   = 348.0;
                    fracture.Porosity     = 0.20;
                    fracture.Permeability = 184.0;
                    fracture.Skin         = 0.0;

                    NaturalFractureProperties<double> natural_fracture = new NaturalFractureProperties<double>(executionSpace);
                    natural_fracture.Count        = 60;
                    natural_fracture.Width        = 0.01;
                    natural_fracture.Porosity     = 0.10;
                    natural_fracture.Permeability = 1.0;

                    Pvt<double> pvt = new Pvt<double>();
                    pvt.OilViscosity             = 0.5;
                    pvt.OilFormationVolumeFactor = 1.5;
                    pvt.TotalCompressibility     = 0.00002;

                    MultiPorosityData<double> mpd = new MultiPorosityData<double>(executionSpace);
                    mpd.ReservoirProperties       = reservoir;
                    mpd.WellProperties            = wellProperties;
                    mpd.FractureProperties        = fracture;
                    mpd.NaturalFractureProperties = natural_fracture;
                    mpd.Pvt                       = pvt;

                    BoundConstraints<double>[] arg_limits = new BoundConstraints<double>[7];

                    // LF      = xe/nF;
                    // Lf      = ye/nf;

                    /*km*/
                    arg_limits[0] = new BoundConstraints<double>(0.0001, 0.01);

                    /*kF*/
                    arg_limits[1] = new BoundConstraints<double>(100.0, 10000.0);

                    /*kf*/
                    arg_limits[2] = new BoundConstraints<double>(0.01, 100.0);

                    /*ye*/
                    arg_limits[3] = new BoundConstraints<double>(1.0, 500.0);

                    /*LF*/
                    arg_limits[4] = new BoundConstraints<double>(50.0, 250.0);

                    /*Lf*/
                    arg_limits[5] = new BoundConstraints<double>(10.0, 150.0);

                    /*sk*/
                    arg_limits[6] = new BoundConstraints<double>(0.0, 0.0);







                    View<double, Cuda> actual_data = new View<double, Cuda>("actual_data", well.MonthlyProduction.Count);

                    View<double, Cuda> actual_time = new View<double, Cuda>("actual_time", well.MonthlyProduction.Count);

                    View<double, Cuda> weights = new View<double, Cuda>("weights", well.MonthlyProduction.Count);

                    for(ulong i0 = 0; i0 < actual_data.Extent(0); ++i0)
                    {
                        actual_data[i0] = HunterDailyData.actualAvgDailyBoe[i0];
                        actual_time[i0] = HunterDailyData.actualMonthlyTime[i0];

                        weights[i0] = 1.0;
                    }

                    //ProductionData<double> productionData = new ProductionData<double>(well.MonthlyProduction.Count);

                    //for(int i = 0; i < well.MonthlyProduction.Count; ++i)
                    //{
                    //    productionData[i].Time  = well.MonthlyProduction[i].Date;
                    //    productionData[i].Qo    = HunterDailyData.qoData[i];
                    //    productionData[i].Qw    = 0.0;
                    //    productionData[i].Qg    = HunterDailyData.qgData[i];
                    //    productionData[i].QgBoe = HunterDailyData.qgData[i] / 5.8;
                    //    productionData[i].Qt    = productionData[i].Qo + productionData[i].QgBoe;
                    //}



                    TriplePorosityModel<double, Cuda> tpm = new TriplePorosityModel<double, Cuda>(mpd);


                    uint estimatedSwarmSize = ParticleSwarmOptimizationOptions.EstimateSwarmSize(7);

                    ParticleSwarmOptimizationOptions options = new ParticleSwarmOptimizationOptions(20, estimatedSwarmSize, 150, 0.0, 0.1, 0.9, false);

                    MultiPorosityResult<double, Cuda> results = tpm.HistoryMatch(options, actual_data, actual_time, weights, arg_limits);


                }
            }
        }

        private static void TestCumulativeSum()
        {
            double[] actualMonthlyBOE =
            {
                33466.0, 35563.0, 23663.0, 21862.0, 15968.0, 20670.0, 16013.0, 13683.0, 9951.0, 8713.0, 9762.0, 8241.0, 6887.0, 6293.0, 8109.0, 6763.0, 6631.0, 6953.0, 6189.0, 1618.0,
                2668.0, 2892.0, 3933.0, 3195.0, 4613.0, 4091.0, 4490.0, 2618.0, 2890.0, 2071.0, 1878.0, 3498.0, 4374.0, 3383.0, 1343.0, 633.0, 1142.0, 1009.0, 1345.0, 1158.0, 1187.0, 908.0,
                315.0, 1985.0, 1606.0, 1744.0, 1353.0, 1346.0, 1729.0, 641.0, 0.0, 1712.0, 1897.0, 1419.0, 235.0, 1379.0, 1500.0, 1636.0, 1237.0, 1209.0, 1252.0, 1200.0, 1107.0, 1098.0,
                998.0, 1076.0, 653.0, 100.0, 433.0, 876.0, 1439.0, 647.0, 2.0, 120.0, 1715.0, 1045.0
            };

            Stopwatch sw = new Stopwatch();

            sw.Start();

            double[] cumActualMonthlyBOE = OilGas.Data.Utilities.CumulativeSum(actualMonthlyBOE);

            sw.Stop();

            Console.WriteLine($"CumulativeSum {sw.ElapsedTicks}");

            sw.Reset();

            for(int i = 0; i < cumActualMonthlyBOE.Length; i++)
            {
                Console.WriteLine(cumActualMonthlyBOE[i]);
            }

            Console.WriteLine("#######################");

            Console.WriteLine($"Scan {sw.ElapsedTicks}");

            sw.Start();

            cumActualMonthlyBOE = OilGas.Data.Utilities.CumulativeSum(actualMonthlyBOE, 0, true);

            sw.Stop();

            Console.WriteLine($"Scan {sw.ElapsedTicks}");

            for(int i = 0; i < cumActualMonthlyBOE.Length; i++)
            {
                Console.WriteLine(cumActualMonthlyBOE[i]);
            }
        }

        private static void TestHistoryMatch()
        {
            //ParallelProcessor.Initialize(new InitArguments(8,
            //                                               -1,
            //                                               0,
            //                                               true));

            InitArguments arguments = new InitArguments(8, -1, 0, true);

            double[] values = new double[7];

            using(ScopeGuard.Get(arguments))
            {
                ProductionData<double> productionData = new ProductionData<double>(131);

                for(int i = 0; i < 131; ++i)
                {
                    productionData[i].Time  = HunterDailyData.timeData[i];
                    productionData[i].Qo    = HunterDailyData.qoData[i];
                    productionData[i].Qw    = HunterDailyData.qwData[i];
                    productionData[i].Qg    = HunterDailyData.qgData[i];
                    productionData[i].QgBoe = HunterDailyData.qgData[i] / 5.8;
                    productionData[i].Qt    = productionData[i].Qo + productionData[i].Qw + productionData[i].QgBoe;
                }

                ReservoirProperties<double> reservoir = new ReservoirProperties<double>();
                reservoir.Length = 6500.0;
                reservoir.Width  = 348.0;
                // reservoir.Area                        = (reservoir.Length * reservoir.Width) / 43560;
                reservoir.Thickness       = 50.0;
                reservoir.Porosity        = 0.06;
                reservoir.Permeability    = 0.002;
                reservoir.Temperature     = 275.0;
                reservoir.InitialPressure = 7000.0;

                WellProperties<double> well = new WellProperties<double>();
                well.LateralLength      = 6500.0;
                well.BottomholePressure = 3500.0;

                FractureProperties<double> fracture = new FractureProperties<double>();
                fracture.Count        = 60;
                fracture.Width        = 0.1;
                fracture.Height       = 50.0;
                fracture.HalfLength   = 348.0;
                fracture.Porosity     = 0.20;
                fracture.Permeability = 184.0;
                fracture.Skin         = 0.0;

                NaturalFractureProperties<double> natural_fracture = new NaturalFractureProperties<double>();
                natural_fracture.Count        = 60;
                natural_fracture.Width        = 0.01;
                natural_fracture.Porosity     = 0.10;
                natural_fracture.Permeability = 1.0;

                Pvt<double> pvt = new Pvt<double>();
                pvt.OilViscosity             = 0.5;
                pvt.OilFormationVolumeFactor = 1.5;
                pvt.TotalCompressibility     = 0.00002;

                MultiPorosityData<double> mpd = new MultiPorosityData<double>();                
                mpd.ReservoirProperties       = reservoir;
                mpd.WellProperties            = well;
                mpd.FractureProperties        = fracture;
                mpd.NaturalFractureProperties = natural_fracture;
                mpd.Pvt                       = pvt;

                BoundConstraints<double>[] arg_limits = new BoundConstraints<double>[7];

                // LF      = xe/nF;
                // Lf      = ye/nf;

                // Hippo Hunter 1
                // xe = 6500
                // Matrix Perm (md)         1.900
                // Hyd Frac Perm (md)       184
                // # of Hyd Frac            60
                // Frac Half Length (ft)    348
                // Nat Frac Perm (md)       0.8
                // Total # of Nat Frac      60*10
                //
                // Hippo Hunter 2
                // Matrix Perm (md)         2.260
                // Hyd Frac Perm (md)       86
                // # of Hyd Frac            60
                // Frac Half Length (ft)	533
                // Nat Frac Perm (md)       0.5
                // Total # of Nat Frac      60*18

                /*km*/
                arg_limits[0] = new BoundConstraints<double>(0.0001, 0.01);

                /*kF*/
                arg_limits[1] = new BoundConstraints<double>(100.0, 10000.0);

                /*kf*/
                arg_limits[2] = new BoundConstraints<double>(0.01, 100.0);

                /*ye*/
                arg_limits[3] = new BoundConstraints<double>(100.0, 1000.0);

                /*LF*/
                arg_limits[4] = new BoundConstraints<double>(50.0, 250.0);

                /*Lf*/
                arg_limits[5] = new BoundConstraints<double>(10.0, 150.0);

                /*sk*/
                arg_limits[6] = new BoundConstraints<double>(0.0, 0.0);

                ///*km*/ arg_limits_mirror(0) = System.ValueLimits<double>(0.001, 0.002);
                ///*kF*/ arg_limits_mirror(1) = System.ValueLimits<double>(100.0, 200.0);
                ///*kf*/ arg_limits_mirror(2) = System.ValueLimits<double>(0.001, 10.0);
                ///*ye*/ arg_limits_mirror(3) = System.ValueLimits<double>(100.0, 500.0);
                ///*LF*/ arg_limits_mirror(4) = System.ValueLimits<double>(50.0, 150.0);
                ///*Lf*/ arg_limits_mirror(5) = System.ValueLimits<double>(10.0, 100.0);
                ///*sk*/ arg_limits_mirror(6) = System.ValueLimits<double>(-2.0, 2.0);

                // Kokkos.deep_copy(arg_limits, arg_limits_mirror);

                View<double, Cuda> actual_data = new View<double, Cuda>("actual_data", HunterDailyData.actualAvgDailyBoe.Length);

                View<double, Cuda> actual_time = new View<double, Cuda>("actual_time", HunterDailyData.actualAvgDailyBoe.Length);

                View<double, Cuda> weights = new View<double, Cuda>("weights", HunterDailyData.actualAvgDailyBoe.Length);

                for(ulong i0 = 0; i0 < actual_data.Extent(0); ++i0)
                {
                    actual_data[i0] = HunterDailyData.actualAvgDailyBoe[i0]; //productionData[i0].Qt;
                    actual_time[i0] = HunterDailyData.actualMonthlyTime[i0]; //timeData[i0];

                    //if(i0 < 8 || i0 >= 16 && i0 <= 17 || i0 > 66 && i0 < 90 || i0 > 125)
                    //{
                    //    weights[i0] = 0.0001;
                    //}
                    //else if(i0 >= 119 && i0 <= 125)
                    //{
                    //    weights[i0] = 1.2;
                    //}
                    //else
                    //{
                    weights[i0] = 1.0;
                    //}
                }

                TriplePorosityModel<double, Cuda> tpm = new TriplePorosityModel<double, Cuda>(mpd);

                //NumericalMethods::Algorithms::ParticleSwarmOptimizationOptions<double> options;

                //Vector<double> best_args = pso.Execute(arg_limits, options);

                uint estimatedSwarmSize = ParticleSwarmOptimizationOptions.EstimateSwarmSize(7);

                ParticleSwarmOptimizationOptions
                    options = new ParticleSwarmOptimizationOptions(100, estimatedSwarmSize, 250, 0.0, 0.1, 0.9, true); //ParticleSwarmOptimizationOptions.Default;

                MultiPorosityResult<double, Cuda> results = tpm.HistoryMatch(options, actual_data, actual_time, weights, arg_limits);

                DataCache cachedData = results.CachedData;

                cachedData.ExportToCsv("PSO.csv");

                {
                    //Dictionary<string, string> name_map = new Dictionary<string, string>
                    //{
                    //    {
                    //        "Iteration", "Iteration"
                    //    },
                    //    {
                    //        "SwarmIndex", "SwarmIndex"
                    //    },
                    //    {
                    //        "Particle", "ParticleIndex"
                    //    },
                    //    {
                    //        "Particle0Position", "km"
                    //    },
                    //    {
                    //        "Particle1Position", "kF"
                    //    },
                    //    {
                    //        "Particle2Position", "kf"
                    //    },
                    //    {
                    //        "Particle3Position", "ye"
                    //    },
                    //    {
                    //        "Particle4Position", "LF"
                    //    },
                    //    {
                    //        "Particle5Position", "Lf"
                    //    },
                    //    {
                    //        "Particle6Position", "sk"
                    //    },
                    //    {
                    //        "Particle0Velocity", "kmVelocity"
                    //    },
                    //    {
                    //        "Particle1Velocity", "kFVelocity"
                    //    },
                    //    {
                    //        "Particle2Velocity", "kfVelocity"
                    //    },
                    //    {
                    //        "Particle3Velocity", "yeVelocity"
                    //    },
                    //    {
                    //        "Particle4Velocity", "LFVelocity"
                    //    },
                    //    {
                    //        "Particle5Velocity", "LfVelocity"
                    //    },
                    //    {
                    //        "Particle6Velocity", "skVelocity"
                    //    }
                    //};

                    //List<TriplePorosityOptimizationResults> dataset = new List<TriplePorosityOptimizationResults>();

                    //for(ulong i = 0; i < cachedData.RowCount; ++i)
                    //{
                    //    List<double> entry = new List<double>((int)cachedData.ColumnCount);

                    //    for(ulong j = 0; j < cachedData.ColumnCount; ++j)
                    //    {
                    //        entry.Add(cachedData[i,
                    //                             j]);
                    //    }

                    //    dataset.Add(new TriplePorosityOptimizationResults(entry.ToArray()));
                    //}

                    //List<string> columnNames = new List<string>();

                    //for(ulong i = 3; i < cachedData.ColumnCount - 2;)
                    //{
                    //    columnNames.Add(name_map[cachedData.GetHeader((int)i)]);

                    //    i += 2;
                    //}

                    ////{
                    ////    "$schema": "https://vega.github.io/schema/vega-lite/v4.json",
                    ////    "description": "Drag the sliders to highlight points.",
                    ////    "data": {"url": "data/cars.json"},
                    ////    "transform": [{"calculate": "year(datum.Year)", "as": "Year"}],
                    ////    "layer": [{
                    ////        "selection": {
                    ////            "CylYr": {
                    ////                "type": "single", "fields": ["Cylinders", "Year"],
                    ////                "init": {"Cylinders": 4, "Year": 1977},
                    ////                "bind": {
                    ////                    "Cylinders": {"input": "range", "min": 3, "max": 8, "step": 1},
                    ////                    "Year": {"input": "range", "min": 1969, "max": 1981, "step": 1}
                    ////                }
                    ////            }
                    ////        },
                    ////        "mark": "circle",
                    ////        "encoding": {
                    ////            "x": {"field": "Horsepower", "type": "quantitative"},
                    ////            "y": {"field": "Miles_per_Gallon", "type": "quantitative"},
                    ////            "color": {
                    ////                "condition": {"selection": "CylYr", "field": "Origin", "type": "nominal"},
                    ////                "value": "grey"
                    ////            }
                    ////        }
                    ////    }, {
                    ////        "transform": [{"filter": {"selection": "CylYr"}}],
                    ////        "mark": "circle",
                    ////        "encoding": {
                    ////            "x": {"field": "Horsepower", "type": "quantitative"},
                    ////            "y": {"field": "Miles_per_Gallon", "type": "quantitative"},
                    ////            "color": {"field": "Origin", "type": "nominal"},
                    ////            "size": {"value": 100}
                    ////        }
                    ////    }]
                    ////}

                    //Specification specification = new Specification
                    //{
                    //    Transform = new List<Transform>
                    //    {
                    //        new Transform
                    //        {
                    //            Filter = new Predicate
                    //            {
                    //                Selection = "Iteration"
                    //            }
                    //        }
                    //    },
                    //    Repeat = new RepeatMapping
                    //    {
                    //        Row = columnNames, Column = columnNames
                    //    },
                    //    Spec = new SpecClass
                    //    {
                    //        DataSource = new DataSource
                    //        {
                    //            Name = nameof(dataset)
                    //        },
                    //        Mark = BoxPlot.Circle,
                    //        Encoding = new Encoding
                    //        {
                    //            X = new XClass
                    //            {
                    //                Type = StandardType.Quantitative,
                    //                Field = new RepeatRef
                    //                {
                    //                    Repeat = RepeatEnum.Column
                    //                }
                    //            },
                    //            Y = new YClass
                    //            {
                    //                Type = StandardType.Quantitative,
                    //                Field = new RepeatRef
                    //                {
                    //                    Repeat = RepeatEnum.Row
                    //                }
                    //            },
                    //            Color = new DefWithConditionMarkPropFieldDefGradientStringNull
                    //            {
                    //                Type = StandardType.Nominal, Field = "SwarmIndex"
                    //            }
                    //        },
                    //        Selection = new Dictionary<string, SelectionDef>
                    //        {
                    //            {
                    //                "IterationSelection", new SelectionDef
                    //                {
                    //                    Type = SelectionDefType.Single,
                    //                    Fields = new List<string>
                    //                    {
                    //                        "Iteration"
                    //                    },
                    //                    Init = new Dictionary<string, InitValue>
                    //                    {
                    //                        {
                    //                            "Iteration", 0
                    //                        }
                    //                    },
                    //                    Bind = new Dictionary<string, AnyStream>
                    //                    {
                    //                        {
                    //                            "Iteration", new AnyBinding
                    //                            {
                    //                                Input = "range",
                    //                                Min   = 0.0,
                    //                                Max   = 99.0
                    //                            }
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //};

                    ////Chart chart = new Chart($"TriplePorosityModel",
                    ////                        specification,
                    ////                        1000,
                    ////                        750);

                    ////chart.ShowInBrowser();
                }

                //Console.WriteLine("RMS Error");
                //Console.WriteLine(results.Error);

                View<double, Cuda> best_args = results.Args;

                Console.WriteLine("best_args");

                for(ulong i = 0; i < best_args.Size(); ++i)
                {
                    values[i] = best_args[i];

                    Console.WriteLine(values[i]);
                }

                //Console.WriteLine();

                ////View<double, Cuda> timeView = new View<double, Cuda>("time",
                ////                                                     4);

                ////for(ulong i0 = 0; i0 < timeView.Extent(0); ++i0)
                ////{
                ////    timeView[i0] = 15 + 30 * i0;
                ////}

                //View<double, Cuda> best_args = new View<double, Cuda>("args",
                //                                                      7);

                //double[] values =
                //{
                //    0.006064035, 451.7930851, 4.277795829, 177.2940392, 77.86379899, 73.58321739 ,0.0
                //};

                //best_args[0] = values[0];
                //best_args[1] = values[1];
                //best_args[2] = values[2];
                //best_args[3] = values[3];
                //best_args[4] = values[4];
                //best_args[5] = values[5];
                //best_args[6] = values[6];

                View<double, Cuda> simulated_data = tpm.Calculate(actual_time, best_args);

                Console.WriteLine("simulated_data");

                for(ulong i0 = 0; i0 < simulated_data.Size(); ++i0)
                {
                    Console.WriteLine(simulated_data[i0]);
                }

                {
                    //List<InlineDatasetElement> dataset = new List<InlineDatasetElement>();

                    //for(ulong i = 0; i < actual_data.Size(); ++i)
                    //{
                    //    dataset.Add(new Dictionary<string, object>(4)
                    //    {
                    //        {
                    //            "API", "##-###-#####"
                    //        },
                    //        {
                    //            "Day", actual_time[i]
                    //        },
                    //        {
                    //            "Type", "Actual"
                    //        },
                    //        {
                    //            "Rate", actual_data[i]
                    //        }
                    //    });
                    //}

                    //for(ulong i = 0; i < simulated_data.Size(); ++i)
                    //{
                    //    dataset.Add(new Dictionary<string, object>(4)
                    //    {
                    //        {
                    //            "API", "##-###-#####"
                    //        },
                    //        {
                    //            "Day", actual_time[i]
                    //        },
                    //        {
                    //            "Type", "Simulated"
                    //        },
                    //        {
                    //            "Rate", simulated_data[i]
                    //        }
                    //    });
                    //}

                    //Specification specification = new Specification
                    //{
                    //    Data = new DataSource
                    //    {
                    //        Values = dataset
                    //    },
                    //    Layer = new List<LayerSpec>
                    //    {
                    //        new LayerSpec
                    //        {
                    //            Encoding = new LayerEncoding
                    //            {
                    //                X = new XClass
                    //                {
                    //                    Type = StandardType.Quantitative, Field = "Day"
                    //                },
                    //                Y = new YClass
                    //                {
                    //                    Type = StandardType.Quantitative, Field = "Rate"
                    //                },
                    //                Color = new DefWithConditionMarkPropFieldDefGradientStringNull
                    //                {
                    //                    Type = StandardType.Nominal, Field = "Type"
                    //                }
                    //            },
                    //            Layer = new List<LayerSpec>
                    //            {
                    //                new LayerSpec
                    //                {
                    //                    Mark = BoxPlot.Line
                    //                },
                    //                new LayerSpec
                    //                {
                    //                    Mark = BoxPlot.Circle
                    //                }
                    //            }
                    //        }
                    //    }
                    //};

                    //Chart chart = new Chart("TriplePorosityModel",
                    //                        specification,
                    //                        1000,
                    //                        750);

                    //chart.ShowInBrowser();
                }
            }

            //ParallelProcessor.Shutdown();
        }

        private static void TestPrediction()
        {
            ////ParallelProcessor.Initialize(new InitArguments(8,
            ////                                               -1,
            ////                                               0,
            ////                                               true));

            //InitArguments arguments = new InitArguments(8, -1, 0, true);

            //double[] values = new double[7];

            //using(ScopeGuard.Get(arguments))
            //{
            //    ProductionData<double> productionData = new ProductionData<double>(131);

            //    for(int i = 0; i < 131; ++i)
            //    {
            //        productionData[i].Time  = HunterDailyData.timeData[i];
            //        productionData[i].Qo    = HunterDailyData.qoData[i];
            //        productionData[i].Qw    = HunterDailyData.qwData[i];
            //        productionData[i].Qg    = HunterDailyData.qgData[i];
            //        productionData[i].QgBoe = HunterDailyData.qgData[i] / 5.8;
            //        productionData[i].Qt    = productionData[i].Qo + productionData[i].Qw + productionData[i].QgBoe;
            //    }

            //    ReservoirProperties<double> reservoir = new ReservoirProperties<double>();
            //    reservoir.Length = 6500.0;
            //    reservoir.Width  = 348.0;
            //    // reservoir.Area                        = (reservoir.Length * reservoir.Width) / 43560;
            //    reservoir.Thickness       = 125.0;
            //    reservoir.Porosity        = 0.06;
            //    reservoir.Permeability    = 0.002;
            //    reservoir.Temperature     = 275.0;
            //    reservoir.InitialPressure = 7000.0;

            //    WellProperties<double> well = new WellProperties<double>();
            //    well.LateralLength      = 6500.0;
            //    well.BottomholePressure = 3500.0;

            //    FractureProperties<double> fracture = new FractureProperties<double>();
            //    fracture.Count        = 60;
            //    fracture.Width        = 0.1;
            //    fracture.Height       = 50.0;
            //    fracture.HalfLength   = 348.0;
            //    fracture.Porosity     = 0.20;
            //    fracture.Permeability = 184.0;
            //    fracture.Skin         = 0.0;

            //    NaturalFractureProperties<double> natural_fracture = new NaturalFractureProperties<double>();
            //    natural_fracture.Count        = 60;
            //    natural_fracture.Width        = 0.01;
            //    natural_fracture.Porosity     = 0.10;
            //    natural_fracture.Permeability = 1.0;

            //    Pvt<double> pvt = new Pvt<double>();
            //    pvt.OilViscosity             = 0.5;
            //    pvt.OilFormationVolumeFactor = 1.5;
            //    pvt.TotalCompressibility     = 0.00002;

            //    MultiPorosityData<double> mpd = new MultiPorosityData<double>();
            //    mpd.ProductionData            = productionData;
            //    mpd.ReservoirProperties       = reservoir;
            //    mpd.WellProperties            = well;
            //    mpd.FractureProperties        = fracture;
            //    mpd.NaturalFractureProperties = natural_fracture;
            //    mpd.Pvt                       = pvt;

            //    BoundConstraints<double>[] arg_limits = new BoundConstraints<double>[7];

            //    // LF      = xe/nF;
            //    // Lf      = ye/nf;

            //    // Hippo Hunter 1
            //    // xe = 6500
            //    // Matrix Perm (md)         1.900
            //    // Hyd Frac Perm (md)       184
            //    // # of Hyd Frac            60
            //    // Frac Half Length (ft)    348
            //    // Nat Frac Perm (md)       0.8
            //    // Total # of Nat Frac      60*10
            //    //
            //    // Hippo Hunter 2
            //    // Matrix Perm (md)         2.260
            //    // Hyd Frac Perm (md)       86
            //    // # of Hyd Frac            60
            //    // Frac Half Length (ft)	533
            //    // Nat Frac Perm (md)       0.5
            //    // Total # of Nat Frac      60*18

            //    /*km*/
            //    arg_limits[0] = new BoundConstraints<double>(0.0001, 0.01);

            //    /*kF*/
            //    arg_limits[1] = new BoundConstraints<double>(100.0, 10000.0);

            //    /*kf*/
            //    arg_limits[2] = new BoundConstraints<double>(0.01, 1000.0);

            //    /*ye*/
            //    arg_limits[3] = new BoundConstraints<double>(100.0, 1000.0);

            //    /*LF*/
            //    arg_limits[4] = new BoundConstraints<double>(50.0, 250.0);

            //    /*Lf*/
            //    arg_limits[5] = new BoundConstraints<double>(10.0, 150.0);

            //    /*sk*/
            //    arg_limits[6] = new BoundConstraints<double>(0.0, 0.0);

            //    ///*km*/ arg_limits_mirror(0) = System.ValueLimits<double>(0.001, 0.002);
            //    ///*kF*/ arg_limits_mirror(1) = System.ValueLimits<double>(100.0, 200.0);
            //    ///*kf*/ arg_limits_mirror(2) = System.ValueLimits<double>(0.001, 10.0);
            //    ///*ye*/ arg_limits_mirror(3) = System.ValueLimits<double>(100.0, 500.0);
            //    ///*LF*/ arg_limits_mirror(4) = System.ValueLimits<double>(50.0, 150.0);
            //    ///*Lf*/ arg_limits_mirror(5) = System.ValueLimits<double>(10.0, 100.0);
            //    ///*sk*/ arg_limits_mirror(6) = System.ValueLimits<double>(-2.0, 2.0);

            //    // Kokkos.deep_copy(arg_limits, arg_limits_mirror);

            //    int months = 36;

            //    View<double, Cuda> actual_data = new View<double, Cuda>("actual_data", months);

            //    View<double, Cuda> actual_time = new View<double, Cuda>("actual_time", months);

            //    View<double, Cuda> weights = new View<double, Cuda>("weights", months);

            //    for(ulong i0 = 0; i0 < actual_data.Extent(0); ++i0)
            //    {
            //        actual_data[i0] = HunterDailyData.actualAvgDailyBoe[i0]; //productionData[i0].Qt;
            //        actual_time[i0] = HunterDailyData.actualMonthlyTime[i0]; //timeData[i0];

            //        //if(i0 < 8 || i0 >= 16 && i0 <= 17 || i0 > 66 && i0 < 90 || i0 > 125)
            //        //{
            //        //    weights[i0] = 0.0001;
            //        //}
            //        //else if(i0 >= 119 && i0 <= 125)
            //        //{
            //        //    weights[i0] = 1.2;
            //        //}
            //        //else
            //        //{
            //        weights[i0] = 1.0;
            //        //}
            //    }

            //    TriplePorosityModel<double, Cuda> tpm = new TriplePorosityModel<double, Cuda>(mpd);

            //    //NumericalMethods::Algorithms::ParticleSwarmOptimizationOptions<double> options;

            //    //Vector<double> best_args = pso.Execute(arg_limits, options);
            //    ParticleSwarmOptimizationOptions options = new ParticleSwarmOptimizationOptions(100, 10, 250, 0.0, 0.1, 0.9, false); //ParticleSwarmOptimizationOptions.Default;

            //    MultiPorosityResult<double, Cuda> results = tpm.HistoryMatch(options, actual_data, actual_time, weights, arg_limits);

            //    //DataCache cachedData = results.CachedData;

            //    {
            //        //Dictionary<string, string> name_map = new Dictionary<string, string>
            //        //{
            //        //    {
            //        //        "Iteration", "Iteration"
            //        //    },
            //        //    {
            //        //        "SwarmIndex", "SwarmIndex"
            //        //    },
            //        //    {
            //        //        "Particle", "ParticleIndex"
            //        //    },
            //        //    {
            //        //        "Particle0Position", "km"
            //        //    },
            //        //    {
            //        //        "Particle1Position", "kF"
            //        //    },
            //        //    {
            //        //        "Particle2Position", "kf"
            //        //    },
            //        //    {
            //        //        "Particle3Position", "ye"
            //        //    },
            //        //    {
            //        //        "Particle4Position", "LF"
            //        //    },
            //        //    {
            //        //        "Particle5Position", "Lf"
            //        //    },
            //        //    {
            //        //        "Particle6Position", "sk"
            //        //    },
            //        //    {
            //        //        "Particle0Velocity", "kmVelocity"
            //        //    },
            //        //    {
            //        //        "Particle1Velocity", "kFVelocity"
            //        //    },
            //        //    {
            //        //        "Particle2Velocity", "kfVelocity"
            //        //    },
            //        //    {
            //        //        "Particle3Velocity", "yeVelocity"
            //        //    },
            //        //    {
            //        //        "Particle4Velocity", "LFVelocity"
            //        //    },
            //        //    {
            //        //        "Particle5Velocity", "LfVelocity"
            //        //    },
            //        //    {
            //        //        "Particle6Velocity", "skVelocity"
            //        //    }
            //        //};

            //        //List<TriplePorosityOptimizationResults> dataset = new List<TriplePorosityOptimizationResults>();

            //        //for(ulong i = 0; i < cachedData.RowCount; ++i)
            //        //{
            //        //    List<double> entry = new List<double>((int)cachedData.ColumnCount);

            //        //    for(ulong j = 0; j < cachedData.ColumnCount; ++j)
            //        //    {
            //        //        entry.Add(cachedData[i,
            //        //                             j]);
            //        //    }

            //        //    dataset.Add(new TriplePorosityOptimizationResults(entry.ToArray()));
            //        //}

            //        //List<string> columnNames = new List<string>();

            //        //for(ulong i = 3; i < cachedData.ColumnCount - 2;)
            //        //{
            //        //    columnNames.Add(name_map[cachedData.GetHeader((int)i)]);

            //        //    i += 2;
            //        //}

            //        ////{
            //        ////    "$schema": "https://vega.github.io/schema/vega-lite/v4.json",
            //        ////    "description": "Drag the sliders to highlight points.",
            //        ////    "data": {"url": "data/cars.json"},
            //        ////    "transform": [{"calculate": "year(datum.Year)", "as": "Year"}],
            //        ////    "layer": [{
            //        ////        "selection": {
            //        ////            "CylYr": {
            //        ////                "type": "single", "fields": ["Cylinders", "Year"],
            //        ////                "init": {"Cylinders": 4, "Year": 1977},
            //        ////                "bind": {
            //        ////                    "Cylinders": {"input": "range", "min": 3, "max": 8, "step": 1},
            //        ////                    "Year": {"input": "range", "min": 1969, "max": 1981, "step": 1}
            //        ////                }
            //        ////            }
            //        ////        },
            //        ////        "mark": "circle",
            //        ////        "encoding": {
            //        ////            "x": {"field": "Horsepower", "type": "quantitative"},
            //        ////            "y": {"field": "Miles_per_Gallon", "type": "quantitative"},
            //        ////            "color": {
            //        ////                "condition": {"selection": "CylYr", "field": "Origin", "type": "nominal"},
            //        ////                "value": "grey"
            //        ////            }
            //        ////        }
            //        ////    }, {
            //        ////        "transform": [{"filter": {"selection": "CylYr"}}],
            //        ////        "mark": "circle",
            //        ////        "encoding": {
            //        ////            "x": {"field": "Horsepower", "type": "quantitative"},
            //        ////            "y": {"field": "Miles_per_Gallon", "type": "quantitative"},
            //        ////            "color": {"field": "Origin", "type": "nominal"},
            //        ////            "size": {"value": 100}
            //        ////        }
            //        ////    }]
            //        ////}

            //        //Specification specification = new Specification
            //        //{
            //        //    Transform = new List<Transform>
            //        //    {
            //        //        new Transform
            //        //        {
            //        //            Filter = new Predicate
            //        //            {
            //        //                Selection = "Iteration"
            //        //            }
            //        //        }
            //        //    },
            //        //    Repeat = new RepeatMapping
            //        //    {
            //        //        Row = columnNames, Column = columnNames
            //        //    },
            //        //    Spec = new SpecClass
            //        //    {
            //        //        DataSource = new DataSource
            //        //        {
            //        //            Name = nameof(dataset)
            //        //        },
            //        //        Mark = BoxPlot.Circle,
            //        //        Encoding = new Encoding
            //        //        {
            //        //            X = new XClass
            //        //            {
            //        //                Type = StandardType.Quantitative,
            //        //                Field = new RepeatRef
            //        //                {
            //        //                    Repeat = RepeatEnum.Column
            //        //                }
            //        //            },
            //        //            Y = new YClass
            //        //            {
            //        //                Type = StandardType.Quantitative,
            //        //                Field = new RepeatRef
            //        //                {
            //        //                    Repeat = RepeatEnum.Row
            //        //                }
            //        //            },
            //        //            Color = new DefWithConditionMarkPropFieldDefGradientStringNull
            //        //            {
            //        //                Type = StandardType.Nominal, Field = "SwarmIndex"
            //        //            }
            //        //        },
            //        //        Selection = new Dictionary<string, SelectionDef>
            //        //        {
            //        //            {
            //        //                "IterationSelection", new SelectionDef
            //        //                {
            //        //                    Type = SelectionDefType.Single,
            //        //                    Fields = new List<string>
            //        //                    {
            //        //                        "Iteration"
            //        //                    },
            //        //                    Init = new Dictionary<string, InitValue>
            //        //                    {
            //        //                        {
            //        //                            "Iteration", 0
            //        //                        }
            //        //                    },
            //        //                    Bind = new Dictionary<string, AnyStream>
            //        //                    {
            //        //                        {
            //        //                            "Iteration", new AnyBinding
            //        //                            {
            //        //                                Input = "range",
            //        //                                Min   = 0.0,
            //        //                                Max   = 99.0
            //        //                            }
            //        //                        }
            //        //                    }
            //        //                }
            //        //            }
            //        //        }
            //        //    }
            //        //};

            //        ////Chart chart = new Chart($"TriplePorosityModel",
            //        ////                        specification,
            //        ////                        1000,
            //        ////                        750);

            //        ////chart.ShowInBrowser();
            //    }

            //    //Console.WriteLine("RMS Error");
            //    //Console.WriteLine(results.Error);

            //    View<double, Cuda> best_args = results.Args;

            //    Console.WriteLine("best_args");

            //    for(ulong i = 0; i < best_args.Size(); ++i)
            //    {
            //        values[i] = best_args[i];

            //        Console.WriteLine(values[i]);
            //    }

            //    //Console.WriteLine();

            //    ////View<double, Cuda> timeView = new View<double, Cuda>("time",
            //    ////                                                     4);

            //    ////for(ulong i0 = 0; i0 < timeView.Extent(0); ++i0)
            //    ////{
            //    ////    timeView[i0] = 15 + 30 * i0;
            //    ////}

            //    //View<double, Cuda> best_args = new View<double, Cuda>("args",
            //    //                                                      7);

            //    //double[] values =
            //    //{
            //    //    0.006064035, 451.7930851, 4.277795829, 177.2940392, 77.86379899, 73.58321739 ,0.0
            //    //};

            //    //best_args[0] = values[0];
            //    //best_args[1] = values[1];
            //    //best_args[2] = values[2];
            //    //best_args[3] = values[3];
            //    //best_args[4] = values[4];
            //    //best_args[5] = values[5];
            //    //best_args[6] = values[6];

            //    View<double, Cuda> simulated_data = tpm.Calculate(actual_time, best_args);

            //    Console.WriteLine("simulated_data");

            //    for(ulong i0 = 0; i0 < simulated_data.Size(); ++i0)
            //    {
            //        Console.WriteLine(simulated_data[i0]);
            //    }

            //    {
            //        //List<InlineDatasetElement> dataset = new List<InlineDatasetElement>();

            //        //for(ulong i = 0; i < actual_data.Size(); ++i)
            //        //{
            //        //    dataset.Add(new Dictionary<string, object>(4)
            //        //    {
            //        //        {
            //        //            "API", "##-###-#####"
            //        //        },
            //        //        {
            //        //            "Day", actual_time[i]
            //        //        },
            //        //        {
            //        //            "Type", "Actual"
            //        //        },
            //        //        {
            //        //            "Rate", actual_data[i]
            //        //        }
            //        //    });
            //        //}

            //        //for(ulong i = 0; i < simulated_data.Size(); ++i)
            //        //{
            //        //    dataset.Add(new Dictionary<string, object>(4)
            //        //    {
            //        //        {
            //        //            "API", "##-###-#####"
            //        //        },
            //        //        {
            //        //            "Day", actual_time[i]
            //        //        },
            //        //        {
            //        //            "Type", "Simulated"
            //        //        },
            //        //        {
            //        //            "Rate", simulated_data[i]
            //        //        }
            //        //    });
            //        //}

            //        //Specification specification = new Specification
            //        //{
            //        //    Data = new DataSource
            //        //    {
            //        //        Values = dataset
            //        //    },
            //        //    Layer = new List<LayerSpec>
            //        //    {
            //        //        new LayerSpec
            //        //        {
            //        //            Encoding = new LayerEncoding
            //        //            {
            //        //                X = new XClass
            //        //                {
            //        //                    Type = StandardType.Quantitative, Field = "Day"
            //        //                },
            //        //                Y = new YClass
            //        //                {
            //        //                    Type = StandardType.Quantitative, Field = "Rate"
            //        //                },
            //        //                Color = new DefWithConditionMarkPropFieldDefGradientStringNull
            //        //                {
            //        //                    Type = StandardType.Nominal, Field = "Type"
            //        //                }
            //        //            },
            //        //            Layer = new List<LayerSpec>
            //        //            {
            //        //                new LayerSpec
            //        //                {
            //        //                    Mark = BoxPlot.Line
            //        //                },
            //        //                new LayerSpec
            //        //                {
            //        //                    Mark = BoxPlot.Circle
            //        //                }
            //        //            }
            //        //        }
            //        //    }
            //        //};

            //        //Chart chart = new Chart("TriplePorosityModel",
            //        //                        specification,
            //        //                        1000,
            //        //                        750);

            //        //chart.ShowInBrowser();
            //    }
            //}

            ////ParallelProcessor.Shutdown();

            //// Prediction

            //double[] predictTimeData = HunterDailyData.actualMonthlyTime; //Sequence.Linear(1.0, 2299.0, 1.0);
            //double[] predictQt;

            //using(ScopeGuard.Get(arguments))
            //{
            //    ProductionData<double> productionData = new ProductionData<double>(131);

            //    for(int i = 0; i < 131; ++i)
            //    {
            //        productionData[i].Time  = HunterDailyData.timeData[i];
            //        productionData[i].Qo    = HunterDailyData.qoData[i];
            //        productionData[i].Qw    = 0.0;
            //        productionData[i].Qg    = HunterDailyData.qgData[i];
            //        productionData[i].QgBoe = HunterDailyData.qgData[i] / 5.8;
            //        productionData[i].Qt    = productionData[i].Qo + productionData[i].QgBoe;
            //    }

            //    ReservoirProperties<double> reservoir = new ReservoirProperties<double>();
            //    reservoir.Length = 6500.0;
            //    reservoir.Width  = 348.0;
            //    // reservoir.Area                        = (reservoir.Length * reservoir.Width) / 43560;
            //    reservoir.Thickness       = 125.0;
            //    reservoir.Porosity        = 0.06;
            //    reservoir.Permeability    = 0.002;
            //    reservoir.Temperature     = 275.0;
            //    reservoir.InitialPressure = 7000.0;

            //    WellProperties<double> well = new WellProperties<double>();
            //    well.LateralLength      = 6500.0;
            //    well.BottomholePressure = 3500.0;

            //    FractureProperties<double> fracture = new FractureProperties<double>();
            //    fracture.Count        = 60;
            //    fracture.Width        = 0.1;
            //    fracture.Height       = 50.0;
            //    fracture.HalfLength   = 348.0;
            //    fracture.Porosity     = 0.20;
            //    fracture.Permeability = 184.0;
            //    fracture.Skin         = 0.0;

            //    NaturalFractureProperties<double> natural_fracture = new NaturalFractureProperties<double>();
            //    natural_fracture.Count        = 60;
            //    natural_fracture.Width        = 0.01;
            //    natural_fracture.Porosity     = 0.10;
            //    natural_fracture.Permeability = 1.0;

            //    Pvt<double> pvt = new Pvt<double>();
            //    pvt.OilViscosity             = 0.5;
            //    pvt.OilFormationVolumeFactor = 1.5;
            //    pvt.TotalCompressibility     = 0.00002;

            //    MultiPorosityData<double> mpd = new MultiPorosityData<double>();
            //    mpd.ProductionData            = productionData;
            //    mpd.ReservoirProperties       = reservoir;
            //    mpd.WellProperties            = well;
            //    mpd.FractureProperties        = fracture;
            //    mpd.NaturalFractureProperties = natural_fracture;
            //    mpd.Pvt                       = pvt;

            //    View<double, Cuda> actualQt = new View<double, Cuda>("actualQt", HunterDailyData.actualAvgDailyBoe.Length);

            //    View<double, Cuda> actualTime = new View<double, Cuda>("actualTime", HunterDailyData.actualAvgDailyBoe.Length);

            //    View<double, Cuda> weights = new View<double, Cuda>("weights", HunterDailyData.actualAvgDailyBoe.Length);

            //    for(ulong i0 = 0; i0 < actualQt.Extent(0); ++i0)
            //    {
            //        actualQt[i0]   = HunterDailyData.actualAvgDailyBoe[i0]; //productionData[i0].Qt;
            //        actualTime[i0] = HunterDailyData.actualMonthlyTime[i0]; //timeData[i0];

            //        //if(i0 < 8 || i0 >= 16 && i0 <= 17 || i0 > 66 && i0 < 90 || i0 > 125)
            //        //{
            //        //    weights[i0] = 0.0001;
            //        //}
            //        //else if(i0 >= 119 && i0 <= 125)
            //        //{
            //        //    weights[i0] = 1.2;
            //        //}
            //        //else
            //        //{
            //        weights[i0] = 1.0;
            //        //}
            //    }

            //    TriplePorosityModel<double, Cuda> tpm = new TriplePorosityModel<double, Cuda>(mpd);

            //    View<double, Cuda> args = new View<double, Cuda>("args", 7);

            //    //km: 0.0018895176183702685
            //    //kF: 83.69857529340902
            //    //kf: 4.762316834147342
            //    //ye: 162.8781340630174
            //    //LF: 148.64640077973382
            //    //Lf: 55.506016771447456
            //    //sk: 0.0

            //    //args[0] = 0.0018895176183702685;
            //    //args[1] = 83.69857529340902;
            //    //args[2] = 4.762316834147342;
            //    //args[3] = 162.8781340630174;
            //    //args[4] = 148.64640077973382;
            //    //args[5] = 55.506016771447456;
            //    //args[6] = 0.0;

            //    for(ulong i = 0; i < args.Size(); ++i)
            //    {
            //        args[i] = values[i];
            //    }

            //    View<double, Cuda> predictedTimeData = new View<double, Cuda>("predictedTimeData", predictTimeData.Length);

            //    for(int i0 = 0; i0 < predictTimeData.Length; ++i0)
            //    {
            //        predictedTimeData[i0] = predictTimeData[i0];
            //    }

            //    View<double, Cuda> predictedQt = tpm.Calculate(predictedTimeData, args);

            //    predictQt = new double[predictedQt.Size()];

            //    for(ulong i0 = 0; i0 < predictedQt.Size(); ++i0)
            //    {
            //        predictQt[i0] = predictedQt[i0];
            //    }
            //}

            //ProductionChartCollection predict_results = new ProductionChartCollection(HunterDailyData.actualMonthlyBoe.Length + predictTimeData.Length);

            //double t;

            //for(int i0 = 0; i0 < predictTimeData.Length; ++i0)
            //{
            //    t = predictTimeData[i0];
            //    predict_results.Add("Predicted", t, predictQt[i0]);
            //}

            ////predict_results.ToMonthlyProduction(new System.DateTime(2012, 10, 15), 15);

            //for(int i0 = 0; i0 < HunterDailyData.actualMonthlyBoe.Length; ++i0)
            //{
            //    t = HunterDailyData.actualMonthlyTime[i0];
            //    predict_results.Add("Actual", t, HunterDailyData.actualMonthlyBoe[i0]);
            //}

            //predict_results.BuildCumulativeProduction();

            //for(int i0 = 0; i0 < predict_results["Predicted"].Count; ++i0)
            //{
            //    Console.WriteLine($"{predict_results["Predicted"][i0].Day} {predict_results["Predicted"][i0].Rate}");
            //}
        }
    }
}
