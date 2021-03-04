using System.Text.Json.Serialization;

using Engineering.DataSource;

using PVT;

namespace MultiPorosity.Services.Models
{
    public class PvtModelProperties
    {
        [JsonPropertyName(nameof(SeparatorPressure))]
        public double SeparatorPressure { get; set; }
        
        [JsonPropertyName(nameof(SeparatorTemperature))]
        public double SeparatorTemperature { get; set; }
        
        [JsonPropertyName(nameof(WaterSalinity))]
        public double WaterSalinity { get; set; }
        
        [JsonPropertyName(nameof(GasViscosityType))]
        public GasViscosityType GasViscosityType { get; set; }

        [JsonPropertyName(nameof(GasFormationVolumeFactorType))]
        public GasFormationVolumeFactorType GasFormationVolumeFactorType { get; set; }

        [JsonPropertyName(nameof(GasCompressibilityFactorType))]
        public GasCompressibilityFactorType GasCompressibilityFactorType { get; set; }

        [JsonPropertyName(nameof(GasPseudoCriticalType))]
        public GasPseudoCriticalType GasPseudoCriticalType { get; set; }

        [JsonPropertyName(nameof(GasCompressibilityType))]
        public GasCompressibilityType GasCompressibilityType { get; set; }

        [JsonPropertyName(nameof(OilSolutionGasType))]
        public OilSolutionGasType OilSolutionGasType { get; set; }

        [JsonPropertyName(nameof(OilBubblePointType))]
        public OilBubblePointType OilBubblePointType { get; set; }

        [JsonPropertyName(nameof(DeadOilViscosityType))]
        public DeadOilViscosityType DeadOilViscosityType { get; set; }

        [JsonPropertyName(nameof(SaturatedOilViscosityType))]
        public SaturatedOilViscosityType SaturatedOilViscosityType { get; set; }

        [JsonPropertyName(nameof(UnderSaturatedOilViscosityType))]
        public UnderSaturatedOilViscosityType UnderSaturatedOilViscosityType { get; set; }

        [JsonPropertyName(nameof(OilFormationVolumeFactorType))]
        public OilFormationVolumeFactorType OilFormationVolumeFactorType { get; set; }

        [JsonPropertyName(nameof(OilCompressibilityType))]
        public OilCompressibilityType OilCompressibilityType { get; set; }

        [JsonPropertyName(nameof(WaterViscosityType))]
        public WaterViscosityType WaterViscosityType { get; set; }

        [JsonPropertyName(nameof(WaterFormationVolumeFactorType))]
        public WaterFormationVolumeFactorType WaterFormationVolumeFactorType { get; set; }

        [JsonPropertyName(nameof(WaterCompressibilityType))]
        public WaterCompressibilityType WaterCompressibilityType { get; set; }

        public PvtModelProperties()
        {

        }

        public PvtModelProperties(double                         separatorPressure,
                                  double                         separatorTemperature,
                                  double                         waterSalinity,
                                  GasViscosityType               gasViscosityType,
                                  GasFormationVolumeFactorType   gasFormationVolumeFactorType,
                                  GasCompressibilityFactorType   gasCompressibilityFactorType,
                                  GasPseudoCriticalType          gasPseudoCriticalType,
                                  GasCompressibilityType         gasCompressibilityType,
                                  OilSolutionGasType             oilSolutionGasType,
                                  OilBubblePointType             oilBubblePointType,
                                  DeadOilViscosityType           deadOilViscosityType,
                                  SaturatedOilViscosityType      saturatedOilViscosityType,
                                  UnderSaturatedOilViscosityType underSaturatedOilViscosityType,
                                  OilFormationVolumeFactorType   oilFormationVolumeFactorType,
                                  OilCompressibilityType         oilCompressibilityType,
                                  WaterViscosityType             waterViscosityType,
                                  WaterFormationVolumeFactorType waterFormationVolumeFactorType,
                                  WaterCompressibilityType       waterCompressibilityType)
        {
            SeparatorPressure              = separatorPressure;
            SeparatorTemperature           = separatorTemperature;
            WaterSalinity                  = waterSalinity;
            GasViscosityType               = gasViscosityType;
            GasFormationVolumeFactorType   = gasFormationVolumeFactorType;
            GasCompressibilityFactorType   = gasCompressibilityFactorType;
            GasPseudoCriticalType          = gasPseudoCriticalType;
            GasCompressibilityType         = gasCompressibilityType;
            OilSolutionGasType             = oilSolutionGasType;
            OilBubblePointType             = oilBubblePointType;
            DeadOilViscosityType           = deadOilViscosityType;
            SaturatedOilViscosityType      = saturatedOilViscosityType;
            UnderSaturatedOilViscosityType = underSaturatedOilViscosityType;
            OilFormationVolumeFactorType   = oilFormationVolumeFactorType;
            OilCompressibilityType         = oilCompressibilityType;
            WaterViscosityType             = waterViscosityType;
            WaterFormationVolumeFactorType = waterFormationVolumeFactorType;
            WaterCompressibilityType       = waterCompressibilityType;
        }

        public PvtModelProperties(PvtModelProperties? pvtModelProperties)
        {
            Throw.IfNull(pvtModelProperties);

            SeparatorPressure              = pvtModelProperties.SeparatorPressure;
            SeparatorTemperature           = pvtModelProperties.SeparatorTemperature;
            WaterSalinity                  = pvtModelProperties.WaterSalinity;
            GasViscosityType               = pvtModelProperties.GasViscosityType;
            GasFormationVolumeFactorType   = pvtModelProperties.GasFormationVolumeFactorType;
            GasCompressibilityFactorType   = pvtModelProperties.GasCompressibilityFactorType;
            GasPseudoCriticalType          = pvtModelProperties.GasPseudoCriticalType;
            GasCompressibilityType         = pvtModelProperties.GasCompressibilityType;
            OilSolutionGasType             = pvtModelProperties.OilSolutionGasType;
            OilBubblePointType             = pvtModelProperties.OilBubblePointType;
            DeadOilViscosityType           = pvtModelProperties.DeadOilViscosityType;
            SaturatedOilViscosityType      = pvtModelProperties.SaturatedOilViscosityType;
            UnderSaturatedOilViscosityType = pvtModelProperties.UnderSaturatedOilViscosityType;
            OilFormationVolumeFactorType   = pvtModelProperties.OilFormationVolumeFactorType;
            OilCompressibilityType         = pvtModelProperties.OilCompressibilityType;
            WaterViscosityType             = pvtModelProperties.WaterViscosityType;
            WaterFormationVolumeFactorType = pvtModelProperties.WaterFormationVolumeFactorType;
            WaterCompressibilityType       = pvtModelProperties.WaterCompressibilityType;
        }
    }
}
