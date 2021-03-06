﻿
using System.Text.Json.Serialization;

using Engineering.DataSource;

namespace MultiPorosity.Services.Models
{
    public sealed class NaturalFractureProperties 
    {        
        [JsonPropertyName(nameof(Count))]
        public int Count { get; set; }
        
        [JsonPropertyName(nameof(Width))]
        public double Width { get; set; }
        
        [JsonPropertyName(nameof(Porosity))]
        public double Porosity { get; set; }
        
        [JsonPropertyName(nameof(Permeability))]
        public double Permeability { get; set; }

        public NaturalFractureProperties()
        {
        }

        public NaturalFractureProperties(int    count,
                                         double width,
                                         double porosity,
                                         double permeability)
        {
            Count        = count;
            Width        = width;
            Porosity     = porosity;
            Permeability = permeability;
        }

        public NaturalFractureProperties(NaturalFractureProperties? naturalFractureProperties)
        {
            Throw.IfNull(naturalFractureProperties);

            Count        = naturalFractureProperties.Count;
            Width        = naturalFractureProperties.Width;
            Porosity     = naturalFractureProperties.Porosity;
            Permeability = naturalFractureProperties.Permeability;
        }
    }
}
