using Coworking_Booking.Api.Models.Seats;
using Coworking_Booking.Api.Models.Users;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Coworking_Booking.Api.Brokers.Storages
{
    public partial class StorageBroker : IStorageBroker
    {
        public DbSet<User> Users { get; set; }
  
        public async ValueTask<User> InsertUserAsync(User user) =>
            await InsertAsync(user);

        public IQueryable<User> SelectAllUsers() =>
            SelectAll<User>();
    }
}