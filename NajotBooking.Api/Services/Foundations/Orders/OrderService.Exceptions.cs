// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using NajotBooking.Api.Models.Orders;
using NajotBooking.Api.Models.Orders.Exceptions;
using Xeptions;

namespace NajotBooking.Api.Services.Foundations.Orders
{
    public partial class OrderService
    {
        private delegate ValueTask<Order> ReturningOrderFunction();

        private async ValueTask<Order> TryCatch(ReturningOrderFunction returningOrderFunction)
        {
            try
            {
                return await returningOrderFunction();
            }
            catch (NullOrderException nullOrderException)
            {
                throw CreateAndLogValidationException(nullOrderException);
            }
            catch (InvalidOrderException invalidOrderException)
            {
                throw CreateAndLogValidationException(invalidOrderException);
            }
            catch (SqlException sqlException)
            {
                var failedOrderStorageException = new FailedOrderStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedOrderStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsOrderException =
                    new AlreadyExistsOrderException(duplicateKeyException);

                throw CreateAndDependencyValidationException(alreadyExistsOrderException);
            }
        }

        private OrderValidationException CreateAndLogValidationException(Xeption exception)
        {
            var orderValidationException = new OrderValidationException(exception);
            this.loggingBroker.LogError(orderValidationException);

            return orderValidationException;
        }

        private OrderDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var orderDependencyException = new OrderDependencyException(exception);
            this.loggingBroker.LogCritical(orderDependencyException);

            return orderDependencyException;
        }

        private OrderDependencyValidationException CreateAndDependencyValidationException(Xeption exception)
        {
            var orderDependencyValidationException =
                new OrderDependencyValidationException(exception);

            this.loggingBroker.LogError(orderDependencyValidationException);

            return orderDependencyValidationException;
        }
    }
}
