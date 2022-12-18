using System;
namespace Coworking_Booking.Api.Brokers.DateTimes
{
    public class DateTimeBroker : IDateTimeBroker
    {
        DateTimeOffset IDateTimeBroker.GetCurrentDateTime() =>
             DateTimeOffset.UtcNow;
    }
}