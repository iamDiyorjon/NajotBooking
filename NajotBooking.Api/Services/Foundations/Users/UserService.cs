using NajotBooking.Api.Brokers.Loggings;
using NajotBooking.Api.Brokers.Storages;
using NajotBooking.Api.Models.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NajotBooking.Api.Services.Foundations.Users
{
    public partial class UserService : IUserService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public UserService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<User> AddUserAsync(User user) =>
             TryCatch(async () =>
             {
                 return await this.storageBroker.InsertUserAsync(user);
             });

        public ValueTask<User> RetrieveUserByIdAsync(Guid userId) =>
            storageBroker.SelectUserByIdAsync(userId);

        public IQueryable<User> RetrieveAllUsers() =>
            storageBroker.SelectAllUsers();

        public ValueTask<User> ModifyUserAsync(User user) =>
            storageBroker.UpdateUserAsync(user);

        public ValueTask<User> RemoveUser(User user) =>
            storageBroker.DeleteUserAsync(user);
    }
}