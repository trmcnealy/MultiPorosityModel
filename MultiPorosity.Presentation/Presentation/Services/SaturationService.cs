using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Engineering.DataSource.Tools;

using Kokkos;

using MultiPorosity.Models;
using MultiPorosity.Services;

namespace MultiPorosity.Presentation.Services
{
    public static class SaturationService
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void ReProportion(double old_gas, double new_gas, ref double oil, ref double water)
        {
            double delta = (new_gas - old_gas) / 2.0;

            oil -= delta;

            water -= delta;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void ReProportion(ref double gas, double old_oil, double new_oil, ref double water)
        {
            double delta = (new_oil - old_oil) / 2.0;

            gas -= delta;

            water -= delta;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void ReProportion(ref double gas, ref double oil, double old_water, double new_water)
        {
            double delta = (new_water - old_water) / 2.0;

            gas -= delta;

            oil -= delta;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool ReBalance(ref double gas, ref double oil, ref double water)
        {
            double sum = gas + oil + water;

            if(Math.Abs(sum - 1.0) < double.Epsilon)
            {
                return false;
            }

            gas /= sum;

            oil /= sum;

            water /= sum;

            return true;
        }
    }
}