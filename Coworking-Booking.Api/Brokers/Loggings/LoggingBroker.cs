using Microsoft.Extensions.Logging;
using System;

namespace Coworking_Booking.Api.Brokers.Loggings
{
    public class LoggingBroker : ILoggingBroker
    {
        private ILogger<LoggingBroker> logger;

        public LoggingBroker(ILogger<LoggingBroker> logger) =>
            this.logger = logger;

        public void LogError(Exception exception) =>
            this.logger.LogError(exception.Message, exception);

        public void LogCritical(Exception exception) =>
            this.logger.LogCritical(exception, exception.Message);
    }
}