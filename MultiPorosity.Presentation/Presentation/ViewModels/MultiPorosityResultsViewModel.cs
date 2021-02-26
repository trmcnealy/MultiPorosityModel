using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Win32;

using MultiPorosity.Presentation.Services;
using MultiPorosity.Services;

using Prism.Commands;
using Prism.Mvvm;

namespace MultiPorosity.Presentation
{
    public class MultiPorosityResultsViewModel : BindableBase
    {
        private double _MatrixPermeability;
        private double _HydraulicFracturePermeability;
        private double _NaturalFracturePermeability;
        private double _HydraulicFractureHalfLength;
        private double _HydraulicFractureSpacing;
        private double _NaturalFractureSpacing;
        private double _Skin;

        public double MatrixPermeability
        {
            get { return _MatrixPermeability; }
            set { SetProperty(ref _MatrixPermeability, value); }
        }

        public double HydraulicFracturePermeability
        {
            get { return _HydraulicFracturePermeability; }
            set { SetProperty(ref _HydraulicFracturePermeability, value); }
        }

        public double NaturalFracturePermeability
        {
            get { return _NaturalFracturePermeability; }
            set { SetProperty(ref _NaturalFracturePermeability, value); }
        }

        public double HydraulicFractureHalfLength
        {
            get { return _HydraulicFractureHalfLength; }
            set { SetProperty(ref _HydraulicFractureHalfLength, value); }
        }

        public double HydraulicFractureSpacing
        {
            get { return _HydraulicFractureSpacing; }
            set { SetProperty(ref _HydraulicFractureSpacing, value); }
        }

        public double NaturalFractureSpacing
        {
            get { return _NaturalFractureSpacing; }
            set { SetProperty(ref _NaturalFractureSpacing, value); }
        }

        public double Skin
        {
            get { return _Skin; }
            set { SetProperty(ref _Skin, value); }
        }

        public DelegateCommand ExportCachedResultsCommand { get; }

        public DelegateCommand CopyResultsCommand { get; }

        private readonly MultiPorosityModelService _multiPorosityModelService;

        public MultiPorosityResultsViewModel(MultiPorosityModelService multiPorosityModelService)
        {
            _multiPorosityModelService = multiPorosityModelService;

            ExportCachedResultsCommand = new DelegateCommand(OnExportCachedResults);
            CopyResultsCommand         = new DelegateCommand(OnCopyResults);

            _multiPorosityModelService.PropertyChanged -= OnPropertyChanged;
            _multiPorosityModelService.PropertyChanged += OnPropertyChanged;

            OnPropertyChanged(this, new PropertyChangedEventArgs("ActiveProject"));

            MatrixPermeability            = _multiPorosityModelService.ActiveProject.MultiPorosityModelResults.MatrixPermeability;
            HydraulicFracturePermeability = _multiPorosityModelService.ActiveProject.MultiPorosityModelResults.HydraulicFracturePermeability;
            NaturalFracturePermeability   = _multiPorosityModelService.ActiveProject.MultiPorosityModelResults.NaturalFracturePermeability;
            HydraulicFractureHalfLength   = _multiPorosityModelService.ActiveProject.MultiPorosityModelResults.HydraulicFractureHalfLength;
            HydraulicFractureSpacing      = _multiPorosityModelService.ActiveProject.MultiPorosityModelResults.HydraulicFractureSpacing;
            NaturalFractureSpacing        = _multiPorosityModelService.ActiveProject.MultiPorosityModelResults.NaturalFractureSpacing;
            Skin                          = _multiPorosityModelService.ActiveProject.MultiPorosityModelResults.Skin;
        }

        private async void OnExportCachedResults()
        {
            if(_multiPorosityModelService.TriplePorosityOptimizationResults.Count == 0)
            {
                MessageBox.Show("The results were not cached.");

                return;
            }

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
                        await Task.Run(() =>
                                       {
                                           DataSources.ExportCsv(_multiPorosityModelService.TriplePorosityOptimizationResults.First().GetNames(),
                                                                 _multiPorosityModelService.TriplePorosityOptimizationResults,
                                                                 saveFileDialog.FileName);

                                           MessageBox.Show($"{saveFileDialog.FileName} saved.");
                                       }).ConfigureAwait(true);

                        break;
                    }
                    case ".xlsx":
                    {
                        await Task.Run(() =>
                                       {
                                           DataSources.ExporXlsx(_multiPorosityModelService.TriplePorosityOptimizationResults.First().GetNames(),
                                                                 _multiPorosityModelService.TriplePorosityOptimizationResults,
                                                                 saveFileDialog.FileName);

                                           MessageBox.Show($"{saveFileDialog.FileName} saved.");
                                       }).ConfigureAwait(true);

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

        private void OnCopyResults()
        {
            _multiPorosityModelService.ActiveProject.MultiPorosityProperties.MultiPorosityModelParameters.MatrixPermeability            = MatrixPermeability;
            _multiPorosityModelService.ActiveProject.MultiPorosityProperties.MultiPorosityModelParameters.HydraulicFracturePermeability = HydraulicFracturePermeability;
            _multiPorosityModelService.ActiveProject.MultiPorosityProperties.MultiPorosityModelParameters.NaturalFracturePermeability   = NaturalFracturePermeability;
            _multiPorosityModelService.ActiveProject.MultiPorosityProperties.MultiPorosityModelParameters.HydraulicFractureHalfLength   = HydraulicFractureHalfLength;
            _multiPorosityModelService.ActiveProject.MultiPorosityProperties.MultiPorosityModelParameters.HydraulicFractureSpacing      = HydraulicFractureSpacing;
            _multiPorosityModelService.ActiveProject.MultiPorosityProperties.MultiPorosityModelParameters.NaturalFractureSpacing        = NaturalFractureSpacing;
            _multiPorosityModelService.ActiveProject.MultiPorosityProperties.MultiPorosityModelParameters.Skin                          = Skin;
        }

        private void OnPropertyChanged(object?                   sender,
                                       PropertyChangedEventArgs? e)
        {
            switch(e.PropertyName)
            {
                case "ActiveProject":
                {
                    _multiPorosityModelService.ActiveProject.PropertyChanged -= OnPropertyChanged;
                    _multiPorosityModelService.ActiveProject.PropertyChanged += OnPropertyChanged;

                    _multiPorosityModelService.ActiveProject.MultiPorosityModelResults.PropertyChanged -= OnPropertyChanged;
                    _multiPorosityModelService.ActiveProject.MultiPorosityModelResults.PropertyChanged += OnPropertyChanged;

                    break;
                }
                case "MultiPorosityModelResults":
                {
                    MatrixPermeability            = _multiPorosityModelService.ActiveProject.MultiPorosityModelResults.MatrixPermeability;
                    HydraulicFracturePermeability = _multiPorosityModelService.ActiveProject.MultiPorosityModelResults.HydraulicFracturePermeability;
                    NaturalFracturePermeability   = _multiPorosityModelService.ActiveProject.MultiPorosityModelResults.NaturalFracturePermeability;
                    HydraulicFractureHalfLength   = _multiPorosityModelService.ActiveProject.MultiPorosityModelResults.HydraulicFractureHalfLength;
                    HydraulicFractureSpacing      = _multiPorosityModelService.ActiveProject.MultiPorosityModelResults.HydraulicFractureSpacing;
                    NaturalFractureSpacing        = _multiPorosityModelService.ActiveProject.MultiPorosityModelResults.NaturalFractureSpacing;
                    Skin                          = _multiPorosityModelService.ActiveProject.MultiPorosityModelResults.Skin;

                    break;
                }
            }
        }
    }
}