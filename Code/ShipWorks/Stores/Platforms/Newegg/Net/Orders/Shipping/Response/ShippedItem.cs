using System;
using System.Xml;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping.Response
{
    /// <summary>
    /// A data transport object in the ShippingResult tree containing information
    /// about the items shipped.
    /// </summary>
    [Serializable]
    [XmlRoot("ItemDes")]
    public class ShippedItem
    {
        public ShippedItem()
        { }

        [XmlElement("NeweggItemNumber")]
        public string NeweggItemNumber { get; set; }

        [XmlElement("SellerPartNumber")]
        public string SellerPartNumber { get; set; }

        [XmlElement("ShippedQty")]
        public int QuantityShipped { get; set; }
    }
}
