using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    [XmlRoot(ElementName = "Promotions")]
    public class YahooPromotions
    {
        [XmlElement(ElementName = "AppliedPromotion")]
        public List<YahooAppliedPromotion> AppliedPromotion { get; set; }
    }
}
