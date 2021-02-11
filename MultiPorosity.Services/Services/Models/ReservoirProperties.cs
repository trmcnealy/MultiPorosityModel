
using System.Text.Json.Serialization;

namespace MultiPorosity.Services.Models
{
    public sealed class ReservoirProperties 
    {
        [JsonPropertyName(nameof(Length))]
        public double Length { get; set; }
        
        [JsonPropertyName(nameof(Width))]
        public double Width { get; set; }
        
        [JsonPropertyName(nameof(Thickness))]
        public double Thickness { get; set; }
        
        [JsonPropertyName(nameof(Porosity))]
        public double Porosity { get; set; }
        
        [JsonPropertyName(nameof(Permeability))]
        public double Permeability { get; set; }
        
        [JsonPropertyName(nameof(Compressibility))]
        public double Compressibility { get; set; }
        
        [JsonPropertyName(nameof(BottomholeTemperature))]
        public double BottomholeTemperature { get; set; }
        
        [JsonPropertyName(nameof(InitialPressure))]
        public double InitialPressure { get; set; }

        public ReservoirProperties()
        {
        }

        public ReservoirProperties(double length,
                                   double width,
                                   double thickness,
                                   double porosity,
                                   double permeability,
                                   double compressibility,
                                   double bottomholeTemperature,
                                   double initialPressure)
        {
            Length                = length;
            Width                 = width;
            Thickness             = thickness;
            Porosity              = porosity;
            Permeability          = permeability;
            Compressibility       = compressibility;
            BottomholeTemperature = bottomholeTemperature;
            InitialPressure       = initialPressure;
        }
    }
}