// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System.Linq;
using System;
using System.Threading.Tasks;
using NajotBooking.Api.Models.Orders;

namespace NajotBooking.Api.Services.Foundations.Orders
{
    public interface IOrderService
    {
        ValueTask<Order> AddOrderAsync(Order order);
        IQueryable<Order> RetrieveAllOrders();
        ValueTask<Order> RemoveOrderByIdAsync(Guid orderId);
    }
}
