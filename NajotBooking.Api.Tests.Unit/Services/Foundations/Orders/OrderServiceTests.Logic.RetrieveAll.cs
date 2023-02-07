// ---------------------------------------------------------------
// Copyright (c) Coalition Of The THE STANDART SHARPISTS
// Free To Use To Book Places In Coworking Zones
// ---------------------------------------------------------------

using FluentAssertions;
using Moq;
using NajotBooking.Api.Models.Orders;
using System.Linq;
using Xunit;

namespace NajotBooking.Api.Tests.Unit.Services.Foundations.Orders
{
    public partial class OrderServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllOrders()
        {
            //givem
            IQueryable<Order> randomOrders = CreateRandomOrders();
            IQueryable<Order> storageOrders = randomOrders;
            IQueryable<Order> expectedOrders = storageOrders;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOrders())
                    .Returns(storageOrders);

            //when
            IQueryable<Order> actualOrders = 
                this.orderService.RetrieveAllOrders();

            //then
            actualOrders.Should().BeEquivalentTo(expectedOrders);

            this.storageBrokerMock.Verify(broker => 
                broker.SelectAllOrders(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }
    }
}
