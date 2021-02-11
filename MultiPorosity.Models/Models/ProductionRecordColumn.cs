using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MultiPorosity.Models
{
    public sealed class ProductionRecordColumn
    {
        private readonly int                _columnIndex;
        private readonly ProductionRecord[] _productionRecords;

        public string Type { get; init; }

        public ProductionRecordColumn(int                columnIndex,
                                      ProductionRecord[] productionRecords)
        {
            _columnIndex       = columnIndex;
            _productionRecords = productionRecords;

            PropertyInfo[] properties = typeof(ProductionRecord).GetProperties();

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
            return _productionRecords[index].GetDate();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref float GetFloat(int index)
        {
            return ref _productionRecords[index].GetFloat(_columnIndex);
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

                return _productionRecords[index][_columnIndex];
            }
        }

        public object[] ToArray()
        {
            object[] array = new object[_productionRecords.Length];

            for(int i = 0; i < _productionRecords.Length; ++i)
            {
                array[i] = this[i];
            }

            return array;
        }
    }
}