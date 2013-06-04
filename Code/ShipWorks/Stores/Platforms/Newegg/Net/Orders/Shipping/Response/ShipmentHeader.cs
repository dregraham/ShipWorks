using System;
using System.Xml;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping.Response
{
    /// <summary>
    /// A data transport object in the ShippingResult tree.
    /// </summary>
    [Serializable]
    [XmlRoot("Header")]
    public class ShipmentHeader
    {

        [XmlElement("SellerID")]
        public string SellerId { get; set; }

        [XmlElement("SONumber")]
        public long OrderNumber { get; set; }
    }
}
