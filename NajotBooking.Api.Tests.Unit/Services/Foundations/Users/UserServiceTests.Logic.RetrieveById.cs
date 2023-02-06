// ---------------------------------------------------------------
// Copyright (c) Coalition Of The THE STANDART SHARPISTS
// Free To Use To Book Places In Coworking Zones
// ---------------------------------------------------------------

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
        public async Task ShouldRetrieveUserByIdAsync()
        {
            // given
            Guid randomUserId = Guid.NewGuid();
            Guid inputUserId = randomUserId;
            User randomUser = CreateRandomUser();
            User storageUser = randomUser;
            User expectedUser = storageUser.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(inputUserId))
                    .ReturnsAsync(storageUser);

            // when
            User actualUser =
                await this.userService.RetrieveUserByIdAsync(
                    inputUserId);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(inputUserId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}