using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.BigCommerce.DTO
{
    /// <summary>
    /// Product dto object that gets populated by the REST call
    /// </summary>
    public class BigCommerceProduct
    {
        public int id { get; set; }
        public int order_id { get; set; }
        public int product_id { get; set; }
        public int order_address_id { get; set; }
        public string name { get; set; }
        public string sku { get; set; }
        public string type { get; set; }
        public string base_price { get; set; }
        public string price_ex_tax { get; set; }
        public string price_inc_tax { get; set; }
        public string price_tax { get; set; }
        public string base_total { get; set; }
        public string total_ex_tax { get; set; }
        public string total_inc_tax { get; set; }
        public string total_tax { get; set; }
        public string weight { get; set; }
        public int quantity { get; set; }
        public string base_cost_price { get; set; }
        public string cost_price_inc_tax { get; set; }
        public string cost_price_ex_tax { get; set; }
        public string cost_price_tax { get; set; }
        public bool is_refunded { get; set; }
        public string refund_amount { get; set; }
        public int return_id { get; set; }
        public string wrapping_name { get; set; }
        public string base_wrapping_cost { get; set; }
        public string wrapping_cost_ex_tax { get; set; }
        public string wrapping_cost_inc_tax { get; set; }
        public string wrapping_cost_tax { get; set; }
        public string wrapping_message { get; set; }
        public int quantity_shipped { get; set; }
        public string event_date { get; set; }
        public string fixed_shipping_cost { get; set; }
        public string ebay_item_id { get; set; }
        public string ebay_transaction_id { get; set; }
        public int option_set_id { get; set; }
        public bool is_bundled_product { get; set; }
        public string bin_picking_number { get; set; }
        public object parent_order_product_id { get; set; }
        public string event_name { get; set; }
        public List<BigCommerceProductDiscount> applied_discounts { get; set; }
        public List<BigCommerceProductOption> product_options { get; set; }
        public List<BigCommerceConfigurableField> configurable_fields { get; set; }
        public string Image { get; set; }
        public string ThumbnailImage { get; set; }
    }

}
