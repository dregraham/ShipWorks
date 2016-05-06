using System;
using System.Collections.Generic;
using ShipWorks.Stores.Platforms.MarketplaceAdvisor.AppDomainHelpers;
using ShipWorks.Stores.Platforms.MarketplaceAdvisor.WebServices.Oms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore;
using System.Diagnostics;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    /// <summary>
    /// Web client for connecting to MarketplaceAdvisor via OMS
    /// </summary>
    public class MarketplaceAdvisorOmsClient : MarshalByRefObject
    {
        private readonly string username;
        private readonly string password;
        private readonly MarketplaceAdvisorOmsFlagTypes downloadFlags;

        private readonly MarketplaceAdvisorLog log;

        const int downloadPageSize = 200;

        /// <summary>
        /// Create an instance of the client
        /// </summary>
        private MarketplaceAdvisorOmsClient(string username, string password, int downloadFlagsInt, MarketplaceAdvisorLog log)
        {
            this.username = username;
            this.password = password;
            this.log = log;
            this.downloadFlags = (MarketplaceAdvisorOmsFlagTypes)downloadFlagsInt;
        }

        /// <summary>
        /// Create an instance of the client on a separate app domain
        /// </summary>
        public static MarketplaceAdvisorOmsClient Create(MarketplaceAdvisorStoreEntity store)
        {
            return TlsAppDomain.Create<MarketplaceAdvisorOmsClient>(store.Username, store.Password, store.DownloadFlags,
                new MarketplaceAdvisorLog(typeof(MarketplaceAdvisorOmsClient)));
        }

        /// <summary>
        /// Create the MarketplaceAdvisor Web Service instance.
        /// </summary>
        private OMService CreateOMService(string requestName)
        {
            OMService service = new OMService(log.CreateApiLogger(requestName));

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
        private Credentials CreateCredentials()
        {
            Credentials mwCredentials = new Credentials
            {
                SellerUserName = username,
                SellerPassword = SecureText.Decrypt(password, username),
                APIUserName = "Interapptive",
                APIPassword = SecureText.Decrypt("GLWw3ReI0rJ9mP0LKD8SiQ==", "interapptive")
            };

            return mwCredentials;
        }

        /// <summary>
        /// Get the custom flags for the given store
        /// </summary>
        public OMFlags GetCustomFlags()
        {
            try
            {
                using (OMService service = CreateOMService("GetCustomFlags"))
                {
                    return service.GetCustomFlags(CreateCredentials());
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
        public OMOrders GetOrders(int currentPage)
        {
            // Create the query to download the appropriate orders
            OMOrderQuery query = new OMOrderQuery();
            query.DetailLevel = OrderQueryDetailEnum.AllDetailsLevel;
            query.Flags = MarketplaceAdvisorOmsFlagManager.BuildOMOrderFlags((MarketplaceAdvisorOmsFlagTypes) downloadFlags, MarketplaceAdvisorOmsFlagTypes.None);
            query.RecordsPerPage = downloadPageSize;
            query.PageNumber = currentPage;

            // Only retrieve orders not yet downloaded (unless magic keys is on)
            query.RetrievePendingOnly = !InterapptiveOnly.MagicKeysDown;

            try
            {
                using (OMService service = CreateOMService("GetOrders"))
                {
                    return service.GetOrders(CreateCredentials(), query);
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
        public void MarkOrdersProcessed(List<long> orderList)
        {
            try
            {
                using (OMService service = CreateOMService("MarkOrdersAsProcessed"))
                {
                    service.MarkOrdersAsProcessed(CreateCredentials(), orderList.ToArray(), true);
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
        public void ChangeOrderFlags(MarketplaceAdvisorOrderDto order, MarketplaceAdvisorOmsFlagTypes flagsOn, MarketplaceAdvisorOmsFlagTypes flagsOff)
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

                    service.UpdateOrders(CreateCredentials(), updateInfo);
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
        public void PromoteOrder(MarketplaceAdvisorOrderDto order)
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

                    service.UpdateOrders(CreateCredentials(), updateInfo);
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
        public void ChangeParcelFlags(MarketplaceAdvisorOrderDto order, MarketplaceAdvisorOmsFlagTypes flagsOn, MarketplaceAdvisorOmsFlagTypes flagsOff)
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

                    service.UpdateParcels(CreateCredentials(), updateInfo);
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
        public void PromoteParcel(MarketplaceAdvisorOrderDto order)
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

                    service.UpdateParcels(CreateCredentials(), updateInfo);
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
        public void UpdateShipmentStatus(MarketplaceAdvisorOrderDto order, MarketplaceAdvisorShipmentDto shipment)
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
                    updateInfo.ShippingCost = (double)shipment.ShipmentCost;

                    updateInfo.ShippingMethodCode = shipment.ShippingMethodCode;
                    updateInfo.IsValidateShippingMethodCode = false;

                    service.UpdateParcels(CreateCredentials(), updateInfo);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(MarketplaceAdvisorException));
            }
        }
    }
}
