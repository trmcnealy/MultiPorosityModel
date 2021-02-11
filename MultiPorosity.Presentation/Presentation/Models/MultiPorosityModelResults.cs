using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

using Engineering.UI.Collections;
using Engineering.DataSource;

using Prism.Mvvm;

namespace MultiPorosity.Presentation.Models
{
    public class MultiPorosityModelResults : BindableBase
    {
        private BindableCollection<MultiPorosityModelProduction>      _production;
        private BindableCollection<TriplePorosityOptimizationResults> _triplePorosityOptimizationResults;
        private double                                                _MatrixPermeability;
        private double                                                _HydraulicFracturePermeability;
        private double                                                _NaturalFracturePermeability;
        private double                                                _HydraulicFractureHalfLength;
        private double                                                _HydraulicFractureSpacing;
        private double                                                _NaturalFractureSpacing;
        private double                                                _Skin;

        public BindableCollection<MultiPorosityModelProduction> Production
        {
            get { return _production; }
            set { SetProperty(ref _production, value); }
        }

        public BindableCollection<TriplePorosityOptimizationResults> TriplePorosityOptimizationResults
        {
            get { return _triplePorosityOptimizationResults; }
            set { SetProperty(ref _triplePorosityOptimizationResults, value); }
        }

        public double MatrixPermeability
        {
            get { return _MatrixPermeability; }
            set { SetProperty(ref _MatrixPermeability, value); }
        }

        public double HydraulicFracturePermeability
        {
            get { return _HydraulicFracturePermeability; }
            set { SetProperty(ref _HydraulicFracturePermeability, value); }
        }

        public double NaturalFracturePermeability
        {
            get { return _NaturalFracturePermeability; }
            set { SetProperty(ref _NaturalFracturePermeability, value); }
        }

        public double HydraulicFractureHalfLength
        {
            get { return _HydraulicFractureHalfLength; }
            set { SetProperty(ref _HydraulicFractureHalfLength, value); }
        }

        public double HydraulicFractureSpacing
        {
            get { return _HydraulicFractureSpacing; }
            set { SetProperty(ref _HydraulicFractureSpacing, value); }
        }

        public double NaturalFractureSpacing
        {
            get { return _NaturalFractureSpacing; }
            set { SetProperty(ref _NaturalFractureSpacing, value); }
        }

        public double Skin
        {
            get { return _Skin; }
            set { SetProperty(ref _Skin, value); }
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
            Production                        = new(production);
            TriplePorosityOptimizationResults = new(triplePorosityOptimizationResults);
            MatrixPermeability                = matrixPermeability;
            HydraulicFracturePermeability     = hydraulicFracturePermeability;
            NaturalFracturePermeability       = naturalFracturePermeability;
            HydraulicFractureHalfLength       = hydraulicFractureHalfLength;
            HydraulicFractureSpacing          = hydraulicFractureSpacing;
            NaturalFractureSpacing            = naturalFractureSpacing;
            Skin                              = skin;
        }

        public MultiPorosityModelResults(MultiPorosity.Services.Models.MultiPorosityModelResults multiPorosityModelResults)
        {
            Production = new(MultiPorosityModelProduction.Convert(multiPorosityModelResults.Production));

            TriplePorosityOptimizationResults = new(MultiPorosity.Presentation.Models.TriplePorosityOptimizationResults.Convert(multiPorosityModelResults.TriplePorosityOptimizationResults));

            MatrixPermeability            = multiPorosityModelResults.MatrixPermeability;
            HydraulicFracturePermeability = multiPorosityModelResults.HydraulicFracturePermeability;
            NaturalFracturePermeability   = multiPorosityModelResults.NaturalFracturePermeability;
            HydraulicFractureHalfLength   = multiPorosityModelResults.HydraulicFractureHalfLength;
            HydraulicFractureSpacing      = multiPorosityModelResults.HydraulicFractureSpacing;
            NaturalFractureSpacing        = multiPorosityModelResults.NaturalFractureSpacing;
            Skin                          = multiPorosityModelResults.Skin;
        }

        public static implicit operator MultiPorosity.Services.Models.MultiPorosityModelResults(MultiPorosityModelResults multiPorosityModelResults)
        {
            return new(MultiPorosityModelProduction.Convert(multiPorosityModelResults.Production.ToList()),
                       MultiPorosity.Presentation.Models.TriplePorosityOptimizationResults.Convert(multiPorosityModelResults.TriplePorosityOptimizationResults.ToList()),
                       multiPorosityModelResults.MatrixPermeability,
                       multiPorosityModelResults.HydraulicFracturePermeability,
                       multiPorosityModelResults.NaturalFracturePermeability,
                       multiPorosityModelResults.HydraulicFractureHalfLength,
                       multiPorosityModelResults.HydraulicFractureSpacing,
                       multiPorosityModelResults.NaturalFractureSpacing,
                       multiPorosityModelResults.Skin);
        }
    }
}