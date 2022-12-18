// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using Xeptions;

namespace Coworking_Booking.Api.Models.Users.Excaptions
{
    public class UserDependencyValidationException: Xeption
    {
        public UserDependencyValidationException(Xeption innerException)
            : base(message: "User dependency validation error occurred, fix the error and try again ", innerException)
        { }
    }
}