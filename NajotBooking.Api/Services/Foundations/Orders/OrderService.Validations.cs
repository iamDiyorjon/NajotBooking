// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System;
using NajotBooking.Api.Models.Orders;
using NajotBooking.Api.Models.Orders.Exceptions;

namespace NajotBooking.Api.Services.Foundations.Orders
{
    public partial class OrderService
    {
        private static void ValidateOrder(Order order)
        {
            ValidateOrderNotNull(order);

            Validate(
                (Rule: IsInvalid(order.Id), Parameter: nameof(Order.Id)),
                (Rule: IsInvalid(order.SeatId), Parameter: nameof(Order.SeatId)),
                (Rule: IsInvalid(order.UserId), Parameter: nameof(Order.UserId)),
                (Rule: IsInvalid(order.StartDate), Parameter: nameof(Order.StartDate)),
                (Rule: IsInvalid(order.EndDate), Parameter: nameof(Order.EndDate)),
                (Rule: IsInvalid(order.Duration), Parameter: nameof(Order.Duration)),
                (Rule: IsNotLessThan(
                    startDate: order.StartDate,
                    endDate: order.EndDate,
                    dateName: nameof(Order.EndDate)),

                    Parameter: nameof(Order.StartDate))
                );
        }

        private static void ValidateOrderNotNull(Order order)
        {
            if (order is null)
            {
                throw new NullOrderException();
            }
        }

        private void ValidateOrderOnModify(Order order)
        {
            ValidateOrderNotNull(order);

            Validate(
                (Rule: IsInvalid(order.SeatId), Parameter: nameof(Order.SeatId)),
                (Rule: IsInvalid(order.UserId), Parameter: nameof(Order.UserId)),
                (Rule: IsInvalid(order.StartDate), Parameter: nameof(Order.StartDate)),
                (Rule: IsInvalid(order.EndDate), Parameter: nameof(Order.EndDate)),
                (Rule: IsInvalid(order.Duration), Parameter: nameof(Order.Duration)),
                (Rule: IsNotLessThan(
                    startDate: order.StartDate,
                    endDate: order.EndDate,
                    dateName: nameof(Order.EndDate)),

                    Parameter: nameof(Order.StartDate))
                );
        }

        private static void ValidateStorageOrderExists(Order maybeOrder, Guid orderId)
        {
            if (maybeOrder is null)
            {
                throw new NotFoundOrderException(orderId);
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            InvalidOrderException invalidOrderException = new InvalidOrderException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidOrderException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidOrderException.ThrowIfContainsErrors();
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(int number) => new
        {
            Condition = number < 0,
            Message = "Value is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Value is required"
        };

        private void ValidationOrderId(Guid orderId) =>
            Validate((Rule: IsInvalid(orderId), Parameter: nameof(Order.Id)));

        private static dynamic IsNotLessThan(
           DateTimeOffset startDate,
           DateTimeOffset endDate,
           string dateName) => new
           {
               Condition = startDate >= endDate,
               Message = $"Date is not less than {dateName}"
           };
    }
}
