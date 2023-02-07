using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using NajotBooking.Api.Models.Seats;
using Xunit;

namespace NajotBooking.Api.Tests.Unit.Services.Foundations.Seats
{
    public partial class SeatServiceTests
    {
        [Fact]
        public async Task ShouldRetriveSeatByIdAsync()
        {
            //given
            Guid randomSeatId = Guid.NewGuid();
            Guid inputSeatId = randomSeatId;
            Seat randomSeat = CreateRandomSeat();
            Seat storedSeat = randomSeat;
            Seat expectedSeat = storedSeat.DeepClone();

            this.storageBrokerMock.Setup(broker =>
              broker.SelectSeatByIdAsync(randomSeatId)).ReturnsAsync(storedSeat);

            //when
            Seat actualSeat =
                await this.seatService.RetrieveSeatByIdAsync(inputSeatId);

            //then
            actualSeat.Should().BeEquivalentTo(expectedSeat);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSeatByIdAsync(inputSeatId), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
