using System;
using System.Net.Sockets;
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

        [Theory]
        [InlineData(null)]
        [InlineData(null)]
        [InlineData(default)]
        public async Task ShouldThrowValidationExceptionOnModifyIfSeatIsInvalidAndLogItAsync(
            int invalidNumber)
        {
            //given
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
                key: nameof(Seat.Floor),
                values: "Number is required");

            invalidSeatException.AddData(
               key: nameof(Seat.Price),
               values: "Price is required");

            invalidSeatException.AddData(
                key: nameof(Seat.CreatedDate),
                values: "Value is required");

            invalidSeatException.AddData(
                key: nameof(Seat.UpdatedDate),
                    values: new[]
                    {
                        "Value is required",
                        "Date is not recent",
                        $"Date is the same as {nameof(Seat.CreatedDate)}"
                    }
                );

            var expectedSeatValidationException =
                new SeatValidationException(invalidSeatException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(GetRandomDateTime);

            //when
            ValueTask<Seat> modifySeatTask = this.seatService.ModifySeatAsync(invalidSeat);

            SeatValidationException actualSeatValidationException =
                await Assert.ThrowsAsync<SeatValidationException>(modifySeatTask.AsTask);

            //then
            actualSeatValidationException.Should().BeEquivalentTo(expectedSeatValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSeatValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSeatAsync(It.IsAny<Seat>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotSameAsCreatedDateAndLogItAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Seat randomSeat = CreateRandomSeat(randomDateTime);
            Seat invalidSeat = randomSeat;
            var invalidSeatException = new InvalidSeatException();

            invalidSeatException.AddData(
                key: nameof(Seat.UpdatedDate),
                values: $"Date is the same as {nameof(Seat.CreatedDate)}");

            var expectedSeatValidationException =
                new SeatValidationException(invalidSeatException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            //when
            ValueTask<Seat> modifySeatTask = this.seatService.ModifySeatAsync(invalidSeat);

            SeatValidationException actualSeatValidationException =
                await Assert.ThrowsAsync<SeatValidationException>(modifySeatTask.AsTask);

            //then
            actualSeatValidationException.Should().BeEquivalentTo(
                expectedSeatValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSeatValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSeatByIdAsync(invalidSeat.Id), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidSeconds))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(
           int invalidSeconds)
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Seat randomSeat = CreateRandomSeat(randomDateTime);
            Seat inputSeat = randomSeat;
            inputSeat.UpdatedDate = randomDateTime.AddSeconds(invalidSeconds);
            var invalidSeatException = new InvalidSeatException();

            invalidSeatException.AddData(
                key: nameof(Seat.UpdatedDate),
                values: "Date is not recent");

            var expectedSeatValidationException =
                new SeatValidationException(invalidSeatException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            //when
            ValueTask<Seat> modifySeatTask = this.seatService.ModifySeatAsync(inputSeat);

            SeatValidationException actualSeatValidationException =
                await Assert.ThrowsAsync<SeatValidationException>(modifySeatTask.AsTask);

            //then
            actualSeatValidationException.Should().BeEquivalentTo(
                expectedSeatValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSeatValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSeatByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
