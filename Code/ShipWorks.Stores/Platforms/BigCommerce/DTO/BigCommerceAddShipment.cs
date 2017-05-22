using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.BigCommerce.DTO
{
    /// <summary>
    /// AddShipment dto object that gets populated by the REST call
    /// </summary>
    public class BigCommerceAddShipment
    {
        public int order_address_id { get; set; }
        public string tracking_number { get; set; }
        public string shipping_method { get; set; }
        public string shipping_provider { get; set; }
        public string comments { get; set; }
        public BigCommerceBillingAddress billing_address { get; set; }
        public BigCommerceAddress shipping_address { get; set; }
        public List<BigCommerceItem> items { get; set; }
    }
}
