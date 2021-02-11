using System.Runtime.InteropServices;

using MultiPorosity.Presentation.Services;
using MultiPorosity.Presentation.Windows;

using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace MultiPorosity.Presentation
{
    public class Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            IRegionManager? regionManager = containerProvider.Resolve<IRegionManager>();
            
            regionManager.RegisterViewWithRegion(RegionNames.Projects, typeof(ProjectView));

            regionManager.RegisterViewWithRegion(RegionNames.MultiPorosityHistoryMatchParameters,  typeof(MultiPorosityHistoryMatchParametersView));
            regionManager.RegisterViewWithRegion(RegionNames.ProductionHistory,              typeof(ProductionHistoryView));

            regionManager.RegisterViewWithRegion(RegionNames.RelativePermeabilitiesMatrixChart,    typeof(RelativePermeabilitiesMatrixChartView));
            regionManager.RegisterViewWithRegion(RegionNames.RelativePermeabilityMatrixParameters, typeof(RelativePermeabilityMatrixParametersView));

            regionManager.RegisterViewWithRegion(RegionNames.RelativePermeabilitiesHydraulicFractureChart,    typeof(RelativePermeabilitiesHydraulicFractureChartView));
            regionManager.RegisterViewWithRegion(RegionNames.RelativePermeabilityHydraulicFractureParameters, typeof(RelativePermeabilityHydraulicFractureParametersView));

            regionManager.RegisterViewWithRegion(RegionNames.RelativePermeabilitiesNaturalFractureChart,    typeof(RelativePermeabilitiesNaturalFractureChartView));
            regionManager.RegisterViewWithRegion(RegionNames.RelativePermeabilityNaturalFractureParameters, typeof(RelativePermeabilityNaturalFractureParametersView));

            regionManager.RegisterViewWithRegion(RegionNames.ProductionChartLogLog,     typeof(ProductionChartLogLogView));
            regionManager.RegisterViewWithRegion(RegionNames.ProductionChart,           typeof(ProductionChartView));
            regionManager.RegisterViewWithRegion(RegionNames.ProductionCumulativeChart, typeof(ProductionCumulativeChartView));

            regionManager.RegisterViewWithRegion(RegionNames.MultiPorosityChartLogLog,     typeof(MultiPorosityChartLogLogView));
            regionManager.RegisterViewWithRegion(RegionNames.MultiPorosityChart,           typeof(MultiPorosityChartView));
            regionManager.RegisterViewWithRegion(RegionNames.MultiPorosityCumulativeChart, typeof(MultiPorosityCumulativeChartView));

            regionManager.RegisterViewWithRegion(RegionNames.MultiPorosityResults, typeof(MultiPorosityResultsView));
            regionManager.RegisterViewWithRegion(RegionNames.MultiPorosityResultsChart, typeof(MultiPorosityResultsChartView));

            MultiPorosityModelService? multiPorosityModelService = containerProvider.Resolve<MultiPorosityModelService>();
            multiPorosityModelService.SetRepositoryPath();
            multiPorosityModelService.SetExecutionSpace();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<MultiPorosityModelService>();
            
            containerRegistry.RegisterDialog<ConnectToDatabase, ConnectToDatabaseViewModel>();

            containerRegistry.RegisterDialogWindow<DialogWindow>();

            
            //containerRegistry.RegisterForNavigation<MultiPorosityModelView>();

            //containerRegistry.RegisterForNavigation<MultiPorosityChartLogLogView>();
            //containerRegistry.RegisterForNavigation<MultiPorosityChartView>();
            //containerRegistry.RegisterForNavigation<MultiPorosityCumulativeChartView>();
            //containerRegistry.RegisterForNavigation<MultiPorosityResultsChartView>();
            
            //containerRegistry.RegisterSingleton<ProjectViewModel>();

            //containerRegistry.RegisterSingleton<MultiPorosityParameterViewModel>();
            //containerRegistry.RegisterSingleton<MultiPorosityResultsViewModel>();
            //containerRegistry.RegisterSingleton<MultiPorosityHistoryMatchParametersViewModel>();

            //containerRegistry.RegisterSingleton<ProductionChartLogLogViewModel>();
            //containerRegistry.RegisterSingleton<ProductionChartViewModel>();
            //containerRegistry.RegisterSingleton<ProductionCumulativeChartViewModel>();

            //containerRegistry.RegisterSingleton<MultiPorosityChartLogLogViewModel>();
            //containerRegistry.RegisterSingleton<MultiPorosityChartViewModel>();
            //containerRegistry.RegisterSingleton<MultiPorosityCumulativeChartViewModel>();

            //containerRegistry.RegisterSingleton<ProductionHistoryViewModel>();
            
            //containerRegistry.RegisterSingleton<RelativePermeabilitiesMatrixChartView>();
            //containerRegistry.RegisterSingleton<RelativePermeabilityMatrixParametersView>();
            //containerRegistry.RegisterSingleton<RelativePermeabilitiesHydraulicFractureChartView>();
            //containerRegistry.RegisterSingleton<RelativePermeabilityHydraulicFractureParametersView>();
            //containerRegistry.RegisterSingleton<RelativePermeabilitiesNaturalFractureChartView>();
            //containerRegistry.RegisterSingleton<RelativePermeabilityNaturalFractureParametersView>();

            //containerRegistry.RegisterSingleton<DataSourceViewModel>();
            //containerRegistry.RegisterSingleton<RelativePermeabilitiesViewModel>();
            //containerRegistry.RegisterSingleton<MultiPorosityModelViewModel>();
            
            containerRegistry.RegisterForNavigation<ProjectView>(PageKeys.PROJECT);
            containerRegistry.RegisterForNavigation<DataSourceView>(PageKeys.DATA_SOURCE);
            containerRegistry.RegisterForNavigation<RelativePermeabilitiesView>(PageKeys.RELATIVE_PERMEABILITIES);
            containerRegistry.RegisterForNavigation<MultiPorosityModelView>(PageKeys.MULTI_POROSITY_MODEL);
        }
    }
}