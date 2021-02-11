using System;
using System.Diagnostics;
using System.Windows.Input;

using Kokkos;

using MultiPorosity.Presentation.Models;
using MultiPorosity.Presentation.Services;
using MultiPorosity.Services;

using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.App;

namespace MultiPorosity.Tool
{
    public class SettingsViewModel : BindableBase, INavigationAware
    {
        private readonly Configuration             _appConfig;
        private readonly IThemeSelectorService     _themeSelectorService;
        private readonly IApplicationInfoService   _applicationInfoService;
        private readonly MultiPorosityModelService _multiPorosityModelService;
        
        private          ThemeKind               _theme;
        private          string                  _versionDescription;
        private          ICommand                _setThemeCommand;
        private          ICommand                _privacyStatementCommand;

        public ThemeKind Theme
        {
            get { return _theme; }
            set { SetProperty(ref _theme, value); }
        }

        public string? RepositoryPath
        {
            get { return _multiPorosityModelService.RepositoryPath; }
            set { _multiPorosityModelService.RepositoryPath = value; }
        }

        public string VersionDescription
        {
            get { return _versionDescription; }
            set { SetProperty(ref _versionDescription, value); }
        }

        public ExecutionSpaceKind ExecutionSpace
        {
            get { return _multiPorosityModelService.ExecutionSpace; }
            set { _multiPorosityModelService.ExecutionSpace = value; }
        }

        public ICommand SetThemeCommand
        {
            get { return _setThemeCommand; }
        }

        public ICommand PrivacyStatementCommand
        {
            get { return _privacyStatementCommand; }
        }

        
        //private readonly IEventAggregator _eventAggregator;
        
        public SettingsViewModel(Configuration             appConfig,
                                 IThemeSelectorService     themeSelectorService,
                                 IApplicationInfoService   applicationInfoService,
                                 MultiPorosityModelService multiPorosityModelService)
        {
            //_eventAggregator = Throw.IfNull(eventAggregator);
            
            _appConfig                 = appConfig;
            _themeSelectorService      = themeSelectorService;
            _applicationInfoService    = applicationInfoService;
            _multiPorosityModelService = multiPorosityModelService;
            
            _setThemeCommand         = new DelegateCommand<string>(OnSetTheme);
            _privacyStatementCommand = new DelegateCommand(OnPrivacyStatement); 
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            VersionDescription = $"MultiPorosity.Tool - {_applicationInfoService.GetVersion()}";
            Theme              = _themeSelectorService.GetCurrentTheme();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        private void OnSetTheme(string themeName)
        {
            ThemeKind theme = (ThemeKind)Enum.Parse(typeof(ThemeKind), themeName);
            _themeSelectorService.SetTheme(theme);
        }

        private void OnPrivacyStatement()
        {
            OpenInWebBrowser(_appConfig.PrivacyStatement);
        }

        public static void OpenInWebBrowser(string url)
        {
            // For more info see https://github.com/dotnet/corefx/issues/10361
            ProcessStartInfo? psi = new ProcessStartInfo
            {
                FileName = url, UseShellExecute = true
            };

            Process.Start(psi);
        }
    }
}