// ---------------------------------------------------------------
// Copyright (c) Coalition Of The THE STANDART SHARPISTS
// Free To Use To Book Places In Coworking Zones
// ---------------------------------------------------------------

using Coworking_Booking.Api.Models.Seats;
using Coworking_Booking.Api.Models.Users;
using System;

namespace Coworking_Booking.Api.Models.Order
{
    public class Order
    {
        public Guid Id { get; set; }
        
        public Guid SeatId { get; set; }
        public Seat Seat { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set;}
        public 
    }
}
