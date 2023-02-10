using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NajotBooking.Api.Models.Orders;
using NajotBooking.Api.Models.Orders.Exceptions;
using NajotBooking.Api.Services.Foundations.Orders;
using RESTFulSense.Controllers;

namespace NajotBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class OrdersController : RESTFulController
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService) =>
            this.orderService = orderService;

        [HttpPost]
        public async ValueTask<ActionResult<Order>> PostOrderAsync(Order order)
        {
            try
            {
                return await this.orderService.AddOrderAsync(order);
            }
            catch (OrderValidationException orderValidationExpection)
            {
                return BadRequest(orderValidationExpection.InnerException);
            }
            catch (OrderDependencyValidationException orderDependencyValidationException)
                when (orderDependencyValidationException.InnerException is AlreadyExistsOrderException)
            {
                return Conflict(orderDependencyValidationException.InnerException);
            }
            catch (OrderDependencyValidationException orderDependencyValidationException)
            {
                return BadRequest(orderDependencyValidationException.InnerException);
            }
            catch (OrderDependencyException orderDependencyException)
            {
                return InternalServerError(orderDependencyException.InnerException);
            }
            catch (OrderServiceException orderServiceException)
            {
                return InternalServerError(orderServiceException);
            }
        }
    }
}
