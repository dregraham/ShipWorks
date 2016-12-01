namespace ShipWorks.Stores.Platforms.Magento.DTO.Interfaces
{
    public interface IItem
    {
        double AmountRefunded { get; set; }
        string AppliedRuleIds { get; set; }
        double BaseAmountRefunded { get; set; }
        double BaseDiscountAmount { get; set; }
        double BaseDiscountInvoiced { get; set; }
        double BaseDiscountTaxCompensationAmount { get; set; }
        double BaseOriginalPrice { get; set; }
        double BasePrice { get; set; }
        double BasePriceInclTax { get; set; }
        double BaseRowInvoiced { get; set; }
        double BaseRowTotal { get; set; }
        double BaseRowTotalInclTax { get; set; }
        double BaseTaxAmount { get; set; }
        double BaseTaxInvoiced { get; set; }
        string CreatedAt { get; set; }
        double DiscountAmount { get; set; }
        double DiscountInvoiced { get; set; }
        double DiscountPercent { get; set; }
        int FreeShipping { get; set; }
        double DiscountTaxCompensationAmount { get; set; }
        int IsQtyDecimal { get; set; }
        int IsVirtual { get; set; }
        int ItemId { get; set; }
        string Name { get; set; }
        int NoDiscount { get; set; }
        int OrderId { get; set; }
        double OriginalPrice { get; set; }
        double Price { get; set; }
        double PriceInclTax { get; set; }
        int ProductId { get; set; }
        string ProductType { get; set; }
        double QtyCanceled { get; set; }
        double QtyInvoiced { get; set; }
        double QtyOrdered { get; set; }
        double QtyRefunded { get; set; }
        double QtyShipped { get; set; }
        int QuoteItemId { get; set; }
        double RowInvoiced { get; set; }
        double RowTotal { get; set; }
        double RowTotalInclTax { get; set; }
        double RowWeight { get; set; }
        string Sku { get; set; }
        int StoreId { get; set; }
        double TaxAmount { get; set; }
        double TaxInvoiced { get; set; }
        double TaxPercent { get; set; }
        string UpdatedAt { get; set; }
        double Weight { get; set; }
        int? ParentItemId { get; set; }
        IParentItem ParentItem { get; set; }
        IProductOption ProductOption { get; set; }
    }
}