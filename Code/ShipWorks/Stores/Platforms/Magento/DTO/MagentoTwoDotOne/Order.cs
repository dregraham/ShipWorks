using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne
{
    public class Order
    {
        [JsonProperty("adjustment_negative")]
        public double AdjustmentNegative { get; set; }

        [JsonProperty("adjustment_positive")]
        public double AdjustmentPositive { get; set; }

        [JsonProperty("applied_rule_ids")]
        public string AppliedRuleIds { get; set; }

        [JsonProperty("base_adjustment_negative")]
        public double BaseAdjustmentNegative { get; set; }

        [JsonProperty("base_adjustment_positive")]
        public double BaseAdjustmentPositive { get; set; }

        [JsonProperty("base_currency_code")]
        public string BaseCurrencyCode { get; set; }

        [JsonProperty("base_discount_amount")]
        public double BaseDiscountAmount { get; set; }

        [JsonProperty("base_discount_canceled")]
        public double BaseDiscountCanceled { get; set; }

        [JsonProperty("base_discount_invoiced")]
        public double BaseDiscountInvoiced { get; set; }

        [JsonProperty("base_discount_refunded")]
        public double BaseDiscountRefunded { get; set; }

        [JsonProperty("base_grand_total")]
        public double BaseGrandTotal { get; set; }

        [JsonProperty("base_discount_tax_compensation_amount")]
        public double BaseDiscountTaxCompensationAmount { get; set; }

        [JsonProperty("base_shipping_amount")]
        public double BaseShippingAmount { get; set; }

        [JsonProperty("base_shipping_discount_amount")]
        public double BaseShippingDiscountAmount { get; set; }

        [JsonProperty("base_shipping_incl_tax")]
        public double BaseShippingInclTax { get; set; }

        [JsonProperty("base_shipping_tax_amount")]
        public double BaseShippingTaxAmount { get; set; }

        [JsonProperty("base_subtotal")]
        public double BaseSubtotal { get; set; }

        [JsonProperty("base_subtotal_incl_tax")]
        public double BaseSubtotalInclTax { get; set; }

        [JsonProperty("base_tax_amount")]
        public double BaseTaxAmount { get; set; }

        [JsonProperty("base_total_due")]
        public double BaseTotalDue { get; set; }

        [JsonProperty("base_to_global_rate")]
        public double BaseToGlobalRate { get; set; }

        [JsonProperty("base_to_order_rate")]
        public double BaseToOrderRate { get; set; }

        [JsonProperty("billing_address_id")]
        public int BillingAddressId { get; set; }

        [JsonProperty("coupon_code")]
        public string CouponCode { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("customer_email")]
        public string CustomerEmail { get; set; }

        [JsonProperty("customer_firstname")]
        public string CustomerFirstname { get; set; }

        [JsonProperty("customer_group_id")]
        public int CustomerGroupId { get; set; }

        [JsonProperty("customer_id")]
        public int CustomerId { get; set; }

        [JsonProperty("customer_is_guest")]
        public int CustomerIsGuest { get; set; }

        [JsonProperty("customer_lastname")]
        public string CustomerLastname { get; set; }

        [JsonProperty("customer_note_notify")]
        public int CustomerNoteNotify { get; set; }

        [JsonProperty("discount_amount")]
        public double DiscountAmount { get; set; }

        [JsonProperty("discount_description")]
        public string DiscountDescription { get; set; }

        [JsonProperty("email_sent")]
        public int EmailSent { get; set; }

        [JsonProperty("entity_id")]
        public int EntityId { get; set; }

        [JsonProperty("global_currency_code")]
        public string GlobalCurrencyCode { get; set; }

        [JsonProperty("grand_total")]
        public double GrandTotal { get; set; }

        [JsonProperty("discount_tax_compensation_amount")]
        public double DiscountTaxCompensationAmount { get; set; }

        [JsonProperty("increment_id")]
        public string IncrementId { get; set; }

        [JsonProperty("is_virtual")]
        public int IsVirtual { get; set; }

        [JsonProperty("order_currency_code")]
        public string OrderCurrencyCode { get; set; }

        [JsonProperty("protect_code")]
        public string ProtectCode { get; set; }

        [JsonProperty("quote_id")]
        public int QuoteId { get; set; }

        [JsonProperty("shipping_amount")]
        public double ShippingAmount { get; set; }

        [JsonProperty("shipping_description")]
        public string ShippingDescription { get; set; }

        [JsonProperty("shipping_discount_amount")]
        public double ShippingDiscountAmount { get; set; }

        [JsonProperty("shipping_discount_tax_compensation_amount")]
        public double ShippingDiscountTaxCompensationAmount { get; set; }

        [JsonProperty("shipping_incl_tax")]
        public double ShippingInclTax { get; set; }

        [JsonProperty("shipping_tax_amount")]
        public double ShippingTaxAmount { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("store_currency_code")]
        public string StoreCurrencyCode { get; set; }

        [JsonProperty("store_id")]
        public int StoreId { get; set; }

        [JsonProperty("store_name")]
        public string StoreName { get; set; }

        [JsonProperty("store_to_base_rate")]
        public double StoreToBaseRate { get; set; }

        [JsonProperty("store_to_order_rate")]
        public double StoreToOrderRate { get; set; }

        [JsonProperty("subtotal")]
        public double Subtotal { get; set; }

        [JsonProperty("subtotal_incl_tax")]
        public double SubtotalInclTax { get; set; }

        [JsonProperty("tax_amount")]
        public double TaxAmount { get; set; }

        [JsonProperty("total_due")]
        public double TotalDue { get; set; }

        [JsonProperty("total_item_count")]
        public int TotalItemCount { get; set; }

        [JsonProperty("total_paid")]
        public double TotalPaid { get; set; }

        [JsonProperty("total_qty_ordered")]
        public double TotalQtyOrdered { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }

        [JsonProperty("items")]
        public IEnumerable<Item> Items { get; set; }

        [JsonProperty("billing_address")]
        public BillingAddress BillingAddress { get; set; }

        [JsonProperty("payment")]
        public Payment Payment { get; set; }

        [JsonProperty("extension_attributes")]
        public ExtensionAttributes ExtensionAttributes { get; set; }
    }
}