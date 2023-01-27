using Coworking_Booking.Api.Brokers.DateTimes;
using Coworking_Booking.Api.Brokers.Loggings;
using Coworking_Booking.Api.Brokers.Storages;
using Coworking_Booking.Api.Models.Seats;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coworking_Booking.Api.Services.Foundations.Seats
{
    public partial class SeatService : ISeatService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public SeatService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Seat> AddSeatAsync(Seat seat) =>
            TryCatch(async () =>
            {
                ValidateSeat(seat);

                return await this.storageBroker.InsertSeatAsync(seat);
            });
        
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
