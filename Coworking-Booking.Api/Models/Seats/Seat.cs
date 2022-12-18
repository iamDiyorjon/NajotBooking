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
        public Guid UserId { get; set; }
        public Branch Branch { get; set; }
        public int Floor { get; set; }
        public int SeatNumber { get; set; }
        public bool IsBooked { get; set; }
        public DateTimeOffset CraetedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}