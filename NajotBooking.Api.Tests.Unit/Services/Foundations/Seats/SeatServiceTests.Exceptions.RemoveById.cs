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
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            //given
            Guid someSeatId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedSeatStorageException =
                new FailedSeatStorageException(sqlException);

            var expectedSeatDependencyException =
                new SeatDependencyException(failedSeatStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSeatByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            //when
            ValueTask<Seat> deleteSeatAsync =
                this.seatService.RemoveSeatByIdAsync(someSeatId);

            SeatDependencyException actualSeatDependencyException =
                await Assert.ThrowsAsync<SeatDependencyException>(deleteSeatAsync.AsTask);

            //then
            actualSeatDependencyException.Should().BeEquivalentTo(expectedSeatDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSeatByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedSeatDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someSeatId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedSeatException =
                new LockedSeatException(databaseUpdateConcurrencyException);

            var expectedSeatDependencyValidationException =
                new SeatDependencyValidationException(lockedSeatException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSeatByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Seat> removeSeatByIdTask =
                this.seatService.RemoveSeatByIdAsync(someSeatId);

            SeatDependencyValidationException actualSeatDependencyValidationException =
                await Assert.ThrowsAsync<SeatDependencyValidationException>(
                    removeSeatByIdTask.AsTask);

            // then
            actualSeatDependencyValidationException.Should().BeEquivalentTo(
                expectedSeatDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSeatByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSeatDependencyValidationException))), Times.Once);
            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSeatAsync(It.IsAny<Seat>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfExceptionOccursAndLogItAsync()
        {
            // given
            Guid someSeatId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedSeatServiceException =
                new FailedSeatServiceException(serviceException);

            var expectedSeatServiceException =
                new SeatServiceException(failedSeatServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSeatByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Seat> removeSeatByIdTask =
                this.seatService.RemoveSeatByIdAsync(someSeatId);

            SeatServiceException actualSeatServiceException =
                await Assert.ThrowsAsync<SeatServiceException>(
                    removeSeatByIdTask.AsTask);

            // then
            actualSeatServiceException.Should().BeEquivalentTo(
                expectedSeatServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSeatByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSeatServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
