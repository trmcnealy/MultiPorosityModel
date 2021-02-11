using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

using Engineering.DataSource;
using Engineering.DataSource.Services.DataAccess;
using Engineering.DataSource.Tools;

using MultiPorosity.Models;
using MultiPorosity.Services.Models;

using PostgreSql;

using ParticleSwarmOptimizationOptions = MultiPorosity.Services.Models.ParticleSwarmOptimizationOptions;

namespace MultiPorosity.Services
{
    public static class DataSources
    {
        public static Project? LoadProject(string filePath)
        {
            string json = File.ReadAllText(filePath);

            Project? project = JsonSerializer.Deserialize<Project>(json, ProjectJsonSettings.Default);

            return project;
        }

        public static void SaveProject(string                              filePath,
                                       string                              name,
                                       ProductionDataSet                   productionDataSet,
                                       MultiPorosityProperties             multiPorosityProperties,
                                       PvtModelProperties                  pvtModelProperties,
                                       RelativePermeabilityProperties      relativePermeabilityProperties,
                                       MultiPorosityHistoryMatchParameters multiPorosityHistoryMatchParameters,
                                       MultiPorosityModelParameters        multiPorosityModelParameters,
                                       ParticleSwarmOptimizationOptions    particleSwarmOptimizationOptions,
                                       MultiPorosityModelResults           multiPorosityModelResults,
                                       DatabaseDataSource                  databaseDataSource,
                                       PorosityModelKind                   porosityModelKind         = PorosityModelKind.Triple,
                                       FlowType                            flowType                  = FlowType.UnsteadyState,
                                       SolutionType                        solutionType              = SolutionType.Linear,
                                       InverseTransformPrecision           inverseTransformPrecision = InverseTransformPrecision.High)
        {
            List<ProductionHistory> productionHistory = new(productionDataSet.Actual.Count);

            for(int i = 0; i < productionDataSet.Actual.Count; ++i)
            {
                productionHistory.Add(new ProductionHistory((int)productionDataSet.Actual[i][0],
                                                            (DateTime)productionDataSet.Actual[i][1],
                                                            (double)productionDataSet.Actual[i][2],
                                                            (double)productionDataSet.Actual[i][3],
                                                            (double)productionDataSet.Actual[i][4],
                                                            (double)productionDataSet.Actual[i][5],
                                                            (double)productionDataSet.Actual[i][6],
                                                            (double)productionDataSet.Actual[i][7]));
            }

            SaveProject(filePath,
                        new(name,
                            productionHistory,
                            multiPorosityProperties,
                            pvtModelProperties,
                            relativePermeabilityProperties,
                            multiPorosityHistoryMatchParameters,
                            multiPorosityModelParameters,
                            particleSwarmOptimizationOptions,
                            multiPorosityModelResults,
                            databaseDataSource,
                            porosityModelKind,
                            flowType,
                            solutionType,
                            inverseTransformPrecision));
        }

        public static void SaveProject(string                              filePath,
                                       string                              name,
                                       List<ProductionHistory>             productionHistory,
                                       MultiPorosityProperties             multiPorosityProperties,
                                       PvtModelProperties                  pvtModelProperties,
                                       RelativePermeabilityProperties      relativePermeabilityProperties,
                                       MultiPorosityHistoryMatchParameters multiPorosityHistoryMatchParameters,
                                       MultiPorosityModelParameters        multiPorosityModelParameters,
                                       ParticleSwarmOptimizationOptions    particleSwarmOptimizationOptions,
                                       MultiPorosityModelResults           multiPorosityModelResults,
                                       DatabaseDataSource                  databaseDataSource,
                                       PorosityModelKind                   porosityModelKind         = PorosityModelKind.Triple,
                                       FlowType                            flowType                  = FlowType.UnsteadyState,
                                       SolutionType                        solutionType              = SolutionType.Linear,
                                       InverseTransformPrecision           inverseTransformPrecision = InverseTransformPrecision.High)
        {
            SaveProject(filePath,
                        new(name,
                            productionHistory,
                            multiPorosityProperties,
                            pvtModelProperties,
                            relativePermeabilityProperties,
                            multiPorosityHistoryMatchParameters,
                            multiPorosityModelParameters,
                            particleSwarmOptimizationOptions,
                            multiPorosityModelResults,
                            databaseDataSource,
                            porosityModelKind,
                            flowType,
                            solutionType,
                            inverseTransformPrecision));
        }

        public static void SaveProject(string  filePath,
                                       Project project)
        {
            string fileContent = JsonSerializer.Serialize(project, ProjectJsonSettings.Default);

            File.WriteAllText(filePath, fileContent, Encoding.UTF8);
        }

        public static ProductionDataSet ImportCsv(string strFilePath)
        {
            ProductionDataSet dataSet = new ProductionDataSet();

            //using(MemoryMapped mm = new(strFilePath))
            //{
            CsvReader csvReader = new(strFilePath);

            (List<string[]> header, List<string[]> rows) = csvReader.ReadFile(1);

            string[]? header_row = header.FirstOrDefault();

            if(header_row is null)
            {
                throw new NullReferenceException();
            }

            bool _hasDateColumn             = header_row.Any(r => r.Contains("Date"));
            bool _hasDaysColumn             = header_row.Any(r => r.Contains("Days"));
            bool _hasGasColumn              = header_row.Any(r => r.Contains("Gas"));
            bool _hasOilColumn              = header_row.Any(r => r.Contains("Oil"));
            bool _hasWaterColumn            = header_row.Any(r => r.Contains("Water"));
            bool _hasWellheadPressureColumn = header_row.Any(r => r.Contains("WellheadPressure"));
            bool _hasWeightColumn           = header_row.Any(r => r.Contains("Weight"));

            int _DateIndex             = _hasDateColumn ? header_row.IndexOf(r => r.Contains("Date")) : -1;
            int _DaysIndex             = _hasDaysColumn ? header_row.IndexOf(r => r.Contains("Days")) : -1;
            int _GasIndex              = _hasGasColumn ? header_row.IndexOf(r => r.Contains("Gas")) : -1;
            int _OilIndex              = _hasOilColumn ? header_row.IndexOf(r => r.Contains("Oil")) : -1;
            int _WaterIndex            = _hasWaterColumn ? header_row.IndexOf(r => r.Contains("Water")) : -1;
            int _WellheadPressureIndex = _hasWellheadPressureColumn ? header_row.IndexOf(r => r.Contains("WellheadPressure")) : -1;
            int _WeightIndex           = _hasWeightColumn ? header_row.IndexOf(r => r.Contains("Weight")) : -1;

            //

            rows = rows.Where(r => r.Length > 3 && !string.IsNullOrEmpty(r[0])).ToList();

            //Parallel.ForEach(Partitioner.Create(0, rows.Count),
            //                 row =>
            {
                bool hasDateColumn             = _hasDateColumn;
                bool hasDaysColumn             = _hasDaysColumn;
                bool hasGasColumn              = _hasGasColumn;
                bool hasOilColumn              = _hasOilColumn;
                bool hasWaterColumn            = _hasWaterColumn;
                bool hasWellheadPressureColumn = _hasWellheadPressureColumn;
                bool hasWeightColumn           = _hasWeightColumn;
                int  DateIndex                 = _DateIndex;
                int  DaysIndex                 = _DaysIndex;
                int  GasIndex                  = _GasIndex;
                int  OilIndex                  = _OilIndex;
                int  WaterIndex                = _WaterIndex;
                int  WellheadPressureIndex     = _WellheadPressureIndex;
                int  WeightIndex               = _WeightIndex;

                DateTime Date;
                double   Days;
                double   Gas;
                double   Oil;
                double   Water;
                double   WellheadPressure;
                double   Weight;

                ValueTuple<int, int> row = new(0, rows.Count);

                for(int i = row.Item1; i < row.Item2; i++)
                {
                    if(hasDateColumn)
                    {
                        if(!DateTime.TryParseExact(rows[i][DateIndex], "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out Date))
                        {
                            if(!DateTime.TryParseExact(rows[i][DateIndex], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out Date))
                            {
                                if(!DateTime.TryParseExact(rows[i][DateIndex], "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out Date))
                                {
                                    if(!DateTime.TryParseExact(rows[i][DateIndex], "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out Date))
                                    {
                                        if(!DateTime.TryParseExact(rows[i][DateIndex], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out Date))
                                        {
                                            if(!DateTime.TryParse(rows[i][DateIndex], out Date))
                                            {
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Date = new DateTime(1, 1, 1);
                    }

                    if(hasDaysColumn)
                    {
                        if(!double.TryParse(rows[i][DaysIndex], out Days))
                        {
                            Days = 0.0;
                        }
                    }
                    else
                    {
                        Days = 0.0;
                    }

                    if(hasGasColumn)
                    {
                        if(!double.TryParse(rows[i][GasIndex], out Gas))
                        {
                            Gas = 0.0;
                        }
                    }
                    else
                    {
                        Gas = 0.0;
                    }

                    if(hasOilColumn)
                    {
                        if(!double.TryParse(rows[i][OilIndex], out Oil))
                        {
                            Oil = 0.0;
                        }
                    }
                    else
                    {
                        Oil = 0.0;
                    }

                    if(hasWaterColumn)
                    {
                        if(!double.TryParse(rows[i][WaterIndex], out Water))
                        {
                            Water = 0.0;
                        }
                    }
                    else
                    {
                        Water = 0.0;
                    }

                    if(hasWellheadPressureColumn)
                    {
                        if(!double.TryParse(rows[i][WellheadPressureIndex], out WellheadPressure))
                        {
                            WellheadPressure = 0.0;
                        }
                    }
                    else
                    {
                        WellheadPressure = 0.0;
                    }

                    if(hasWeightColumn)
                    {
                        if(!double.TryParse(rows[i][WeightIndex], out Weight))
                        {
                            Weight = 0.0;
                        }
                    }
                    else
                    {
                        Weight = 0.0;
                    }

                    dataSet.Actual.AddActualRow(i, Date, Days, Gas, Oil, Water, WellheadPressure, Weight);
                }
            }
            //);

            if(_hasDateColumn)
            {
                dataSet.Actual[0]["Days"] = 1;

                for(int i = 1; i < dataSet.Actual.Count; ++i)
                {
                    dataSet.Actual[i]["Days"] = (double)dataSet.Actual[i - 1]["Days"] + ((DateTime)dataSet.Actual[i]["Date"] - (DateTime)dataSet.Actual[i - 1]["Date"]).Days;
                }
            }
            //}

            return dataSet;
        }

        public static void ExportCsv(DataView dtDataTable,
                                     string   strFilePath)
        {
            StreamWriter sw = new StreamWriter(strFilePath, false);

            if(dtDataTable.Table != null)
            {
                for(int i = 1; i < dtDataTable.Table.Columns.Count; i++)
                {
                    sw.Write(dtDataTable.Table.Columns[i]);

                    if(i < dtDataTable.Table.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
            }

            sw.Write(sw.NewLine);

            string value;

            //DataRow row;
            foreach(DataRowView dr in dtDataTable)
            {
                //row = dr.Row;

                if(dtDataTable.Table != null)
                {
                    for(int i = 1; i < dtDataTable.Table.Columns.Count; i++)
                    {
                        if(!Convert.IsDBNull(dr[i]))
                        {
                            value = dr[i].ToString() ?? string.Empty;

                            if(value.Contains(','))
                            {
                                value = $"\"{value}\"";
                                sw.Write(value);
                            }
                            else
                            {
                                sw.Write(dr[i].ToString());
                            }
                        }

                        if(i < dtDataTable.Table.Columns.Count - 1)
                        {
                            sw.Write(",");
                        }
                    }
                }

                sw.Write(sw.NewLine);
            }

            sw.Flush();
            sw.Close();
        }

        public static void ExportCsv(DataTable dtDataTable,
                                     string    strFilePath)
        {
            StreamWriter sw = new StreamWriter(strFilePath, false);

            for(int i = 1; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);

                if(i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }

            sw.Write(sw.NewLine);

            string value;

            foreach(DataRow dr in dtDataTable.Rows)
            {
                for(int i = 1; i < dtDataTable.Columns.Count; i++)
                {
                    if(!Convert.IsDBNull(dr[i]))
                    {
                        value = dr[i].ToString() ?? string.Empty;

                        if(value.Contains(','))
                        {
                            value = $"\"{value}\"";
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }

                    if(i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }

                sw.Write(sw.NewLine);
            }

            sw.Flush();
            sw.Close();
        }

        public static void ExportCsv(string[]                                                              cached_header,
                                     List<MultiPorosity.Services.Models.TriplePorosityOptimizationResults> cached_results,
                                     string                                                                strFilePath)
        {
            List<string>                                                          header  = new(cached_header);
            List<MultiPorosity.Services.Models.TriplePorosityOptimizationResults> results = cached_results;

            StreamWriter sw = new StreamWriter(strFilePath, false);

            sw.Write(header[0]);

            for(int i = 1; i < header.Count; i++)
            {
                sw.Write(",");
                sw.Write(header[i]);
            }

            sw.Write(sw.NewLine);

            for(int i = 0; i < results.Count; i++)
            {
                sw.Write(results[i].ToString());

                sw.Write(sw.NewLine);
            }

            sw.Flush();
            sw.Close();
        }

        public static void ExporXlsx(DataView dtDataTable,
                                     string   strFilePath)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Visible       = false;
            excel.DisplayAlerts = false;

            Microsoft.Office.Interop.Excel.Workbook  excelworkBook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel.Worksheet excelSheet    = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.ActiveSheet;

            if(dtDataTable.Table != null)
            {
                excel.ScreenUpdating = false;

                for(int i = 0; i < dtDataTable.Table.Columns.Count; i++)
                {
                    excelSheet.Cells[1, i + 1] = dtDataTable.Table.Columns[i];
                }

                string value;

                int j = 0;

                foreach(DataRowView dr in dtDataTable)
                {
                    for(int i = 0; i < dtDataTable.Table.Columns.Count; i++)
                    {
                        if(!Convert.IsDBNull(dr[i]))
                        {
                            value = dr[i].ToString() ?? string.Empty;

                            excelSheet.Cells[j + 1 + 1, i + 1] = value;
                        }
                    }

                    ++j;
                }

                excelworkBook.SaveAs(strFilePath,
                                     Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault,
                                     Type.Missing,
                                     Type.Missing,
                                     false,
                                     false,
                                     Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                                     Type.Missing,
                                     Type.Missing,
                                     Type.Missing,
                                     Type.Missing,
                                     Type.Missing);

                excel.ScreenUpdating = true;

                excelworkBook.Close();
                excel.Quit();
            }
        }

        public static void ExporXlsx(DataTable dtDataTable,
                                     string    strFilePath)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Visible       = false;
            excel.DisplayAlerts = false;

            Microsoft.Office.Interop.Excel.Workbook  excelworkBook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel.Worksheet excelSheet    = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.ActiveSheet;

            excel.ScreenUpdating = false;

            for(int i = 1; i < dtDataTable.Columns.Count; i++)
            {
                excelSheet.Cells[1, i + 1] = dtDataTable.Columns[i];
            }

            string value;

            int j = 0;

            foreach(DataRow dr in dtDataTable.Rows)
            {
                for(int i = 1; i < dtDataTable.Columns.Count; i++)
                {
                    if(!Convert.IsDBNull(dr[i]))
                    {
                        value = dr[i].ToString() ?? string.Empty;

                        excelSheet.Cells[j + 1 + 1, i + 1] = value;
                    }
                }

                ++j;
            }

            excelworkBook.SaveAs(strFilePath,
                                 Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault,
                                 Type.Missing,
                                 Type.Missing,
                                 false,
                                 false,
                                 Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                                 Type.Missing,
                                 Type.Missing,
                                 Type.Missing,
                                 Type.Missing,
                                 Type.Missing);

            excel.ScreenUpdating = true;

            excelworkBook.Close();
            excel.Quit();
        }

        public static void ExporXlsx(string[]                                                              cached_header,
                                     List<MultiPorosity.Services.Models.TriplePorosityOptimizationResults> cached_results,
                                     string                                                                strFilePath)
        {
            List<string>                                                          header  = new(cached_header);
            List<MultiPorosity.Services.Models.TriplePorosityOptimizationResults> results = cached_results;

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Visible       = false;
            excel.DisplayAlerts = false;

            Microsoft.Office.Interop.Excel.Workbook  excelworkBook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel.Worksheet excelSheet    = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.ActiveSheet;

            excel.ScreenUpdating = false;

            for(int i = 0; i < header.Count; i++)
            {
                excelSheet.Cells[1, i + 1] = header[i];
            }
            
            for(int i = 0; i < results.Count; i++)
            {
                for(int j = 0; j < results[i].GetLength(); j++)
                {
                    excelSheet.Cells[i + 1 + 1, j + 1] = results[i][j];
                }
            }

            excelworkBook.SaveAs(strFilePath,
                                 Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault,
                                 Type.Missing,
                                 Type.Missing,
                                 false,
                                 false,
                                 Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                                 Type.Missing,
                                 Type.Missing,
                                 Type.Missing,
                                 Type.Missing,
                                 Type.Missing);

            excel.ScreenUpdating = true;

            excelworkBook.Close();
            excel.Quit();
        }

        #region Database

        public static (DbConnection Connection, string sessionId) ConnectToDatabase(DatabaseDataSource databaseDataSource)
        {
            DbConnection dbConnection = new();

            string sessionId = string.Empty;

            try
            {
                sessionId = dbConnection.ConnectAsync(databaseDataSource.Host, databaseDataSource.Port, databaseDataSource.Username, databaseDataSource.Password, databaseDataSource.DatabaseName).
                                         Result;
            }
            catch(Exception)
            {
                // ignored
            }

            return (dbConnection, sessionId);
        }

        public static DataTable QueryDatabase(DbConnection dbConnection,
                                              string       query)
        {
            pg_result result = dbConnection.SqlQueryAsync(query).Result;

            return dbConnection.ResultAsDataTable(result);
        }

        public static void DisconnectFromDatabase(DbConnection dbConnection,
                                                  string       session)
        {
            dbConnection.DisconnectAsync(session);
        }

        #endregion
    }
}