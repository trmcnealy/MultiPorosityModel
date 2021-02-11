using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

using Prism.Mvvm;

namespace MultiPorosity.Presentation
{
    public partial class MultiPorosityHistoryMatchParametersView
    {
        private readonly Regex regex = new Regex("[^0-9/./-]+");

        public MultiPorosityHistoryMatchParametersView()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object                   sender,
                                             TextCompositionEventArgs e)
        {
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}