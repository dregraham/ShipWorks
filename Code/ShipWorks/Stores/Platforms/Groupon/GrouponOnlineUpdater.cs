using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Groupon.DTO;

namespace ShipWorks.Stores.Platforms.Groupon
{
    /// <summary>
    /// Uploads shipment details to Groupon
    /// </summary>
    public class GrouponOnlineUpdater
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(GrouponOnlineUpdater));

        // the store this instance for
        private readonly GrouponStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public GrouponOnlineUpdater(GrouponStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Push the shipment details to the store.
        /// </summary>
        public void UpdateShipmentDetails(IEnumerable<long> orderKeys)
        {
            List<GrouponTracking> trackingList = new List<GrouponTracking>();
            foreach (long orderKey in orderKeys)
            {
                ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderKey);

                // Check to see if shipment exists
                if (shipment == null)
                {
                    log.InfoFormat("Not uploading orderid {0} has no items.", orderKey);
                    continue;
                }

                trackingList.AddRange(GetGrouponTracking(shipment));
            }

            PerformUpload(trackingList);
        }

        /// <summary>
        /// Push the online status for an shipment.
        /// </summary>
        public void UpdateShipmentDetails(ShipmentEntity shipment)
        {
            List<GrouponTracking> trackingList = GetGrouponTracking(shipment);

            PerformUpload(trackingList);
        }

        /// <summary>
        /// Gets the tracking info to send to groupon
        /// </summary>
        private static List<GrouponTracking> GetGrouponTracking(ShipmentEntity shipment)
        {
            ShippingManager.EnsureShipmentLoaded(shipment);
            OrderEntity order = shipment.Order;

            if (order.IsManual)
            {
                return new List<GrouponTracking>();
            }

            // Fetch the order items
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.FetchEntityCollection(order.OrderItems, new RelationPredicateBucket(OrderItemFields.OrderID == order.OrderID));
            }

            List<GrouponTracking> tracking = new List<GrouponTracking>();

            foreach (GrouponOrderItemEntity item in order.OrderItems)
            {
                //Need to have a CI_LineItemID to upload tracking
                if (!string.IsNullOrEmpty(item.GrouponLineItemID))
                {
                    string trackingNumber = shipment.TrackingNumber;
                    string carrier = GrouponCarrier.GetCarrierCode(shipment);
                    Int64 CILineItemID = Convert.ToInt64(item.GrouponLineItemID);

                    GrouponTracking gTracking = new GrouponTracking(carrier, CILineItemID, trackingNumber);

                    tracking.Add(gTracking);
                }
            }

            return tracking;
        }

        /// <summary>
        /// Perform the upload
        /// </summary>
        private void PerformUpload(List<GrouponTracking> trackingList)
        {
            if (trackingList.Any())
            {
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    var client = lifetimeScope.Resolve<IGrouponWebClient>();
                    client.UploadShipmentDetails(store, trackingList);
                }
            }
        }
    }
}