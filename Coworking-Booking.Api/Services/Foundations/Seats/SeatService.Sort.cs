﻿using Coworking_Booking.Api.Models.Seats;
using System.Linq;

namespace Coworking_Booking.Api.Services.Foundations.Seats
{
    public partial class SeatService
    {
        private IQueryable<Seat> GetEmptySeats()
        {
            IQueryable<Seat> listOfData = storageBroker.SelectAllSeats()
                .OrderBy(u => u.IsBooked ? 0 : 1);

            return listOfData;
        }
    }
}