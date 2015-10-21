using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.DTO
{
    [XmlRoot(ElementName = "Promotions")]
    public class YahooPromotions
    {
        [XmlElement(ElementName = "AppliedPromotion")]
        public YahooAppliedPromotion AppliedPromotion { get; set; }
    }
}
