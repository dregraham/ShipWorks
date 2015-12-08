using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    [XmlRoot(ElementName = "ResponseResourceList")]
    public class YahooResponseResourceList
    {
        [XmlElement(ElementName = "OrderListQuery")]
        public YahooOrderListQuery OrderListQuery { get; set; }

        [XmlElement(ElementName = "OrderList")]
        public YahooOrderList OrderList { get; set; }

        [XmlElement(ElementName = "Catalog")]
        public YahooCatalog Catalog { get; set; }

        [XmlElement(ElementName = "CustomOrderStatusList")]
        public YahooCustomOrderStatusList CustomOrderStatusList { get; set; }
    }
}
