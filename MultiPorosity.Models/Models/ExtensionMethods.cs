namespace MultiPorosity.Models
{
    public static class ExtensionMethods
    {
        public static float Area(this ReservoirProperties<float> reservoirProperties)
        {
            return (reservoirProperties.Length * reservoirProperties.Width) / 43560.0f;
        }

        public static double Area(this ReservoirProperties<double> reservoirProperties)
        {
            return (reservoirProperties.Length * reservoirProperties.Width) / 43560.0;
        }


    }
}