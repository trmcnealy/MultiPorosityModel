
using System.Text.Json.Serialization;

namespace MultiPorosity.Services.Models
{
    public sealed class Pvt 
    {
        [JsonPropertyName(nameof(GasSaturation))]
        public double GasSaturation { get; set; }

        [JsonPropertyName(nameof(GasSpecificGravity))]
        public double GasSpecificGravity { get; set; }

        [JsonPropertyName(nameof(GasViscosity))]
        public double GasViscosity { get; set; }

        [JsonPropertyName(nameof(GasFormationVolumeFactor))]
        public double GasFormationVolumeFactor { get; set; }

        [JsonPropertyName(nameof(GasCompressibilityFactor))]
        public double GasCompressibilityFactor { get; set; }

        [JsonPropertyName(nameof(GasCompressibility))]
        public double GasCompressibility { get; set; }

        [JsonPropertyName(nameof(OilSaturation))]
        public double OilSaturation { get; set; }

        [JsonPropertyName(nameof(OilApiGravity))]
        public double OilApiGravity { get; set; }

        [JsonPropertyName(nameof(OilViscosity))]
        public double OilViscosity { get; set; }

        [JsonPropertyName(nameof(OilFormationVolumeFactor))]
        public double OilFormationVolumeFactor { get; set; }

        [JsonPropertyName(nameof(OilCompressibility))]
        public double OilCompressibility { get; set; }

        [JsonPropertyName(nameof(WaterSaturation))]
        public double WaterSaturation { get; set; }

        [JsonPropertyName(nameof(WaterSpecificGravity))]
        public double WaterSpecificGravity { get; set; }

        [JsonPropertyName(nameof(WaterViscosity))]
        public double WaterViscosity { get; set; }

        [JsonPropertyName(nameof(WaterFormationVolumeFactor))]
        public double WaterFormationVolumeFactor { get; set; }

        [JsonPropertyName(nameof(WaterCompressibility))]
        public double WaterCompressibility { get; set; }

        public Pvt()
        {
        }

        public Pvt(double gasSaturation,
                   double gasSpecificGravity,
                   double gasViscosity,
                   double gasFormationVolumeFactor,
                   double gasCompressibilityFactor,
                   double gasCompressibility,
                   double oilSaturation,
                   double oilApiGravity,
                   double oilViscosity,
                   double oilFormationVolumeFactor,
                   double oilCompressibility,
                   double waterSaturation,
                   double waterSpecificGravity,
                   double waterViscosity,
                   double waterFormationVolumeFactor,
                   double waterCompressibility)
        {
            GasSaturation              = gasSaturation;
            GasSpecificGravity         = gasSpecificGravity;
            GasViscosity               = gasViscosity;
            GasFormationVolumeFactor   = gasFormationVolumeFactor;
            GasCompressibilityFactor   = gasCompressibilityFactor;
            GasCompressibility         = gasCompressibility;
            OilSaturation              = oilSaturation;
            OilApiGravity              = oilApiGravity;
            OilViscosity               = oilViscosity;
            OilFormationVolumeFactor   = oilFormationVolumeFactor;
            OilCompressibility         = oilCompressibility;
            WaterSaturation            = waterSaturation;
            WaterSpecificGravity       = waterSpecificGravity;
            WaterViscosity             = waterViscosity;
            WaterFormationVolumeFactor = waterFormationVolumeFactor;
            WaterCompressibility       = waterCompressibility;
        }
    }
}