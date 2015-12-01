using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    [XmlRoot(ElementName = "Item")]
    public class YahooCatalogItem
    {
        [XmlElement(ElementName = "ShipWeight")]
        public double ShipWeight { get; set; }
    }
}