using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    [XmlRoot(ElementName = "CustomOrderStatusList")]
    public class YahooCustomOrderStatusList
    {
        [XmlElement(ElementName = "CustomOrderStatus")]
        public List<YahooCustomOrderStatus> CustomOrderStatus { get; set; }
    }
}