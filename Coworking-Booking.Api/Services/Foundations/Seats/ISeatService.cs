using Coworking_Booking.Api.Models.Users;
using System.Linq;
using System.Threading.Tasks;
using System;
using Coworking_Booking.Api.Models.Seats;

namespace Coworking_Booking.Api.Services.Foundations.Seats
{
    public interface ISeatService
    {
        ValueTask<Seat> AddSeatAsync(Seat seat);
        ValueTask<Seat> RetrieveSeatByIdAsync(Guid seatId);
        IQueryable<Seat> RetrieveAllSeatAsync();
        ValueTask<Seat> ModifySeatAsync(Seat seat);
        ValueTask<Seat> RemoveSeatByIdAsync(Seat seat);
    }
}
