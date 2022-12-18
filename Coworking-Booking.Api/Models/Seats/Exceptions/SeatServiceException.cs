﻿using System;
using Xeptions;

namespace Coworking_Booking.Api.Models.Seats.Exceptions
{
    public class SeatServiceException : Xeption
    {
        public SeatServiceException(Exception innerException)
            : base(message: "Seat service error occured, contact support.", innerException)
        { }
    }
}
