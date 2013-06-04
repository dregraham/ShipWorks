using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.MarketplaceAdvisor.WebServices.Oms;
using Interapptive.Shared.Win32;
using ShipWorks.ApplicationCore.Logging;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using System.Net;
using System.Web.Services.Protocols;
using ShipWorks.ApplicationCore;
using log4net;
using System.Diagnostics;
using Interapptive.Shared.Net;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    /// <summary>
    /// Web client for connecting to MarketplaceAdvisor via OMS
    /// </summary>
    public static class MarketplaceAdvisorOmsClient
    {
        static readonly ILog log = LogManager.GetLogger(typeof(MarketplaceAdvisorOmsClient));

        const int downloadPageSize = 200;

        /// <summary>
        /// Create the MarketplaceAdvisor Web Service instance.
        /// </summary>
        private static OMService CreateOMService(string requestName)
        {
            OMService service = new OMService(new ApiLogEntry(ApiLogSource.MarketplaceAdvisor, requestName));

            // Set the timeout for 6 minutes
            service.Timeout = 360000;

            service.Url = UseLiveServer ?
                "https://ws.marketplaceadvisor.channeladvisor.com/nextgen/mwapi/current/omservice.asmx" :
                "https://sandbox.marketplaceadvisor.channeladvisor.com/nextgen/mwapi/current/omservice.asmx";

            return service;
        }

        /// <summary>
        /// Determines whether the live server is currently being used
        /// </summary>
        public static bool UseLiveServer
        {
            get
            {
                return InterapptiveOnly.Registry.GetValue("MarketplaceAdvisorLive", true);
            }
            set
            {
                InterapptiveOnly.Registry.SetValue("MarketplaceAdvisorLive", value);
            }
        }

        /// <summary>
        /// Create the MarketplaceAdvisor credentials object for the store.
        /// </summary>
        private static Credentials CreateCredentials(MarketplaceAdvisorStoreEntity store)
        {
            Credentials mwCredentials = new Credentials();
            mwCredentials.SellerUserName = store.Username;
            mwCredentials.SellerPassword = SecureText.Decrypt(store.Password, store.Username);
            mwCredentials.APIUserName = "Interapptive";
            mwCredentials.APIPassword = SecureText.Decrypt("GLWw3ReI0rJ9mP0LKD8SiQ==", "interapptive");

            return mwCredentials;
        }

        /// <summary>
        /// Get the custom flags for the given store
        /// </summary>
        public static OMFlags GetCustomFlags(MarketplaceAdvisorStoreEntity store)
        {
            try
            {
                using (OMService service = CreateOMService("GetCustomFlags"))
                {
                    return service.GetCustomFlags(CreateCredentials(store));
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(MarketplaceAdvisorException));
            }
        }

        /// <summary>
        /// Get the orders for the given store for the given page
        /// </summary>
        public static OMOrders GetOrders(MarketplaceAdvisorStoreEntity store, int currentPage)
        {
            // Create the query to download the appropriate orders
            OMOrderQuery query = new OMOrderQuery();
            query.DetailLevel = OrderQueryDetailEnum.AllDetailsLevel;
            query.Flags = MarketplaceAdvisorOmsFlagManager.BuildOMOrderFlags((MarketplaceAdvisorOmsFlagTypes) store.DownloadFlags, MarketplaceAdvisorOmsFlagTypes.None);
            query.RecordsPerPage = downloadPageSize;
            query.PageNumber = currentPage;

            // Only retrieve orders not yet downloaded (unless magic keys is on)
            query.RetrievePendingOnly = !InterapptiveOnly.MagicKeysDown;

            try
            {
                using (OMService service = CreateOMService("GetOrders"))
                {
                    return service.GetOrders(CreateCredentials(store), query);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(MarketplaceAdvisorException));
            }
        }

        /// <summary>
        /// Mark the given list of MarketplaceAdvisor orders as processed
        /// </summary>
        public static void MarkOrdersProcessed(MarketplaceAdvisorStoreEntity store, List<long> orderList)
        {
            try
            {
                using (OMService service = CreateOMService("MarkOrdersAsProcessed"))
                {
                    service.MarkOrdersAsProcessed(CreateCredentials(store), orderList.ToArray(), true);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(MarketplaceAdvisorException));
            }
        }

        /// <summary>
        /// Update the online flags for the given order.
        /// </summary>
        public static void ChangeOrderFlags(MarketplaceAdvisorStoreEntity store, MarketplaceAdvisorOrderEntity order, MarketplaceAdvisorOmsFlagTypes flagsOn, MarketplaceAdvisorOmsFlagTypes flagsOff)
        {
            if (order.IsManual)
            {
                log.InfoFormat("Not updating order {0} online since its manual.", order.OrderID);
                return;
            }

            try
            {
                using (OMService service = CreateOMService("ChangeOrderFlags"))
                {
                    OMUpdateOrderInfo updateInfo = new OMUpdateOrderInfo();
                    updateInfo.OrderUid = order.OrderNumber;

                    updateInfo.Notification = FlagState.Ignore;
                    updateInfo.PromoteOrder = OrderPromoteEnum.Ignore;
                    updateInfo.IsAPIProcessed = FlagState.Ignore;
                    updateInfo.OrderCancelledFlag = FlagState.Ignore;
                    updateInfo.PaymentClearedFlag = FlagState.Ignore;

                    OMOrderFlags orderFlags = MarketplaceAdvisorOmsFlagManager.BuildOMOrderFlags(flagsOn, flagsOff);
                    updateInfo.CustomFlags = orderFlags.FlagStatuses;

                    service.UpdateOrders(CreateCredentials(store), updateInfo);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(MarketplaceAdvisorException));
            }
        }

        /// <summary>
        /// Promote the given order to the next step in teh MarketplaceAdvisor workflow
        /// </summary>
        public static void PromoteOrder(MarketplaceAdvisorStoreEntity store, MarketplaceAdvisorOrderEntity order)
        {
            if (order.IsManual)
            {
                log.InfoFormat("Not updating order {0} online since its manual.", order.OrderID);
                return;
            }

            try
            {
                using (OMService service = CreateOMService("PromoteOrder"))
                {
                    OMUpdateOrderInfo updateInfo = new OMUpdateOrderInfo();
                    updateInfo.OrderUid = order.OrderNumber;

                    updateInfo.Notification = FlagState.Ignore;
                    updateInfo.IsAPIProcessed = FlagState.Ignore;
                    updateInfo.OrderCancelledFlag = FlagState.Ignore;
                    updateInfo.PaymentClearedFlag = FlagState.Ignore;

                    updateInfo.PromoteOrder = OrderPromoteEnum.PromoteOneStep;

                    service.UpdateOrders(CreateCredentials(store), updateInfo);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(MarketplaceAdvisorException));
            }
        }

        /// <summary>
        /// Update the online flags for the given parcel.
        /// </summary>
        public static void ChangeParcelFlags(MarketplaceAdvisorStoreEntity store, MarketplaceAdvisorOrderEntity order, MarketplaceAdvisorOmsFlagTypes flagsOn, MarketplaceAdvisorOmsFlagTypes flagsOff)
        {
            if (order.IsManual)
            {
                log.InfoFormat("Not updating order {0} online since its manual.", order.OrderID);
                return;
            }

            try
            {
                using (OMService service = CreateOMService("ChangeParcelFlags"))
                {
                    OMUpdateParcelInfo updateInfo = new OMUpdateParcelInfo();
                    updateInfo.OrderUid = order.OrderNumber;
                    updateInfo.ParcelUid = order.ParcelID;
                    updateInfo.IsInsured = FlagState.Ignore;
                    updateInfo.PromoteParcel = OrderPromoteEnum.Ignore;

                    updateInfo.Flags = MarketplaceAdvisorOmsFlagManager.BuildOMOrderFlags(flagsOn, flagsOff).FlagStatuses;

                    service.UpdateParcels(CreateCredentials(store), updateInfo);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(MarketplaceAdvisorException));
            }
        }

        /// <summary>
        /// Promote the given parcel to the next phase in the MarketplaceAdvisor workflow
        /// </summary>
        public static void PromoteParcel(MarketplaceAdvisorStoreEntity store, MarketplaceAdvisorOrderEntity order)
        {
            if (order.IsManual)
            {
                log.InfoFormat("Not updating order {0} online since its manual.", order.OrderID);
                return;
            }

            try
            {
                using (OMService service = CreateOMService("PromoteParcel"))
                {
                    OMUpdateParcelInfo updateInfo = new OMUpdateParcelInfo();
                    updateInfo.OrderUid = order.OrderNumber;
                    updateInfo.ParcelUid = order.ParcelID;
                    updateInfo.IsInsured = FlagState.Ignore;

                    updateInfo.PromoteParcel = OrderPromoteEnum.PromoteOneStep;

                    service.UpdateParcels(CreateCredentials(store), updateInfo);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(MarketplaceAdvisorException));
            }
        }

        /// <summary>
        /// Update the shipment status of the given order\parcel for the specified shipment
        /// </summary>
        public static void UpdateShipmentStatus(MarketplaceAdvisorStoreEntity store, MarketplaceAdvisorOrderEntity order, ShipmentEntity shipment)
        {
            // This stuff is already looked at in the OnlineUpdater class
            Debug.Assert(!order.IsManual && shipment.Processed && !shipment.Voided);

            try
            {
                using (OMService service = CreateOMService("UpdateShipmentStatus"))
                {
                    OMUpdateParcelInfo updateInfo = new OMUpdateParcelInfo();
                    updateInfo.OrderUid = order.OrderNumber;
                    updateInfo.ParcelUid = order.ParcelID;

                    updateInfo.TrackingNumber = shipment.TrackingNumber;
                    updateInfo.ShippingCost = (double) shipment.ShipmentCost;

                    updateInfo.ShippingMethodCode = MarketplaceAdvisorUtility.GetShippingMethodID(shipment);
                    updateInfo.IsValidateShippingMethodCode = false;

                    service.UpdateParcels(CreateCredentials(store), updateInfo);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(MarketplaceAdvisorException));
            }
        }
    }
}
