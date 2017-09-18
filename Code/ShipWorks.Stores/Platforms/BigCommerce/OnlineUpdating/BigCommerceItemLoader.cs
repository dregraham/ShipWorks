using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.BigCommerce.DTO;

namespace ShipWorks.Stores.Platforms.BigCommerce.OnlineUpdating
{
    /// <summary>
    /// Item loader for BigCommerce online updater
    /// </summary>
    [Component]
    public class BigCommerceItemLoader : IBigCommerceItemLoader
    {
        // Valid list of providers from https://developer.bigcommerce.com/api/stores/v2/orders/shipments#create-a-shipment
        private static readonly Dictionary<ShipmentTypeCode, string> validProviders = new Dictionary<ShipmentTypeCode, string>
        {
            { ShipmentTypeCode.Endicia, "endicia" },
            { ShipmentTypeCode.Express1Endicia, "endicia" },
            { ShipmentTypeCode.Usps, "usps" },
            { ShipmentTypeCode.Express1Usps, "usps" },
            { ShipmentTypeCode.FedEx, "fedex" },
            { ShipmentTypeCode.UpsOnLineTools, "ups" },
            { ShipmentTypeCode.UpsWorldShip, "ups" },
        };

        private readonly ILog log;
        private readonly IBigCommerceDataAccess dataAccess;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="createLogger"></param>
        public BigCommerceItemLoader(IBigCommerceDataAccess dataAccess, Func<Type, ILog> createLogger)
        {
            this.dataAccess = dataAccess;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Load items
        /// </summary>
        public async Task<GenericResult<BigCommerceOnlineItems>> LoadItems(
            IEnumerable<IOrderItemEntity> orderItems,
            string orderNumberComplete,
            long orderNumber,
            IBigCommerceWebClient webClient)
        {
            IEnumerable<BigCommerceProduct> orderProducts = null;
            long bigCommerceOrderAddressId = BigCommerceConstants.InvalidOrderAddressID;

            // If the order item doesn't have a valid OrderAddressID, we know it's a legacy order.  Set a flag noting this.
            // Note: Digital items have an order address id of 0 when downloaded
            bool hasBigCommerceRequiredShippingFields = orderItems.OfType<IBigCommerceOrderItemEntity>().Any(oi => oi.OrderAddressID > 0);

            // If this is a legacy order AND it is a multi-ship to order, we can't determine which items go to which order.
            // So we'll skip uploading shipment info, and inform the user to use the website to finish.
            if (!hasBigCommerceRequiredShippingFields && orderNumberComplete.IndexOf("-", 0, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return GenericResult.FromError<BigCommerceOnlineItems>($"Order number {orderNumberComplete} was downloaded prior to the new BigCommerce API upgrade.  Uploading shipment information is not supported for orders with multiple ship-to addresses that were downloaded before the upgrade. Please enter shipping information using the BigCommerce admin website.");
            }

            if (!hasBigCommerceRequiredShippingFields)
            {
                // We don't have a valid order item address id, so ask the cart for it
                // Note: Digital items have an order address id of 0 when downloaded
                orderProducts = await webClient.GetOrderProducts(orderNumber).ConfigureAwait(false);
                bigCommerceOrderAddressId = orderProducts?.FirstOrDefault(op => op.order_address_id > 0)?.order_address_id ?? BigCommerceConstants.InvalidOrderAddressID;
            }
            else
            {
                // This is an order downloaded via the API, we should be good to go with the downloaded data.
                IBigCommerceOrderItemEntity orderItem = orderItems.OfType<IBigCommerceOrderItemEntity>().FirstOrDefault(oi => oi.OrderAddressID > 0);
                bigCommerceOrderAddressId = orderItem.OrderAddressID;
            }

            // If we couldn't find an order address id, the order is not supposed to be shipped, so just return
            if (bigCommerceOrderAddressId == BigCommerceConstants.InvalidOrderAddressID)
            {
                return GenericResult.FromError<BigCommerceOnlineItems>($"Not processing shipment for order {orderNumberComplete} since it has no BigCommerce shipping order address.  The order probably has only digital items.");
            }

            var items = GetOrderItems(orderItems, orderProducts);
            return GenericResult.FromSuccess(new BigCommerceOnlineItems(bigCommerceOrderAddressId, items));
        }

        /// <summary>
        /// Gets a list of BigCommerceItems for the order's items
        /// </summary>
        /// <param name="orderEntity">The order from which to populate the list</param>
        /// <param name="orderProducts">List of BigCommerceProducts.  If populated, will be used for creating the list of BigCommerceItems.  Otherwise, order.OrderItems will be used.</param>
        /// <returns>List of BigCommerceItems for the order's items</returns>
        private static IEnumerable<BigCommerceItem> GetOrderItems(IEnumerable<IOrderItemEntity> orderItems, IEnumerable<BigCommerceProduct> orderProducts)
        {
            // To support legacy order items that wouldn't have order address id or order product id, we may need to
            // use the list of order products received from BigCommerce.  If the passed orderProducts is null, we use
            // order.OrderItems, otherwise we use orderProducts.  This assumes the check has already been done and the
            // appropriate orderProducts has been passed.
            return orderProducts == null ?
                orderItems.OfType<IBigCommerceOrderItemEntity>()
                    .Select(x => new BigCommerceItem { order_product_id = (int) x.OrderProductID, quantity = (int) x.Quantity }) :
                orderProducts.Select(x => new BigCommerceItem { order_product_id = x.id, quantity = x.quantity });
        }

        /// <summary>
        /// Gets the shipping method.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns></returns>
        public Tuple<string, string> GetShippingMethod(ShipmentEntity shipment)
        {
            ShipmentTypeCode shipmentType = shipment.ShipmentTypeCode;

            string carrier = validProviders.ContainsKey(shipmentType) ? validProviders[shipmentType] : string.Empty;
            string service = GetShippingService(shipment, carrier);

            return Tuple.Create(carrier, service);
        }

        /// <summary>
        /// Get the service used for the shipment
        /// </summary>
        private string GetShippingService(ShipmentEntity shipment, string carrier)
        {
            string service = dataAccess.GetOverriddenServiceUsed(shipment) ?? string.Empty;

            // If the service starts with the carrier name, cut the carrier name off
            if (!string.IsNullOrEmpty(carrier) && service.ToLower().StartsWith(carrier))
            {
                service = service.Substring(carrier.Length + 1);
            }

            // BigCommerce doesn't like it when you set shipping_method to an empty string
            return string.IsNullOrWhiteSpace(service) ? "other" : service;
        }
    }
}
