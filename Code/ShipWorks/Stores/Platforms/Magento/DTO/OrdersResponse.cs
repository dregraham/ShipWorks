using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO
{
    public class Item
    {
        public int amount_refunded { get; set; }
        public string applied_rule_ids { get; set; }
        public int base_amount_refunded { get; set; }
        public double base_discount_amount { get; set; }
        public int base_discount_invoiced { get; set; }
        public int base_discount_tax_compensation_amount { get; set; }
        public int base_original_price { get; set; }
        public int base_price { get; set; }
        public int base_price_incl_tax { get; set; }
        public int base_row_invoiced { get; set; }
        public int base_row_total { get; set; }
        public int base_row_total_incl_tax { get; set; }
        public int base_tax_amount { get; set; }
        public int base_tax_invoiced { get; set; }
        public string created_at { get; set; }
        public double discount_amount { get; set; }
        public int discount_invoiced { get; set; }
        public int discount_percent { get; set; }
        public int free_shipping { get; set; }
        public int discount_tax_compensation_amount { get; set; }
        public int is_qty_decimal { get; set; }
        public int is_virtual { get; set; }
        public int item_id { get; set; }
        public string name { get; set; }
        public int no_discount { get; set; }
        public int order_id { get; set; }
        public int original_price { get; set; }
        public int price { get; set; }
        public int price_incl_tax { get; set; }
        public int product_id { get; set; }
        public string product_type { get; set; }
        public int qty_canceled { get; set; }
        public int qty_invoiced { get; set; }
        public int qty_ordered { get; set; }
        public int qty_refunded { get; set; }
        public int qty_shipped { get; set; }
        public int quote_item_id { get; set; }
        public int row_invoiced { get; set; }
        public int row_total { get; set; }
        public int row_total_incl_tax { get; set; }
        public int row_weight { get; set; }
        public string sku { get; set; }
        public int store_id { get; set; }
        public int tax_amount { get; set; }
        public int tax_invoiced { get; set; }
        public int tax_percent { get; set; }
        public string updated_at { get; set; }
        public int weight { get; set; }
    }

    public class BillingAddress
    {
        public string address_type { get; set; }
        public string city { get; set; }
        public string country_id { get; set; }
        public string email { get; set; }
        public int entity_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int parent_id { get; set; }
        public string postcode { get; set; }
        public string region { get; set; }
        public string region_code { get; set; }
        public int region_id { get; set; }
        public List<string> street { get; set; }
        public string telephone { get; set; }
        public string company { get; set; }
        public int? customer_address_id { get; set; }
        public string middlename { get; set; }
    }

    public class Payment
    {
        public object account_status { get; set; }
        public List<string> additional_information { get; set; }
        public double amount_ordered { get; set; }
        public double base_amount_ordered { get; set; }
        public int base_shipping_amount { get; set; }
        public object cc_last4 { get; set; }
        public int entity_id { get; set; }
        public string method { get; set; }
        public int parent_id { get; set; }
        public int shipping_amount { get; set; }
        public List<object> extension_attributes { get; set; }
        public string cc_exp_year { get; set; }
        public string cc_ss_start_month { get; set; }
        public string cc_ss_start_year { get; set; }
    }

    public class Address
    {
        public string address_type { get; set; }
        public string city { get; set; }
        public string country_id { get; set; }
        public string email { get; set; }
        public int entity_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int parent_id { get; set; }
        public string postcode { get; set; }
        public string region { get; set; }
        public string region_code { get; set; }
        public int region_id { get; set; }
        public List<string> street { get; set; }
        public string telephone { get; set; }
        public string company { get; set; }
        public int? customer_address_id { get; set; }
        public string middlename { get; set; }
    }

    public class Total
    {
        public int base_shipping_amount { get; set; }
        public int base_shipping_discount_amount { get; set; }
        public int base_shipping_incl_tax { get; set; }
        public int base_shipping_tax_amount { get; set; }
        public int shipping_amount { get; set; }
        public int shipping_discount_amount { get; set; }
        public int shipping_discount_tax_compensation_amount { get; set; }
        public int shipping_incl_tax { get; set; }
        public int shipping_tax_amount { get; set; }
    }

    public class Shipping
    {
        public Address address { get; set; }
        public string method { get; set; }
        public Total total { get; set; }
    }

    public class ShippingAssignment
    {
        public Shipping shipping { get; set; }
        public List<Item> items { get; set; }
    }

    public class ExtensionAttributes
    {
        public List<ShippingAssignment> shipping_assignments { get; set; }
    }

    public class Order
    {
        public string applied_rule_ids { get; set; }
        public string base_currency_code { get; set; }
        public double base_discount_amount { get; set; }
        public double base_grand_total { get; set; }
        public int base_discount_tax_compensation_amount { get; set; }
        public int base_shipping_amount { get; set; }
        public int base_shipping_discount_amount { get; set; }
        public int base_shipping_incl_tax { get; set; }
        public int base_shipping_tax_amount { get; set; }
        public int base_subtotal { get; set; }
        public int base_subtotal_incl_tax { get; set; }
        public int base_tax_amount { get; set; }
        public double base_total_due { get; set; }
        public int base_to_global_rate { get; set; }
        public int base_to_order_rate { get; set; }
        public int billing_address_id { get; set; }
        public string coupon_code { get; set; }
        public string created_at { get; set; }
        public string customer_email { get; set; }
        public int customer_group_id { get; set; }
        public int customer_is_guest { get; set; }
        public int customer_note_notify { get; set; }
        public double discount_amount { get; set; }
        public string discount_description { get; set; }
        public int email_sent { get; set; }
        public int entity_id { get; set; }
        public string global_currency_code { get; set; }
        public double grand_total { get; set; }
        public int discount_tax_compensation_amount { get; set; }
        public string increment_id { get; set; }
        public int is_virtual { get; set; }
        public string order_currency_code { get; set; }
        public string protect_code { get; set; }
        public int quote_id { get; set; }
        public string remote_ip { get; set; }
        public int shipping_amount { get; set; }
        public string shipping_description { get; set; }
        public int shipping_discount_amount { get; set; }
        public int shipping_discount_tax_compensation_amount { get; set; }
        public int shipping_incl_tax { get; set; }
        public int shipping_tax_amount { get; set; }
        public string state { get; set; }
        public string status { get; set; }
        public string store_currency_code { get; set; }
        public int store_id { get; set; }
        public string store_name { get; set; }
        public int store_to_base_rate { get; set; }
        public int store_to_order_rate { get; set; }
        public int subtotal { get; set; }
        public int subtotal_incl_tax { get; set; }
        public int tax_amount { get; set; }
        public double total_due { get; set; }
        public int total_item_count { get; set; }
        public int total_qty_ordered { get; set; }
        public string updated_at { get; set; }
        public int weight { get; set; }
        public List<Item> items { get; set; }
        public BillingAddress billing_address { get; set; }
        public Payment payment { get; set; }
        public List<object> status_histories { get; set; }
        public ExtensionAttributes extension_attributes { get; set; }
        public string customer_firstname { get; set; }
        public int? customer_id { get; set; }
        public string customer_lastname { get; set; }
        public string customer_middlename { get; set; }
    }

    public class Filter
    {
        public string field { get; set; }
        public string value { get; set; }
        public string condition_type { get; set; }
    }

    public class FilterGroup
    {
        public List<Filter> filters { get; set; }
    }

    public class SearchCriteria
    {
        public List<FilterGroup> filter_groups { get; set; }
    }

    public class OrdersResponse
    {
        [JsonProperty("items")]
        public List<Order> Orders { get; set; }
        public SearchCriteria search_criteria { get; set; }
        public int total_count { get; set; }
    }
}