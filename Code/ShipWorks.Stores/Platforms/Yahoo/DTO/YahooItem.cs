﻿using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.DTO
{
    [XmlRoot(ElementName = "Item")]
    public class YahooItem
    {
        [XmlElement(ElementName = "LineNumber")]
        public string LineNumber { get; set; }

        [XmlElement(ElementName = "ItemID")]
        public string ItemID { get; set; }

        [XmlElement(ElementName = "ItemCode")]
        public string ItemCode { get; set; }

        [XmlElement(ElementName = "Quantity")]
        public string Quantity { get; set; }

        [XmlElement(ElementName = "UnitPrice")]
        public string UnitPrice { get; set; }

        [XmlElement(ElementName = "Description")]
        public string Description { get; set; }

        [XmlElement(ElementName = "URL")]
        public string URL { get; set; }

        [XmlElement(ElementName = "Taxable")]
        public string Taxable { get; set; }

        [XmlElement(ElementName = "ThumbnailURL")]
        public string ThumbnailUrl { get; set; }
    }
}
