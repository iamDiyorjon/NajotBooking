// ---------------------------------------------------------------
// Copyright (c) Coalition Of The THE STANDART SHARPISTS
// Free To Use To Book Places In Coworking Zones
// ---------------------------------------------------------------

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
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guid someUserId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedUserStorageException =
                new FailedUserStorageException(sqlException);

            var expectedUserDependencyException =
                new UserDependencyException(failedUserStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(someUserId))
                    .ThrowsAsync(sqlException);

            //when
            ValueTask<User> retrieveUserByIdTask =
                this.userService.RetrieveUserByIdAsync(someUserId);

            UserDependencyException actualUserDependencyException =
                await Assert.ThrowsAsync<UserDependencyException>(
                    retrieveUserByIdTask.AsTask);

            //then
            actualUserDependencyException.Should().BeEquivalentTo(
                   expectedUserDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync((It.IsAny<Guid>())),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedUserDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            //given
            Guid someUserId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedUserServiceException =
                new FailedUserServiceException(serviceException);

            var expectedUserServiceException =
                new UserServiceException(failedUserServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(someUserId))
                    .ThrowsAsync(serviceException);

            //when
            ValueTask<User> retrieveUserByIdTask =
                this.userService.RetrieveUserByIdAsync(someUserId);

            UserServiceException actualGroupPostServiceException =
                 await Assert.ThrowsAsync<UserServiceException>(
                     retrieveUserByIdTask.AsTask);

            //then
            actualGroupPostServiceException.Should().BeEquivalentTo(
                expectedUserServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync((It.IsAny<Guid>())),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}