using System;

namespace MultiPorosity.Models
{
    public static class ExtensionMethods
    {
        public static float Area(this ReservoirProperties<float> reservoirProperties)
        {
            return reservoirProperties.Length * reservoirProperties.Width / 43560.0f;
        }

        public static double Area(this ReservoirProperties<double> reservoirProperties)
        {
            return reservoirProperties.Length * reservoirProperties.Width / 43560.0;
        }





        
        public static float RockCompressibility_HallCorrelation(in float porosity)
        {
            return 10E-6f * 1.782f / MathF.Pow(porosity, 0.438f);
        }

        public static double RockCompressibility_HallCorrelation(in double porosity)
        {
            return 10E-6 * 1.782 / Math.Pow(porosity, 0.438);
        }




















        public static float TotalCompressibility(in float saturation_gas,
                                                 in float saturation_oil,
                                                 in float compressibility_gas,
                                                 in float compressibility_oil,
                                                 in float compressibility_rock)
        {
            float gasPart = saturation_gas * compressibility_gas;
            float oilPart = saturation_oil * compressibility_oil;

            return gasPart + oilPart + compressibility_rock;
        }

        public static double TotalCompressibility(in double saturation_gas,
                                                  in double saturation_oil,
                                                  in double compressibility_gas,
                                                  in double compressibility_oil,
                                                  in double compressibility_rock)
        {
            double gasPart = saturation_gas * compressibility_gas;
            double oilPart = saturation_oil * compressibility_oil;

            return gasPart + oilPart + compressibility_rock;
        }
    }
}