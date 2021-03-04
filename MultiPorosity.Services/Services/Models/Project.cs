using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using MultiPorosity.Models;

using Prism.Events;

using PVT;

namespace MultiPorosity.Services.Models
{
    public class ProjectIsBusyEvent : PubSubEvent<bool>
    {
    }

    public class ProjectChangedEvent : PubSubEvent<string?>
    {
    }
    
    public sealed class Project
    {
        [JsonPropertyName(nameof(Name))]
        public string Name { get; set; }
        
        [JsonPropertyName(nameof(ProductionHistory))]
        public List<ProductionHistory> ProductionHistory { get; set; }

        [JsonPropertyName(nameof(MultiPorosityProperties))]
        public MultiPorosityProperties MultiPorosityProperties { get; set; }

        [JsonPropertyName(nameof(PvtModelProperties))]
        public PvtModelProperties PvtModelProperties { get; set; }
        
        [JsonPropertyName(nameof(RelativePermeabilityProperties))]
        public RelativePermeabilityProperties RelativePermeabilityProperties { get; set; }
        
        [JsonPropertyName(nameof(MultiPorosityHistoryMatchParameters))]
        public MultiPorosityHistoryMatchParameters MultiPorosityHistoryMatchParameters { get; set; }
        
        [JsonPropertyName(nameof(MultiPorosityModelParameters))]
        public MultiPorosityModelParameters MultiPorosityModelParameters { get; set; }

        [JsonPropertyName(nameof(ParticleSwarmOptimizationOptions))]
        public ParticleSwarmOptimizationOptions ParticleSwarmOptimizationOptions { get; set; }

        [JsonPropertyName(nameof(MultiPorosityModelResults))]
        public MultiPorosityModelResults MultiPorosityModelResults { get; set; }

        [JsonPropertyName(nameof(PorosityModelKind))]
        public PorosityModelKind PorosityModelKind { get; set; }

        [JsonPropertyName(nameof(FlowType))]
        public FlowType FlowType { get; set; }

        [JsonPropertyName(nameof(SolutionType))]
        public SolutionType SolutionType { get; set; }

        [JsonPropertyName(nameof(InverseTransformPrecision))]
        public InverseTransformPrecision InverseTransformPrecision { get; set; }

        [JsonPropertyName(nameof(DatabaseDataSource))]
        public DatabaseDataSource? DatabaseDataSource { get; set; }

        [JsonPropertyName(nameof(ProductionSmoothing))]
        public ProductionSmoothing? ProductionSmoothing { get; set; }


        public Project()
        {
            Name                                = string.Empty;
            ProductionHistory                   = new();
            MultiPorosityProperties             = new();
            PvtModelProperties                  = new();
            RelativePermeabilityProperties      = new();
            MultiPorosityHistoryMatchParameters = new();
            MultiPorosityModelParameters        = new();
            ParticleSwarmOptimizationOptions    = new();
            MultiPorosityModelResults           = new();
            ProductionSmoothing                 = new();
            
            PorosityModelKind                   = PorosityModelKind.Triple;
            FlowType                            = FlowType.UnsteadyState;
            SolutionType                        = SolutionType.Linear;
            InverseTransformPrecision           = InverseTransformPrecision.High;
            
#if DEBUG
            DefaultSettings();
#endif
        }
        
        public Project(string name)
        {
            Name                                = name;
            ProductionHistory                   = new();
            MultiPorosityProperties             = new();
            PvtModelProperties                  = new();
            RelativePermeabilityProperties      = new();
            MultiPorosityHistoryMatchParameters = new();
            MultiPorosityModelParameters        = new();
            ParticleSwarmOptimizationOptions    = new();
            MultiPorosityModelResults           = new();
            DatabaseDataSource                  = new();
            ProductionSmoothing                 = new();
            
            PorosityModelKind                   = PorosityModelKind.Triple;
            FlowType                            = FlowType.UnsteadyState;
            SolutionType                        = SolutionType.Linear;
            InverseTransformPrecision           = InverseTransformPrecision.High;
            
#if DEBUG
            DefaultSettings();
#endif
        }

        public Project(string                              name,
                       List<ProductionHistory>             productionHistory,
                       MultiPorosityProperties             multiPorosityProperties,
                       PvtModelProperties                  pvtModelProperties,
                       RelativePermeabilityProperties      relativePermeabilityProperties,
                       MultiPorosityHistoryMatchParameters multiPorosityHistoryMatchParameters,
                       MultiPorosityModelParameters        multiPorosityModelParameters,
                       ParticleSwarmOptimizationOptions    particleSwarmOptimizationOptions,
                       MultiPorosityModelResults           multiPorosityModelResults,
                       DatabaseDataSource?                 databaseDataSource,
                       ProductionSmoothing                 productionSmoothing,
                       PorosityModelKind                   porosityModelKind         = PorosityModelKind.Triple,
                       FlowType                            flowType                  = FlowType.UnsteadyState,
                       SolutionType                        solutionType              = SolutionType.Linear,
                       InverseTransformPrecision           inverseTransformPrecision = InverseTransformPrecision.High)
        {
            Name                                = name;
            ProductionHistory                   = productionHistory;
            MultiPorosityProperties             = multiPorosityProperties;
            PvtModelProperties                  = pvtModelProperties;
            RelativePermeabilityProperties      = relativePermeabilityProperties;
            MultiPorosityHistoryMatchParameters = multiPorosityHistoryMatchParameters;
            MultiPorosityModelParameters        = multiPorosityModelParameters;
            ParticleSwarmOptimizationOptions    = particleSwarmOptimizationOptions;
            MultiPorosityModelResults           = multiPorosityModelResults;
            DatabaseDataSource                  = databaseDataSource;
            ProductionSmoothing                 = productionSmoothing;

            PorosityModelKind                   = porosityModelKind;
            FlowType                            = flowType;
            SolutionType                        = solutionType;
            InverseTransformPrecision           = inverseTransformPrecision;
        }

        public Project(Project project)
        {
            Name                                = project.Name;
            ProductionHistory                   = new(project.ProductionHistory);
            MultiPorosityProperties             = new(project.MultiPorosityProperties);
            PvtModelProperties                  = new(project.PvtModelProperties);
            RelativePermeabilityProperties      = new(project.RelativePermeabilityProperties);
            MultiPorosityHistoryMatchParameters = new(project.MultiPorosityHistoryMatchParameters);
            MultiPorosityModelParameters        = new(project.MultiPorosityModelParameters);
            ParticleSwarmOptimizationOptions    = new(project.ParticleSwarmOptimizationOptions);
            MultiPorosityModelResults           = new(project.MultiPorosityModelResults);
            PorosityModelKind                   = project.PorosityModelKind;
            FlowType                            = project.FlowType;
            SolutionType                        = project.SolutionType;
            InverseTransformPrecision           = project.InverseTransformPrecision;
            DatabaseDataSource                  = new(project.DatabaseDataSource);
            ProductionSmoothing                 = new(project.ProductionSmoothing);
        }

        private void DefaultSettings()
        {
            int index = 0;
            #region ProductionHistory
            ProductionHistory.Add(new ProductionHistory(index++, DateTime.Parse("11/1/2012"), 1.0, 270.7299, 476.1036, 0.0, 1500.0, 1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("12/1/2012"),
                                                        (DateTime.Parse("12/1/2012") - DateTime.Parse("11/1/2012")).Days,
                                                        190.127903225806,
                                                        303.849,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("1/1/2013"),
                                                        (DateTime.Parse("1/1/2013") - DateTime.Parse("11/1/2012")).Days,
                                                        164.749064516129,
                                                        282.59564516129,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("2/1/2013"),
                                                        (DateTime.Parse("2/1/2013") - DateTime.Parse("11/1/2012")).Days,
                                                        125.19675,
                                                        229.90275,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("3/1/2013"),
                                                        (DateTime.Parse("3/1/2013") - DateTime.Parse("11/1/2012")).Days,
                                                        177.836806451613,
                                                        263.390806451613,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("4/1/2013"),
                                                        (DateTime.Parse("4/1/2013") - DateTime.Parse("11/1/2012")).Days,
                                                        126.7434,
                                                        213.5322,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("5/1/2013"),
                                                        (DateTime.Parse("5/1/2013") - DateTime.Parse("11/1/2012")).Days,
                                                        122.07164516129,
                                                        173.611741935484,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("6/1/2013"),
                                                        (DateTime.Parse("6/1/2013") - DateTime.Parse("11/1/2012")).Days,
                                                        74.4408,
                                                        133.4466,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("7/1/2013"),
                                                        (DateTime.Parse("7/1/2013") - DateTime.Parse("11/1/2012")).Days,
                                                        68.5683870967742,
                                                        112.127806451613,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("8/1/2013"),
                                                        (DateTime.Parse("8/1/2013") - DateTime.Parse("11/1/2012")).Days,
                                                        75.2829677419355,
                                                        125.898387096774,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("9/1/2013"),
                                                        (DateTime.Parse("9/1/2013") - DateTime.Parse("11/1/2012")).Days,
                                                        63.0483,
                                                        110.2794,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("10/1/2013"),
                                                        (DateTime.Parse("10/1/2013") - DateTime.Parse("11/1/2012")).Days,
                                                        45.5794838709677,
                                                        90.1204838709677,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("11/1/2013"),
                                                        (DateTime.Parse("11/1/2013") - DateTime.Parse("11/1/2012")).Days,
                                                        43.6443,
                                                        84.9807,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("12/1/2013"),
                                                        (DateTime.Parse("12/1/2013") - DateTime.Parse("11/1/2012")).Days,
                                                        57.9986129032258,
                                                        105.356322580645,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("1/1/2014"),
                                                        (DateTime.Parse("1/1/2014") - DateTime.Parse("11/1/2012")).Days,
                                                        53.8162258064516,
                                                        86.9339032258064,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("2/1/2014"),
                                                        (DateTime.Parse("2/1/2014") - DateTime.Parse("11/1/2012")).Days,
                                                        59.39325,
                                                        94.20075,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("3/1/2014"),
                                                        (DateTime.Parse("3/1/2014") - DateTime.Parse("11/1/2012")).Days,
                                                        61.3132258064516,
                                                        88.3422580645161,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("4/1/2014"),
                                                        (DateTime.Parse("4/1/2014") - DateTime.Parse("11/1/2012")).Days,
                                                        52.9788,
                                                        81.8496,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("5/1/2014"),
                                                        (DateTime.Parse("5/1/2014") - DateTime.Parse("11/1/2012")).Days,
                                                        11.0392258064516,
                                                        21.1110967741935,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("6/1/2014"),
                                                        (DateTime.Parse("6/1/2014") - DateTime.Parse("11/1/2012")).Days,
                                                        18.6837,
                                                        36.0003,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("7/1/2014"),
                                                        (DateTime.Parse("7/1/2014") - DateTime.Parse("11/1/2012")).Days,
                                                        18.1379032258065,
                                                        38.0113548387097,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("8/1/2014"),
                                                        (DateTime.Parse("8/1/2014") - DateTime.Parse("11/1/2012")).Days,
                                                        31.4105806451613,
                                                        50.530064516129,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("9/1/2014"),
                                                        (DateTime.Parse("9/1/2014") - DateTime.Parse("11/1/2012")).Days,
                                                        20.5947,
                                                        43.4091,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("10/1/2014"),
                                                        (DateTime.Parse("10/1/2014") - DateTime.Parse("11/1/2012")).Days,
                                                        22.491,
                                                        61.74,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("11/1/2014"),
                                                        (DateTime.Parse("11/1/2014") - DateTime.Parse("11/1/2012")).Days,
                                                        22.7115,
                                                        56.2275,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("12/1/2014"),
                                                        (DateTime.Parse("12/1/2014") - DateTime.Parse("11/1/2012")).Days,
                                                        37.4423225806452,
                                                        57.4153548387097,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("1/1/2015"),
                                                        (DateTime.Parse("1/1/2015") - DateTime.Parse("11/1/2012")).Days,
                                                        15.4207741935484,
                                                        34.582935483871,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("2/1/2015"),
                                                        (DateTime.Parse("2/1/2015") - DateTime.Parse("11/1/2012")).Days,
                                                        9.18225,
                                                        43.92675,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("3/1/2015"),
                                                        (DateTime.Parse("3/1/2015") - DateTime.Parse("11/1/2012")).Days,
                                                        11.4517741935484,
                                                        27.4842580645161,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("4/1/2015"),
                                                        (DateTime.Parse("4/1/2015") - DateTime.Parse("11/1/2012")).Days,
                                                        9.0846,
                                                        26.0337,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("5/1/2015"),
                                                        (DateTime.Parse("5/1/2015") - DateTime.Parse("11/1/2012")).Days,
                                                        23.6575161290323,
                                                        45.679064516129,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("6/1/2015"),
                                                        (DateTime.Parse("6/1/2015") - DateTime.Parse("11/1/2012")).Days,
                                                        32.4135,
                                                        58.7118,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("7/1/2015"),
                                                        (DateTime.Parse("7/1/2015") - DateTime.Parse("11/1/2012")).Days,
                                                        25.4357419354839,
                                                        43.7443548387097,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("8/1/2015"),
                                                        (DateTime.Parse("8/1/2015") - DateTime.Parse("11/1/2012")).Days,
                                                        8.2651935483871,
                                                        17.6826774193548,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("9/1/2015"),
                                                        (DateTime.Parse("9/1/2015") - DateTime.Parse("11/1/2012")).Days,
                                                        4.1454,
                                                        8.5848,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("10/1/2015"),
                                                        (DateTime.Parse("10/1/2015") - DateTime.Parse("11/1/2012")).Days,
                                                        6.81416129032258,
                                                        15.0651290322581,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("11/1/2015"),
                                                        (DateTime.Parse("11/1/2015") - DateTime.Parse("11/1/2012")).Days,
                                                        6.174,
                                                        13.7739,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("12/1/2015"),
                                                        (DateTime.Parse("12/1/2015") - DateTime.Parse("11/1/2012")).Days,
                                                        7.58235483870968,
                                                        17.824935483871,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("1/1/2016"),
                                                        (DateTime.Parse("1/1/2016") - DateTime.Parse("11/1/2012")).Days,
                                                        7.8241935483871,
                                                        15.1220322580645,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("2/1/2016"),
                                                        (DateTime.Parse("2/1/2016") - DateTime.Parse("11/1/2012")).Days,
                                                        4.82058620689655,
                                                        17.2142068965517,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("3/1/2016"),
                                                        (DateTime.Parse("3/1/2016") - DateTime.Parse("11/1/2012")).Days,
                                                        1.26609677419355,
                                                        12.7036451612903,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("4/1/2016"),
                                                        (DateTime.Parse("4/1/2016") - DateTime.Parse("11/1/2012")).Days,
                                                        1.6464,
                                                        4.3512,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("5/1/2016"),
                                                        (DateTime.Parse("5/1/2016") - DateTime.Parse("11/1/2012")).Days,
                                                        11.0676774193548,
                                                        26.3319677419355,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("6/1/2016"),
                                                        (DateTime.Parse("6/1/2016") - DateTime.Parse("11/1/2012")).Days,
                                                        7.8204,
                                                        22.2558,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("7/1/2016"),
                                                        (DateTime.Parse("7/1/2016") - DateTime.Parse("11/1/2012")).Days,
                                                        9.41748387096774,
                                                        23.188064516129,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("8/1/2016"),
                                                        (DateTime.Parse("8/1/2016") - DateTime.Parse("11/1/2012")).Days,
                                                        7.07022580645161,
                                                        18.0240967741935,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("9/1/2016"),
                                                        (DateTime.Parse("9/1/2016") - DateTime.Parse("11/1/2012")).Days,
                                                        9.8784,
                                                        18.081,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("10/1/2016"),
                                                        (DateTime.Parse("10/1/2016") - DateTime.Parse("11/1/2012")).Days,
                                                        13.927064516129,
                                                        22.1922580645161,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("11/1/2016"),
                                                        (DateTime.Parse("11/1/2016") - DateTime.Parse("11/1/2012")).Days,
                                                        4.9833,
                                                        8.5701,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("12/1/2016"),
                                                        (DateTime.Parse("12/1/2016") - DateTime.Parse("11/1/2012")).Days,
                                                        0.01,
                                                        0.01,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("1/1/2017"),
                                                        (DateTime.Parse("1/1/2017") - DateTime.Parse("11/1/2012")).Days,
                                                        12.1346129032258,
                                                        22.2633870967742,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("2/1/2017"),
                                                        (DateTime.Parse("2/1/2017") - DateTime.Parse("11/1/2012")).Days,
                                                        13.3245,
                                                        27.57825,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("3/1/2017"),
                                                        (DateTime.Parse("3/1/2017") - DateTime.Parse("11/1/2012")).Days,
                                                        11.6224838709677,
                                                        18.1805806451613,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("4/1/2017"),
                                                        (DateTime.Parse("4/1/2017") - DateTime.Parse("11/1/2012")).Days,
                                                        1.3671,
                                                        3.2193,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("5/1/2017"),
                                                        (DateTime.Parse("5/1/2017") - DateTime.Parse("11/1/2012")).Days,
                                                        6.9421935483871,
                                                        18.4224193548387,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("6/1/2017"),
                                                        (DateTime.Parse("6/1/2017") - DateTime.Parse("11/1/2012")).Days,
                                                        13.6269,
                                                        19.698,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("7/1/2017"),
                                                        (DateTime.Parse("7/1/2017") - DateTime.Parse("11/1/2012")).Days,
                                                        11.3664193548387,
                                                        21.3102580645161,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("8/1/2017"),
                                                        (DateTime.Parse("8/1/2017") - DateTime.Parse("11/1/2012")).Days,
                                                        9.01916129032258,
                                                        16.0467096774194,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("9/1/2017"),
                                                        (DateTime.Parse("9/1/2017") - DateTime.Parse("11/1/2012")).Days,
                                                        10.2165,
                                                        16.0083,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("10/1/2017"),
                                                        (DateTime.Parse("10/1/2017") - DateTime.Parse("11/1/2012")).Days,
                                                        9.3748064516129,
                                                        16.1889677419355,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("11/1/2017"),
                                                        (DateTime.Parse("11/1/2017") - DateTime.Parse("11/1/2012")).Days,
                                                        8.9229,
                                                        16.0965,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("12/1/2017"),
                                                        (DateTime.Parse("12/1/2017") - DateTime.Parse("11/1/2012")).Days,
                                                        7.78151612903226,
                                                        14.4107419354839,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("1/1/2018"),
                                                        (DateTime.Parse("1/1/2018") - DateTime.Parse("11/1/2012")).Days,
                                                        7.15558064516129,
                                                        14.3822903225806,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("2/1/2018"),
                                                        (DateTime.Parse("2/1/2018") - DateTime.Parse("11/1/2012")).Days,
                                                        10.08,
                                                        13.986,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("3/1/2018"),
                                                        (DateTime.Parse("3/1/2018") - DateTime.Parse("11/1/2012")).Days,
                                                        6.615,
                                                        14.1689032258065,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("4/1/2018"),
                                                        (DateTime.Parse("4/1/2018") - DateTime.Parse("11/1/2012")).Days,
                                                        5.1597,
                                                        8.7024,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("5/1/2018"),
                                                        (DateTime.Parse("5/1/2018") - DateTime.Parse("11/1/2012")).Days,
                                                        0.0142258064516129,
                                                        1.42258064516129,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("6/1/2018"),
                                                        (DateTime.Parse("6/1/2018") - DateTime.Parse("11/1/2012")).Days,
                                                        0.01,
                                                        6.3651,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("7/1/2018"),
                                                        (DateTime.Parse("7/1/2018") - DateTime.Parse("11/1/2012")).Days,
                                                        0.298741935483871,
                                                        12.4049032258065,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("8/1/2018"),
                                                        (DateTime.Parse("8/1/2018") - DateTime.Parse("11/1/2012")).Days,
                                                        4.5238064516129,
                                                        19.6885161290323,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("9/1/2018"),
                                                        (DateTime.Parse("9/1/2018") - DateTime.Parse("11/1/2012")).Days,
                                                        5.1891,
                                                        8.6142,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("10/1/2018"),
                                                        (DateTime.Parse("10/1/2018") - DateTime.Parse("11/1/2012")).Days,
                                                        0.01,
                                                        0.0284516129032258,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("11/1/2018"),
                                                        (DateTime.Parse("11/1/2018") - DateTime.Parse("11/1/2012")).Days,
                                                        0.0588,
                                                        1.7493,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index++,
                                                        DateTime.Parse("12/1/2018"),
                                                        (DateTime.Parse("12/1/2018") - DateTime.Parse("11/1/2012")).Days,
                                                        8.80577419354839,
                                                        22.8750967741936,
                                                        0.0,
                                                        1500.0,
                                                        1.0));

            ProductionHistory.Add(new ProductionHistory(index,
                                                        DateTime.Parse("1/1/2019"),
                                                        (DateTime.Parse("1/1/2019") - DateTime.Parse("11/1/2012")).Days,
                                                        5.69032258064516,
                                                        13.8843870967742,
                                                        0.0,
                                                        1500.0,
                                                        1.0)); 
            #endregion

            MultiPorosityProperties.FractureProperties.Count        = 60;
            MultiPorosityProperties.FractureProperties.Width        = 0.1 / 12.0;
            MultiPorosityProperties.FractureProperties.Height       = 50.0;
            MultiPorosityProperties.FractureProperties.HalfLength   = 348.0;
            MultiPorosityProperties.FractureProperties.Porosity     = 0.20;
            MultiPorosityProperties.FractureProperties.Permeability = 184.0;
            MultiPorosityProperties.FractureProperties.Skin         = 0.0;
            
            MultiPorosityProperties.NaturalFractureProperties.Count        = 60;
            MultiPorosityProperties.NaturalFractureProperties.Width        = 0.01 / 12.0;
            MultiPorosityProperties.NaturalFractureProperties.Porosity     = 0.10;
            MultiPorosityProperties.NaturalFractureProperties.Permeability = 1.0;
            
            MultiPorosityProperties.ReservoirProperties.Length                = 6500.0;
            MultiPorosityProperties.ReservoirProperties.Width                 = 348.0;
            MultiPorosityProperties.ReservoirProperties.Thickness             = 125.0;
            MultiPorosityProperties.ReservoirProperties.Porosity              = 0.06;
            MultiPorosityProperties.ReservoirProperties.Permeability          = 0.002;
            MultiPorosityProperties.ReservoirProperties.Compressibility       = 0.00002;
            MultiPorosityProperties.ReservoirProperties.BottomholeTemperature = 275.0;
            MultiPorosityProperties.ReservoirProperties.InitialPressure       = 7000.0;

            MultiPorosityProperties.WellProperties.API                = "##############";
            MultiPorosityProperties.WellProperties.LateralLength      = 6500.0;
            MultiPorosityProperties.WellProperties.BottomholePressure = 3500.0;

            MultiPorosityProperties.Pvt.GasSaturation              = 0.5;
            MultiPorosityProperties.Pvt.GasSpecificGravity         = 0.7;
            MultiPorosityProperties.Pvt.GasViscosity               = 0.0;
            MultiPorosityProperties.Pvt.GasFormationVolumeFactor   = 0.0;
            MultiPorosityProperties.Pvt.GasCompressibilityFactor   = 0.0;
            MultiPorosityProperties.Pvt.GasCompressibility         = 0.0;
            MultiPorosityProperties.Pvt.OilSaturation              = 0.5;
            MultiPorosityProperties.Pvt.OilApiGravity              = 50.0;
            MultiPorosityProperties.Pvt.OilViscosity               = 0.0;
            MultiPorosityProperties.Pvt.OilFormationVolumeFactor   = 0.0;
            MultiPorosityProperties.Pvt.OilCompressibility         = 0.0;
            MultiPorosityProperties.Pvt.WaterSaturation            = 0.0;
            MultiPorosityProperties.Pvt.WaterSpecificGravity       = 1.05;
            MultiPorosityProperties.Pvt.WaterViscosity             = 0.0;
            MultiPorosityProperties.Pvt.WaterFormationVolumeFactor = 0.0;
            MultiPorosityProperties.Pvt.WaterCompressibility       = 0.0;
            
            MultiPorosityProperties.RelativePermeabilities.MatrixOil            = 0.0;
            MultiPorosityProperties.RelativePermeabilities.MatrixWater          = 0.0;
            MultiPorosityProperties.RelativePermeabilities.MatrixGas            = 0.0;
            MultiPorosityProperties.RelativePermeabilities.FractureOil          = 0.0;
            MultiPorosityProperties.RelativePermeabilities.FractureWater        = 0.0;
            MultiPorosityProperties.RelativePermeabilities.FractureGas          = 0.0;
            MultiPorosityProperties.RelativePermeabilities.NaturalFractureOil   = 0.0;
            MultiPorosityProperties.RelativePermeabilities.NaturalFractureWater = 0.0;
            MultiPorosityProperties.RelativePermeabilities.NaturalFractureGas   = 0.0;


            PvtModelProperties.SeparatorPressure              = 0.0;
            PvtModelProperties.SeparatorTemperature           = 0.0;
            PvtModelProperties.WaterSalinity                  = 0.0;
            PvtModelProperties.GasViscosityType               = GasViscosityType.LeeGonzalezEakin;
            PvtModelProperties.GasFormationVolumeFactorType   = GasFormationVolumeFactorType.General;
            PvtModelProperties.GasCompressibilityFactorType   = GasCompressibilityFactorType.DranchukAbuKassem;
            PvtModelProperties.GasPseudoCriticalType          = GasPseudoCriticalType.Sutton;
            PvtModelProperties.GasCompressibilityType         = GasCompressibilityType.McCain;
            PvtModelProperties.OilSolutionGasType             = OilSolutionGasType.PetroskyFarshad;
            PvtModelProperties.OilBubblePointType             = OilBubblePointType.PetroskyFarshad;
            PvtModelProperties.DeadOilViscosityType           = DeadOilViscosityType.PetroskyFarshad;
            PvtModelProperties.SaturatedOilViscosityType      = SaturatedOilViscosityType.PetroskyFarshad;
            PvtModelProperties.UnderSaturatedOilViscosityType = UnderSaturatedOilViscosityType.PetroskyFarshad;
            PvtModelProperties.OilFormationVolumeFactorType   = OilFormationVolumeFactorType.PetroskyFarshad;
            PvtModelProperties.OilCompressibilityType         = OilCompressibilityType.PetroskyFarshad;
            PvtModelProperties.WaterViscosityType             = WaterViscosityType.McCain;
            PvtModelProperties.WaterFormationVolumeFactorType = WaterFormationVolumeFactorType.McCain;
            PvtModelProperties.WaterCompressibilityType       = WaterCompressibilityType.McCain;
            
            
            /*km*/
            MultiPorosityModelParameters.MatrixPermeability = 0.00019;
            /*kF*/
            MultiPorosityModelParameters.HydraulicFracturePermeability = 184.0;
            /*kf*/
            MultiPorosityModelParameters.NaturalFracturePermeability = 0.8;
            /*ye*/
            MultiPorosityModelParameters.HydraulicFractureHalfLength = MultiPorosityProperties.FractureProperties.HalfLength;
            /*LF*/
            MultiPorosityModelParameters.HydraulicFractureSpacing = MultiPorosityProperties.ReservoirProperties.Length / 60.0;
            /*Lf*/
            MultiPorosityModelParameters.NaturalFractureSpacing = MultiPorosityProperties.FractureProperties.HalfLength / 10.0;
            /*sk*/
            MultiPorosityModelParameters.Skin = 0.0;
            
            

            /*km*/
            //arg_limits[0] = new BoundConstraints<double>(0.0001, 0.01);
            MultiPorosityHistoryMatchParameters.MatrixPermeability.Lower = 0.0001;
            MultiPorosityHistoryMatchParameters.MatrixPermeability.Upper  = 0.01;

            /*kF*/
            //arg_limits[1] = new BoundConstraints<double>(100.0, 10000.0);
            MultiPorosityHistoryMatchParameters.HydraulicFracturePermeability.Lower = 100.0;
            MultiPorosityHistoryMatchParameters.HydraulicFracturePermeability.Upper = 10000.0;

            /*kf*/
            //arg_limits[2] = new BoundConstraints<double>(0.01, 100.0);
            MultiPorosityHistoryMatchParameters.NaturalFracturePermeability.Lower = 0.01;
            MultiPorosityHistoryMatchParameters.NaturalFracturePermeability.Upper = 100.0;

            /*ye*/
            //arg_limits[3] = new BoundConstraints<double>(1.0, 500.0);
            MultiPorosityHistoryMatchParameters.HydraulicFractureHalfLength.Lower = 1.0;
            MultiPorosityHistoryMatchParameters.HydraulicFractureHalfLength.Upper = 500.0;

            /*LF*/
            //arg_limits[4] = new BoundConstraints<double>(50.0, 250.0);
            MultiPorosityHistoryMatchParameters.HydraulicFractureSpacing.Lower = 50.0;
            MultiPorosityHistoryMatchParameters.HydraulicFractureSpacing.Upper = 250.0;

            /*Lf*/
            MultiPorosityHistoryMatchParameters.NaturalFractureSpacing.Lower = 10.0;
            MultiPorosityHistoryMatchParameters.NaturalFractureSpacing.Upper = 150.0;
            //arg_limits[5] = new BoundConstraints<double>(10.0, 150.0);

            /*sk*/
            //arg_limits[6] = new BoundConstraints<double>(0.0, 0.0);
            MultiPorosityHistoryMatchParameters.Skin.Lower = 0.0;
            MultiPorosityHistoryMatchParameters.Skin.Upper = 0.0;

            RelativePermeabilityProperties.Matrix.SaturationWaterConnate                  = 0.0;
            RelativePermeabilityProperties.Matrix.SaturationWaterCritical                 = 0.4;
            RelativePermeabilityProperties.Matrix.SaturationOilIrreducibleWater           = 0.0;
            RelativePermeabilityProperties.Matrix.SaturationOilResidualWater              = 0.0;
            RelativePermeabilityProperties.Matrix.SaturationOilIrreducibleGas             = 0.0;
            RelativePermeabilityProperties.Matrix.SaturationOilResidualGas                = 0.0;
            RelativePermeabilityProperties.Matrix.SaturationGasConnate                    = 0.0;
            RelativePermeabilityProperties.Matrix.SaturationGasCritical                   = 0.2;
            RelativePermeabilityProperties.Matrix.PermeabilityRelativeWaterOilIrreducible = 1.0;
            RelativePermeabilityProperties.Matrix.PermeabilityRelativeOilWaterConnate     = 1.0;
            RelativePermeabilityProperties.Matrix.PermeabilityRelativeGasLiquidConnate    = 1.0;
            RelativePermeabilityProperties.Matrix.ExponentPermeabilityRelativeWater       = 4.0;
            RelativePermeabilityProperties.Matrix.ExponentPermeabilityRelativeOilWater    = 1.0;
            RelativePermeabilityProperties.Matrix.ExponentPermeabilityRelativeGas         = 1.0;
            RelativePermeabilityProperties.Matrix.ExponentPermeabilityRelativeOilGas      = 1.0;

            RelativePermeabilityProperties.HydraulicFracture.SaturationWaterConnate                  = 0.0;
            RelativePermeabilityProperties.HydraulicFracture.SaturationWaterCritical                 = 0.0;
            RelativePermeabilityProperties.HydraulicFracture.SaturationOilIrreducibleWater           = 0.0;
            RelativePermeabilityProperties.HydraulicFracture.SaturationOilResidualWater              = 0.0;
            RelativePermeabilityProperties.HydraulicFracture.SaturationOilIrreducibleGas             = 0.0;
            RelativePermeabilityProperties.HydraulicFracture.SaturationOilResidualGas                = 0.0;
            RelativePermeabilityProperties.HydraulicFracture.SaturationGasConnate                    = 0.0;
            RelativePermeabilityProperties.HydraulicFracture.SaturationGasCritical                   = 0.0;
            RelativePermeabilityProperties.HydraulicFracture.PermeabilityRelativeWaterOilIrreducible = 1.0;
            RelativePermeabilityProperties.HydraulicFracture.PermeabilityRelativeOilWaterConnate     = 1.0;
            RelativePermeabilityProperties.HydraulicFracture.PermeabilityRelativeGasLiquidConnate    = 1.0;
            RelativePermeabilityProperties.HydraulicFracture.ExponentPermeabilityRelativeWater       = 1.0;
            RelativePermeabilityProperties.HydraulicFracture.ExponentPermeabilityRelativeOilWater    = 1.0;
            RelativePermeabilityProperties.HydraulicFracture.ExponentPermeabilityRelativeGas         = 1.0;
            RelativePermeabilityProperties.HydraulicFracture.ExponentPermeabilityRelativeOilGas      = 1.0;

            RelativePermeabilityProperties.NaturalFracture.SaturationWaterConnate                  = 0.0;
            RelativePermeabilityProperties.NaturalFracture.SaturationWaterCritical                 = 0.0;
            RelativePermeabilityProperties.NaturalFracture.SaturationOilIrreducibleWater           = 0.0;
            RelativePermeabilityProperties.NaturalFracture.SaturationOilResidualWater              = 0.0;
            RelativePermeabilityProperties.NaturalFracture.SaturationOilIrreducibleGas             = 0.0;
            RelativePermeabilityProperties.NaturalFracture.SaturationOilResidualGas                = 0.0;
            RelativePermeabilityProperties.NaturalFracture.SaturationGasConnate                    = 0.0;
            RelativePermeabilityProperties.NaturalFracture.SaturationGasCritical                   = 0.0;
            RelativePermeabilityProperties.NaturalFracture.PermeabilityRelativeWaterOilIrreducible = 1.0;
            RelativePermeabilityProperties.NaturalFracture.PermeabilityRelativeOilWaterConnate     = 1.0;
            RelativePermeabilityProperties.NaturalFracture.PermeabilityRelativeGasLiquidConnate    = 1.0;
            RelativePermeabilityProperties.NaturalFracture.ExponentPermeabilityRelativeWater       = 2.0;
            RelativePermeabilityProperties.NaturalFracture.ExponentPermeabilityRelativeOilWater    = 2.0;
            RelativePermeabilityProperties.NaturalFracture.ExponentPermeabilityRelativeGas         = 2.0;
            RelativePermeabilityProperties.NaturalFracture.ExponentPermeabilityRelativeOilGas      = 2.0;

            DatabaseDataSource = new()
            {
                Host         = "timothyrmcnealy.com",
                Port         = 15432,
                Username     = "db_user",
                Password     = "dbAccess",
                DatabaseName = "OilGas"
            };
        }
    }
}
