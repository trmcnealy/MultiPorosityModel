using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Microsoft.Data.Analysis;

namespace MultiPorosity.Driver
{
    public static class PrincipalComponentAnalysis
    {
        public static Series<string, double> eigenValues<a>(t<a> pca)
        {
            return pca.EigenValues;
        }

        public static DataFrame<a, string> eigenVectors<a>(t<a> pca)
        {
            return pca.EigenVectors;
        }

        internal static Series<a, double> normalizeSeriesUsing<a>(double mean, double stdDev, Series<a, double> series)
        {
            return SeriesModule.MapValues<double, double, a>(new normalizeSeriesUsing(mean, stdDev), series);
        }

        internal static Series<a, double> normalizeSeries<a>(Series<a, double> series)
        {
            OptionalValue<double>[] array = StatsInternal.valuesAllOpt<a, double>(series);
            int num = 1;
            double[] array2 = ArrayModule.Choose<OptionalValue<double>, double>(new normalizeSeries(), array);
            if (array2 == null)
            {
                throw new ArgumentNullException("array");
            }
            double[] array3 = new double[array2.Length];
            int moment = num;
            for (int i = 0; i < array3.Length; i++)
            {
                double[] array4 = array3;
                int num2 = i;
                double num3 = array2[i];
                array4[num2] = Convert.ToDouble(num3);
            }
            StatsInternal.Sums sums = StatsInternal.initSumsDense(moment, array3);
            double num4 = sums.sum / sums.nobs;
            array = StatsInternal.valuesAllOpt<a, double>(series);
            int num5 = 2;
            array2 = ArrayModule.Choose<OptionalValue<double>, double>(new normalizeSeries(), array);
            if (array2 != null)
            {
                array3 = new double[array2.Length];
                moment = num5;
                double num3 = num4;
                for (int i = 0; i < array3.Length; i++)
                {
                    double[] array5 = array3;
                    int num6 = i;
                    double num7 = array2[i];
                    array5[num6] = Convert.ToDouble(num7);
                }
                return normalizeSeriesUsing<a>(num3, Math.Sqrt(StatsInternal.varianceSums(StatsInternal.initSumsDense(moment, array3))), series);
            }
            throw new ArgumentNullException("array");
        }

        public static DataFrame<a, b> normalizeColumns<a, b>(DataFrame<a, b> df)
        {
            return FrameModule.SelectColumns<b, a, Series<a, double>, a>(new PrincipalComponentAnalysis<a, b>.normalizeColumns(), df);
        }

        public static t<b> pca<a, b>(DataFrame<a, b> dataFrame)
        {
            Evd<double>                                factorization = MemoryAllocator.Stats.covMatrix<a, b>(dataFrame).Evd(0);
            FSharpFunc<int, string>                    createPcNameForIndex = new createPcNameForIndex();
            b[]                                        colKeyArray = SeqModule.ToArray<b>(dataFrame.ColumnKeys);
            IEnumerable<double>                        eigenValuesSeq = factorization.EigenValues.Map<double>(new Func<Complex, double>(new eigenValuesSeq().Invoke), 1).Enumerate(1);
            IEnumerable<Vector<double>>                eigenVectorsSeq = factorization.EigenVectors.EnumerateColumns();
            IEnumerable<Tuple<double, Vector<double>>> enumerable = SeqModule.Zip<double, Vector<double>>(eigenValuesSeq, eigenVectorsSeq);
            if (enumerable == null)
            {
                throw new ArgumentNullException("source");
            }
            IEnumerable<Tuple<double, Vector<double>>> pairs = SeqModule.SortWith<Tuple<double, Vector<double>>>(new pairs@79(), enumerable);
            Series<string, double> eigenValues = SeriesModule.MapKeys<int, string, double>(createPcNameForIndex, F#\u0020Series\u0020extensions.Series.ofValues<double>(SeqModule.Map<Tuple<double, Vector<double>>, double>(new eigenValues@83(), pairs)));
            DataFrame<b, string> eigenVectors = FrameModule.SelectRowKeys<int, b, string>(new PrincipalComponentAnalysis<b>.eigenVectors@92(colKeyArray), F#\u0020Frame\u0020extensions.DataFrame.ofColumns.Static<string, Series<int, double>, int>(SeqModule.MapIndexed<Vector<double>, Tuple<string, Series<int, double>>>(new eigenVectors@90-1(), SeqModule.Map<Tuple<double, Vector<double>>, Vector<double>>(new eigenVectors@89-2(), pairs))));
            if (eigenVectors.RowCount != dataFrame.ColumnCount)
            {
                throw new Exception("Row count of eigen vectors does not match the input columns");
            }
            return new t<b>(eigenVectors, eigenValues);
        }

        [CompilationMapping(2)]
        [Serializable]
        public sealed class t<a> : IEquatable<t<a>>, IStructuralEquatable
        {
            [CompilationMapping(4, 0)]
            public DataFrame<a, string> EigenVectors
            {
                get
                {
                    return this.EigenVectors@;
                }
            }

            [CompilationMapping(4, 1)]
            public Series<string, double> EigenValues
            {
                get
                {
                    return this.EigenValues@;
                }
            }

            public t(DataFrame<a, string> eigenVectors, Series<string, double> eigenValues)
            {
                this.EigenVectors@ = eigenVectors;
                this.EigenValues@ = eigenValues;
            }

            [CompilerGenerated]
            public override string ToString()
            {
                return ExtraTopLevelOperators.PrintFormatToString<FSharpFunc<t<a>, string>>(new PrintfFormat<FSharpFunc<t<a>, string>, Unit, string, string, t<a>>("%+A")).Invoke(this);
            }

            [CompilerGenerated]
            public sealed int GetHashCode(IEqualityComparer comp)
            {
                if (this != null)
                {
                    int num = 0;
                    num = -0x61C88647 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic<Series<string, double>>(comp, this.EigenValues@) + ((num << 6) + (num >> 2)));
                    return -0x61C88647 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic<DataFrame<a, string>>(comp, this.EigenVectors@) + ((num << 6) + (num >> 2)));
                }
                return 0;
            }

            [CompilerGenerated]
            public sealed override int GetHashCode()
            {
                return this.GetHashCode(LanguagePrimitives.GenericEqualityComparer);
            }

            [CompilerGenerated]
            public sealed bool Equals(object obj, IEqualityComparer comp)
            {
                if (this != null)
                {
                    t<a> t = obj as t<a>;
                    return t != null && LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic<DataFrame<a, string>>(comp, this.EigenVectors@, t.EigenVectors@) && LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic<Series<string, double>>(comp, this.EigenValues@, t.EigenValues@);
                }
                return obj == null;
            }

            [CompilerGenerated]
            public sealed override bool Equals(t<a> obj)
            {
                if (this != null)
                {
                    return obj != null && LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic<DataFrame<a, string>>(this.EigenVectors@, obj.EigenVectors@) && LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic<Series<string, double>>(this.EigenValues@, obj.EigenValues@);
                }
                return obj == null;
            }

            [CompilerGenerated]
            public sealed override bool Equals(object obj)
            {
                t<a> t = obj as t<a>;
                return t != null && this.Equals(t);
            }

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal DataFrame<a, string> EigenVectors@;

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal Series<string, double> EigenValues@;
        }

        [Serializable]
        internal sealed class normalizeSeriesUsing@30 : FSharpFunc<double, double>
        {
            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal normalizeSeriesUsing@30(double mean, double stdDev)
            {
                this.mean = mean;
                this.stdDev = stdDev;
            }

            public override double Invoke(double x)
            {
                return (x - this.mean) / this.stdDev;
            }

            public double mean;

            public double stdDev;
        }

        [Serializable]
        internal sealed class normalizeSeries@35 : FSharpFunc<OptionalValue<double>, FSharpOption<double>>
        {
            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal normalizeSeries@35()
            {
            }

            public override FSharpOption<double> Invoke(OptionalValue<double> value)
            {
                if (value.HasValue)
                {
                    return FSharpOption<double>.Some(value.Value);
                }
                return null;
            }
        }

        [Serializable]
        internal sealed class normalizeSeries@36-1 : FSharpFunc<OptionalValue<double>, FSharpOption<double>>
        {
            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal normalizeSeries@36-1()
            {
            }

            public override FSharpOption<double> Invoke(OptionalValue<double> value)
            {
                if (value.HasValue)
                {
                    return FSharpOption<double>.Some(value.Value);
                }
                return null;
            }
        }

        [Serializable]
        internal sealed class normalizeColumns@46<a, b> : OptimizedClosures.FSharpFunc<b, ObjectSeries<a>, Series<a, double>>
        {
            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal normalizeColumns@46()
            {
            }

            public override Series<a, double> Invoke(b k, ObjectSeries<a> row)
            {
                return normalizeSeries<a>(row.As<double>());
            }
        }

        [Serializable]
        internal sealed class createPcNameForIndex@64 : FSharpFunc<int, string>
        {
            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal createPcNameForIndex@64()
            {
            }

            public override string Invoke(int n)
            {
                return PrintfModule.PrintFormatToStringThen<FSharpFunc<int, string>>(new PrintfFormat<FSharpFunc<int, string>, Unit, string, string, int>("PC%d")).Invoke(n + 1);
            }
        }

        [CompilationMapping(6)]
        [Serializable]
        [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
        internal sealed class eigenValuesSeq@71
        {
            public eigenValuesSeq@71()
            {
            }

            internal double Invoke(Complex x)
            {
                return x.Real;
            }
        }

        [Serializable]
        internal sealed class pairs@79 : OptimizedClosures.FSharpFunc<Tuple<double, Vector<double>>, Tuple<double, Vector<double>>, int>
        {
            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal pairs@79()
            {
            }

            public override int Invoke(Tuple<double, Vector<double>> a, Tuple<double, Vector<double>> b)
            {
                double item = b.Item1;
                double item2 = a.Item1;
                if (item < item2)
                {
                    return -1;
                }
                if (item > item2)
                {
                    return 1;
                }
                if (item == item2)
                {
                    return 0;
                }
                if (item2 == item2)
                {
                    return -1;
                }
                return (item == item) ? 1 : 0;
            }
        }

        [Serializable]
        internal sealed class eigenValues@83 : FSharpFunc<Tuple<double, Vector<double>>, double>
        {
            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal eigenValues@83()
            {
            }

            public override double Invoke(Tuple<double, Vector<double>> tupledArg)
            {
                return tupledArg.Item1;
            }
        }

        [Serializable]
        internal sealed class eigenVectors@92<b> : FSharpFunc<int, b>
        {
            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal eigenVectors@92(b[] colKeyArray)
            {
                this.colKeyArray = colKeyArray;
            }

            public override b Invoke(int i)
            {
                return this.colKeyArray[i];
            }

            public b[] colKeyArray;
        }

        [Serializable]
        internal sealed class eigenVectors@90-1 : OptimizedClosures.FSharpFunc<int, Vector<double>, Tuple<string, Series<int, double>>>
        {
            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal eigenVectors@90-1()
            {
            }

            public override Tuple<string, Series<int, double>> Invoke(int i, Vector<double> x)
            {
                return new Tuple<string, Series<int, double>>(PrintfModule.PrintFormatToStringThen<FSharpFunc<int, string>>(new PrintfFormat<FSharpFunc<int, string>, Unit, string, string, int>("PC%d")).Invoke(i + 1), F#\u0020Series\u0020extensions.Series.ofValues<double>(x.Enumerate(1)));
            }
        }

        [Serializable]
        internal sealed class eigenVectors@89-2 : FSharpFunc<Tuple<double, Vector<double>>, Vector<double>>
        {
            [CompilerGenerated]
            [DebuggerNonUserCode]
            internal eigenVectors@89-2()
            {
            }

            public override Vector<double> Invoke(Tuple<double, Vector<double>> tupledArg)
            {
                return tupledArg.Item2;
            }
        }
    }
}