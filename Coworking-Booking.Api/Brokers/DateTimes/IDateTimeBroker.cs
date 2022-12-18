using System;

namespace Coworking_Booking.Api.Brokers.DateTimes
{
    public interface IDateTimeBroker
    {
        DateTimeOffset GetCurrentDateTime();
    }
}
