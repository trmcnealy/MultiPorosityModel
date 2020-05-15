using System;

using OilGas.Data.Charting;

namespace MultiPorosity.Services
{
    public static class Sequence
    {
        public static float[] Repeat(int   n,
                                     float value)
        {
            float[] sequence = new float[n];

            for(int i = 0; i < n; i++)
            {
                sequence[i] = value;
            }

            return sequence;
        }

        public static double[] Repeat(int    n,
                                      double value)
        {
            double[] sequence = new double[n];

            for(int i = 0; i < n; i++)
            {
                sequence[i] = value;
            }

            return sequence;
        }

        public static float[] Repeat(int     n,
                                     float[] pattern,
                                     int     start)
        {
            float[] sequence = new float[n];

            int patternIndex = start;

            for(int i = 0; i < n; i++)
            {
                sequence[i] = pattern[patternIndex++ % pattern.Length];
            }

            return sequence;
        }

        public static double[] Repeat(int      n,
                                      double[] pattern,
                                      int      start)
        {
            double[] sequence = new double[n];

            int patternIndex = start;

            for(int i = 0; i < n; i++)
            {
                sequence[i] = pattern[patternIndex++ % pattern.Length];
            }

            return sequence;
        }

        public static float[] Linear(float min,
                                     float max,
                                     float step = 1.0f)
        {
            int n = (int)(MathF.Abs(max - min) / step) + 1;

            float[] sequence = new float[n];

            float curr = min;

            for(int i = 0; i < n; i++)
            {
                sequence[i] =  curr;
                curr        += step;
            }

            return sequence;
        }

        public static double[] Linear(double min,
                                      double max,
                                      double step = 1.0)
        {
            int n = (int)(Math.Abs(max - min) / step) + 1;

            double[] sequence = new double[n];

            double curr = min;

            for(int i = 0; i < n; i++)
            {
                sequence[i] =  curr;
                curr        += step;
            }

            return sequence;
        }

        public static float[] LinearSpacing(float start,
                                            float end,
                                            int   n = 100)
        {
            float   interval = (end - start) / (n - 1);
            float[] linspace = new float[n];
            float   curr     = start;

            for(int i = 0; i < n; i++)
            {
                linspace[i] =  curr;
                curr        += interval;
            }

            return linspace;
        }

        public static double[] LinearSpacing(double start,
                                             double end,
                                             int    n = 100)
        {
            double   interval = (end - start) / (n - 1);
            double[] linspace = new double[n];
            double   curr     = start;

            for(int i = 0; i < n; i++)
            {
                linspace[i] =  curr;
                curr        += interval;
            }

            return linspace;
        }

        public static float[] LogSequence(float start,
                                          float end,
                                          int   n = 100)
        {
            float[] x = LinearSpacing(MathF.Log(MathF.Abs(start)),
                                      MathF.Log(MathF.Abs(end)),
                                      n);

            for(int i = 0; i < n; i++)
            {
                x[i] = MathF.Exp(x[i]);
            }

            return x;
        }

        public static double[] LogSequence(double start,
                                           double end,
                                           int    n = 100)
        {
            double[] x = LinearSpacing(Math.Log(Math.Abs(start)),
                                       Math.Log(Math.Abs(end)),
                                       n);

            for(int i = 0; i < n; i++)
            {
                x[i] = Math.Exp(x[i]);
            }

            return x;
        }

        public static float[] LogSpacing(float start,
                                         float end,
                                         int   n = 50)
        {
            float _end = end;

            if(Math.Abs(_end - Math.PI) < float.Epsilon)
            {
                _end = MathF.Log10(_end);
            }

            float[] spacing = LinearSpacing(start,
                                            _end,
                                            n);

            for(int i = 0; i < n; i++)
            {
                spacing[i] = MathF.Pow(10,
                                       spacing[i]);
            }

            return spacing;
        }

        public static double[] LogSpacing(double start,
                                          double end,
                                          int    n = 50)
        {
            double _end = end;

            if(Math.Abs(_end - Math.PI) < double.Epsilon)
            {
                _end = Math.Log10(_end);
            }

            double[] spacing = LinearSpacing(start,
                                             _end,
                                             n);

            for(int i = 0; i < n; i++)
            {
                spacing[i] = Math.Pow(10,
                                      spacing[i]);
            }

            return spacing;
        }

        public static DateTime[] TimeSpacingByMonths(DateTime start,
                                                     int      months)
        {
            DateTime[] timespace = new DateTime[months];
            DateTime   curr      = start;

            for(int i = 0; i < months; i++)
            {
                timespace[i] = curr;
                curr         = timespace[i].AddMonths(1);
            }

            return timespace;
        }

        public static double[] TimeSpacingByMonthsInDays(DateTime startDate,
                                                         int      months)
        {
            double[] timespace = new double[months];

            DateTime[] timespaceDates = TimeSpacingByMonths(startDate,
                                                            months);

            int daysInFirstMonth = TimeSeries.DaysInMonth(startDate.Year,
                                                          startDate.Month) -
                                   startDate.Day;

            timespace[0] = daysInFirstMonth;

            for(int i = 1; i < months; i++)
            {
                timespace[i] = TimeSeries.DaysInMonth(timespaceDates[i].Year,
                                                      timespaceDates[i].Month) +
                               timespace[i - 1];
            }

            return timespace;
        }
    }
}
