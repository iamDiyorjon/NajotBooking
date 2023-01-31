// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using Xeptions;

namespace NajotBooking.Api.Models.Users.Excaptions
{
    public class UserValidationException: Xeption
    {
        public UserValidationException(Xeption innerExeption)
            : base(message: "User validation error occured, fix the errors and try again.", innerExeption)
        { }
    }
}