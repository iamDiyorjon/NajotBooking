// ---------------------------------------------------------------
// Copyright (c) Coalition Of The THE STANDART SHARPISTS
// Free To Use To Book Places In Coworking Zones
// ---------------------------------------------------------------

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using NajotBooking.Api.Models.Orders.Exceptions;
using System;
using Xunit;
using Xunit.Sdk;

namespace NajotBooking.Api.Tests.Unit.Services.Foundations.Orders
{
    public partial class OrderServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            //given
            SqlException sqlException = CreateSqlException();
            var failedOrderServiceException = new FailedOrderServiceException(sqlException);

            var expectedOrderDependencyException =
                new OrderDependencyException(failedOrderServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOrders()).Throws(sqlException);

            //when
            Action retrieveAllOrderAction = () =>
                this.orderService.RetrieveAllOrders();

            OrderDependencyException actualOrderDependencyException =
                Assert.Throws<OrderDependencyException>(retrieveAllOrderAction);

            //then
            actualOrderDependencyException.Should().BeEquivalentTo(expectedOrderDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllOrders(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedOrderDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
