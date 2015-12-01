using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    [XmlRoot(ElementName = "OrderList")]
    public class YahooOrderList
    {
        [XmlElement(ElementName = "Order")]
        public List<YahooOrder> Order { get; set; }
    }
}
