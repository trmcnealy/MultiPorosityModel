using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

using Engineering.DataSource.OilGas;
using Engineering.DataSource.Tools;

using MultiPorosity.Models;

namespace MultiPorosity.Services
{
    public static class ProductionService
    {
        public static List<ProductionRecord> GenerateDailyAverages(MonthlyProductionUnit mpu)
        {
            List<ProductionRecord> dailyAverages = new(mpu.MonthlyProductionRecords.Count);

            ProductionRecord first = mpu.MonthlyProductionRecords.First();
            ProductionRecord last  = mpu.MonthlyProductionRecords.Last();

            int total_days = (int)mpu.Points.Last().Days;
            
            bool ConstantArgFunctor(int equation_index,
                                    int arg_index)
            {
                if (arg_index == 1)
                {
                    return true;
                }

                //if(equation_index == 0)
                //{
                //    return false;
                //}

                //if(arg_index == 0)
                //{
                //    return true;
                //}

                return false;
                //if(index % 2 == 0)
                //{
                //    return false;
                //}
                //return true;
            }

            (List<double> errors, double[][] points) gas_results    = NewtonRaphson.Solve(mpu.Units.Count, mpu.GetGasFunctorArgs,   mpu.GasFunctor,   ConstantArgFunctor);
            (List<double> errors, double[][] points) oil_results    = NewtonRaphson.Solve(mpu.Units.Count, mpu.GetOilFunctorArgs,   mpu.OilFunctor,   ConstantArgFunctor);
            (List<double> errors, double[][] points) waters_results = NewtonRaphson.Solve(mpu.Units.Count, mpu.GetWaterFunctorArgs, mpu.WaterFunctor, ConstantArgFunctor);

            for(int i = 0; i < mpu.Units.Count; ++i)
            {
                mpu.Units[i].Start.GasVolume = gas_results.points[i][0];
                mpu.Units[i].Middle.GasVolume = gas_results.points[i][1];
                mpu.Units[i].End.GasVolume = gas_results.points[i][2];

                mpu.Units[i].Start.OilVolume  = oil_results.points[i][0];
                mpu.Units[i].Middle.OilVolume = oil_results.points[i][1];
                mpu.Units[i].End.OilVolume    = oil_results.points[i][2];

                mpu.Units[i].Start.WaterVolume  = waters_results.points[i][0];
                mpu.Units[i].Middle.WaterVolume = waters_results.points[i][1];
                mpu.Units[i].End.WaterVolume    = waters_results.points[i][2];
            }

            int day;

            for(int i = 0; i < total_days; ++i)
            {
                day = i + 1;
                dailyAverages.Add(new(i, first.Date.AddDays(i), day, mpu.GenerateDailyGas(day), mpu.GenerateDailyOil(day), mpu.GenerateDailyWater(day)));
            }

            return dailyAverages;
        }

        //Start.GasVolume   = FindStartY(MonthlyProductionRecord.Gas,   Start.Days, (Middle.Days, Middle.GasVolume),   (End.Days, End.GasVolume));
        //Start.OilVolume   = FindStartY(MonthlyProductionRecord.Oil,   Start.Days, (Middle.Days, Middle.OilVolume),   (End.Days, End.OilVolume));
        //Start.WaterVolume = FindStartY(MonthlyProductionRecord.Water, Start.Days, (Middle.Days, Middle.WaterVolume), (End.Days, End.WaterVolume));

        //End.GasVolume   = FindStartY(MonthlyProductionRecord.Gas,   End.Days, (Middle.Days, Middle.GasVolume),   (Start.Days, Start.GasVolume));
        //End.OilVolume   = FindStartY(MonthlyProductionRecord.Oil,   End.Days, (Middle.Days, Middle.OilVolume),   (Start.Days, Start.OilVolume));
        //End.WaterVolume = FindStartY(MonthlyProductionRecord.Water, End.Days, (Middle.Days, Middle.WaterVolume), (Start.Days, Start.WaterVolume));

        public static List<ProductionRecord> ConvertMonthlyToDaily(List<ProductionRecord> monthlyProductionRecords)
        {
            List<ProductionRecord> monthlyProduction = monthlyProductionRecords.OrderBy(o => o.Date).ToList();

            MonthlyProductionUnit mpu = new(monthlyProduction);

            List<ProductionRecord> dailyProduction = GenerateDailyAverages(mpu);

            return dailyProduction;

            //List<MonthlyProductionRecord> monthlyRecords = new(monthlyProduction.Count);

            //DateTime startDate = new(monthlyProduction[0].Date.Year, monthlyProduction[0].Date.Month, 1);

            //ProductionRecord production = monthlyProduction[0];

            //MonthlyProductionRecord previousrecord;
            //MonthlyProductionRecord record = new (startDate, production.Date.Year, production.Date.Month, production.Gas, production.Oil, production.Water);

            //monthlyRecords.Add(record);

            //for(int i = 1; i < monthlyProduction.Count; ++i)
            //{
            //    production = monthlyProduction[i];

            //    previousrecord = monthlyRecords.Last();

            //    record = new MonthlyProductionRecord(startDate, production.Date.Year, production.Date.Month, production.Gas, production.Oil, production.Water)
            //    {
            //        PreviousMonth = monthlyRecords.Last()
            //    };

            //    previousrecord.NextMonth = record;

            //    monthlyRecords.Add(record);
            //}

            //double error = 0.0;

            //for(int i = 0; i < 10; ++i)
            //{
            //    foreach(var monthlyRecord in monthlyRecords)
            //    {
            //        error += monthlyRecord.Update();
            //    }

            //    Debug.WriteLine(error);

            //    error = 0.0;
            //}

            //List<ProductionRecord> dailyProduction = new(monthlyRecords.Count * 31);

            //foreach(var monthlyRecord in monthlyRecords)
            //{
            //    dailyProduction.AddRange(monthlyRecord.DailyProductionRecords);
            //}

            //return dailyProduction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static List<CumulativeProductionRecord> CumulativeProduction(List<ProductionRecord> dailyProductionRecords)
        {
            List<CumulativeProductionRecord> output = new(dailyProductionRecords.Count)
            {
                new CumulativeProductionRecord(dailyProductionRecords[0].Index,
                                               dailyProductionRecords[0].Date,
                                               dailyProductionRecords[0].Days,
                                               dailyProductionRecords[0].Gas,
                                               dailyProductionRecords[0].Oil,
                                               dailyProductionRecords[0].Water)
            };

            CumulativeProductionRecord previousRecord;

            for(int i = 1; i < dailyProductionRecords.Count; ++i)
            {
                previousRecord = output[^1];

                output.Add(new CumulativeProductionRecord(dailyProductionRecords[i].Index,
                                                          dailyProductionRecords[i].Date,
                                                          dailyProductionRecords[i].Days,
                                                          dailyProductionRecords[i].Gas   + previousRecord.Gas,
                                                          dailyProductionRecords[i].Oil   + previousRecord.Oil,
                                                          dailyProductionRecords[i].Water + previousRecord.Water));
            }

            return output;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static List<CumulativeMultiPorosityModelProduction> CumulativeProduction(List<MultiPorosityModelProduction> dailyProductionRecords)
        {
            List<CumulativeMultiPorosityModelProduction> output = new(dailyProductionRecords.Count)
            {
                new CumulativeMultiPorosityModelProduction(dailyProductionRecords[0].Days, dailyProductionRecords[0].Gas, dailyProductionRecords[0].Oil, dailyProductionRecords[0].Water)
            };

            CumulativeMultiPorosityModelProduction previousRecord;

            for(int i = 1; i < dailyProductionRecords.Count; ++i)
            {
                previousRecord = output[^1];

                output.Add(new CumulativeMultiPorosityModelProduction(dailyProductionRecords[i].Days,
                                                                      dailyProductionRecords[i].Gas   + previousRecord.Gas,
                                                                      dailyProductionRecords[i].Oil   + previousRecord.Oil,
                                                                      dailyProductionRecords[i].Water + previousRecord.Water));
            }

            return output;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static List<CumulativeProductionRecord> CumulativeProductionForAverageDaily(List<ProductionRecord> dailyProductionRecords)
        {
            int daysInMonth = dailyProductionRecords[0].Date.DaysInMonth();

            List<CumulativeProductionRecord> output = new(dailyProductionRecords.Count)
            {
                new CumulativeProductionRecord(dailyProductionRecords[0].Index,
                                               dailyProductionRecords[0].Date,
                                               dailyProductionRecords[0].Days,
                                               dailyProductionRecords[0].Gas   * daysInMonth,
                                               dailyProductionRecords[0].Oil   * daysInMonth,
                                               dailyProductionRecords[0].Water * daysInMonth)
            };

            CumulativeProductionRecord previousRecord;

            for(int i = 1; i < dailyProductionRecords.Count; ++i)
            {
                previousRecord = output[^1];
                daysInMonth    = dailyProductionRecords[i].Date.DaysInMonth();

                output.Add(new CumulativeProductionRecord(dailyProductionRecords[i].Index,
                                                          dailyProductionRecords[i].Date,
                                                          dailyProductionRecords[i].Days,
                                                          dailyProductionRecords[i].Gas   * daysInMonth + previousRecord.Gas,
                                                          dailyProductionRecords[i].Oil   * daysInMonth + previousRecord.Oil,
                                                          dailyProductionRecords[i].Water * daysInMonth + previousRecord.Water));
            }

            return output;
        }
    }
}

//private static List<ProductionRecord> convertMonthlyToDaily(List<ProductionRecord> monthlyProduction)
//{
//    monthlyProduction = monthlyProduction.OrderBy(o => o.Date).ToList();

//    List<ProductionRecord> dailyProduction = new List<ProductionRecord>(monthlyProduction.Count * 30);

//    ProductionRecord production1;
//    ProductionRecord production2;

//    DateTime startDate = new DateTime(monthlyProduction[0].Date.Year, monthlyProduction[0].Date.Month, 1);

//    DateTime midMonth1;
//    DateTime midMonth2;

//    int deltaDays1;
//    int deltaDays2;

//    int daysInMonth1;
//    int daysInMonth2;

//    LinearEquation GasEquation;
//    LinearEquation OilEquation;
//    LinearEquation WaterEquation;

//    double dailyGas;
//    double dailyOil;
//    double dailyWater;

//    int index = 0;

//    for(int i = 1; i < monthlyProduction.Count; ++i)
//    {
//        production1 = monthlyProduction[i - 1];
//        production2 = monthlyProduction[i];

//        midMonth1 = new DateTime(production1.Date.Year, production1.Date.Month, 15);
//        midMonth2 = new DateTime(production2.Date.Year, production2.Date.Month, 15);

//        daysInMonth1 = midMonth1.DaysInMonth();
//        daysInMonth2 = midMonth2.DaysInMonth();

//        deltaDays1 = (midMonth1 - startDate).Days;
//        deltaDays2 = (midMonth2 - startDate).Days;

//        GasEquation = new LinearEquation(production2.Gas   / daysInMonth2, deltaDays2, production1.Gas / daysInMonth1, deltaDays1);
//        OilEquation = new LinearEquation(production2.Oil / daysInMonth2, deltaDays2, production1.Oil / daysInMonth1, deltaDays1);
//        WaterEquation = new LinearEquation(production2.Water / daysInMonth2, deltaDays2, production1.Water / daysInMonth1, deltaDays1);

//        if(i == 1)
//        {
//            for(int day = 0; day < deltaDays1; ++day)
//            {
//                dailyGas   = GasEquation.y(day);
//                dailyOil   = OilEquation.y(day);
//                dailyWater = WaterEquation.y(day);

//                dailyProduction.Add(new ProductionRecord(index++, new DateTime(startDate.Year, startDate.Month, startDate.Day).AddDays(day), day, dailyGas, dailyOil, dailyWater));
//            }

//            for(int day = deltaDays1; day < deltaDays2; ++day)
//            {
//                dailyGas   = GasEquation.y(day);
//                dailyOil   = OilEquation.y(day);
//                dailyWater = WaterEquation.y(day);

//                dailyProduction.Add(new ProductionRecord(index++, new DateTime(startDate.Year, startDate.Month, startDate.Day).AddDays(day), day, dailyGas, dailyOil, dailyWater));
//            }
//        }
//        else if(i == monthlyProduction.Count - 1)
//        {
//            for(int day = deltaDays1; day < deltaDays2; ++day)
//            {
//                dailyGas   = GasEquation.y(day);
//                dailyOil   = OilEquation.y(day);
//                dailyWater = WaterEquation.y(day);

//                dailyProduction.Add(new ProductionRecord(index++, new DateTime(startDate.Year, startDate.Month, startDate.Day).AddDays(day), day, dailyGas, dailyOil, dailyWater));
//            }

//            DateTime lastMonth = new DateTime(production2.Date.Year, production2.Date.Month, daysInMonth2);

//            for(int day = deltaDays2; day < deltaDays2 + (lastMonth - midMonth2).Days; ++day)
//            {
//                dailyGas   = GasEquation.y(day);
//                dailyOil   = OilEquation.y(day);
//                dailyWater = WaterEquation.y(day);

//                dailyProduction.Add(new ProductionRecord(index++, new DateTime(startDate.Year, startDate.Month, startDate.Day).AddDays(day), day, dailyGas, dailyOil, dailyWater));
//            }
//        }
//        else
//        {
//            for(int day = deltaDays1; day < deltaDays2; ++day)
//            {
//                dailyGas   = GasEquation.y(day);
//                dailyOil   = OilEquation.y(day);
//                dailyWater = WaterEquation.y(day);

//                dailyProduction.Add(new ProductionRecord(index++, new DateTime(startDate.Year, startDate.Month, startDate.Day).AddDays(day), day, dailyGas, dailyOil, dailyWater));
//            }
//        }
//    }

//    return dailyProduction;
//}

//[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
//public static float[] DailyCumulativeSum(float[] rate)
//{
//    float[] output = new float[rate.Length];

//    output[0] = rate[0];

//    for(int i = 1; i < rate.Length; ++i)
//    {
//        output[i] = output[i - 1] + rate[i];
//    }

//    return output;
//}

//[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
//private static double _scan(double lhs,
//                            double rhs)
//{
//    double result = lhs + rhs;

//    return result;
//}

//[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
//public static double[] DailyCumulativeSum(double[] rate,
//                                          int      startIndex = 0,
//                                          bool     parallel   = false)
//{
//    double[] output = new double[rate.Length];

//    if(parallel)
//    {
//        output = System.Threading.Algorithms.ParallelAlgorithms.Scan(rate.Skip(startIndex), _scan);
//    }
//    else
//    {
//        output[startIndex] = rate[startIndex];

//        for(int i = startIndex + 1; i < rate.Length; ++i)
//        {
//            output[i] = output[i - 1] + rate[i];
//        }
//    }

//    return output;
//}

//[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
//public static float[] CumulativeAvgSum(float[] rate,
//                                       float[] days)
//{
//    float[] output = new float[rate.Length];

//    output[0] = rate[0] * days[0];

//    for(int i = 1; i < rate.Length; ++i)
//    {
//        output[i] = output[i - 1] + rate[i] * days[i];
//    }

//    return output;
//}

//[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
//public static double[] CumulativeAvgSum(double[] rate,
//                                        double[] days)
//{
//    double[] output = new double[rate.Length];

//    output[0] = rate[0] * days[0];

//    for(int i = 1; i < rate.Length; ++i)
//    {
//        output[i] = output[i - 1] + rate[i] * days[i];
//    }

//    return output;
//}

//[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
//public static float[] CumulativeSum(float[] rate,
//                                    float[] rateOverride,
//                                    int     untilIndex)
//{
//    float[] output = new float[rate.Length];

//    output[0] = rateOverride[0];

//    for(int i = 1; i < untilIndex; ++i)
//    {
//        output[i] = output[i - 1] + rateOverride[i];
//    }

//    for(int i = untilIndex; i < rate.Length; ++i)
//    {
//        output[i] = output[i - 1] + rate[i];
//    }

//    return output;
//}

//[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
//public static double[] CumulativeSum(double[] rate,
//                                     double[] rateOverride,
//                                     int      untilIndex)
//{
//    double[] output = new double[rate.Length];

//    output[0] = rateOverride[0];

//    for(int i = 1; i < untilIndex; ++i)
//    {
//        output[i] = output[i - 1] + rateOverride[i];
//    }

//    for(int i = untilIndex; i < rate.Length; ++i)
//    {
//        output[i] = output[i - 1] + rate[i];
//    }

//    return output;
//}