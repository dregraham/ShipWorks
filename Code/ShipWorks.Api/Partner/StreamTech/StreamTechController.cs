using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Interapptive.Shared;
using Interapptive.Shared.Collections;
using log4net;
using Microsoft.Web.Http;
using ShipWorks.Api.Orders;
using ShipWorks.Api.Orders.Shipments;
using ShipWorks.Common.IO.Hardware.Printers;
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
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public StreamTechController(
            IApiOrderRepository orderRepository,
            IShipmentFactory shipmentFactory,
            IApiShipmentProcessor shipmentProcessor,
            ICarrierShipmentAdapterFactory carrierShipmentAdapterFactory,
            IApiLabelFactory apiLabelFactory,
            IShippingProfileRepository shippingProfileRepository,
            IShipmentTypeManager shipmentTypeManager,
            Func<Type, ILog> logFactory)
        {
            this.orderRepository = orderRepository;
            this.shipmentFactory = shipmentFactory;
            this.shipmentProcessor = shipmentProcessor;
            this.carrierShipmentAdapterFactory = carrierShipmentAdapterFactory;
            this.apiLabelFactory = apiLabelFactory;
            this.shippingProfileRepository = shippingProfileRepository;
            this.shipmentTypeManager = shipmentTypeManager;
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
            RequestData request = streamTechRequest.Request;

            if (request == null)
            {
                return CreateResponse(Request, HttpStatusCode.BadRequest, new ResponseData()
                {
                    MessageNumber = "0",
                    ErrorCode = 400,
                    ZplLabel = Convert.ToBase64String(Encoding.UTF8.GetBytes("Invalid Request"))
                });
            }

            try
            {
                IEnumerable<OrderEntity> orders = orderRepository.GetOrders(barcode);
                ComparisonResult comparisonResult = orders.CompareCountTo(1);

                switch (comparisonResult)
                {
                    case ComparisonResult.Equal:
                        ProcessShipmentResult processResult = await ProcessShipment(orders.First(), request);
                        ResponseData responseData = BuildResponseData(processResult, request);

                        return CreateResponse(Request, HttpStatusCode.OK, responseData);
                    case ComparisonResult.More:
                        // More than 1 order found, return 409
                        return CreateResponse(Request, HttpStatusCode.Conflict, new ResponseData()
                        {
                            MessageNumber = request.MessageNumber,
                            ErrorCode = 409,
                            ZplLabel = Convert.ToBase64String(Encoding.UTF8.GetBytes("Multiple Orders found matching the OrderNumber"))
                        }); ;
                    default:
                        // No orders found, return 404
                        return CreateResponse(Request, HttpStatusCode.NotFound, new ResponseData()
                        {
                            MessageNumber = request.MessageNumber,
                            ErrorCode = 404,
                            ZplLabel = Convert.ToBase64String(Encoding.UTF8.GetBytes("No order found"))
                        });
                }
            }
            catch (Exception ex)
            {
                log.Error("An error occured getting orders", ex);

                return CreateResponse(Request, HttpStatusCode.InternalServerError, new ResponseData()
                {
                    MessageNumber = request.MessageNumber,
                    ErrorCode = 500,
                    ZplLabel = Convert.ToBase64String(Encoding.UTF8.GetBytes(ex.Message))
                });
            }
        }

        /// <summary>
        /// Build the ResponseData based on the shipment result
        /// </summary>
        /// <param name="processShipmentResult">The result of processing the shipment</param>
        /// <param name="requestData">The request data from StreamTech</param>
        private ResponseData BuildResponseData(ProcessShipmentResult processShipmentResult, RequestData requestData)
        {
            ResponseData responseData = new ResponseData()
            {
                MessageNumber = requestData.MessageNumber, 
                PackageIDBarcode = requestData.PackageIDBarcode
            };

            if (processShipmentResult.IsSuccessful)
            {
                ShipmentEntity processedShipment = processShipmentResult.Shipment;
                ICarrierShipmentAdapter processedShipmentAdapter = carrierShipmentAdapterFactory.Get(processedShipment);

                responseData.VerifyBarcodes = processedShipment.TrackingNumber;
                responseData.CarrierCode = processedShipment.ShipmentTypeCode.ToString();
                responseData.ExpectedWeight = processedShipment.TotalWeight;

                responseData.SortCode = processedShipmentAdapter.ServiceTypeName;

                // This feature is only available in some StreamTech situations
                // in the future we might want to have a user configurable option
                // to set tolerance for what we consider max and min allowed weights 
                //responseData.MinimumWeight = processedShipment.TotalWeight;
                //responseData.MaximumWeight = processedShipment.TotalWeight;

                var label = apiLabelFactory.GetLabels(processedShipmentAdapter).FirstOrDefault();
                if (label == null)
                {
                    responseData.ErrorCode = 99;
                    responseData.ZplLabel = Convert.ToBase64String(Encoding.UTF8.GetBytes("Label not Generated."));
                }
                else
                {
                    responseData.ZplLabel = label.Image;
                }
            }
            else
            {
                Exception ex = processShipmentResult.Error;
                responseData.ErrorCode = 99;
                responseData.ZplLabel = Convert.ToBase64String(Encoding.UTF8.GetBytes(ex.Message));
            }

            return responseData;
        }

        /// <summary>
        /// Process a shipment for the given order based on the RquestData
        /// </summary>
        /// <param name="order">The order to process a shipment for</param>
        /// <param name="request">Data used to process the shipment, dims, weight and profile to apply</param>
        /// <returns></returns>
        private Task<ProcessShipmentResult> ProcessShipment(OrderEntity order, RequestData request)
        {
            ShipmentEntity shipment = shipmentFactory.Create(order);

            shippingProfileRepository.GetAll()
                .Where(p => p.Shortcut.Barcode == request.PackageType)
                .FirstOrDefault()?.Apply(shipment);

            ICarrierShipmentAdapter shipmentAdapter = carrierShipmentAdapterFactory.Get(shipment);

            foreach (IPackageAdapter package in shipmentAdapter.GetPackageAdapters())
            {
                package.Weight = request.Weight;
                package.DimsLength = request.Length;
                package.DimsWidth = request.Width;
                package.DimsHeight = request.Height;
            }

            shipmentAdapter.UpdateDynamicData();

            // Force ZPL because its the only format streamtech supports
            shipmentTypeManager.Get(shipment).SaveRequestedLabelFormat(ThermalLanguage.ZPL, shipment);

            return shipmentProcessor.Process(shipment);
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
