using System.Collections.Generic;
using System.Text.Json.Serialization;

using Engineering.DataSource;

namespace MultiPorosity.Services.Models
{
    public class MultiPorosityModelResults
    {
        [JsonPropertyName(nameof(Production))]
        public List<MultiPorosityModelProduction> Production { get; set; }

        [JsonIgnore]
        public List<TriplePorosityOptimizationResults> TriplePorosityOptimizationResults { get; set; }
        
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

        public MultiPorosityModelResults()
        {
            Production                        = new();
            TriplePorosityOptimizationResults = new();
            MatrixPermeability                = 0.0;
            HydraulicFracturePermeability     = 0.0;
            NaturalFracturePermeability       = 0.0;
            HydraulicFractureHalfLength       = 0.0;
            HydraulicFractureSpacing          = 0.0;
            NaturalFractureSpacing            = 0.0;
            Skin                              = 0.0;
        }

        public MultiPorosityModelResults(List<MultiPorosityModelProduction>      production,
                                         List<TriplePorosityOptimizationResults> triplePorosityOptimizationResults,
                                         double                                  matrixPermeability,
                                         double                                  hydraulicFracturePermeability,
                                         double                                  naturalFracturePermeability,
                                         double                                  hydraulicFractureHalfLength,
                                         double                                  hydraulicFractureSpacing,
                                         double                                  naturalFractureSpacing,
                                         double                                  skin)
        {
            Production                        = production;
            TriplePorosityOptimizationResults = triplePorosityOptimizationResults;
            MatrixPermeability                = matrixPermeability;
            HydraulicFracturePermeability     = hydraulicFracturePermeability;
            NaturalFracturePermeability       = naturalFracturePermeability;
            HydraulicFractureHalfLength       = hydraulicFractureHalfLength;
            HydraulicFractureSpacing          = hydraulicFractureSpacing;
            NaturalFractureSpacing            = naturalFractureSpacing;
            Skin                              = skin;
        }

        public MultiPorosityModelResults(MultiPorosityModelResults? multiPorosityModelResults)
        {
            Throw.IfNull(multiPorosityModelResults);

            Production                        = multiPorosityModelResults.Production;
            TriplePorosityOptimizationResults = multiPorosityModelResults.TriplePorosityOptimizationResults;
            MatrixPermeability                = multiPorosityModelResults.MatrixPermeability;
            HydraulicFracturePermeability     = multiPorosityModelResults.HydraulicFracturePermeability;
            NaturalFracturePermeability       = multiPorosityModelResults.NaturalFracturePermeability;
            HydraulicFractureHalfLength       = multiPorosityModelResults.HydraulicFractureHalfLength;
            HydraulicFractureSpacing          = multiPorosityModelResults.HydraulicFractureSpacing;
            NaturalFractureSpacing            = multiPorosityModelResults.NaturalFractureSpacing;
            Skin                              = multiPorosityModelResults.Skin;
        }
    }
}
