﻿using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [XmlRoot(ElementName = "ResponseResourceList")]
    public class YahooResponseResourceList
    {
        [XmlElement(ElementName = "OrderListQuery")]
        public YahooOrderListQuery OrderListQuery { get; set; }

        [XmlElement(ElementName = "OrderList")]
        public YahooOrderList OrderList { get; set; }
    }
}
