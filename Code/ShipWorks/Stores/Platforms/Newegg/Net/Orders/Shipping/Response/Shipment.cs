using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping.Response
{
    /// <summary>
    /// A data transport object in the ShippingResult tree containing the packages
    /// that were uploaded to Newegg.
    /// </summary>
    [Serializable]
    [XmlRoot("Shipment")]
    public class Shipment
    {
        public Shipment()
        {
            Header = new ShipmentHeader();
            Packages = new List<ShipmentPackage>();
        }

        [XmlElement("Header")]
        public ShipmentHeader Header { get; set; }

        [XmlArray("PackageList")]
        [XmlArrayItem("Package")]
        public List<ShipmentPackage> Packages { get; set; }

    }
}
