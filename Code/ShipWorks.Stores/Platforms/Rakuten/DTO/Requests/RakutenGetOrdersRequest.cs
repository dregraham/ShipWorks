﻿using System;
using System.Reflection;
using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Rakuten.DTO.Requests
{
    /// <summary>
    /// The request to retrieve orders from Rakuten
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class RakutenGetOrdersRequest
    {
        [JsonProperty("query")]
        public RakutenOrderSearchQuery Query { get; set; }

        [JsonProperty("maxResultsPerPage")]
        public int MaxResultsPerPage { get; set; } = 100;

        [JsonProperty("returnOrderDetail")]
        public bool ReturnOrderDetail { get; set; } = true;

        /// <summary>
        /// Constructor
        /// </summary>
        public RakutenGetOrdersRequest(IRakutenStoreEntity store, DateTime createdBefore, DateTime createdAfter, DateTime lastModified)
        {
            this.Query = new RakutenOrderSearchQuery(store)
            {
                CreatedAfter = createdAfter,
                CreatedBefore = createdBefore,
                LastModifiedAfter = lastModified
            };
        }
    }

    /// <summary>
    /// The search query used to filter orders from Rakuten
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class RakutenOrderSearchQuery
    {
        [JsonProperty("shopKey")]
        public RakutenShopKey ShopKey { get; set; }

        [JsonProperty("createdAfter")]
        public DateTime CreatedAfter { get; set; }

        [JsonProperty("createdBefore")]
        public DateTime CreatedBefore { get; set; }

        [JsonProperty("lastModifiedAfter")]
        public DateTime? LastModifiedAfter { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public RakutenOrderSearchQuery(IRakutenStoreEntity store)
        {
            this.ShopKey = new RakutenShopKey(store);
        }
    }
}
