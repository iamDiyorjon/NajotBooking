using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using NajotBooking.Api.Models.Seats;
using NajotBooking.Api.Models.Seats.Exceptions;
using Xunit;

namespace NajotBooking.Api.Tests.Unit.Services.Foundations.Seats
{
    public partial class SeatServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset someDateTime = GetRandomDateTime();
            Seat randomSeat = CreateRandomSeat(someDateTime);
            Seat someSeat = randomSeat;
            Guid seatId = someSeat.Id;
            SqlException sqlException = CreateSqlException();

            var failedSeatStorageException =
                new FailedSeatStorageException(sqlException);

            var expectedSeatDependencyException =
                new SeatDependencyException(failedSeatStorageException);
            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Throws(sqlException);

            // when
            ValueTask<Seat> modifySeatTask =
                this.seatService.ModifySeatAsync(someSeat);

            SeatDependencyException actualSeatDependencyException =
              await Assert.ThrowsAsync<SeatDependencyException>(
                  modifySeatTask.AsTask);

            // then
            actualSeatDependencyException.Should().BeEquivalentTo(
                expectedSeatDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedSeatDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSeatByIdAsync(seatId), Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSeatAsync(someSeat), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Seat randomSeat = CreateRandomSeat(randomDateTime);
            Seat SomeSeat = randomSeat;
            Guid seatId = SomeSeat.Id;
            SomeSeat.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            var databaseUpdateException = new DbUpdateException();

            var failedSeatException =
                new FailedSeatStorageException(databaseUpdateException);

            var expectedSeatDependencyException =
                new SeatDependencyException(failedSeatException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSeatByIdAsync(seatId)).ThrowsAsync(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Seat> modifySeatTask =
                this.seatService.ModifySeatAsync(SomeSeat);

            SeatDependencyException actualSeatDependencyException =
              await Assert.ThrowsAsync<SeatDependencyException>(
                  modifySeatTask.AsTask);

            // then
            actualSeatDependencyException.Should().BeEquivalentTo(
                expectedSeatDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSeatByIdAsync(seatId), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSeatDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Seat randomSeat = CreateRandomSeat(randomDateTime);
            Seat someSeat = randomSeat;
            someSeat.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            Guid seatId = someSeat.Id;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedSeatException =
                new LockedSeatException(databaseUpdateConcurrencyException);

            var expectedSeatDependencyValidationException =
                new SeatDependencyValidationException(lockedSeatException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSeatByIdAsync(seatId)).ThrowsAsync(databaseUpdateConcurrencyException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            // when
            ValueTask<Seat> modifySeatTask =
                this.seatService.ModifySeatAsync(someSeat);

            SeatDependencyValidationException actualSeatDependencyValidationException =
                await Assert.ThrowsAsync<SeatDependencyValidationException>(modifySeatTask.AsTask);

            // then
            actualSeatDependencyValidationException.Should().BeEquivalentTo(
                expectedSeatDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSeatByIdAsync(seatId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSeatDependencyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
