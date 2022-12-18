using Coworking_Booking.Api.Models.Seats;
using Coworking_Booking.Api.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;

namespace Coworking_Booking.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Seat> Seats { get; set; }
    }
}