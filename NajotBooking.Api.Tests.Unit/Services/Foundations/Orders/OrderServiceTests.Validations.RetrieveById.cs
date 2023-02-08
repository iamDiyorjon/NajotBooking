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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidOrderId = Guid.Empty;
            InvalidOrderException invalidOrderException = new InvalidOrderException();

            invalidOrderException.AddData(
                key: nameof(Order.Id),
                values: "Id is required");

            OrderValidationException expectedOrderValidationException =
                new OrderValidationException(invalidOrderException);

            // when
            ValueTask<Order> retrieveOrderByIdTask =
                this.orderService.RetrieveOrderByIdAsync(invalidOrderId);

            OrderValidationException actaulOrderValidationException =
                await Assert.ThrowsAsync<OrderValidationException>(
                    retrieveOrderByIdTask.AsTask);

            // then
            actaulOrderValidationException.Should()
                .BeEquivalentTo(expectedOrderValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOrderValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOrderByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfOrderIsNotFoundAndLogItAsync()
        {
            //given
            Guid someOrderId = Guid.NewGuid();
            Order noOrder = null;

            var notFoundOrderException =
                new NotFoundOrderException(someOrderId);

            var expectedOrderValidationException =
                new OrderValidationException(notFoundOrderException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOrderByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noOrder);

            //when
            ValueTask<Order> retrieveOrderByIdTask =
                this.orderService.RetrieveOrderByIdAsync(someOrderId);

            OrderValidationException actualOrderValidationException =
                await Assert.ThrowsAsync<OrderValidationException>(
                    retrieveOrderByIdTask.AsTask);

            // then
            actualOrderValidationException.Should()
                .BeEquivalentTo(expectedOrderValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOrderByIdAsync(It.IsAny<Guid>()), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedOrderValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
