// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using NajotBooking.Api.Brokers.Loggings;
using NajotBooking.Api.Brokers.Storages;
using NajotBooking.Api.Models.Users;

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
                 ValidateUser(user);
                 return await this.storageBroker.InsertUserAsync(user);
             });

        public ValueTask<User> RetrieveUserByIdAsync(Guid userId) =>
        TryCatch(async () =>
        {
            ValidateUserId(userId);
            User maybeUser =
                    await this.storageBroker.SelectUserByIdAsync(userId);

            ValidateStorageUser(maybeUser, userId);

            return maybeUser;
        });

        public IQueryable<User> RetrieveAllUsers() =>
            TryCatch(() => this.storageBroker.SelectAllUsers());

        public ValueTask<User> ModifyUserAsync(User user) =>
            TryCatch(async () =>
            {
                ValidateUser(user);

                User maybeUser =
                    await this.storageBroker.SelectUserByIdAsync(user.Id);

                ValidateStorageUser(maybeUser, user.Id);

                return await this.storageBroker.UpdateUserAsync(user);
            });

        public ValueTask<User> RemoveUserByIdAsync(Guid userId) =>
        TryCatch(async () =>
        {
            ValidateUserId(userId);

            User maybeUser =
                await this.storageBroker.SelectUserByIdAsync(userId);

            ValidateStorageUser(maybeUser, userId);

            return await this.storageBroker.DeleteUserAsync(maybeUser);
        });
    }
}