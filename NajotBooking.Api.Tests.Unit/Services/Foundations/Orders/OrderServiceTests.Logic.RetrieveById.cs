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
        public async Task ShouldRetrieveOrderByIdAsync()
        {
            // given
            Guid randomOrderId = Guid.NewGuid();
            Guid inputOrderId = randomOrderId;
            Order randomOrder = CreateRandomOrder();
            Order storageOrder = randomOrder;
            Order expectedOrder = storageOrder.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOrderByIdAsync(inputOrderId))
                    .ReturnsAsync(storageOrder);

            // when
            Order actualOrder =
                await this.orderService.RetrieveOrderByIdAsync(inputOrderId);

            // then
            actualOrder.Should().BeEquivalentTo(expectedOrder);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOrderByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
