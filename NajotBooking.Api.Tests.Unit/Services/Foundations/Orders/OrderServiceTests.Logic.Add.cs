// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

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
        public async Task ShouldAddOrderAsync()
        {
            // given 
            Order randomOrder = CreateRandomOrder();
            Order inputOrder = randomOrder;
            Order persistOrder = inputOrder;
            Order expectedOrder = persistOrder.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertOrderAsync(inputOrder))
                    .ReturnsAsync(persistOrder);

            // when
            Order order =
                await this.orderService.AddOrderAsync(inputOrder);

            // then
            order.Should().BeEquivalentTo(expectedOrder);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOrderAsync(inputOrder), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
