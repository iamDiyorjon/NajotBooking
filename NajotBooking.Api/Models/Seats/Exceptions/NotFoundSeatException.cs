using System;
using Xeptions;

namespace NajotBooking.Api.Models.Seats.Exceptions
{
    public class NotFoundSeatException : Xeption
    {
        public NotFoundSeatException(Guid seatId)
            : base(message: $"Couldn't find seat with it {seatId}.")
        { }
    }
}
