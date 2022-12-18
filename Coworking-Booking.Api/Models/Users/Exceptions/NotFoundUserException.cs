// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Coworking_Booking.Api.Models.Users.Excaptions
{
    public class NotFoundUserException : Xeption
    {
        public NotFoundUserException(Guid userId)
            : base(message: $"Could not find user with id:{userId}.")
        { }
    }
}