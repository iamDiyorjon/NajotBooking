using Coworking_Booking.Api.Models.Users;
using System.Linq;
using System.Threading.Tasks;
using System;
using Coworking_Booking.Api.Models.Seats;

namespace Coworking_Booking.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Seat> InsertSeatAsync(User user);
        IQueryable<Seat> SelectAllSeats();
        ValueTask<Seat> SelectSeatByIdAsync(Guid id);
        ValueTask<Seat> UpdateSeatAsync(User user);
        ValueTask<Seat> DeleteSeatAsync(User user);
    }
}
