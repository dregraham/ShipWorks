using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.DTO
{
    [XmlRoot(ElementName = "ResponseResourceList")]
    public class YahooResponseResourceList
    {
        [XmlElement(ElementName = "OrderListQuery")]
        public YahooOrderListQuery OrderListQuery { get; set; }
    }
}
