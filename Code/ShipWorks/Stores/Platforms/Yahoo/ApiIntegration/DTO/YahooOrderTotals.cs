using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [XmlRoot(ElementName = "OrderTotals")]
    public class YahooOrderTotals
    {
        [XmlElement(ElementName = "Subtotal")]
        public decimal Subtotal { get; set; }

        [XmlElement(ElementName = "Shipping")]
        public decimal Shipping { get; set; }

        [XmlElement(ElementName = "Tax")]
        public decimal Tax { get; set; }

        [XmlElement(ElementName = "Promotions")]
        public YahooPromotions Promotions { get; set; }

        [XmlElement(ElementName = "Total")]
        public decimal Total { get; set; }

        [XmlElement(ElementName = "GiftWrap")]
        public decimal GiftWrap { get; set; }
    }
}
