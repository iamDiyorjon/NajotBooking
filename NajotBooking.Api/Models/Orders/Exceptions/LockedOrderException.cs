// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace NajotBooking.Api.Models.Orders.Exceptions
{
    public class LockedOrderException : Xeption
    {
        public LockedOrderException(Exception innerException)
            : base(message: "Order is locked, please try again.", innerException)
        { }
    }
}
