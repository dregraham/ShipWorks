﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Yahoo.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration
{
    /// <summary>
    /// Uploads shipment details and order status to Yahoo
    /// </summary>
    [Component]
    public class YahooApiOnlineUpdater : IYahooApiOnlineUpdater
    {
        private readonly ILog log;
        private readonly IYahooApiWebClient webClient;
        private readonly IShippingManager shippingManager;
        private readonly IYahooCombineOrderSearchProvider orderNumberSearchProvider;

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
        public YahooApiOnlineUpdater(
            Func<Type, ILog> createLog,
            IYahooApiWebClient webClient,
            IShippingManager shippingManager,
            IYahooCombineOrderSearchProvider orderNumberSearchProvider)
        {
            this.orderNumberSearchProvider = orderNumberSearchProvider;
            this.webClient = webClient;
            this.log = createLog(GetType());
            this.shippingManager = shippingManager;
        }

        /// <summary>
        /// Changes the status of a Yahoo order to that specified
        /// </summary>
        public async Task UpdateOrderStatus(IYahooStoreEntity store, long orderID, string status)
        {
            YahooOrderEntity order = (YahooOrderEntity) DataProvider.GetEntity(orderID);

            if (order != null)
            {
                if (order.IsManual)
                {
                    return;
                }

                var identifiers = await orderNumberSearchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

                var handler = Result.Handle<YahooException>();
                identifiers.Select(x => handler.Execute(() => webClient.UploadOrderStatus(store, x, status)))
                    .ThrowFailures((msg, ex) => new YahooException(msg, ex));

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
        public async Task UpdateShipmentDetails(IYahooStoreEntity store, long orderKey)
        {
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderKey);

            // Check to see if shipment exists
            if (shipment == null)
            {
                log.InfoFormat("Not uploading order ID {0}: Has no items.", orderKey);
                return;
            }

            await UpdateShipmentDetails(store, shipment).ConfigureAwait(false);
        }

        /// <summary>
        ///     Push the online status for an shipment.
        /// </summary>
        public async Task UpdateShipmentDetails(IYahooStoreEntity store, ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            IYahooOrderEntity order = (IYahooOrderEntity) shipment.Order;

            if (!order.IsManual)
            {
                var identifiers = await orderNumberSearchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

                var resultHandler = Result.Handle<YahooException>();
                identifiers
                    .Select(x => resultHandler.Execute(() => webClient.UploadShipmentDetails(store, x, shipment.TrackingNumber, GetCarrierCode(shipment))))
                    .ToList()
                    .ThrowFailures((msg, ex) => new YahooException(msg, ex));
            }

            string status = shipment.TrackingNumber.IsNullOrWhiteSpace() ? "Shipped" : "Tracked";

            SaveOrderStatus(order.OrderID, status);
        }

        /// <summary>
        /// Saves the order status.
        /// </summary>
        private void SaveOrderStatus(long orderID, string status)
        {
            UnitOfWork2 unitOfWork = new OpeningUnitOfWork2();

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

