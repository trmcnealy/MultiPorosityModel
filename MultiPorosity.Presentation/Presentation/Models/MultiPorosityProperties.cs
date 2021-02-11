using System.ComponentModel;
using System.Text.Json;

using Engineering.UI.Controls;

using MultiPorosity.Models;
using MultiPorosity.Presentation.Services;
using MultiPorosity.Services;

using Prism.Mvvm;

using PVT;

namespace MultiPorosity.Presentation.Models
{
    [CategoryOrder("Multi-Porosity Model Properties", 0)]
    [CategoryOrder("Pvt Model Properties",            1)]
    [CategoryOrder("Model Parameters",                2)]
    public class MultiPorosityProperties : BindableBase
    {
        private FractureProperties _fractureProperties;

        private NaturalFractureProperties _naturalFractureProperties;

        private ReservoirProperties _reservoirProperties;

        private WellProperties _wellProperties;

        private Pvt _pvt;

        private RelativePermeabilities _relativePermeabilities;

        private PvtModelProperties _pvtModelProperties;

        private MultiPorosityModelParameters _multiPorosityModelParameters;

        //public MultiPorosityData<double>? GetMultiPorosityData()
        //{
        //    if(Kokkos.KokkosLibrary.Initialized)
        //    {
        //        ReservoirProperties<double> reservoir = new(MultiPorosityModelService.ExecutionSpace);
        //        reservoir.Length                = ReservoirPropertiesLength;
        //        reservoir.Width                 = ReservoirPropertiesWidth;
        //        reservoir.Thickness             = ReservoirPropertiesThickness;
        //        reservoir.Porosity              = ReservoirPropertiesPorosity;
        //        reservoir.Permeability          = ReservoirPropertiesPermeability;
        //        reservoir.Compressibility       = ReservoirPropertiesCompressibility;
        //        reservoir.BottomholeTemperature = ReservoirPropertiesBottomholeTemperature;
        //        reservoir.InitialPressure       = ReservoirPropertiesInitialPressure;

        //        WellProperties<double> wellProperties = new(MultiPorosityModelService.ExecutionSpace);
        //        wellProperties.LateralLength      = 6500.0;
        //        wellProperties.BottomholePressure = 3500.0;

        //        FractureProperties<double> fracture = new(MultiPorosityModelService.ExecutionSpace);
        //        fracture.Count        = 60;
        //        fracture.Width        = 0.1 / 12.0;
        //        fracture.Height       = 150.0;
        //        fracture.HalfLength   = 348.0;
        //        fracture.Porosity     = 0.20;
        //        fracture.Permeability = 184.0;
        //        fracture.Skin         = 0.0;

        //        NaturalFractureProperties<double> natural_fracture = new(MultiPorosityModelService.ExecutionSpace);
        //        natural_fracture.Count        = 10;
        //        natural_fracture.Width        = 0.01 / 12.0;
        //        natural_fracture.Porosity     = 0.10;
        //        natural_fracture.Permeability = 0.8;

        //        Pvt<double> pvt = new(MultiPorosityModelService.ExecutionSpace);
        //        pvt.OilSaturation            = 0.8;
        //        pvt.OilApiGravity            = 46.80;
        //        pvt.OilViscosity             = 0.11;
        //        pvt.OilFormationVolumeFactor = 1.56;
        //        pvt.OilCompressibility       = 5.993058E-05;

        //        pvt.WaterSaturation            = 0.0;
        //        pvt.WaterSpecificGravity       = 1.0;
        //        pvt.WaterViscosity             = 1.0;
        //        pvt.WaterFormationVolumeFactor = 1.0;
        //        pvt.WaterCompressibility       = 1.0;

        //        pvt.GasSaturation            = 0.2;
        //        pvt.GasSpecificGravity       = 0.75;
        //        pvt.GasViscosity             = 0.0239;
        //        pvt.GasFormationVolumeFactor = 1.2610E-003;
        //        pvt.GasCompressibilityFactor = 1.5645;
        //        pvt.GasCompressibility       = 2.3418E-004;

        //        RelativePermeabilities<double> relativePermeabilities = new(MultiPorosityModelService.ExecutionSpace);
        //        relativePermeabilities.MatrixOil            = 0.5;
        //        relativePermeabilities.MatrixWater          = 0.0;
        //        relativePermeabilities.MatrixGas            = 0.15;
        //        relativePermeabilities.FractureOil          = 0.5;
        //        relativePermeabilities.FractureWater        = 0.0;
        //        relativePermeabilities.FractureGas          = 0.15;
        //        relativePermeabilities.NaturalFractureOil   = 0.5;
        //        relativePermeabilities.NaturalFractureWater = 0.0;
        //        relativePermeabilities.NaturalFractureGas   = 0.15;

        //        MultiPorosityData<double> mpd = new(MultiPorosityModelService.ExecutionSpace);
        //        mpd.ReservoirProperties       = reservoir;
        //        mpd.WellProperties            = wellProperties;
        //        mpd.FractureProperties        = fracture;
        //        mpd.NaturalFractureProperties = natural_fracture;
        //        mpd.Pvt                       = pvt;
        //        mpd.RelativePermeability      = relativePermeabilities;

        //        return mpd;
        //    }

        //    return null;
        //}

        [PropertyOrder(0)]
        [Category("Multi-Porosity Model Properties")]
        [ExpandableObject]
        [DisplayName("Well Properties")]
        public WellProperties WellProperties
        {
            get { return _wellProperties; }
            set
            {
                if(SetProperty(ref _wellProperties, value))
                {
                    _wellProperties.PropertyChanged -= OnPropertyChanged;
                    _wellProperties.PropertyChanged += OnPropertyChanged;
                }
            }
        }

        [PropertyOrder(1)]
        [Category("Multi-Porosity Model Properties")]
        [ExpandableObject]
        [DisplayName("Reservoir Properties")]
        public ReservoirProperties ReservoirProperties
        {
            get { return _reservoirProperties; }
            set
            {
                if(SetProperty(ref _reservoirProperties, value))
                {
                    _reservoirProperties.PropertyChanged -= OnPropertyChanged;
                    _reservoirProperties.PropertyChanged += OnPropertyChanged;
                }
            }
        }

        [PropertyOrder(2)]
        [Category("Multi-Porosity Model Properties")]
        [ExpandableObject]
        [DisplayName("Fracture Properties")]
        public FractureProperties FractureProperties
        {
            get { return _fractureProperties; }
            set
            {
                if(SetProperty(ref _fractureProperties, value))
                {
                    _fractureProperties.PropertyChanged -= OnPropertyChanged;
                    _fractureProperties.PropertyChanged += OnPropertyChanged;
                }
            }
        }

        [PropertyOrder(3)]
        [Category("Multi-Porosity Model Properties")]
        [ExpandableObject]
        [DisplayName("Natural Fracture Properties")]
        public NaturalFractureProperties NaturalFractureProperties
        {
            get { return _naturalFractureProperties; }
            set
            {
                if(SetProperty(ref _naturalFractureProperties, value))
                {
                    _naturalFractureProperties.PropertyChanged -= OnPropertyChanged;
                    _naturalFractureProperties.PropertyChanged += OnPropertyChanged;
                }
            }
        }

        [PropertyOrder(4)]
        [Category("Multi-Porosity Model Properties")]
        [ExpandableObject]
        [DisplayName("Pvt")]
        public Pvt Pvt
        {
            get { return _pvt; }
            set
            {
                if(SetProperty(ref _pvt, value))
                {
                    _pvt.PropertyChanged -= OnPropertyChanged;
                    _pvt.PropertyChanged += OnPropertyChanged;
                }
            }
        }

        [PropertyOrder(5)]
        [Category("Multi-Porosity Model Properties")]
        [ExpandableObject]
        [DisplayName("Relative Permeabilities")]
        public RelativePermeabilities RelativePermeabilities
        {
            get { return _relativePermeabilities; }
            set
            {
                if(SetProperty(ref _relativePermeabilities, value))
                {
                    _relativePermeabilities.PropertyChanged -= OnPropertyChanged;
                    _relativePermeabilities.PropertyChanged += OnPropertyChanged;
                }
            }
        }

        [PropertyOrder(6)]
        [Category("Pvt Model Properties")]
        [ExpandableObject]
        [DisplayName("Pvt Model")]
        public PvtModelProperties PvtModelProperties
        {
            get { return _pvtModelProperties; }
            set
            {
                if(SetProperty(ref _pvtModelProperties, value))
                {
                    _pvtModelProperties.PropertyChanged -= OnPropertyChanged;
                    _pvtModelProperties.PropertyChanged += OnPropertyChanged;
                }
            }
        }

        [PropertyOrder(7)]
        [Category("Model Parameters")]
        [ExpandableObject]
        [DisplayName("Multi-Porosity Model Parameters")]
        public MultiPorosityModelParameters MultiPorosityModelParameters
        {
            get { return _multiPorosityModelParameters; }
            set
            {
                if(SetProperty(ref _multiPorosityModelParameters, value))
                {
                    _multiPorosityModelParameters.PropertyChanged -= OnPropertyChanged;
                    _multiPorosityModelParameters.PropertyChanged += OnPropertyChanged;
                }
            }
        }

        public MultiPorosityProperties(MultiPorosity.Services.Models.MultiPorosityProperties      multiPorosityProperties,
                                       MultiPorosity.Services.Models.PvtModelProperties           pvtModelProperties,
                                       MultiPorosity.Services.Models.MultiPorosityModelParameters multiPorosityModelParameters)
        {
            FractureProperties        = new(multiPorosityProperties.FractureProperties);
            NaturalFractureProperties = new(multiPorosityProperties.NaturalFractureProperties);
            ReservoirProperties       = new(multiPorosityProperties.ReservoirProperties);
            WellProperties            = new(multiPorosityProperties.WellProperties);
            Pvt                       = new(multiPorosityProperties.Pvt);
            RelativePermeabilities    = new(multiPorosityProperties.RelativePermeabilities);

            PvtModelProperties           = new(pvtModelProperties);
            MultiPorosityModelParameters = new(multiPorosityModelParameters);
        }

        public static implicit operator (MultiPorosity.Services.Models.MultiPorosityProperties multiPorosityProperties, MultiPorosity.Services.Models.PvtModelProperties pvtModelProperties,
            MultiPorosity.Services.Models.MultiPorosityModelParameters multiPorosityModelParameters)(MultiPorosityProperties multiPorosityProperties)
        {
            return
                (new(multiPorosityProperties._fractureProperties, multiPorosityProperties._naturalFractureProperties, multiPorosityProperties._reservoirProperties, multiPorosityProperties._wellProperties, multiPorosityProperties._pvt, multiPorosityProperties._relativePermeabilities),
                 multiPorosityProperties._pvtModelProperties, multiPorosityProperties._multiPorosityModelParameters);
        }
        
        private void OnPropertyChanged(object?                   sender,
                                       PropertyChangedEventArgs? e)
        {
            switch (sender)
            {
                case WellProperties _:
                {
                    RaisePropertyChanged(nameof(WellProperties));
                    break;
                }
                case ReservoirProperties _:
                {
                    RaisePropertyChanged(nameof(ReservoirProperties));
                    break;
                }
                case FractureProperties _:
                {
                    RaisePropertyChanged(nameof(FractureProperties));
                    break;
                }
                case NaturalFractureProperties _:
                {
                    RaisePropertyChanged(nameof(NaturalFractureProperties));
                    break;
                }
                case Pvt _:
                {
                    RaisePropertyChanged(nameof(Pvt));
                    break;
                }
                case PvtModelProperties _:
                {
                    RaisePropertyChanged(nameof(PvtModelProperties));
                    break;
                }
                case RelativePermeabilities _:
                {
                    RaisePropertyChanged(nameof(RelativePermeabilities));
                    break;
                }
                case MultiPorosityModelParameters _:
                {
                    RaisePropertyChanged(nameof(MultiPorosityModelParameters));
                    break;
                }
            }
            

        }
    }
}