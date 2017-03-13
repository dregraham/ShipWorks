using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;
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
        private readonly IOrderManager orderManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="WalmartOnlineUpdater"/> class.
        /// </summary>
        /// <param name="orderManager"></param>
        public WalmartOnlineUpdater(IWalmartWebClient webClient, 
		            WalmartStoreEntity store,
					IOrderManager orderManager,
					IOrderRepository orderRepository)
        {
            this.webClient = webClient;
            this.store = store;
            this.orderManager = orderManager;
            this.orderRepository = orderRepository;
        }

        /// <summary>
        /// Upload carrier and tracking information for the given orders
        /// </summary>
        public void UpdateShipmentDetails(long orderKey)
        {
            ShipmentEntity shipment = orderManager.GetLatestActiveShipment(orderKey);

            // Check to see if shipment exists
            if (shipment != null)
            {
                UpdateShipmentDetails(shipment);
            }
        }

        /// <summary>
        /// Upload carrier and tracking information for the given shipment
        /// </summary>
        public void UpdateShipmentDetails(ShipmentEntity shipment)
        {
            if (!shipment.Order.IsManual)
            {
                orderShipment orderShipment = CreateShipment(shipment);
                string purchaseOrderID = ((WalmartOrderEntity) shipment.Order).PurchaseOrderID;

                webClient.UpdateShipmentDetails(store, orderShipment, purchaseOrderID);
            }
        }

        /// <summary>
        /// Creates the Walmart shipment from the ShipWorks shipment entity
        /// </summary>
        private orderShipment CreateShipment(ShipmentEntity shipment)
        {
            WalmartOrderEntity order = shipment.Order as WalmartOrderEntity;
            orderRepository.PopulateOrderDetails(order);

            shippingMethodCodeType methodCode =
                (shippingMethodCodeType)
                    Enum.Parse(typeof(shippingMethodCodeType), order.RequestedShippingMethodCode, true);

            orderShipment orderShipment = new orderShipment
            {
                orderLines = shipment.Order.OrderItems.Cast<WalmartOrderItemEntity>()
                    .Select(item => CreateShippingLineType(shipment, item, methodCode)).ToArray()
            };

            return orderShipment;
        }

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