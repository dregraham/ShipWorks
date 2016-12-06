using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder
{
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
}