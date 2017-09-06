using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Walmart.DTO;

namespace ShipWorks.Stores.Platforms.Walmart.OnlineUpdating
{
    /// <summary>
    /// Online updater for Walmart
    /// </summary>
    [Component]
    public class ShipmentDetailsUpdater : IShipmentDetailsUpdater
    {
        private readonly IWalmartWebClient webClient;
        private readonly IOrderRepository orderRepository;
        private readonly IWalmartOrderLoader orderLoader;
        private readonly IOrderManager orderManager;
        readonly IWalmartCombineOrderSearchProvider combineOrderSearchProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="WalmartOnlineUpdater"/> class.
        /// </summary>
        public ShipmentDetailsUpdater(IWalmartWebClient webClient,
            IOrderManager orderManager,
            IOrderRepository orderRepository,
            IWalmartOrderLoader orderLoader,
            IWalmartCombineOrderSearchProvider combineOrderSearchProvider)
        {
            this.combineOrderSearchProvider = combineOrderSearchProvider;
            this.webClient = webClient;
            this.orderManager = orderManager;
            this.orderRepository = orderRepository;
            this.orderLoader = orderLoader;
        }

        /// <summary>
        /// Upload carrier and tracking information for the given orders
        /// </summary>
        public async Task UpdateShipmentDetails(IWalmartStoreEntity store, long orderID)
        {
            ShipmentEntity shipment = orderManager.GetLatestActiveShipment(orderID);

            // Check to see if shipment exists and order has shippable line item
            if (shipment != null)
            {
                await UpdateShipmentDetails(store, shipment).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Upload carrier and tracking information for the given shipment
        /// </summary>
        /// <remarks>
        /// Only uploads if there is at least one line that has an OnlineStatus = Acknowledged.
        /// If Walmart returns an error, we download the order again, save it and try again if
        /// there is still an acknowledged line.
        /// </remarks>
        public async Task UpdateShipmentDetails(IWalmartStoreEntity store, ShipmentEntity shipment)
        {
            var order = shipment.Order as WalmartOrderEntity;
            orderRepository.PopulateOrderDetails(order);

            if (order.IsManual)
            {
                return;
            }

            var identifiers = await combineOrderSearchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

            identifiers
                .Where(x => x != null)
                .Select(x => UploadCombinedShipmentDetails(store, order, shipment, x))
                .ThrowFailures((msg, ex) => new WalmartException(msg, ex));
        }

        /// <summary>
        /// Upload shipment details for orders, including combined orders
        /// </summary>
        private IResult UploadCombinedShipmentDetails(IWalmartStoreEntity store, WalmartOrderEntity order, ShipmentEntity shipment, WalmartCombinedIdentifier identifier)
        {
            GenericResult<Order> result = InternalUpdateShipmentDetails(store, order, shipment, identifier);
            if (result.Failure && ShouldRetry(result.Exception))
            {
                Order downloadedOrder = webClient.GetOrder(store, identifier.PurchaseOrderID);

                orderLoader.LoadItems(downloadedOrder.orderLines, order);
                orderRepository.Save(order);

                result = InternalUpdateShipmentDetails(store, order, shipment, identifier);
            }

            if (result.Success && result.Value != null)
            {
                try
                {
                    orderLoader.LoadItems(result.Value.orderLines, order);
                    orderRepository.Save(order);
                }
                catch (SqlException ex)
                {
                    return Result.FromError(ex);
                }
            }

            return result;
        }

        /// <summary>
        /// Should the upload be retried
        /// </summary>
        private bool ShouldRetry(Exception exception)
        {
            HttpStatusCode? httpStatusCode = ((exception.GetBaseException() as WebException)?.Response as HttpWebResponse)?.StatusCode;
            return httpStatusCode.HasValue && httpStatusCode.Value == HttpStatusCode.BadRequest;
        }

        /// <summary>
        /// Internals the update shipment details.
        /// </summary>
        /// <remarks>
        /// If no lines are shippable, does nothing.
        /// </remarks>
        private GenericResult<Order> InternalUpdateShipmentDetails(IWalmartStoreEntity store, IWalmartOrderEntity order, ShipmentEntity shipment, WalmartCombinedIdentifier identifier)
        {
            orderShipment orderShipment = CreateShipment(order, shipment, identifier.OriginalOrderID);
            if (orderShipment.orderLines.None())
            {
                return GenericResult.FromSuccess<Order>(null);
            }

            try
            {
                var downloadedOrder = webClient.UpdateShipmentDetails(store, orderShipment, identifier.PurchaseOrderID);
                return GenericResult.FromSuccess(downloadedOrder);
            }
            catch (SqlException ex)
            {
                return GenericResult.FromError<Order>(new WalmartException(ex.Message, ex));
            }
            catch (WalmartException ex)
            {
                return GenericResult.FromError<Order>(ex);
            }
        }

        /// <summary>
        /// Creates the Walmart shipment from the ShipWorks shipment entity
        /// </summary>
        private orderShipment CreateShipment(IWalmartOrderEntity order, IShipmentEntity shipment, long originalOrderID)
        {
            shippingMethodCodeType methodCode =
                (shippingMethodCodeType)
                    Enum.Parse(typeof(shippingMethodCodeType), order.RequestedShippingMethodCode, true);

            return new orderShipment
            {
                orderLines = order.OrderItems
                    .Where(x => x.OriginalOrderID == originalOrderID)
                    .OfType<IWalmartOrderItemEntity>()
                    .Where(IsLineShippable)
                    .Select(item => CreateShippingLineType(shipment, item, methodCode)).ToArray()
            };
        }

        /// <summary>
        /// Determines whether [is line shippable] [the specified item].
        /// </summary>
        private static bool IsLineShippable(IWalmartOrderItemEntity item) =>
            item.OnlineStatus == orderLineStatusValueType.Acknowledged.ToString();

        /// <summary>
        /// Create a new Shipping Line Type.
        /// </summary>
        private shippingLineType CreateShippingLineType(IShipmentEntity shipment, IWalmartOrderItemEntity item, shippingMethodCodeType methodCode)
        {
            return new shippingLineType
            {
                lineNumber = item.LineNumber,
                orderLineStatuses = new[] {
                    new shipLineStatusType
                    {
                        status = orderLineStatusValueType.Shipped,
                        statusQuantity = new quantityType()
                        {
                            amount = item.Quantity.ToString(CultureInfo.InvariantCulture)
                        },
                        trackingInfo = new trackingInfoType()
                        {
                            methodCode = methodCode,
                            shipDateTime = DateTime.UtcNow,
                            carrierName = GetCarrierName(shipment.ShipmentTypeCode),
                            trackingNumber = shipment.TrackingNumber
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Gets the name of the carrier used for the shipment
        /// </summary>
        private carrierNameType GetCarrierName(ShipmentTypeCode shipmentTypeCode)
        {
            carrierNameType carrierName = new carrierNameType();

            switch (shipmentTypeCode)
            {
                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.Usps:
                    carrierName.Item = carrierType.USPS;
                    break;
                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    carrierName.Item = carrierType.UPS;
                    break;
                case ShipmentTypeCode.FedEx:
                    carrierName.Item = carrierType.FedEx;
                    break;
                case ShipmentTypeCode.OnTrac:
                    carrierName.Item = carrierType.OnTrac;
                    break;
                default:
                    carrierName.Item = EnumHelper.GetDescription(shipmentTypeCode);
                    break;
            }

            return carrierName;
        }
    }
}