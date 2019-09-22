using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Service.Infrastructure
{
    public interface ILogger
    {
        void LogException(Exception ex);
        void LogMessage(string message, LogLevel lvl);
    }
}
