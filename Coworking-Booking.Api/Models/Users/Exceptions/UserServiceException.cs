// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace NajotBooking.Api.Models.Users.Excaptions
{
    public class UserServiceException: Xeption
    {
        public UserServiceException(Exception innerException)
            : base(message: "User service error occured, contact support.", innerException)
        { }
    }
}