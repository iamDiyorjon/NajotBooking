//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Xeptions;

namespace NajotBooking.Api.Models.Seats.Exceptions
{
    public class LockedSeatException : Xeption
    {
        public LockedSeatException(Exception innerException)
            : base(message: "Seat is locked, please try again.", innerException)
        { }
    }
}
