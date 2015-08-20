using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.Amazon.Api.DTOs
{
    public class ShipmentRequestDetails
    {
        public string AmazonOrderId { get; set; }
        public List<Item> ItemList { get; set; }
        public Address ShipFromAddress { get; set; }
        public PackageDimensions PackageDimensions { get; set; }
        public Weight Weight { get; set; }
        public CurrencyAmount Insurance { get; set; }
        public DateTime? MustArriveByDate { get; set; }
        public ShippingServiceOptions ShippingServiceOptions { get; set; }
    }

    public class CurrencyAmount
    {
        public string CurrencyCode { get; set; }
        public decimal Amount { get; set; }
    }

    public class Weight
    {
        public decimal Value { get; set; }
        public Unit Unit { get; set; }
    }

    public enum Unit
    {
        ounces,
        grams
    }

    public class PackageDimensions
    {}

    public class Item
    {
        
    }

    public class Address
    {
        
    }
}
