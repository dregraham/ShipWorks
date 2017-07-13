using System;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Enums;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.DTO
{
    /// <summary>
    /// Order Item Adjustment entity for ChannelAdvisor
    /// </summary>
    public class ChannelAdvisorOrderItemAdjustment
    {
        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("ProfileID")]
        public int ProfileID { get; set; }

        [JsonProperty("OrderID")]
        public int OrderID { get; set; }

        [JsonProperty("OrderItemID")]
        public int OrderItemID { get; set; }

        [JsonProperty("Quantity")]
        public int Quantity { get; set; }

        [JsonProperty("IsRestock")]
        public bool IsRestock { get; set; }

        [JsonProperty("Reason")]
        public ChannelAdvisorAdjustmentReason Reason { get; set; }

        [JsonProperty("PreventSiteProcessing")]
        public bool PreventSiteProcessing { get; set; }

        [JsonProperty("ItemAdjustment")]
        public double ItemAdjustment { get; set; }

        [JsonProperty("TaxAdjustment")]
        public double TaxAdjustment { get; set; }

        [JsonProperty("ShippingAdjustment")]
        public double ShippingAdjustment { get; set; }

        [JsonProperty("ShippingTaxAdjustment")]
        public double ShippingTaxAdjustment { get; set; }

        [JsonProperty("GiftWrapAdjustment")]
        public double GiftWrapAdjustment { get; set; }

        [JsonProperty("GiftWrapTaxAdjustment")]
        public double GiftWrapTaxAdjustment { get; set; }

        [JsonProperty("RecyclingFeeAdjustment")]
        public double RecyclingFeeAdjustment { get; set; }

        [JsonProperty("Type")]
        public Type Type { get; set; }

        [JsonProperty("SellerAdjustmentID")]
        public string SellerAdjustmentID { get; set; }

        [JsonProperty("RmaNumber")]
        public string RmaNumber { get; set; }

        [JsonProperty("Comment")]
        public string Comment { get; set; }

        [JsonProperty("CreatedDateUtc")]
        public string CreatedDateUtc { get; set; }

        [JsonProperty("RequestStatus")]
        public ChannelAdvisorAdjustmentRequestStatus RequestStatus { get; set; }

        [JsonProperty("RestockStatus")]
        public ChannelAdvisorAdjustmentRequestStatus RestockStatus { get; set; }
    }
}