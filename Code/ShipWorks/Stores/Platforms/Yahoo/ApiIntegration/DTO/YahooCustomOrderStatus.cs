using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    [XmlRoot(ElementName = "CustomOrderStatus")]
    public class YahooCustomOrderStatus
    {
        [XmlElement(ElementName = "StatusID")]
        public int StatusID { get; set; }

        [XmlElement(ElementName = "Code")]
        public string Code { get; set; }
    }
}