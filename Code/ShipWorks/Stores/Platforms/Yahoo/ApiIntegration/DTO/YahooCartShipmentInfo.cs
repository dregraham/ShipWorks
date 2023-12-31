﻿using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    [XmlRoot(ElementName = "CartShipmentInfo")]
    public class YahooCartShipmentInfo
    {
        [XmlElement(ElementName = "ShipState")]
        public string ShipState { get; set; }

        [XmlElement(ElementName = "TrackingNumber")]
        public string TrackingNumber { get; set; }

        [XmlElement(ElementName = "Shipper")]
        public string Shipper { get; set; }
    }
}
