using Coworking_Booking.Api.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Coworking_Booking.Api.Brokers.Storages
{
    public partial class StorageBroker : IStorageBroker
    {
        public DbSet<User> Users { get; set; }
    }
}