// ---------------------------------------------------------------
// Copyright (c) Coalition Of The THE STANDART SHARPISTS
// Free To Use To Book Places In Coworking Zones
// ---------------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionOnModifyIfOrderIsNullAndLogItAsync()
        {
            // given
            Order nullOrder = null;
            var nullOrderException = new NullOrderException();

            var expectedOrderValidationException =
                new OrderValidationException(nullOrderException);

            // when
            ValueTask<Order> modifyOrderTask =
                this.orderService.ModifyOrderAsync(nullOrder);

            OrderValidationException actualOrderValidationException =
                await Assert.ThrowsAsync<OrderValidationException>(
                    modifyOrderTask.AsTask);

            // then
            actualOrderValidationException.Should().BeEquivalentTo(expectedOrderValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOrderValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOrderAsync(It.IsAny<Order>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        public async Task ShouldThrowValidationExceptionOnModifyIfOrderIsInvalidAndLogItAsync(
            int invalidNumber)
        {
            // given
            var invalidOrder = new Order
            {
                Duration = invalidNumber
            };

            var invalidOrderException = new InvalidOrderException();

            invalidOrderException.AddData(
                key: nameof(Order.SeatId),
                values: "Id is required");

            invalidOrderException.AddData(
                key: nameof(Order.UserId),
                values: "Id is required");

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
            ValueTask<Order> modifyOrderTask =
                this.orderService.ModifyOrderAsync(invalidOrder);

            OrderValidationException actualOrderValidationException =
                await Assert.ThrowsAsync<OrderValidationException>(modifyOrderTask.AsTask);

            // then
            actualOrderValidationException.Should().BeEquivalentTo(expectedOrderValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedOrderValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOrderAsync(It.IsAny<Order>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
