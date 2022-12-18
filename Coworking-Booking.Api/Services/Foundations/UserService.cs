using Coworking_Booking.Api.Brokers.Storages;
using Coworking_Booking.Api.Models.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coworking_Booking.Api.Services.Foundations
{
    public partial class UserService : IUserService
    {
        private readonly IStorageBroker storageBroker;

        public UserService(IStorageBroker storageBroker)
        {
            this.storageBroker = storageBroker;
        }

        public ValueTask<User> AddUserAsync(User user) =>
            this.storageBroker.InsertUserAsync(user);

        public ValueTask<User> RetrieveUserByIdAsync(Guid userId) =>
            this.storageBroker.SelectUserByIdAsync(userId);

        public IQueryable<User> RetrieveAllUsers() =>
            this.storageBroker.SelectAllUsers();

        public ValueTask<User> ModifyUserAsync(User user) =>
            this.storageBroker.UpdateUserAsync(user);

        public ValueTask<User> RemoveUser(User user) =>
            this.storageBroker.DeleteUserAsync(user);
            
    }

}
