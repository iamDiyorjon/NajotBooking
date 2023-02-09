using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NajotBooking.Api.Models.Seats;
using NajotBooking.Api.Models.Seats.Exceptions;
using NajotBooking.Api.Services.Foundations.Seats;
using RESTFulSense.Controllers;

namespace NajotBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatsController : RESTFulController
    {
        private readonly ISeatService seatService;

        public SeatsController(ISeatService seatService) =>
            this.seatService = seatService;

        [HttpPost]
        public async ValueTask<ActionResult<Seat>> PostSeatAsync(Seat seat)
        {
            try
            {
                return await this.seatService.AddSeatAsync(seat);
            }
            catch (SeatValidationException seatValidationException)
            {
                return BadRequest(seatValidationException.InnerException);
            }
            catch (SeatDependencyValidationException seatDependencyValidationException)
                when (seatDependencyValidationException.InnerException is AlreadyExistsSeatException)
            {
                return Conflict(seatDependencyValidationException.InnerException);
            }
            catch (SeatDependencyValidationException seatDependencyValidationException)
            {
                return BadRequest(seatDependencyValidationException.InnerException);
            }
            catch (SeatDependencyException seatDependencyException)
            {
                return InternalServerError(seatDependencyException.InnerException);
            }
            catch (SeatServiceException seatServiceException)
            {
                return InternalServerError(seatServiceException.InnerException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Seat>> GetAllSeats()
        {
            try
            {
                IQueryable<Seat> allSeats = this.seatService.RetrieveAllSeat();

                return Ok(allSeats);
            }
            catch (SeatDependencyException seatDependencyException)
            {
                return InternalServerError(seatDependencyException.InnerException);
            }
            catch (SeatServiceException seatServiceException)
            {
                return InternalServerError(seatServiceException.InnerException);
            }
        }

        [HttpGet("{seatId}")]
        public async ValueTask<ActionResult<Seat>> GetSeatByIdAsync(Guid seatId)
        {
            try
            {
                return await this.seatService.RetrieveSeatByIdAsync(seatId);
            }
            catch (SeatDependencyException seatDependencyException)
            {
                return InternalServerError(seatDependencyException.InnerException);
            }
            catch (SeatValidationException seatValidationException)
                when (seatValidationException.InnerException is InvalidSeatException)
            {
                return BadRequest(seatValidationException.InnerException);
            }
            catch (SeatValidationException seatValidationException)
                when (seatValidationException.InnerException is NotFoundSeatException)
            {
                return NotFound(seatValidationException.InnerException);
            }
            catch (SeatServiceException seatServiceException)
            {
                return InternalServerError(seatServiceException.InnerException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Seat>> PutSeatAsync(Seat seat)
        {
            try
            {
                Seat modifiedSeat =
                    await this.seatService.ModifySeatAsync(seat);

                return Ok(modifiedSeat);
            }
            catch (SeatValidationException seatValidationException)
                when (seatValidationException.InnerException is NotFoundSeatException)
            {
                return NotFound(seatValidationException.InnerException);
            }
            catch (SeatValidationException seatValidationException)
            {
                return BadRequest(seatValidationException.InnerException);
            }
            catch (SeatDependencyValidationException seatDependencyValidationException)
                when (seatDependencyValidationException.InnerException is AlreadyExistsSeatException)
            {
                return Conflict(seatDependencyValidationException.InnerException);
            }
            catch (SeatDependencyException seatDependencyException)
            {
                return InternalServerError(seatDependencyException.InnerException);
            }
            catch (SeatServiceException seatServiceException)
            {
                return InternalServerError(seatServiceException.InnerException);
            }
        }

        [HttpDelete]
        public async ValueTask<ActionResult<Seat>> DeleteSeatAsync(Guid seatId) =>
            await this.seatService.RemoveSeatByIdAsync(seatId);
    }
}
