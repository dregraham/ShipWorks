using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.BigCommerce.DTO
{
    /// <summary>
    /// ProductOption dto object that gets populated by the REST call
    /// </summary>
    public class BigCommerceProductOption
    {
        public int id { get; set; }
        public int order_product_id { get; set; }
        public int option_id { get; set; }
        public int product_option_id { get; set; }
        public string display_name { get; set; }
        public string display_value { get; set; }
        public string value { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string display_style { get; set; }
    }
}
