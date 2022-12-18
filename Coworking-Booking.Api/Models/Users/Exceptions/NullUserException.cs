// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using Xeptions;

namespace Coworking_Booking.Api.Models.Users.Excaptions
{
    public class NullUserException : Xeption
    {
        public NullUserException()
            : base(message: "User is null.")
        { }
    }
}