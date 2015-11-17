using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [XmlRoot(ElementName = "AppliedPromotion")]
    public class YahooAppliedPromotion
    {
        [XmlElement(ElementName = "Id")]
        public string Id { get; set; }

        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "Type")]
        public string Type { get; set; }

        [XmlElement(ElementName = "Discount")]
        public decimal Discount { get; set; }

        [XmlElement(ElementName = "Message")]
        public string Message { get; set; }
    }
}
