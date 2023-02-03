// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using Xeptions;

namespace NajotBooking.Api.Models.Orders.Exceptions
{
    public class OrderDependencyValidationException : Xeption
    {
        public OrderDependencyValidationException(Xeption innerException)
            : base(message: "Order dependency validation error occurred, fix the errors and try again.",
                innerException)
        { }
    }
}