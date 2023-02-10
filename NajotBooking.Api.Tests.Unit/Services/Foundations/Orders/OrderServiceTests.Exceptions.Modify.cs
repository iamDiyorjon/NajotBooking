// ---------------------------------------------------------------
// Copyright (c) Coalition Of The THE STANDART SHARPISTS
// Free To Use To Book Places In Coworking Zones
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Order randomOrder = CreateRandomOrder();
            randomOrder.EndDate = GetAfterRandomDateTime(randomOrder.StartDate);
            randomOrder.Duration = GetRandomNumber();
            Order someOrder = randomOrder;
            Guid orderId = someOrder.Id;
            SqlException sqlException = CreateSqlException();

            var failedOrderStorageException =
                new FailedOrderStorageException(sqlException);

            var expectedOrderDependencyException =
                new OrderDependencyException(failedOrderStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOrderByIdAsync(orderId))
                    .Throws(sqlException);

            // when
            ValueTask<Order> modifyOrderTask =
                this.orderService.ModifyOrderAsync(someOrder);

            OrderDependencyException actualOrderDependencyException =
                await Assert.ThrowsAsync<OrderDependencyException>(
                     modifyOrderTask.AsTask);

            // then
            actualOrderDependencyException.Should().BeEquivalentTo(
                expectedOrderDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedOrderDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOrderByIdAsync(orderId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOrderAsync(someOrder), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            Order randomOrder = CreateRandomOrder();
            randomOrder.EndDate = GetAfterRandomDateTime(randomOrder.StartDate);
            randomOrder.Duration = GetRandomNumber();
            Order someOrder = randomOrder;
            Guid orderId = someOrder.Id;
            var databaseUpdateException = new DbUpdateException();

            var failedOrderStorageException =
                new FailedOrderStorageException(databaseUpdateException);

            var expectedOrderDependencyException =
                new OrderDependencyException(failedOrderStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOrderByIdAsync(orderId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Order> modifyOrderTask =
                this.orderService.ModifyOrderAsync(someOrder);

            OrderDependencyException actualOrderDependencyException =
                await Assert.ThrowsAsync<OrderDependencyException>(
                     modifyOrderTask.AsTask);

            // then
            actualOrderDependencyException.Should().BeEquivalentTo(
                expectedOrderDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOrderByIdAsync(orderId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOrderDependencyException))), Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Order randomOrder = CreateRandomOrder();
            randomOrder.EndDate = GetAfterRandomDateTime(randomOrder.StartDate);
            randomOrder.Duration = GetRandomNumber();
            Order someOrder = randomOrder;
            Guid orderId = someOrder.Id;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedOrderException =
                new LockedOrderException(databaseUpdateConcurrencyException);

            var expectedOrderDependencyValidationException =
                new OrderDependencyValidationException(lockedOrderException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOrderByIdAsync(orderId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Order> modifyOrderTask =
                this.orderService.ModifyOrderAsync(someOrder);

            OrderDependencyValidationException actualOrderDependencyValidationException =
                await Assert.ThrowsAsync<OrderDependencyValidationException>(
                     modifyOrderTask.AsTask);

            // then
            actualOrderDependencyValidationException.Should().BeEquivalentTo(
                expectedOrderDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOrderByIdAsync(orderId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOrderDependencyValidationException))), Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            Order randomOrder = CreateRandomOrder();
            randomOrder.EndDate = GetAfterRandomDateTime(randomOrder.StartDate);
            randomOrder.Duration = GetRandomNumber();
            Order someOrder = randomOrder;
            Guid orderId = someOrder.Id;
            var serviceException = new Exception();

            var failedOrderException =
                new FailedOrderServiceException(serviceException);

            var expectedOrderServiceException =
                new OrderServiceException(failedOrderException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOrderByIdAsync(orderId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Order> modifyOrderTask =
                this.orderService.ModifyOrderAsync(someOrder);

            OrderServiceException actualOrderServiceException =
                await Assert.ThrowsAsync<OrderServiceException>(
                     modifyOrderTask.AsTask);

            // then
            actualOrderServiceException.Should().BeEquivalentTo(
                expectedOrderServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOrderByIdAsync(orderId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOrderServiceException))), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
