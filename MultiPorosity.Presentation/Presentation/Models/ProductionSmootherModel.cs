using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

using Engineering.DataSource;
using Engineering.DataSource.Services.DataAccess;
using Engineering.DataSource.Tools;
using Engineering.UI.Collections;
using Engineering.UI.Controls;

using Microsoft.Web.WebView2.Core.Raw;
using Microsoft.Win32;

using MultiPorosity.Models;
using MultiPorosity.Presentation.Services;
using MultiPorosity.Services;
using MultiPorosity.Services.Models;

using NpgsqlTypes;

using OilGas.Data.RRC.Texas;

using Plotly;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

using MessageBox = System.Windows.MessageBox;

namespace MultiPorosity.Presentation.Models
{
    public sealed class ProductionSmootherModel : BindableBase
    {
        private BindableCollection<ProductionRecord> productionRecords;
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

        private BindableCollection<ProductionRecord> smoothedProductionRecords = new();
        public BindableCollection<ProductionRecord> SmoothedProductionRecords
        {
            get { return smoothedProductionRecords; }
            set
            {
                if(SetProperty(ref smoothedProductionRecords, value))
                {
                    void OnCollectionChanged(object?                          sender,
                                             NotifyCollectionChangedEventArgs args)
                    {
                        RaisePropertyChanged(nameof(SmoothedProductionRecords));
                    }

                    smoothedProductionRecords.CollectionChanged -= OnCollectionChanged;
                    smoothedProductionRecords.CollectionChanged += OnCollectionChanged;
                }
            }
        }


        private ProductionSmoothing productionSmoothing;

        public ProductionSmoothing ProductionSmoothing
        {
            get { return productionSmoothing; }
            set { SetProperty(ref productionSmoothing, value); }
        }

        public ProductionSmootherModel(MultiPorosityModelService? multiPorosityModelService)
        {
            productionSmoothing = multiPorosityModelService.ActiveProject.ProductionSmoothing;

            ProductionRecords = multiPorosityModelService.ActiveProject.ProductionRecords;
        }
    }
}