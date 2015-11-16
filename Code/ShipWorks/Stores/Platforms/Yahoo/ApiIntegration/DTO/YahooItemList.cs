using System.Collections.Generic;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [XmlRoot(ElementName = "ItemList")]
    public class YahooItemList
    {
        [XmlElement(ElementName = "Item")]
        public List<YahooItem> Item { get; set; }
    }
}
