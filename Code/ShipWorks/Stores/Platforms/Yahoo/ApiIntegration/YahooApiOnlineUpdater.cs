using System;
using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration
{
    /// <summary>
    /// Uploads shipment details and order status to Yahoo
    /// </summary>
    [Component(RegistrationType.Self)]
    public class YahooApiOnlineUpdater
    {
        private readonly ILog log;
        private readonly IYahooApiWebClient client;
        private readonly IShippingManager shippingManager;

        private readonly List<string> acceptedCarrierNames = new List<string>()
        {
            "usps",
            "ups",
            "fedex",
            "dhl",
            "airborne"
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="YahooApiOnlineUpdater"/> class.
        /// </summary>
        public YahooApiOnlineUpdater(YahooStoreEntity store,
            Func<Type, ILog> createLog,
            Func<YahooStoreEntity, IYahooApiWebClient> createWebClient,
            IShippingManager shippingManager)
        {
            this.log = createLog(GetType());
            this.client = createWebClient(store);
            this.shippingManager = shippingManager;
        }

        /// <summary>
        /// Changes the status of a Yahoo order to that specified
        /// </summary>
        public void UpdateOrderStatus(long orderID, string status)
        {
            YahooOrderEntity order = (YahooOrderEntity) DataProvider.GetEntity(orderID);

            if (order != null)
            {
                if (order.IsManual)
                {
                    return;
                }

                client.UploadOrderStatus(order.YahooOrderID, status);

                SaveOrderStatus(orderID, status);
            }
            else
            {
                log.WarnFormat("Unable to update online status for order {0}: cannot find order", orderID);
            }
        }

        /// <summary>
        /// Updates the shipment details.
        /// </summary>
        /// <param name="orderKeys">The order keys.</param>
        public void UpdateShipmentDetails(IEnumerable<long> orderKeys)
        {
            foreach (long orderKey in orderKeys)
            {
                ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderKey);

                // Check to see if shipment exists
                if (shipment == null)
                {
                    log.InfoFormat("Not uploading order ID {0}: Has no items.", orderKey);
                    continue;
                }

                UpdateShipmentDetails(shipment);
            }
        }

        /// <summary>
        ///     Push the online status for an shipment.
        /// </summary>
        public void UpdateShipmentDetails(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            YahooOrderEntity order = (YahooOrderEntity) shipment.Order;

            if (!order.IsManual)
            {
                client.UploadShipmentDetails(order.OrderNumber.ToString(), shipment.TrackingNumber, GetCarrierCode(shipment));
            }

            string status = shipment.TrackingNumber.IsNullOrWhiteSpace() ? "Shipped" : "Tracked";

            SaveOrderStatus(order.OrderID, status);
        }

        /// <summary>
        /// Saves the order status.
        /// </summary>
        private void SaveOrderStatus(long orderID, string status)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();

            // Update the local database with the new status
            OrderEntity basePrototype = new OrderEntity(orderID)
            {
                IsNew = false,
                OnlineStatusCode = status,
                OnlineStatus = status
            };

            unitOfWork.AddForSave(basePrototype);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Gets the carrier code.
        /// </summary>
        public string GetCarrierCode(ShipmentEntity shipment)
        {
            shippingManager.EnsureShipmentLoaded(shipment);

            // Yahoo only supports usps, ups, fedex, dhl and airborne.
            // so if the carrier is something else (OnTrac, iparcel...)
            // just give an empty string and we just won't upload the carrier
            switch ((ShipmentTypeCode) shipment.ShipmentType)
            {
                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.Endicia:
                    return GetEndiciaCarrierCode(shipment);

                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.PostalWebTools:
                    return "usps";

                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    return GetUpsCarrierCode(shipment);

                case ShipmentTypeCode.FedEx:
                    return "fedex";

                case ShipmentTypeCode.iParcel:
                    return "";

                case ShipmentTypeCode.OnTrac:
                    return "";

                case ShipmentTypeCode.Other:
                    return GetOtherCarrierCode(shipment);

                default:
                    return "";
            }
        }

        /// <summary>
        /// Gets the carrier code for Endicia shipment
        /// </summary>
        private string GetEndiciaCarrierCode(ShipmentEntity shipment)
        {
            PostalServiceType service = (PostalServiceType) shipment.Postal.Service;
            return ShipmentTypeManager.IsDhl(service) ? "dhl" : "usps";
        }

        /// <summary>
        /// Gets the carrier code for Ups shipment
        /// </summary>
        private string GetUpsCarrierCode(ShipmentEntity shipment)
        {
            if (UpsUtility.IsUpsMiService((UpsServiceType) shipment.Ups.Service))
            {
                if (shipment.Ups.UspsTrackingNumber.Length > 0)
                {
                    return "usps";
                }
            }

            return "ups";
        }

        /// <summary>
        /// Gets the carrier code for the Other shipment
        /// </summary>
        private string GetOtherCarrierCode(ShipmentEntity shipment)
        {
            foreach (string acceptedCarrierName in acceptedCarrierNames)
            {
                if (shipment.Other.Carrier.ToLower().Contains(acceptedCarrierName))
                {
                    return acceptedCarrierName;
                }
            }

            return "";
        }
    }
}

