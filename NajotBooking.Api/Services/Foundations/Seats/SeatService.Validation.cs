using System;
using System.Data;
using System.Net.Sockets;
using System.Reflection.Metadata;
using NajotBooking.Api.Models.Seats;
using NajotBooking.Api.Models.Seats.Exceptions;

namespace NajotBooking.Api.Services.Foundations.Seats
{
    public partial class SeatService
    {
        private void ValidateSeat(Seat seat)
        {
            ValidateSeatNotNull(seat);
            Validate(
                (Rule: IsInvalid(seat.Id), Parameter: nameof(Seat.Id)),
                (Rule: IsInvalid(seat.Number), Parameter: nameof(Seat.Number)),
                (Rule: IsInvalid(seat.Branch), Parameter: nameof(Seat.Branch)),
                (Rule: IsInvalid(seat.Floor), Parameter: nameof(Seat.Floor)),
                (Rule: IsInvalid(seat.Price), Parameter: nameof(Seat.Price)),
                (Rule: IsInvalid(seat.CreatedDate), Parameter: nameof(Seat.CreatedDate)),
                (Rule: IsInvalid(seat.UpdatedDate), Parameter: nameof(Seat.UpdatedDate)),
                (Rule: IsNotRecent(seat.CreatedDate), Parameter: nameof(Seat.CreatedDate)),

                (Rule: IsNotSame(
                        firstDate: seat.CreatedDate,
                        secondDate: seat.UpdatedDate,
                        secondDateName: nameof(Seat.UpdatedDate)),

                Parameter: nameof(Seat.CreatedDate)));
        }

        private void ValidateAginstStorageSeatOnModify(Seat inputSeat, Seat storageSeat)
        {
            ValidateStorageSeat(storageSeat, inputSeat.Id);

            Validate(
                (Rule: IsNotSame(
                    firstDate: inputSeat.CreatedDate,
                    secondDate: storageSeat.CreatedDate,
                    secondDateName: nameof(Seat.CreatedDate)),
                Parameter: nameof(Seat.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputSeat.UpdatedDate,
                    secondDate: storageSeat.UpdatedDate,
                    secondDateName: nameof(Seat.UpdatedDate)),
                Parameter: nameof(Seat.UpdatedDate)));
        }

        private void ValidateSeatOnModify(Seat seat)
        {
            ValidateSeatNotNull(seat);

            Validate(
                (Rule: IsInvalid(seat.Id), Parameter: nameof(Seat.Id)),
                (Rule: IsInvalid(seat.Number), Parameter: nameof(Seat.Number)),
                (Rule: IsInvalid(seat.Branch), Parameter: nameof(Seat.Branch)),
                (Rule: IsInvalid(seat.Floor), Parameter: nameof(Seat.Floor)),
                (Rule: IsInvalid(seat.Price), Parameter: nameof(Seat.Price)),
                (Rule: IsInvalid(seat.CreatedDate), Parameter: nameof(Seat.CreatedDate)),
                (Rule: IsInvalid(seat.UpdatedDate), Parameter: nameof(Seat.UpdatedDate)),
                (Rule: IsNotRecent(seat.UpdatedDate), Parameter: nameof(Seat.UpdatedDate)),

                (Rule: IsSame(
                        firstDate: seat.UpdatedDate,
                        secondDate: seat.CreatedDate,
                        secondDateName: nameof(Seat.CreatedDate)),

                Parameter: nameof(Seat.UpdatedDate)));
        }

        private static void ValidateStorageSeatExists(Seat maybeSeat, Guid seatId)
        {
            if (maybeSeat is null)
            {
                throw new NotFoundSeatException(seatId);
            }
        }

        private void ValidateSeatId(Guid seatId) =>
            Validate((Rule: IsInvalid(seatId), Parameter: nameof(Seat.Id)));

        private static dynamic IsInvalid(int floor) => new
        {
            Condition = floor == default,
            Message = "Number is required"
        };

        private static dynamic IsInvalid(decimal price) => new
        {
            Condition = price == default,
            Message = "Price is required"
        };

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Value is required"
        };

        private static dynamic IsInvalid<T>(T value) => new
        {
            Condition = IsEnumInvalid(value),
            Message = "Value is not recognized"
        };

        private static bool IsEnumInvalid<T>(T value)
        {
            bool isDefined = Enum.IsDefined(typeof(T), value);

            return isDefined is false;
        }

        private dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime = this.dateTimeBroker.GetCurrentDateTime();
            TimeSpan timeDifference = currentDateTime.Subtract(date);

            return timeDifference.TotalSeconds is > 60 or < 0;
        }

        private static void ValidateSeatNotNull(Seat seat)
        {
            if (seat is null)
            {
                throw new NullSeatException();
            }
        }

        private void ValidateStorageSeat(Seat maybeSeat, Guid seatId)
        {
            if (maybeSeat is null)
            {
                throw new NotFoundSeatException(seatId);
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
