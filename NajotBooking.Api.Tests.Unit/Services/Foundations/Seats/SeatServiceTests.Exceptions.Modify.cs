using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
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
    }
}
