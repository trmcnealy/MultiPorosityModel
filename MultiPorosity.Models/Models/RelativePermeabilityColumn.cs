using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MultiPorosity.Models
{
    public sealed class RelativePermeabilityColumn
    {
        private readonly int                         _columnIndex;
        private readonly RelativePermeabilityModel[] _relativePermeabilityModels;

        public string Type { get; init; }

        public RelativePermeabilityColumn(int                         columnIndex,
                                          RelativePermeabilityModel[] relativePermeabilityModels)
        {
            _columnIndex                = columnIndex;
            _relativePermeabilityModels = relativePermeabilityModels;

            PropertyInfo[] properties = typeof(RelativePermeabilityModel).GetProperties();

            Type = properties[_columnIndex].PropertyType.Name;
        }
        
        public object this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _relativePermeabilityModels[index][_columnIndex];
            }
        }

        public object[] ToArray()
        {
            object[] array = new object[_relativePermeabilityModels.Length];

            for(int i = 0; i < _relativePermeabilityModels.Length; ++i)
            {
                array[i] = this[i];
            }

            return array;
        }
    }
}