using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

using Engineering.UI.Collections;

using Microsoft.VisualBasic;

using Plotly;

namespace MultiPorosity.Presentation
{
    public partial class WellMapView
    {
        #region WellLocations Property

        public ObservableDictionary<string, (string type, object[] array)> WellLocations
        {
            get { return (ObservableDictionary<string, (string type, object[] array)>)GetValue(WellLocationsProperty); }
            set
            {
                SetValue(WellLocationsProperty, value);

                void WellLocations_CollectionChanged(object?                                                         sender,
                                                     System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
                {
                    DependencyPropertyChangedEventArgs args = new(WellLocationsProperty, e.OldItems, e.NewItems);

                    if(sender is DependencyObject dependencyObject)
                    {
                        OnWellLocationsPropertyChanged(dependencyObject, args);
                    }
                    else
                    {
                        OnWellLocationsPropertyChanged(this, args);
                    }
                }

                WellLocations.CollectionChanged -= WellLocations_CollectionChanged;
                WellLocations.CollectionChanged += WellLocations_CollectionChanged;
            }
        }

        public static readonly DependencyProperty WellLocationsProperty = DependencyProperty.Register("WellLocations",
                                                                                                      typeof(ObservableDictionary<string, (string type, object[] array)>),
                                                                                                      typeof(WellMapView),
                                                                                                      new FrameworkPropertyMetadata(null, OnWellLocationsPropertyChanged));

        private static void OnWellLocationsPropertyChanged(DependencyObject                   sender,
                                                           DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("OnWellLocationsPropertyChanged");

            if(sender is WellMapView {DataContext: WellMapViewModel wellMapViewModel} wellMapView && e.NewValue is ObservableDictionary<string, (string type, object[] array)> data_source)
            {
                wellMapViewModel.DataSource = data_source;

                void CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
                {
                    if(e.NewItems is BindableCollection<string> selectedWells)
                    {
                        wellMapView.SelectedWells = selectedWells;
                    }
                }

                void OnPropertyChanged(object? sender, PropertyChangedEventArgs? e)
                {
                    switch(e.PropertyName)
                    {
                        case "SelectedWells":
                        {
                            wellMapView.SelectedWells = wellMapViewModel.SelectedWells;

                            break;
                        }
                    }
                }
                wellMapViewModel.PropertyChanged -= OnPropertyChanged;
                wellMapViewModel.PropertyChanged += OnPropertyChanged;
                    
                wellMapViewModel.SelectedWells.CollectionChanged -= CollectionChanged;
                wellMapViewModel.SelectedWells.CollectionChanged += CollectionChanged;

            }
        }
        #endregion

        public BindableCollection<string> SelectedWells
        {
            get { return (BindableCollection<string>)GetValue(SelectedWellsProperty); }
            set { SetValue(SelectedWellsProperty, value); }
        }
        
        public static readonly DependencyProperty SelectedWellsProperty =
            DependencyProperty.Register("SelectedWells", typeof(BindableCollection<string>), typeof(WellMapView), new FrameworkPropertyMetadata(default));

        public WellMapView()
        {
            InitializeComponent();
        }
    }
}