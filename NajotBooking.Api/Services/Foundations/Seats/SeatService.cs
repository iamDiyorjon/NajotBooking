using System;
using System.Linq;
using System.Threading.Tasks;
using NajotBooking.Api.Brokers.DateTimes;
using NajotBooking.Api.Brokers.Loggings;
using NajotBooking.Api.Brokers.Storages;
using NajotBooking.Api.Models.Seats;

namespace NajotBooking.Api.Services.Foundations.Seats
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

        public IQueryable<Seat> RetrieveAllSeat() =>
            TryCatch(() => this.storageBroker.SelectAllSeats());

        public ValueTask<Seat> RetrieveSeatByIdAsync(Guid seatId) =>
            TryCatch(async () =>
            {
                ValidateSeatId(seatId);

                Seat maybeSeat =
                    await this.storageBroker.SelectSeatByIdAsync(seatId);

                ValidateStorageSeat(maybeSeat, seatId);

                return maybeSeat;
            });

        public ValueTask<Seat> ModifySeatAsync(Seat seat) =>
            TryCatch(async () =>
            {
                ValidateSeatOnModify(seat);

                var maybeSeat =
                    await this.storageBroker.SelectSeatByIdAsync(seat.Id);

                ValidateAginstStorageSeatOnModify(inputSeat: seat, storageSeat: maybeSeat);

                return await this.storageBroker.UpdateSeatAsync(seat);
            });

        public async ValueTask<Seat> RemoveSeatByIdAsync(Guid seatId)
        {
            Seat maybeSeat = await this.storageBroker.SelectSeatByIdAsync(seatId);

            return await this.storageBroker.DeleteSeatAsync(maybeSeat);
        }
    }
}
