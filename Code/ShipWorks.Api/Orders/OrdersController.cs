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
using Swashbuckle.Swagger.Annotations;

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
        private readonly IOrderResponseFactory responseFactory;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrdersController(IApiOrderRepository orderRepository, IOrderResponseFactory responseFactory, Func<Type, ILog> logFactory)
        {
            this.orderRepository = orderRepository;
            this.responseFactory = responseFactory;

            log = logFactory(typeof(OrdersController));
        }

        /// <summary>
        /// Returns an order matching the number or order ID
        /// </summary>
        /// <param name="orderNumber">The order number or internal order ID of the order to return</param>
        [HttpGet]
        [Route("{orderNumber}")]
        [SwaggerResponse(HttpStatusCode.OK,
            Type = typeof(OrderResponse),
            Description = "An Order object")]
        [SwaggerResponse(HttpStatusCode.NotFound, Description = "No order found.")]
        [SwaggerResponse(HttpStatusCode.Conflict, Description = "Multiple Orders found matching the OrderNumber")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "The server is experiencing errors")]
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
                        OrderResponse response = responseFactory.Create(orders.SingleOrDefault());
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
