using Coworking_Booking.Api.Models.Seats;
using Coworking_Booking.Api.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coworking_Booking.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Seat> Seats { get; set; }

        public async ValueTask<Seat> InsertSeatAsync(Seat seat)=>
            await InsertSeatAsync(seat);
     
        public IQueryable<Seat> SelectAllSeats() =>
            SelectAll<Seat>();

        public async ValueTask<Seat> SelectSeatByIdAsync(Guid id) =>
            await SelectAsync<Seat>(id);

        public async ValueTask<Seat> UpdateSeatAsync(Seat seat)=>
            await UpdateSeatAsync(seat);

        public async ValueTask<Seat> DeleteSeatAsync(Seat seat)=>
            await DeleteSeatAsync(seat);
    }
}