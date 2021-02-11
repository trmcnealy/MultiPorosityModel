using System;
using System.Linq;

using Engineering.DataSource.Equations;
using Engineering.DataSource.Geometry;
using Engineering.DataSource.Tools;

namespace MultiPorosity.Models
{
    public sealed class MonthlyProductionRecord
    {
        public int Index { get; }

        public DateTime StartDate { get; }

        public DateTime Date { get; }

        public double Days { get; }

        //public int Year { get; }

        //public int Month { get; }

        //public  DaysInMonth { get; }

        public double GasVolume { get; }

        public double OilVolume { get; }

        public double WaterVolume { get; }

        public ProductionRecord DailyProductionRecord { get; }

        //public MonthlyProductionRecord? PreviousMonth { get; set; }

        //public MonthlyProductionRecord? NextMonth { get; set; }

        //public double AverageDailyGas { get; private set; }

        //public double AverageDailyOil { get; private set; }

        //public double AverageDailyWater { get; private set; }

        //private Linear? PreviousGasEquation;
        //private Linear? PreviousOilEquation;
        //private Linear? PreviousWaterEquation;

        //private Linear? NextGasEquation;
        //private Linear? NextOilEquation;
        //private Linear? NextWaterEquation;

        //private readonly DateTime _startOfMonth;

        public MonthlyProductionRecord(ProductionRecord productionRecord)
        {
            Index = productionRecord.Index;

            StartDate = new(productionRecord.Date.Year, productionRecord.Date.Month, 1);

            int DaysInMonth = StartDate.DaysInMonth();

            Date = new(productionRecord.Date.Year, productionRecord.Date.Month, (int)Math.Floor(DaysInMonth / 2.0));

            Days = productionRecord.Days + (int)Math.Floor(DaysInMonth / 2.0);

            //Year  = year;
            //Month = month;

            GasVolume   = productionRecord.Gas;
            OilVolume   = productionRecord.Oil;
            WaterVolume = productionRecord.Water;

            double AverageDailyGas   = GasVolume   / DaysInMonth;
            double AverageDailyOil   = OilVolume   / DaysInMonth;
            double AverageDailyWater = WaterVolume / DaysInMonth;

            DailyProductionRecord = new(Index, Date, Days, AverageDailyGas, AverageDailyOil, AverageDailyWater);
        }

        public MonthlyProductionRecord(int      index,
                                       DateTime startDate,
                                       int      year,
                                       int      month,
                                       double   gasVolume,
                                       double   oilVolume,
                                       double   waterVolume)
        {
            Index = index;

            StartDate = new(year, month, 1);

            int DaysInMonth = StartDate.DaysInMonth();

            Date = new(year, month, (int)Math.Floor(DaysInMonth / 2.0));

            Days = (Date - startDate).Days;

            //Year  = year;
            //Month = month;

            GasVolume   = gasVolume;
            OilVolume   = oilVolume;
            WaterVolume = waterVolume;

            double AverageDailyGas   = GasVolume   / DaysInMonth;
            double AverageDailyOil   = OilVolume   / DaysInMonth;
            double AverageDailyWater = WaterVolume / DaysInMonth;

            DailyProductionRecord = new(Index, Date, Days, AverageDailyGas, AverageDailyOil, AverageDailyWater);
        }

        //public static implicit operator ProductionRecord(MonthlyProductionRecord monthlyProductionRecord)
        //{
        //    return monthlyProductionRecord.DailyProductionRecord;
        //}

        //public double Update()
        //{
        //    int midMonth1      = (int)Math.Floor(DaysInMonth / 2.0);
        //    int deltaMidMonth1 = DateOffset + midMonth1;

        //    if(NextMonth is not null)
        //    {
        //        double deltaMidMonth2 = NextMonth.DateOffset + Math.Floor(NextMonth.DaysInMonth / 2.0);

        //        NextGasEquation   = new Linear(NextMonth.AverageDailyGas,   deltaMidMonth2, AverageDailyGas,   deltaMidMonth1);
        //        NextOilEquation   = new Linear(NextMonth.AverageDailyOil,   deltaMidMonth2, AverageDailyOil,   deltaMidMonth1);
        //        NextWaterEquation = new Linear(NextMonth.AverageDailyWater, deltaMidMonth2, AverageDailyWater, deltaMidMonth1);
        //    }

        //    if(PreviousMonth is not null)
        //    {
        //        double deltaMidMonth0 = PreviousMonth.DateOffset + Math.Floor(PreviousMonth.DaysInMonth / 2.0);

        //        PreviousGasEquation   = new Linear(AverageDailyGas,   deltaMidMonth1, PreviousMonth.AverageDailyGas,   deltaMidMonth0);
        //        PreviousOilEquation   = new Linear(AverageDailyOil,   deltaMidMonth1, PreviousMonth.AverageDailyOil,   deltaMidMonth0);
        //        PreviousWaterEquation = new Linear(AverageDailyWater, deltaMidMonth1, PreviousMonth.AverageDailyWater, deltaMidMonth0);
        //    }

        //    if(PreviousMonth is null && NextMonth is not null)
        //    {
        //        PreviousGasEquation   = NextGasEquation;
        //        PreviousOilEquation   = NextOilEquation;
        //        PreviousWaterEquation = NextWaterEquation;
        //    }

        //    if(NextMonth is null && PreviousMonth is not null)
        //    {
        //        NextGasEquation   = PreviousGasEquation;
        //        NextOilEquation   = PreviousOilEquation;
        //        NextWaterEquation = PreviousWaterEquation;
        //    }

        //    double dailyGas;
        //    double dailyOil;
        //    double dailyWater;

        //    for(int i = 0; i < midMonth1; ++i)
        //    {
        //        dailyGas   = PreviousGasEquation!.Value.y(DateOffset   + i);
        //        dailyOil   = PreviousOilEquation!.Value.y(DateOffset   + i);
        //        dailyWater = PreviousWaterEquation!.Value.y(DateOffset + i);

        //        DailyProductionRecords[i] = new ProductionRecord(DateOffset + i, StartOfMonth.AddDays(i), DateOffset + i, dailyGas, dailyOil, dailyWater);
        //    }

        //    dailyGas   = AverageDailyGas;
        //    dailyOil   = AverageDailyOil;
        //    dailyWater = AverageDailyWater;

        //    DailyProductionRecords[midMonth1] = new ProductionRecord(DateOffset + midMonth1, StartOfMonth.AddDays(midMonth1), DateOffset + midMonth1, dailyGas, dailyOil, dailyWater);

        //    for(int i = midMonth1 + 1; i < DailyProductionRecords.Length; ++i)
        //    {
        //        dailyGas   = NextGasEquation!.Value.y(DateOffset   + i);
        //        dailyOil   = NextOilEquation!.Value.y(DateOffset   + i);
        //        dailyWater = NextWaterEquation!.Value.y(DateOffset + i);

        //        DailyProductionRecords[i] = new ProductionRecord(DateOffset + i, StartOfMonth.AddDays(i), DateOffset + i, dailyGas, dailyOil, dailyWater);
        //    }

        //    double errorGas   = (GasVolume   - DailyProductionRecords.Sum(p => p.Gas))   / DaysInMonth;
        //    double errorOil   = (OilVolume   - DailyProductionRecords.Sum(p => p.Oil))   / DaysInMonth;
        //    double errorWater = (WaterVolume - DailyProductionRecords.Sum(p => p.Water)) / DaysInMonth;

        //    AverageDailyGas   += errorGas;
        //    AverageDailyOil   += errorOil;
        //    AverageDailyWater += errorWater;

        //    return errorGas + errorOil + errorWater;
        //}
    }
}