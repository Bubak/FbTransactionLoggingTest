using FirebirdSql.Data.Logging;
using FirebirdSql.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LogTest
{
    internal class LoggerProvider : IFbLoggingProvider
    {
        private readonly FbLogLevel _minimumLevel;

        public LoggerProvider(FbLogLevel minimumLevel = FbLogLevel.Info)
        {
            _minimumLevel = minimumLevel;
        }

        public IFbLogger CreateLogger(string name) => new Logger(_minimumLevel);

        private sealed class Logger : IFbLogger
        {
            private readonly FbLogLevel _minimumLevel;

            public Logger(FbLogLevel minimumLevel)
            {
                _minimumLevel = minimumLevel;
            }

            public bool IsEnabled(FbLogLevel level)
            {
                return level >= _minimumLevel;
            }

            public void Log(FbLogLevel level, string msg, Exception? exception = null)
            {
                if (!IsEnabled(level))
                    return;

                var sb = new StringBuilder();
                sb.AppendLine(msg);

                if (exception != null)
                    sb.AppendLine(exception.ToString());

                LogDebug(sb.ToString());
            }
        }
        public static void LogDebug(string message)
        {
            var logMessage = FormatLogMessage(message, null);
            LogToFile(logMessage);
        }
        private static string FormatLogMessage(string message, object? obj)
        {
            var logMessage = new StringBuilder();
            logMessage.AppendLine(DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + Environment.NewLine + message.Trim());
            if (obj != null)
            {
                logMessage.AppendLine(JsonSerializer.Serialize(obj));
            }
            return logMessage.ToString();
        }

        private static void LogToFile(string logMessage)
        {
            //var dir = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\logs\";
            //Directory.CreateDirectory(dir);
            var sWriter = File.AppendText($"{DateTime.Today:yy-MM-dd}.log");
            sWriter.WriteAsync(logMessage + Environment.NewLine);
            sWriter.Close();
        }
    }
}
