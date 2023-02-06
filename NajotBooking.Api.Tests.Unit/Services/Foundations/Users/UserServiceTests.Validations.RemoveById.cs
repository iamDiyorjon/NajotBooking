using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NajotBooking.Api.Models.Users;
using NajotBooking.Api.Models.Users.Exceptions;
using Xunit;

namespace NajotBooking.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidUserId = Guid.Empty;

            var invalidUserException = new InvalidUserException();

            invalidUserException.AddData(
                key: nameof(User.Id),
                values: "Id is required");

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            // when
            ValueTask<User> removeUserByIdTask =
                this.userService.RemoveUserByIdAsync(invalidUserId);

            UserValidationException actualUserValidationException =
                await Assert.ThrowsAsync<UserValidationException>(
                    removeUserByIdTask.AsTask);

            // then
            actualUserValidationException.Should()
                .BeEquivalentTo(expectedUserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteUserAsync(It.IsAny<User>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRemoveIfGroupPostIsNotFoundAndLogItAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            User randomUser = CreateRandomUser(randomDateTime);
            Guid inputUserId = randomUser.Id;
            User nullStorageUser = null;

            var notFoundUserException =
                new NotFoundUserException(inputUserId);

            var expectedUserValidationException =
                new UserValidationException(notFoundUserException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(inputUserId))
                    .ReturnsAsync(nullStorageUser);

            //when
            ValueTask<User> removeUserTask =
                this.userService.RemoveUserByIdAsync(inputUserId);

            UserValidationException actualUserValidationException =
                await Assert.ThrowsAsync<UserValidationException>(
                    removeUserTask.AsTask);

            //then
            actualUserValidationException.Should().BeEquivalentTo(
                expectedUserValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteUserAsync(It.IsAny<User>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}