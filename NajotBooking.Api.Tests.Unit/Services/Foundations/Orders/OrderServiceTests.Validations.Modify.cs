// ---------------------------------------------------------------
// Copyright (c) Coalition Of The THE STANDART SHARPISTS
// Free To Use To Book Places In Coworking Zones
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NajotBooking.Api.Models.Orders;
using NajotBooking.Api.Models.Orders.Exceptions;
using Xunit;

namespace NajotBooking.Api.Tests.Unit.Services.Foundations.Orders
{
    public partial class OrderServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfOrderIsNullAndLogItAsync()
        {
            // given
            Order nullOrder = null;
            var nullOrderException = new NullOrderException();

            var expectedOrderValidationException =
                new OrderValidationException(nullOrderException);

            // when
            ValueTask<Order> modifyOrderTask =
                this.orderService.ModifyOrderAsync(nullOrder);

            OrderValidationException actualOrderValidationException =
                await Assert.ThrowsAsync<OrderValidationException>(
                    modifyOrderTask.AsTask);

            // then
            actualOrderValidationException.Should().BeEquivalentTo(expectedOrderValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOrderValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOrderAsync(It.IsAny<Order>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
