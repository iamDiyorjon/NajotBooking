// ---------------------------------------------------------------
// Copyright (c) Coalition Of The THE STANDART SHARPISTS
// Free To Use To Book Places In Coworking Zones
// ---------------------------------------------------------------

using Coworking_Booking.Api.Models.Seats;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coworking_Booking.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Seat> InsertSeatAsync(Seat seat);
        IQueryable<Seat> SelectAllSeats();
        ValueTask<Seat> SelectSeatByIdAsync(Guid id);
        ValueTask<Seat> UpdateSeatAsync(Seat seat);
        ValueTask<Seat> DeleteSeatAsync(Seat seat);
    }
}
