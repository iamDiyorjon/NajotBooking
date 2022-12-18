using Coworking_Booking.Api.Models.Seats.Exceptions;
using Coworking_Booking.Api.Models.Seats;
using System;
using System.Reflection.Metadata;
using System.Data;

namespace Coworking_Booking.Api.Services.Foundations.Seats
{
    public partial class SeatService
    {
        private void ValidateSeat(Seat seat)
        {
            ValidateSeatNotNull(seat);
            Validate(
                (Rule: IsInvalid(seat.Id), Parameter: nameof(Seat.Id)),
                (Rule: IsInvalid(seat.UserId), Parameter: nameof(Seat.Id)),
                (Rule: IsInvalid(seat.Floor), Parameter: nameof(Seat.Floor)),
                (Rule: IsInvalid(seat.SeatNumber), Parameter: nameof(Seat.SeatNumber)),
                (Rule: IsInvalid(seat.CraetedDate), Parameter: nameof(Seat.CraetedDate)),
                (Rule: IsInvalid(seat.UpdatedDate), Parameter: nameof(Seat.UpdatedDate)));
        }
        private void ValidateSeatOnModify(Seat seat)
        {
            ValidateSeatNotNull(seat);
            Validate(
                 (Rule: IsInvalid(seat.Id), Parameter: nameof(Seat.Id)),
                 (Rule: IsInvalid(seat.UserId), Parameter: nameof(Seat.Id)),
                 (Rule: IsInvalid(seat.Floor), Parameter: nameof(Seat.Floor)),
                 (Rule: IsInvalid(seat.SeatNumber), Parameter: nameof(Seat.SeatNumber)),
                 (Rule: IsInvalid(seat.CraetedDate), Parameter: nameof(Seat.CraetedDate)),
                 (Rule: IsInvalid(seat.UpdatedDate), Parameter: nameof(Seat.UpdatedDate)));
        }

        private void ValidateStorageSeat(Seat maybeSeat,Guid seatId)
        {
            if(maybeSeat is null )
            {
                throw new NotFoundUserException(seatId);
            }
        }
        private static dynamic IsInvalid(int floor) => new
        {
            Condition = floor == default,
            Message = "Number is required"
        };

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required"
        };
        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };
        private static void ValidateSeatNotNull(Seat seat)
        {
            if (seat is null)
            {
                throw new NullSeatException();
            }
        }
        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidSeatException = new InvalidSeatException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidSeatException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidSeatException.ThrowIfContainsErrors();
        }
    }
}
