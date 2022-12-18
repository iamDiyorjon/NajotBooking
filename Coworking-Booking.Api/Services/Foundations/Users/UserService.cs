using Coworking_Booking.Api.Brokers.DateTimes;
using Coworking_Booking.Api.Brokers.Loggings;
using Coworking_Booking.Api.Brokers.Storages;
using Coworking_Booking.Api.Models.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coworking_Booking.Api.Services.Foundations.Users
{
    public partial class UserService : IUserService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        public UserService(IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<User> AddUserAsync(User user) =>
            storageBroker.InsertUserAsync(user);

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
