using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

using MahApps.Metro.Controls;

namespace MultiPorosity.Tool
{
    public class HamburgerMenuNotifyImageItem : HamburgerMenuImageItem, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;


        public static readonly DependencyProperty NotifyVisibilityProperty = DependencyProperty.Register(nameof(NotifyVisibility), typeof(bool), typeof(HamburgerMenuNotifyImageItem), new PropertyMetadata(false, OnNotifyVisibilityPropertyChanged));

        private static void OnNotifyVisibilityPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if(sender is HamburgerMenuNotifyImageItem item)
            {
                PropertyChangedEventHandler? h = item.PropertyChanged;

                h?.Invoke(sender, new PropertyChangedEventArgs("NotifyVisibility"));
            }
        }

        public bool NotifyVisibility
        {
            get { return (bool)GetValue(NotifyVisibilityProperty); }

            set { SetValue(NotifyVisibilityProperty, value); }
        }

        public static readonly DependencyProperty NotifyLabelProperty = DependencyProperty.Register(nameof(NotifyLabel), typeof(string), typeof(HamburgerMenuNotifyImageItem), new PropertyMetadata(string.Empty, OnNotifyLabelPropertyChanged));
        
        private static void OnNotifyLabelPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if(sender is HamburgerMenuNotifyImageItem item)
            {
                PropertyChangedEventHandler? h = item.PropertyChanged;

                h?.Invoke(sender, new PropertyChangedEventArgs("NotifyLabel"));
            }
        }

        public string NotifyLabel
        {
            get { return (string)GetValue(NotifyLabelProperty); }

            set { SetValue(NotifyLabelProperty, value); }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new HamburgerMenuNotifyImageItem();
        }
    }
}