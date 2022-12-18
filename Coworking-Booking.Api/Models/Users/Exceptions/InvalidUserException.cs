// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using Xeptions;

namespace Coworking_Booking.Api.Models.Users.Excaptions
{
    public class InvalidUserException : Xeption
    {
        public InvalidUserException()
          : base(message: "User is invalid.")
        { }
    }
}