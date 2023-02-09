using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
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

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfSeatDoesNotExistAndLogItAsync()
        {
            //given
            int randomNegativeMInutes = GetRandomNegativeNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Seat randomSeat = CreateRandomSeat(dateTime);
            Seat nonExistSeat = randomSeat;
            nonExistSeat.CreatedDate = dateTime.AddMinutes(randomNegativeMInutes);
            Seat nullSeat = null;

            var notFoundSeatException =
                new NotFoundSeatException(nonExistSeat.Id);

            var expectedSeatValidationException = new
                SeatValidationException(notFoundSeatException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSeatByIdAsync(nonExistSeat.Id)).ReturnsAsync(nullSeat);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(dateTime);

            //when
            ValueTask<Seat> modifySeatTask =
                this.seatService.ModifySeatAsync(nonExistSeat);

            SeatValidationException actualSeatValidationException =
                await Assert.ThrowsAsync<SeatValidationException>(modifySeatTask.AsTask);

            //then
            actualSeatValidationException.Should().BeEquivalentTo(expectedSeatValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSeatByIdAsync(nonExistSeat.Id), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSeatValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Seat randomSeat = CreateRandomModifySeat(randomDateTime);
            Seat invalidSeat = randomSeat.DeepClone();
            Seat storageSeat = invalidSeat.DeepClone();
            storageSeat.CreatedDate = storageSeat.CreatedDate.AddMinutes(randomMinutes);
            storageSeat.UpdatedDate = storageSeat.UpdatedDate.AddMinutes(randomMinutes);
            var invalidSeatException = new InvalidSeatException();
            Guid seatId = invalidSeat.Id;

            invalidSeatException.AddData(
                key: nameof(Seat.CreatedDate),
                values: $"Date is not the same as {nameof(Seat.CreatedDate)}");

            var expectedSeatValidationException =
                new SeatValidationException(invalidSeatException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSeatByIdAsync(seatId)).ReturnsAsync(storageSeat);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Seat> modifySeatTask =
                this.seatService.ModifySeatAsync(invalidSeat);

            SeatValidationException actualSeatValidationException =
                await Assert.ThrowsAsync<SeatValidationException>(modifySeatTask.AsTask);

            // then
            actualSeatValidationException.Should().BeEquivalentTo(expectedSeatValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSeatByIdAsync(invalidSeat.Id), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedSeatValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
