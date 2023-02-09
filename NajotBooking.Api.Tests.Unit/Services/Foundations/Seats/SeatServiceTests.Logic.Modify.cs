using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
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
        public async Task ShouldModifySeatAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTime();
            Seat randomSeat = CreateRandomModifySeat(randomDate);
            Seat inputSeat = randomSeat;
            Seat storageSeat = inputSeat.DeepClone();
            storageSeat.UpdatedDate = randomSeat.CreatedDate;
            Seat updatedSeat = inputSeat;
            Seat expectedSeat = updatedSeat.DeepClone();
            Guid seatId = inputSeat.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime()).Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSeatByIdAsync(seatId)).ReturnsAsync(storageSeat);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateSeatAsync(inputSeat)).ReturnsAsync(updatedSeat);

            // when
            Seat actualSeat =
                await this.seatService.ModifySeatAsync(inputSeat);

            // then
            actualSeat.Should().BeEquivalentTo(expectedSeat);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSeatByIdAsync(seatId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSeatAsync(inputSeat), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
