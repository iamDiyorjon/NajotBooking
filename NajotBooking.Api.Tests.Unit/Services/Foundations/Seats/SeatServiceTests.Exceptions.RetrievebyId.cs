using Microsoft.Data.SqlClient;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdAsyncIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedSeatStorageException =
                new FailedSeatStorageException(sqlException);

            var expectedSeatDependencyException =
                new SeatDependencyException(failedSeatStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSeatByIdAsync(It.IsAny<Guid>())).ThrowsAsync(sqlException);

            //when
            ValueTask<Seat> retrieveSeatByIdTask =
                this.seatService.RetrieveSeatByIdAsync(someId);

            SeatDependencyException actualSeatDependencyException =
                await Assert.ThrowsAsync<SeatDependencyException>(retrieveSeatByIdTask.AsTask);

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
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdAsyncIfServiceErrorOccursAndLogItAsync()
        {
            //given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedSeatServiceException =
                new FailedSeatServiceException(serviceException);

            var expectedSeatServiceExcpetion =
                new SeatServiceException(failedSeatServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSeatByIdAsync(It.IsAny<Guid>())).ThrowsAsync(serviceException);

            //when
            ValueTask<Seat> retrieveSeatById =
                this.seatService.RetrieveSeatByIdAsync(someId);

            SeatServiceException actualTeamServiceException =
                await Assert.ThrowsAsync<SeatServiceException>(retrieveSeatById.AsTask);

            // then
            actualTeamServiceException.Should().BeEquivalentTo(expectedSeatServiceExcpetion);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSeatByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedSeatServiceExcpetion))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
