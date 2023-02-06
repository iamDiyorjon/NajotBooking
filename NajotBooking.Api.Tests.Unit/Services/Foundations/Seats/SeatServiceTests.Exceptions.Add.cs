using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Seat someSeat = CreateRandomSeat();
            SqlException sqlException = CreateSqlException();
            var failedSeatStorageException = new FailedSeatStorageException(sqlException);

            var expectedSeatDependencyException =
                new SeatDependencyException(failedSeatStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<Seat> addSeatTask = this.seatService.AddSeatAsync(someSeat);

            SeatDependencyException actualSeatDependencyException =
                await Assert.ThrowsAsync<SeatDependencyException>(addSeatTask.AsTask);

            // then
            actualSeatDependencyException.Should().BeEquivalentTo(expectedSeatDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
            expectedSeatDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSeatAsync(It.IsAny<Seat>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccursAndLogItAsync()
        {
            // given
            Seat someSeat = CreateRandomSeat();
            string someMessage = GetRandomString();
            var duplicateKeyException = new DuplicateKeyException(someMessage);

            var alreadyExistsSeatException =
                new AlreadyExistsSeatException(duplicateKeyException);

            var expectedSeatDependencyValidationException =
                new SeatDependencyValidationException(alreadyExistsSeatException);

            this.dateTimeBrokerMock.Setup(broker => broker.GetCurrentDateTime())
                .Throws(duplicateKeyException);

            // when
            ValueTask<Seat> addSeatTask = this.seatService.AddSeatAsync(someSeat);

            SeatDependencyValidationException actualSeatDependencyValidationException =
                await Assert.ThrowsAsync<SeatDependencyValidationException>(addSeatTask.AsTask);

            // then
            actualSeatDependencyValidationException.Should().BeEquivalentTo(
                expectedSeatDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(SameExceptionAs(
                expectedSeatDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker => broker.InsertSeatAsync(
                It.IsAny<Seat>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDbConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Seat someSeat = CreateRandomSeat();
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedSeatException = new LockedSeatException(dbUpdateConcurrencyException);
            var expectedSeatDependencyValidationException = new SeatDependencyValidationException(lockedSeatException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(dbUpdateConcurrencyException);

            // when
            ValueTask<Seat> addSeatTask = this.seatService.AddSeatAsync(someSeat);

            SeatDependencyValidationException actualSeatDependencyValidationException =
                 await Assert.ThrowsAsync<SeatDependencyValidationException>(addSeatTask.AsTask);

            // then
            actualSeatDependencyValidationException.Should().BeEquivalentTo(expectedSeatDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
                SameExceptionAs(expectedSeatDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker => broker.InsertSeatAsync(It.IsAny<Seat>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Seat someSeat = CreateRandomSeat();
            var serviceException = new Exception();

            var failedSeatServiceException =
                new FailedSeatServiceException(serviceException);

            var expectedSeatServiceException =
                new SeatServiceException(failedSeatServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Throws(serviceException);

            // when
            ValueTask<Seat> addSeatTask =
                this.seatService.AddSeatAsync(someSeat);

            SeatServiceException actualSeatServiceException =
                await Assert.ThrowsAsync<SeatServiceException>(addSeatTask.AsTask);

            // then
            actualSeatServiceException.Should().BeEquivalentTo(
                expectedSeatServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSeatServiceException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSeatAsync(It.IsAny<Seat>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
