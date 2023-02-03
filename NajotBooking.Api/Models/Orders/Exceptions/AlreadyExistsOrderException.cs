// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace NajotBooking.Api.Models.Orders.Exceptions
{
    public class AlreadyExistsOrderException : Xeption
    {
        public AlreadyExistsOrderException(Exception innerException)
            : base(message: "Order already exists.", innerException)
        { }
    }
}
