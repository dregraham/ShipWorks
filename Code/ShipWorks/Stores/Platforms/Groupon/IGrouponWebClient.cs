using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        Task<JToken> GetOrders(IGrouponStoreEntity store, DateTime start, int currentPage);

        /// <summary>
        /// Uploads a batch of shipments to Groupon
        /// </summary>
        Task UploadShipmentDetails(IGrouponStoreEntity store, List<GrouponTracking> trackingList);
    }
}