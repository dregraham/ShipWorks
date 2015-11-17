using System;
using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Amazon.Api.DTOs
{
    public class ShipmentRequestDetails
    {
        public string AmazonOrderId { get; set; }
        public List<Item> ItemList { get; set; }
        public Address ShipFromAddress { get; set; }
        public PackageDimensions PackageDimensions { get; set; }
        public double Weight { get; set; }
        public CurrencyAmount Insurance { get; set; }
        public DateTime? MustArriveByDate { get; set; }
        public bool SendDateMustArriveBy { get; set; }
        public ShippingServiceOptions ShippingServiceOptions { get; set; }
    }

    public class CurrencyAmount
    {
        public string CurrencyCode { get; set; }
        public decimal Amount { get; set; }
    }

    public class PackageDimensions
    {
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }

    public class Item
    {
        public string OrderItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class Address
    {
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string StateOrProvinceCode { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
