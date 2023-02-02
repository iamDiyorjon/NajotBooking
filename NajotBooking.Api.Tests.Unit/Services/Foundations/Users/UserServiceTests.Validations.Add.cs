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
        public async Task ShouldThrowValidationExceptionOnAddIfUserIsNullAndLogItAsync()
        {
            // given
            User nullUser = null;

            var nullUserException =
                new NullUserException();

            var expecteUserValidationException =
                new UserValidationException(nullUserException);

            // when
            ValueTask<User> addUserTask =
                this.userService.AddUserAsync(nullUser);

            UserValidationException actualUserValidationException =
                await Assert.ThrowsAsync<UserValidationException>(
                    addUserTask.AsTask);

            // then
            actualUserValidationException.Should().BeEquivalentTo(
                expecteUserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expecteUserValidationException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
