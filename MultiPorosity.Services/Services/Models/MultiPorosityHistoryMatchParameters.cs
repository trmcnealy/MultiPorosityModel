using System.Text.Json.Serialization;
using Engineering.DataSource;

namespace MultiPorosity.Services.Models
{
    public class MultiPorosityHistoryMatchParameters
    {
        [JsonPropertyName(nameof(MatrixPermeability))]
        public Range<double> MatrixPermeability { get; set; }

        [JsonPropertyName(nameof(HydraulicFracturePermeability))]
        public Range<double> HydraulicFracturePermeability { get; set; }
        
        [JsonPropertyName(nameof(NaturalFracturePermeability))]
        public Range<double> NaturalFracturePermeability { get; set; }

        [JsonPropertyName(nameof(HydraulicFractureHalfLength))]
        public Range<double> HydraulicFractureHalfLength { get; set; }

        [JsonPropertyName(nameof(HydraulicFractureSpacing))]
        public Range<double> HydraulicFractureSpacing { get; set; }

        [JsonPropertyName(nameof(NaturalFractureSpacing))]
        public Range<double> NaturalFractureSpacing { get; set; }

        [JsonPropertyName(nameof(Skin))]
        public Range<double> Skin { get; set; }

        public MultiPorosityHistoryMatchParameters()
        {
            MatrixPermeability           = new ();
            HydraulicFracturePermeability = new ();
            NaturalFracturePermeability  = new ();
            HydraulicFractureHalfLength   = new ();
            HydraulicFractureSpacing      = new ();
            NaturalFractureSpacing       = new();
            Skin                         = new();
        }

        public MultiPorosityHistoryMatchParameters(Range<double> matrixPermeability,
                                                   Range<double> hydraulicFracturePermeability,
                                                   Range<double> naturalFracturePermeability,
                                                   Range<double> hydraulicFractureHalfLength,
                                                   Range<double> hydraulicFractureSpacing,
                                                   Range<double> naturalFractureSpacing,
                                                   Range<double> skin)
        {
            MatrixPermeability            = matrixPermeability;
            HydraulicFracturePermeability = hydraulicFracturePermeability;
            NaturalFracturePermeability   = naturalFracturePermeability;
            HydraulicFractureHalfLength   = hydraulicFractureHalfLength;
            HydraulicFractureSpacing      = hydraulicFractureSpacing;
            NaturalFractureSpacing        = naturalFractureSpacing;
            Skin                          = skin;
        }

        public MultiPorosityHistoryMatchParameters(MultiPorosityHistoryMatchParameters? multiPorosityHistoryMatchParameters)
        {
            Throw.IfNull(multiPorosityHistoryMatchParameters);

            MatrixPermeability            = new (multiPorosityHistoryMatchParameters.MatrixPermeability);
            HydraulicFracturePermeability = new (multiPorosityHistoryMatchParameters.HydraulicFracturePermeability);
            NaturalFracturePermeability   = new (multiPorosityHistoryMatchParameters.NaturalFracturePermeability);
            HydraulicFractureHalfLength   = new (multiPorosityHistoryMatchParameters.HydraulicFractureHalfLength);
            HydraulicFractureSpacing      = new (multiPorosityHistoryMatchParameters.HydraulicFractureSpacing);
            NaturalFractureSpacing        = new (multiPorosityHistoryMatchParameters.NaturalFractureSpacing);
            Skin                          = new (multiPorosityHistoryMatchParameters.Skin);
        }
        
        ///*km*/
        ////arg_limits[0] = new BoundConstraints<double>(0.0001, 0.01);
        //MatrixPermeabilityLower = 0.0001;
        //MatrixPermeabilityUpper = 0.01;

        ///*kF*/
        ////arg_limits[1] = new BoundConstraints<double>(100.0, 10000.0);
        //HydraulicFracturePermeabilityLower = 100.0;
        //HydraulicFracturePermeabilityUpper = 10000.0;

        ///*kf*/
        ////arg_limits[2] = new BoundConstraints<double>(0.01, 100.0);
        //NaturalFracturePermeabilityLower = 0.01;
        //NaturalFracturePermeabilityUpper = 100.0;

        ///*ye*/
        ////arg_limits[3] = new BoundConstraints<double>(1.0, 500.0);
        //HydraulicFractureHalfLengthLower = 1.0;
        //HydraulicFractureHalfLengthUpper = 500.0;

        ///*LF*/
        ////arg_limits[4] = new BoundConstraints<double>(50.0, 250.0);
        //HydraulicFractureSpacingLower = 50.0;
        //HydraulicFractureSpacingUpper = 250.0;

        ///*Lf*/
        //NaturalFractureSpacingLower = 10.0;
        //NaturalFractureSpacingUpper = 150.0;
        ////arg_limits[5] = new BoundConstraints<double>(10.0, 150.0);

        ///*sk*/
        ////arg_limits[6] = new BoundConstraints<double>(0.0, 0.0);
    }
}
