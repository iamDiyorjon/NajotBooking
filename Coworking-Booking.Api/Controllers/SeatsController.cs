using Coworking_Booking.Api.Models.Seats;
using Coworking_Booking.Api.Services.Foundations.Seats;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coworking_Booking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService seatService;

        public SeatsController(ISeatService seatService) =>
            this.seatService = seatService;

        [HttpPost]
        public async ValueTask<ActionResult<Seat>> PostSeatAsync(Seat seat) =>
            await this.seatService.AddSeatAsync(seat);

        [HttpGet("{seatId}")]
        public async ValueTask<ActionResult<Seat>> GetByIdAsync(Guid id) =>
            await this.seatService.RetrieveSeatByIdAsync(id);

        [HttpGet]
        public ActionResult<IQueryable<Seat>> GetAll()
        {
            IQueryable<Seat> seats = this.seatService.RetrieveAllSeatAsync();
            return Ok(seats);
        }

        [HttpPut]
        public async ValueTask<ActionResult<Seat>> PutSeatAsync(Seat seat) =>
            await this.seatService.ModifySeatAsync(seat);

        [HttpDelete]
        public async ValueTask<ActionResult<Seat>> DeleteSeatAsync(Seat seat) =>
            await this.seatService.RemoveSeat(seat);
    }
}
