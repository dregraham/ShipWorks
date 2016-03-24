using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.SparkPay.DTO
{
    public class Order
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("customer_id")]
        public int? CustomerId { get; set; }

        [JsonProperty("customer_type_id")]
        public int? CustomerTypeId { get; set; }

        [JsonProperty("adcode")]
        public string AdCode { get; set; }

        [JsonProperty("ordered_at")]
        public DateTimeOffset? OrderedAt { get; set; }

        [JsonProperty("order_status_id")]
        public int? OrderStatusId { get; set; }
        
        [JsonProperty("special_instructions")]
        public string SpecialEnstructions { get; set; }

        [JsonProperty("subtotal")]
        public decimal? Subtotal { get; set; }

        [JsonProperty("tax_total")]
        public decimal? TaxTotal { get; set; }

        [JsonProperty("shipping_total")]
        public decimal? ShippingTotal { get; set; }

        [JsonProperty("discount_total")]
        public decimal? DiscountTotal { get; set; }

        [JsonProperty("grand_total")]
        public decimal? GrandTotal { get; set; }

        [JsonProperty("cost_total")]
        public decimal? CostTotal { get; set; }

        [JsonProperty("selected_shipping_method")]
        public string SelectedShippingMethod { get; set; }

        [JsonProperty("ip_address")]
        public string IpAddress { get; set; }

        [JsonProperty("referrer")]
        public string Referrer { get; set; }

        [JsonProperty("order_shipping_address_id")]
        public int? OrderShippingAddressId { get; set; }

        [JsonProperty("order_billing_address_id")]
        public int? OrderBillingAddressId { get; set; }

        [JsonProperty("admin_comments")]
        public string AdminComments { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("search_phrase")]
        public string SearchPhrase { get; set; }

        [JsonProperty("is_ppc")]
        public bool IsPpc { get; set; }

        [JsonProperty("ppc_keyword")]
        public string PpcKeyword { get; set; }

        [JsonProperty("affiliate_id")]
        public object AffiliateId { get; set; }

        [JsonProperty("store_id")]
        public int? StoreId { get; set; }

        [JsonProperty("session_id")]
        public int? SessionId { get; set; }

        [JsonProperty("handling_total")]
        public double? HandlingTotal { get; set; }

        [JsonProperty("is_payment_order_only")]
        public bool IsPaymentOrderOnly { get; set; }

        [JsonProperty("payments")]
        public IList<Payment> Payments { get; set; }

        [JsonProperty("selected_shipping_provider_service")]
        public string SelectedShippingProviderService { get; set; }

        [JsonProperty("additional_fees")]
        public double? AdditionalFees { get; set; }

        [JsonProperty("adcode_id")]
        public int? AdCodeId { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        [JsonProperty("is_gift")]
        public bool IsGift { get; set; }

        [JsonProperty("gift_message")]
        public string GiftMessage { get; set; }

        [JsonProperty("public_comments")]
        public string PublicComments { get; set; }

        [JsonProperty("instructions")]
        public string Instructions { get; set; }

        [JsonProperty("source_group")]
        public string SourceGroup { get; set; }

        [JsonProperty("from_subscription_id")]
        public object FromSubscriptionId { get; set; }

        [JsonProperty("previous_order_status_id")]
        public int? PreviousOrderStatusId { get; set; }

        [JsonProperty("order_status_last_changed_at")]
        public string OrderStatusLastChangedAt { get; set; }

        [JsonProperty("discounted_shipping_total")]
        public double? DiscountedShippingTotal { get; set; }

        [JsonProperty("order_number")]
        public string OrderNumber { get; set; }

        [JsonProperty("coupon_code")]
        public string CouponCode { get; set; }

        [JsonProperty("order_type")]
        public string OrderType { get; set; }

        [JsonProperty("expires_at")]
        public object ExpiresAt { get; set; }

        [JsonProperty("expires")]
        public bool Expires { get; set; }

        [JsonProperty("from_quote_id")]
        public object FromQuoteId { get; set; }

        [JsonProperty("campaign_code")]
        public string CampaignCode { get; set; }

        [JsonProperty("reward_points_credited")]
        public bool RewardPointsCredited { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("device")]
        public string Device { get; set; }

        [JsonProperty("manufacturer_invoice_number")]
        public string ManufacturerInvoiceNumber { get; set; }

        [JsonProperty("manufacturer_invoice_amount")]
        public double? ManufacturerInvoiceAmount { get; set; }

        [JsonProperty("manufacturer_invoice_paid")]
        public bool ManufacturerInvoicePaid { get; set; }

        [JsonProperty("items")]
        public IList<Item> Items { get; set; }
    }
}
