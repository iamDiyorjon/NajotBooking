// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using NajotBooking.Api.Brokers.Storages;
using NajotBooking.Api.Models.Orders;

namespace NajotBooking.Api.Services.Foundations.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IStorageBroker storageBroker;

        public OrderService(IStorageBroker storageBroker) =>
            this.storageBroker = storageBroker;

        public ValueTask<Order> AddOrderAsync(Order order) =>
            throw new NotImplementedException();
    }
}
