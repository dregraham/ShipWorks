using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.BigCommerce.DTO
{
    /// <summary>
    /// Coupon dto object that gets populated by the REST call
    /// </summary>
    public class BigCommerceCoupon
    {
        public int id { get; set; }
        public int order_id { get; set; }
        public int coupon_id { get; set; }
        public string code { get; set; }
        public decimal amount { get; set; }
        public string type { get; set; }
        public decimal discount { get; set; }
    }
}
