using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public string comments { get; set; }
        public BigCommerceBillingAddress billing_address { get; set; }
        public BigCommerceAddress shipping_address { get; set; }
        public List<BigCommerceItem> items { get; set; }
    }
}
