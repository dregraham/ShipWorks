using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    [XmlRoot(ElementName = "OrderListQuery")]
    public class YahooOrderListQuery
    {
        [XmlElement(ElementName = "Order")]
        public List<YahooOrder> YahooOrders { get; set; }
    }
}
