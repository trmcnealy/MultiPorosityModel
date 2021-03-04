using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace MultiPorosity.Presentation.Services
{
    public sealed class DatabaseConnectionService
    {
        private readonly MultiPorosityModelService _multiPorosityModelService;

        public DbConnection Connection { get; internal set; }

        internal DatabaseConnectionService(MultiPorosityModelService? multiPorosityModelService)
        {
            _multiPorosityModelService = Throw.IfNull(multiPorosityModelService);
        }

        internal DatabaseDataSource DatabaseDataSource
        {
            get { return _multiPorosityModelService.ActiveProject.DatabaseDataSource; }
            set { _multiPorosityModelService.ActiveProject.DatabaseDataSource = value; }
        }

        internal ObservableDictionary<string, (string type, object[] array)>? UpdateLocations(DataView dataFilteredView)
        {
            if(dataFilteredView.Count > 0)
            {
                List<object> apis       = new(dataFilteredView.Count);
                List<object> latitudes  = new(dataFilteredView.Count);
                List<object> longitudes = new(dataFilteredView.Count);

                string? api;
                double? latitude;
                double? longitude;

                for(int i = 0; i < dataFilteredView.Count; ++i)
                {
                    api       = dataFilteredView[i]["Api"].StringValue();
                    latitude  = dataFilteredView[i]["SurfaceLatitude"].DoubleValue();
                    longitude = dataFilteredView[i]["SurfaceLongitude"].DoubleValue();

                    if(!string.IsNullOrEmpty(api) && latitude.HasValue && longitude.HasValue)
                    {
                        apis.Add(api);
                        latitudes.Add(latitude.Value);
                        longitudes.Add(longitude.Value);
                    }
                }

                return new ObservableDictionary<string, (string type, object[] array)>
                {
                    {
                        "Api", ("string", apis.ToArray())
                    },
                    {
                        "Latitude", ("double", latitudes.ToArray())
                    },
                    {
                        "Longitude", ("double", longitudes.ToArray())
                    }
                };
            }

            return null;
        }

        internal DataTable? GetWellListQuery(string sessionId)
        {
            if(!string.IsNullOrEmpty(sessionId))
            {
                string query = "SELECT * \nFROM \"WellListView\";";

                return DataSources.QueryDatabase(Connection, query);
            }

            return null;
        }

        internal DataTable? GetWellProductionQuery(string       sessionId,
                                                   DataRowView? SelectedDataRow)
        {
            if(!string.IsNullOrEmpty(sessionId) && SelectedDataRow is not null)
            {
                ApiNumber api = new(SelectedDataRow["Api"].ToString());

                string query = $"SELECT * \nFROM \"WellMonthlyProductionView\"\nWHERE \"Api\"='{api}';";

                return DataSources.QueryDatabase(Connection, query);
            }

            return null;
        }

        internal DataTable? GetSelectedWellListQuery(string             sessionId,
                                                     Collection<string> SelectedWells)
        {
            if(!string.IsNullOrEmpty(sessionId) && SelectedWells.Count > 0)
            {
                string query = "SELECT * FROM \"WellListView\" WHERE \"WellListView\".\"Api\" = ANY(ARRAY[";

                StringBuilder sb = new();

                sb.Append($" '{SelectedWells[0]}'");

                for(int i = 1; i < SelectedWells.Count; ++i)
                {
                    sb.Append($", '{SelectedWells[i]}'");
                }

                query += sb + "]::text[]);";

                return DataSources.QueryDatabase(Connection, query);
            }

            return null;
        }

        internal void ImportTable(DataTable WellProductionData)
        {
            if(WellProductionData.Rows.Count > 0)
            {
                DataView productionData = WellProductionData.DefaultView;

                ProductionDataSet productionDataSet = new();

                //const int IdIndex               = 0;
                //const int ApiIndex              = 1;
                //const int NameIndex             = 2;
                //const int NumberIndex           = 3;
                const int DateIndex             = 4;
                const int GasVolumeIndex        = 5;
                const int OilVolumeIndex        = 6;
                const int CondensateVolumeIndex = 7;
                const int WaterVolumeIndex      = 8;

                int      index;
                DateTime date;
                double   days;
                double   gas;
                double   oil;
                double   water;
                double   wellheadPressure;
                double   weight;

                productionData.Sort = "Date ASC";

                for(int i = 0; i < WellProductionData.Rows.Count; ++i)
                {
                    index = i;
                    days  = 0.0;

                    date = (DateTime)productionData[i][DateIndex];

                    gas   =  productionData[i][GasVolumeIndex].DoubleValue()        ?? 0.0;
                    oil   =  productionData[i][OilVolumeIndex].DoubleValue()        ?? 0.0;
                    oil   += productionData[i][CondensateVolumeIndex].DoubleValue() ?? 0.0;
                    water =  productionData[i][WaterVolumeIndex].DoubleValue()      ?? 0.0;

                    wellheadPressure = 0.0;
                    weight           = 1.0;

                    productionDataSet.Actual.AddActualRow(index, date, days, gas, oil, water, wellheadPressure, weight);
                }

                _multiPorosityModelService.ActiveProject.ProductionDataSet = productionDataSet;
            }
        }

        internal void ExportWellList(DataView WellListDataFiltered)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter           = "Csv file (*.csv)|*.csv|Excel file (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                InitialDirectory = _multiPorosityModelService.RepositoryPath ?? Environment.GetFolderPath(Environment.SpecialFolder.Recent)
            };

            if(saveFileDialog.ShowDialog() == true)
            {
                switch(Path.GetExtension(saveFileDialog.FileName))
                {
                    case ".csv":
                    {
                        DataSources.ExportCsv(WellListDataFiltered, saveFileDialog.FileName);
                        MessageBox.Show($"{saveFileDialog.FileName} saved.");

                        break;
                    }
                    case ".xlsx":
                    {
                        DataSources.ExporXlsx(WellListDataFiltered, saveFileDialog.FileName);
                        MessageBox.Show($"{saveFileDialog.FileName} saved.");

                        break;
                    }
                    default:
                    {
                        MessageBox.Show("Invalid file format.");

                        break;
                    }
                }
            }
        }

        internal void ExportWellProduction(DataTable WellProductionData)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter           = "Csv file (*.csv)|*.csv|Excel file (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                InitialDirectory = _multiPorosityModelService.RepositoryPath ?? Environment.GetFolderPath(Environment.SpecialFolder.Recent)
            };
            //Environment.GetFolderPath(Environment.SpecialFolder.Recent);

            if(saveFileDialog.ShowDialog() == true)
            {
                switch(Path.GetExtension(saveFileDialog.FileName))
                {
                    case ".csv":
                    {
                        DataSources.ExportCsv(WellProductionData, saveFileDialog.FileName);
                        MessageBox.Show($"{saveFileDialog.FileName} saved.");

                        break;
                    }
                    case ".xlsx":
                    {
                        DataSources.ExporXlsx(WellProductionData, saveFileDialog.FileName);
                        MessageBox.Show($"{saveFileDialog.FileName} saved.");

                        break;
                    }
                    default:
                    {
                        MessageBox.Show("Invalid file format.");

                        break;
                    }
                }
            }
        }

        
    }
}
