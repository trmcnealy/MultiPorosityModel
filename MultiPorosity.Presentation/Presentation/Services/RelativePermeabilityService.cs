using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engineering.DataSource.Tools;

using Kokkos;

using MultiPorosity.Models;
using MultiPorosity.Services;

namespace MultiPorosity.Presentation.Services
{
    public class RelativePermeabilityService
    {
        private const int _increments = 250;

        public List<RelativePermeabilityModel> Execute(double saturation_water_connate,
                                                       double saturation_water_critical,
                                                       double saturation_oil_irreducible_water,
                                                       double saturation_oil_residual_water,
                                                       double saturation_oil_irreducible_gas,
                                                       double saturation_oil_residual_gas,
                                                       double saturation_gas_connate,
                                                       double saturation_gas_critical,
                                                       double permeability_relative_water_oil_irreducible,
                                                       double permeability_relative_oil_water_connate,
                                                       double permeability_relative_gas_liquid_connate,
                                                       double exponent_permeability_relative_water,
                                                       double exponent_permeability_relative_oil_water,
                                                       double exponent_permeability_relative_gas,
                                                       double exponent_permeability_relative_oil_gas)
        {
            List<RelativePermeabilityModel> models = new();

            InitArguments arguments = new InitArguments(OpenMP.NumberOfThreads, -1, Cuda.Id, OpenMP.DisableWarnings)
            {
                ndevices = Cuda.NumberOfDevices, skip_device = Cuda.SkipDevice
            };

            using(ScopeGuard.Get(arguments))
            {
                View<double, Serial>? view = RelativePermeability.StoneIITable<Serial>(_increments,
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

                RelativePermeabilityModel model;

                for(ulong i0 = 0; i0 < view.Extent(0); ++i0)
                {
                    for(ulong i1 = 0; i1 < view.Extent(1); ++i1)
                    {
                        model = new RelativePermeabilityModel(view[i0, i1, RelativePermeabilityTableIndex.Sg],
                                                              view[i0, i1, RelativePermeabilityTableIndex.So],
                                                              view[i0, i1, RelativePermeabilityTableIndex.Krg],
                                                              view[i0, i1, RelativePermeabilityTableIndex.Kro],
                                                              view[i0, i1, RelativePermeabilityTableIndex.Krw]);

                        if(model.Krg < 1000.0 && model.Kro < 1000.0 && model.Krw < 1000.0)
                        {
                            models.Add(model);
                        }
                    }
                }
            }

            return models;
        }

        public List<RelativePermeabilityModel> ExecuteOpenMP(double saturation_water_connate,
                                                             double saturation_water_critical,
                                                             double saturation_oil_irreducible_water,
                                                             double saturation_oil_residual_water,
                                                             double saturation_oil_irreducible_gas,
                                                             double saturation_oil_residual_gas,
                                                             double saturation_gas_connate,
                                                             double saturation_gas_critical,
                                                             double permeability_relative_water_oil_irreducible,
                                                             double permeability_relative_oil_water_connate,
                                                             double permeability_relative_gas_liquid_connate,
                                                             double exponent_permeability_relative_water,
                                                             double exponent_permeability_relative_oil_water,
                                                             double exponent_permeability_relative_gas,
                                                             double exponent_permeability_relative_oil_gas)
        {
            List<RelativePermeabilityModel> models = new();

            InitArguments arguments = new InitArguments(OpenMP.NumberOfThreads, -1, Cuda.Id, OpenMP.DisableWarnings)
            {
                ndevices = Cuda.NumberOfDevices, skip_device = Cuda.SkipDevice
            };

            using(ScopeGuard.Get(arguments))
            {
                View<double, OpenMP>? view = RelativePermeability.StoneIITable<OpenMP>(_increments,
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

                RelativePermeabilityModel model;

                for(ulong i0 = 0; i0 < view.Extent(0); ++i0)
                {
                    for(ulong i1 = 0; i1 < view.Extent(1); ++i1)
                    {
                        model = new RelativePermeabilityModel(view[i0, i1, RelativePermeabilityTableIndex.Sg],
                                                              view[i0, i1, RelativePermeabilityTableIndex.So],
                                                              view[i0, i1, RelativePermeabilityTableIndex.Krg],
                                                              view[i0, i1, RelativePermeabilityTableIndex.Kro],
                                                              view[i0, i1, RelativePermeabilityTableIndex.Krw]);

                        if(model.Krg < 1000.0 && model.Kro < 1000.0 && model.Krw < 1000.0)
                        {
                            models.Add(model);
                        }
                    }
                }
            }

            return models;
        }

        public List<RelativePermeabilityModel> ExecuteCuda(double saturation_water_connate,
                                                           double saturation_water_critical,
                                                           double saturation_oil_irreducible_water,
                                                           double saturation_oil_residual_water,
                                                           double saturation_oil_irreducible_gas,
                                                           double saturation_oil_residual_gas,
                                                           double saturation_gas_connate,
                                                           double saturation_gas_critical,
                                                           double permeability_relative_water_oil_irreducible,
                                                           double permeability_relative_oil_water_connate,
                                                           double permeability_relative_gas_liquid_connate,
                                                           double exponent_permeability_relative_water,
                                                           double exponent_permeability_relative_oil_water,
                                                           double exponent_permeability_relative_gas,
                                                           double exponent_permeability_relative_oil_gas)
        {
            List<RelativePermeabilityModel> models = new();

            InitArguments arguments = new InitArguments(OpenMP.NumberOfThreads, -1, Cuda.Id, OpenMP.DisableWarnings)
            {
                ndevices = Cuda.NumberOfDevices, skip_device = Cuda.SkipDevice
            };

            using(ScopeGuard.Get(arguments))
            {
                View<double, Cuda>? view = RelativePermeability.StoneIITable<Cuda>(_increments,
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

                RelativePermeabilityModel model;

                for(ulong i0 = 0; i0 < view.Extent(0); ++i0)
                {
                    for(ulong i1 = 0; i1 < view.Extent(1); ++i1)
                    {
                        model = new RelativePermeabilityModel(view[i0, i1, RelativePermeabilityTableIndex.Sg],
                                                              view[i0, i1, RelativePermeabilityTableIndex.So],
                                                              view[i0, i1, RelativePermeabilityTableIndex.Krg],
                                                              view[i0, i1, RelativePermeabilityTableIndex.Kro],
                                                              view[i0, i1, RelativePermeabilityTableIndex.Krw]);
                        
                        ulong offset = view_triangle_offset_right(i0, i1);

                        if(model.Sw >= 0.0)
                        {
                            models.Add(model);
                        }
                    }
                }
            }

            return models;
        }
        
        static ulong view_triangle_offset_right(ulong i0, ulong i1)
        {
            return ((i1 * (i1 + 1)) / 2) + i0;
        }
    }
}