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
        public async Task ShouldAddSeatAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Seat randomSeat = CreateRandomSeat(randomDateTime);
            Seat inputSeat = randomSeat;
            Seat persistedSeat = inputSeat;
            Seat expectedSeat = persistedSeat.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertSeatAsync(inputSeat))
                    .ReturnsAsync(persistedSeat);

            // when
            Seat actualSeat = await this.seatService.AddSeatAsync(inputSeat);

            // then
            actualSeat.Should().BeEquivalentTo(expectedSeat);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSeatAsync(inputSeat), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
