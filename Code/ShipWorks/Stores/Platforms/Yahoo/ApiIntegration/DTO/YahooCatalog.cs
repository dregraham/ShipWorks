using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [XmlRoot(ElementName = "Catalog")]
    public class YahooCatalog
    {
        [XmlElement(ElementName = "ItemList")]
        public YahooCatalogItemList ItemList { get; set; }
    }
}