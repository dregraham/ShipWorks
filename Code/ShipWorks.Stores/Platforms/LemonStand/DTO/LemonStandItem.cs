using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace ShipWorks.Stores.Platforms.LemonStand.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public class LemonStandItem
    {
        //"id": 3,
        //"name": "Football helmet",
        //"shop_product_type_id": 1,
        //"shop_manufacturer_id": null,
        //"shop_tax_class_id": 1,
        //"title": null,
        //"sku": "helmet",
        //"description": "<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.<p>",
        //"short_description": null,
        //"meta_description": null,
        //"meta_keywords": null,
        //"url_name": "helmet",
        //"base_price": 139.99,
        //"cost": null,
        //"depth": null,
        //"width": null,
        //"height": null,
        //"weight": null,
        //"enabled": 1,
        //"is_on_sale": 0,
        //"sale_price_or_discount": null,
        //"track_inventory": 0,
        //"allow_preorder": 0,
        //"in_stock_amount": null,
        //"hide_out_of_stock": 0,
        //"out_of_stock_threshold": 0,
        //"low_stock_threshold": null,
        //"expected_availability_date": null,
        //"allow_negative_stock": 0,
        //"is_catalog_visible": 1,
        //"is_search_visible": 1,
        //"created_by": null,
        //"updated_by": null,
        //"created_at": "2015-09-02T09:41:05-0700",
        //"updated_at": "2015-09-02T09:41:05-0700"

        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("quantity")]
        public string Quantity { get; set; }

        [JsonProperty("base_price")]
        public string BasePrice { get; set; }

        [JsonProperty("weight")]
        public string Weight { get; set; }

        [JsonProperty("url_name")]
        public string UrlName { get; set; }

        [JsonProperty("cost")]
        public string Cost { get; set; }

        [JsonProperty("is_on_sale")]
        public string IsOnSale { get; set; }

        [JsonProperty("sale_price_or_discount")]
        public string SalePriceOrDiscount { get; set; }

        [JsonProperty("short_description")]
        public string ShortDescription { get; set; }

        [JsonIgnore]
        public string Category { get; set; }

        [JsonIgnore]
        public string Thumbnail { get; set; }
        [JsonIgnore]
        public IList<JToken> Attributes { get; set; }
    }
}