using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NajotBooking.Api.Models.Orders;
using NajotBooking.Api.Services.Foundations.Orders.Exceptions;
using Xunit;

namespace NajotBooking.Api.Tests.Unit.Services.Foundations.Orders
{
    public partial class OrderServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfInputIsNullAndLogItAsync()
        {
            // given
            Order noOrder = null;
            var nullOrderException = new NullOrderException();

            var expectedOrderValidationException =
                new OrderValidationException(nullOrderException);

            // when
            ValueTask<Order> addOrderTask =
                this.orderService.AddOrderAsync(noOrder);

            OrderValidationException actualOrderValidationException =
                await Assert.ThrowsAsync<OrderValidationException>(addOrderTask.AsTask);

            // then
            actualOrderValidationException.Should().BeEquivalentTo(expectedOrderValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExpressionAs(
                    expectedOrderValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertOrderAsync(It.IsAny<Order>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
