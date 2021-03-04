
using System.Text.Json.Serialization;

using Engineering.DataSource;

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

        public ReservoirProperties(ReservoirProperties? reservoirProperties)
        {
            Throw.IfNull(reservoirProperties);

            Length                = reservoirProperties.Length;
            Width                 = reservoirProperties.Width;
            Thickness             = reservoirProperties.Thickness;
            Porosity              = reservoirProperties.Porosity;
            Permeability          = reservoirProperties.Permeability;
            Compressibility       = reservoirProperties.Compressibility;
            BottomholeTemperature = reservoirProperties.BottomholeTemperature;
            InitialPressure       = reservoirProperties.InitialPressure;
        }
    }
}
