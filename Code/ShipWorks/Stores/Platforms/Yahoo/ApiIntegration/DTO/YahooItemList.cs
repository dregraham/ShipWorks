using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    [XmlRoot(ElementName = "ItemList")]
    public class YahooItemList
    {
        [XmlElement(ElementName = "Item")]
        public List<YahooItem> Item { get; set; }
    }
}
