using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Platform.OnlineUpdating
{
    public class NotifyMarketplaceShippedRequest
    {
        [JsonProperty("notify_marketplace_shipped_requests")]
        public IEnumerable<PlatformBulkOnlineUpdateItem> NotifyMarketplaceShippedRequests { get; set; }
    }
    /// <summary>
    /// The request takes an array of these items, so there is no root
    /// </summary>
    public class PlatformBulkOnlineUpdateItem
    {
        [JsonProperty("sales_order_id")]
        public string SalesOrderId { get; set; }

        [JsonProperty("tracking_number")]
        public string TrackingNumber { get; set; }

        [JsonProperty("carrier_code")]
        public string CarrierCode { get; set; }

        [JsonProperty("ship_from")]
        public ShipFrom ShipFrom { get; set; }
    }

    public class SalesOrderItem
    {
        [JsonProperty("sales_order_item_id")]
        public string SalesOrderItemId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }

    public class ShipFrom
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("address_line1")]
        public string AddressLine1 { get; set; }

        [JsonProperty("address_line2")]
        public string AddressLine2 { get; set; }

        [JsonProperty("address_line3")]
        public string AddressLine3 { get; set; }

        [JsonProperty("city_locality")]
        public string CityLocality { get; set; }

        [JsonProperty("state_province")]
        public string StateProvince { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }
    }
}
