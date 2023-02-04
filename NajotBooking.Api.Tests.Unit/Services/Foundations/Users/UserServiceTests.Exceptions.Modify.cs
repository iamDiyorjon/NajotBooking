using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using NajotBooking.Api.Models.Users;
using NajotBooking.Api.Models.Users.Exceptions;
using Xunit;

namespace NajotBooking.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset someDateTime = GetRandomDateTimeOffset();
            User randomUser = CreateRandomUser(someDateTime);
            User someUser = randomUser;
            Guid userId = someUser.Id;
            SqlException sqlException = GetSqlException();

            var failedUserStorageException =
                new FailedUserStorageException(sqlException);

            var expectedUserDependencyException =
                new UserDependencyException(failedUserStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(userId))
                    .Throws(sqlException);

            // when
            ValueTask<User> modifyUserTask =
                this.userService.ModifyUserAsync(someUser);

            UserDependencyException actualUserDependencyException =
                await Assert.ThrowsAsync<UserDependencyException>(
                     modifyUserTask.AsTask);

            // then
            actualUserDependencyException.Should().BeEquivalentTo(
                expectedUserDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedUserDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(userId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(someUser), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}