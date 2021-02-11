#if false

using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

using ControlzEx.Theming;

namespace MultiPorosity.Tool
{
    public static class ThemeGenerator
    {
        public static Tuple<string, Theme?> CreateTheme(string baseColorScheme,
                                                        Color  accentBaseColor,
                                                        bool   changeImmediately = true)
        {
            Theme? theme = RuntimeThemeGenerator.Current.GenerateRuntimeTheme(baseColorScheme, accentBaseColor, false);

            if(changeImmediately && theme is not null)
            {
                Theme changedTheme = ThemeManager.Current.ChangeTheme(Application.Current, theme);

                return new Tuple<string, Theme?>(string.Join(Environment.NewLine, changedTheme.GetAllResources().Select(GetResourceDictionaryContent)), changedTheme);
            }

            return new Tuple<string, Theme?>(Environment.NewLine, theme);
        }

        public static string GetResourceDictionaryContent(ResourceDictionary resourceDictionary)
        {
            using StringWriter sw = new StringWriter();

            using XmlWriter writer = XmlWriter.Create(sw,
                                                      new XmlWriterSettings
                                                      {
                                                          Indent = true, IndentChars = "    "
                                                      });

            XamlWriter.Save(resourceDictionary, writer);

            return sw.ToString();
        }
    }
}
#endif