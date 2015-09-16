using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.LemonStand.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public class LemonStandOrder
    {
        //"data": [{
        //"id": 1,
        //"shop_order_id": null,
        //"shop_order_status_id": 2,
        //"shop_customer_id": 1,
        //"status": "Paid",
        //"number": 1,
        //"is_quote": 0,
        //"is_tax_exempt": 0,
        //"total": 42.03,
        //"total_invoiced": 42.03,
        //"total_paid": 42.03,
        //"total_refunded": null,
        //"subtotal_invoiced": 39.99,
        //"subtotal_paid": 39.99,
        //"subtotal_refunded": null,
        //"total_discount": 0,
        //"total_sales_tax": 0,
        //"total_sales_tax_invoiced": 0,
        //"total_sales_tax_paid": 0,
        //"total_sales_tax_refunded": null,
        //"total_shipping_tax": 0,
        //"total_shipping_tax_invoiced": 0,
        //"total_shipping_tax_paid": 0,
        //"total_shipping_tax_refunded": null,
        //"total_shipping_quote": 2.04,
        //"total_shipping_invoiced": 2.04,
        //"total_shipping_paid": 2.04,
        //"total_shipping_refunded": null,
        //"status_updated_at": "2015-09-02T14:43:35-0700",
        //"created_by": null,
        //"updated_by": null,
        //"created_at": "2015-09-02T14:43:34-0700",
        //"updated_at": "2015-09-02T14:43:35-0700"
        //}]

        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("shop_order_id")]
        public string ShopOrderId { get; set; }

        [JsonProperty("shop_order_status_id")]
        public string ShopOrderStatusId { get; set; }

        [JsonProperty("shop_customer_id")]
        public string ShopCustomerId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("subtotal_paid")]
        public string SubtotalPaid { get; set; }

        [JsonProperty("total")]
        public string Total { get; set; }

        [JsonProperty("status_updated_at")]
        public string StatusUpdatedAt { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("invoices")]
        public LemonStandInvoice Invoices { get; set; }
    }
}