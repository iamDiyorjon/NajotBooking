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
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = CreateSqlException();
            var failedSeatServiceException = new FailedSeatServiceException(sqlException);

            var expectedSeatDependencyException =
                new SeatDependencyException(failedSeatServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSeats()).Throws(sqlException);

            // when
            Action retrieveAllSeatAction = () =>
                this.seatService.RetrieveAllSeat();

            SeatDependencyException actualSeatDependencyException =
                Assert.Throws<SeatDependencyException>(retrieveAllSeatAction);

            // then
            actualSeatDependencyException.Should().BeEquivalentTo(expectedSeatDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSeats(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedSeatDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
