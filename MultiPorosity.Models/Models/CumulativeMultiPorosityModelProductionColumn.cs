using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace MultiPorosity.Models
{
    public sealed class CumulativeMultiPorosityModelProductionColumn
    {
        private readonly int                                      _columnIndex;
        private readonly CumulativeMultiPorosityModelProduction[] _cumulativeMultiPorosityModelProductions;

        public string Type { get; init; }

        public CumulativeMultiPorosityModelProductionColumn(int                                      columnIndex,
                                                            CumulativeMultiPorosityModelProduction[] cumulativeMultiPorosityModelProductions)
        {
            _columnIndex                   = columnIndex;
            _cumulativeMultiPorosityModelProductions = cumulativeMultiPorosityModelProductions;

            PropertyInfo[] properties = typeof(MultiPorosityModelProduction).GetProperties();

            Type = properties[_columnIndex].PropertyType.Name;
        }

        public double this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _cumulativeMultiPorosityModelProductions[index][_columnIndex];
            }
        }

        public object[] ToArray()
        {
            object[] array = new object[_cumulativeMultiPorosityModelProductions.Length];

            for(int i = 0; i < _cumulativeMultiPorosityModelProductions.Length; ++i)
            {
                array[i] = this[i];
            }

            return array;
        }
    }
}