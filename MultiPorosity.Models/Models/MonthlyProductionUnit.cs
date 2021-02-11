using System;
using System.Collections.Generic;
using System.Diagnostics;

using Engineering.DataSource.Tools;

namespace MultiPorosity.Models
{
    public sealed class MonthlyProductionUnit
    {
        public const int GasUnitType   = 0;
        public const int OilUnitType   = 1;
        public const int WaterUnitType = 2;

        public List<ProductionRecord> MonthlyProductionRecords { get; }

        public List<UnitPoint> Points { get; } // +1 MonthlyProductionRecords.Count

        public List<MonthlyUnit> Units { get; }

        public MonthlyProductionUnit(List<ProductionRecord> monthlyProductionRecords)
        {
            MonthlyProductionRecords = monthlyProductionRecords;

            Points = new((2 * MonthlyProductionRecords.Count) + 1);

            Units = new(MonthlyProductionRecords.Count);

            ProductionRecord firstProductionRecord = MonthlyProductionRecords[0];

            DateTime startDate = firstProductionRecord.Date;

            MonthlyUnit unit = new(startDate, firstProductionRecord);

            Units.Add(unit);

            UnitPoint Start  = unit.Start;
            UnitPoint Middle = unit.Middle;
            UnitPoint End    = unit.End;

            Points.Add(Start);
            Points.Add(Middle);
            Points.Add(End);

            for(int i = 1; i < MonthlyProductionRecords.Count; ++i)
            {
                unit = new(startDate, Units[^1], MonthlyProductionRecords[i]);

                Units.Add(unit);

                Start  = unit.Start;
                Middle = unit.Middle;
                End    = unit.End;

                Points.Add(Middle);
                Points.Add(End);
            }

            //unit                   =  Units[0];
            //unit.Start.GasVolume   *= 0.9; //MonthlyUnit.FindStartY(unit.MonthlyProductionRecord.Gas, 1, (unit.Middle.Days, unit.Middle.GasVolume), (unit.End.Days, unit.End.GasVolume));
            //unit.Start.OilVolume   *= 0.9; //MonthlyUnit.FindStartY(unit.MonthlyProductionRecord.Oil,   1, (unit.Middle.Days, unit.Middle.OilVolume),   (unit.End.Days, unit.End.OilVolume));
            //unit.Start.WaterVolume *= 0.9; //MonthlyUnit.FindStartY(unit.MonthlyProductionRecord.Water, 1, (unit.Middle.Days, unit.Middle.WaterVolume), (unit.End.Days, unit.End.WaterVolume));

            //unit                 = Units[^1];
            //unit.End.GasVolume   *= 0.9; //MonthlyUnit.FindEndY(unit.MonthlyProductionRecord.Gas,   (unit.Start.Days, unit.Start.GasVolume),   (unit.Middle.Days, unit.Middle.GasVolume),   unit.End.Days);
            //unit.End.OilVolume   *= 0.9; //MonthlyUnit.FindEndY(unit.MonthlyProductionRecord.Oil,   (unit.Start.Days, unit.Start.OilVolume),   (unit.Middle.Days, unit.Middle.OilVolume),   unit.End.Days);
            //unit.End.WaterVolume *= 0.9; //MonthlyUnit.FindEndY(unit.MonthlyProductionRecord.Water, (unit.Start.Days, unit.Start.WaterVolume), (unit.Middle.Days, unit.Middle.WaterVolume), unit.End.Days);
        }

        public List<double> GetUnitPoints(int unit_type)
        {
            switch(unit_type)
            {
                case GasUnitType:
                {
                    List<double> args = new(Points.Count);

                    for(int i = 0; i < Points.Count; ++i)
                    {
                        args.Insert(i, Points[i].GasVolume);
                    }

                    return args;
                }
                case OilUnitType:
                {
                    List<double> args = new(Points.Count);

                    for(int i = 0; i < Points.Count; ++i)
                    {
                        args.Insert(i, Points[i].OilVolume);
                    }

                    return args;
                }
                case WaterUnitType:
                {
                    List<double> args = new(Points.Count);

                    for(int i = 0; i < Points.Count; ++i)
                    {
                        args.Insert(i, Points[i].WaterVolume);
                    }

                    return args;
                }
            }

            return new();
        }

        public double[] GetGasFunctorArgs(int unit_index)
        {
            return new double[]
            {
                Units[unit_index].Start.GasVolume, Units[unit_index].Middle.GasVolume, Units[unit_index].End.GasVolume
            };
        }

        public double[] GetOilFunctorArgs(int unit_index)
        {
            return new double[]
            {
                Units[unit_index].Start.OilVolume, Units[unit_index].Middle.OilVolume, Units[unit_index].End.OilVolume
            };
        }

        public double[] GetWaterFunctorArgs(int unit_index)
        {
            return new double[]
            {
                Units[unit_index].Start.WaterVolume, Units[unit_index].Middle.WaterVolume, Units[unit_index].End.WaterVolume
            };
        }

        public double GasFunctor(int             unit_index,
                                 double[]        args,
                                 params double[] additional_args)
        {
            Units[unit_index].Start.GasVolume  = args[0];
            Units[unit_index].Middle.GasVolume = args[1];
            Units[unit_index].End.GasVolume    = args[2];

            return Units[unit_index].GasError();
        }

        public double OilFunctor(int             unit_index,
                                 double[]        args,
                                 params double[] additional_args)
        {
            Units[unit_index].Start.OilVolume  = args[0];
            Units[unit_index].Middle.OilVolume = args[1];
            Units[unit_index].End.OilVolume    = args[2];

            return Units[unit_index].OilError();
        }

        public double WaterFunctor(int             unit_index,
                                   double[]        args,
                                   params double[] additional_args)
        {
            Units[unit_index].Start.WaterVolume  = args[0];
            Units[unit_index].Middle.WaterVolume = args[1];
            Units[unit_index].End.WaterVolume    = args[2];

            return Units[unit_index].WaterError();
        }

        public double GenerateDailyGas(int day)
        {
            for(int i = 0; i < Units.Count; ++i)
            {
                if(Units[i].Start.Days <= day && day <= Units[i].End.Days)
                {
                    return Units[i].GenerateDailyGas(day);
                }
            }

            return double.NaN;
        }

        public double GenerateDailyOil(int day)
        {
            for(int i = 0; i < Units.Count; ++i)
            {
                if(Units[i].Start.Days <= day && day <= Units[i].End.Days)
                {
                    return Units[i].GenerateDailyOil(day);
                }
            }

            return double.NaN;
        }

        public double GenerateDailyWater(int day)
        {
            for(int i = 0; i < Units.Count; ++i)
            {
                if(Units[i].Start.Days <= day && day <= Units[i].End.Days)
                {
                    return Units[i].GenerateDailyWater(day);
                }
            }

            return double.NaN;
        }

        public sealed class UnitPoint
        {
            private readonly DateTime _startDate;

            private double _gasVolume;

            private double _oilVolume;

            private double _waterVolume;

            public DateTime Date { get; }

            public double GasVolume
            {
                get { return _gasVolume; }
                set
                {
                    _gasVolume = value;

                    //if(double.IsNaN(_gasVolume))
                    //{
                    //    Debugger.Break();
                    //}
                }
            }

            public double OilVolume
            {
                get { return _oilVolume; }
                set
                {
                    _oilVolume = value;

                    //if(double.IsNaN(_gasVolume))
                    //{
                    //    Debugger.Break();
                    //}
                }
            }

            public double WaterVolume
            {
                get { return _waterVolume; }
                set
                {
                    _waterVolume = value;

                    //if(double.IsNaN(_gasVolume))
                    //{
                    //    Debugger.Break();
                    //}
                }
            }

            public double Days
            {
                get { return 1 + (Date - _startDate).Days; }
            }

            public UnitPoint(DateTime startDate,
                             DateTime date,
                             double   gasVolume,
                             double   oilVolume,
                             double   waterVolume)
            {
                _startDate = startDate;
                Date       = date;

                double daysInMonth = Date.DaysInMonth();

                GasVolume   = gasVolume   / daysInMonth;
                OilVolume   = oilVolume   / daysInMonth;
                WaterVolume = waterVolume / daysInMonth;
            }
        }

        public sealed class MonthlyUnit
        {
            private Engineering.DataSource.Equations.Linear _gasFirstHalf;
            private Engineering.DataSource.Equations.Linear _gasSecondHalf;
            private Engineering.DataSource.Equations.Linear _oilFirstHalf;
            private Engineering.DataSource.Equations.Linear _oilSecondHalf;
            private Engineering.DataSource.Equations.Linear _waterFirstHalf;
            private Engineering.DataSource.Equations.Linear _waterSecondHalf;

            public UnitPoint Start { get; set; }

            public UnitPoint Middle { get; set; }

            public UnitPoint End { get; set; }

            public ProductionRecord MonthlyProductionRecord { get; }

            //public MonthlyUnit(UnitPoint        start,
            //                   UnitPoint        middle,
            //                   UnitPoint        end,
            //                   ProductionRecord monthlyProductionRecord)
            //{
            //    MonthlyProductionRecord = monthlyProductionRecord;

            //    Start  = start;
            //    Middle = middle;
            //    End    = end;
            //}

            public MonthlyUnit(DateTime         startDate,
                               ProductionRecord monthlyProductionRecord)
            {
                MonthlyProductionRecord = monthlyProductionRecord;

                double daysInMonth = MonthlyProductionRecord.Date.DaysInMonth();

                Start = new UnitPoint(startDate,
                                      new DateTime(MonthlyProductionRecord.Date.Year, MonthlyProductionRecord.Date.Month, 1),
                                      MonthlyProductionRecord.Gas,
                                      MonthlyProductionRecord.Oil,
                                      MonthlyProductionRecord.Water);

                Middle = new UnitPoint(startDate,
                                       new DateTime(MonthlyProductionRecord.Date.Year, MonthlyProductionRecord.Date.Month, (int)Math.Round(daysInMonth / 2.0, MidpointRounding.ToZero)),
                                       MonthlyProductionRecord.Gas,
                                       MonthlyProductionRecord.Oil,
                                       MonthlyProductionRecord.Water);

                End = new UnitPoint(startDate,
                                    new DateTime(MonthlyProductionRecord.Date.Year, MonthlyProductionRecord.Date.Month, (int)daysInMonth),
                                    MonthlyProductionRecord.Gas,
                                    MonthlyProductionRecord.Oil,
                                    MonthlyProductionRecord.Water);
            }

            public MonthlyUnit(DateTime         startDate,
                               MonthlyUnit      previousMonthlyUnit,
                               ProductionRecord monthlyProductionRecord)
            {
                MonthlyProductionRecord = monthlyProductionRecord;

                double daysInMonth = MonthlyProductionRecord.Date.DaysInMonth();

                Start = previousMonthlyUnit.End;

                Start.GasVolume += MonthlyProductionRecord.Gas / daysInMonth;
                Start.GasVolume /= 2.0;

                Start.OilVolume += MonthlyProductionRecord.Oil / daysInMonth;
                Start.OilVolume /= 2.0;

                Start.WaterVolume += MonthlyProductionRecord.Water / daysInMonth;
                Start.WaterVolume /= 2.0;

                Middle = new UnitPoint(startDate,
                                       new DateTime(MonthlyProductionRecord.Date.Year, MonthlyProductionRecord.Date.Month, (int)Math.Round(daysInMonth / 2.0, MidpointRounding.ToZero)),
                                       MonthlyProductionRecord.Gas,
                                       MonthlyProductionRecord.Oil,
                                       MonthlyProductionRecord.Water);

                End = new UnitPoint(startDate,
                                    new DateTime(MonthlyProductionRecord.Date.Year, MonthlyProductionRecord.Date.Month, (int)daysInMonth),
                                    MonthlyProductionRecord.Gas,
                                    MonthlyProductionRecord.Oil,
                                    MonthlyProductionRecord.Water);
            }

            //public MonthlyUnit(DateTime         startDate,
            //                   MonthlyUnit      previousMonthlyUnit,
            //                   ProductionRecord monthlyProductionRecord,
            //                   MonthlyUnit      nextMonthlyUnit)
            //{
            //    MonthlyProductionRecord = monthlyProductionRecord;

            //    double daysInMonth = MonthlyProductionRecord.Date.DaysInMonth();

            //    Start = previousMonthlyUnit.End;

            //    Middle = new UnitPoint(startDate,
            //                           new DateTime(MonthlyProductionRecord.Date.Year, MonthlyProductionRecord.Date.Month, (int)Math.Round(daysInMonth / 2.0, MidpointRounding.ToZero)),
            //                           MonthlyProductionRecord.Gas    ,
            //                           MonthlyProductionRecord.Oil    ,
            //                           MonthlyProductionRecord.Water  );

            //    End = new UnitPoint(startDate,
            //                        new DateTime(MonthlyProductionRecord.Date.Year, MonthlyProductionRecord.Date.Month, (int)daysInMonth),
            //                        MonthlyProductionRecord.Gas    ,
            //                        MonthlyProductionRecord.Oil    ,
            //                        MonthlyProductionRecord.Water );

            //    nextMonthlyUnit.Start = End;

            //    Start.GasVolume   = FindStartY(MonthlyProductionRecord.Gas,   Start.Days, (Middle.Days, Middle.GasVolume),   (End.Days, End.GasVolume));
            //    Start.OilVolume   = FindStartY(MonthlyProductionRecord.Oil,   Start.Days, (Middle.Days, Middle.OilVolume),   (End.Days, End.OilVolume));
            //    Start.WaterVolume = FindStartY(MonthlyProductionRecord.Water, Start.Days, (Middle.Days, Middle.WaterVolume), (End.Days, End.WaterVolume));

            //    End.GasVolume   = FindStartY(MonthlyProductionRecord.Gas,   End.Days, (Middle.Days, Middle.GasVolume),   (Start.Days, Start.GasVolume));
            //    End.OilVolume   = FindStartY(MonthlyProductionRecord.Oil,   End.Days, (Middle.Days, Middle.OilVolume),   (Start.Days, Start.OilVolume));
            //    End.WaterVolume = FindStartY(MonthlyProductionRecord.Water, End.Days, (Middle.Days, Middle.WaterVolume), (Start.Days, Start.WaterVolume));
            //}

            public double GenerateDailyGas(int day)
            {
                if(day <= Middle.Days)
                {
                    return _gasFirstHalf.y(day);
                }

                return _gasSecondHalf.y(day);
            }

            public double GenerateDailyOil(int day)
            {
                if(day <= Middle.Days)
                {
                    return _oilFirstHalf.y(day);
                }

                return _oilSecondHalf.y(day);
            }

            public double GenerateDailyWater(int day)
            {
                if(day <= Middle.Days)
                {
                    return _waterFirstHalf.y(day);
                }

                return _waterSecondHalf.y(day);
            }

            #region Methods

            private static double Area((double X, double Y) A,
                                       (double X, double Y) B,
                                       (double X, double Y) C)
            {
                double area = ((A.X * (B.Y - C.Y)) + (B.X * (C.Y - A.Y)) + (C.X * (A.Y - B.Y))) / 2.0;

                return area;
            }

            internal static double FindStartY(double               area,
                                              double               AX,
                                              (double X, double Y) B,
                                              (double X, double Y) C)
            {
                double lower_tri = Area((B.X, B.Y), (AX, 0.0),  (C.X, 0.0));
                double right_tri = Area((C.X, C.Y), (B.X, B.Y), (C.X, 0.0));

                return ((2.0 * (area - lower_tri - right_tri)) - (AX * (B.Y - C.Y)) - (B.X * C.Y) + (C.X * B.Y)) / (C.X - B.X);
            }

            internal static double FindEndY(double               area,
                                            (double X, double Y) A,
                                            (double X, double Y) B,
                                            double               CX)
            {
                double left_tri  = Area((A.X, A.Y), (A.X, 0.0), (B.X, B.Y));
                double lower_tri = Area((B.X, B.Y), (A.X, 0.0), (CX, 0.0));

                return ((2.0 * (area - left_tri - lower_tri)) - (CX * (A.Y - B.Y)) + (B.X * A.Y) - (A.X * B.Y)) / (B.X - A.X);
            }

            public static double UnitArea((double Days, double Volume) Start,
                                          (double Days, double Volume) Middle,
                                          (double Days, double Volume) End)
            {
                return Area((Start.Days, Start.Volume),   (Start.Days, 0.0),            (Middle.Days, Middle.Volume)) +
                       Area((Middle.Days, Middle.Volume), (Start.Days, 0.0),            (End.Days, 0.0))              +
                       Area((End.Days, End.Volume),       (Middle.Days, Middle.Volume), (End.Days, 0.0));
            }

            public double GasVolume()
            {
                _gasFirstHalf  = new(Middle.GasVolume, Middle.Days, Start.GasVolume, Start.Days);
                _gasSecondHalf = new(End.GasVolume, End.Days, Middle.GasVolume, Middle.Days);

                return UnitArea((Start.Days, Start.GasVolume), (Middle.Days, Middle.GasVolume), (End.Days, End.GasVolume));
            }

            public double OilVolume()
            {
                _oilFirstHalf  = new(Middle.OilVolume, Middle.Days, Start.OilVolume, Start.Days);
                _oilSecondHalf = new(End.OilVolume, End.Days, Middle.OilVolume, Middle.Days);

                return UnitArea((Start.Days, Start.OilVolume), (Middle.Days, Middle.OilVolume), (End.Days, End.OilVolume));
            }

            public double WaterVolume()
            {
                _waterFirstHalf  = new(Middle.WaterVolume, Middle.Days, Start.WaterVolume, Start.Days);
                _waterSecondHalf = new(End.WaterVolume, End.Days, Middle.WaterVolume, Middle.Days);

                return UnitArea((Start.Days, Start.WaterVolume), (Middle.Days, Middle.WaterVolume), (End.Days, End.WaterVolume));
            }

            public double GasError()
            {
                return MonthlyProductionRecord.Gas - GasVolume();
            }

            public double OilError()
            {
                return MonthlyProductionRecord.Oil - OilVolume();
            }

            public double WaterError()
            {
                return MonthlyProductionRecord.Water - WaterVolume();
            }

            #endregion
        }
    }
}