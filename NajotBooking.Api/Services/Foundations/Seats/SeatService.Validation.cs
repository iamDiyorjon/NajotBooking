﻿using System;
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
                (Rule: IsInvalid(seat.CraetedDate), Parameter: nameof(Seat.CraetedDate)),
                (Rule: IsInvalid(seat.UpdatedDate), Parameter: nameof(Seat.UpdatedDate)));
        }

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
