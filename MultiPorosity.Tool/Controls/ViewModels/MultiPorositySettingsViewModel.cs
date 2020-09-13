using System;
using System.Reactive;
using System.Reactive.Linq;

using ReactiveUI;

namespace MultiPorosity.Tool
{
    public class MultiPorositySettingsViewModel : ReactiveObject
    {
        private double _MatrixPermLower;

        private double _HydralicFracturePermLower;

        private double _NaturalFracturePermLower;

        private double _HydralicFractureHalfLengthLower;

        private double _HydralicFractureSpacingLower;

        private double _NaturalFractureSpacingLower;

        private double _MatrixPermUpper;

        private double _HydralicFracturePermUpper;

        private double _NaturalFracturePermUpper;

        private double _HydralicFractureHalfLengthUpper;

        private double _HydralicFractureSpacingUpper;

        private double _NaturalFractureSpacingUpper;

        public double MatrixPermLower { get { return _MatrixPermLower; } set { this.RaiseAndSetIfChanged(ref _MatrixPermLower, value); } }

        public double HydralicFracturePermLower { get { return _HydralicFracturePermLower; } set { this.RaiseAndSetIfChanged(ref _HydralicFracturePermLower, value); } }

        public double NaturalFracturePermLower { get { return _NaturalFracturePermLower; } set { this.RaiseAndSetIfChanged(ref _NaturalFracturePermLower, value); } }

        public double HydralicFractureHalfLengthLower { get { return _HydralicFractureHalfLengthLower; } set { this.RaiseAndSetIfChanged(ref _HydralicFractureHalfLengthLower, value); } }

        public double HydralicFractureSpacingLower { get { return _HydralicFractureSpacingLower; } set { this.RaiseAndSetIfChanged(ref _HydralicFractureSpacingLower, value); } }

        public double NaturalFractureSpacingLower { get { return _NaturalFractureSpacingLower; } set { this.RaiseAndSetIfChanged(ref _NaturalFractureSpacingLower, value); } }

        public double MatrixPermUpper { get { return _MatrixPermUpper; } set { this.RaiseAndSetIfChanged(ref _MatrixPermUpper, value); } }

        public double HydralicFracturePermUpper { get { return _HydralicFracturePermUpper; } set { this.RaiseAndSetIfChanged(ref _HydralicFracturePermUpper, value); } }

        public double NaturalFracturePermUpper { get { return _NaturalFracturePermUpper; } set { this.RaiseAndSetIfChanged(ref _NaturalFracturePermUpper, value); } }

        public double HydralicFractureHalfLengthUpper { get { return _HydralicFractureHalfLengthUpper; } set { this.RaiseAndSetIfChanged(ref _HydralicFractureHalfLengthUpper, value); } }

        public double HydralicFractureSpacingUpper { get { return _HydralicFractureSpacingUpper; } set { this.RaiseAndSetIfChanged(ref _HydralicFractureSpacingUpper, value); } }

        public double NaturalFractureSpacingUpper { get { return _NaturalFractureSpacingUpper; } set { this.RaiseAndSetIfChanged(ref _NaturalFractureSpacingUpper, value); } }

        public MultiPorositySettingsViewModel()
        {
        }
    }
}