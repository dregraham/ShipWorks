using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [XmlRoot(ElementName = "Item")]
    public class YahooCatalogItem
    {
        [XmlElement(ElementName = "ShipWeight")]
        public double ShipWeight { get; set; }
    }
}