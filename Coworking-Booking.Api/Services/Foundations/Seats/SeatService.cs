using Coworking_Booking.Api.Brokers.Storages;
using Coworking_Booking.Api.Models.Seats;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coworking_Booking.Api.Services.Foundations.Seats
{
    public class SeatService : ISeatService
    {
        private readonly IStorageBroker storageBroker;

        public SeatService(IStorageBroker storageBroker)
        {
            this.storageBroker = storageBroker;
        }

        public ValueTask<Seat> AddSeatAsync(Seat seat) =>
            this.storageBroker.InsertSeatAsync(seat);
        

        public ValueTask<Seat> ModifySeatAsync(Seat seat) =>
            this.storageBroker.UpdateSeatAsync(seat);
        

        public ValueTask<Seat> RemoveSeat(Seat seat) =>
            this.storageBroker.DeleteSeatAsync(seat);

        public IQueryable<Seat> RetrieveAllSeat() =>
            this.storageBroker.SelectAllSeats();

        public ValueTask<Seat> RetrieveSeatByIdAsync(Guid seatId) =>
            this.storageBroker.SelectSeatByIdAsync(seatId);
    }
}
