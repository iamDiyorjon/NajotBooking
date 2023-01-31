using System;
using Xeptions;

namespace NajotBooking.Api.Models.Seats.Exceptions
{
    public class NotFoundUserException : Xeption
    {
        public NotFoundUserException(Guid seatId)
            : base(message: $"Couldn't find seat with it {seatId}.")
        { }
    }
}
