// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Coworking_Booking.Api.Models.Users.Excaptions
{
    public class FailedUserStorageException: Xeption
    {
        public FailedUserStorageException(Exception innerException)
            : base(message: "Failed user storage error occurred, contact support.", innerException)
        { }
    }
}