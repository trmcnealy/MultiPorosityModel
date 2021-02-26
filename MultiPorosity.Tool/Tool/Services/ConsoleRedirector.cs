using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;

namespace MultiPorosity.Tool
{
    internal sealed class ConsoleRedirector : IDisposable
    {
        private static ConsoleRedirector? _instance;

        private const    int        PERIOD      = 500;
        private const    int        BUFFER_SIZE = 4096;
        private readonly TextWriter _writer;
        private readonly IntPtr     _stdout;
        private readonly IntPtr     _stderr;
        private readonly Mutex      _sync;

        private readonly Thread _outThread;
        private readonly Thread _errThread;

        private readonly Timer                     _timer;
        private readonly char[]                    _buffer;
        private readonly AnonymousPipeServerStream _outServer;
        private readonly AnonymousPipeServerStream _errServer;
        private          TextReader?               _outClient;
        private          TextReader?               _errClient;
        private volatile bool                      _isDisposed;

        public static bool IsAttached
        {
            get { return null != _instance; }
        }

        private ConsoleRedirector(TextWriter writer)
        {
            _writer = writer;

            _stdout = PlatformApi.Win32.Kernel32.Native.GetStdHandle(PlatformApi.Win32.Kernel32.Native.STD_OUTPUT_HANDLE);
            _stderr = PlatformApi.Win32.Kernel32.Native.GetStdHandle(PlatformApi.Win32.Kernel32.Native.STD_ERROR_HANDLE);
            _sync   = new Mutex();
            _buffer = new char[BUFFER_SIZE];

            _outServer = new AnonymousPipeServerStream(PipeDirection.Out);
            AnonymousPipeClientStream client = new AnonymousPipeClientStream(PipeDirection.In, _outServer.ClientSafePipeHandle);

            Debug.Assert(_outServer.IsConnected);
            _outClient = new StreamReader(client, Encoding.Default);
            int ret = PlatformApi.Win32.Kernel32.Native.SetStdHandle(PlatformApi.Win32.Kernel32.Native.STD_OUTPUT_HANDLE, _outServer.SafePipeHandle.DangerousGetHandle());
            Debug.Assert(ret != 0);

            _errServer = new AnonymousPipeServerStream(PipeDirection.Out);
            client     = new AnonymousPipeClientStream(PipeDirection.In, _errServer.ClientSafePipeHandle);

            Debug.Assert(_errServer.IsConnected);
            _errClient = new StreamReader(client, Encoding.Default);
            ret        = PlatformApi.Win32.Kernel32.Native.SetStdHandle(PlatformApi.Win32.Kernel32.Native.STD_ERROR_HANDLE, _errServer.SafePipeHandle.DangerousGetHandle());
            Debug.Assert(ret != 0);

            _outThread      = new Thread(DoRead);
            _errThread      = new Thread(DoRead);
            _outThread.Name = "stdout logger";
            _errThread.Name = "stderr logger";
            _outThread.Start(_outClient);
            _errThread.Start(_errClient);
            _timer = new Timer(Flush, null, PERIOD, PERIOD);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public static void Attach(TextWriter writer)
        {
            Debug.Assert(null == _instance);
            _instance = new ConsoleRedirector(writer);
        }

        public static void Detatch()
        {
            _instance?.Dispose();
            _instance = null;
        }

        private void Flush(object? state)
        {
            _outServer.Flush();
            _errServer.Flush();
        }

        private void DoRead(object? clientObj)
        {
            if(clientObj is TextReader client)
            {
                try
                {
                    while(_outClient is not null && _errClient is not null)
                    {
                        int read = client.Read(_buffer, 0, BUFFER_SIZE);

                        if(read > 0)
                            //Console.WriteLine(" log :"+_buffer.ToString()+read);
                        {
                            _writer.Write(_buffer, 0, read);
                        }
                    }
                }
                catch(Exception)
                {
                    // Pipe was closed... terminate
                }
            }
        }

        ~ConsoleRedirector()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if(!_isDisposed)
            {
                lock(_sync)
                {
                    if(!_isDisposed)
                    {
                        _isDisposed = true;

                        _timer.Change(Timeout.Infinite, Timeout.Infinite);
                        _timer.Dispose();

                        Flush(null);

                        try
                        {
                            PlatformApi.Win32.Kernel32.Native.SetStdHandle(PlatformApi.Win32.Kernel32.Native.STD_OUTPUT_HANDLE, _stdout);
                        }
                        catch(Exception)
                        {
                            // ignored
                        }

                        _outClient?.Dispose();
                        _outClient = null;
                        _outServer.Dispose();

                        try
                        {
                            PlatformApi.Win32.Kernel32.Native.SetStdHandle(PlatformApi.Win32.Kernel32.Native.STD_ERROR_HANDLE, _stderr);
                        }
                        catch(Exception)
                        {
                            // ignored
                        }

                        _errClient?.Dispose();
                        _errClient = null;
                        _errServer.Dispose();
                    }
                }
            }
        }
    }
}