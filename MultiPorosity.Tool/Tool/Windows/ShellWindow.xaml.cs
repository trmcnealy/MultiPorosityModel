
using System.Windows;
using System.Windows.Controls;

using MahApps.Metro.Controls;

using Prism.Regions;

namespace MultiPorosity.Tool
{
    public partial class ShellWindow
    {
        public ShellWindow(IRegionManager regionManager)
        {
            InitializeComponent();
            
            RegionManager.SetRegionName(hamburgerMenuContentControl, RegionNames.Main);
            RegionManager.SetRegionManager(hamburgerMenuContentControl, regionManager);
        }
    }
}
