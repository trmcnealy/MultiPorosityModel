using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

using Engineering.UI.Controls;

using Kokkos;

using MahApps.Metro.Controls;

using MultiPorosity.Presentation.Services;
using MultiPorosity.Services.Models;

using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace MultiPorosity.Tool
{
    public class ShellViewModel : BindableBase
    {
        private static readonly ImageSource? HomeIcon;
        private static readonly ImageSource? LogIcon;
        private static readonly ImageSource? DataSourceIcon;
        private static readonly ImageSource? RelativePermeabilitiesIcon;
        private static readonly ImageSource? MultiPorosityModelIcon;

        static ShellViewModel()
        {
            ResourceDictionary resources = new ResourceDictionary
            {
                Source = new Uri("/MultiPorosity.Tool;component/Tool/Resources/ShellWindowResources.xaml", UriKind.RelativeOrAbsolute)
            };

            HomeIcon = resources["HomeIcon"] as DrawingImage;
            HomeIcon?.Freeze();
            DataSourceIcon = resources["DataSourceIcon"] as DrawingImage;
            DataSourceIcon?.Freeze();
            RelativePermeabilitiesIcon = resources["RelativePermeabilitiesIcon"] as DrawingImage;
            RelativePermeabilitiesIcon?.Freeze();
            MultiPorosityModelIcon = resources["MultiPorosityModelIcon"] as DrawingImage;
            MultiPorosityModelIcon?.Freeze();
            LogIcon = resources["LogIcon"] as DrawingImage;
            LogIcon?.Freeze();
        }

        private readonly IRegionManager            _regionManager;
        private          IRegionNavigationService? _navigationService;
        private          HamburgerMenuItem?        _selectedMenuItem;
        private          HamburgerMenuItem?        _selectedOptionsMenuItem;
        private readonly DelegateCommand           _goBackCommand;
        private readonly DelegateCommand           _goForwardCommand;
        private readonly ICommand                  _loadedCommand;
        private readonly ICommand                  _unloadedCommand;
        private readonly ICommand                  _menuItemInvokedCommand;
        private readonly ICommand                  _optionsMenuItemInvokedCommand;

        private bool _activeProjectLoaded;

        private string? _title;

        public string? Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private bool _IsProgressRingVisible;

        public bool IsProgressRingVisible
        {
            get { return _IsProgressRingVisible; }
            set
            {
                if(SetProperty(ref _IsProgressRingVisible, value))
                {
                    if(_IsProgressRingVisible)
                    {
                        _timer.Start();
                    }
                    else
                    {
                        _timer.Stop();
                    }
                }
            }
        }

        private bool _IsShellVisible;

        public bool IsShellVisible
        {
            get { return _IsShellVisible; }
            set { SetProperty(ref _IsShellVisible, value); }
        }

        public HamburgerMenuItem? SelectedMenuItem
        {
            get { return _selectedMenuItem; }
            set { SetProperty(ref _selectedMenuItem, value); }
        }

        public HamburgerMenuItem? SelectedOptionsMenuItem
        {
            get { return _selectedOptionsMenuItem; }
            set { SetProperty(ref _selectedOptionsMenuItem, value); }
        }

        private bool _ShellLogNotifyVisibility;

        public bool ShellLogNotifyVisibility
        {
            get { return _ShellLogNotifyVisibility; }
            set { SetProperty(ref _ShellLogNotifyVisibility, value); }
        }

        private string _ShellLogNotifyLabel;

        public string ShellLogNotifyLabel
        {
            get { return _ShellLogNotifyLabel; }
            set { SetProperty(ref _ShellLogNotifyLabel, value); }
        }

        private DispatcherTimer _timer;

        private void UpdateUsages(object?   sender,
                                  EventArgs e)
        {
            Devices devices = MultiPorosityModelService.GetDevices();

            CpuUsage = devices.Cpus.Sum(cpu => cpu.GetUsage());

            GpuUsage = devices.Gpus.Sum(gpu => gpu.GetUsage());
        }

        private int _CpuUsage;

        public int CpuUsage
        {
            get { return _CpuUsage; }
            set { SetProperty(ref _CpuUsage, value); }
        }

        private int _GpuUsage;

        public int GpuUsage
        {
            get { return _GpuUsage; }
            set { SetProperty(ref _GpuUsage, value); }
        }

        public ObservableCollection<HamburgerMenuItem>? MenuItems { get; } = new ObservableCollection<HamburgerMenuItem>()
        {
            new HamburgerMenuImageItem()
            {
                Label     = Strings.ShellMainPage,
                Thumbnail = HomeIcon,
                Tag       = Presentation.PageKeys.PROJECT
            },
            new HamburgerMenuImageItem()
            {
                IsEnabled = false,
                Label     = Strings.ShellDataSourcePage,
                Thumbnail = DataSourceIcon,
                Tag       = Presentation.PageKeys.DATA_SOURCE
            },
            new HamburgerMenuImageItem()
            {
                IsEnabled = false,
                Label     = Strings.ShellRelativePermeabilitiesPage,
                Thumbnail = RelativePermeabilitiesIcon,
                Tag       = Presentation.PageKeys.RELATIVE_PERMEABILITIES
            },
            new HamburgerMenuImageItem()
            {
                IsEnabled = false,
                Label     = Strings.ShellMultiPorosityModelPage,
                Thumbnail = MultiPorosityModelIcon,
                Tag       = Presentation.PageKeys.MULTI_POROSITY_MODEL
            }
        };

        private static HamburgerMenuNotifyImageItem getShellLogItem(ShellViewModel viewModel)
        {
            HamburgerMenuNotifyImageItem item = new HamburgerMenuNotifyImageItem()
            {
                Label     = Strings.ShellLog,
                Thumbnail = LogIcon,
                Tag       = PageKeys.OUTPUT,
                //NotifyVisibility = true,
                //NotifyLabel = "0",
                Command = new DelegateCommand(viewModel.OnShellLogClicked)
            };

            Binding NotifyVisibilityBinding = new Binding("ShellLogNotifyVisibility")
            {
                Source              = viewModel,
                Mode                = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            BindingOperations.SetBinding(item, HamburgerMenuNotifyImageItem.NotifyVisibilityProperty, NotifyVisibilityBinding);

            Binding NotifyLabelBinding = new Binding("ShellLogNotifyLabel")
            {
                Source              = viewModel,
                Mode                = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            BindingOperations.SetBinding(item, HamburgerMenuNotifyImageItem.NotifyLabelProperty, NotifyLabelBinding);

            return item;
        }

        public ObservableCollection<HamburgerMenuItem> OptionMenuItems { get; } = new ObservableCollection<HamburgerMenuItem>()
        {
            new HamburgerMenuGlyphItem()
            {
                Label = Strings.ShellSettingsPage,
                Glyph = "\uE713",
                Tag   = PageKeys.SETTINGS
            }
        };

        public DelegateCommand GoBackCommand
        {
            get { return _goBackCommand; }
        }

        public DelegateCommand GoForwardCommand
        {
            get { return _goForwardCommand; }
        }

        public ICommand LoadedCommand
        {
            get { return _loadedCommand; }
        }

        public ICommand UnloadedCommand
        {
            get { return _unloadedCommand; }
        }

        public ICommand MenuItemInvokedCommand
        {
            get { return _menuItemInvokedCommand; }
        }

        public ICommand OptionsMenuItemInvokedCommand
        {
            get { return _optionsMenuItemInvokedCommand; }
        }

        public long UsedMemory
        {
            get { return GC.GetTotalMemory(true) / 1024; }
        }

        private readonly IEventAggregator _eventAggregator;

        public ShellViewModel(IRegionManager   regionManager,
                              IEventAggregator eventAggregator)
        {
            _regionManager   = regionManager;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<ProjectChangedEvent>().Subscribe(SetTitle);
            _eventAggregator.GetEvent<ProjectIsBusyEvent>().Subscribe(OnIsBusy);
            _eventAggregator.GetEvent<EventConsoleWrite>().Subscribe(OnConsoleWrite);

            IsShellVisible        = true;
            IsProgressRingVisible = !IsShellVisible;
            SetTitle();

            _goBackCommand                 = new DelegateCommand(OnGoBack,    CanGoBack);
            _goForwardCommand              = new DelegateCommand(OnGoForward, CanGoForward);
            _loadedCommand                 = new DelegateCommand(OnLoaded);
            _unloadedCommand               = new DelegateCommand(OnUnloaded);
            _menuItemInvokedCommand        = new DelegateCommand(OnMenuItemInvoked);
            _optionsMenuItemInvokedCommand = new DelegateCommand(OnOptionsMenuItemInvoked);
        }

        private void OnConsoleWrite(ConsoleWritePayload payload)
        {
            if(!string.IsNullOrEmpty(ShellLogNotifyLabel))
            {
                if(int.TryParse(ShellLogNotifyLabel, out int NotifyLabelValue))
                {
                    ShellLogNotifyLabel      = (NotifyLabelValue + 1).ToString();
                    ShellLogNotifyVisibility = true;

                    return;
                }
            }

            ShellLogNotifyLabel      = "1";
            ShellLogNotifyVisibility = true;

            //if(OptionMenuItems.LastOrDefault() is HamburgerMenuNotifyImageItem item)
            //{
            //    //item.Command ??= new DelegateCommand(OnShellLogClicked);

            //    if(!string.IsNullOrEmpty(item.NotifyLabel))
            //    {
            //        if(int.TryParse(item.NotifyLabel, out int NotifyLabelValue))
            //        {
            //            item.NotifyLabel      = (NotifyLabelValue + 1).ToString();
            //            item.NotifyVisibility = true;

            //            return;
            //        }
            //    }

            //    item.NotifyLabel      = "1";
            //    item.NotifyVisibility = true;
            //}
        }

        private void OnShellLogClicked()
        {
            //if(OptionMenuItems.LastOrDefault() is HamburgerMenuNotifyImageItem item)
            //{
            //    item.NotifyLabel      = string.Empty;
            //    item.NotifyVisibility = false;
            //}

            ShellLogNotifyLabel      = string.Empty;
            ShellLogNotifyVisibility = false;
        }

        private void OnLoaded()
        {
            _navigationService           =  _regionManager.Regions[RegionNames.Main].NavigationService;
            _navigationService.Navigated += OnNavigated;

            if(MenuItems is not null)
            {
                SelectedMenuItem = MenuItems.First();
            }

            OptionMenuItems.Add(getShellLogItem(this));

            _timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 1000)
            };

            _timer.Tick += UpdateUsages;
        }

        private void OnUnloaded()
        {
            if(_navigationService is not null)
            {
                _navigationService.Navigated -= OnNavigated;
            }

            _regionManager.Regions.Remove(RegionNames.Main);
        }

        private bool CanGoBack()
        {
            return _navigationService is not null && _navigationService.Journal.CanGoBack;
        }

        private void OnGoBack()
        {
            _navigationService?.Journal.GoBack();
        }

        private bool CanGoForward()
        {
            return _navigationService is not null && _navigationService.Journal.CanGoForward;
        }

        private void OnGoForward()
        {
            _navigationService?.Journal.GoForward();
        }

        private void OnMenuItemInvoked()
        {
            if(SelectedMenuItem?.Tag is not null)
            {
                RequestNavigate(SelectedMenuItem.Tag.ToString());
            }
        }

        private void OnOptionsMenuItemInvoked()
        {
            if(SelectedOptionsMenuItem?.Tag is not null)
            {
                RequestNavigate(SelectedOptionsMenuItem.Tag.ToString());
            }
        }

        private void RequestNavigate(string? target)
        {
            if(_navigationService is not null && _navigationService.CanNavigate(target))
            {
                _navigationService.RequestNavigate(target);
            }
        }

        private void OnNavigated(object?                   sender,
                                 RegionNavigationEventArgs e)
        {
            if(MenuItems is not null)
            {
                HamburgerMenuItem? item = MenuItems.FirstOrDefault(i => e.Uri.ToString() == i.Tag.ToString());

                if(item is not null)
                {
                    SelectedMenuItem = item;
                }
                else
                {
                    SelectedOptionsMenuItem = OptionMenuItems.FirstOrDefault(i => e.Uri.ToString() == i.Tag?.ToString());
                }
            }

            GoBackCommand.RaiseCanExecuteChanged();
            GoForwardCommand.RaiseCanExecuteChanged();
        }

        private void SetTitle(string? title = null)
        {
            if(!string.IsNullOrEmpty(title))
            {
                if(!_activeProjectLoaded)
                {
                    for(int i = 0; i < MenuItems!.Count; ++i)
                    {
                        if(!MenuItems[i].IsEnabled)
                        {
                            MenuItems[i].IsEnabled = true;
                        }
                    }

                    _activeProjectLoaded = true;
                }

                Title = Strings.MainWindowTitle + " [ " + title + " ]";
            }
            else
            {
                Title = Strings.MainWindowTitle;
            }
        }

        private void OnIsBusy(bool isBusy)
        {
            IsProgressRingVisible = isBusy;
            IsShellVisible        = !IsProgressRingVisible;
        }
    }
}