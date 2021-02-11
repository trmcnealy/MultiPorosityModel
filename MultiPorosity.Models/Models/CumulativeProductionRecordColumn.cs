using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MultiPorosity.Models
{
    public sealed class CumulativeProductionRecordColumn
    {
        private readonly int                          _columnIndex;
        private readonly CumulativeProductionRecord[] _cumulativeProductionRecords;

        public string Type { get; init; }

        public CumulativeProductionRecordColumn(int                          columnIndex,
                                                CumulativeProductionRecord[] productionRecords)
        {
            _columnIndex       = columnIndex;
            _cumulativeProductionRecords = productionRecords;

            PropertyInfo[] properties = typeof(CumulativeProductionRecord).GetProperties();

            Type = properties[_columnIndex].PropertyType.Name;

            //foreach (PropertyInfo property in properties)
            //{
            //    property.SetValue(record, value);
            //}

            //2013-10-04 22:23:00
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetDate(int index)
        {
            return _cumulativeProductionRecords[index].GetDate();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref float GetFloat(int index)
        {
            return ref _cumulativeProductionRecords[index].GetFloat(_columnIndex);
        }

        public object this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if(_columnIndex == 1)
                {
                    return GetDate(index);
                }

                return _cumulativeProductionRecords[index][_columnIndex];
            }
        }

        public object[] ToArray()
        {
            object[] array = new object[_cumulativeProductionRecords.Length];

            for(int i = 0; i < _cumulativeProductionRecords.Length; ++i)
            {
                array[i] = this[i];
            }

            return array;
        }
    }
}