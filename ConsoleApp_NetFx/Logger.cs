using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace ConsoleApp_NetFx
{
    class Logger
    {
        private static bool _initialized;
        private static EventLog _eventLog;

        public bool WriteToEventLog
        {
            get { return _eventLog != null; }
            set
            {
                if (value)
                {
                    if (_eventLog == null)
                    {
                        _eventLog = new EventLog();
                        _eventLog.Source = "FabrikamAssetManagement";
                    }
                }
                else
                {
                    _eventLog?.Dispose();
                    _eventLog = null;
                }
            }
        }

        public static void Log(string message)
        {
            if (!_initialized)
            {
                Initialize();
                _initialized = true;
            }

            Trace.WriteLine(message);
            _eventLog?.WriteEntry(message);
        }

        public static void Initialize()
        {
            // Write an informational entry to the event log.    

            var loggingPath = GetLoggingPath();
            var path = Path.Combine(loggingPath, "log.txt");
            var fileListener = new TextWriterTraceListener(path);
            Trace.Listeners.Add(fileListener);

            var consoleListener = new TextWriterTraceListener(Console.Out);
            Trace.Listeners.Add(consoleListener);
        }

        private static string GetLoggingPath()
        {
            using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Fabrikam\AssetManagement"))
            {
                if (key?.GetValue("LoggingDirectoryPath") is string configuredPath)
                    return configuredPath;
            }

            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(appDataPath, "Fabrikam", "AssetManagement", "Logging");
        }
    }
}
