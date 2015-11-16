using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [XmlRoot(ElementName = "Promotions")]
    public class YahooPromotions
    {
        [XmlElement(ElementName = "AppliedPromotion")]
        public YahooAppliedPromotion AppliedPromotion { get; set; }
    }
}
