using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotZero
{
    public class Item : IItem
    {
        [JsonProperty("additionalData")]
        public string AdditionalData { get; set; }

        [JsonProperty("amountRefunded")]
        public double AmountRefunded { get; set; }

        [JsonProperty("appliedRuleIds")]
        public string AppliedRuleIds { get; set; }

        [JsonProperty("baseAmountRefunded")]
        public double BaseAmountRefunded { get; set; }

        [JsonProperty("baseCost")]
        public int BaseCost { get; set; }

        [JsonProperty("baseDiscountAmount")]
        public double BaseDiscountAmount { get; set; }

        [JsonProperty("baseDiscountInvoiced")]
        public double BaseDiscountInvoiced { get; set; }

        [JsonProperty("baseDiscountRefunded")]
        public int BaseDiscountRefunded { get; set; }

        [JsonProperty("baseDiscountTaxCompensationAmount")]
        public double BaseDiscountTaxCompensationAmount { get; set; }

        [JsonProperty("baseDiscountTaxCompensationInvoiced")]
        public int BaseDiscountTaxCompensationInvoiced { get; set; }

        [JsonProperty("baseDiscountTaxCompensationRefunded")]
        public int BaseDiscountTaxCompensationRefunded { get; set; }

        [JsonProperty("baseOriginalPrice")]
        public double BaseOriginalPrice { get; set; }

        [JsonProperty("basePrice")]
        public double BasePrice { get; set; }

        [JsonProperty("basePriceInclTax")]
        public double BasePriceInclTax { get; set; }

        [JsonProperty("baseRowInvoiced")]
        public double BaseRowInvoiced { get; set; }

        [JsonProperty("baseRowTotal")]
        public double BaseRowTotal { get; set; }

        [JsonProperty("baseRowTotalInclTax")]
        public double BaseRowTotalInclTax { get; set; }

        [JsonProperty("baseTaxAmount")]
        public double BaseTaxAmount { get; set; }

        [JsonProperty("baseTaxBeforeDiscount")]
        public int BaseTaxBeforeDiscount { get; set; }

        [JsonProperty("baseTaxInvoiced")]
        public double BaseTaxInvoiced { get; set; }

        [JsonProperty("baseTaxRefunded")]
        public int BaseTaxRefunded { get; set; }

        [JsonProperty("baseWeeeTaxAppliedAmount")]
        public int BaseWeeeTaxAppliedAmount { get; set; }

        [JsonProperty("baseWeeeTaxAppliedRowAmnt")]
        public int BaseWeeeTaxAppliedRowAmnt { get; set; }

        [JsonProperty("baseWeeeTaxDisposition")]
        public int BaseWeeeTaxDisposition { get; set; }

        [JsonProperty("baseWeeeTaxRowDisposition")]
        public int BaseWeeeTaxRowDisposition { get; set; }

        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("discountAmount")]
        public double DiscountAmount { get; set; }

        [JsonProperty("discountInvoiced")]
        public double DiscountInvoiced { get; set; }

        [JsonProperty("discountPercent")]
        public double DiscountPercent { get; set; }

        [JsonProperty("discountRefunded")]
        public double DiscountRefunded { get; set; }

        [JsonProperty("eventId")]
        public int EventId { get; set; }

        [JsonProperty("extOrderItemId")]
        public string ExtOrderItemId { get; set; }

        [JsonProperty("freeShipping")]
        public int FreeShipping { get; set; }

        [JsonProperty("discountTaxCompensationAmount")]
        public double DiscountTaxCompensationAmount { get; set; }

        [JsonProperty("discountTaxCompensationCanceled")]
        public double DiscountTaxCompensationCanceled { get; set; }

        [JsonProperty("discountTaxCompensationInvoiced")]
        public double DiscountTaxCompensationInvoiced { get; set; }

        [JsonProperty("discountTaxCompensationRefunded")]
        public double DiscountTaxCompensationRefunded { get; set; }

        [JsonProperty("isQtyDecimal")]
        public int IsQtyDecimal { get; set; }

        [JsonProperty("isVirtual")]
        public int IsVirtual { get; set; }

        [JsonProperty("itemId")]
        public int ItemId { get; set; }

        [JsonProperty("lockedDoInvoice")]
        public int LockedDoInvoice { get; set; }

        [JsonProperty("lockedDoShip")]
        public int LockedDoShip { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("noDiscount")]
        public int NoDiscount { get; set; }

        [JsonProperty("orderId")]
        public int OrderId { get; set; }

        [JsonProperty("originalPrice")]
        public double OriginalPrice { get; set; }

        [JsonProperty("parentItemId")]
        public int? ParentItemId { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("priceInclTax")]
        public double PriceInclTax { get; set; }

        [JsonProperty("productId")]
        public int ProductId { get; set; }

        [JsonProperty("productType")]
        public string ProductType { get; set; }

        [JsonProperty("qtyBackordered")]
        public int QtyBackordered { get; set; }

        [JsonProperty("qtyCanceled")]
        public double QtyCanceled { get; set; }

        [JsonProperty("qtyInvoiced")]
        public double QtyInvoiced { get; set; }

        [JsonProperty("qtyOrdered")]
        public double QtyOrdered { get; set; }

        [JsonProperty("qtyRefunded")]
        public double QtyRefunded { get; set; }

        [JsonProperty("qtyReturned")]
        public int QtyReturned { get; set; }

        [JsonProperty("qtyShipped")]
        public double QtyShipped { get; set; }

        [JsonProperty("quoteItemId")]
        public int QuoteItemId { get; set; }

        [JsonProperty("rowInvoiced")]
        public double RowInvoiced { get; set; }

        [JsonProperty("rowTotal")]
        public double RowTotal { get; set; }

        [JsonProperty("rowTotalInclTax")]
        public double RowTotalInclTax { get; set; }

        [JsonProperty("rowWeight")]
        public double RowWeight { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("storeId")]
        public int StoreId { get; set; }

        [JsonProperty("taxAmount")]
        public double TaxAmount { get; set; }

        [JsonProperty("taxBeforeDiscount")]
        public int TaxBeforeDiscount { get; set; }

        [JsonProperty("taxCanceled")]
        public int TaxCanceled { get; set; }

        [JsonProperty("taxInvoiced")]
        public double TaxInvoiced { get; set; }

        [JsonProperty("taxPercent")]
        public double TaxPercent { get; set; }

        [JsonProperty("taxRefunded")]
        public int TaxRefunded { get; set; }

        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }

        [JsonProperty("weeeTaxApplied")]
        public string WeeeTaxApplied { get; set; }

        [JsonProperty("weeeTaxAppliedAmount")]
        public int WeeeTaxAppliedAmount { get; set; }

        [JsonProperty("weeeTaxAppliedRowAmount")]
        public int WeeeTaxAppliedRowAmount { get; set; }

        [JsonProperty("weeeTaxDisposition")]
        public int WeeeTaxDisposition { get; set; }

        [JsonProperty("weeeTaxRowDisposition")]
        public int WeeeTaxRowDisposition { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }

        [JsonProperty("parentItem")]
        public IParentItem ParentItem { get; set; }

        [JsonProperty("productOption")]
        public ProductOption ProductOption { get; set; }

        [JsonProperty("extensionAttributes")]
        public ExtensionAttributes ExtensionAttributes { get; set; }
    }
}