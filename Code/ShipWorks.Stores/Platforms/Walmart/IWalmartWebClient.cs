using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Collections.Generic;
using ShipWorks.Stores.Platforms.Walmart.DTO;

namespace ShipWorks.Stores.Platforms.Walmart
{
    /// <summary>
    /// Web client for interacting with Walmart
    /// </summary>
    public interface IWalmartWebClient
    {
        /// <summary>
        /// Tests the connection to Walmart
        /// </summary>
        void TestConnection(WalmartStoreEntity store);

        /// <summary>
        /// Get orders created after the given start date
        /// </summary>
        IEnumerable<Order> GetOrders(DateTime start);
    }
}