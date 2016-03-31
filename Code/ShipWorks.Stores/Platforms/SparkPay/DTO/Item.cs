using Newtonsoft.Json;
using System;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.SparkPay.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public class Item
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("order_id")]
        public int? OrderId { get; set; }

        [JsonProperty("product_id")]
        public int? ProductId { get; set; }

        [JsonProperty("item_number")]
        public string ItemNumber { get; set; }

        [JsonProperty("item_name")]
        public string ItemName { get; set; }

        [JsonProperty("price")]
        public decimal? Price { get; set; }

        [JsonProperty("cost")]
        public decimal? Cost { get; set; }

        [JsonProperty("quantity")]
        public int? Quantity { get; set; }

        [JsonProperty("is_discount_item")]
        public bool? IsDiscountItem { get; set; }

        [JsonProperty("weight")]
        public double? Weight { get; set; }

        [JsonProperty("is_taxable")]
        public bool? IsTaxable { get; set; }

        [JsonProperty("weight_unit")]
        public string WeightUnit { get; set; }

        [JsonProperty("warehouse_id")]
        public int? WarehouseId { get; set; }

        [JsonProperty("parent_order_item_id")]
        public object ParentOrderItemId { get; set; }

        [JsonProperty("is_quantity_bound_to_parent")]
        public bool? IsQuantityBoundToParent { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("height")]
        public double? Height { get; set; }

        [JsonProperty("length")]
        public double? Length { get; set; }

        [JsonProperty("width")]
        public double? Width { get; set; }

        [JsonProperty("size_unit")]
        public string SizeUnit { get; set; }

        [JsonProperty("tax_code")]
        public string TaxCode { get; set; }

        [JsonProperty("item_number_full")]
        public string ItemNumberFull { get; set; }

        [JsonProperty("admin_comments")]
        public string AdminComments { get; set; }

        [JsonProperty("do_not_discount")]
        public bool DoNotDiscount { get; set; }

        [JsonProperty("line_item_note")]
        public string LineItemNote { get; set; }

        [JsonProperty("order_shipping_address_id")]
        public object OrderShippingAddressId { get; set; }

        [JsonProperty("gift_message")]
        public string GiftMessage { get; set; }

        [JsonProperty("delivery_date")]
        public object DeliveryDate { get; set; }

        [JsonProperty("discount_amount")]
        public double? DiscountAmount { get; set; }

        [JsonProperty("discount_percentage")]
        public double? DiscountPercentage { get; set; }

        [JsonProperty("is_subscription_product")]
        public bool IsSubscriptionProduct { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("variants")]
        public object Variants { get; set; }

        [JsonProperty("personalizations")]
        public object Personalizations { get; set; }
    }
}
