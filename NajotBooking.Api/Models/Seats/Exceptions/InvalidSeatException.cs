using Xeptions;

namespace NajotBooking.Api.Models.Seats.Exceptions
{
    public class InvalidSeatException : Xeption
    {
        public InvalidSeatException()
            : base(message: "Seat is invalid. ")
        { }
    }
}
