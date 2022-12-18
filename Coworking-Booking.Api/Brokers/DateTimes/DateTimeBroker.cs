// ---------------------------------------------------------------
// Copyright (c) Coalition Of The THE STANDART SHARPISTS
// Free To Use To Book Places In Coworking Zones
// ---------------------------------------------------------------

using System;
namespace Coworking_Booking.Api.Brokers.DateTimes
{
    public class DateTimeBroker : IDateTimeBroker
    {
        DateTimeOffset IDateTimeBroker.GetCurrentDateTime() =>
             DateTimeOffset.UtcNow;
    }
}