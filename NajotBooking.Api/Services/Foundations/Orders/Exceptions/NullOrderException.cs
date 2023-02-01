// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using Xeptions;

namespace NajotBooking.Api.Services.Foundations.Orders.Exceptions
{
    public class NullOrderException : Xeption
    {
        public NullOrderException() : base(message: "Order is null.")
        { }
    }
}