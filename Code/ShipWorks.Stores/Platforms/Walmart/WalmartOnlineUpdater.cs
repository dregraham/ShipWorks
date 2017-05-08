using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Walmart.DTO;

namespace ShipWorks.Stores.Platforms.Walmart
{
    /// <summary>
    /// Online updater for Walmart
    /// </summary>
    [Component(RegistrationType.Self)]
    public class WalmartOnlineUpdater
    {
        private readonly IWalmartWebClient webClient;
        private readonly WalmartStoreEntity store;
        private readonly IOrderRepository orderRepository;
        private readonly IWalmartOrderLoader orderLoader;
        private readonly IOrderManager orderManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="WalmartOnlineUpdater"/> class.
        /// </summary>
        public WalmartOnlineUpdater(IWalmartWebClient webClient, 
		            WalmartStoreEntity store,
					IOrderManager orderManager,
					IOrderRepository orderRepository,
                    IWalmartOrderLoader orderLoader)
        {
            this.webClient = webClient;
            this.store = store;
            this.orderManager = orderManager;
            this.orderRepository = orderRepository;
            this.orderLoader = orderLoader;
        }

        /// <summary>
        /// Upload carrier and tracking information for the given orders
        /// </summary>
        public void UpdateShipmentDetails(long orderKey)
        {
            ShipmentEntity shipment = orderManager.GetLatestActiveShipment(orderKey);

            // Check to see if shipment exists and order has shippable line item
            if (shipment != null)
            {
                UpdateShipmentDetails(shipment);
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
        public void UpdateShipmentDetails(ShipmentEntity shipment)
        {
            orderRepository.PopulateOrderDetails(shipment.Order);

            if (!shipment.Order.IsManual)
            {
                string purchaseOrderID = ((WalmartOrderEntity) shipment.Order).PurchaseOrderID;

                try
                {
                    InternalUpdateShipmentDetails(shipment, purchaseOrderID);
                }
                catch (WalmartException e) 
                {
                    HttpStatusCode? httpStatusCode = ((e.InnerException as WebException)?.Response as HttpWebResponse)?.StatusCode;
                    if (!httpStatusCode.HasValue || httpStatusCode.Value != HttpStatusCode.BadRequest)
                    {
                        throw;
                    }

                    Order order = webClient.GetOrder(store, purchaseOrderID);

                    orderLoader.LoadOrder(order, (WalmartOrderEntity) shipment.Order);
                    orderRepository.Save(shipment.Order);
                    
                    InternalUpdateShipmentDetails(shipment, purchaseOrderID);
                }
            }
        }

        /// <summary>
        /// Internals the update shipment details.
        /// </summary>
        /// <remarks>
        /// If no lines are shippable, does nothing.
        /// </remarks>
        private void InternalUpdateShipmentDetails(ShipmentEntity shipment, string purchaseOrderID)
        {
            try
            {
                orderShipment orderShipment = CreateShipment(shipment);
                if (orderShipment.orderLines.Length > 0)
                {
                    Order updatedOrder = webClient.UpdateShipmentDetails(store, orderShipment, purchaseOrderID);
                    orderLoader.LoadOrder(updatedOrder, (WalmartOrderEntity) shipment.Order);
                    orderRepository.Save(shipment.Order);
                }
            }
            catch (SqlException ex)
            {
                throw new WalmartException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Creates the Walmart shipment from the ShipWorks shipment entity
        /// </summary>
        private orderShipment CreateShipment(ShipmentEntity shipment)
        {
            WalmartOrderEntity order = shipment.Order as WalmartOrderEntity;
            
            shippingMethodCodeType methodCode =
                (shippingMethodCodeType)
                    Enum.Parse(typeof(shippingMethodCodeType), order.RequestedShippingMethodCode, true);

            orderShipment orderShipment = new orderShipment
            {
                orderLines = shipment.Order.OrderItems.Cast<WalmartOrderItemEntity>()
                    .Where(IsLineShippable)
                    .Select(item => CreateShippingLineType(shipment, item, methodCode)).ToArray()
            };

            return orderShipment;
        }

        /// <summary>
        /// Determines whether [is line shippable] [the specified item].
        /// </summary>
        private static bool IsLineShippable(WalmartOrderItemEntity item) => 
            item.OnlineStatus == orderLineStatusValueType.Acknowledged.ToString();

        /// <summary>
        /// Create a new Shipping Line Type.
        /// </summary>
        private shippingLineType CreateShippingLineType(ShipmentEntity shipment, WalmartOrderItemEntity item, shippingMethodCodeType methodCode)
        {
            return new shippingLineType
            {
                lineNumber = item.LineNumber,
                orderLineStatuses = new[]
                                    {
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
                                    carrierName = GetCarrierName(shipment),
                                    trackingNumber = shipment.TrackingNumber
                                }
                            }
                        }
            };
        }

        /// <summary>
        /// Gets the name of the carrier used for the shipment
        /// </summary>
        private carrierNameType GetCarrierName(ShipmentEntity shipment)
        {
            ShipmentTypeCode shipmentTypeCode = (ShipmentTypeCode)shipment.ShipmentType;
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