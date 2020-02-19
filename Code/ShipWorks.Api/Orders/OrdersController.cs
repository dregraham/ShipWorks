using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using Interapptive.Shared.Collections;
using log4net;
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
    public class OrdersController : ApiController
    {
        private readonly IApiOrderRepository orderRepository;
        private readonly IOrdersResponseFactory responseFactory;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrdersController(IApiOrderRepository orderRepository, IOrdersResponseFactory responseFactory, Func<Type, ILog> logFactory)
        {
            this.orderRepository = orderRepository;
            this.responseFactory = responseFactory;

            log = logFactory(typeof(OrdersController));
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

                ComparisonResult comparisonResult = orders.CompareCountTo(1);

                switch (comparisonResult)
                {
                    case ComparisonResult.Equal:
                        // For a single order, create the response and return it with a 200
                        OrderResponse response = responseFactory.CreateOrdersResponse(orders.SingleOrDefault());
                        return Request.CreateResponse(HttpStatusCode.OK, response);
                    case ComparisonResult.More:
                        // More than 1 order found, return 409
                        return Request.CreateResponse(HttpStatusCode.Conflict);
                    default:
                        // No orders found, return 404
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                log.Error("An error occured getting orders", ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
