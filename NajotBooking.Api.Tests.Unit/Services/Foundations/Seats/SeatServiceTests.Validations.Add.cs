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
        public async Task ShouldThrowValidationExceptionOnAddIfInputIsNullAndLogItAsync()
        {
            // given
            Seat noSeat = null;
            var nullSeatException = new NullSeatException();

            var expectedSeatValidationException =
                new SeatValidationException(nullSeatException);

            // when
            ValueTask<Seat> addSeatTask =
                this.seatService.AddSeatAsync(noSeat);

            SeatValidationException actualSeatValidationException =
                await Assert.ThrowsAsync<SeatValidationException>(addSeatTask.AsTask);

            // then
            actualSeatValidationException.Should().BeEquivalentTo(
                expectedSeatValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSeatValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSeatAsync(It.IsAny<Seat>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(default)]
        public async Task ShouldThrowValidationExceptionOnAddIfSeatIsInvalidAndLogItAsync(
            int invalidNumber)
        {
            // given
            var invalidSeat = new Seat
            {
                Number = invalidNumber
            };

            var invalidSeatException = new InvalidSeatException();

            invalidSeatException.AddData(
                key: nameof(Seat.Id),
                values: "Id is required");

            invalidSeatException.AddData(
                key: nameof(Seat.Number),
                values: "Number is required");

            invalidSeatException.AddData(
                key: nameof(Seat.Branch),
                values: "Value is not recognized");

            invalidSeatException.AddData(
               key: nameof(Seat.Floor),
               values: "Number is required");

            invalidSeatException.AddData(
               key: nameof(Seat.Price),
               values: "Price is required");

            invalidSeatException.AddData(
               key: nameof(Seat.CraetedDate),
               values: "Value is required");

            invalidSeatException.AddData(
               key: nameof(Seat.UpdatedDate),
               values: "Value is required");

            var expectedSeatValidationException =
                new SeatValidationException(invalidSeatException);

            // when
            ValueTask<Seat> addSeatTask = this.seatService.AddSeatAsync(invalidSeat);

            SeatValidationException actualSeatValidationException =
                await Assert.ThrowsAsync<SeatValidationException>(addSeatTask.AsTask);

            // then
            actualSeatValidationException.Should().BeEquivalentTo(expectedSeatValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSeatValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSeatAsync(It.IsAny<Seat>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
