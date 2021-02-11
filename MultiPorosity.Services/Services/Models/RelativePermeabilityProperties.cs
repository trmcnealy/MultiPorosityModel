
using System.Text.Json.Serialization;

namespace MultiPorosity.Services.Models
{
    public sealed class RelativePermeabilityProperties
    {
        [JsonPropertyName(nameof(Matrix))]
        public RelativePermeabilityPropertyModel Matrix { get; set; }
        
        [JsonPropertyName(nameof(HydraulicFracture))]
        public RelativePermeabilityPropertyModel HydraulicFracture { get; set; }
        
        [JsonPropertyName(nameof(NaturalFracture))]
        public RelativePermeabilityPropertyModel NaturalFracture { get; set; }
        
        public RelativePermeabilityProperties()
        {
            Matrix            = new();
            HydraulicFracture = new();
            NaturalFracture   = new();
        }

        public RelativePermeabilityProperties(RelativePermeabilityPropertyModel matrix,
                                              RelativePermeabilityPropertyModel hydraulicFracture,
                                              RelativePermeabilityPropertyModel naturalFracture)
        {
            Matrix            = matrix;
            HydraulicFracture = hydraulicFracture;
            NaturalFracture   = naturalFracture;
        }
    }
    
    public sealed class RelativePermeabilityPropertyModel 
    {

        [JsonPropertyName(nameof(SaturationWaterConnate))]
        public double SaturationWaterConnate { get; set; }
        
        [JsonPropertyName(nameof(SaturationWaterCritical))]
        public double SaturationWaterCritical { get; set; }
        
        [JsonPropertyName(nameof(SaturationOilIrreducibleWater))]
        public double SaturationOilIrreducibleWater { get; set; }
        
        [JsonPropertyName(nameof(SaturationOilResidualWater))]
        public double SaturationOilResidualWater { get; set; }
        
        [JsonPropertyName(nameof(SaturationOilIrreducibleGas))]
        public double SaturationOilIrreducibleGas { get; set; }
        
        [JsonPropertyName(nameof(SaturationOilResidualGas))]
        public double SaturationOilResidualGas { get; set; }
        
        [JsonPropertyName(nameof(SaturationGasConnate))]
        public double SaturationGasConnate { get; set; }
        
        [JsonPropertyName(nameof(SaturationGasCritical))]
        public double SaturationGasCritical { get; set; }
        
        [JsonPropertyName(nameof(PermeabilityRelativeWaterOilIrreducible))]
        public double PermeabilityRelativeWaterOilIrreducible { get; set; }
        
        [JsonPropertyName(nameof(PermeabilityRelativeOilWaterConnate))]
        public double PermeabilityRelativeOilWaterConnate { get; set; }
        
        [JsonPropertyName(nameof(PermeabilityRelativeGasLiquidConnate))]
        public double PermeabilityRelativeGasLiquidConnate { get; set; }
        
        [JsonPropertyName(nameof(ExponentPermeabilityRelativeWater))]
        public double ExponentPermeabilityRelativeWater { get; set; }
        
        [JsonPropertyName(nameof(ExponentPermeabilityRelativeOilWater))]
        public double ExponentPermeabilityRelativeOilWater { get; set; }
        
        [JsonPropertyName(nameof(ExponentPermeabilityRelativeGas))]
        public double ExponentPermeabilityRelativeGas { get; set; }
        
        [JsonPropertyName(nameof(ExponentPermeabilityRelativeOilGas))]
        public double ExponentPermeabilityRelativeOilGas { get; set; }

        public RelativePermeabilityPropertyModel()
        {
        }

        public RelativePermeabilityPropertyModel(double saturationWaterConnate,
                                                 double saturationWaterCritical,
                                                 double saturationOilIrreducibleWater,
                                                 double saturationOilResidualWater,
                                                 double saturationOilIrreducibleGas,
                                                 double saturationOilResidualGas,
                                                 double saturationGasConnate,
                                                 double saturationGasCritical,
                                                 double permeabilityRelativeWaterOilIrreducible,
                                                 double permeabilityRelativeOilWaterConnate,
                                                 double permeabilityRelativeGasLiquidConnate,
                                                 double exponentPermeabilityRelativeWater,
                                                 double exponentPermeabilityRelativeOilWater,
                                                 double exponentPermeabilityRelativeGas,
                                                 double exponentPermeabilityRelativeOilGas)
        {
            SaturationWaterConnate                  = saturationWaterConnate;
            SaturationWaterCritical                 = saturationWaterCritical;
            SaturationOilIrreducibleWater           = saturationOilIrreducibleWater;
            SaturationOilResidualWater              = saturationOilResidualWater;
            SaturationOilIrreducibleGas             = saturationOilIrreducibleGas;
            SaturationOilResidualGas                = saturationOilResidualGas;
            SaturationGasConnate                    = saturationGasConnate;
            SaturationGasCritical                   = saturationGasCritical;
            PermeabilityRelativeWaterOilIrreducible = permeabilityRelativeWaterOilIrreducible;
            PermeabilityRelativeOilWaterConnate     = permeabilityRelativeOilWaterConnate;
            PermeabilityRelativeGasLiquidConnate    = permeabilityRelativeGasLiquidConnate;
            ExponentPermeabilityRelativeWater       = exponentPermeabilityRelativeWater;
            ExponentPermeabilityRelativeOilWater    = exponentPermeabilityRelativeOilWater;
            ExponentPermeabilityRelativeGas         = exponentPermeabilityRelativeGas;
            ExponentPermeabilityRelativeOilGas      = exponentPermeabilityRelativeOilGas;
        }
    }
}
