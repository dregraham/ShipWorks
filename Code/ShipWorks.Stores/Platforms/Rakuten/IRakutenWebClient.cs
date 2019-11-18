﻿using System;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Rakuten.DTO;

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
        /// Mark order as shipped and upload tracking number
        /// </summary>
        Task<RakutenBaseResponse> ConfirmShipping(IRakutenStoreEntity store, ShipmentEntity shipment);

        /// <summary>
        /// Verify we can connect with Rakuten
        /// </summary>
        Task<bool> TestConnection(RakutenStoreEntity testStore);
    }
}