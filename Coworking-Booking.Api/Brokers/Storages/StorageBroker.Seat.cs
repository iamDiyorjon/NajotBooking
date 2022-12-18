using Coworking_Booking.Api.Models.Seats;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coworking_Booking.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Seat> Seats { get; set; }

        public async ValueTask<Seat> InsertSeatAsync(Seat seat) =>
            await InsertAsync(seat);

        public IQueryable<Seat> SelectAllSeats() =>
            SelectAll<Seat>();
    }
}