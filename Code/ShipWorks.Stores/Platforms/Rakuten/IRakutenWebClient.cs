﻿using System;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Rakuten.DTO;
using ShipWorks.Stores.Platforms.Rakuten.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Rakuten
{
    /// <summary>
    /// The client for communicating with the Rakuten API
    /// </summary>
    public interface IRakutenWebClient
    {
        /// <summary>
        /// Get a list of orders from Rakuten
        /// </summary>
        Task<RakutenOrdersResponse> GetOrders(IRakutenStoreEntity store, DateTime startDate);

        /// <summary>
        /// Get a product's variants and details from Rakuten
        /// </summary>
        Task<RakutenProductsResponse> GetProduct(IRakutenStoreEntity store, string baseSKU);

        /// <summary>
        /// Verify we can connect with Rakuten
        /// </summary>
        Task<GenericResult<bool>> TestConnection(RakutenStoreEntity testStore);

        /// <summary>
        /// Mark order as shipped and upload tracking number
        /// </summary>
        Task<RakutenBaseResponse> ConfirmShipping(IRakutenStoreEntity store, ShipmentEntity shipment, RakutenUploadDetails details);
    }
}