using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal;
using log4net;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Platforms.BuyDotCom.Fulfillment;
using ShipWorks.Stores.Content;
using ShipWorks.Data;
using ShipWorks.Data.Model;

namespace ShipWorks.Stores.Platforms.BuyDotCom
{
    /// <summary>
    /// Uploads tracking information on Buy.com
    /// </summary>
    public class BuyDotComOnlineUpdater
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(BuyDotComOnlineUpdater));

        BuyDotComStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public BuyDotComOnlineUpdater(BuyDotComStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Upload ship confirmation to buy.com for order numbers
        /// </summary>
        internal void UploadOrderShipmentDetails(IEnumerable<long> orderKeys)
        {
            List<ShipmentEntity> shipments = new List<ShipmentEntity>();

            foreach (long orderID in orderKeys)
            {
                // upload tracking number for the most recent processed, not voided shipment
                ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderID);
                if (shipment == null)
                {
                    log.InfoFormat("There were no Processed and not Voided shipments to upload for OrderID {0}.", orderID);
                }
                else
                {
                    shipments.Add(shipment);
                }
            }

            UploadShipmentDetails(shipments);
        }

        /// <summary>
        /// Upload ship confirmation to buy.com for shipment IDs
        /// </summary>
        internal void UploadShipmentDetails(IEnumerable<long> shipmentKeys)
        {
            List<ShipmentEntity> shipments = new List<ShipmentEntity>();

            foreach (long shipmentID in shipmentKeys)
            {
                ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
                if (shipment == null)
                {
                    log.InfoFormat(String.Format("Not uploading shipment details, since the shipment {0} was deleted.", shipmentID));
                }
                else
                {
                    shipments.Add(shipment);
                }
            }

            UploadShipmentDetails(shipments);
        }

        /// <summary>
        /// Uploads shipment details for the given shipments
        /// </summary>
        private void UploadShipmentDetails(List<ShipmentEntity> shipments)
        {
            if (shipments == null)
            {
                throw new ArgumentNullException("shipments");
            }

            List<BuyDotComShipConfirmation> confirmations = new List<BuyDotComShipConfirmation>();

            foreach (var shipment in shipments)
            {
                if (shipment.Order.IsManual)
                {
                    log.InfoFormat("Not uploading tracking number since order {0} is manual.", shipment.Order.OrderID);
                    continue;
                }

                try
                {
                    ShippingManager.EnsureShipmentLoaded(shipment);
                }
                catch (ObjectDeletedException)
                {
                    log.InfoFormat("Not uploading tracking number since shipment {0} or related information has been deleted.", shipment.ShipmentID);
                    continue;
                }
                catch (SqlForeignKeyException)
                {
                    log.InfoFormat("Not uploading tracking number since shipment {0} or related information has been deleted.", shipment.ShipmentID);
                    continue;
                }

                // Create the order level confirmation
                BuyDotComShipConfirmation confirmation =
                    new BuyDotComShipConfirmation()
                    {
                        ReceiptID = shipment.Order.OrderNumber,
                        ShipDate = shipment.ShipDate,
                        TrackingNumber = shipment.TrackingNumber,
                        TrackingType = GetTrackingType(shipment)
                    };
                confirmations.Add(confirmation);

                var orderItems = DataProvider.GetRelatedEntities(shipment.OrderID, EntityType.OrderItemEntity);

                // Add in each line
                foreach (BuyDotComOrderItemEntity item in orderItems.OfType<BuyDotComOrderItemEntity>())
                {
                    confirmation.OrderLines.Add(
                        new BuyDotComShipConfirmationLine()
                        {
                            Quantity = item.Quantity,
                            ReceiptItemID = item.ReceiptItemID
                        });
                }
            }

            // Upload the confirmation details
            using (BuyDotComFtpClient ftpClient = new BuyDotComFtpClient(store))
            {
                ftpClient.UploadShipConfirmation(confirmations);
            }
        }

        /// <summary>
        /// translates local shipment code to buy.com "TrackingType"
        /// </summary>
        public BuyDotComTrackingType GetTrackingType(ShipmentEntity shipment)
        {
            ShipmentTypeCode type = (ShipmentTypeCode) shipment.ShipmentType;

            // Other codes we could support:
            /*   4 = DHL
                 6 = UPS-MI
                 7 = FedEx SmartPost
                 8 = DHL Global Mail */

            switch (type)
            {
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.PostalWebTools:
                    return BuyDotComTrackingType.Usps;

                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.Endicia:
                    return GetEndiciaTrackingType(shipment);

                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    if (shipment.Ups != null && UpsUtility.IsUpsMiService((UpsServiceType) shipment.Ups.Service))
                    {
                        return BuyDotComTrackingType.UPSMI;
                    }

                    return BuyDotComTrackingType.Ups;

                case ShipmentTypeCode.FedEx:
                    return BuyDotComTrackingType.FedEx;

                case ShipmentTypeCode.Other:
                    return GetOtherTrackingType(shipment);

                default:
                    return BuyDotComTrackingType.Other;
            }
        }

        /// <summary>
        /// A helper method to get the tracking type for a shipment processed with the "Other" shipment type.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>The BuyDotComTrackingType value.</returns>
        private static BuyDotComTrackingType GetOtherTrackingType(ShipmentEntity shipment)
        {
            // Get the carrier name based on the free text the user entered
            CarrierDescription description = ShippingManager.GetOtherCarrierDescription(shipment);

            // See if the parsed free text maps to any Buy.com carrier
            return description.IsUPS ? BuyDotComTrackingType.Ups : 
                description.IsFedEx ? BuyDotComTrackingType.FedEx : 
                description.IsUSPS ? BuyDotComTrackingType.Usps : 
                description.IsDHL ? BuyDotComTrackingType.DHLGlobalMail : 
                BuyDotComTrackingType.Other;
        }

        /// <summary>
        /// A helper method to get the tracking type for a shipment processed with the Endicia shipment type.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>The BuyDotComTrackingType value.</returns>
        private static BuyDotComTrackingType GetEndiciaTrackingType(ShipmentEntity shipment)
        {
            PostalServiceType service = (PostalServiceType) shipment.Postal.Service;

            // The shipment is an Endicia shipment, check to see if it's DHL
            if (ShipmentTypeManager.IsDhl(service))
            {
                // The DHL carrier for Endicia is:
                return BuyDotComTrackingType.DHLGlobalMail;
            }
            
            if (ShipmentTypeManager.IsConsolidator(service))
            {
                return BuyDotComTrackingType.Other;
            }
            
            // Use the default carrier for other Endicia types
            return BuyDotComTrackingType.Usps;
        }
    }
}