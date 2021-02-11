using Engineering.UI;

using Prism.Mvvm;

namespace MultiPorosity.Presentation.Models
{
    public class MultiPorosityHistoryMatchParameters : BindableBase
    {
        private Range<double> _matrixPermeability;
        private Range<double> _hydraulicFracturePermeability;
        private Range<double> _naturalFracturePermeability;
        private Range<double> _hydraulicFractureHalfLength;
        private Range<double> _hydraulicFractureSpacing;
        private Range<double> _naturalFractureSpacing;
        private Range<double> _skin;

        public Range<double> MatrixPermeability
        {
            get { return _matrixPermeability; }
            set
            {
                if(SetProperty(ref _matrixPermeability, value))
                {
                }
            }
        }
        
        public Range<double> HydraulicFracturePermeability
        {
            get { return _hydraulicFracturePermeability; }
            set
            {
                if(SetProperty(ref _hydraulicFracturePermeability, value))
                {
                }
            }
        }
        
        public Range<double> NaturalFracturePermeability
        {
            get { return _naturalFracturePermeability; }
            set
            {
                if(SetProperty(ref _naturalFracturePermeability, value))
                {
                }
            }
        }
        
        public Range<double> HydraulicFractureHalfLength
        {
            get { return _hydraulicFractureHalfLength; }
            set
            {
                if(SetProperty(ref _hydraulicFractureHalfLength, value))
                {
                }
            }
        }
        
        public Range<double> HydraulicFractureSpacing
        {
            get { return _hydraulicFractureSpacing; }
            set
            {
                if(SetProperty(ref _hydraulicFractureSpacing, value))
                {
                }
            }
        }
        
        public Range<double> NaturalFractureSpacing
        {
            get { return _naturalFractureSpacing; }
            set
            {
                if(SetProperty(ref _naturalFractureSpacing, value))
                {
                }
            }
        }
        
        public Range<double> Skin
        {
            get { return _skin; }
            set
            {
                if(SetProperty(ref _skin, value))
                {
                }
            }
        }
        
        public MultiPorosityHistoryMatchParameters(MultiPorosity.Services.Models.MultiPorosityHistoryMatchParameters multiPorosityHistoryMatchParameters)
        {
            _matrixPermeability           = new Range<double>(multiPorosityHistoryMatchParameters.MatrixPermeability.Lower, multiPorosityHistoryMatchParameters.MatrixPermeability.Upper);
            _hydraulicFracturePermeability = new Range<double>(multiPorosityHistoryMatchParameters.HydraulicFracturePermeability.Lower, multiPorosityHistoryMatchParameters.HydraulicFracturePermeability.Upper);
            _naturalFracturePermeability  = new Range<double>(multiPorosityHistoryMatchParameters.NaturalFracturePermeability.Lower, multiPorosityHistoryMatchParameters.NaturalFracturePermeability.Upper);
            _hydraulicFractureHalfLength   = new Range<double>(multiPorosityHistoryMatchParameters.HydraulicFractureHalfLength.Lower, multiPorosityHistoryMatchParameters.HydraulicFractureHalfLength.Upper);
            _hydraulicFractureSpacing      = new Range<double>(multiPorosityHistoryMatchParameters.HydraulicFractureSpacing.Lower, multiPorosityHistoryMatchParameters.HydraulicFractureSpacing.Upper);
            _naturalFractureSpacing       = new Range<double>(multiPorosityHistoryMatchParameters.NaturalFractureSpacing.Lower, multiPorosityHistoryMatchParameters.NaturalFractureSpacing.Upper);
            _skin                         = new Range<double>(multiPorosityHistoryMatchParameters.Skin.Lower, multiPorosityHistoryMatchParameters.Skin.Upper);
        }
        
        public static implicit operator MultiPorosity.Services.Models.MultiPorosityHistoryMatchParameters(MultiPorosityHistoryMatchParameters multiPorosityHistoryMatchParameters)
        {
            return new(new Engineering.DataSource.Range<double>(multiPorosityHistoryMatchParameters.MatrixPermeability.Lower, multiPorosityHistoryMatchParameters.MatrixPermeability.Upper),
                       new Engineering.DataSource.Range<double>(multiPorosityHistoryMatchParameters.HydraulicFracturePermeability.Lower, multiPorosityHistoryMatchParameters.HydraulicFracturePermeability.Upper),
                       new Engineering.DataSource.Range<double>(multiPorosityHistoryMatchParameters.NaturalFracturePermeability.Lower, multiPorosityHistoryMatchParameters.NaturalFracturePermeability.Upper),
                       new Engineering.DataSource.Range<double>(multiPorosityHistoryMatchParameters.HydraulicFractureHalfLength.Lower, multiPorosityHistoryMatchParameters.HydraulicFractureHalfLength.Upper),
                       new Engineering.DataSource.Range<double>(multiPorosityHistoryMatchParameters.HydraulicFractureSpacing.Lower, multiPorosityHistoryMatchParameters.HydraulicFractureSpacing.Upper),
                       new Engineering.DataSource.Range<double>(multiPorosityHistoryMatchParameters.NaturalFractureSpacing.Lower, multiPorosityHistoryMatchParameters.NaturalFractureSpacing.Upper),
                       new Engineering.DataSource.Range<double>(multiPorosityHistoryMatchParameters.Skin.Lower, multiPorosityHistoryMatchParameters.Skin.Upper));
        }
    }
}