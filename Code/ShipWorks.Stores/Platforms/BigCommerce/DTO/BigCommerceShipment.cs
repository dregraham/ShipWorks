using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.BigCommerce.DTO
{
    /// <summary>
    /// Shipment dto object that gets populated by the REST call
    /// </summary>
    public class BigCommerceShipment
    {
        public int id { get; set; }
        public int order_id { get; set; }
        public int customer_id { get; set; }
        public int order_address_id { get; set; }
        public string date_created { get; set; }
        public string tracking_number { get; set; }
        public string shipping_method { get; set; }
        public string comments { get; set; }
        public BigCommerceBillingAddress billing_address { get; set; }
        public BigCommerceAddress shipping_address { get; set; }
        public List<BigCommerceItem> items { get; set; }
    }
}
