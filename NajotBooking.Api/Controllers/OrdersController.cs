using System;
using System.Linq;
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
                return InternalServerError(orderServiceException.InnerException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Order>> GetAllOrders()
        {
            try
            {
                IQueryable<Order> allOrders = this.orderService.RetrieveAllOrders();

                return Ok(allOrders);
            }
            catch (OrderDependencyException orderDependencyException)
            {
                return InternalServerError(orderDependencyException.InnerException);
            }
            catch (OrderServiceException orderServiceException)
            {
                return InternalServerError(orderServiceException.InnerException);
            }
        }

        [HttpGet("{orderId}")]
        public async ValueTask<ActionResult<Order>> GetOrderByIdAsync(Guid Id)
        {
            try
            {
                return await this.orderService.RetrieveOrderByIdAsync(Id);
            }
            catch (OrderDependencyException orderDependencyException)
            {
                return InternalServerError(orderDependencyException.InnerException);
            }
            catch (OrderValidationException orderValidationException)
                when (orderValidationException.InnerException is InvalidOrderException)
            {
                return BadRequest(orderValidationException.InnerException);
            }
            catch (OrderValidationException orderValidationException)
                when (orderValidationException.InnerException is NotFoundOrderException)
            {
                return NotFound(orderValidationException.InnerException);
            }
            catch (OrderServiceException orderServiceException)
            {
                return InternalServerError(orderServiceException.InnerException);
            }
        }

        [HttpDelete("{orderId}")]
        public async ValueTask<ActionResult<Order>> DeleteOrderByIdAsync(Guid Id)
        {
            try
            {
                Order deletedOrder =
                    await this.orderService.RemoveOrderByIdAsync(Id);

                return Ok(deletedOrder);
            }
            catch (OrderValidationException orderValidationException)
                when (orderValidationException.InnerException is NotFoundOrderException)
            {
                return NotFound(orderValidationException.InnerException);
            }
            catch (OrderValidationException orderValidationException)
            {
                return BadRequest(orderValidationException.InnerException);
            }
            catch (OrderDependencyValidationException orderDependencyValidationException)
                when (orderDependencyValidationException.InnerException is LockedOrderException)
            {
                return Locked(orderDependencyValidationException.InnerException);
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
                return InternalServerError(orderServiceException.InnerException);
            }
        }
    }
}
