﻿using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.DTO
{
    [XmlRoot(ElementName = "OrderStatus")]
    public class YahooOrderStatus
    {
        [XmlElement(ElementName = "StatusID")]
        public string StatusID { get; set; }
    }
}
