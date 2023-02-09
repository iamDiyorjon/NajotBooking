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
    }
}
