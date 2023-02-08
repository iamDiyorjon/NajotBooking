// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using NajotBooking.Api.Brokers.Loggings;
using NajotBooking.Api.Brokers.Storages;
using NajotBooking.Api.Models.Orders;

namespace NajotBooking.Api.Services.Foundations.Orders
{
    public partial class OrderService : IOrderService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public OrderService(IStorageBroker storageBroker, ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Order> AddOrderAsync(Order order) =>
        TryCatch(async () =>
        {
            ValidateOrder(order);

            return await this.storageBroker.InsertOrderAsync(order);
        });

        public async ValueTask<Order> RetrieveOrderByIdAsync(Guid orderId) =>
            await this.storageBroker.SelectOrderByIdAsync(orderId);

        public IQueryable<Order> RetrieveAllOrders() =>
        TryCatch(() => this.storageBroker.SelectAllOrders());

        public ValueTask<Order> RemoveOrderByIdAsync(Guid orderId) =>
        TryCatch(async () =>
        {
            ValidationOrderId(orderId);

            Order maybeOrder = await this.storageBroker.SelectOrderByIdAsync(orderId);

            ValidateStorageOrderExists(maybeOrder, orderId);

            return await this.storageBroker.DeleteOrderAsync(maybeOrder);
        });
    }
}