using Coworking_Booking.Api.Models.Seats;
using Microsoft.EntityFrameworkCore;

namespace Coworking_Booking.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Seat> Seats { get; set; }
    }
}