using System.Collections.Generic;
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

        public static implicit operator MultiPorosity.Models.MultiPorosityModelProduction(MultiPorosityModelProduction multiPorosityModelProduction)
        {
            return new(multiPorosityModelProduction.Days, multiPorosityModelProduction.Gas, multiPorosityModelProduction.Oil, multiPorosityModelProduction.Water);
        }

        public static List<MultiPorosity.Models.MultiPorosityModelProduction> Convert(List<MultiPorosityModelProduction> multiPorosityModelProduction)
        {
            List<MultiPorosity.Models.MultiPorosityModelProduction> multiPorosityModelProductions = new(multiPorosityModelProduction.Count);

            for (int i = 0; i < multiPorosityModelProduction.Count; ++i)
            {
                multiPorosityModelProductions.Add(multiPorosityModelProduction[i]);
            }

            return multiPorosityModelProductions;
        }
    }
}