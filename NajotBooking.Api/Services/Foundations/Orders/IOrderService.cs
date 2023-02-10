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
        IQueryable<Order> RetrieveAllOrders();
        ValueTask<Order> RetrieveOrderByIdAsync(Guid orderId);
        ValueTask<Order> ModifyOrderAsync(Order order);
        ValueTask<Order> RemoveOrderByIdAsync(Guid orderId);
    }
}
