#nullable enable

using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

using ReactiveUI;

using Splat;

namespace MultiPorosity.Tool
{
    public partial class MultiPorositySettingsView
    {
        //private static double GetValue(string text)
        //{
        //    if(double.TryParse(text, out double result))
        //    {
        //        return result;
        //    }
        //    return 0.0;
        //}

        public MultiPorositySettingsView()
        {
            InitializeComponent();

            ViewModel = (MultiPorositySettingsViewModel)Locator.Current.GetService(typeof(MultiPorositySettingsViewModel));

            DataContext = this;
        }

        private readonly Regex regex = new Regex("[^0-9/./-]+");

        private void NumberValidationTextBox(object                   sender,
                                             TextCompositionEventArgs e)
        {
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}