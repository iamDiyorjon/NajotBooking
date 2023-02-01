// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using Xeptions;

namespace NajotBooking.Api.Services.Foundations.Orders.Exceptions
{
    public class OrderValidationException : Xeption
    {
        public OrderValidationException(Xeption innerException)
            : base(message: "Order error occured, fix the errors and try again.", innerException)
        { }
    }
}