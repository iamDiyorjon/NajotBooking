using Coworking_Booking.Api.Models.Users;
using System.Linq;
using System.Threading.Tasks;
using System;
using Coworking_Booking.Api.Models.Seats;

namespace Coworking_Booking.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Seat> InsertSeatAsync(Seat seat);
        IQueryable<Seat> SelectAllSeats();
        ValueTask<Seat> SelectSeatByIdAsync(Guid id);
        ValueTask<Seat> UpdateSeatAsync(Seat seat);
        ValueTask<Seat> DeleteSeatAsync(Seat seat);
    }
}
