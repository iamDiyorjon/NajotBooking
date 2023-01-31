// ---------------------------------------------------------------
// Copyright (c) Coalition Of The THE STANDART SHARPISTS
// Free To Use To Book Places In Coworking Zones
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NajotBooking.Api.Models.Seats;

namespace NajotBooking.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Seat> Seats { get; set; }

        public async ValueTask<Seat> InsertSeatAsync(Seat seat) =>
            await InsertAsync(seat);

        public IQueryable<Seat> SelectAllSeats() =>
            SelectAll<Seat>();

        public async ValueTask<Seat> SelectSeatByIdAsync(Guid id) =>
            await SelectAsync<Seat>(id);

        public async ValueTask<Seat> UpdateSeatAsync(Seat seat) =>
            await UpdateAsync(seat);

        public async ValueTask<Seat> DeleteSeatAsync(Seat seat) =>
            await DeleteAsync(seat);
    }
}