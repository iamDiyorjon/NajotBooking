using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using NajotBooking.Api.Models.Seats;
using NajotBooking.Api.Models.Seats.Exceptions;
using Xeptions;

namespace NajotBooking.Api.Services.Foundations.Seats
{
    public partial class SeatService
    {
        private delegate ValueTask<Seat> ReturningSeatFunction();
        private delegate IQueryable<Seat> ReturningSeatsFunction();

        private async ValueTask<Seat> TryCatch(ReturningSeatFunction returningSeatFunction)
        {
            try
            {
                return await returningSeatFunction();
            }
            catch (NullSeatException nullSeatException)
            {
                throw CreateAndLogValidationException(nullSeatException);
            }
            catch (InvalidSeatException invalidSeatException)
            {
                throw CreateAndLogValidationException(invalidSeatException);
            }
            catch (NotFoundUserException notFoundUserException)
            {
                throw CreateAndLogValidationException(notFoundUserException);
            }
            catch (SqlException sqlException)
            {
                var failedSeatStorageException =
                    new FailedSeatStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedSeatStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsSeatException =
                    new AlreadyExistsSeatException(duplicateKeyException);

                throw CreateAndDependencyValidationException(alreadyExistsSeatException);
            }
            catch (Exception serviceException)
            {
                var failedSeatServiceException =
                    new FailedSeatServiceException(serviceException);

                throw CreateAndLogServiceException(failedSeatServiceException);
            }

        }
        private IQueryable<Seat> TryCatch(ReturningSeatsFunction returningSeatsFunction)
        {
            try
            {
                return returningSeatsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedSeatStorageException =
                    new FailedSeatStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedSeatStorageException);
            }
            catch (Exception serviceException)
            {
                var failedSeatServiceException =
                    new FailedSeatServiceException(serviceException);

                throw CreateAndLogServiceException(failedSeatServiceException);
            }
        }

        private SeatValidationException CreateAndLogValidationException(Xeption exception)
        {
            var seatVadlidationException = new SeatValidationException(exception);
            this.loggingBroker.LogError(seatVadlidationException);

            return seatVadlidationException;

        }
        private SeatDependencyException CreateAndLogCriticalDependencyException(Xeption exeption)
        {
            var seatDependencyException = new SeatDependencyException(exeption);
            this.loggingBroker.LogCritical(seatDependencyException);

            return seatDependencyException;
        }
        private SeatDependencyValidationException CreateAndDependencyValidationException(Xeption exception)
        {
            var seatDependencyValidationException =
                new SeatDependencyValidationException(exception);

            this.loggingBroker.LogError(seatDependencyValidationException);

            return seatDependencyValidationException;
        }
        private SeatServiceException CreateAndLogServiceException(Xeption exception)
        {
            var seatServiceException =
                new SeatServiceException(exception);

            this.loggingBroker.LogError(seatServiceException);

            return seatServiceException;
        }
    }
}
