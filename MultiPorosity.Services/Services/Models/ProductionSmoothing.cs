using System.Text.Json.Serialization;


namespace MultiPorosity.Services.Models
{
    public sealed class ProductionSmoothing
    {
        [JsonPropertyName(nameof(NumberOfPoints))]
        public int NumberOfPoints { get; set; }
        
        [JsonPropertyName(nameof(Iterations))]
        public int Iterations { get; set; }
        
        [JsonPropertyName(nameof(Normalized))]
        public bool Normalized { get; set; }

        public ProductionSmoothing()
        {
        }

        public ProductionSmoothing(int m)
        {
            NumberOfPoints = m;
            Iterations     = 3;
            Normalized     = false;
        }

        public ProductionSmoothing(int m,
                                   int k)
        {
            NumberOfPoints = m;
            Iterations     = k;
            Normalized     = false;
        }

        public ProductionSmoothing(int  m,
                                   int  k,
                                   bool normalized)
        {
            NumberOfPoints = m;
            Iterations     = k;
            Normalized     = normalized;
        }
        
        public static explicit operator ProductionSmoothing(MultiPorosity.Models.ProductionSmoothing productionSmoothing)
        {
            return new(productionSmoothing.NumberOfPoints, productionSmoothing.Iterations, productionSmoothing.Normalized);
        }
        
        public static implicit operator MultiPorosity.Models.ProductionSmoothing(ProductionSmoothing productionSmoothing)
        {
            return new(productionSmoothing.NumberOfPoints, productionSmoothing.Iterations, productionSmoothing.Normalized);
        }
    }
}