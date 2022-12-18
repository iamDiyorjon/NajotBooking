using Xeptions;

namespace Coworking_Booking.Api.Models.Seats.Exceptions
{
    public class SeatDependencyException : Xeption
    {
        public SeatDependencyException(Xeption innerException)
            : base(message: " Seat dependency error occured, contact support.", innerException)
        { }
    }
}
