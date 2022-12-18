using System;
using Xeptions;

namespace Coworking_Booking.Api.Models.Seats.Exceptions
{
    public class AlreadyExistsSeatException : Xeption
    {
        public AlreadyExistsSeatException(Exception innerException)
            : base(message: "Seat with the same id already exists.", innerException)
        { }
    }
}
