using Coworking_Booking.Api.Brokers.DateTimes;
using Coworking_Booking.Api.Brokers.Loggings;
using Coworking_Booking.Api.Brokers.Storages;
using Coworking_Booking.Api.Models.Orders;
using System.Linq;

namespace Coworking_Booking.Api.Services.Foundations.Orders
{
    public partial class OrderService : IOrderService
    {
        IStorageBroker storageBroker;
        ILoggingBroker logginBroker;
        IDateTimeBroker dateTimeBroker;

        public OrderService(
            IStorageBroker storageBroker, 
            ILoggingBroker logginBroker, 
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.logginBroker = logginBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public IQueryable<Order> RetrieveAllOrder() => 
            this.storageBroker.SelectAllOrders();
    }
}
