using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace MultiPorosity.Models
{
    public sealed class MultiPorosityModelProductionColumn
    {
        private readonly int                            _columnIndex;
        private readonly MultiPorosityModelProduction[] _multiPorosityModelProductions;

        public string Type { get; init; }

        public MultiPorosityModelProductionColumn(int                            columnIndex,
                                                  MultiPorosityModelProduction[] multiPorosityModelProductions)
        {
            _columnIndex                   = columnIndex;
            _multiPorosityModelProductions = multiPorosityModelProductions;

            PropertyInfo[] properties = typeof(MultiPorosityModelProduction).GetProperties();

            Type = properties[_columnIndex].PropertyType.Name;
        }

        public double this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _multiPorosityModelProductions[index][_columnIndex];
            }
        }

        public object[] ToArray()
        {
            object[] array = new object[_multiPorosityModelProductions.Length];

            for(int i = 0; i < _multiPorosityModelProductions.Length; ++i)
            {
                array[i] = this[i];
            }

            return array;
        }
    }
}