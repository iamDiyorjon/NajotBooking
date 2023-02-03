using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using NajotBooking.Api.Models.Users;
using Xunit;

namespace NajotBooking.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldModifyUserAsync()
        {
            //given
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            User randomUser = CreateRandomModifyUser(randomDate);
            User inputUser = randomUser;
            User storageUser = inputUser.DeepClone();
            User updatedUser = inputUser;
            User exceptedUser = updatedUser.DeepClone();
            Guid Id = inputUser.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(Id))
                    .ReturnsAsync(storageUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(Id))
                    .ReturnsAsync(updatedUser);

            //when
            User actualUser =
                await this.userService.ModifyUserAsync(inputUser);

            //then
            actualUser.Should().BeEquivalentTo(exceptedUser);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(inputUser), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(Id), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}