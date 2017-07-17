using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.DTO
{
    /// <summary>
    /// Order Item entity returned by ChannelAdvisor
    /// </summary>
    public class ChannelAdvisorOrderItem
    {
        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("ProfileID")]
        public int ProfileID { get; set; }

        [JsonProperty("FulfillmentID")]
        public int FulfillmentID { get; set; }

        [JsonProperty("OrderID")]
        public int OrderID { get; set; }

        [JsonProperty("OrderItemID")]
        public int OrderItemID { get; set; }

        [JsonProperty("ProductID")]
        public int ProductID { get; set; }

        [JsonProperty("SiteOrderItemID")]
        public string SiteOrderItemID { get; set; }

        [JsonProperty("SiteListingID")]
        public string SiteListingID { get; set; }

        [JsonProperty("Sku")]
        public string Sku { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Quantity")]
        public int Quantity { get; set; }

        [JsonProperty("UnitPrice")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("TaxPrice")]
        public int TaxPrice { get; set; }

        [JsonProperty("ShippingPrice")]
        public int ShippingPrice { get; set; }

        [JsonProperty("ShippingTaxPrice")]
        public int ShippingTaxPrice { get; set; }

        [JsonProperty("RecyclingFee")]
        public int RecyclingFee { get; set; }

        [JsonProperty("GiftMessage")]
        public string GiftMessage { get; set; }

        [JsonProperty("GiftNotes")]
        public string GiftNotes { get; set; }

        [JsonProperty("GiftPrice")]
        public decimal GiftPrice { get; set; }

        [JsonProperty("GiftTaxPrice")]
        public decimal GiftTaxPrice { get; set; }

        [JsonProperty("IsBundle")]
        public bool IsBundle { get; set; }

        [JsonProperty("ItemURL")]
        public string ItemURL { get; set; }

        [JsonProperty("Promotions")]
        public IList<ChannelAdvisorPromotion> Promotions { get; set; }

        [JsonProperty("FulfillmentItems")]
        public IList<ChannelAdvisorFulfillmentItem> FulfillmentItems { get; set; }

        [JsonProperty("BundleComponents")]
        public IList<ChannelAdvisorOrderItemBundleComponents> BundleComponents { get; set; }

        [JsonProperty("Adjustments")]
        public IList<ChannelAdvisorOrderItemAdjustment> Adjustments { get; set; }
    }
}