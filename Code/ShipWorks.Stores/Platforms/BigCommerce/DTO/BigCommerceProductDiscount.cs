using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.BigCommerce.DTO
{
    /// <summary>
    /// ProductDiscount dto object that gets populated by the REST call
    /// </summary>
    public class BigCommerceProductDiscount
    {
        public string id { get; set; }
        public decimal amount { get; set; }
    }
}
