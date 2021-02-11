using System.Text.Json.Serialization;

namespace MultiPorosity.Services.Models
{
    public sealed class MultiPorosityModelProduction
    {
        [JsonPropertyName(nameof(Days))]
        public double Days { get; set; }
        
        [JsonPropertyName(nameof(Gas))]
        public double Gas { get; set; }
        
        [JsonPropertyName(nameof(Oil))]
        public double Oil { get; set; }
        
        [JsonPropertyName(nameof(Water))]
        public double Water { get; set; }

        public MultiPorosityModelProduction(double days,
                                            double gas,
                                            double oil,
                                            double water)
        {
            Days  = days;
            Gas   = gas;
            Oil   = oil;
            Water = water;
        }
    }
}