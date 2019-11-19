using System;
using System.Collections.Generic;
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

		[JsonProperty("sortOrder")]
		public string SortOrder { get; set; }

		[JsonProperty("maxResultsPerPage")]
		public int MaxResultsPerPage { get; set; } = 100;

		[JsonProperty("pageIndex")]
		public int PageIndex { get; set; }

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
	public class RakutenOrderSearchQuery
	{
		[JsonProperty("shopKey")]
		public RakutenShopKey ShopKey { get; set; }

		[JsonProperty("createdAfter")]
		public DateTime CreatedAfter { get; set; }

		[JsonProperty("createdBefore")]
		public DateTime CreatedBefore { get; set; }

		[JsonProperty("orderNumbers")]
		public IList<string> OrderNumbers { get; set; }

		[JsonProperty("orderStatus")]
		public IList<String> OrderStatus { get; set; }

		[JsonProperty("lastModifiedAfter")]
		public DateTime? LastModifiedAfter { get; set; }

		[JsonProperty("lastModifiedBefore")]
		public DateTime? LastModifiedBefore { get; set; }

		[JsonProperty("buyerName")]
		public string BuyerName { get; set; }

		[JsonProperty("shippingMethod")]
		public string ShippingMethod { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		public RakutenOrderSearchQuery(IRakutenStoreEntity store)
		{
			this.ShopKey = new RakutenShopKey(store);
		}
	}

	/// <summary>
	/// The store-specific identification key of the request
	/// </summary>
	public class RakutenShopKey
	{
		[JsonProperty("marketplaceIdentifier")]
		public string MarketplaceID { get; set; }

		[JsonProperty("shopUrl")]
		public string ShopURL { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		public RakutenShopKey(IRakutenStoreEntity store)
		{
			this.MarketplaceID = store.MarketplaceID;
			this.ShopURL = store.ShopURL;
		}
	}
}
