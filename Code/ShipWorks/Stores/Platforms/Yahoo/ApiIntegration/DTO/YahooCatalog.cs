using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    [XmlRoot(ElementName = "Catalog")]
    public class YahooCatalog
    {
        [XmlElement(ElementName = "ItemList")]
        public YahooCatalogItemList ItemList { get; set; }
    }
}