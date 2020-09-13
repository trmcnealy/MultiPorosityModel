using System;
using System.Reactive;
using System.Reactive.Linq;

using ReactiveUI;

namespace MultiPorosity.Tool
{
    public class MultiPorosityResultsViewModel : ReactiveObject
    {
        private double _MatrixPerm;

        private double _HydralicFracturePerm;

        private double _NaturalFracturePerm;

        private double _HydralicFractureHalfLength;

        private double _HydralicFractureSpacing;

        private double _NaturalFractureSpacing;

        public double MatrixPerm { get { return _MatrixPerm; } set { this.RaiseAndSetIfChanged(ref _MatrixPerm, value); } }

        public double HydralicFracturePerm { get { return _HydralicFracturePerm; } set { this.RaiseAndSetIfChanged(ref _HydralicFracturePerm, value); } }

        public double NaturalFracturePerm { get { return _NaturalFracturePerm; } set { this.RaiseAndSetIfChanged(ref _NaturalFracturePerm, value); } }

        public double HydralicFractureHalfLength { get { return _HydralicFractureHalfLength; } set { this.RaiseAndSetIfChanged(ref _HydralicFractureHalfLength, value); } }

        public double HydralicFractureSpacing { get { return _HydralicFractureSpacing; } set { this.RaiseAndSetIfChanged(ref _HydralicFractureSpacing, value); } }

        public double NaturalFractureSpacing { get { return _NaturalFractureSpacing; } set { this.RaiseAndSetIfChanged(ref _NaturalFractureSpacing, value); } }

        public MultiPorosityResultsViewModel()
        {
        }
    }
}