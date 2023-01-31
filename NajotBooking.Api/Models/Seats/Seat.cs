// ---------------------------------------------------------------
// Copyright (c) Coalition Of The THE STANDART SHARPISTS
// Free To Use To Book Places In Coworking Zones
// ---------------------------------------------------------------

using System;

namespace NajotBooking.Api.Models.Seats
{
    public class Seat
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public Branch Branch { get; set; }
        public int Floor { get; set; }
        public DateTimeOffset StartingTime { get; set; }
        public DateTimeOffset EndingTime { get; set; }
        public DateTimeOffset CraetedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}