// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace NajotBooking.Api.Models.Orders.Exceptions
{
    public class OrderServiceException : Xeption
    {
        public OrderServiceException(Exception innerException)
            : base(message: "Profile service occurred, contact support.", innerException)
        { }
    }
}
