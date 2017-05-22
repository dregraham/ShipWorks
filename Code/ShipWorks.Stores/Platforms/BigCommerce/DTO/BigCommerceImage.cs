using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.BigCommerce.DTO
{
    public class BigCommerceImage
    {
        public int id { get; set; }
        public int product_id { get; set; }
        public string image_file { get; set; }
        public bool is_thumbnail { get; set; }
        public int sort_order { get; set; }
        public string date_created { get; set; }

        public string thumbnail_url { get; set; }
        public string standard_url { get; set; }
        public string tiny_url { get; set; }
        public string description { get; set; }
    }
}
