using System.Collections.Generic;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [XmlRoot(ElementName = "OrderListQuery")]
    public class YahooOrderListQuery
    {
        [XmlElement(ElementName = "Order")]
        public List<YahooOrder> YahooOrders { get; set; }
    }
}
