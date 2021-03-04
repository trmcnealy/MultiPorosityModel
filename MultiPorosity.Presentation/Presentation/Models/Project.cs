using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;

using Engineering.UI.Collections;

using MultiPorosity.Models;
using MultiPorosity.Services;
using MultiPorosity.Services.Models;

using Prism.Mvvm;

namespace MultiPorosity.Presentation.Models
{
    public sealed class Project : BindableBase
    {
        private string _name;

        private ProductionDataSet dataSet;

        private DataView _productionHistory;

        private PvtModelProperties _pvtModelProperties;

        private MultiPorosityProperties _multiPorosityProperties;

        private RelativePermeabilityProperties _relativePermeabilityProperties;

        private MultiPorosityHistoryMatchParameters _multiPorosityHistoryMatchParameters;

        private MultiPorosityModelParameters _multiPorosityModelParameters;

        private ParticleSwarmOptimizationOptions _particleSwarmOptimizationOptions;

        private MultiPorosityModelResults _multiPorosityModelResults;

        private PorosityModelKind _porosityModelKind;

        private FlowType _flowType;

        private SolutionType _solutionType;

        private InverseTransformPrecision _inverseTransformPrecision;

        private DatabaseDataSource _DatabaseDataSource;

        private ProductionSmoothing _ProductionSmoothing;

        private double maxGas;

        private double maxOil;

        public string Name
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get { return _name; }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set { SetProperty(ref _name, value, nameof(Name)); }
        }

        public double MaxGas
        {
            get { return maxGas; }
            set { SetProperty(ref maxGas, value); }
        }

        public double MaxOil
        {
            get { return maxOil; }
            set { SetProperty(ref maxOil, value); }
        }

        public ProductionDataSet ProductionDataSet
        {
            get { return dataSet; }
            set
            {
                if(SetProperty(ref dataSet, value))
                {
                    ProductionHistory = dataSet.Actual.DefaultView;
                }
            }
        }

        public DataView ProductionHistory
        {
            get { return _productionHistory; }
            set
            {
                if(SetProperty(ref _productionHistory, value))
                {
                    dataSet.Actual.DefaultView.ListChanged -= OnDataViewChanged;
                    dataSet.Actual.DefaultView.ListChanged += OnDataViewChanged;

                    UpdateRecords();
                }
            }
        }

        public MultiPorosityProperties MultiPorosityProperties
        {
            get { return _multiPorosityProperties; }
            set
            {
                if(SetProperty(ref _multiPorosityProperties, value))
                {
                    RaisePropertyChanged(nameof(MultiPorosityModelParameters));
                }
            }
        }

        public PvtModelProperties PvtModelProperties
        {
            get { return _pvtModelProperties; }
            set
            {
                if(SetProperty(ref _pvtModelProperties, value))
                {
                }
            }
        }

        public RelativePermeabilityProperties RelativePermeabilityProperties
        {
            get { return _relativePermeabilityProperties; }
            set
            {
                if(SetProperty(ref _relativePermeabilityProperties, value))
                {
                }
            }
        }

        public MultiPorosityHistoryMatchParameters MultiPorosityHistoryMatchParameters
        {
            get { return _multiPorosityHistoryMatchParameters; }
            set
            {
                if(SetProperty(ref _multiPorosityHistoryMatchParameters, value))
                {
                    void onPropertyChanged(object?                   sender,
                                           PropertyChangedEventArgs? e)
                    {
                        RaisePropertyChanged(nameof(MultiPorosityHistoryMatchParameters));
                    }

                    _multiPorosityHistoryMatchParameters.PropertyChanged -= onPropertyChanged;
                    _multiPorosityHistoryMatchParameters.PropertyChanged += onPropertyChanged;
                }
            }
        }

        public MultiPorosityModelParameters MultiPorosityModelParameters
        {
            get { return _multiPorosityModelParameters; }
            set
            {
                if(SetProperty(ref _multiPorosityModelParameters, value))
                {
                    void onPropertyChanged(object?                   sender,
                                           PropertyChangedEventArgs? e)
                    {
                        RaisePropertyChanged(nameof(MultiPorosityModelParameters));
                    }

                    _multiPorosityModelParameters.PropertyChanged -= onPropertyChanged;
                    _multiPorosityModelParameters.PropertyChanged += onPropertyChanged;
                }
            }
        }

        public ParticleSwarmOptimizationOptions ParticleSwarmOptimizationOptions
        {
            get { return _particleSwarmOptimizationOptions; }
            set
            {
                if(SetProperty(ref _particleSwarmOptimizationOptions, value))
                {
                }
            }
        }

        public MultiPorosityModelResults MultiPorosityModelResults
        {
            get { return _multiPorosityModelResults; }
            set
            {
                if(SetProperty(ref _multiPorosityModelResults, value))
                {
                }
            }
        }

        public PorosityModelKind PorosityModelKind
        {
            get { return _porosityModelKind; }
            set
            {
                if(SetProperty(ref _porosityModelKind, value))
                {
                }
            }
        }

        public FlowType FlowType
        {
            get { return _flowType; }
            set
            {
                if(SetProperty(ref _flowType, value))
                {
                }
            }
        }

        public SolutionType SolutionType
        {
            get { return _solutionType; }
            set
            {
                if(SetProperty(ref _solutionType, value))
                {
                }
            }
        }

        public InverseTransformPrecision InverseTransformPrecision
        {
            get { return _inverseTransformPrecision; }
            set
            {
                if(SetProperty(ref _inverseTransformPrecision, value))
                {
                }
            }
        }

        public DatabaseDataSource DatabaseDataSource
        {
            get { return _DatabaseDataSource; }
            set
            {
                if(SetProperty(ref _DatabaseDataSource, value))
                {
                }
            }
        }

        public ProductionSmoothing ProductionSmoothing
        {
            get { return _ProductionSmoothing; }
            set
            {
                if(SetProperty(ref _ProductionSmoothing, value))
                {
                }
            }
        }

        #region Collections

        private BindableCollection<ProductionRecord> productionRecords = new();

        private BindableCollection<ProductionRecord> selectedProductionRecords = new();

        private BindableCollection<CumulativeProductionRecord> cumulativeProductionRecords = new();

        private BindableCollection<CumulativeProductionRecord> _SelectedCumulativeProductionRecords = new();

        private BindableCollection<MultiPorosity.Models.MultiPorosityModelProduction> multiPorosityModelProduction = new();

        private BindableCollection<CumulativeMultiPorosityModelProduction> cumulativeMultiPorosityModelProduction = new();

        private BindableCollection<RelativePermeabilityModel> _relativePermeabilityMatrixModels = new();

        private BindableCollection<RelativePermeabilityModel> selectedRelativePermeabilityMatrixModels = new();

        private BindableCollection<RelativePermeabilityModel> _relativePermeabilityHydraulicFractureModels = new();

        private BindableCollection<RelativePermeabilityModel> selectedRelativePermeabilityHydraulicFractureModels = new();

        private BindableCollection<RelativePermeabilityModel> _relativePermeabilityNaturalFractureModels = new();

        private BindableCollection<RelativePermeabilityModel> selectedRelativePermeabilityNaturalFractureModels = new();

        public BindableCollection<ProductionRecord> ProductionRecords
        {
            get { return productionRecords; }
            set
            {
                if(SetProperty(ref productionRecords, value))
                {
                    void OnCollectionChanged(object?                          sender,
                                             NotifyCollectionChangedEventArgs args)
                    {
                        RaisePropertyChanged(nameof(ProductionRecords));
                    }

                    productionRecords.CollectionChanged -= OnCollectionChanged;
                    productionRecords.CollectionChanged += OnCollectionChanged;
                }
            }
        }

        public BindableCollection<ProductionRecord> SelectedProductionRecords
        {
            get { return selectedProductionRecords; }
            set
            {
                if(SetProperty(ref selectedProductionRecords, value))
                {
                    void OnCollectionChanged(object?                          sender,
                                             NotifyCollectionChangedEventArgs args)
                    {
                        RaisePropertyChanged(nameof(SelectedProductionRecords));
                    }

                    selectedProductionRecords.CollectionChanged -= OnCollectionChanged;
                    selectedProductionRecords.CollectionChanged += OnCollectionChanged;
                }
            }
        }

        public BindableCollection<CumulativeProductionRecord> CumulativeProductionRecords
        {
            get { return cumulativeProductionRecords; }
            set
            {
                if(SetProperty(ref cumulativeProductionRecords, value))
                {
                    void OnCollectionChanged(object?                          sender,
                                             NotifyCollectionChangedEventArgs args)
                    {
                        RaisePropertyChanged(nameof(CumulativeProductionRecords));
                    }

                    cumulativeProductionRecords.CollectionChanged -= OnCollectionChanged;
                    cumulativeProductionRecords.CollectionChanged += OnCollectionChanged;
                }
            }
        }

        public BindableCollection<CumulativeProductionRecord> SelectedCumulativeProductionRecords
        {
            get { return _SelectedCumulativeProductionRecords; }
            set
            {
                if(SetProperty(ref _SelectedCumulativeProductionRecords, value))
                {
                    void OnCollectionChanged(object?                          sender,
                                             NotifyCollectionChangedEventArgs args)
                    {
                        RaisePropertyChanged(nameof(SelectedCumulativeProductionRecords));
                    }

                    _SelectedCumulativeProductionRecords.CollectionChanged -= OnCollectionChanged;
                    _SelectedCumulativeProductionRecords.CollectionChanged += OnCollectionChanged;
                }
            }
        }

        public BindableCollection<MultiPorosity.Models.MultiPorosityModelProduction> MultiPorosityModelProduction
        {
            get { return multiPorosityModelProduction; }
            set
            {
                if(SetProperty(ref multiPorosityModelProduction, value))
                {
                    void OnCollectionChanged(object?                          sender,
                                             NotifyCollectionChangedEventArgs args)
                    {
                        RaisePropertyChanged(nameof(MultiPorosityModelProduction));
                    }

                    multiPorosityModelProduction.CollectionChanged -= OnCollectionChanged;
                    multiPorosityModelProduction.CollectionChanged += OnCollectionChanged;
                }
            }
        }

        public BindableCollection<CumulativeMultiPorosityModelProduction> CumulativeMultiPorosityModelProduction
        {
            get { return cumulativeMultiPorosityModelProduction; }
            set
            {
                if(SetProperty(ref cumulativeMultiPorosityModelProduction, value))
                {
                    void OnCollectionChanged(object?                          sender,
                                             NotifyCollectionChangedEventArgs args)
                    {
                        RaisePropertyChanged(nameof(CumulativeMultiPorosityModelProduction));
                    }

                    cumulativeMultiPorosityModelProduction.CollectionChanged -= OnCollectionChanged;
                    cumulativeMultiPorosityModelProduction.CollectionChanged += OnCollectionChanged;
                }
            }
        }

        public BindableCollection<RelativePermeabilityModel> RelativePermeabilityMatrixModels
        {
            get { return _relativePermeabilityMatrixModels; }
            set
            {
                if(SetProperty(ref _relativePermeabilityMatrixModels, value))
                {
                    _relativePermeabilityMatrixModels.CollectionChanged -= OnRelativePermeabilityMatrixModelsCollectionChanged;
                    _relativePermeabilityMatrixModels.CollectionChanged += OnRelativePermeabilityMatrixModelsCollectionChanged;
                }
            }
        }

        private void OnRelativePermeabilityMatrixModelsCollectionChanged(object?                          sender,
                                                                         NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(RelativePermeabilityMatrixModels));
        }

        public BindableCollection<RelativePermeabilityModel> SelectedRelativePermeabilityMatrixModels
        {
            get { return selectedRelativePermeabilityMatrixModels; }
            set
            {
                if(SetProperty(ref selectedRelativePermeabilityMatrixModels, value))
                {
                    selectedRelativePermeabilityMatrixModels.CollectionChanged -= OnSelectedRelativePermeabilityMatrixModelsCollectionChanged;
                    selectedRelativePermeabilityMatrixModels.CollectionChanged += OnSelectedRelativePermeabilityMatrixModelsCollectionChanged;
                }
            }
        }

        private void OnSelectedRelativePermeabilityMatrixModelsCollectionChanged(object?                          sender,
                                                                                 NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(SelectedRelativePermeabilityMatrixModels));
        }

        public BindableCollection<RelativePermeabilityModel> RelativePermeabilityHydraulicFractureModels
        {
            get { return _relativePermeabilityHydraulicFractureModels; }
            set
            {
                if(SetProperty(ref _relativePermeabilityHydraulicFractureModels, value))
                {
                    _relativePermeabilityHydraulicFractureModels.CollectionChanged -= OnRelativePermeabilityHydraulicFractureModelsCollectionChanged;
                    _relativePermeabilityHydraulicFractureModels.CollectionChanged += OnRelativePermeabilityHydraulicFractureModelsCollectionChanged;
                }
            }
        }

        private void OnRelativePermeabilityHydraulicFractureModelsCollectionChanged(object?                          sender,
                                                                                    NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(RelativePermeabilityHydraulicFractureModels));
        }

        public BindableCollection<RelativePermeabilityModel> SelectedRelativePermeabilityHydraulicFractureModels
        {
            get { return selectedRelativePermeabilityHydraulicFractureModels; }
            set
            {
                if(SetProperty(ref selectedRelativePermeabilityHydraulicFractureModels, value))
                {
                    selectedRelativePermeabilityHydraulicFractureModels.CollectionChanged -= OnSelectedRelativePermeabilityHydraulicFractureModelsCollectionChanged;
                    selectedRelativePermeabilityHydraulicFractureModels.CollectionChanged += OnSelectedRelativePermeabilityHydraulicFractureModelsCollectionChanged;
                }
            }
        }

        private void OnSelectedRelativePermeabilityHydraulicFractureModelsCollectionChanged(object?                          sender,
                                                                                            NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(SelectedRelativePermeabilityHydraulicFractureModels));
        }

        public BindableCollection<RelativePermeabilityModel> RelativePermeabilityNaturalFractureModels
        {
            get { return _relativePermeabilityNaturalFractureModels; }
            set
            {
                if(SetProperty(ref _relativePermeabilityNaturalFractureModels, value))
                {
                    _relativePermeabilityNaturalFractureModels.CollectionChanged -= OnRelativePermeabilityNaturalFractureModelsCollectionChanged;
                    _relativePermeabilityNaturalFractureModels.CollectionChanged += OnRelativePermeabilityNaturalFractureModelsCollectionChanged;
                }
            }
        }

        private void OnRelativePermeabilityNaturalFractureModelsCollectionChanged(object?                          sender,
                                                                                  NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(RelativePermeabilityNaturalFractureModels));
        }

        public BindableCollection<RelativePermeabilityModel> SelectedRelativePermeabilityNaturalFractureModels
        {
            get { return selectedRelativePermeabilityNaturalFractureModels; }
            set
            {
                if(SetProperty(ref selectedRelativePermeabilityNaturalFractureModels, value))
                {
                    selectedRelativePermeabilityNaturalFractureModels.CollectionChanged -= OnSelectedRelativePermeabilityNaturalFractureModelsCollectionChanged;
                    selectedRelativePermeabilityNaturalFractureModels.CollectionChanged += OnSelectedRelativePermeabilityNaturalFractureModelsCollectionChanged;
                }
            }
        }

        private void OnSelectedRelativePermeabilityNaturalFractureModelsCollectionChanged(object?                          sender,
                                                                                          NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(SelectedRelativePermeabilityNaturalFractureModels));
        }

        public int HistoryCount
        {
            get { return ProductionHistory.Count; }
            //set
            //{
            //    //if(productionHistory.Count < value)
            //    //{
            //    //    int num = productionHistory.Count;

            //    //    for(int i = 0; i < value - num; i++)
            //    //    {
            //    //        productionHistory.Add(new ProductionHistoryModel());
            //    //    }
            //    //}
            //    //else if(productionHistory.Count > value)
            //    //{
            //    //    int num = productionHistory.Count;

            //    //    for(int i = num - value; i > 0; i--)
            //    //    {
            //    //        productionHistory.RemoveAt(productionHistory.Count - i);
            //    //    }
            //    //}

            //    //if (ProductionHistory.Records != null)
            //    //{
            //    //    ProductionHistory.Records.ListChanged -= (o, e) => RaisePropertyChanged(nameof(HistoryModels));
            //    //    ProductionHistory.Records.ListChanged += (o, e) => RaisePropertyChanged(nameof(HistoryModels));
            //    //}

            //    UpdateIndex();
            //}
        }

        #endregion

        public Project(MultiPorosity.Services.Models.Project project)
        {
            ProductionDataSet productionDataSet = new();

            int      index;
            DateTime date;
            double   days;
            double   gas;
            double   oil;
            double   water;
            double   wellheadPressure;
            double   weight;

            ProductionHistory productionHistory;

            for(int i = 0; i < project.ProductionHistory.Count; ++i)
            {
                productionHistory = project.ProductionHistory[i];
                index             = productionHistory.Index;
                date              = productionHistory.Date;
                days              = productionHistory.Days;
                gas               = productionHistory.Gas;
                oil               = productionHistory.Oil;
                water             = productionHistory.Water;
                wellheadPressure  = productionHistory.WellheadPressure;
                weight            = productionHistory.Weight;

                productionDataSet.Actual.AddActualRow(index, date, days, gas, oil, water, wellheadPressure, weight);
            }

            ProductionDataSet = productionDataSet;

            //ProductionHistory.ListChanged += OnDataViewChanged;

            Name                                = project.Name;
            MultiPorosityProperties             = new(project.MultiPorosityProperties, project.PvtModelProperties, project.MultiPorosityModelParameters);
            PvtModelProperties                  = new(project.PvtModelProperties);
            RelativePermeabilityProperties      = new(project.RelativePermeabilityProperties);
            MultiPorosityHistoryMatchParameters = new(project.MultiPorosityHistoryMatchParameters);
            MultiPorosityModelParameters        = new(project.MultiPorosityModelParameters);
            ParticleSwarmOptimizationOptions    = new(project.ParticleSwarmOptimizationOptions);
            MultiPorosityModelResults           = new(project.MultiPorosityModelResults);
            DatabaseDataSource                  = new(project.DatabaseDataSource);
            ProductionSmoothing                 = (ProductionSmoothing)project.ProductionSmoothing;


            PorosityModelKind                   = project.PorosityModelKind;
            FlowType                            = project.FlowType;
            SolutionType                        = project.SolutionType;
            InverseTransformPrecision           = project.InverseTransformPrecision;
        }

        public static implicit operator MultiPorosity.Services.Models.Project(Project project)
        {
            List<ProductionHistory> productionHistory = new(project.ProductionDataSet.Actual.Count);

            int                         index;
            DateTime                    date;
            double                      days;
            double                      gas;
            double                      oil;
            double                      water;
            double                      wellheadPressure;
            double                      weight;
            ProductionDataSet.ActualRow row;

            for(int i = 0; i < project.ProductionDataSet.Actual.Count; ++i)
            {
                row              = project.ProductionDataSet.Actual[i];
                index            = row.Index;
                date             = row.Date;
                days             = row.Days;
                gas              = row.Gas;
                oil              = row.Oil;
                water            = row.Water;
                wellheadPressure = row.WellheadPressure;
                weight           = row.Weight;

                productionHistory.Add(new ProductionHistory(index, date, days, gas, oil, water, wellheadPressure, weight));
            }

            (MultiPorosity.Services.Models.MultiPorosityProperties multiPorosityProperties, 
             MultiPorosity.Services.Models.PvtModelProperties pvtModelProperties,
             MultiPorosity.Services.Models.MultiPorosityModelParameters multiPorosityModelParameters) models = project._multiPorosityProperties;

            return new(project._name,
                       productionHistory,
                       models.multiPorosityProperties,
                       models.pvtModelProperties,
                       project._relativePermeabilityProperties,
                       project._multiPorosityHistoryMatchParameters,
                       models.multiPorosityModelParameters,
                       project._particleSwarmOptimizationOptions,
                       project._multiPorosityModelResults,
                       project._DatabaseDataSource,
                       project._ProductionSmoothing,
                       project._porosityModelKind,
                       project._flowType,
                       project._solutionType,
                       project._inverseTransformPrecision);
        }

        private void OnDataViewChanged(object               sender,
                                       ListChangedEventArgs e)
        {
            UpdateRecords();
        }

        private void UpdateRecords()
        {
            if(ProductionDataSet.Actual.Count == 0)
            {
                return;
            }

            //productionRecords.Clear();
            //OilPoints.Clear();
            //DaysToDateMap.Clear();

            List<ProductionRecord> records = new(ProductionDataSet.Actual.Count);

            double days = 15;

            for(int i = 0; i < ProductionDataSet.Actual.Count; ++i)
            {
                records.Add(new ProductionRecord(i, ProductionDataSet.Actual[i]));

                days += 30;
            }

            ProductionRecords           = new BindableCollection<ProductionRecord>(records);
            CumulativeProductionRecords = new BindableCollection<CumulativeProductionRecord>(ProductionService.CumulativeProduction(productionRecords.ToList()));

            RaisePropertyChanged(nameof(HistoryCount));
            //RaisePropertyChanged(nameof(ProductionRecords));
            //RaisePropertyChanged(nameof(CumulativeProductionRecords));
            //this.RaisePropertyChanged(nameof(OilPoints));
        }

        public void UpdateProductionDataset(List<ProductionRecord> production_records)
        {
            ProductionDataSet productionDataSet = new();

            int      index;
            DateTime date;
            double   days;
            double   gas;
            double   oil;
            double   water;
            double   wellheadPressure;
            double   weight;

            ProductionRecord productionRecord;

            for(int i = 0; i < production_records.Count; ++i)
            {
                productionRecord = production_records[i];
                index            = productionRecord.Index;
                date             = productionRecord.Date;
                days             = productionRecord.Days;
                gas              = productionRecord.Gas;
                oil              = productionRecord.Oil;
                water            = productionRecord.Water;
                wellheadPressure = productionRecord.WellheadPressure;
                weight           = productionRecord.Weight;

                productionDataSet.Actual.AddActualRow(index, date, days, gas, oil, water, wellheadPressure, weight);
            }

            ProductionDataSet = productionDataSet;
        }

        public void UpdateModel(MultiPorosity.Services.Models.MultiPorosityModelResults results)
        {
            List<MultiPorosity.Models.MultiPorosityModelProduction> records = new(results.Production.Count);

            for(int i = 0; i < results.Production.Count; ++i)
            {
                records.Add(new MultiPorosity.Models.MultiPorosityModelProduction(results.Production[i].Days, results.Production[i].Gas, results.Production[i].Oil, results.Production[i].Water));
            }

            MultiPorosityModelResults = new(results);

            MultiPorosityModelProduction           = new BindableCollection<MultiPorosity.Models.MultiPorosityModelProduction>(records);
            CumulativeMultiPorosityModelProduction = new BindableCollection<CumulativeMultiPorosityModelProduction>(ProductionService.CumulativeProduction(multiPorosityModelProduction.ToList()));
        }
        
    }
}