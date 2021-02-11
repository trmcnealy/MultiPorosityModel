using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Management;
using System.Numerics;

using Kokkos;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MultiPorosity.Models;
using MultiPorosity.Services;

namespace MultiPorosityModel.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private static readonly double[] timeData =
        {
            1.00, 2.00, 3.00, 4.00, 5.00, 6.00, 7.00, 8.00, 9.00, 10.00, 11.00, 12.00, 13.00, 14.00, 15.00, 16.00, 17.00, 18.00, 19.00, 20.00, 21.00, 22.00, 23.00, 24.00, 25.00, 26.00,
            27.00, 28.00, 29.00, 30.00, 31.00, 32.00, 33.00, 34.00, 35.00, 36.00, 37.00, 38.00, 39.00, 40.00, 41.00, 42.00, 43.00, 44.00, 45.00, 46.00, 47.00, 48.00, 49.00, 50.00, 51.00,
            52.00, 53.00, 54.00, 55.00, 56.00, 57.00, 58.00, 59.00, 60.00, 61.00, 62.00, 63.00, 64.00, 65.00, 66.00, 67.00, 68.00, 69.00, 70.00, 71.00, 72.00, 73.00, 74.00, 75.00, 76.00,
            77.00, 78.00, 79.00, 80.00, 81.00, 82.00, 83.00, 84.00, 85.00, 86.00, 87.00, 88.00, 89.00, 90.00, 91.00, 92.00, 93.00, 94.00, 95.00, 96.00, 97.00, 98.00, 99.00, 100.00, 101.00,
            102.00, 103.00, 104.00, 105.00, 106.00, 107.00, 108.00, 109.00, 110.00, 111.00, 112.00, 113.00, 114.00, 115.00, 116.00, 117.00, 118.00, 119.00, 120.00, 121.00, 122.00, 123.00,
            124.00, 125.00, 126.00, 127.00, 128.00, 129.00, 130.00, 131.00
        };

        private static readonly double[] qoData =
        {
            215.00, 626.00, 650.00, 686.90, 610.70, 578.87, 603.25, 801.39, 940.65, 764.58, 705.05, 690.91, 691.24, 719.37, 771.83, 781.97, 698.35, 701.45, 679.09, 690.09, 650.20, 607.93,
            622.11, 602.76, 560.85, 571.40, 588.63, 559.52, 565.43, 528.30, 517.26, 516.94, 517.19, 516.56, 513.51, 501.84, 486.37, 468.29, 475.70, 449.74, 456.89, 466.28, 451.18, 432.44,
            439.75, 356.65, 529.26, 401.97, 390.66, 467.53, 437.38, 433.74, 372.42, 390.37, 403.21, 390.26, 363.43, 406.69, 367.72, 441.42, 410.57, 377.83, 371.07, 331.53, 401.77, 343.77,
            353.99, 397.16, 338.24, 318.80, 303.40, 323.43, 293.02, 303.23, 294.21, 293.01, 298.53, 304.64, 294.30, 294.66, 290.98, 288.29, 290.95, 288.24, 502.21, 408.63, 390.16, 374.39,
            347.30, 338.92, 313.71, 316.73, 307.36, 305.74, 292.65, 293.01, 297.31, 291.52, 282.22, 273.84, 295.63, 271.26, 278.18, 277.49, 266.27, 273.97, 265.47, 267.83, 268.14, 255.22,
            259.11, 261.13, 260.31, 254.44, 253.05, 254.78, 247.38, 253.74, 246.34, 240.89, 247.23, 244.87, 242.36, 242.69, 240.33, 269.18, 261.42, 252.41, 264.48, 260.17, 257.04
        };

        private static readonly double[] qwData =
        {
            0.00, 0.00, 621.00, 639.00, 640.00, 631.00, 671.00, 822.00, 960.00, 900.00, 918.00, 710.00, 765.00, 775.00, 728.00, 630.00, 770.00, 774.00, 660.00, 608.00, 548.00, 542.00,
            530.00, 496.00, 521.00, 453.00, 438.00, 497.00, 432.00, 416.00, 387.00, 362.00, 348.00, 388.00, 326.00, 403.00, 360.00, 380.00, 344.00, 305.00, 361.00, 353.00, 294.00, 303.00,
            260.00, 242.00, 233.00, 213.00, 245.00, 226.00, 235.00, 258.00, 263.00, 206.00, 183.00, 204.00, 230.00, 193.00, 257.00, 231.00, 209.00, 200.00, 231.00, 246.00, 193.00, 145.00,
            192.00, 232.00, 187.00, 196.00, 161.00, 152.00, 135.00, 118.00, 106.00, 119.00, 129.00, 111.00, 108.00, 126.00, 119.00, 119.00, 111.00, 295.00, 110.00, 153.00, 169.00, 148.00,
            185.00, 145.00, 141.00, 145.00, 139.00, 151.00, 132.00, 150.00, 132.00, 123.00, 137.00, 139.00, 142.00, 133.00, 142.00, 130.00, 125.00, 130.00, 118.00, 119.00, 121.00, 117.00,
            120.00, 118.00, 143.00, 115.00, 124.00, 141.00, 122.00, 118.00, 108.00, 114.00, 110.00, 113.00, 111.00, 109.00, 119.00, 127.00, 126.00, 120.00, 145.00, 160.00, 125.00
        };

        private static readonly double[] qgData =
        {
            380.00, 380.00, 380.00, 380.00, 380.00, 380.00, 380.00, 533.00, 551.00, 537.00, 528.00, 520.00, 507.00, 503.00, 514.00, 507.00, 491.00, 311.00, 388.00, 380.00, 373.00, 430.00,
            424.00, 424.00, 409.00, 398.00, 390.00, 392.00, 389.00, 331.00, 351.00, 350.00, 351.00, 348.00, 353.00, 339.00, 337.00, 323.00, 322.00, 308.00, 325.00, 299.00, 302.00, 304.00,
            308.00, 281.00, 301.00, 290.00, 288.00, 291.00, 293.00, 299.00, 277.00, 227.00, 252.00, 269.00, 270.00, 265.00, 274.00, 289.00, 277.00, 219.00, 269.00, 250.00, 268.00, 253.00,
            248.00, 258.00, 238.00, 221.00, 219.00, 194.00, 200.00, 197.00, 93.00, 183.00, 175.00, 184.00, 181.00, 183.00, 180.00, 179.00, 177.00, 220.00, 271.00, 266.00, 254.00, 234.00,
            228.00, 218.00, 204.00, 205.00, 202.00, 200.00, 199.00, 197.00, 189.00, 188.00, 185.00, 182.00, 182.00, 180.00, 179.00, 169.00, 164.00, 176.00, 174.00, 172.00, 166.00, 164.00,
            163.00, 163.00, 161.00, 158.00, 157.00, 157.00, 153.00, 153.00, 151.00, 149.00, 148.00, 145.00, 144.00, 143.00, 143.00, 158.00, 161.00, 155.00, 166.00, 164.00, 157.00
        };

        private readonly Random _rand = new Random();

        [TestMethod]
        public void TestMethod1()
        {
            ParallelProcessor.Initialize(4);

            {
                double length          = _rand.NextDouble();
                double width           = _rand.NextDouble();
                double thickness       = _rand.NextDouble();
                double porosity        = _rand.NextDouble();
                double permeability    = _rand.NextDouble();
                double temperature     = _rand.NextDouble();
                double initialPressure = _rand.NextDouble();

                ReservoirProperties<double> reservoirProperties = new ReservoirProperties<double>();

                reservoirProperties.Length          = length;
                reservoirProperties.Width           = width;
                reservoirProperties.Thickness       = thickness;
                reservoirProperties.Porosity        = porosity;
                reservoirProperties.Permeability    = permeability;
                reservoirProperties.BottomholeTemperature     = temperature;
                reservoirProperties.InitialPressure = initialPressure;

                Assert.AreEqual(reservoirProperties.Length,
                                length);

                Assert.AreEqual(reservoirProperties.Width,
                                width);

                Assert.AreEqual(reservoirProperties.Thickness,
                                thickness);

                Assert.AreEqual(reservoirProperties.Porosity,
                                porosity);

                Assert.AreEqual(reservoirProperties.Permeability,
                                permeability);

                Assert.AreEqual(reservoirProperties.BottomholeTemperature,
                                temperature);

                Assert.AreEqual(reservoirProperties.InitialPressure,
                                initialPressure);
            }

            {
                FractureProperties<double> fractureProperties = new FractureProperties<double>
                {
                    Skin         = 1.0,
                    Count        = 2,
                    Width        = 4.0,
                    Height       = 5.0,
                    HalfLength   = 6.0,
                    Porosity     = 7.0,
                    Permeability = 8.0
                };

                Assert.AreEqual(fractureProperties.Skin,
                                1.0);

                Assert.AreEqual(fractureProperties.Count,
                                2);

                Assert.AreEqual(fractureProperties.Width,
                                4.0);

                Assert.AreEqual(fractureProperties.Height,
                                5.0);

                Assert.AreEqual(fractureProperties.HalfLength,
                                6.0);

                Assert.AreEqual(fractureProperties.Porosity,
                                7.0);

                Assert.AreEqual(fractureProperties.Permeability,
                                8.0);
            }

            {
                FractureProperties<float> fractureProperties = new FractureProperties<float>
                {
                    Skin         = 1.0f,
                    Count        = 2,
                    Width        = 4.0f,
                    Height       = 5.0f,
                    HalfLength   = 6.0f,
                    Porosity     = 7.0f,
                    Permeability = 8.0f
                };

                Assert.AreEqual(fractureProperties.Skin,
                                1.0f);

                Assert.AreEqual(fractureProperties.Count,
                                2);

                Assert.AreEqual(fractureProperties.Width,
                                4.0f);

                Assert.AreEqual(fractureProperties.Height,
                                5.0f);

                Assert.AreEqual(fractureProperties.HalfLength,
                                6.0f);

                Assert.AreEqual(fractureProperties.Porosity,
                                7.0f);

                Assert.AreEqual(fractureProperties.Permeability,
                                8.0f);
            }

            {
                NaturalFractureProperties<double> naturalFractureProperties = new NaturalFractureProperties<double>
                {
                    Count        = 2,
                    Width        = 4.0,
                    Porosity     = 7.0,
                    Permeability = 8.0
                };

                Assert.AreEqual(naturalFractureProperties.Count,
                                2);

                Assert.AreEqual(naturalFractureProperties.Width,
                                4.0);

                Assert.AreEqual(naturalFractureProperties.Porosity,
                                7.0);

                Assert.AreEqual(naturalFractureProperties.Permeability,
                                8.0);
            }

            {
                ProductionDataRecord<double> record = new ProductionDataRecord<double>
                {
                    Time  = 1.0,
                    Qo    = 2.0,
                    Qw    = 4.0,
                    Qg    = 5.0,
                    QgBoe = 6.0,
                    Qt    = 7.0
                };

                Assert.AreEqual(record.Time,
                                1.0);

                Assert.AreEqual(record.Qo,
                                2.0);

                Assert.AreEqual(record.Qw,
                                4.0);

                Assert.AreEqual(record.Qg,
                                5.0);

                Assert.AreEqual(record.QgBoe,
                                6.0);

                Assert.AreEqual(record.Qt,
                                7.0);
            }

            {
                ProductionData<double> productionData = new ProductionData<double>(10);

                Assert.AreEqual(productionData.Count,
                                10);

                ProductionDataRecord<double> record = productionData[0];

                record.Time  = 1.0;
                record.Qo    = 2.0;
                record.Qw    = 4.0;
                record.Qg    = 5.0;
                record.QgBoe = 6.0;
                record.Qt    = 7.0;

                Assert.AreEqual(record.Time,
                                1.0);

                Assert.AreEqual(record.Qo,
                                2.0);

                Assert.AreEqual(record.Qw,
                                4.0);

                Assert.AreEqual(record.Qg,
                                5.0);

                Assert.AreEqual(record.QgBoe,
                                6.0);

                Assert.AreEqual(record.Qt,
                                7.0);
            }

            {
                Pvt<double> pvt = new Pvt<double>
                {
                    OilViscosity             = 1.0,
                    OilFormationVolumeFactor = 2.0
                };

                Assert.AreEqual(pvt.OilViscosity,
                                1.0);

                Assert.AreEqual(pvt.OilFormationVolumeFactor,
                                2.0);
            }

            {
                WellProperties<double> well = new WellProperties<double>
                {
                    API                = 1ul,
                    LateralLength      = 2.0,
                    BottomholePressure = 4.0
                };

                Assert.IsTrue(well.API == 1ul);

                Assert.AreEqual(well.LateralLength,
                                2.0);

                Assert.AreEqual(well.BottomholePressure,
                                4.0);
            }

            {
                ProductionData<double>       productionData = new ProductionData<double>(1);
                ProductionDataRecord<double> record         = productionData[0];
                record.Time  = 1.0;
                record.Qo    = 2.0;
                record.Qw    = 4.0;
                record.Qg    = 5.0;
                record.QgBoe = 6.0;
                record.Qt    = 7.0;

                FractureProperties<double> fractureProperties = new FractureProperties<double>
                {
                    Skin         = 1.0,
                    Count        = 2,
                    Width        = 4.0,
                    Height       = 5.0,
                    HalfLength   = 6.0,
                    Porosity     = 7.0,
                    Permeability = 8.0
                };

                NaturalFractureProperties<double> naturalFractureProperties = new NaturalFractureProperties<double>
                {
                    Count        = 2,
                    Width        = 4.0,
                    Porosity     = 7.0,
                    Permeability = 8.0
                };

                Pvt<double> pvt = new Pvt<double>
                {
                    OilViscosity             = 1.0,
                    OilFormationVolumeFactor = 2.0
                };

                ReservoirProperties<double> reservoir = new ReservoirProperties<double>
                {
                    Length          = 1.0,
                    Width           = 2.0,
                    Thickness       = 4.0,
                    Porosity        = 5.0,
                    Permeability    = 6.0,
                    BottomholeTemperature     = 7.0,
                    InitialPressure = 8.0
                };

                WellProperties<double> well = new WellProperties<double>
                {
                    API                = 1,
                    LateralLength      = 2.0,
                    BottomholePressure = 4.0
                };
                
                RelativePermeabilities<double> relativePermeabilities = new RelativePermeabilities<double>
                {
                    MatrixOil            = 1,
                    MatrixWater          = 2.0,
                    MatrixGas            = 4.0,
                    FractureOil          = 1,
                    FractureWater        = 2.0,
                    FractureGas          = 4.0,
                    NaturalFractureOil   = 1,
                    NaturalFractureWater = 2.0,
                    NaturalFractureGas   = 4.0
                };

                MultiPorosityData<double> mpd = new MultiPorosityData<double>(reservoir,
                                                                              well,
                                                                              fractureProperties,
                                                                              naturalFractureProperties,
                                                                              pvt,
                                                                              relativePermeabilities);

                Assert.AreEqual(mpd.WellProperties.API,
                                1ul);
            }

            ParallelProcessor.Shutdown();
        }

        [TestMethod]
        public void TestMethod2()
        {
            ParallelProcessor.Initialize(4);

            ProductionData<double> productionData = new ProductionData<double>(131);

            for(int i = 0; i < 131; ++i)
            {
                productionData[i].Time  = timeData[i];
                productionData[i].Qo    = qoData[i];
                productionData[i].Qw    = qwData[i];
                productionData[i].Qg    = qgData[i];
                productionData[i].QgBoe = qgData[i] / 5.8;
                productionData[i].Qt    = productionData[i].Qo + productionData[i].Qw + productionData[i].QgBoe;
            }

            ReservoirProperties<double> reservoir = new ReservoirProperties<double>();
            reservoir.Length = 6500.0;
            reservoir.Width  = 348.0;
            // reservoir.Area                        = (reservoir.Length * reservoir.Width) / 43560;
            reservoir.Thickness       = 125.0;
            reservoir.Porosity        = 0.06;
            reservoir.Permeability    = 0.002;
            reservoir.BottomholeTemperature     = 275.0;
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

            RelativePermeabilities<double> relativePermeabilities = new RelativePermeabilities<double>();

            relativePermeabilities.MatrixOil            = 1;
            relativePermeabilities.MatrixWater          = 2.0;
            relativePermeabilities.MatrixGas            = 4.0;
            relativePermeabilities.FractureOil          = 1;
            relativePermeabilities.FractureWater        = 2.0;
            relativePermeabilities.FractureGas          = 4.0;
            relativePermeabilities.NaturalFractureOil   = 1;
            relativePermeabilities.NaturalFractureWater = 2.0;
            relativePermeabilities.NaturalFractureGas   = 4.0;
            

            MultiPorosityData<double> mpd = new MultiPorosityData<double>();
            mpd.ReservoirProperties       = reservoir;
            mpd.WellProperties            = well;
            mpd.FractureProperties        = fracture;
            mpd.NaturalFractureProperties = natural_fracture;
            mpd.Pvt                       = pvt;
            mpd.RelativePermeability    = relativePermeabilities;

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
            arg_limits[0] = new BoundConstraints<double>(0.0001,
                                                         0.01);

            /*kF*/
            arg_limits[1] = new BoundConstraints<double>(10.0,
                                                         1000.0);

            /*kf*/
            arg_limits[2] = new BoundConstraints<double>(0.01,
                                                         10.0);

            /*ye*/
            arg_limits[3] = new BoundConstraints<double>(100.0,
                                                         1000.0);

            /*LF*/
            arg_limits[4] = new BoundConstraints<double>(50.0,
                                                         250.0);

            /*Lf*/
            arg_limits[5] = new BoundConstraints<double>(10.0,
                                                         150.0);

            /*sk*/
            arg_limits[6] = new BoundConstraints<double>(0.0,
                                                         0.0);

            ///*km*/ arg_limits_mirror(0) = System.ValueLimits<double>(0.001, 0.002);
            ///*kF*/ arg_limits_mirror(1) = System.ValueLimits<double>(100.0, 200.0);
            ///*kf*/ arg_limits_mirror(2) = System.ValueLimits<double>(0.001, 10.0);
            ///*ye*/ arg_limits_mirror(3) = System.ValueLimits<double>(100.0, 500.0);
            ///*LF*/ arg_limits_mirror(4) = System.ValueLimits<double>(50.0, 150.0);
            ///*Lf*/ arg_limits_mirror(5) = System.ValueLimits<double>(10.0, 100.0);
            ///*sk*/ arg_limits_mirror(6) = System.ValueLimits<double>(-2.0, 2.0);

            // Kokkos.deep_copy(arg_limits, arg_limits_mirror);

            View<double, Cuda> actual_data = new View<double, Cuda>("actual_data",
                                                                    131);

            View<double, Cuda> actual_time = new View<double, Cuda>("actual_time",
                                                                    131);

            View<double, Cuda> weights = new View<double, Cuda>("weights",
                                                                131);

            for(ulong i0 = 0; i0 < actual_data.Extent(0); ++i0)
            {
                actual_data[i0] = productionData[i0].Qt;
                actual_time[i0] = timeData[i0];

                if(i0 < 8 || i0 >= 16 && i0 <= 17 || i0 > 66 && i0 < 90 || i0 > 124)
                {
                    weights[i0] = 0.0001;
                }
                else
                {
                    weights[i0] = 1.0;
                }
            }

            TriplePorosityModel<double, Cuda> tpm = new TriplePorosityModel<double, Cuda>(mpd);

            ParticleSwarmOptimizationOptions options = new ParticleSwarmOptimizationOptions();

            //Vector<double> best_args = pso.Execute(arg_limits, options);
            MultiPorosityResult<double, Cuda> best_args = tpm.HistoryMatch(options,
                                                                           actual_data,
                                                                           actual_time,
                                                                           weights,
                                                                           arg_limits);

            Console.WriteLine("best_args");

            for(ulong i = 0; i < best_args.Args.Extent(0); ++i)
            {
                Console.WriteLine(best_args.Args[i]);
            }

            Console.WriteLine();

            View<double, Cuda> timeView = new View<double, Cuda>("time",
                                                                 4);

            for(ulong i0 = 0; i0 < timeView.Extent(0); ++i0)
            {
                timeView[i0] = 15 + 30 * i0;
            }

            View<double, Cuda> argsView = new View<double, Cuda>("args",
                                                                 7);

            argsView[0] = 0.01;
            argsView[1] = 1000.00;
            argsView[2] = 52.16;
            argsView[3] = 176.68;
            argsView[4] = 242.76;
            argsView[5] = 92.80;
            argsView[6] = 0.00;

            View<double, Cuda> simulated_data = tpm.Calculate(actual_time,
                                                              argsView);

            for(ulong i0 = 0; i0 < simulated_data.Extent(0); ++i0)
            {
                Console.WriteLine(simulated_data[i0]);
            }

            ParallelProcessor.Shutdown();
        }
    }
}