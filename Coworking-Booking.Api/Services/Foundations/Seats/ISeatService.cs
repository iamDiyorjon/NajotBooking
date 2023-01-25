using NajotBooking.Api.Models.Seats;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NajotBooking.Api.Services.Foundations.Seats
{
    public interface ISeatService
    {
        ValueTask<Seat> AddSeatAsync(Seat seat);
        ValueTask<Seat> RetrieveSeatByIdAsync(Guid seatId);
        IQueryable<Seat> RetrieveAllSeat();
        ValueTask<Seat> ModifySeatAsync(Seat seat);
        ValueTask<Seat> RemoveSeat(Seat seat);
    }
}
