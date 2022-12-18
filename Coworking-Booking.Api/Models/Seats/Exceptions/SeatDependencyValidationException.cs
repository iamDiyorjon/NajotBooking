using Xeptions;

namespace Coworking_Booking.Api.Models.Seats.Exceptions
{
    public class SeatDependencyValidationException : Xeption
    {
        public SeatDependencyValidationException(Xeption innerException)
            : base(message: "Seat dependency validation error occured, fix the error and try again. ", innerException)
        { }
    }
}
