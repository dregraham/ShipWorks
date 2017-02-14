using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder
{
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
        public double? ShippingDiscountTaxCompensationAmount { get; set; }

        [JsonProperty("shipping_incl_tax")]
        public double ShippingInclTax { get; set; }

        [JsonProperty("shipping_tax_amount")]
        public double ShippingTaxAmount { get; set; }
    }
}