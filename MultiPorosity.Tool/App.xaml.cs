using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Engineering.UI.Controls;

using MultiPorosity.Models;
using MultiPorosity.Services;
using MultiPorosity.Tool;

using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;

namespace MultiPorosity
{
    public class ConsoleWriter : TextWriter
    {
        private readonly IEventAggregator _eventAggregator;

        private readonly StringBuilder _stringBuilder;

        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }

        public ConsoleWriter(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _stringBuilder   = new();
        }

        public override void WriteLine(string? value)
        {
            _stringBuilder.AppendLine(value);

            Flush();
        }

        public override void Write(string? value)
        {
            _stringBuilder.Append(value);
        }

        public override void WriteLine(object? value)
        {
            WriteLine(value?.ToString());
        }

        public override void WriteLine(string           format,
                                       params object?[] args)
        {
            WriteLine(string.Format(null, format, args));
        }

        public override void Write(object? value)
        {
            Write(value?.ToString());
        }

        #region Overrides of TextWriter

        public override void Close()
        {
            Debug.Close();
        }

        public override void Flush()
        {
            _eventAggregator.GetEvent<EventConsoleWrite>().Publish(new ConsoleWritePayload(_stringBuilder.ToString()));

            _stringBuilder.Clear();
        }

        public override void Write(bool value)
        {
            _stringBuilder.Append(value);
        }

        public override void Write(char value)
        {
            _stringBuilder.Append(value);
        }

        public override void Write(char[]? buffer)
        {
            _stringBuilder.Append(new string(buffer));
        }

        public override void Write(decimal value)
        {
            _stringBuilder.Append(value);
        }

        public override void Write(double value)
        {
            _stringBuilder.Append(value);
        }

        public override void Write(int value)
        {
            _stringBuilder.Append(value);
        }

        public override void Write(long value)
        {
            _stringBuilder.Append(value);
        }

        public override void Write(float value)
        {
            _stringBuilder.Append(value);
        }

        public override void Write(StringBuilder? value)
        {
            _stringBuilder.Append(value);
        }

        public override void Write(uint value)
        {
            _stringBuilder.Append(value);
        }

        public override void Write(ulong value)
        {
            _stringBuilder.Append(value);
        }

        public override void WriteLine()
        {
            _stringBuilder.AppendLine();

            Flush();
        }

        public override void WriteLine(bool value)
        {
            _stringBuilder.AppendLine($"{value}");

            Flush();
        }

        public override void WriteLine(char value)
        {
            _stringBuilder.AppendLine($"{value}");

            Flush();
        }

        public override void WriteLine(char[]? buffer)
        {
            _stringBuilder.AppendLine(new string(buffer));

            Flush();
        }

        public override void WriteLine(decimal value)
        {
            _stringBuilder.AppendLine($"{value}");

            Flush();
        }

        public override void WriteLine(double value)
        {
            _stringBuilder.AppendLine($"{value}");

            Flush();
        }

        public override void WriteLine(int value)
        {
            _stringBuilder.AppendLine($"{value}");

            Flush();
        }

        public override void WriteLine(long value)
        {
            _stringBuilder.AppendLine($"{value}");

            Flush();
        }

        public override void WriteLine(float value)
        {
            _stringBuilder.AppendLine($"{value}");

            Flush();
        }

        public override void WriteLine(string  format,
                                       object? arg0)
        {
            _stringBuilder.AppendLine(string.Format(format, arg0));

            Flush();
        }

        public override void WriteLine(string  format,
                                       object? arg0,
                                       object? arg1)
        {
            _stringBuilder.AppendLine(string.Format(format, arg0, arg1));

            Flush();
        }

        public override void WriteLine(string  format,
                                       object? arg0,
                                       object? arg1,
                                       object? arg2)
        {
            _stringBuilder.AppendLine(string.Format(format, arg0, arg1, arg2));

            Flush();
        }

        public override void WriteLine(StringBuilder? value)
        {
            _stringBuilder.Append(value?.ToString());

            Flush();
        }

        public override void WriteLine(uint value)
        {
            _stringBuilder.AppendLine($"{value}");

            Flush();
        }

        public override void WriteLine(ulong value)
        {
            _stringBuilder.AppendLine($"{value}");

            Flush();
        }

        #endregion
    }

    public sealed partial class App //: ConfiguredPrismApplication<ShellWindow, ShellViewModel>
    {
        //private static void InstallExceptionHandlers()
        //{
        //    AppDomain.CurrentDomain.UnhandledException += (s,
        //                                                   e) => ShowException(e.ExceptionObject as Exception);
        //}

        //private static void ShowException(Exception? ex)
        //{
        //    string msg = ex?.ToString() ?? "Unknown exception";
        //    MessageBox.Show(msg, "MultiPorosity.Tool", MessageBoxButton.OK, MessageBoxImage.Error);
        //}

        private ConsoleWriter _consoleWriter;
        
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);

            //containerRegistry.RegisterForNavigation<ShellView, MainViewModel>(PageKeys.MAIN);
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsViewModel>(PageKeys.SETTINGS);
            containerRegistry.RegisterForNavigation<OutputView, OutputViewModel>(PageKeys.OUTPUT);

            _consoleWriter = Container.Resolve<ConsoleWriter>();

            Console.SetOut(_consoleWriter);
            Console.SetError(_consoleWriter);

            ConsoleRedirector.Attach(_consoleWriter);
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<Presentation.Module>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ConsoleRedirector.Detatch();
            base.OnExit(e);
        }
    }
}