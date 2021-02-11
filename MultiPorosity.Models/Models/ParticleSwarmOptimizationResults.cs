using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MultiPorosity.Models
{
    public sealed class TriplePorosityOptimizationResultsColumn
    {
        private static readonly PropertyInfo[] properties;

        static TriplePorosityOptimizationResultsColumn()
        {
            properties = typeof(TriplePorosityOptimizationResults).GetProperties().Where(prop => prop.GetIndexParameters().Length == 0).ToArray();
        }

        private readonly int                                       _columnIndex;
        private readonly TriplePorosityOptimizationResults[] _triplePorosityOptimizationResultsRecords;

        public string Type { get; init; }

        public TriplePorosityOptimizationResultsColumn(int                                 columnIndex,
                                                       TriplePorosityOptimizationResults[] triplePorosityOptimizationResults)
        {
            _columnIndex                              = columnIndex;
            _triplePorosityOptimizationResultsRecords = triplePorosityOptimizationResults;

            Type = properties[_columnIndex].PropertyType.Name;
        }

        public TriplePorosityOptimizationResultsColumn(int                                                                                  columnIndex,
                                                       Engineering.DataSource.Array<TriplePorosityOptimizationResults> triplePorosityOptimizationResults)
        {
            _columnIndex                              = columnIndex;
            _triplePorosityOptimizationResultsRecords = triplePorosityOptimizationResults.ToArray();

            Type = properties[_columnIndex].PropertyType.Name;
        }

        public object this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _triplePorosityOptimizationResultsRecords[index][_columnIndex];
            }
        }

        public object[] ToArray()
        {
            object[] array = new object[_triplePorosityOptimizationResultsRecords.Length];

            for(int i = 0; i < _triplePorosityOptimizationResultsRecords.Length; ++i)
            {
                array[i] = this[i];
            }

            return array;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TriplePorosityOptimizationResults
    {
        public long Iteration { get; set; }

        public long SwarmIndex { get; set; }

        public long ParticleIndex { get; set; }

        public double Residual { get; set; }

        public double km { get; set; }

        public double kmVelocity { get; set; }

        public double kF { get; set; }

        public double kFVelocity { get; set; }

        public double kf { get; set; }

        public double kfVelocity { get; set; }

        public double ye { get; set; }

        public double yeVelocity { get; set; }

        public double LF { get; set; }

        public double LFVelocity { get; set; }

        public double Lf { get; set; }

        public double LfVelocity { get; set; }

        public double sk { get; set; }

        public double skVelocity { get; set; }

        public TriplePorosityOptimizationResults(double iteration,
                                                 double swarmIndex,
                                                 double particleIndex,
                                                 double residual,
                                                 double km,
                                                 double kmVelocity,
                                                 double kF,
                                                 double kFVelocity,
                                                 double kf,
                                                 double kfVelocity,
                                                 double ye,
                                                 double yeVelocity,
                                                 double lF,
                                                 double lFVelocity,
                                                 double lf,
                                                 double lfVelocity,
                                                 double sk,
                                                 double skVelocity)
        {
            Iteration       = (long)iteration;
            SwarmIndex      = (long)swarmIndex;
            ParticleIndex   = (long)particleIndex;
            Residual        = residual;
            this.km         = km;
            this.kmVelocity = kmVelocity;
            this.kF         = kF;
            this.kFVelocity = kFVelocity;
            this.kf         = kf;
            this.kfVelocity = kfVelocity;
            this.ye         = ye;
            this.yeVelocity = yeVelocity;
            LF              = lF;
            LFVelocity      = lFVelocity;
            Lf              = lf;
            LfVelocity      = lfVelocity;
            this.sk         = sk;
            this.skVelocity = skVelocity;
        }

        public TriplePorosityOptimizationResults(double[] values)
        {
            int index = 0;
            Iteration     = (long)values[index++];
            SwarmIndex    = (long)values[index++];
            ParticleIndex = (long)values[index++];
            Residual      = values[index++];
            km            = values[index++];
            kmVelocity    = values[index++];
            kF            = values[index++];
            kFVelocity    = values[index++];
            kf            = values[index++];
            kfVelocity    = values[index++];
            ye            = values[index++];
            yeVelocity    = values[index++];
            LF            = values[index++];
            LFVelocity    = values[index++];
            Lf            = values[index++];
            LfVelocity    = values[index++];
            sk            = values[index++];
            skVelocity    = values[index];
        }

        public object this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                switch(index)
                {
                    case 0:
                    {
                        return Iteration;
                    }
                    case 1:
                    {
                        return SwarmIndex;
                    }
                    case 2:
                    {
                        return ParticleIndex;
                    }
                    case 3:
                    {
                        return Residual;
                    }
                    case 4:
                    {
                        return km;
                    }
                    case 5:
                    {
                        return kmVelocity;
                    }
                    case 6:
                    {
                        return kF;
                    }
                    case 7:
                    {
                        return kFVelocity;
                    }
                    case 8:
                    {
                        return kf;
                    }
                    case 9:
                    {
                        return kfVelocity;
                    }
                    case 10:
                    {
                        return ye;
                    }
                    case 11:
                    {
                        return yeVelocity;
                    }
                    case 12:
                    {
                        return LF;
                    }
                    case 13:
                    {
                        return LFVelocity;
                    }
                    case 14:
                    {
                        return Lf;
                    }
                    case 15:
                    {
                        return LfVelocity;
                    }
                    case 16:
                    {
                        return sk;
                    }
                    case 17:
                    {
                        return skVelocity;
                    }
                    default:
                    {
                        return Iteration;
                    }
                }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                switch(index)
                {
                    case 0:
                    {
                        if(value is long longValue)
                        {
                            Iteration = longValue;
                        }
                        else if(value is double newValue)
                        {
                            Iteration = (long)newValue;
                        }
                        break;
                    }
                    case 1:
                    {
                        if(value is long longValue)
                        {
                            SwarmIndex = longValue;
                        }
                        else if(value is double newValue)
                        {
                            SwarmIndex = (long)newValue;
                        }
                        break;
                    }
                    case 2:
                    {
                        if(value is long longValue)
                        {
                            ParticleIndex = longValue;
                        }
                        else if(value is double newValue)
                        {
                            ParticleIndex = (long)newValue;
                        }
                        break;
                    }
                    case 3:
                    {
                        if(value is double newValue)
                        {
                            Residual = newValue;
                        }
                        break;
                    }
                    case 4:
                    {
                        if(value is double newValue)
                        {
                            km = newValue;
                        }
                        break;
                    }
                    case 5:
                    {
                        if(value is double newValue)
                        {
                            kmVelocity = (long)newValue;
                        }
                        break;
                    }
                    case 6:
                    {
                        if(value is double newValue)
                        {
                            kF = newValue;
                        }
                        break;
                    }
                    case 7:
                    {
                        if(value is double newValue)
                        {
                            kFVelocity = newValue;
                        }
                        break;
                    }
                    case 8:
                    {
                        if(value is double newValue)
                        {
                            kf = newValue;
                        }
                        break;
                    }
                    case 9:
                    {
                        if(value is double newValue)
                        {
                            kfVelocity = newValue;
                        }
                        break;
                    }
                    case 10:
                    {
                        if(value is double newValue)
                        {
                            ye = newValue;
                        }
                        break;
                    }
                    case 11:
                    {
                        if(value is double newValue)
                        {
                            yeVelocity = newValue;
                        }
                        break;
                    }
                    case 12:
                    {
                        if(value is double newValue)
                        {
                            LF = newValue;
                        }
                        break;
                    }
                    case 13:
                    {
                        if(value is double newValue)
                        {
                            LFVelocity = newValue;
                        }
                        break;
                    }
                    case 14:
                    {
                        if(value is double newValue)
                        {
                            Lf = newValue;
                        }
                        break;
                    }
                    case 15:
                    {
                        if(value is double newValue)
                        {
                            LfVelocity = newValue;
                        }
                        break;
                    }
                    case 16:
                    {
                        if(value is double newValue)
                        {
                            sk = newValue;
                        }
                        break;
                    }
                    case 17:
                    {
                        if(value is double newValue)
                        {
                            skVelocity = newValue;
                        }
                        break;
                    }
                }
            }
        }
    }
}