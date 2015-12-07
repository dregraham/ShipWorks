using System;
using System.Collections.Generic;
using Interapptive.Shared;
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
    public class YahooApiOnlineUpdater
    {
        private readonly ILog log;
        private readonly IYahooApiWebClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="YahooApiOnlineUpdater"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        public YahooApiOnlineUpdater(YahooStoreEntity store) :
            this(LogManager.GetLogger(typeof (YahooApiOnlineUpdater)), new YahooApiWebClient(store))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YahooApiOnlineUpdater"/> class.
        /// </summary>
        /// <param name="log">The logger</param>
        /// <param name="client">The api web client.</param>
        public YahooApiOnlineUpdater(ILog log, IYahooApiWebClient client)
        {
            this.log = log;
            this.client = client;
        }

        /// <summary>
        /// Changes the status of a Yahoo order to that specified
        /// </summary>
        public void UpdateOrderStatus(long orderID, string status)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();
            UpdateOrderStatus(orderID, status, unitOfWork);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Changes the status of a Yahoo order to that specified
        /// </summary>
        private void UpdateOrderStatus(long orderID, string status, UnitOfWork2 unitOfWork)
        {
            YahooOrderEntity order = (YahooOrderEntity)DataProvider.GetEntity(orderID);
            if (order != null)
            {
                if (order.IsManual)
                {
                    return;
                }

                client.UploadOrderStatus(order.YahooOrderID, status);

                // Update the local database with the new status
                OrderEntity basePrototype = new OrderEntity(orderID)
                {
                    IsNew = false,
                    OnlineStatusCode = status,
                    OnlineStatus = status
                };

                unitOfWork.AddForSave(basePrototype);
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

                YahooOrderEntity order = (YahooOrderEntity)shipment.Order;
                if (!order.IsManual)
                {
                    client.UploadShipmentDetails(order.OrderNumber.ToString(), shipment.TrackingNumber, GetCarrierCode(shipment),
                        order.OnlineStatus);
                }
            }
        }

        /// <summary>
        ///     Push the online status for an shipment.
        /// </summary>
        public void UpdateShipmentDetails(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            YahooOrderEntity order = (YahooOrderEntity)shipment.Order;

            if (!order.IsManual)
            {
                client.UploadShipmentDetails(order.OrderNumber.ToString(), shipment.TrackingNumber, GetCarrierCode(shipment),
                        order.OnlineStatus);
            }
        }

        /// <summary>
        /// Gets the carrier code.
        /// </summary>
        /// <param name="shipment">The shipment entity.</param>
        /// <returns></returns>
        [NDependIgnoreComplexMethod]
        public string GetCarrierCode(ShipmentEntity shipment)
        {
            // Yahoo only supports usps, ups, fedex, dhl and airborne.
            // so if the carrier is something else (OnTrac, iparcel...)
            // just give an empty string and we just won't upload the carrier
            switch ((ShipmentTypeCode) shipment.ShipmentType)
            {
                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.Endicia:
                    PostalServiceType service = (PostalServiceType)shipment.Postal.Service;
                    return ShipmentTypeManager.IsDhl(service) ? "dhl" : "usps";

                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.PostalWebTools:
                    return "usps";

                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    if (UpsUtility.IsUpsMiService((UpsServiceType)shipment.Ups.Service))
                    {
                        if (shipment.Ups.UspsTrackingNumber.Length > 0)
                        {
                            return "usps";
                        }
                    }

                    return "ups";

                case ShipmentTypeCode.FedEx:
                    return "fedex";

                case ShipmentTypeCode.iParcel:
                    return "";

                case ShipmentTypeCode.OnTrac:
                    return "";

                case ShipmentTypeCode.Other:
                    if (shipment.Other.Carrier.ToLower().Contains("usps"))
                    {
                        return "usps";
                    }

                    if (shipment.Other.Carrier.ToLower().Contains("ups"))
                    {
                        return "ups";
                    }

                    if (shipment.Other.Carrier.ToLower().Contains("fedex"))
                    {
                        return "fedex";
                    }

                    if (shipment.Other.Carrier.ToLower().Contains("dhl"))
                    {
                        return "dhl";
                    }

                    if (shipment.Other.Carrier.ToLower().Contains("airborne"))
                    {
                        return "airborne";
                    }

                    return "";

                default:
                    return "";
            }
        }
    }
}

