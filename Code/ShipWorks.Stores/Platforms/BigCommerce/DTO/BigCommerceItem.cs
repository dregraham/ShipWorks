using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.BigCommerce.DTO
{
    /// <summary>
    /// Item dto object that gets populated by the REST call
    /// </summary>
    [SuppressMessage("SonarQube", "S125:Remove this commented out code",
        Justification = "The commented out properties are included in the API but we don't use them")]
    public class BigCommerceItem
    {
        public int order_product_id { get; set; }
        //public int product_id { get; set; }
        public int quantity { get; set; }
    }
}
