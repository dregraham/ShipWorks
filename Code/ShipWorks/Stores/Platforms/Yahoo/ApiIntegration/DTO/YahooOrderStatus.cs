using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    [XmlRoot(ElementName = "OrderStatus")]
    public class YahooOrderStatus
    {
        [XmlElement(ElementName = "StatusID")]
        public string StatusID { get; set; }
    }
}
