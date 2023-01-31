using Xeptions;

namespace NajotBooking.Api.Models.Seats.Exceptions
{
    public class InvalidSeatException : Xeption
    {
        public InvalidSeatException()
            : base(message: "Invalid seat. Please correct the errors and try again. ")
        { }
    }
}
