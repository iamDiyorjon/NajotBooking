using System;
using Xeptions;

namespace NajotBooking.Api.Models.Seats.Exceptions
{
    public class FailedSeatStorageException : Xeption
    {
        public FailedSeatStorageException(Exception innerException)
            : base(message: " Failed seat storage error occured, contact support.", innerException)
        { }
    }
}
