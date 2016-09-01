﻿using System;
using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Connection;
using log4net;
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
            foreach(long orderKey in orderKeys)
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

            if(trackingList.Count > 0)
            {
                GrouponWebClient client = new GrouponWebClient(store);

                client.UploadShipmentDetails(trackingList);
            }
        }

        /// <summary>
        /// Push the online status for an shipment.
        /// </summary>
        public void UpdateShipmentDetails(ShipmentEntity shipment)
        {
            List<GrouponTracking> trackingList = GetGrouponTracking(shipment);

            if(trackingList.Count > 0)
            {
                GrouponWebClient client = new GrouponWebClient(store);

                client.UploadShipmentDetails(trackingList);
            }
        }

        /// <summary>
        /// Gets the tracking info to send to groupon
        /// </summary>
        private static List<GrouponTracking> GetGrouponTracking(ShipmentEntity shipment)
        {
            OrderEntity order = shipment.Order;

            List<GrouponTracking> tracking = new List<GrouponTracking>();
            // Fetch the order items
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.FetchEntityCollection(order.OrderItems, new RelationPredicateBucket(OrderItemFields.OrderID == order.OrderID));
            }

            foreach (GrouponOrderItemEntity item in order.OrderItems)
            {
                //Need to have a CI_LineItemID to upload tracking
                if(!string.IsNullOrEmpty(item.GrouponLineItemID))
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
    }
}