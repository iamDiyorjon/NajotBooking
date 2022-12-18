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
        public DbSet<User> Users { get; set; }
  
        public async ValueTask<User> InsertUserAsync(User user) =>
            await InsertAsync(user);

        public IQueryable<User> SelectAllUsers() =>
            SelectAll<User>() ;

        public async ValueTask<User> SelectUserByIdAsync(Guid id)=>
            await SelectAsync<User>(id);

        public async ValueTask<User> UpdateUserAsync(User user) =>
           await UpdateAsync(user);

        public async ValueTask<User> DeleteUserAsync(User user) =>
            await DeleteAsync(user);

       
    }
}