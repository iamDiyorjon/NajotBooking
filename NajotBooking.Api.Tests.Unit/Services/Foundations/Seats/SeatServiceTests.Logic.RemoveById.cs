using Moq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System;
using Xunit;
using NajotBooking.Api.Models.Seats;
using Force.DeepCloner;
using FluentAssertions;

namespace NajotBooking.Api.Tests.Unit.Services.Foundations.Seats
{
    public partial class SeatServiceTests
    {
        [Fact]
        public async Task ShouldRemoveSeatByIdAsync()
        {
            //given
            Guid randomId = Guid.NewGuid();
            Guid inputSeatId = randomId;
            Seat randomSeat = CreateRandomSeat();
            Seat storageSeat = randomSeat;
            Seat expectedInputSeat = storageSeat;
            Seat deletedSeat = expectedInputSeat;
            Seat expectedSeat = deletedSeat.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSeatByIdAsync(inputSeatId))
                    .ReturnsAsync(storageSeat);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteSeatAsync(expectedInputSeat))
                    .ReturnsAsync(deletedSeat);

            //when
            Seat actualSeat = await this.seatService
                .RemoveSeatByIdAsync(inputSeatId);

            //then
            actualSeat.Should().BeEquivalentTo(expectedSeat);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSeatByIdAsync(inputSeatId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSeatAsync(expectedInputSeat), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
