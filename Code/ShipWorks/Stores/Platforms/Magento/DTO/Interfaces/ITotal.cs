namespace ShipWorks.Stores.Platforms.Magento.DTO.Interfaces
{
    public interface ITotal
    {
        double BaseShippingAmount { get; set; }
        double BaseShippingDiscountAmount { get; set; }
        double BaseShippingInclTax { get; set; }
        double BaseShippingTaxAmount { get; set; }
        double ShippingAmount { get; set; }
        double ShippingDiscountAmount { get; set; }
        double ShippingDiscountTaxCompensationAmount { get; set; }
        double ShippingInclTax { get; set; }
        double ShippingTaxAmount { get; set; }
    }
}