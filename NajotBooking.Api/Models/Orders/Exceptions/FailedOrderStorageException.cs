// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace NajotBooking.Api.Models.Orders.Exceptions
{
    public class FailedOrderStorageException : Xeption
    {
        public FailedOrderStorageException(Exception innerException)
            : base(message: "Failed Order error occurred, contact support.", innerException)
        { }
    }
}