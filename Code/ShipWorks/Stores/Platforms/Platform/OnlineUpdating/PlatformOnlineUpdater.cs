using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Stores.Content;
using ShipWorks.Warehouse.Orders;

namespace ShipWorks.Stores.Platforms.Platform.OnlineUpdating
{
    /// <summary>
    /// Uploads shipment details to Platform
    /// </summary>
    [Component]
    public class PlatformOnlineUpdater : IPlatformOnlineUpdater
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(PlatformOnlineUpdater));
        private readonly IOrderManager orderManager;
        private readonly IShippingManager shippingManager;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly Func<IWarehouseOrderClient> createWarehouseOrderClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public PlatformOnlineUpdater(IOrderManager orderManager, IShippingManager shippingManager,
            ISqlAdapterFactory sqlAdapterFactory, Func<IWarehouseOrderClient> createWarehouseOrderClient)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.createWarehouseOrderClient = createWarehouseOrderClient;
            this.shippingManager = shippingManager;
            this.orderManager = orderManager;
        }

        /// <summary>
        /// Update the online status of the given order
        /// </summary>
        public async Task UploadOrderShipmentDetails(IEnumerable<long> orderKeys)
        {
            List<ShipmentEntity> shipments = new List<ShipmentEntity>();

            var orders = await LoadOrders(orderKeys).ConfigureAwait(false);

            foreach (var order in orders)
            {
                // upload tracking number for the most recent processed, not voided shipment
                ShipmentEntity shipment = await orderManager.GetLatestActiveShipmentAsync(order.OrderID, false).ConfigureAwait(false);
                if (shipment == null)
                {
                    log.InfoFormat("There were no Processed and not Voided shipments to upload for OrderID {0}.", order.OrderID);
                }
                else
                {
                    shipment.Order = order;
                    shipments.Add(shipment);
                }
            }

            await UploadShipmentDetails(shipments).ConfigureAwait(false);
        }

        /// <summary>
        /// Load the orders for the given keys
        /// </summary>
        private async Task<IEnumerable<OrderEntity>> LoadOrders(IEnumerable<long> orderKeys)
        {
            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                return await orderManager.LoadOrdersAsync(orderKeys, sqlAdapter).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Uploads shipment details for a particular shipment
        /// </summary>
        public async Task UploadShipmentDetails(IEnumerable<long> shipmentKeys)
        {
            List<ShipmentEntity> shipments = new List<ShipmentEntity>();

            foreach (long shipmentID in shipmentKeys)
            {
                var shipmentAdapter = await shippingManager.GetShipmentAsync(shipmentID).ConfigureAwait(false);
                ShipmentEntity shipment = shipmentAdapter?.Shipment;

                if (shipment == null)
                {
                    log.InfoFormat("Not uploading shipment details, since the shipment {0} was deleted.", shipmentID);
                }
                else
                {
                    shipments.Add(shipment);
                }
            }

            await UploadShipmentDetails(shipments).ConfigureAwait(false);
        }

        /// <summary>
        /// Uploads shipment details for a particular shipment
        /// </summary>
        public async Task UploadShipmentDetails(List<ShipmentEntity> shipments)
        {
            var client = createWarehouseOrderClient();

            foreach (var shipment in shipments)
            {
                await shippingManager.EnsureShipmentLoadedAsync(shipment).ConfigureAwait(false);
                string carrier = GetCarrierName(shipment);
                var result = await client.NotifyShipped(shipment.Order.ChannelOrderID, shipment.TrackingNumber, carrier).ConfigureAwait(false);
                result.OnFailure(ex => throw new PlatformStoreException($"Error uploading shipment details: {ex.Message}", ex));
            }            
        }

        /// <summary>
        /// Gets the carrier name that is allowed in shipengine
        /// </summary>
        private string GetCarrierName(ShipmentEntity shipment)
        {
            CarrierDescription otherDesc = null;
            ShipmentTypeCode shipmentTypeCode = shipment.ShipmentTypeCode;

            if (shipmentTypeCode == ShipmentTypeCode.Other)
            {
                otherDesc = shippingManager.GetOtherCarrierDescription(shipment);
            }

            string sfpName = string.Empty;
            if(shipmentTypeCode == ShipmentTypeCode.AmazonSFP)
            {
                sfpName = shipment.AmazonSFP.CarrierName.ToUpperInvariant();
            }

            switch (true)
            {
                case true when otherDesc?.IsDHL ?? false:
                case true when ShipmentTypeManager.ShipmentTypeCodeSupportsDhl(shipmentTypeCode) &&
                        ShipmentTypeManager.IsDhl((PostalServiceType) shipment.Postal.Service):
                    return "dhl_ecommerce";

                case true when shipmentTypeCode == ShipmentTypeCode.Endicia:
                case true when shipmentTypeCode == ShipmentTypeCode.Express1Endicia:
                    return "endicia";

                case true when ShipmentTypeManager.IsPostal(shipmentTypeCode):
                case true when otherDesc?.IsUSPS ?? false:
                case true when sfpName.Equals("USPS", StringComparison.OrdinalIgnoreCase):
                case true when sfpName.Equals("STAMPS_DOT_COM", StringComparison.OrdinalIgnoreCase):
                    return "stamps_com";
                    
                case true when ShipmentTypeManager.IsFedEx(shipmentTypeCode):
                case true when otherDesc?.IsFedEx ?? false:
                case true when sfpName.Equals("FEDEX", StringComparison.OrdinalIgnoreCase):
                    return "fedex";
                    
                case true when ShipmentTypeManager.IsUps(shipmentTypeCode):
                case true when otherDesc?.IsUPS ?? false:
                case true when sfpName.Equals("UPS", StringComparison.OrdinalIgnoreCase):
                    return "ups";
                    
                case true when shipmentTypeCode == ShipmentTypeCode.DhlExpress:
                    return "dhl_express";
                    
                case true when shipmentTypeCode == ShipmentTypeCode.OnTrac:
                case true when sfpName.Equals("ONTRAC", StringComparison.OrdinalIgnoreCase):
                    return "ontrac";

                case true when shipmentTypeCode == ShipmentTypeCode.AmazonSWA:
                    return "amazon_shipping";

                case true when shipmentTypeCode == ShipmentTypeCode.Asendia:
                    return "asendia";

                default:
                    return "other";                
            }
        }
    }
}