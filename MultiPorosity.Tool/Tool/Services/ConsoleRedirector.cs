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

        public static bool IsAttached
        {
            get { return null != _instance; }
        }

        private const    int                       _PERIOD      = 500;
        private const    int                       _BUFFER_SIZE = 4096;
        private volatile bool                      _isDisposed;
        private readonly TextWriter                _writer;
        private readonly IntPtr                    _stdout;
        private readonly IntPtr                    _stderr;
        private readonly Mutex                     _sync;
        private readonly Timer                     _timer;
        private readonly char[]                    _buffer;
        private readonly AnonymousPipeServerStream _outServer;
        private readonly AnonymousPipeServerStream _errServer;
        private readonly TextReader                _outClient;
        private readonly TextReader                _errClient;

        
        private const int STD_OUTPUT_HANDLE = -11;
        private const int STD_ERROR_HANDLE  = -12;

        private ConsoleRedirector(TextWriter writer)
        {
            int                       ret;
            AnonymousPipeClientStream client;

            _writer = writer;
            _stdout = PlatformApi.Win32.Kernel32.Native.GetStdHandle(STD_OUTPUT_HANDLE);
            _stderr = PlatformApi.Win32.Kernel32.Native.GetStdHandle(STD_ERROR_HANDLE);
            _sync   = new Mutex();
            _buffer = new char[_BUFFER_SIZE];

            _outServer = new AnonymousPipeServerStream(PipeDirection.Out);
            client     = new AnonymousPipeClientStream(PipeDirection.In, _outServer.ClientSafePipeHandle);

            //client.ReadTimeout = 0;

            Debug.Assert(_outServer.IsConnected);

            _outClient = new StreamReader(client, Encoding.Default);
            ret        = PlatformApi.Win32.Kernel32.Native.SetStdHandle(STD_OUTPUT_HANDLE, _outServer.SafePipeHandle.DangerousGetHandle());

            Debug.Assert(ret != 0);

            _errServer = new AnonymousPipeServerStream(PipeDirection.Out);
            client     = new AnonymousPipeClientStream(PipeDirection.In, _errServer.ClientSafePipeHandle);

            //client.ReadTimeout = 0;

            Debug.Assert(_errServer.IsConnected);

            _errClient = new StreamReader(client, Encoding.Default);
            ret        = PlatformApi.Win32.Kernel32.Native.SetStdHandle(STD_ERROR_HANDLE, _errServer.SafePipeHandle.DangerousGetHandle());

            Debug.Assert(ret != 0);

            Thread outThread = new Thread(DoRead);
            Thread errThread = new Thread(DoRead);
            outThread.Name = "stdout logger";
            errThread.Name = "stderr logger";
            outThread.Start(_outClient);
            errThread.Start(_errClient);
            _timer = new Timer(Flush, null, _PERIOD, _PERIOD);
        }
        // ReSharper restore UseObjectOrCollectionInitializer

        private void Flush(object? state)
        {
            _outServer.Flush();
            _errServer.Flush();
        }

        private void DoRead(object? clientObj)
        {
            TextReader? client = clientObj as TextReader;

            if(client == null)
            {
                return;
            }

            try
            {
                int read;

                while(true)
                {
                    read = client.Read(_buffer, 0, _BUFFER_SIZE);

                    if(read > 0)
                    {
                        //Console.WriteLine(" log :"+_buffer.ToString()+read);
                        _writer.Write(_buffer, 0, read);
                    }
                }
            }
            catch(ObjectDisposedException)
            {
                // Pipe was closed... terminate
            }
        }

        public void Dispose()
        {
            Dispose(true);
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
                            PlatformApi.Win32.Kernel32.Native.SetStdHandle(STD_OUTPUT_HANDLE, _stdout);
                        }
                        catch(Exception)
                        {
                            // ignored
                        }

                        _outClient.Dispose();
                        _outServer.Dispose();

                        try
                        {
                            PlatformApi.Win32.Kernel32.Native.SetStdHandle(STD_ERROR_HANDLE, _stderr);
                        }
                        catch(Exception)
                        {
                            // ignored
                        }

                        _errClient.Dispose();
                        _errServer.Dispose();
                    }
                }
            }
        }
    }
}