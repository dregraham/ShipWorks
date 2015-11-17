using System.Collections.Generic;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [XmlRoot(ElementName = "ItemList")]
    public class YahooCatalogItemList
    {
        [XmlElement(ElementName = "Item")]
        public List<YahooCatalogItem> Item { get; set; }
    }
}