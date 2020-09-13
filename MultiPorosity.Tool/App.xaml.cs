using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using ReactiveUI;

using SciChart.Charting.Visuals;

namespace MultiPorosity.Tool
{
    public sealed partial class App : Application
    {
        private MainViewModel _mainViewModel;

        static App()
        {
            InstallExceptionHandlers();
        }

        public App()
        {
            _mainViewModel = new MainViewModel();

            // Set this code once in App.xaml.cs or application startup
            SciChartSurface.SetRuntimeLicenseKey("4PYKF1IQWzEuyRbu3oeMphUq2gHFfsF8r+f3gwQvgJomt7K73Oz/pgMqA3g2HX4ZlsjPnAGfWEf4iIvQtAMRCcCJQc2J+MpxaLnNXY5ifa5AEU+6CPMbRIholeXKsHLGt338qOJ/vjm58w/W0/io2O8fLtY9Fcei1cFZiHuHkxrs0Yl9ferpNvdUZknm3AxPIrcuuGLvWc19VcO7xD0z2D+lEcUVVpcamJzr+jprW6jgQfvcQZBGjx2zlfXtBJ18/MTyKJ/6N7PwxfPJhj++FKgaXZTgRZX2C3BsK8I1f7FlKYAmiYz6Bxb2nPmIvdUQUG8btosqGyaDge2/3TKkF3tJwuNARqXvUry5ILeCKYrx9bbDrkRJhtLe3+5fCTiUgwkoaeRCTpHl7nMcpYdUKuAYeuz0lmbGHWvQcf5HHm7umAsA9p6K3oyxlXH2eNv6Oa9cv8W8r7jjf/f+AkTUMSVCMNhGs863unuOr3nHel0/ecVaIqLtv9q/4qWYBovpTRX0bWAuJW2H1VUnkeCN0CHJRIq+JMCG7luNjfxl6T5jCKxsrPzj8r3g0ooeW/3sQU7dLUwkzg==");
        }

        static void InstallExceptionHandlers()
        {
            AppDomain.CurrentDomain.UnhandledException += (s,
                                                           e) => ShowException(e.ExceptionObject as Exception);
        }

        static void ShowException(Exception ex)
        {
            string msg = ex?.ToString() ?? "Unknown exception";
            MessageBox.Show(msg, "MultiPorosity.Tool", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            new MainWindow().Show();
        }
    }
}