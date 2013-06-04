using System;
using System.Xml;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping.Response
{
    /// <summary>
    /// A data transport object in the ShippingResult tree containing summary information 
    /// about the packages within an uploaded shipment.
    /// </summary>
    [Serializable]
    [XmlRoot("PackageProcessingSummary")]
    public class PackageProcessingSummary
    {
        [XmlElement("TotalPackageCount")]
        public int TotalPackages { get; set; }

        [XmlElement("SuccessCount")]
        public int SuccessCount { get; set; }

        [XmlElement("FailCount")]
        public int FailedCount { get; set; }
    }
}
