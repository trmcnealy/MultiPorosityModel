
using System.Text.Json.Serialization;

namespace MultiPorosity.Services.Models
{
    public sealed class RelativePermeabilities 
    {
        [JsonPropertyName(nameof(MatrixOil))]
        public double MatrixOil { get; set; }
        
        [JsonPropertyName(nameof(MatrixWater))]
        public double MatrixWater { get; set; }
        
        [JsonPropertyName(nameof(MatrixGas))]
        public double MatrixGas { get; set; }
        
        [JsonPropertyName(nameof(FractureOil))]
        public double FractureOil { get; set; }
        
        [JsonPropertyName(nameof(FractureWater))]
        public double FractureWater { get; set; }
        
        [JsonPropertyName(nameof(FractureGas))]
        public double FractureGas { get; set; }
        
        [JsonPropertyName(nameof(NaturalFractureOil))]
        public double NaturalFractureOil { get; set; }
        
        [JsonPropertyName(nameof(NaturalFractureWater))]
        public double NaturalFractureWater { get; set; }
        
        [JsonPropertyName(nameof(NaturalFractureGas))]
        public double NaturalFractureGas { get; set; }

        public RelativePermeabilities()
        {
        }

        public RelativePermeabilities(double matrixOil,
                                      double matrixWater,
                                      double matrixGas,
                                      double fractureOil,
                                      double fractureWater,
                                      double fractureGas,
                                      double naturalFractureOil,
                                      double naturalFractureWater,
                                      double naturalFractureGas)
        {
            MatrixOil            = matrixOil;
            MatrixWater          = matrixWater;
            MatrixGas            = matrixGas;
            FractureOil          = fractureOil;
            FractureWater        = fractureWater;
            FractureGas          = fractureGas;
            NaturalFractureOil   = naturalFractureOil;
            NaturalFractureWater = naturalFractureWater;
            NaturalFractureGas   = naturalFractureGas;
        }
    }
}