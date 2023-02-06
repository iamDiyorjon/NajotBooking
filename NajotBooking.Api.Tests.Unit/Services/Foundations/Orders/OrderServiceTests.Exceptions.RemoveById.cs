// ---------------------------------------------------------------
// Copyright (c) Coalition Of The THE STANDART SHARPISTS
// Free To Use To Book Places In Coworking Zones
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NajotBooking.Api.Models.Orders;
using NajotBooking.Api.Models.Orders.Exceptions;
using Xunit;

namespace NajotBooking.Api.Tests.Unit.Services.Foundations.Orders
{
    public partial class OrderServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someOrderId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedOrderException =
                new LockedOrderException(databaseUpdateConcurrencyException);

            var expectedOrderDependencyValidationException =
                new OrderDependencyValidationException(lockedOrderException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOrderByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Order> removeOrderByIdTask =
                this.orderService.RemoveOrderByIdAsync(someOrderId);

            OrderDependencyValidationException actualOrderDependencyValidationException =
                await Assert.ThrowsAsync<OrderDependencyValidationException>(
                    removeOrderByIdTask.AsTask);

            // then
            actualOrderDependencyValidationException.Should().BeEquivalentTo(
                expectedOrderDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOrderByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedOrderDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteOrderAsync(It.IsAny<Order>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
