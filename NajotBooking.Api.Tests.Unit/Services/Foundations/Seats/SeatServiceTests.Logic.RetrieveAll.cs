using FluentAssertions;
using Moq;
using NajotBooking.Api.Models.Seats;
using System.Linq;
using System.Net.Sockets;
using Xunit;

namespace NajotBooking.Api.Tests.Unit.Services.Foundations.Seats
{
    public partial class SeatServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllSeats()
        {
            // given
            IQueryable<Seat> randomSeats = CreateRandomSeats();
            IQueryable<Seat> storageSeats = randomSeats;
            IQueryable<Seat> expectedSeats = storageSeats;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSeats()).Returns(storageSeats);

            // when
            IQueryable<Seat> actualSeat =
                this.seatService.RetrieveAllSeat();

            // then
            actualSeat.Should().BeEquivalentTo(expectedSeats);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSeats(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
