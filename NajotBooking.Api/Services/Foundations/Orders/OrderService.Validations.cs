// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using NajotBooking.Api.Models.Orders;
using NajotBooking.Api.Models.Orders.Exceptions;

namespace NajotBooking.Api.Services.Foundations.Orders
{
    public partial class OrderService
    {
        private static void ValidateOrderNotNull(Order order)
        {
            if (order is null)
            {
                throw new NullOrderException();
            }
        }
    }
}
