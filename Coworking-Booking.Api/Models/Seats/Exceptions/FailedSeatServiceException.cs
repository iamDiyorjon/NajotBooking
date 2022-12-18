using System;
using Xeptions;

namespace Coworking_Booking.Api.Models.Seats.Exceptions
{
    public class FailedSeatServiceException : Xeption
    {
        public FailedSeatServiceException(Exception innerException)
            : base(message: "Failed seat service occured, please contact support", innerException)
        { }
    }
}
