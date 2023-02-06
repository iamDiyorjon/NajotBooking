using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using NajotBooking.Api.Models.Seats.Exceptions;
using Xunit;

namespace NajotBooking.Api.Tests.Unit.Services.Foundations.Seats
{
    public partial class SeatServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccursAndLogIt()
        {
            //given
            SqlException sqlException = CreateSqlException();

            var failedSeatStorageException =
                new FailedSeatStorageException(sqlException);

            var expectedSeatDependencyException =
                new SeatDependencyException(failedSeatStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSeats()).Throws(sqlException);

            //when
            Action retrieveAllSeatsAction = () =>
                this.seatService.RetrieveAllSeat();

            SeatDependencyException actualSeatDependencyException =
                Assert.Throws<SeatDependencyException>(retrieveAllSeatsAction);

            //then
            actualSeatDependencyException.Should().BeEquivalentTo(expectedSeatDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSeats(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedSeatDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
