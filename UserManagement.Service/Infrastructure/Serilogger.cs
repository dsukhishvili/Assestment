
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace UserManagement.Service.Infrastructure
{
    public class Serilogger:ILogger
    {
        private readonly Logger _logger;
        public Serilogger(string logPath, LogEventLevel minLogLevel = LogEventLevel.Warning)
        {
            var levelSwitch = new LoggingLevelSwitch { MinimumLevel = minLogLevel };
            _logger = new LoggerConfiguration()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Hour)
                .MinimumLevel.ControlledBy(levelSwitch)
                .CreateLogger();
        }

        public Serilogger(LoggerConfiguration config, LogEventLevel minLogLevel = LogEventLevel.Warning)
        {
            var levelSwitch = new LoggingLevelSwitch { MinimumLevel = minLogLevel };
            _logger = config.MinimumLevel.ControlledBy(levelSwitch)
                .CreateLogger();
        }
        public void LogException(Exception ex)
        {
            _logger.Error(ex, ex.ToString());
        }

        public void LogMessage(string message, LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Error:
                    _logger.Error(message);
                    break;
                case LogLevel.Warning:
                    _logger.Warning(message);
                    break;
                case LogLevel.Information:
                    _logger.Information(message);
                    break;
                case LogLevel.Debug:
                    _logger.Error(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }
    }
}
