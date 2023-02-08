using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NajotBooking.Api.Models.Seats;
using NajotBooking.Api.Models.Seats.Exceptions;
using Xunit;

namespace NajotBooking.Api.Tests.Unit.Services.Foundations.Seats
{
    public partial class SeatServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfSeatIsNullAndLogItAsync()
        {
            //given
            Seat nullSeat = null;
            var nullSeatException = new NullSeatException();

            var expectedSeatValidationException =
                new SeatValidationException(nullSeatException);

            //when
            ValueTask<Seat> modifySeatTask =
                this.seatService.ModifySeatAsync(nullSeat);

            SeatValidationException actualSeatValidationException =
                await Assert.ThrowsAsync<SeatValidationException>(modifySeatTask.AsTask);

            //then
            actualSeatValidationException.Should()
                .BeEquivalentTo(expectedSeatValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSeatValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSeatByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSeatAsync(It.IsAny<Seat>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
