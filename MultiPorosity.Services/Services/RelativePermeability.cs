using System;
using System.Diagnostics;

using Kokkos;

namespace MultiPorosity.Services
{
    public static class RelativePermeabilityTableIndex
    {
        public const ulong Sg  = 0ul;
        public const ulong So  = 1ul;
        public const ulong Sw  = 2ul;
        public const ulong Krg = 3ul;
        public const ulong Kro = 4ul;
        public const ulong Krw = 5ul;
    }

    public static class RelativePermeability
    {
        public static (float permeability_relative_gas, float permeability_relative_oil, float permeability_relative_water) StoneII(in float saturation_gas,
                                                                                                                                    in float saturation_oil,
                                                                                                                                    in float saturation_water,
                                                                                                                                    in float saturation_water_connate,
                                                                                                                                    in float saturation_water_critical,
                                                                                                                                    in float saturation_oil_irreducible_water,
                                                                                                                                    in float saturation_oil_residual_water,
                                                                                                                                    in float saturation_oil_irreducible_gas,
                                                                                                                                    in float saturation_oil_residual_gas,
                                                                                                                                    in float saturation_gas_connate,
                                                                                                                                    in float saturation_gas_critical,
                                                                                                                                    in float permeability_relative_water_oil_irreducible,
                                                                                                                                    in float permeability_relative_oil_water_connate,
                                                                                                                                    in float permeability_relative_gas_liquid_connate,
                                                                                                                                    in float exponent_permeability_relative_water,
                                                                                                                                    in float exponent_permeability_relative_oil_water,
                                                                                                                                    in float exponent_permeability_relative_gas,
                                                                                                                                    in float exponent_permeability_relative_oil_gas)
        {
            MultiPorosityLibrary.RelativePermeabilityStoneIISingle(saturation_gas,
                                                                   saturation_oil,
                                                                   saturation_water,
                                                                   saturation_water_connate,
                                                                   saturation_water_critical,
                                                                   saturation_oil_irreducible_water,
                                                                   saturation_oil_residual_water,
                                                                   saturation_oil_irreducible_gas,
                                                                   saturation_oil_residual_gas,
                                                                   saturation_gas_connate,
                                                                   saturation_gas_critical,
                                                                   permeability_relative_water_oil_irreducible,
                                                                   permeability_relative_oil_water_connate,
                                                                   permeability_relative_gas_liquid_connate,
                                                                   exponent_permeability_relative_water,
                                                                   exponent_permeability_relative_oil_water,
                                                                   exponent_permeability_relative_gas,
                                                                   exponent_permeability_relative_oil_gas,
                                                                   out float permeability_relative_gas,
                                                                   out float permeability_relative_oil,
                                                                   out float permeability_relative_water);

            return (permeability_relative_gas, permeability_relative_oil, permeability_relative_water);
        }

        public static (double permeability_relative_gas, double permeability_relative_oil, double permeability_relative_water) StoneII(in double saturation_gas,
            in                                                                                                                             double saturation_oil,
            in                                                                                                                             double saturation_water,
            in                                                                                                                             double saturation_water_connate,
            in                                                                                                                             double saturation_water_critical,
            in                                                                                                                             double saturation_oil_irreducible_water,
            in                                                                                                                             double saturation_oil_residual_water,
            in                                                                                                                             double saturation_oil_irreducible_gas,
            in                                                                                                                             double saturation_oil_residual_gas,
            in                                                                                                                             double saturation_gas_connate,
            in                                                                                                                             double saturation_gas_critical,
            in                                                                                                                             double permeability_relative_water_oil_irreducible,
            in                                                                                                                             double permeability_relative_oil_water_connate,
            in                                                                                                                             double permeability_relative_gas_liquid_connate,
            in                                                                                                                             double exponent_permeability_relative_water,
            in                                                                                                                             double exponent_permeability_relative_oil_water,
            in                                                                                                                             double exponent_permeability_relative_gas,
            in                                                                                                                             double exponent_permeability_relative_oil_gas)
        {
            MultiPorosityLibrary.RelativePermeabilityStoneIIDouble(saturation_gas,
                                                                   saturation_oil,
                                                                   saturation_water,
                                                                   saturation_water_connate,
                                                                   saturation_water_critical,
                                                                   saturation_oil_irreducible_water,
                                                                   saturation_oil_residual_water,
                                                                   saturation_oil_irreducible_gas,
                                                                   saturation_oil_residual_gas,
                                                                   saturation_gas_connate,
                                                                   saturation_gas_critical,
                                                                   permeability_relative_water_oil_irreducible,
                                                                   permeability_relative_oil_water_connate,
                                                                   permeability_relative_gas_liquid_connate,
                                                                   exponent_permeability_relative_water,
                                                                   exponent_permeability_relative_oil_water,
                                                                   exponent_permeability_relative_gas,
                                                                   exponent_permeability_relative_oil_gas,
                                                                   out double permeability_relative_gas,
                                                                   out double permeability_relative_oil,
                                                                   out double permeability_relative_water);

            return (permeability_relative_gas, permeability_relative_oil, permeability_relative_water);
        }

        public static View<float, TExecutionSpace> StoneIITable<TExecutionSpace>(int      increments,
                                                                                 in float saturation_water_connate,
                                                                                 in float saturation_water_critical,
                                                                                 in float saturation_oil_irreducible_water,
                                                                                 in float saturation_oil_residual_water,
                                                                                 in float saturation_oil_irreducible_gas,
                                                                                 in float saturation_oil_residual_gas,
                                                                                 in float saturation_gas_connate,
                                                                                 in float saturation_gas_critical,
                                                                                 in float permeability_relative_water_oil_irreducible,
                                                                                 in float permeability_relative_oil_water_connate,
                                                                                 in float permeability_relative_gas_liquid_connate,
                                                                                 in float exponent_permeability_relative_water,
                                                                                 in float exponent_permeability_relative_oil_water,
                                                                                 in float exponent_permeability_relative_gas,
                                                                                 in float exponent_permeability_relative_oil_gas)
            where TExecutionSpace : IExecutionSpace, new()
        {
            unsafe
            {
                View<float, TExecutionSpace> view = new View<float, TExecutionSpace>("RelativePermeability", increments, increments, 6);

                ExecutionSpaceKind executionSpace = ExecutionSpace<TExecutionSpace>.GetKind();

                MultiPorosityLibrary.RelativePermeabilityTableStoneIISingle(view.Pointer,
                                                                            executionSpace,
                                                                            saturation_water_connate,
                                                                            saturation_water_critical,
                                                                            saturation_oil_irreducible_water,
                                                                            saturation_oil_residual_water,
                                                                            saturation_oil_irreducible_gas,
                                                                            saturation_oil_residual_gas,
                                                                            saturation_gas_connate,
                                                                            saturation_gas_critical,
                                                                            permeability_relative_water_oil_irreducible,
                                                                            permeability_relative_oil_water_connate,
                                                                            permeability_relative_gas_liquid_connate,
                                                                            exponent_permeability_relative_water,
                                                                            exponent_permeability_relative_oil_water,
                                                                            exponent_permeability_relative_gas,
                                                                            exponent_permeability_relative_oil_gas);

                return view;
            }
        }

        public static View<double, TExecutionSpace> StoneIITable<TExecutionSpace>(int       increments,
                                                                                  in double saturation_water_connate,
                                                                                  in double saturation_water_critical,
                                                                                  in double saturation_oil_irreducible_water,
                                                                                  in double saturation_oil_residual_water,
                                                                                  in double saturation_oil_irreducible_gas,
                                                                                  in double saturation_oil_residual_gas,
                                                                                  in double saturation_gas_connate,
                                                                                  in double saturation_gas_critical,
                                                                                  in double permeability_relative_water_oil_irreducible,
                                                                                  in double permeability_relative_oil_water_connate,
                                                                                  in double permeability_relative_gas_liquid_connate,
                                                                                  in double exponent_permeability_relative_water,
                                                                                  in double exponent_permeability_relative_oil_water,
                                                                                  in double exponent_permeability_relative_gas,
                                                                                  in double exponent_permeability_relative_oil_gas)
            where TExecutionSpace : IExecutionSpace, new()
        {
            unsafe
            {
                View<double, TExecutionSpace> view = new View<double, TExecutionSpace>("RelativePermeability", increments, increments, 6);

                ExecutionSpaceKind executionSpace = ExecutionSpace<TExecutionSpace>.GetKind();

                MultiPorosityLibrary.RelativePermeabilityTableStoneIIDouble(view.Pointer,
                                                                            executionSpace,
                                                                            saturation_water_connate,
                                                                            saturation_water_critical,
                                                                            saturation_oil_irreducible_water,
                                                                            saturation_oil_residual_water,
                                                                            saturation_oil_irreducible_gas,
                                                                            saturation_oil_residual_gas,
                                                                            saturation_gas_connate,
                                                                            saturation_gas_critical,
                                                                            permeability_relative_water_oil_irreducible,
                                                                            permeability_relative_oil_water_connate,
                                                                            permeability_relative_gas_liquid_connate,
                                                                            exponent_permeability_relative_water,
                                                                            exponent_permeability_relative_oil_water,
                                                                            exponent_permeability_relative_gas,
                                                                            exponent_permeability_relative_oil_gas);

                return view;
            }
        }
    }
}