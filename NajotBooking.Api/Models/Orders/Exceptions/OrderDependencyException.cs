// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using Xeptions;

namespace NajotBooking.Api.Models.Orders.Exceptions
{
    public class OrderDependencyException : Xeption
    {
        public OrderDependencyException(Xeption innerException)
            : base(message: "Order dependency error occurred, contact support.", innerException)
        { }
    }
}