// ---------------------------------------------------------------
// Copyright (c) Coalition Of The THE STANDART SHARPISTS
// Free To Use To Book Places In Coworking Zones
// ---------------------------------------------------------------

using System;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guid someOrderId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedOrderStorageException =
                new FailedOrderStorageException(sqlException);

            var expectedOrderDependencyException =
                new OrderDependencyException(failedOrderStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOrderByIdAsync(someOrderId))
                    .ThrowsAsync(sqlException);

            //when
            ValueTask<Order> retrieveOrderByIdTask =
                this.orderService.RetrieveOrderByIdAsync(someOrderId);

            OrderDependencyException actualOrderDependencyException =
                await Assert.ThrowsAsync<OrderDependencyException>(
                    retrieveOrderByIdTask.AsTask);

            //then
            actualOrderDependencyException.Should().BeEquivalentTo(
                   expectedOrderDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOrderByIdAsync((It.IsAny<Guid>())), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExpressionAs(
                    expectedOrderDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            //given
            Guid someOrderId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedOrderServiceException =
                new FailedOrderServiceException(serviceException);

            var expectedOrderServiceException =
                new OrderServiceException(failedOrderServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOrderByIdAsync(someOrderId))
                    .ThrowsAsync(serviceException);

            //when
            ValueTask<Order> retrieveOrderByIdTask =
                this.orderService.RetrieveOrderByIdAsync(someOrderId);

            OrderServiceException actualOrderServiceException =
                 await Assert.ThrowsAsync<OrderServiceException>(
                     retrieveOrderByIdTask.AsTask);

            //then
            actualOrderServiceException.Should()
                .BeEquivalentTo(expectedOrderServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOrderByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedOrderServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
