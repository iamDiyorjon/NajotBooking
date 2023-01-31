using Xeptions;

namespace NajotBooking.Api.Models.Seats.Exceptions
{
    public class SeatValidationException : Xeption
    {
        public SeatValidationException(Xeption innerException)
            : base(message: "Seat validation errors occured, please try again",
                  innerException)
        { }
    }
}
