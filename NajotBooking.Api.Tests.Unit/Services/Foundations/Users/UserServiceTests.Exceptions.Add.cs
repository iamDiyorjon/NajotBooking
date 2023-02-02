using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using NajotBooking.Api.Models.Users;
using NajotBooking.Api.Models.Users.Exceptions;
using Xunit;

namespace NajotBooking.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            User someUser = CreateRandomUser();
            SqlException sqlException = GetSqlException();

            var failedUserStorageException =
                new FailedUserStorageException(sqlException);

            var expectedUserDependencyException =
                new UserDependencyException(failedUserStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertUserAsync(It.IsAny<User>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<User> addUserTask =
                this.userService.AddUserAsync(someUser);

            // then
            await Assert.ThrowsAsync<UserDependencyException>(() =>
                addUserTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertUserAsync(It.IsAny<User>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedUserDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfUserAlreadyExistsAndLogItAsync()
        {
            // given
            User someUser = CreateRandomUser();
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsUserException =
                new AlreadyExistsUserException(duplicateKeyException);

            var expectedUserDependencyValidationException =
                new UserDependencyValidationException(alreadyExistsUserException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertUserAsync(It.IsAny<User>()))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<User> addUserTask =
                this.userService.AddUserAsync(someUser);

            UserDependencyValidationException actualUserDependencyValidationException =
                await Assert.ThrowsAsync<UserDependencyValidationException>(
                    addUserTask.AsTask);

            // then
            actualUserDependencyValidationException.Should().BeEquivalentTo(
                expectedUserDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertUserAsync(someUser),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            //given
            User someUser = CreateRandomUser();

            var databaseUpdateException =
                new DbUpdateException();

            var failedUserStorageException =
                new FailedUserStorageException(databaseUpdateException);

            var expectedUserDependencyException =
                new UserDependencyException(failedUserStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertUserAsync(someUser))
                    .Throws(databaseUpdateException);

            //when
            ValueTask<User> addUserTask =
                this.userService.AddUserAsync(someUser);

            UserDependencyException actualUserDependencyException =
                await Assert.ThrowsAsync<UserDependencyException>(
                    addUserTask.AsTask);

            //then
            actualUserDependencyException.Should().BeEquivalentTo(
                expectedUserDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertUserAsync(It.IsAny<User>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowDependencyValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            //given
            User someUser = CreateRandomUser();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidUserReferenceException =
                new InvalidUserReferenceException(foreignKeyConstraintConflictException);

            var expectedUserDependencyValidationException =
                new UserDependencyValidationException(invalidUserReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertUserAsync(someUser))
                    .Throws(foreignKeyConstraintConflictException);

            //when
            ValueTask<User> addUserTask =
                this.userService.AddUserAsync(someUser);

            UserDependencyValidationException actualUserDependencyValidationException =
                await Assert.ThrowsAsync<UserDependencyValidationException>(
                    addUserTask.AsTask);

            //then
            actualUserDependencyValidationException.Should().BeEquivalentTo(
                expectedUserDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertUserAsync(It.IsAny<User>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}