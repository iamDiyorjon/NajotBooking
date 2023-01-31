// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace NajotBooking.Api.Models.Users.Exceptions
{
    public class AlreadyExistsUserException : Xeption
    {
        public AlreadyExistsUserException(Exception innerException)
            : base(message: "Failed user dependency validation error occurred, fix errors and try again.", innerException)
        { }
    }
}