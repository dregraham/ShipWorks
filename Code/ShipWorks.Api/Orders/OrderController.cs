using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using Microsoft.Web.Http;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Api.Orders
{
    /// <summary>
    /// Controller for getting an order
    /// </summary>
    [ApiVersion("1.0")]
    [RoutePrefix("shipworks/api/v{version:apiVersion}/orders")]
    [Obfuscation(Exclude = true)]
    public class OrderController : ApiController
    {
        private readonly IApiOrderRepository orderRepository;
        private readonly IOrderResponseFactory responseFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderController(IApiOrderRepository orderRepository, IOrderResponseFactory responseFactory)
        {
            this.orderRepository = orderRepository;
            this.responseFactory = responseFactory;
        }

        /// <summary>
        /// Returns an order matching the number or order ID
        /// </summary>
        /// <param name="orderNumber">The order number or internal order ID of the order to return</param>
        /// <response code="200">An Order object</response>
        /// <response code="404">No Order found</response>
        /// <response code="409">Multiple Orders found matching the OrderNumber</response>
        /// <response code="500">The server is experiencing errors</response>
        [HttpGet]
        [Route("{orderNumber}")]
        public HttpResponseMessage Get(string orderNumber)
        {
            try
            {
                IEnumerable<OrderEntity> orders = orderRepository.GetOrders(orderNumber);

                int orderCount = orders.Count();

                if (orderCount > 0)
                {
                    if (orderCount == 1)
                    {
                        OrderResponse response = responseFactory.Create(orders.SingleOrDefault());

                        // return the single order
                        return Request.CreateResponse(HttpStatusCode.OK, response);
                    }

                    // More than 1 order found, return 409
                    return Request.CreateResponse(HttpStatusCode.Conflict);
                }

                // No orders found, return 404
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
