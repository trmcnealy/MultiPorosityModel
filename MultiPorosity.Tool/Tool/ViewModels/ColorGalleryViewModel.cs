#if false
#nullable enable
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

using ControlzEx.Theming;

using Engineering.UI;

using Prism.Mvvm;

namespace MultiPorosity.Tool
{
    public class ColorGalleryViewModel : BindableBase
    {
        private Color standardColor;
        private Color highlightColor;

        private Color? themeColor = ((SolidColorBrush)Application.Current.Resources["Fluent.Ribbon.Brushes.AccentBaseColorBrush"])?.Color ?? Colors.Pink;

        private string? currentBaseColor;

        private Theme? currentTheme = ThemeManager.Current.DetectTheme(Application.Current);

        public Color StandardColor
        {
            get { return standardColor; }
            set
            {
                if(this.SetProperty(ref standardColor, value))
                {
                }
            }
        }

        public Color HighlightColor
        {
            get { return highlightColor; }
            set
            {
                if(this.SetProperty(ref highlightColor, value))
                {
                }
            }
        }

        public Color[] ThemeColors { get; } =
        {
            Colors.Red, Colors.Green, Colors.Blue, Colors.White, Colors.Black, Colors.Purple
        };

        public Color? ThemeColor
        {
            get { return themeColor; }

            set
            {
                if(value is null)
                {
                    return;
                }

                if(this.SetProperty(ref themeColor, value) && themeColor is not null)
                {
                    SolidColorBrush solidColorBrush = new SolidColorBrush(value.Value);
                    solidColorBrush.Freeze();
                    Application.Current.Resources["Fluent.Ribbon.Brushes.AccentBaseColorBrush"] = solidColorBrush;
                }
            }
        }

        public string? CurrentBaseColor
        {
            get { return currentBaseColor; }

            set
            {
                if(value is null)
                {
                    return;
                }

                if(this.SetProperty(ref currentBaseColor, value) && currentBaseColor is not null)
                {
                    CurrentTheme = ThemeManager.Current.ChangeThemeBaseColor(Application.Current, value);
                }
            }
        }

        public Theme? CurrentTheme
        {
            get { return currentTheme; }

            set
            {
                if(value is null)
                {
                    return;
                }

                if(this.SetProperty(ref currentTheme, value) && currentTheme is not null)
                {
                    ThemeManager.Current.ChangeTheme(Application.Current, value);

                    CurrentBaseColor = currentTheme.BaseColorScheme;
                }
            }
        }

        public ColorGalleryViewModel()
        {
            StandardColor  = Colors.Black;
            HighlightColor = Colors.Yellow;

            CollectionViewSource.GetDefaultView(ThemeManager.Current.Themes).GroupDescriptions.Add(new PropertyGroupDescription(nameof(Theme.BaseColorScheme)));
        }
    }
}
#endif