using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    [XmlRoot(ElementName = "StatusList")]
    public class YahooStatusList
    {
        [XmlElement(ElementName = "OrderStatus")]
        public List<YahooOrderStatus> OrderStatus { get; set; }
    }
}
