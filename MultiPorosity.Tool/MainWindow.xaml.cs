using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

using ReactiveUI;

using SciChart.Charting.ChartModifiers;

using Splat;

namespace MultiPorosity.Tool
{
    public sealed partial class MainWindow : ReactiveWindow<MainViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();

            ViewModel = (MainViewModel)Locator.Current.GetService(typeof(IMainViewModel));

            if(ViewModel != null)
            {
                PointMarkersSelectionModifier.SelectionChanged       += ViewModel.PointMarkersSelectionModifier_SelectionChanged;
                PointMarkersLogLogSelectionModifier.SelectionChanged += ViewModel.PointMarkersSelectionModifier_SelectionChanged;
            }

            DataContext = this;
        }
    }
}