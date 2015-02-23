﻿using System;
using ShipWorks.Stores.Content;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Groupon
{
    /// <summary>
    /// Uniquely identifies an GrouponStore order in the database
    /// </summary>
    public class GrouponOrderIdentifier : OrderIdentifier
    {
        // GrouponStore's Order ID
        private readonly string GrouponStoreOrderId = "";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="GrouponStoreOrderID"></param>
        public GrouponOrderIdentifier(string GrouponStoreOrderId)
        {
            this.GrouponStoreOrderId = GrouponStoreOrderId;
        }

        /// <summary>
        /// Apply the order number to the order provided
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            GrouponOrderEntity GrouponStoreOrder = order as GrouponOrderEntity;

            if (GrouponStoreOrder == null)
            {
                throw new InvalidOperationException("A non GrouponStore order was passed to the GrouponStore order identifier.");
            }

            GrouponStoreOrder.GrouponOrderID = GrouponStoreOrderId;
        }

        /// <summary>
        /// Apply the order number to the download log entity
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
            downloadDetail.ExtraStringData1 = GrouponStoreOrderId;
        }

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return string.Format("GrouponStoreOrderID:{0}", GrouponStoreOrderId);
        }
    }
}
