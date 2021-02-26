using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engineering.DataSource;
using Engineering.UI.Collections;

using MultiPorosity.Models;
using MultiPorosity.Presentation.Models;

using Prism.Mvvm;

namespace MultiPorosity.Presentation.Services
{
    public sealed class ProductionSmootherService : BindableBase
    {
        private readonly MultiPorosityModelService _multiPorosityModelService;

        internal ProductionSmootherModel _model;

        public ProductionSmootherModel Model
        {
            get { return _model; }
            set
            {
                if(SetProperty(ref _model, value))
                {
                }
            }
        }

        public ProductionSmootherService(MultiPorosityModelService? multiPorosityModelService)
        {
            _multiPorosityModelService = Throw.IfNull(multiPorosityModelService);

            _model = new(multiPorosityModelService);
        }

        internal void SmoothProduction()
        {
            if(Model.ProductionRecords.Count > 0)
            {
                Models.ProductionSmoothing productionSmoothing = _multiPorosityModelService.ActiveProject.ProductionSmoothing;

                double[] days  = new ProductionRecordColumn(ProductionColumn.Days,  Model.ProductionRecords.ToArray()).ToArray().Cast<double>().ToArray();
                double[] gas   = new ProductionRecordColumn(ProductionColumn.Gas,   Model.ProductionRecords.ToArray()).ToArray().Cast<double>().ToArray();
                double[] oil   = new ProductionRecordColumn(ProductionColumn.Oil,   Model.ProductionRecords.ToArray()).ToArray().Cast<double>().ToArray();
                double[] water = new ProductionRecordColumn(ProductionColumn.Water, Model.ProductionRecords.ToArray()).ToArray().Cast<double>().ToArray();

                int  m          = productionSmoothing.NumberOfPoints;
                int  k          = productionSmoothing.Iterations;
                bool normalized = productionSmoothing.Normalized;

                double[] new_gas   = MultiPorosity.Services.ProductionService.KolmogorovZurbenko(days, gas,   m, k, normalized);
                double[] new_oil   = MultiPorosity.Services.ProductionService.KolmogorovZurbenko(days, oil,   m, k, normalized);
                double[] new_water = MultiPorosity.Services.ProductionService.KolmogorovZurbenko(days, water, m, k, normalized);

                List<ProductionRecord> smoothed = new(days.Length);

                for(int i = 0; i < days.Length; ++i)
                {
                    smoothed.Add(new ProductionRecord(Model.ProductionRecords[i].Index,
                                                      Model.ProductionRecords[i].Date,
                                                      Model.ProductionRecords[i].Days,
                                                      new_gas[i],
                                                      new_oil[i],
                                                      new_water[i],
                                                      Model.ProductionRecords[i].WellheadPressure,
                                                      Model.ProductionRecords[i].Weight));
                }

                Model.SmoothedProductionRecords = new(smoothed);
            }
        }

        internal void ImportTable()
        {
            if(Model.ProductionRecords.Count > 0)
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

                List<ProductionRecord> sortedRecords = Model.SmoothedProductionRecords.OrderByDescending(p => p.Date).ToList();

                for(int i = 0; i < sortedRecords.Count; ++i)
                {
                    index = i;

                    date = (DateTime)sortedRecords[i][ProductionColumn.Date];

                    days = sortedRecords[i][ProductionColumn.Days].DoubleValue() ?? 0.0;

                    gas   = sortedRecords[i][ProductionColumn.Gas].DoubleValue()   ?? 0.0;
                    oil   = sortedRecords[i][ProductionColumn.Oil].DoubleValue()   ?? 0.0;
                    water = sortedRecords[i][ProductionColumn.Water].DoubleValue() ?? 0.0;

                    wellheadPressure = sortedRecords[i][ProductionColumn.WellheadPressure].DoubleValue() ?? 0.0;
                    weight           = sortedRecords[i][ProductionColumn.Weight].DoubleValue()           ?? 0.0;

                    productionDataSet.Actual.AddActualRow(index, date, days, gas, oil, water, wellheadPressure, weight);
                }

                _multiPorosityModelService.ActiveProject.ProductionDataSet = productionDataSet;
            }
        }
    }
}