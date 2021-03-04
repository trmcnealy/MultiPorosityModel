using System.Text.Json.Serialization;

using Engineering.DataSource;

namespace MultiPorosity.Services.Models
{
    public class MultiPorosityModelParameters
    {
        [JsonPropertyName(nameof(Days))]
        public double Days { get; set; }
        
        [JsonPropertyName(nameof(MatrixPermeability))]
        public double MatrixPermeability { get; set; }

        [JsonPropertyName(nameof(HydraulicFracturePermeability))]
        public double HydraulicFracturePermeability { get; set; }
        
        [JsonPropertyName(nameof(NaturalFracturePermeability))]
        public double NaturalFracturePermeability { get; set; }

        [JsonPropertyName(nameof(HydraulicFractureHalfLength))]
        public double HydraulicFractureHalfLength { get; set; }

        [JsonPropertyName(nameof(HydraulicFractureSpacing))]
        public double HydraulicFractureSpacing { get; set; }

        [JsonPropertyName(nameof(NaturalFractureSpacing))]
        public double NaturalFractureSpacing { get; set; }

        [JsonPropertyName(nameof(Skin))]
        public double Skin { get; set; }

        public MultiPorosityModelParameters()
        {
            Days                         = 0.0;
            MatrixPermeability           = 0.0;
            HydraulicFracturePermeability = 0.0;
            NaturalFracturePermeability  = 0.0;
            HydraulicFractureHalfLength   = 0.0;
            HydraulicFractureSpacing      = 0.0;
            NaturalFractureSpacing       = 0.0;
            Skin                         = 0.0;
        }

        public MultiPorosityModelParameters(double days,
                                            double matrixPermeability,
                                            double hydraulicFracturePermeability,
                                            double naturalFracturePermeability,
                                            double hydraulicFractureHalfLength,
                                            double hydraulicFractureSpacing,
                                            double naturalFractureSpacing,
                                            double skin)
        {
            Days                          = days;
            MatrixPermeability            = matrixPermeability;
            HydraulicFracturePermeability = hydraulicFracturePermeability;
            NaturalFracturePermeability   = naturalFracturePermeability;
            HydraulicFractureHalfLength   = hydraulicFractureHalfLength;
            HydraulicFractureSpacing      = hydraulicFractureSpacing;
            NaturalFractureSpacing        = naturalFractureSpacing;
            Skin                          = skin;
        }

        public MultiPorosityModelParameters(MultiPorosityModelParameters? multiPorosityModelParameters)
        {
            Throw.IfNull(multiPorosityModelParameters);

            Days                          = multiPorosityModelParameters.Days;
            MatrixPermeability            = multiPorosityModelParameters.MatrixPermeability;
            HydraulicFracturePermeability = multiPorosityModelParameters.HydraulicFracturePermeability;
            NaturalFracturePermeability   = multiPorosityModelParameters.NaturalFracturePermeability;
            HydraulicFractureHalfLength   = multiPorosityModelParameters.HydraulicFractureHalfLength;
            HydraulicFractureSpacing      = multiPorosityModelParameters.HydraulicFractureSpacing;
            NaturalFractureSpacing        = multiPorosityModelParameters.NaturalFractureSpacing;
            Skin                          = multiPorosityModelParameters.Skin;
        }
    }
}
