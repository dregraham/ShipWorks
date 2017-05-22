using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.BigCommerce.DTO
{
    /// <summary>
    /// Coupons dto object that gets populated by the REST call
    /// Yes, it's poorly named...but thats what the BigCommerce REST result named it.
    /// </summary>
    public class BigCommerceCoupons
    {
        public string url { get; set; }
        public string resource { get; set; }
    }
}
