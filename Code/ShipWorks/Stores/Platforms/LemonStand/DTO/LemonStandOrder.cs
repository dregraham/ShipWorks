using Newtonsoft.Json;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.LemonStand.DTO
{
    class LemonStandOrder
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
        public string OrderId { get; set; }
    }
}
