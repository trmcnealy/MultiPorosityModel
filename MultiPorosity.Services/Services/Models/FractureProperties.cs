
using System.Text.Json.Serialization;

using Engineering.DataSource;

namespace MultiPorosity.Services.Models
{    
    public sealed class FractureProperties 
    {
        [JsonPropertyName(nameof(Count))]
        public int Count { get; set; }
        
        [JsonPropertyName(nameof(Width))]
        public double Width { get; set; }
        
        [JsonPropertyName(nameof(Height))]
        public double Height { get; set; }
        
        [JsonPropertyName(nameof(HalfLength))]
        public double HalfLength { get; set; }
        
        [JsonPropertyName(nameof(Porosity))]
        public double Porosity { get; set; }
        
        [JsonPropertyName(nameof(Permeability))]
        public double Permeability { get; set; }
        
        [JsonPropertyName(nameof(Skin))]
        public double Skin { get; set; }

        public FractureProperties()
        {
        }

        public FractureProperties(int    count,
                                  double width,
                                  double height,
                                  double halfLength,
                                  double porosity,
                                  double permeability,
                                  double skin)
        {
            Count        = count;
            Width        = width;
            Height       = height;
            HalfLength   = halfLength;
            Porosity     = porosity;
            Permeability = permeability;
            Skin         = skin;
        }

        public FractureProperties(FractureProperties? fractureProperties)
        {
            Throw.IfNull(fractureProperties);

            Count        = fractureProperties.Count;
            Width        = fractureProperties.Width;
            Height       = fractureProperties.Height;
            HalfLength   = fractureProperties.HalfLength;
            Porosity     = fractureProperties.Porosity;
            Permeability = fractureProperties.Permeability;
            Skin         = fractureProperties.Skin;
        }
    }
}
