using System.Collections.Generic;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotZero
{
    public class Order : IOrder
    {
        [JsonProperty("adjustmentNegative")]
        public double AdjustmentNegative { get; set; }

        [JsonProperty("adjustmentPositive")]
        public double AdjustmentPositive { get; set; }

        [JsonProperty("appliedRuleIds")]
        public string AppliedRuleIds { get; set; }

        [JsonProperty("baseAdjustmentNegative")]
        public double BaseAdjustmentNegative { get; set; }

        [JsonProperty("baseAdjustmentPositive")]
        public double BaseAdjustmentPositive { get; set; }

        [JsonProperty("baseCurrencyCode")]
        public string BaseCurrencyCode { get; set; }

        [JsonProperty("baseDiscountAmount")]
        public double BaseDiscountAmount { get; set; }

        [JsonProperty("baseDiscountCanceled")]
        public double BaseDiscountCanceled { get; set; }

        [JsonProperty("baseDiscountInvoiced")]
        public double BaseDiscountInvoiced { get; set; }

        [JsonProperty("baseDiscountRefunded")]
        public double BaseDiscountRefunded { get; set; }

        [JsonProperty("baseGrandTotal")]
        public double BaseGrandTotal { get; set; }

        [JsonProperty("baseDiscountTaxCompensationAmount")]
        public double BaseDiscountTaxCompensationAmount { get; set; }

        [JsonProperty("baseDiscountTaxCompensationInvoiced")]
        public int BaseDiscountTaxCompensationInvoiced { get; set; }

        [JsonProperty("baseDiscountTaxCompensationRefunded")]
        public int BaseDiscountTaxCompensationRefunded { get; set; }

        [JsonProperty("baseShippingAmount")]
        public double BaseShippingAmount { get; set; }

        [JsonProperty("baseShippingCanceled")]
        public int BaseShippingCanceled { get; set; }

        [JsonProperty("baseShippingDiscountAmount")]
        public double BaseShippingDiscountAmount { get; set; }

        [JsonProperty("baseShippingDiscountTaxCompensationAmnt")]
        public int BaseShippingDiscountTaxCompensationAmnt { get; set; }

        [JsonProperty("baseShippingInclTax")]
        public double BaseShippingInclTax { get; set; }

        [JsonProperty("baseShippingInvoiced")]
        public int BaseShippingInvoiced { get; set; }

        [JsonProperty("baseShippingRefunded")]
        public int BaseShippingRefunded { get; set; }

        [JsonProperty("baseShippingTaxAmount")]
        public double BaseShippingTaxAmount { get; set; }

        [JsonProperty("baseShippingTaxRefunded")]
        public int BaseShippingTaxRefunded { get; set; }

        [JsonProperty("baseSubtotal")]
        public double BaseSubtotal { get; set; }

        [JsonProperty("baseSubtotalCanceled")]
        public int BaseSubtotalCanceled { get; set; }

        [JsonProperty("baseSubtotalInclTax")]
        public double BaseSubtotalInclTax { get; set; }

        [JsonProperty("baseSubtotalInvoiced")]
        public int BaseSubtotalInvoiced { get; set; }

        [JsonProperty("baseSubtotalRefunded")]
        public int BaseSubtotalRefunded { get; set; }

        [JsonProperty("baseTaxAmount")]
        public double BaseTaxAmount { get; set; }

        [JsonProperty("baseTaxCanceled")]
        public int BaseTaxCanceled { get; set; }

        [JsonProperty("baseTaxInvoiced")]
        public int BaseTaxInvoiced { get; set; }

        [JsonProperty("baseTaxRefunded")]
        public int BaseTaxRefunded { get; set; }

        [JsonProperty("baseTotalCanceled")]
        public int BaseTotalCanceled { get; set; }

        [JsonProperty("baseTotalDue")]
        public double BaseTotalDue { get; set; }

        [JsonProperty("baseTotalInvoiced")]
        public double BaseTotalInvoiced { get; set; }

        [JsonProperty("baseTotalInvoicedCost")]
        public double BaseTotalInvoicedCost { get; set; }

        [JsonProperty("baseTotalOfflineRefunded")]
        public double BaseTotalOfflineRefunded { get; set; }

        [JsonProperty("baseTotalOnlineRefunded")]
        public double BaseTotalOnlineRefunded { get; set; }

        [JsonProperty("baseTotalPaid")]
        public int BaseTotalPaid { get; set; }

        [JsonProperty("baseTotalQtyOrdered")]
        public int BaseTotalQtyOrdered { get; set; }

        [JsonProperty("baseTotalRefunded")]
        public int BaseTotalRefunded { get; set; }

        [JsonProperty("baseToGlobalRate")]
        public double BaseToGlobalRate { get; set; }

        [JsonProperty("baseToOrderRate")]
        public double BaseToOrderRate { get; set; }

        [JsonProperty("billingAddressId")]
        public int BillingAddressId { get; set; }

        [JsonProperty("canShipPartially")]
        public int CanShipPartially { get; set; }

        [JsonProperty("canShipPartiallyItem")]
        public int CanShipPartiallyItem { get; set; }

        [JsonProperty("couponCode")]
        public string CouponCode { get; set; }

        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("customerDob")]
        public string CustomerDob { get; set; }

        [JsonProperty("customerEmail")]
        public string CustomerEmail { get; set; }

        [JsonProperty("customerFirstname")]
        public string CustomerFirstname { get; set; }

        [JsonProperty("customerGender")]
        public int CustomerGender { get; set; }

        [JsonProperty("customerGroupId")]
        public int CustomerGroupId { get; set; }

        [JsonProperty("customerId")]
        public int CustomerId { get; set; }

        [JsonProperty("customerIsGuest")]
        public int CustomerIsGuest { get; set; }

        [JsonProperty("customerLastname")]
        public string CustomerLastname { get; set; }

        [JsonProperty("customerMiddlename")]
        public string CustomerMiddlename { get; set; }

        [JsonProperty("customerNote")]
        public string CustomerNote { get; set; }

        [JsonProperty("customerNoteNotify")]
        public int CustomerNoteNotify { get; set; }

        [JsonProperty("customerPrefix")]
        public string CustomerPrefix { get; set; }

        [JsonProperty("customerSuffix")]
        public string CustomerSuffix { get; set; }

        [JsonProperty("customerTaxvat")]
        public string CustomerTaxvat { get; set; }

        [JsonProperty("discountAmount")]
        public double DiscountAmount { get; set; }

        [JsonProperty("discountCanceled")]
        public int DiscountCanceled { get; set; }

        [JsonProperty("discountDescription")]
        public string DiscountDescription { get; set; }

        [JsonProperty("discountInvoiced")]
        public int DiscountInvoiced { get; set; }

        [JsonProperty("discountRefunded")]
        public int DiscountRefunded { get; set; }

        [JsonProperty("editIncrement")]
        public int EditIncrement { get; set; }

        [JsonProperty("emailSent")]
        public int EmailSent { get; set; }

        [JsonProperty("entityId")]
        public int EntityId { get; set; }

        [JsonProperty("extCustomerId")]
        public string ExtCustomerId { get; set; }

        [JsonProperty("extOrderId")]
        public string ExtOrderId { get; set; }

        [JsonProperty("forcedShipmentWithInvoice")]
        public int ForcedShipmentWithInvoice { get; set; }

        [JsonProperty("globalCurrencyCode")]
        public string GlobalCurrencyCode { get; set; }

        [JsonProperty("grandTotal")]
        public double GrandTotal { get; set; }

        [JsonProperty("discountTaxCompensationAmount")]
        public double DiscountTaxCompensationAmount { get; set; }

        [JsonProperty("discountTaxCompensationInvoiced")]
        public int DiscountTaxCompensationInvoiced { get; set; }

        [JsonProperty("discountTaxCompensationRefunded")]
        public int DiscountTaxCompensationRefunded { get; set; }

        [JsonProperty("holdBeforeState")]
        public string HoldBeforeState { get; set; }

        [JsonProperty("holdBeforeStatus")]
        public string HoldBeforeStatus { get; set; }

        [JsonProperty("incrementId")]
        public string IncrementId { get; set; }

        [JsonProperty("isVirtual")]
        public int IsVirtual { get; set; }

        [JsonProperty("orderCurrencyCode")]
        public string OrderCurrencyCode { get; set; }

        [JsonProperty("originalIncrementId")]
        public string OriginalIncrementId { get; set; }

        [JsonProperty("paymentAuthorizationAmount")]
        public int PaymentAuthorizationAmount { get; set; }

        [JsonProperty("paymentAuthExpiration")]
        public int PaymentAuthExpiration { get; set; }

        [JsonProperty("protectCode")]
        public string ProtectCode { get; set; }

        [JsonProperty("quoteAddressId")]
        public int QuoteAddressId { get; set; }

        [JsonProperty("quoteId")]
        public int QuoteId { get; set; }

        [JsonProperty("relationChildId")]
        public string RelationChildId { get; set; }

        [JsonProperty("relationChildRealId")]
        public string RelationChildRealId { get; set; }

        [JsonProperty("relationParentId")]
        public string RelationParentId { get; set; }

        [JsonProperty("relationParentRealId")]
        public string RelationParentRealId { get; set; }

        [JsonProperty("remoteIp")]
        public string RemoteIp { get; set; }

        [JsonProperty("shippingAmount")]
        public double ShippingAmount { get; set; }

        [JsonProperty("shippingCanceled")]
        public int ShippingCanceled { get; set; }

        [JsonProperty("shippingDescription")]
        public string ShippingDescription { get; set; }

        [JsonProperty("shippingDiscountAmount")]
        public double ShippingDiscountAmount { get; set; }

        [JsonProperty("shippingDiscountTaxCompensationAmount")]
        public double ShippingDiscountTaxCompensationAmount { get; set; }

        [JsonProperty("shippingInclTax")]
        public double ShippingInclTax { get; set; }

        [JsonProperty("shippingInvoiced")]
        public int ShippingInvoiced { get; set; }

        [JsonProperty("shippingRefunded")]
        public int ShippingRefunded { get; set; }

        [JsonProperty("shippingTaxAmount")]
        public double ShippingTaxAmount { get; set; }

        [JsonProperty("shippingTaxRefunded")]
        public int ShippingTaxRefunded { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("storeCurrencyCode")]
        public string StoreCurrencyCode { get; set; }

        [JsonProperty("storeId")]
        public int StoreId { get; set; }

        [JsonProperty("storeName")]
        public string StoreName { get; set; }

        [JsonProperty("storeToBaseRate")]
        public double StoreToBaseRate { get; set; }

        [JsonProperty("storeToOrderRate")]
        public double StoreToOrderRate { get; set; }

        [JsonProperty("subtotal")]
        public double Subtotal { get; set; }

        [JsonProperty("subtotalCanceled")]
        public int SubtotalCanceled { get; set; }

        [JsonProperty("subtotalInclTax")]
        public double SubtotalInclTax { get; set; }

        [JsonProperty("subtotalInvoiced")]
        public int SubtotalInvoiced { get; set; }

        [JsonProperty("subtotalRefunded")]
        public int SubtotalRefunded { get; set; }

        [JsonProperty("taxAmount")]
        public double TaxAmount { get; set; }

        [JsonProperty("taxCanceled")]
        public int TaxCanceled { get; set; }

        [JsonProperty("taxInvoiced")]
        public int TaxInvoiced { get; set; }

        [JsonProperty("taxRefunded")]
        public int TaxRefunded { get; set; }

        [JsonProperty("totalCanceled")]
        public int TotalCanceled { get; set; }

        [JsonProperty("totalDue")]
        public double TotalDue { get; set; }

        [JsonProperty("totalInvoiced")]
        public int TotalInvoiced { get; set; }

        [JsonProperty("totalItemCount")]
        public int TotalItemCount { get; set; }

        [JsonProperty("totalOfflineRefunded")]
        public int TotalOfflineRefunded { get; set; }

        [JsonProperty("totalOnlineRefunded")]
        public int TotalOnlineRefunded { get; set; }

        [JsonProperty("totalPaid")]
        public double TotalPaid { get; set; }

        [JsonProperty("totalQtyOrdered")]
        public double TotalQtyOrdered { get; set; }

        [JsonProperty("totalRefunded")]
        public double TotalRefunded { get; set; }

        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }

        [JsonProperty("xForwardedFor")]
        public string XForwardedFor { get; set; }

        [JsonProperty("items")]
        public IEnumerable<IItem> Items { get; set; }

        [JsonProperty("billingAddress")]
        public IBillingAddress BillingAddress { get; set; }

        [JsonProperty("payment")]
        public IPayment Payment { get; set; }

        [JsonProperty("statusHistories")]
        public IList<StatusHistory> StatusHistories { get; set; }

        [JsonProperty("extensionAttributes")]
        public IExtensionAttributes ExtensionAttributes { get; set; }
    }
}