using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.BigCommerce.DTO
{
    /// <summary>
    /// Item dto object that gets populated by the REST call
    /// </summary>
    public class BigCommerceItem
    {
        public int order_product_id { get; set; }
        //public int product_id { get; set; }
        public int quantity { get; set; }
    }
}
