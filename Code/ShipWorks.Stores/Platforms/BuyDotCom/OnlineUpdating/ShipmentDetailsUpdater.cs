using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Platforms.BuyDotCom.Fulfillment;

namespace ShipWorks.Stores.Platforms.BuyDotCom.OnlineUpdating
{
    /// <summary>
    /// Uploads tracking information on Buy.com
    /// </summary>
    [Component]
    public class ShipmentDetailsUpdater : IShipmentDetailsUpdater
    {
        private readonly ILog log;
        private readonly IFtpClientFactory ftpClientFactory;
        private readonly IDataAccess dataAccess;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentDetailsUpdater(IFtpClientFactory ftpClientFactory, IDataAccess dataAccess, Func<Type, ILog> createLogger)
        {
            this.dataAccess = dataAccess;
            this.ftpClientFactory = ftpClientFactory;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Upload ship confirmation to buy.com for shipment IDs
        /// </summary>
        public async Task UploadShipmentDetails(IBuyDotComStoreEntity store, IEnumerable<long> shipmentKeys)
        {
            var shipmentData = await dataAccess.GetShipmentDataAsync(shipmentKeys).ConfigureAwait(false);
            await UploadShipmentDetails(store, shipmentData).ConfigureAwait(false);
        }

        /// <summary>
        /// Uploads shipment details for the given shipments
        /// </summary>
        private async Task UploadShipmentDetails(IBuyDotComStoreEntity store, IEnumerable<ShipmentUpload> shipmentData)
        {
            List<BuyDotComShipConfirmation> confirmations = new List<BuyDotComShipConfirmation>();

            foreach (var uploadData in shipmentData.SelectMany(x => x.Orders.Select(o => new { Shipment = x.Shipment, Order = o })))
            {
                if (uploadData.Order.IsManual)
                {
                    log.InfoFormat("Not uploading tracking number since order {0} is manual.", uploadData.Order.OrderNumber);
                    continue;
                }

                // Create the order level confirmation
                BuyDotComShipConfirmation confirmation =
                    new BuyDotComShipConfirmation()
                    {
                        ReceiptID = uploadData.Order.OrderNumber,
                        ShipDate = uploadData.Shipment.ShipDate,
                        TrackingNumber = uploadData.Shipment.TrackingNumber,
                        TrackingType = GetTrackingType(uploadData.Shipment)
                    };
                confirmations.Add(confirmation);

                // Add in each line
                foreach (var item in uploadData.Order.Items)
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
            using (IFtpClient ftpClient = await ftpClientFactory.LoginAsync(store).ConfigureAwait(false))
            {
                await ftpClient.UploadShipConfirmation(confirmations).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// translates local shipment code to buy.com "TrackingType"
        /// </summary>
        public BuyDotComTrackingType GetTrackingType(IShipmentEntity shipment)
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
                    return GetPostalTrackingType(shipment);

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
        private static BuyDotComTrackingType GetOtherTrackingType(IShipmentEntity shipment)
        {
            // Get the carrier name based on the free text the user entered
            CarrierDescription description = new CarrierDescription(shipment.Other.Carrier);

            // See if the parsed free text maps to any Buy.com carrier
            return description.IsUPS ? BuyDotComTrackingType.Ups :
                description.IsFedEx ? BuyDotComTrackingType.FedEx :
                description.IsUSPS ? BuyDotComTrackingType.Usps :
                description.IsDHL ? BuyDotComTrackingType.DHLGlobalMail :
                BuyDotComTrackingType.Other;
        }

        /// <summary>
        /// A helper method to get the tracking type for a shipment processed with the postal shipment type.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>The BuyDotComTrackingType value.</returns>
        private static BuyDotComTrackingType GetPostalTrackingType(IShipmentEntity shipment)
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
