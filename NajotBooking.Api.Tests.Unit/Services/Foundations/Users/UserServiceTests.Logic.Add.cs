using FluentAssertions;
using Force.DeepCloner;
using Moq;
using NajotBooking.Api.Models.Users;
using System.Threading.Tasks;
using Xunit;

namespace NajotBooking.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldAddUserAsync()
        {
            // given
            User randomUser = CreateRandomUser();
            User inputUser = randomUser;
            User storageUser = inputUser;
            User expectedUser = storageUser.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertUserAsync(inputUser))
                    .ReturnsAsync(storageUser);

            // when
            User actualUser =
                await this.userService.AddUserAsync(inputUser);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertUserAsync(inputUser),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}