using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ShipWorks.Stores.Platforms.BigCommerce.DTO
{
    /// <summary>
    /// Order dto object that gets populated by the REST call
    /// </summary>
    [SuppressMessage("SonarQube", "S125:Remove this commented out code",
        Justification = "The commented out properties are included in the API but we don't use them")]
    public class BigCommerceOrder
    {
        public int id { get; set; }
        public long customer_id { get; set; }
        public string date_created { get; set; }
        public string date_modified { get; set; }
        //public string date_shipped { get; set; }
        public long status_id { get; set; }
        public string status { get; set; }
        //public string subtotal_ex_tax { get; set; }
        //public string subtotal_inc_tax { get; set; }
        //public string subtotal_tax { get; set; }
        //public string base_shipping_cost { get; set; }
        public string shipping_cost_ex_tax { get; set; }
        //public string shipping_cost_inc_tax { get; set; }
        //public string shipping_cost_tax { get; set; }
        //public long shipping_cost_tax_class_id { get; set; }
        //public string base_handling_cost { get; set; }
        public string handling_cost_ex_tax { get; set; }
        //public string handling_cost_inc_tax { get; set; }
        //public string handling_cost_tax { get; set; }
        //public long handling_cost_tax_class_id { get; set; }
        //public string base_wrapping_cost { get; set; }
        public string wrapping_cost_ex_tax { get; set; }
        //public string wrapping_cost_inc_tax { get; set; }
        //public string wrapping_cost_tax { get; set; }
        //public long wrapping_cost_tax_class_id { get; set; }
        //public string total_ex_tax { get; set; }
        public string total_inc_tax { get; set; }
        public string total_tax { get; set; }
        //public long items_total { get; set; }
        //public long items_shipped { get; set; }
        public string payment_method { get; set; }
        //public object payment_provider_id { get; set; }
        //public string payment_status { get; set; }
        //public string refunded_amount { get; set; }
        public bool order_is_digital { get; set; }
        public string store_credit_amount { get; set; }
        public string gift_certificate_amount { get; set; }
        //public string ip_address { get; set; }
        //public string geoip_country { get; set; }
        //public string geoip_country_iso2 { get; set; }
        //public long currency_id { get; set; }
        //public string currency_code { get; set; }
        //public string currency_exchange_rate { get; set; }
        //public long default_currency_id { get; set; }
        //public string default_currency_code { get; set; }
        public string staff_notes { get; set; }
        public string customer_message { get; set; }
        public string discount_amount { get; set; }
        //public string coupon_discount { get; set; }
        //public long shipping_address_count { get; set; }
        public bool is_deleted { get; set; }
        public BigCommerceBillingAddress billing_address { get; set; }
        public BigCommerceProducts products { get; set; }
        public BigCommerceShippingAddresses shipping_addresses { get; set; }
        public BigCommerceCoupons coupons { get; set; }

        public List<BigCommerceProduct> OrderProducts { get; set; }
        public List<BigCommerceCoupon> OrderCoupons { get; set; }
        public List<BigCommerceAddress> OrderShippingAddresses { get; set; }
        public List<BigCommerceShipment> OrderShipments { get; set; }
    }
}
