using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.BigCommerce.DTO
{
    /// <summary>
    /// OrderStatus dto object that gets populated by the REST call
    /// </summary>
    public class BigCommerceApiOrderStatus
    {
        public int id { get; set; }
        public string name { get; set; }
        public int order { get; set; }
    }
}
