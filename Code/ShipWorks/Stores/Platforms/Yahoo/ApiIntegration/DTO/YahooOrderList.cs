using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [XmlRoot(ElementName = "OrderList")]
    public class YahooOrderList
    {
        [XmlElement(ElementName = "Order")]
        public List<YahooOrder> Order { get; set; }
    }
}
