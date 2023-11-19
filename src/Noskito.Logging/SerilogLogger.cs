using System;
using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Noskito.Logging
{
    public class SerilogLogger : ILogger
    {
        private readonly Serilog.ILogger _logger;

        public SerilogLogger() => _logger = CreateLogger(LogLevel.Information);

        public void Debug(string msg)
        {
            _logger.Debug(msg);
        }

        public void Debug(string msg, Exception ex)
        {
            _logger.Debug(msg, ex);
        }

        public void DebugFormat(string msg, params object[] objs)
        {
            _logger.Debug(msg, objs);
        }

        public void Info(string msg)
        {
            _logger.Information(msg);
        }

        public void Info(string msg, Exception ex)
        {
            _logger.Information(msg, ex);
        }

        public void InfoFormat(string msg, params object[] objs)
        {
            _logger.Information(msg, objs);
        }

        public void Warn(string msg)
        {
            _logger.Warning(msg);
        }

        public void Warn(string msg, Exception ex)
        {
            _logger.Warning(msg, ex);
        }

        public void WarnFormat(string msg, params object[] objs)
        {
            _logger.Warning(msg, objs);
        }

        public void Error(string msg, Exception ex)
        {
            _logger.Error(ex, msg);
        }

        public void ErrorFormat(string msg, Exception ex, params object[] objs)
        {
            _logger.Error(ex, msg, objs);
        }

        public void Fatal(string msg, Exception ex)
        {
            _logger.Fatal(ex, msg);
        }

        internal static Serilog.ILogger CreateLogger(LogLevel logEventLevel = LogLevel.Debug)
        {
            LoggerConfiguration config = new LoggerConfiguration()
                .Enrich.WithThreadId()
                .Enrich.FromLogContext()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code,
                    outputTemplate: "[{Timestamp:HH:mm:ss}][{Level:u4}][Thread:{ThreadId}] {Message:lj} {NewLine}{Exception}");

            string? logLevel = Environment.GetEnvironmentVariable("LOG_LEVEL");

            if (!string.IsNullOrEmpty(logLevel))
            {
                config.MinimumLevel.Is(ParseLogLevel(logLevel));
            }
            else
            {
                config.MinimumLevel.Is(ParseLogLevel(logEventLevel));
            }

            return config.CreateLogger();
        }

        private static LogEventLevel ParseLogLevel(string level)
        {
            return level.ToUpper() switch
            {
                "DEBUG" => LogEventLevel.Debug,
                "INFO" or "INFORMATION" => LogEventLevel.Information,
                "WARN" or "WARNING" => LogEventLevel.Warning,
                "FATAL" => LogEventLevel.Fatal,
                "ERROR" => LogEventLevel.Error,
                _ => throw new InvalidEnumArgumentException("LOG_LEVEL authorized: Debug / Info / Warning / Error / Fatal")
            };
        }

        private static LogEventLevel ParseLogLevel(LogLevel level)
        {
            return level switch
            {
                LogLevel.Trace => LogEventLevel.Verbose,
                LogLevel.Debug => LogEventLevel.Debug,
                LogLevel.Information => LogEventLevel.Information,
                LogLevel.Warning => LogEventLevel.Warning,
                LogLevel.Error or LogLevel.Critical or LogLevel.None => LogEventLevel.Error,
                _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
            };
        }
    }
}
