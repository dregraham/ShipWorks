using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Magento.DTO.Interfaces
{
    public interface IOrder
    {
        double AdjustmentNegative { get; set; }
        double AdjustmentPositive { get; set; }
        string AppliedRuleIds { get; set; }
        double BaseAdjustmentNegative { get; set; }
        double BaseAdjustmentPositive { get; set; }
        string BaseCurrencyCode { get; set; }
        double BaseDiscountAmount { get; set; }
        double BaseDiscountCanceled { get; set; }
        double BaseDiscountInvoiced { get; set; }
        double BaseDiscountRefunded { get; set; }
        double BaseDiscountTaxCompensationAmount { get; set; }
        double BaseGrandTotal { get; set; }
        double BaseShippingAmount { get; set; }
        double BaseShippingDiscountAmount { get; set; }
        double BaseShippingInclTax { get; set; }
        double BaseShippingTaxAmount { get; set; }
        double BaseSubtotal { get; set; }
        double BaseSubtotalInclTax { get; set; }
        double BaseTaxAmount { get; set; }
        double BaseToGlobalRate { get; set; }
        double BaseToOrderRate { get; set; }
        double BaseTotalDue { get; set; }
        IBillingAddress BillingAddress { get; set; }
        int BillingAddressId { get; set; }
        string CouponCode { get; set; }
        string CreatedAt { get; set; }
        string CustomerEmail { get; set; }
        string CustomerFirstname { get; set; }
        int CustomerGroupId { get; set; }
        int CustomerId { get; set; }
        int CustomerIsGuest { get; set; }
        string CustomerLastname { get; set; }
        int CustomerNoteNotify { get; set; }
        double DiscountAmount { get; set; }
        string DiscountDescription { get; set; }
        double DiscountTaxCompensationAmount { get; set; }
        int EmailSent { get; set; }
        int EntityId { get; set; }
        IExtensionAttributes ExtensionAttributes { get; set; }
        string GlobalCurrencyCode { get; set; }
        double GrandTotal { get; set; }
        string IncrementId { get; set; }
        int IsVirtual { get; set; }
        IEnumerable<IItem> Items { get; set; }
        string OrderCurrencyCode { get; set; }
        IPayment Payment { get; set; }
        string ProtectCode { get; set; }
        int QuoteId { get; set; }
        double ShippingAmount { get; set; }
        string ShippingDescription { get; set; }
        double ShippingDiscountAmount { get; set; }
        double ShippingDiscountTaxCompensationAmount { get; set; }
        double ShippingInclTax { get; set; }
        double ShippingTaxAmount { get; set; }
        string State { get; set; }
        string Status { get; set; }
        string StoreCurrencyCode { get; set; }
        int StoreId { get; set; }
        string StoreName { get; set; }
        double StoreToBaseRate { get; set; }
        double StoreToOrderRate { get; set; }
        double Subtotal { get; set; }
        double SubtotalInclTax { get; set; }
        double TaxAmount { get; set; }
        double TotalDue { get; set; }
        int TotalItemCount { get; set; }
        double TotalPaid { get; set; }
        double TotalQtyOrdered { get; set; }
        string UpdatedAt { get; set; }
        double Weight { get; set; }
    }
}