using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace MultiPorosity.Services.Models
{
    public sealed class ProductionHistory
    {
        
        [JsonPropertyName(nameof(Index))]
        public int Index { get; set; }
        
        [JsonPropertyName(nameof(Date))]
        public DateTime Date { get; set; }
        
        [JsonPropertyName(nameof(Days))]
        public double Days { get; set; }
        
        [JsonPropertyName(nameof(Gas))]
        public double Gas { get; set; }
        
        [JsonPropertyName(nameof(Oil))]
        public double Oil { get; set; }
        
        [JsonPropertyName(nameof(Water))]
        public double Water { get; set; }
        
        [JsonPropertyName(nameof(WellheadPressure))]
        public double WellheadPressure { get; set; }
        
        [JsonPropertyName(nameof(Weight))]
        public double Weight { get; set; }

        public ProductionHistory(int      index,
                                 DateTime date,
                                 double   days,
                                 double   gas,
                                 double   oil,
                                 double   water,
                                 double   wellheadPressure,
                                 double   weight)
        {
            Index            = index;
            Date             = date;
            Days             = days;
            Gas              = gas;
            Oil              = oil;
            Water            = water;
            WellheadPressure = wellheadPressure;
            Weight           = weight;
        }
    }
}