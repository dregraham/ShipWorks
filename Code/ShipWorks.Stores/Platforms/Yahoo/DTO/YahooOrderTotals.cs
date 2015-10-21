﻿using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.DTO
{
    [XmlRoot(ElementName = "OrderTotals")]
    public class YahooOrderTotals
    {
        [XmlElement(ElementName = "Subtotal")]
        public string Subtotal { get; set; }

        [XmlElement(ElementName = "Shipping")]
        public string Shipping { get; set; }

        [XmlElement(ElementName = "Tax")]
        public string Tax { get; set; }

        [XmlElement(ElementName = "Promotions")]
        public YahooPromotions Promotions { get; set; }

        [XmlElement(ElementName = "Total")]
        public string Total { get; set; }
    }
}
