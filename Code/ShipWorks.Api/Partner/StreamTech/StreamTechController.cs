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
using ShipWorks.Api.Orders;
using ShipWorks.Api.Orders.Shipments;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;
using Swashbuckle.Swagger.Annotations;

namespace ShipWorks.Api.Partner.StreamTech
{
    /// <summary>
    /// Controller for StreamTech
    /// </summary>
    [ApiVersion("1.0")]
    [RoutePrefix("shipworks/api/v{version:apiVersion}/partner/streamtech")]
    [Obfuscation(Exclude = true)]
    public class StreamTechController : ApiController
    {
        private readonly IApiOrderRepository orderRepository;
        private readonly IShipmentFactory shipmentFactory;
        private readonly IApiShipmentProcessor shipmentProcessor;
        private readonly ICarrierShipmentAdapterFactory carrierShipmentAdapterFactory;
        private readonly IApiLabelFactory apiLabelFactory;
        private readonly IShippingProfileRepository shippingProfileRepository;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public StreamTechController(
            IApiOrderRepository orderRepository,
            IShipmentFactory shipmentFactory,
            IApiShipmentProcessor shipmentProcessor,
            ICarrierShipmentAdapterFactory carrierShipmentAdapterFactory,
            IApiLabelFactory apiLabelFactory,
            IShippingProfileRepository shippingProfileRepository,
            Func<Type, ILog> logFactory)
        {
            this.orderRepository = orderRepository;
            this.shipmentFactory = shipmentFactory;
            this.shipmentProcessor = shipmentProcessor;
            this.carrierShipmentAdapterFactory = carrierShipmentAdapterFactory;
            this.apiLabelFactory = apiLabelFactory;
            this.shippingProfileRepository = shippingProfileRepository;
            log = logFactory(typeof(StreamTechController));
        }

        /// <summary>
        /// Returns an order matching the number or order ID
        /// </summary>
        /// <param name="barcode">The request from StreamTech</param>
        /// <param name="streamTechRequest">The request from StreamTech</param>
        [HttpPost]
        [Route("{barcode}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string), Description = "Label information conforming to the StreamTech spec")]
        [SwaggerResponse(HttpStatusCode.NotFound, Type = typeof(string), Description = "No order found")]
        [SwaggerResponse(HttpStatusCode.Conflict, Type = typeof(string), Description = "Multiple Orders found matching the OrderNumber")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(string), Description = "The server is experiencing errors")]

        public async Task<HttpResponseMessage> ProcessShipment([FromUri]string barcode, [FromBody]StreamTechRequest streamTechRequest)
        {
            var request = streamTechRequest.Request;
            try
            {
                IEnumerable<OrderEntity> orders = orderRepository.GetOrders(barcode);

                ComparisonResult comparisonResult = orders.CompareCountTo(1);

                switch (comparisonResult)
                {
                    case ComparisonResult.Equal:
                        ShipmentEntity shipment = shipmentFactory.Create(orders.First());

                        var shipmentAdapter = carrierShipmentAdapterFactory.Get(shipment);

                        shippingProfileRepository.GetAll()
                            .Where(p => p.Shortcut.Barcode == request.PackageType)
                            .FirstOrDefault()?.Apply(shipment);

                        foreach (IPackageAdapter package in shipmentAdapter.GetPackageAdapters())
                        {
                            package.Weight = request.Weight;
                            package.DimsLength = request.Length;
                            package.DimsWidth = request.Width;
                            package.DimsHeight = request.Height;
                        }

                        ProcessShipmentResult processResult = await shipmentProcessor.Process(shipment).ConfigureAwait(false);

                        var responseData = new ResponseData()
                        {
                            MessageNumber = request.MessageNumber
                        };

                        if (processResult.IsSuccessful)
                        {
                            var processedShipment = processResult.Shipment;
                            var processedShipmentAdapter = carrierShipmentAdapterFactory.Get(processedShipment);

                            responseData.VerifyBarcodes = processedShipment.TrackingNumber;
                            responseData.CarrierCode = processedShipment.ShipmentTypeCode.ToString();
                            responseData.ExpectedWeight = processedShipment.TotalWeight;
                            //responseData.MinimumWeight = processedShipment.TotalWeight;
                            //responseData.MaximumWeight = processedShipment.TotalWeight;
                            responseData.ZplLabel = apiLabelFactory.GetLabels(processedShipmentAdapter).First().Image;
                        }
                        else
                        {
                            Exception ex = processResult.Error;
                            responseData.ErrorCode = "99";
                            responseData.ErrorDescription = ex.Message;
                        }

                        return CreateResponse(Request, HttpStatusCode.OK, responseData);

                    case ComparisonResult.More:
                        // More than 1 order found, return 409
                        return CreateResponse(Request, HttpStatusCode.Conflict, new ResponseData()
                        {
                            MessageNumber = request.MessageNumber,
                            ErrorCode = "409",
                            ErrorDescription = "Multiple Orders found matching the OrderNumber"
                        });
                    default:
                        // No orders found, return 404
                        return CreateResponse(Request, HttpStatusCode.NotFound, new ResponseData()
                        {
                            MessageNumber = request.MessageNumber,
                            ErrorCode = "404",
                            ErrorDescription = "No order found"
                        });
                }
            }
            catch (Exception ex)
            {
                log.Error("An error occured getting orders", ex);

                return CreateResponse(Request, HttpStatusCode.InternalServerError, new ResponseData()
                {
                    MessageNumber = request.MessageNumber,
                    ErrorCode = "500",
                    ErrorDescription = ex.Message
                });
            }
        }

        /// <summary>
        /// Create a response using the given data
        /// </summary>
        private static HttpResponseMessage CreateResponse(HttpRequestMessage request, HttpStatusCode statusCode, ResponseData responseData)
        {
            return request.CreateResponse(statusCode, new StreamTechResponse()
            {
                Response = responseData
            });
        }
    }
}
