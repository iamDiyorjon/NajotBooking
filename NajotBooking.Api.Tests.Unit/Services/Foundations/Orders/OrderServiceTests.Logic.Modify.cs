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
        public async Task ShouldModifyOrderAsync()
        {
            //given
            Order randomOrder = CreateRandomOrder();
            randomOrder.EndDate = GetAfterRandomDateTime(randomOrder.StartDate);
            randomOrder.Duration = GetRandomNumber();
            Order inputOrder = randomOrder;
            Order storageOrder = inputOrder.DeepClone();
            Order updateOrder = inputOrder;
            Order expectedOrder = updateOrder.DeepClone();
            Guid orderId = inputOrder.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectOrderByIdAsync(orderId)).ReturnsAsync(storageOrder);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateOrderAsync(inputOrder)).ReturnsAsync(updateOrder);

            //when
            Order actualOrder =
                await this.orderService.ModifyOrderAsync(inputOrder);

            //then
            actualOrder.Should().BeEquivalentTo(expectedOrder);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOrderByIdAsync(orderId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOrderAsync(inputOrder), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
