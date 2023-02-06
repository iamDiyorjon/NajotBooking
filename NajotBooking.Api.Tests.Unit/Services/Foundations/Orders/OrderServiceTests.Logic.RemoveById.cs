// ---------------------------------------------------------------
// Copyright (c) Coalition Of The THE STANDART SHARPISTS
// Free To Use To Book Places In Coworking Zones
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using NajotBooking.Api.Models.Orders;
using Xunit;

namespace NajotBooking.Api.Tests.Unit.Services.Foundations.Orders
{
    public partial class OrderServiceTests
    {
        [Fact]
        public async Task ShouldRemoveOderByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputOrderId = randomId;
            Order randomOrder = CreateRandomOrder();
            Order storageOrder = randomOrder;
            Order expectedInputOrder = storageOrder;
            Order deletedOrder = expectedInputOrder;
            Order expectedOrder = deletedOrder.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOrderById(inputOrderId)).ReturnsAsync(storageOrder);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteOrderAsync(expectedInputOrder)).ReturnsAsync(deletedOrder);

            // when
            ValueTask<Order> actualOrder =
                this.orderService.RemoveOrderByIdAsync(inputOrderId);

            // then
            actualOrder.Should().BeEquivalentTo(expectedOrder);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOrderById(inputOrderId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteOrderAsync(expectedInputOrder), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
