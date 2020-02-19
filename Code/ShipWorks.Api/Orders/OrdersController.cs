using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using Interapptive.Shared.Collections;
using log4net;
using Microsoft.Web.Http;
using ShipWorks.Api.Orders.Shipments;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping;

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
        private readonly IShipmentFactory shipmentFactory;
        private readonly IApiShipmentProcessor shipmentProcessor;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrdersController(
            IApiOrderRepository orderRepository, 
            IOrdersResponseFactory responseFactory, 
            IShipmentFactory shipmentFactory,
            IApiShipmentProcessor shipmentProcessor, 
            Func<Type, ILog> logFactory)
        {
            this.orderRepository = orderRepository;
            this.responseFactory = responseFactory;
            this.shipmentFactory = shipmentFactory;
            this.shipmentProcessor = shipmentProcessor;

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
        public async Task<HttpResponseMessage> Get(string orderNumber)
        {
            return await ExecuteOrdersAction((o) => {
                OrderResponse response = responseFactory.CreateOrdersResponse(o);
                return Task.FromResult(Request.CreateResponse(HttpStatusCode.OK, response));
            }, orderNumber).ConfigureAwait(false);
        }

        /// <summary>
        /// Create a shipment for the given order
        /// </summary>
        /// <param name="orderNumber">The order number or internal order ID of the order to return</param>
        /// <response code="200">An Order object</response>
        /// <response code="404">No Order found</response>
        /// <response code="409">Multiple Orders found matching the OrderNumber</response>
        /// <response code="500">The server is experiencing errors</response>
        [HttpPost]
        [Route("{orderNumber}/shipments")]
        public Task<HttpResponseMessage> CreateShipment(string orderNumber) => 
            ExecuteOrdersAction(ProcessShipment, orderNumber);

        /// <summary>
        /// Process a shipment for the given order
        /// </summary>
        private async Task<HttpResponseMessage> ProcessShipment(OrderEntity order)
        {
            ShipmentEntity shipment = shipmentFactory.Create(order);
            ProcessShipmentResult processResult = await shipmentProcessor.Process(shipment).ConfigureAwait(false);

            if (!processResult.IsSuccessful)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, processResult.Error.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK, responseFactory.CreateProcessShipmentResponse(processResult));
        }

        /// <summary>
        /// Execute the given action on an order found matching the OrderNumber
        /// </summary>
        public async Task<HttpResponseMessage> ExecuteOrdersAction(Func<OrderEntity, Task<HttpResponseMessage>> action, string orderNumber)
        {
            try
            {
                IEnumerable<OrderEntity> orders = orderRepository.GetOrders(orderNumber);

                ComparisonResult comparisonResult = orders.CompareCountTo(1);

                switch (comparisonResult)
                {
                    case ComparisonResult.Equal:
                        return await action(orders.First());
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
