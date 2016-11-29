using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO
{
    public class OrdersResponse
    {
        [JsonProperty("items")]
        public List<Order> Orders { get; set; }

        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
    }

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
        public List<Item> Items { get; set; }

        [JsonProperty("billing_address")]
        public BillingAddress BillingAddress { get; set; }

        [JsonProperty("payment")]
        public Payment Payment { get; set; }

        [JsonProperty("status_histories")]
        public List<object> StatusHistories { get; set; }

        [JsonProperty("extension_attributes")]
        public ExtensionAttributes ExtensionAttributes { get; set; }
    }

    public class ParentItem
    {
        [JsonProperty("amount_refunded")]
        public double AmountRefunded { get; set; }

        [JsonProperty("applied_rule_ids")]
        public string AppliedRuleIds { get; set; }

        [JsonProperty("base_amount_refunded")]
        public double BaseAmountRefunded { get; set; }

        [JsonProperty("base_discount_amount")]
        public double BaseDiscountAmount { get; set; }

        [JsonProperty("base_discount_invoiced")]
        public double BaseDiscountInvoiced { get; set; }

        [JsonProperty("base_discount_tax_compensation_amount")]
        public double BaseDiscountTaxCompensationAmount { get; set; }

        [JsonProperty("base_original_price")]
        public double BaseOriginalPrice { get; set; }

        [JsonProperty("base_price")]
        public double BasePrice { get; set; }

        [JsonProperty("base_price_incl_tax")]
        public double BasePriceInclTax { get; set; }

        [JsonProperty("base_row_invoiced")]
        public double BaseRowInvoiced { get; set; }

        [JsonProperty("base_row_total")]
        public double BaseRowTotal { get; set; }

        [JsonProperty("base_row_total_incl_tax")]
        public double BaseRowTotalInclTax { get; set; }

        [JsonProperty("base_tax_amount")]
        public double BaseTaxAmount { get; set; }

        [JsonProperty("base_tax_invoiced")]
        public double BaseTaxInvoiced { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("discount_amount")]
        public double DiscountAmount { get; set; }

        [JsonProperty("discount_invoiced")]
        public double DiscountInvoiced { get; set; }

        [JsonProperty("discount_percent")]
        public double DiscountPercent { get; set; }

        [JsonProperty("free_shipping")]
        public int FreeShipping { get; set; }

        [JsonProperty("discount_tax_compensation_amount")]
        public double DiscountTaxCompensationAmount { get; set; }

        [JsonProperty("is_qty_decimal")]
        public int IsQtyDecimal { get; set; }

        [JsonProperty("is_virtual")]
        public int IsVirtual { get; set; }

        [JsonProperty("item_id")]
        public int ItemId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("no_discount")]
        public int NoDiscount { get; set; }

        [JsonProperty("order_id")]
        public int OrderId { get; set; }

        [JsonProperty("original_price")]
        public double OriginalPrice { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("price_incl_tax")]
        public double PriceInclTax { get; set; }

        [JsonProperty("product_id")]
        public int ProductId { get; set; }

        [JsonProperty("product_type")]
        public string ProductType { get; set; }

        [JsonProperty("qty_canceled")]
        public double QtyCanceled { get; set; }

        [JsonProperty("qty_invoiced")]
        public double QtyInvoiced { get; set; }

        [JsonProperty("qty_ordered")]
        public double QtyOrdered { get; set; }

        [JsonProperty("qty_refunded")]
        public double QtyRefunded { get; set; }

        [JsonProperty("qty_shipped")]
        public double QtyShipped { get; set; }

        [JsonProperty("quote_item_id")]
        public int QuoteItemId { get; set; }

        [JsonProperty("row_invoiced")]
        public double RowInvoiced { get; set; }

        [JsonProperty("row_total")]
        public double RowTotal { get; set; }

        [JsonProperty("row_total_incl_tax")]
        public double RowTotalInclTax { get; set; }

        [JsonProperty("row_weight")]
        public double RowWeight { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("store_id")]
        public int StoreId { get; set; }

        [JsonProperty("tax_amount")]
        public double TaxAmount { get; set; }

        [JsonProperty("tax_invoiced")]
        public double TaxInvoiced { get; set; }

        [JsonProperty("tax_percent")]
        public double TaxPercent { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }
    }

    public class Item
    {
        [JsonProperty("amount_refunded")]
        public double AmountRefunded { get; set; }

        [JsonProperty("applied_rule_ids")]
        public string AppliedRuleIds { get; set; }

        [JsonProperty("base_amount_refunded")]
        public double BaseAmountRefunded { get; set; }

        [JsonProperty("base_discount_amount")]
        public double BaseDiscountAmount { get; set; }

        [JsonProperty("base_discount_invoiced")]
        public double BaseDiscountInvoiced { get; set; }

        [JsonProperty("base_discount_tax_compensation_amount")]
        public double BaseDiscountTaxCompensationAmount { get; set; }

        [JsonProperty("base_original_price")]
        public double BaseOriginalPrice { get; set; }

        [JsonProperty("base_price")]
        public double BasePrice { get; set; }

        [JsonProperty("base_price_incl_tax")]
        public double BasePriceInclTax { get; set; }

        [JsonProperty("base_row_invoiced")]
        public double BaseRowInvoiced { get; set; }

        [JsonProperty("base_row_total")]
        public double BaseRowTotal { get; set; }

        [JsonProperty("base_row_total_incl_tax")]
        public double BaseRowTotalInclTax { get; set; }

        [JsonProperty("base_tax_amount")]
        public double BaseTaxAmount { get; set; }

        [JsonProperty("base_tax_invoiced")]
        public double BaseTaxInvoiced { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("discount_amount")]
        public double DiscountAmount { get; set; }

        [JsonProperty("discount_invoiced")]
        public double DiscountInvoiced { get; set; }

        [JsonProperty("discount_percent")]
        public double DiscountPercent { get; set; }

        [JsonProperty("free_shipping")]
        public int FreeShipping { get; set; }

        [JsonProperty("discount_tax_compensation_amount")]
        public double DiscountTaxCompensationAmount { get; set; }

        [JsonProperty("is_qty_decimal")]
        public int IsQtyDecimal { get; set; }

        [JsonProperty("is_virtual")]
        public int IsVirtual { get; set; }

        [JsonProperty("item_id")]
        public int ItemId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("no_discount")]
        public int NoDiscount { get; set; }

        [JsonProperty("order_id")]
        public int OrderId { get; set; }

        [JsonProperty("original_price")]
        public double OriginalPrice { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("price_incl_tax")]
        public double PriceInclTax { get; set; }

        [JsonProperty("product_id")]
        public int ProductId { get; set; }

        [JsonProperty("product_type")]
        public string ProductType { get; set; }

        [JsonProperty("qty_canceled")]
        public double QtyCanceled { get; set; }

        [JsonProperty("qty_invoiced")]
        public double QtyInvoiced { get; set; }

        [JsonProperty("qty_ordered")]
        public double QtyOrdered { get; set; }

        [JsonProperty("qty_refunded")]
        public double QtyRefunded { get; set; }

        [JsonProperty("qty_shipped")]
        public double QtyShipped { get; set; }

        [JsonProperty("quote_item_id")]
        public int QuoteItemId { get; set; }

        [JsonProperty("row_invoiced")]
        public double RowInvoiced { get; set; }

        [JsonProperty("row_total")]
        public double RowTotal { get; set; }

        [JsonProperty("row_total_incl_tax")]
        public double RowTotalInclTax { get; set; }

        [JsonProperty("row_weight")]
        public double RowWeight { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("store_id")]
        public int StoreId { get; set; }

        [JsonProperty("tax_amount")]
        public double TaxAmount { get; set; }

        [JsonProperty("tax_invoiced")]
        public double TaxInvoiced { get; set; }

        [JsonProperty("tax_percent")]
        public double TaxPercent { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }

        [JsonProperty("parent_item_id")]
        public int? ParentItemId { get; set; }

        [JsonProperty("parent_item")]
        public ParentItem ParentItem { get; set; }
    }

    public class BillingAddress
    {
        [JsonProperty("address_type")]
        public string AddressType { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country_id")]
        public string CountryId { get; set; }

        [JsonProperty("customer_address_id")]
        public int CustomerAddressId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("entity_id")]
        public int EntityId { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("middlename")]
        public string Middlename { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("parent_id")]
        public int ParentId { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("region_code")]
        public string RegionCode { get; set; }

        [JsonProperty("region_id")]
        public int RegionId { get; set; }

        [JsonProperty("street")]
        public List<string> Street { get; set; }

        [JsonProperty("telephone")]
        public string Telephone { get; set; }
    }

    public class Payment
    {
        [JsonProperty("account_status")]
        public object AccountStatus { get; set; }

        [JsonProperty("additional_information")]
        public IList<string> AdditionalInformation { get; set; }

        [JsonProperty("amount_ordered")]
        public double AmountOrdered { get; set; }

        [JsonProperty("base_amount_ordered")]
        public double BaseAmountOrdered { get; set; }

        [JsonProperty("base_shipping_amount")]
        public double BaseShippingAmount { get; set; }

        [JsonProperty("cc_exp_year")]
        public string CcExpYear { get; set; }

        [JsonProperty("cc_last4")]
        public object CcLast4 { get; set; }

        [JsonProperty("cc_ss_start_month")]
        public string CcSsStartMonth { get; set; }

        [JsonProperty("cc_ss_start_year")]
        public string CcSsStartYear { get; set; }

        [JsonProperty("entity_id")]
        public int EntityId { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("parent_id")]
        public int ParentId { get; set; }

        [JsonProperty("shipping_amount")]
        public double ShippingAmount { get; set; }

        [JsonProperty("extension_attributes")]
        public IList<object> ExtensionAttributes { get; set; }
    }

    public class Address
    {
        [JsonProperty("address_type")]
        public string AddressType { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country_id")]
        public string CountryId { get; set; }

        [JsonProperty("customer_address_id")]
        public int CustomerAddressId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("entity_id")]
        public int EntityId { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("iddlename")]
        public string Middlename { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("parent_id")]
        public int ParentId { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("region_code")]
        public string RegionCode { get; set; }

        [JsonProperty("region_id")]
        public int RegionId { get; set; }

        [JsonProperty("street")]
        public IList<string> Street { get; set; }

        [JsonProperty("telephone")]
        public string Telephone { get; set; }
    }

    public class Total
    {
        [JsonProperty("base_shipping_amount")]
        public double BaseShippingAmount { get; set; }

        [JsonProperty("base_shipping_discount_amount")]
        public double BaseShippingDiscountAmount { get; set; }

        [JsonProperty("base_shipping_incl_tax")]
        public double BaseShippingInclTax { get; set; }

        [JsonProperty("base_shipping_tax_amount")]
        public double BaseShippingTaxAmount { get; set; }

        [JsonProperty("shipping_amount")]
        public double ShippingAmount { get; set; }

        [JsonProperty("shipping_discount_amount")]
        public double ShippingDiscountAmount { get; set; }

        [JsonProperty("shipping_discount_tax_compensation_amount")]
        public double ShippingDiscountTaxCompensationAmount { get; set; }

        [JsonProperty("shipping_incl_tax")]
        public double ShippingInclTax { get; set; }

        [JsonProperty("shipping_tax_amount")]
        public double ShippingTaxAmount { get; set; }
    }

    public class Shipping
    {
        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("total")]
        public Total Total { get; set; }
    }

    public class ShippingAssignment
    {
        [JsonProperty("shipping")]
        public Shipping Shipping { get; set; }

        [JsonProperty("items")]
        public IList<Item> Items { get; set; }
    }

    public class ExtensionAttributes
    {
        [JsonProperty("shipping_assignments")]
        public IList<ShippingAssignment> ShippingAssignments { get; set; }
    }
}