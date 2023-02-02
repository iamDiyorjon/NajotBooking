using System;
using Xeptions;

namespace NajotBooking.Api.Models.Users.Exceptions
{
    public class InvalidUserReferenceException : Xeption
    {
        public InvalidUserReferenceException(Exception innerException)
            : base(message: "Invalid user reference error occurred.", innerException)
        { }
    }
}