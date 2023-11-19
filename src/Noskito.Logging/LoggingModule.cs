using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Noskito.Logging
{

    public static class LoggingModule
    {
        public static void UseLoggingModule(this IServiceCollection services, LogLevel logLevel = LogLevel.Warning)
        {
            services.AddLogging(builder =>
            {
                builder.ConfigureLoggingModule(logLevel);
            });
        }

        public static void ConfigureLoggingModule(this ILoggingBuilder logging, LogLevel logLevel = LogLevel.Debug)
        {
            logging.AddSerilog(SerilogLogger.CreateLogger(logLevel), true);
        }
    }
}
