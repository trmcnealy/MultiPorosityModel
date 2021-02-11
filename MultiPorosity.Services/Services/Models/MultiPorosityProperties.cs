
using System.Text.Json.Serialization;

using MultiPorosity.Models;

namespace MultiPorosity.Services.Models
{
    public sealed class MultiPorosityProperties
    {
    
        [JsonPropertyName(nameof(FractureProperties))]
        public FractureProperties FractureProperties { get; set; }

        [JsonPropertyName(nameof(NaturalFractureProperties))]
        public NaturalFractureProperties NaturalFractureProperties { get; set; }

        [JsonPropertyName(nameof(ReservoirProperties))]
        public ReservoirProperties ReservoirProperties { get; set; }

        [JsonPropertyName(nameof(WellProperties))]
        public WellProperties WellProperties { get; set; }
        
        [JsonPropertyName(nameof(Pvt))]
        public Pvt Pvt { get; set; }

        [JsonPropertyName(nameof(RelativePermeabilities))]
        public RelativePermeabilities RelativePermeabilities { get; set; }

        public MultiPorosityProperties()
        {
            FractureProperties        = new ();
            NaturalFractureProperties = new ();
            ReservoirProperties       = new ();
            WellProperties            = new ();
            Pvt                       = new ();
            RelativePermeabilities    = new ();
        }

        public MultiPorosityProperties(FractureProperties        fractureProperties,
                                       NaturalFractureProperties naturalFractureProperties,
                                       ReservoirProperties       reservoirProperties,
                                       WellProperties            wellProperties,
                                       Pvt                       pvt,
                                       RelativePermeabilities    relativePermeabilities)
        {
            FractureProperties        = fractureProperties;
            NaturalFractureProperties = naturalFractureProperties;
            ReservoirProperties       = reservoirProperties;
            WellProperties            = wellProperties;
            Pvt                       = pvt;
            RelativePermeabilities    = relativePermeabilities;
        }
        
        


        //// LateralLength = 6500.0;
        ////reservoir.Width                                    = 348.0;
        //ReservoirThickness = 50.0;
        //MatrixPorosity     = 0.06;
        //MatrixPermeability = 0.002;
        ////reservoir.Temperature                              = 275.0;
        ////reservoir.InitialPressure                          = 7000.0;

        //LateralLength = 6500.0;
        ////wellProperties.BottomholePressure = 3500.0;

        ////fracture.Count        = 60;
        //HydraulicFractureSpacing = LateralLength / 60.0;
        //HydraulicFractureWidth   = 0.1;
        ////fracture.Height       = 50.0;
        //HydraulicFractureHalfLength   = 348.0;
        //HydraulicFracturePorosity     = 0.20;
        //HydraulicFracturePermeability = 184.0;
        ////fracture.Skin                                                = 0.0;

        ////natural_fracture.Count                                       = 60;
        //NaturalFractureSpacing      = HydraulicFractureHalfLength / 60.0;
        //NaturalFractureWidth        = 0.01;
        //NaturalFracturePorosity     = 0.10;
        //NaturalFracturePermeability = 1.0;

        //ReservoirOilViscosity             = 0.5;
        //ReservoirOilFormationVolumeFactor = 1.5;
        //TotalCompressibility              = 0.00002;

        //InitialDeltaPressure = 7000.0 - 3500.0;
    }
}