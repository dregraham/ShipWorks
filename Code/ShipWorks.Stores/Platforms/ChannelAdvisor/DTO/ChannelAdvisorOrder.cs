﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.DTO
{
    /// <summary>
    /// Order entity returned by ChannelAdvisor
    /// </summary>
    public class ChannelAdvisorOrder
    {
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }

        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("ProfileID")]
        public int ProfileID { get; set; }

        [JsonProperty("SiteID")]
        public int SiteID { get; set; }

        [JsonProperty("SiteName")]
        public string SiteName { get; set; }

        [JsonProperty("SiteOrderID")]
        public string SiteOrderID { get; set; }

        [JsonProperty("SecondarySiteOrderID")]
        public object SecondarySiteOrderID { get; set; }

        [JsonProperty("SellerOrderID")]
        public object SellerOrderID { get; set; }

        [JsonProperty("CheckoutSourceID")]
        public object CheckoutSourceID { get; set; }

        [JsonProperty("CreatedDateUtc")]
        public DateTime CreatedDateUtc { get; set; }

        [JsonProperty("ImportDateUtc")]
        public DateTime ImportDateUtc { get; set; }

        [JsonProperty("PublicNotes")]
        public object PublicNotes { get; set; }

        [JsonProperty("PrivateNotes")]
        public string PrivateNotes { get; set; }

        [JsonProperty("SpecialInstructions")]
        public string SpecialInstructions { get; set; }

        [JsonProperty("TotalPrice")]
        public int TotalPrice { get; set; }

        [JsonProperty("TotalTaxPrice")]
        public int TotalTaxPrice { get; set; }

        [JsonProperty("TotalShippingPrice")]
        public int TotalShippingPrice { get; set; }

        [JsonProperty("TotalShippingTaxPrice")]
        public object TotalShippingTaxPrice { get; set; }

        [JsonProperty("TotalInsurancePrice")]
        public int TotalInsurancePrice { get; set; }

        [JsonProperty("TotalGiftOptionPrice")]
        public int TotalGiftOptionPrice { get; set; }

        [JsonProperty("TotalGiftOptionTaxPrice")]
        public object TotalGiftOptionTaxPrice { get; set; }

        [JsonProperty("AdditionalCostOrDiscount")]
        public int AdditionalCostOrDiscount { get; set; }

        [JsonProperty("EstimatedShipDateUtc")]
        public object EstimatedShipDateUtc { get; set; }

        [JsonProperty("DeliverByDateUtc")]
        public object DeliverByDateUtc { get; set; }

        [JsonProperty("ResellerID")]
        public object ResellerID { get; set; }

        [JsonProperty("FlagID")]
        public int FlagID { get; set; }

        [JsonProperty("FlagDescription")]
        public object FlagDescription { get; set; }

        [JsonProperty("OrderTags")]
        public object OrderTags { get; set; }

        [JsonProperty("DistributionCenterTypeRollup")]
        public string DistributionCenterTypeRollup { get; set; }

        [JsonProperty("CheckoutStatus")]
        public string CheckoutStatus { get; set; }

        [JsonProperty("PaymentStatus")]
        public string PaymentStatus { get; set; }

        [JsonProperty("ShippingStatus")]
        public string ShippingStatus { get; set; }

        [JsonProperty("CheckoutDateUtc")]
        public DateTime CheckoutDateUtc { get; set; }

        [JsonProperty("PaymentDateUtc")]
        public DateTime PaymentDateUtc { get; set; }

        [JsonProperty("ShippingDateUtc")]
        public object ShippingDateUtc { get; set; }

        [JsonProperty("BuyerUserId")]
        public string BuyerUserId { get; set; }

        [JsonProperty("BuyerEmailAddress")]
        public string BuyerEmailAddress { get; set; }

        [JsonProperty("BuyerEmailOptIn")]
        public bool BuyerEmailOptIn { get; set; }

        [JsonProperty("OrderTaxType")]
        public string OrderTaxType { get; set; }

        [JsonProperty("ShippingTaxType")]
        public string ShippingTaxType { get; set; }

        [JsonProperty("GiftOptionsTaxType")]
        public string GiftOptionsTaxType { get; set; }

        [JsonProperty("PaymentMethod")]
        public string PaymentMethod { get; set; }

        [JsonProperty("PaymentTransactionID")]
        public object PaymentTransactionID { get; set; }

        [JsonProperty("PaymentPaypalAccountID")]
        public object PaymentPaypalAccountID { get; set; }

        [JsonProperty("PaymentCreditCardLast4")]
        public string PaymentCreditCardLast4 { get; set; }

        [JsonProperty("PaymentMerchantReferenceNumber")]
        public object PaymentMerchantReferenceNumber { get; set; }

        [JsonProperty("ShippingTitle")]
        public object ShippingTitle { get; set; }

        [JsonProperty("ShippingFirstName")]
        public string ShippingFirstName { get; set; }

        [JsonProperty("ShippingLastName")]
        public string ShippingLastName { get; set; }

        [JsonProperty("ShippingSuffix")]
        public object ShippingSuffix { get; set; }

        [JsonProperty("ShippingCompanyName")]
        public object ShippingCompanyName { get; set; }

        [JsonProperty("ShippingCompanyJobTitle")]
        public object ShippingCompanyJobTitle { get; set; }

        [JsonProperty("ShippingDaytimePhone")]
        public string ShippingDaytimePhone { get; set; }

        [JsonProperty("ShippingEveningPhone")]
        public object ShippingEveningPhone { get; set; }

        [JsonProperty("ShippingAddressLine1")]
        public string ShippingAddressLine1 { get; set; }

        [JsonProperty("ShippingAddressLine2")]
        public string ShippingAddressLine2 { get; set; }

        [JsonProperty("ShippingCity")]
        public string ShippingCity { get; set; }

        [JsonProperty("ShippingStateOrProvince")]
        public string ShippingStateOrProvince { get; set; }

        [JsonProperty("ShippingPostalCode")]
        public string ShippingPostalCode { get; set; }

        [JsonProperty("ShippingCountry")]
        public string ShippingCountry { get; set; }

        [JsonProperty("BillingTitle")]
        public object BillingTitle { get; set; }

        [JsonProperty("BillingFirstName")]
        public string BillingFirstName { get; set; }

        [JsonProperty("BillingLastName")]
        public string BillingLastName { get; set; }

        [JsonProperty("BillingSuffix")]
        public string BillingSuffix { get; set; }

        [JsonProperty("BillingCompanyName")]
        public string BillingCompanyName { get; set; }

        [JsonProperty("BillingCompanyJobTitle")]
        public object BillingCompanyJobTitle { get; set; }

        [JsonProperty("BillingDaytimePhone")]
        public string BillingDaytimePhone { get; set; }

        [JsonProperty("BillingEveningPhone")]
        public object BillingEveningPhone { get; set; }

        [JsonProperty("BillingAddressLine1")]
        public string BillingAddressLine1 { get; set; }

        [JsonProperty("BillingAddressLine2")]
        public string BillingAddressLine2 { get; set; }

        [JsonProperty("BillingCity")]
        public string BillingCity { get; set; }

        [JsonProperty("BillingStateOrProvince")]
        public string BillingStateOrProvince { get; set; }

        [JsonProperty("BillingPostalCode")]
        public string BillingPostalCode { get; set; }

        [JsonProperty("BillingCountry")]
        public string BillingCountry { get; set; }

        [JsonProperty("PromotionCode")]
        public object PromotionCode { get; set; }

        [JsonProperty("PromotionAmount")]
        public int PromotionAmount { get; set; }

        [JsonProperty("Items")]
        public IList<ChannelAdvisorOrderItem> Items { get; set; }

        [JsonProperty("Fulfillments")]
        public IList<ChannelAdvisorFulfillment> Fulfillments { get; set; }

        [JsonProperty("Adjustments")]
        public IList<ChannelAdvisorOrderAdjustment> Adjustments { get; set; }

        [JsonProperty("CustomFields")]
        public IList<ChannelAdvisorCustomField> CustomFields { get; set; }
    }
}
