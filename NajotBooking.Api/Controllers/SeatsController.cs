﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NajotBooking.Api.Models.Seats;
using NajotBooking.Api.Services.Foundations.Seats;

namespace NajotBooking.Api.Controllers
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
            IQueryable<Seat> seats = this.seatService.RetrieveAllSeat();
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