using Prism.Mvvm;

namespace MultiPorosity.Presentation.Models
{
    public sealed class ProductionSmoothing : BindableBase
    {
        private int _numberOfPoints;

        public int NumberOfPoints
        {
            get { return _numberOfPoints; }
            set { this.SetProperty(ref _numberOfPoints, value); }
        }

        private int _iterations;

        public int Iterations
        {
            get { return _iterations; }
            set { this.SetProperty(ref _iterations, value); }
        }

        private bool _normalized;

        public bool Normalized
        {
            get { return _normalized; }
            set { this.SetProperty(ref _normalized, value); }
        }

        public ProductionSmoothing(int m)
        {
            NumberOfPoints = m;
            Iterations     = 3;
            Normalized     = false;
        }

        public ProductionSmoothing(int m,
                                   int k)
        {
            NumberOfPoints = m;
            Iterations     = k;
            Normalized     = false;
        }

        public ProductionSmoothing(int  m,
                                   int  k,
                                   bool normalized)
        {
            NumberOfPoints = m;
            Iterations     = k;
            Normalized     = normalized;
        }
        
        public static explicit operator ProductionSmoothing(MultiPorosity.Services.Models.ProductionSmoothing productionSmoothing)
        {
            return new(productionSmoothing.NumberOfPoints, productionSmoothing.Iterations, productionSmoothing.Normalized);
        }
        
        public static implicit operator MultiPorosity.Services.Models.ProductionSmoothing(ProductionSmoothing productionSmoothing)
        {
            return new(productionSmoothing.NumberOfPoints, productionSmoothing.Iterations, productionSmoothing.Normalized);
        }
    }
}