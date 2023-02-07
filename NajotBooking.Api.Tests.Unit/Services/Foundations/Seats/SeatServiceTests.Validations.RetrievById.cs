using Moq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System;
using Xunit;
using NajotBooking.Api.Models.Seats.Exceptions;
using NajotBooking.Api.Models.Seats;
using FluentAssertions;

namespace NajotBooking.Api.Tests.Unit.Services.Foundations.Seats
{
    public partial class SeatServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            //given 
            var invalidSeatId = Guid.Empty;

            var invalidSeatException =
                new InvalidSeatException();

            invalidSeatException.AddData(
                key: nameof(Seat.Id),
                values: "Id is required");

            var expectedSeatValidationException = new
                SeatValidationException(invalidSeatException);

            //when 
            ValueTask<Seat> retrieveSeatByIdTask =
                this.seatService.RetrieveSeatByIdAsync(invalidSeatId);

            SeatValidationException actualSeatValidationException =
                await Assert.ThrowsAsync<SeatValidationException>(retrieveSeatByIdTask.AsTask);

            //then
            actualSeatValidationException.Should().BeEquivalentTo(expectedSeatValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSeatValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSeatByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfSeatNotFoundAndLogItAsync()
        {
            //given
            Guid someSeatId = Guid.NewGuid();
            Seat noSeat = null;

            var notFoundSeatValidationException =
                new NotFoundSeatException(someSeatId);

            var expectedValidationException =
                new SeatValidationException(notFoundSeatValidationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSeatByIdAsync(It.IsAny<Guid>())).ReturnsAsync(noSeat);

            //when
            ValueTask<Seat> retrieveByIdSeatTask =
                this.seatService.RetrieveSeatByIdAsync(someSeatId);

            SeatValidationException actualValidationException =
                await Assert.ThrowsAsync<SeatValidationException>(
                    retrieveByIdSeatTask.AsTask);

            //then
            actualValidationException.Should().BeEquivalentTo(expectedValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSeatByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
