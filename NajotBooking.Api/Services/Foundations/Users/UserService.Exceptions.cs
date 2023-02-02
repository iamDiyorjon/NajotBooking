using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NajotBooking.Api.Models.Users;
using NajotBooking.Api.Models.Users.Exceptions;
using Xeptions;

namespace NajotBooking.Api.Services.Foundations.Users
{
    public partial class UserService
    {
        private delegate ValueTask<User> ReturningUserFunction();
        private delegate IQueryable<User> ReturningUsersFunction();

        private async ValueTask<User> TryCatch(ReturningUserFunction returningUserFunction)
        {
            try
            {
                return await returningUserFunction();
            }
            catch (NullUserException nullUserException)
            {
                throw CreateAndLogValidationException(nullUserException);
            }
            catch (InvalidUserException invalidUserException)
            {
                throw CreateAndLogValidationException(invalidUserException);
            }
            catch (NotFoundUserException notFoundUserException)
            {
                throw CreateAndLogValidationException(notFoundUserException);
            }
            catch (SqlException sqlException)
            {
                var failedUserStorageException =
                    new FailedUserStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedUserStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsUserException =
                    new AlreadyExistsUserException(duplicateKeyException);

                throw CreateAndDependencyValidationException(alreadyExistsUserException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedUserException = new LockedUserException(dbUpdateConcurrencyException);

                throw CreateAndDependencyValidationException(lockedUserException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedUserStorageException =
                    new FailedUserStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedUserStorageException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidUserReferenceException =
                    new InvalidUserReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidUserReferenceException);
            }
            //catch (Exception serviceException)
            //{
            //    var failedUserServiceException =
            //        new FailedUserServiceException(serviceException);

            //    throw CreateAndLogServiceException(failedUserServiceException);
            //}
        }

        private IQueryable<User> TryCatch(ReturningUsersFunction returningUsersFunction)
        {
            try
            {
                return returningUsersFunction();
            }
            catch (SqlException sqlException)
            {
                var failedUserStorageException =
                    new FailedUserStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedUserStorageException);
            }
            catch (Exception serviceException)
            {
                var failedUserServiceException =
                    new FailedUserServiceException(serviceException);

                throw CreateAndLogServiceException(failedUserServiceException);
            }
        }

        private UserDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var userDependencyValidationException =
                new UserDependencyValidationException(exception);

            this.loggingBroker.LogError(userDependencyValidationException);

            return userDependencyValidationException;
        }

        private UserDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var userDependencyException =
                new UserDependencyException(exception);

            this.loggingBroker.LogError(userDependencyException);

            return userDependencyException;
        }

        private UserValidationException CreateAndLogValidationException(Xeption exception)
        {
            var userValidationException =
                new UserValidationException(exception);

            this.loggingBroker.LogError(userValidationException);

            return userValidationException;
        }

        private UserDependencyException CreateAndLogCriticalDependencyException(Xeption exeption)
        {
            var userDependencyException = new UserDependencyException(exeption);
            this.loggingBroker.LogCritical(userDependencyException);

            return userDependencyException;
        }

        private UserServiceException CreateAndLogServiceException(Xeption exception)
        {
            var userServiceException =
                new UserServiceException(exception);

            this.loggingBroker.LogError(userServiceException);

            return userServiceException;
        }

        private UserDependencyValidationException CreateAndDependencyValidationException(Xeption exception)
        {
            var userDependencyValidationException =
                new UserDependencyValidationException(exception);

            this.loggingBroker.LogError(userDependencyValidationException);

            return userDependencyValidationException;
        }
    }
}
