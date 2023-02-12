// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System;

namespace NajotBooking.Api.Models.Orders
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid SeatId { get; set; }
        public Guid UserId { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public int Duration { get; set; }
    }
}
