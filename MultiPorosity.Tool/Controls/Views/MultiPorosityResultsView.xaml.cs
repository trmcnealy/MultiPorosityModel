#nullable enable

using System.Windows;

using ReactiveUI;

using Splat;

namespace MultiPorosity.Tool
{
    public partial class MultiPorosityResultsView
    {
        public MultiPorosityResultsView()
        {
            InitializeComponent();

            ViewModel = (MultiPorosityResultsViewModel)Locator.Current.GetService(typeof(MultiPorosityResultsViewModel));

            DataContext = this;
        }
    }
}