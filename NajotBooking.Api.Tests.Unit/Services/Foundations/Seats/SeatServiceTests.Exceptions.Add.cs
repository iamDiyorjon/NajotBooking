using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using NajotBooking.Api.Models.Seats;
using NajotBooking.Api.Models.Seats.Exceptions;
using System.Net.Sockets;
using System.Threading.Tasks;
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
    }
}
