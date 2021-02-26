namespace MultiPorosity.Models
{
    public sealed class ProductionSmoothing
    {
        public int NumberOfPoints { get; set; }

        public int Iterations { get; set; }

        public bool Normalized { get; set; }

        public ProductionSmoothing()
        {
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
    }
}