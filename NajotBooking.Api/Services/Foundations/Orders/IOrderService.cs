// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using NajotBooking.Api.Models.Orders;

namespace NajotBooking.Api.Services.Foundations.Orders
{
    public interface IOrderService
    {
        ValueTask<Order> AddOrderAsync(Order order);
        ValueTask<Order> RetrieveOrderByIdAsync(Guid orderId);
        IQueryable<Order> RetrieveAllOrders();
        ValueTask<Order> RemoveOrderByIdAsync(Guid orderId);
    }
}
