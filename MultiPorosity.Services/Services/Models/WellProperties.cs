
using System.Text.Json.Serialization;

namespace MultiPorosity.Services.Models
{
    public sealed class WellProperties 
    {
        [JsonPropertyName(nameof(API))]
        public string API { get; set; }
        
        [JsonPropertyName(nameof(LateralLength))]
        public double LateralLength { get; set; }
        
        [JsonPropertyName(nameof(BottomholePressure))]
        public double BottomholePressure { get; set; }

        public WellProperties()
        {
            API = "##-###-#####-####";
        }

        public WellProperties(string api,
                              double lateralLength,
                              double bottomholePressure)
        {
            API                = api;
            LateralLength      = lateralLength;
            BottomholePressure = bottomholePressure;
        }
    }
}