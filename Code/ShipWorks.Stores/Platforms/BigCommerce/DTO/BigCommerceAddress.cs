using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.BigCommerce.DTO
{
    /// <summary>
    /// Address dto object that gets populated by the REST call
    /// </summary>
    public class BigCommerceAddress
    {
        public int id { get; set; }
        public int order_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string company { get; set; }
        public string street_1 { get; set; }
        public string street_2 { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string country_iso2 { get; set; }
        public string state { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public int items_total { get; set; }
        public long items_shipped { get; set; }
        public string shipping_method { get; set; }
        public string base_cost { get; set; }
        public string cost_ex_tax { get; set; }
        public string cost_inc_tax { get; set; }
        public string cost_tax { get; set; }
        public int cost_tax_class_id { get; set; }
        public string base_handling_cost { get; set; }
        public string handling_cost_ex_tax { get; set; }
        public string handling_cost_inc_tax { get; set; }
        public string handling_cost_tax { get; set; }
        public int handling_cost_tax_class_id { get; set; }
        public int shipping_zone_id { get; set; }
        public string shipping_zone_name { get; set; }
    }
}
