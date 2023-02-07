// ---------------------------------------------------------------
// Copyright (c) Coalition Of The THE STANDART SHARPISTS
// Free To Use To Book Places In Coworking Zones
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
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidOrderId = Guid.Empty;
            var invalidOrderException = new InvalidOrderException();

            invalidOrderException.AddData(
                key: nameof(Order.Id),
                values: "Id is required");

            var expectedOrderValidationException =
                new OrderValidationException(invalidOrderException);

            // when
            ValueTask<Order> removeOrderByIdTask =
                this.orderService.RemoveOrderByIdAsync(invalidOrderId);

            OrderValidationException actualOrderValidationException =
                await Assert.ThrowsAsync<OrderValidationException>(removeOrderByIdTask.AsTask);

            // then
            actualOrderValidationException.Should().BeEquivalentTo(expectedOrderValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedOrderValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteOrderAsync(It.IsAny<Order>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRemoveIfOrderIsNotFoundAndLogItAsync()
        {
            //given
            Guid randomOrderId = Guid.NewGuid();
            Guid inputOrderId = randomOrderId;
            Order noOrder = null;

            var notFoundOrderException =
                new NotFoundOrderException(inputOrderId);

            var expectedOrderValidationException =
                new OrderValidationException(notFoundOrderException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOrderByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noOrder);

            //when
            ValueTask<Order> removeOrderByIdTask =
                this.orderService.RemoveOrderByIdAsync(inputOrderId);

            OrderValidationException actualOrderValidationException =
                await Assert.ThrowsAsync<OrderValidationException>(
                    removeOrderByIdTask.AsTask);

            //then
            actualOrderValidationException.Should()
                .BeEquivalentTo(expectedOrderValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOrderByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedOrderValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteOrderAsync(It.IsAny<Order>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
