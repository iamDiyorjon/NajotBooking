// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace NajotBooking.Api.Models.Orders.Exceptions
{
    public class NotFoundOrderException : Xeption
    {
        public NotFoundOrderException(Guid orderId)
           : base(message: $"Couldn't find order with id: {orderId}.")
        { }
    }
}
