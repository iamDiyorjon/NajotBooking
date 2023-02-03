// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Order someOrder = CreateRandomOrder();
            someOrder.EndDate = GetAfterRandomDateTime(someOrder.StartDate);
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

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccursAndLogItAsync()
        {
            // given
            Order someOrder = CreateRandomOrder();
            someOrder.EndDate = GetAfterRandomDateTime(someOrder.StartDate);
            string someMessage = GetRandomString();
            var duplicateKeyException = new DuplicateKeyException(someMessage);

            var alreadyExistsOrderException =
                new AlreadyExistsOrderException(duplicateKeyException);

            var expectedOrderDependencyValidationException =
                new OrderDependencyValidationException(
                    alreadyExistsOrderException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertOrderAsync(It.IsAny<Order>()))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Order> addOrderTask =
                this.orderService.AddOrderAsync(someOrder);

            OrderDependencyValidationException actualOrderDependencyValidationException =
                await Assert.ThrowsAsync<OrderDependencyValidationException>(addOrderTask.AsTask);

            // then
            actualOrderDependencyValidationException.Should()
                .BeEquivalentTo(expectedOrderDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOrderAsync(It.IsAny<Order>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedOrderDependencyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDbConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Order someOrder = CreateRandomOrder();
            someOrder.EndDate = GetAfterRandomDateTime(someOrder.StartDate);
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedOrderException =
                new LockedOrderException(dbUpdateConcurrencyException);

            var expectedOrderDependencyValidationException =
                new OrderDependencyValidationException(lockedOrderException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertOrderAsync(It.IsAny<Order>()))
                    .ThrowsAsync(dbUpdateConcurrencyException);

            // when
            ValueTask<Order> addOrderTask =
                this.orderService.AddOrderAsync(someOrder);

            OrderDependencyValidationException actualOrderDependencyValidationException =
                await Assert.ThrowsAsync<OrderDependencyValidationException>(addOrderTask.AsTask);

            // then
            actualOrderDependencyValidationException.Should()
                .BeEquivalentTo(expectedOrderDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOrderAsync(It.IsAny<Order>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedOrderDependencyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Order someOrder = CreateRandomOrder();
            someOrder.EndDate = GetAfterRandomDateTime(someOrder.StartDate);
            var serviceException = new Exception();

            var failedOrderServiceException =
                new FailedOrderServiceException(serviceException);

            var expectedOrderServiceException =
                new OrderServiceException(failedOrderServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertOrderAsync(It.IsAny<Order>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Order> addOrderTask =
                this.orderService.AddOrderAsync(someOrder);

            OrderServiceException actualOrderServiceException =
                await Assert.ThrowsAsync<OrderServiceException>(addOrderTask.AsTask);

            // then
            actualOrderServiceException.Should().BeEquivalentTo(expectedOrderServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOrderAsync(It.IsAny<Order>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedOrderServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
