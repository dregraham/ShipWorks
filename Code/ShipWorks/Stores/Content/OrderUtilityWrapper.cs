﻿using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Order management
    /// </summary>
    public class OrderManager : IOrderManager
    {
        /// <summary>
        /// Get a populated order from a given shipment
        /// </summary>
        public void PopulateOrderDetails(ShipmentEntity shipment)
        {
            OrderUtility.PopulateOrderDetails(shipment);
        }
    }
}