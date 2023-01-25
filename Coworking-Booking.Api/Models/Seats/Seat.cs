// ---------------------------------------------------------------
// Copyright (c) Coalition Of The THE STANDART SHARPISTS
// Free To Use To Book Places In Coworking Zones
// ---------------------------------------------------------------

using System;

namespace Coworking_Booking.Api.Models.Seats
{
    public class Seat
    {
        public Guid Id { get; set; }
        public Branch Branch { get; set; }
        public int Floor { get; set; }
        public int Number { get; set; }
        public decimal Price { get; set; }
    }
}