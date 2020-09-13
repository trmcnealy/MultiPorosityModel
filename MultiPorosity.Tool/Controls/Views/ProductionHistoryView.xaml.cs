#nullable enable

using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using ReactiveUI;

using Splat;

namespace MultiPorosity.Tool
{
    public partial class ProductionHistoryView
    {
        public ProductionHistoryView()
        {
            InitializeComponent();

            //Loaded += (sender, e) => MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

            ViewModel = (ProductionHistoryViewModel)Locator.Current.GetService(typeof(ProductionHistoryViewModel));


            ViewModel.HistoryView_DataGrid = HistoryViewDataGrid;

            DataContext = this;
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
    }
}