using Coworking_Booking.Api.Models.Orders;
using System.Linq;

namespace Coworking_Booking.Api.Services.Foundations.Orders
{
    public interface IOrderService
    {
        IQueryable<Order> RetrieveAllOrder();
    }
}
