// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using NajotBooking.Api.Models.Orders;
using NajotBooking.Api.Models.Orders.Exceptions;
using Xunit;

namespace NajotBooking.Api.Tests.Unit.Services.Foundations.Orders
{
    public partial class OrderServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Order someOrder = CreateRandomOrder();
            SqlException sqlException = CreateSqlException();

            var failedOrderStorageException =
                new FailedOrderStorageException(sqlException);

            var expectedOrderDependencyException =
                new OrderDependencyException(failedOrderStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertOrderAsync(It.IsAny<Order>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Order> addOrderTask =
                this.orderService.AddOrderAsync(someOrder);

            OrderDependencyException actualOrderDependencyException =
                await Assert.ThrowsAsync<OrderDependencyException>(addOrderTask.AsTask);

            // then
            actualOrderDependencyException.Should().BeEquivalentTo(expectedOrderDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOrderAsync(It.IsAny<Order>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExpressionAs(
                    expectedOrderDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
