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

using DatabaseDataSource = MultiPorosity.Presentation.Models.DatabaseDataSource;
using MessageBox = System.Windows.MessageBox;

namespace MultiPorosity.Presentation.Models
{
    public sealed class ConnectToDatabaseModel : BindableBase
    {
        private DatabaseDataSource _DatabaseDataSource;
        public DatabaseDataSource DatabaseDataSource
        {
            get { return _DatabaseDataSource; }
            set { SetProperty(ref _DatabaseDataSource, value); }
        }

        private DataRowView? _SelectedDataRow;
        public DataRowView? SelectedDataRow
        {
            get { return _SelectedDataRow; }
            set
            {
                if(SetProperty(ref _SelectedDataRow, value))
                {
                }
            }
        }

        internal static string BuildRowFilter(CountyKind county,
                                              string?    api,
                                              string?    company,
                                              string?    reservoir)
        {
            string RowFilter = "";

            if(api is not null)
            {
                RowFilter = $"Api = '{api}'";
            }
            else if(county != CountyKind.None_Selected)
            {
                int countyValue = (int)county;

                RowFilter = $"Api LIKE '*-{countyValue:D3}-*'";
            }

            if(!string.IsNullOrEmpty(reservoir))
            {
                if(!string.IsNullOrEmpty(RowFilter) && RowFilter.Length > 0)
                {
                    RowFilter += $" AND ReservoirName LIKE '*{reservoir}*'";
                }
                else
                {
                    RowFilter = $"ReservoirName LIKE '*{reservoir}*'";
                }
            }

            if(!string.IsNullOrEmpty(company))
            {
                if(!string.IsNullOrEmpty(RowFilter) && RowFilter.Length > 0)
                {
                    RowFilter += $" AND CompanyName LIKE '*{company}*'";
                }
                else
                {
                    RowFilter = $"CompanyName LIKE '*{company}*'";
                }
            }

            return RowFilter;
        }

        internal void UpdateWellListDataFiltered()
        {
            DataView dataView = WellListData.DefaultView;

            if(WellListData.Rows.Count > 0)
            {
                try
                {
                    dataView.RowFilter = BuildRowFilter(FilterByCounty, FilterByApi, FilterByCompany, FilterByReservoir);
                }
                catch(Exception)
                {
                    // ignored
                }
            }
                    
            WellListDataFiltered = dataView;
        }

        private CountyKind _FilterByCounty;
        public CountyKind FilterByCounty
        {
            get { return _FilterByCounty; }
            set
            {
                if(SetProperty(ref _FilterByCounty, value))
                {
                    UpdateWellListDataFiltered();
                }
            }
        }

        private string? _FilterByApi;
        public string? FilterByApi
        {
            get { return _FilterByApi; }
            set
            {
                if(SetProperty(ref _FilterByApi, value))
                {
                    UpdateWellListDataFiltered();
                }
            }
        }

        private string? _FilterByCompany;
        public string? FilterByCompany
        {
            get { return _FilterByCompany; }
            set
            {
                if(SetProperty(ref _FilterByCompany, value))
                {
                    UpdateWellListDataFiltered();
                }
            }
        }

        private string? _FilterByReservoir;
        public string? FilterByReservoir
        {
            get { return _FilterByReservoir; }
            set
            {
                if(SetProperty(ref _FilterByReservoir, value))
                {
                    UpdateWellListDataFiltered();
                }
            }
        }

        private DataTable _wellListData;

        public DataTable WellListData
        {
            get { return _wellListData; }
            set
            {
                if(SetProperty(ref _wellListData, value))
                {
                    UpdateWellListDataFiltered();
                }
            }
        }

        public DataView WellListDataView
        {
            get { return _wellListData.DefaultView; }
        }

        private ObservableDictionary<string, (string type, object[] array)> _Locations;
        public ObservableDictionary<string, (string type, object[] array)> Locations
        {
            get { return _Locations; }
            set
            {
                if(SetProperty(ref _Locations, value))
                {
                }
            }
        }

        private BindableCollection<string> _SelectedWells;

        public event NotifyCollectionChangedEventHandler? SelectedWellsCollectionChanged;
        public BindableCollection<string> SelectedWells
        {
            get { return _SelectedWells; }
            set
            {
                if(SetProperty(ref _SelectedWells, value))
                {
                    NotifyCollectionChangedEventHandler? handler = SelectedWellsCollectionChanged;

                    handler?.Invoke(_SelectedWells, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
            }
        }

        void OnSelectedWellsChanged(object? sender, NotifyCollectionChangedEventArgs args)
        {
            RaisePropertyChanged(nameof(SelectedWells));
        }

        private DataTable _WellProductionData = new();
        public DataTable WellProductionData
        {
            get { return _WellProductionData; }
            set
            {
                if(SetProperty(ref _WellProductionData, value))
                {
                    RaisePropertyChanged(nameof(WellListDataFiltered));
                }
            }
        }


        private DataView _WellListDataFiltered;
        public DataView WellListDataFiltered
        {
            get { return _WellListDataFiltered; }
            set
            {
                if(SetProperty(ref _WellListDataFiltered, value))
                {
                }
            }
        }

        private string sessionId;
        public string SessionId
        {
            get { return sessionId; }
            set { SetProperty(ref sessionId, value); }
        }

        public ConnectToDatabaseModel()
        {
            _FilterByCounty = CountyKind.None_Selected;

            WellListData = new DataTable();

            SelectedDataRow = null;

            SessionId = string.Empty;
            
            _wellListData         = new();
            _WellListDataFiltered = new();
            _Locations            = new();
            _SelectedWells        = new();
        }
    }
}