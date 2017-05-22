using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.BigCommerce.DTO
{
    /// <summary>
    /// ConfigurableField dto object that gets populated by the REST call
    /// </summary>
    public class BigCommerceConfigurableField
    {
        public int id { get; set; }
        public int configurable_field_id { get; set; }
        public int order_id { get; set; }
        public int order_product_id { get; set; }
        public int product_id { get; set; }
        public object value { get; set; }
        public string original_filename { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public List<string> select_box_options { get; set; }
    }
}
