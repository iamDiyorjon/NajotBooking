// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NajotBooking.Api.Models.Orders;
using NajotBooking.Api.Models.Orders.Exceptions;
using Xunit;

namespace NajotBooking.Api.Tests.Unit.Services.Foundations.Orders
{
    public partial class OrderServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfInputIsNullAndLogItAsync()
        {
            // given
            Order noOrder = null;
            var nullOrderException = new NullOrderException();

            var expectedOrderValidationException =
                new OrderValidationException(nullOrderException);

            // when
            ValueTask<Order> addOrderTask =
                this.orderService.AddOrderAsync(noOrder);

            OrderValidationException actualOrderValidationException =
                await Assert.ThrowsAsync<OrderValidationException>(addOrderTask.AsTask);

            // then
            actualOrderValidationException.Should().BeEquivalentTo(expectedOrderValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedOrderValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOrderAsync(It.IsAny<Order>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        public async Task ShouldThrowValidationExceptionOnAddIfOrderIsInvalidAndLogItAsync(
            int invalidNumber)
        {
            // given
            var invalidOrder = new Order
            {
                Duration = invalidNumber
            };

            var invalidOrderException = new InvalidOrderException();

            invalidOrderException.AddData(
                key: nameof(Order.Id),
                values: "Id is required");

            invalidOrderException.AddData(
                key: nameof(Order.SeatId),
                values: "Id is required");

            invalidOrderException.AddData(
                key: nameof(Order.UserId),
                values: "Id is required");

            invalidOrderException.AddData(
               key: nameof(Order.Price),
               values: "Price is required");

            invalidOrderException.AddData(
                key: nameof(Order.StartDate),

                values: new string[] {
                    "Value is required",
                    $"Date is not less than {nameof(Order.EndDate)}"});

            invalidOrderException.AddData(
                key: nameof(Order.EndDate),
                values: "Value is required");

            invalidOrderException.AddData(
                key: nameof(Order.Duration),
                values: "Value is required");

            OrderValidationException expectedOrderValidationException =
                new OrderValidationException(invalidOrderException);

            // when
            ValueTask<Order> addOrderTask =
                this.orderService.AddOrderAsync(invalidOrder);

            OrderValidationException actualOrderValidationException =
                await Assert.ThrowsAsync<OrderValidationException>(addOrderTask.AsTask);

            // then
            actualOrderValidationException.Should().BeEquivalentTo(expectedOrderValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedOrderValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOrderAsync(It.IsAny<Order>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfStartDateIsNotLessThanEndDateAndLogItAsync()
        {
            // given
            Order randomOrder = CreateRandomOrder();
            DateTimeOffset beforeRandomDateTime = GetBeforeRandomDateTime(randomOrder.StartDate);
            Order invalidOrder = randomOrder;
            randomOrder.EndDate = beforeRandomDateTime;
            var invalidOrderException = new InvalidOrderException();

            invalidOrderException.AddData(
                key: nameof(Order.StartDate),
                values: $"Date is not less than {nameof(Order.EndDate)}");

            var expectedOrderValidationException =
                new OrderValidationException(invalidOrderException);

            // when
            ValueTask<Order> addOrderTask =
                this.orderService.AddOrderAsync(invalidOrder);

            OrderValidationException actualOrderValidationException =
                await Assert.ThrowsAsync<OrderValidationException>(addOrderTask.AsTask);

            // then
            actualOrderValidationException.Should().BeEquivalentTo(expectedOrderValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedOrderValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOrderAsync(It.IsAny<Order>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
