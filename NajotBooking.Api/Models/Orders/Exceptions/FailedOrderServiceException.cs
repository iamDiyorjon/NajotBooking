// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace NajotBooking.Api.Models.Orders.Exceptions
{
    public class FailedOrderServiceException : Xeption
    {
        public FailedOrderServiceException(Exception innerException)
            : base(message: "Failed profile service occurred, please contact support", innerException)
        { }
    }
}
