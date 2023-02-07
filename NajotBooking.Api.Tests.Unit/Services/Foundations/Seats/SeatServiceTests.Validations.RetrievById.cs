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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            //given 
            var invalidSeatId = Guid.Empty;

            var invalidSeatException =
                new InvalidSeatException();

            invalidSeatException.AddData(
                key: nameof(Seat.Id),
                values: "Id is required");

            var expectedSeatValidationException = new
                SeatValidationException(invalidSeatException);

            //when 
            ValueTask<Seat> retrieveSeatByIdTask =
                this.seatService.RetrieveSeatByIdAsync(invalidSeatId);

            SeatValidationException actualSeatValidationException =
                await Assert.ThrowsAsync<SeatValidationException>(retrieveSeatByIdTask.AsTask);

            //then
            actualSeatValidationException.Should().BeEquivalentTo(expectedSeatValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSeatValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSeatByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
