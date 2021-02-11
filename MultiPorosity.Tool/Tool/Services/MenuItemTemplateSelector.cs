using System.Windows;
using System.Windows.Controls;

using MahApps.Metro.Controls;

namespace MultiPorosity.Tool
{
    public class MenuItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? GlyphDataTemplate { get; set; }

        public DataTemplate? ImageDataTemplate { get; set; }

        public DataTemplate? NotifyImageDataTemplate { get; set; }

        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            if (item is HamburgerMenuGlyphItem)
            {
                return GlyphDataTemplate;
            }

            if (item is HamburgerMenuNotifyImageItem)
            {
                return NotifyImageDataTemplate;
            }

            if (item is HamburgerMenuImageItem)
            {
                return ImageDataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
