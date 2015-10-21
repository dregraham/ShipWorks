using System.Collections.Generic;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.DTO
{
    [XmlRoot(ElementName = "StatusList")]
    public class YahooStatusList
    {
        [XmlElement(ElementName = "OrderStatus")]
        public List<YahooOrderStatus> OrderStatus { get; set; }
    }
}
