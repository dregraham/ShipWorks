using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Groupon.DTO;

namespace ShipWorks.Stores.Platforms.Groupon
{
    /// <summary>
    /// Web client for Groupon
    /// </summary>
    public interface IGrouponWebClient
    {
        /// <summary>
        /// Download orders from Groupon
        /// </summary>
        JToken GetOrders(IGrouponStoreEntity store, DateTime start, int currentPage);

        /// <summary>
        /// Uploads a batch of shipments to Groupon
        /// </summary>
        void UploadShipmentDetails(IGrouponStoreEntity store, List<GrouponTracking> trackingList);
    }
}