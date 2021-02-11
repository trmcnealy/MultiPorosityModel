
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

using Engineering.DataSource.Tools;

using MultiPorosity.Models;
using MultiPorosity.Services.Models;

namespace MultiPorosity.Presentation
{
    public partial class ProductionHistoryView
    {
        public ProductionHistoryView()
        {
            InitializeComponent();

            if(DataContext is ProductionHistoryViewModel viewModel)
            {
                viewModel.SetDataGrid(HistoryViewDataGrid);
            }
        }

        private static readonly Key[] OverrideKeys = new Key[]
        {
            Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G,
            Key.H, Key.I, Key.J, Key.K, Key.L, Key.M, Key.N,
            Key.O, Key.P, Key.Q, Key.R, Key.S, Key.T, Key.U,
            Key.V, Key.W, Key.X, Key.Y, Key.Z,
            Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5,
            Key.D6, Key.D7, Key.D8, Key.D9,
            Key.NumPad0, Key.NumPad1, Key.NumPad2, Key.NumPad3,
            Key.NumPad4, Key.NumPad5, Key.NumPad6, Key.NumPad7,
            Key.NumPad8, Key.NumPad9, Key.Subtract, Key.Divide
        };

        private static string getKeyString(Key key)
        {
            if (key == Key.None)
            {
                return string.Empty;
            }
 
            if (key >= Key.D0 && key <= Key.D9)
            {
                return char.ToString((char)(key - Key.D0 + '0'));
            }
 
            if (key >= Key.NumPad0 && key <= Key.NumPad9)
            {
                return char.ToString((char)(key - Key.NumPad0 + '0'));
            }
 
            if (key >= Key.A && key <= Key.Z)
            {
                return char.ToString((char)(key - Key.A + 'A'));
            }
            
            if(key == Key.Subtract)
            {
                return "-";
            }

            if(key == Key.Divide)
            {
                return "/";
            }

            return string.Empty;
        }

        private void HistoryViewDataGrid_OnItemOverride(object       sender,
                                                        KeyEventArgs e)
        {
            if(sender is DataGrid dataGrid &&
               e.KeyboardDevice != null &&
               e.KeyboardDevice.Modifiers == ModifierKeys.None &&
               OverrideKeys.Any(k => k == e.Key))
            {
                if(dataGrid.CurrentItem != null && dataGrid.CurrentItem.ToString() == "{NewItemPlaceholder}")
                {
                    if(dataGrid.BeginEdit())
                    {
                        if (dataGrid.CurrentCell.Item is DataRowView dataGridRow)
                        {
                            dataGridRow.BeginEdit();
                            //dataGridRow[dataGrid.CurrentCell.Column.DisplayIndex] = getKeyString(e.Key);
                        }
                    }

                    //if (dataGrid.CurrentItem is DataRowView dataGridRow)
                    //{
                    //    dataGridRow[dataGrid.CurrentCell.Column.DisplayIndex] = getKeyString(e.Key);
                    //}

                    //dataGrid.CurrentItem = getKeyString(e.Key);
                    e.Handled            = true;
                }
            }

        }
        
        private void HistoryViewDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Delete && sender is DataGrid dataGrid && DataContext is ProductionHistoryViewModel viewModel)
            {
                List<int> toRemove = new();

                DataRowView dataRowView;

                for (int i = 0; i < dataGrid.SelectedItems.Count; ++i)
                {
                    dataRowView = dataGrid.SelectedItems[i] as DataRowView;

                    toRemove.Add((int)dataRowView[0]);
                }

                ProductionDataSet dataSet           = viewModel._multiPorosityModelService.ActiveProject.ProductionDataSet;
                ProductionDataSet productionDataSet = new();

                foreach (int i in Enumerable.Range(0, dataSet.Actual.Rows.Count).Except(toRemove))
                {
                    productionDataSet.Actual.AddActualRow(dataSet.Actual[i].Index,
                                                          dataSet.Actual[i].Date,
                                                          dataSet.Actual[i].Days,
                                                          dataSet.Actual[i].Gas,
                                                          dataSet.Actual[i].Oil,
                                                          dataSet.Actual[i].Water,
                                                          dataSet.Actual[i].WellheadPressure,
                                                          dataSet.Actual[i].Weight);
                }
                
                for (int i = 0; i < productionDataSet.Actual.Rows.Count; ++i)
                {
                    productionDataSet.Actual[i].Index = i;
                }

                viewModel._multiPorosityModelService.ActiveProject.ProductionDataSet = productionDataSet;

                e.Handled = true;
            }
        }
    }
}