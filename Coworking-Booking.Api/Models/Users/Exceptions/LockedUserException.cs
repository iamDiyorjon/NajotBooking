// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Coworking_Booking.Api.Models.Users.Excaptions
{
    public class LockedUserException: Xeption
    {
        public LockedUserException(Exception innerException)
            : base(message: "User is locked, please try again.", innerException)
        { }
    }
}