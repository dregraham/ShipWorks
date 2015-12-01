using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    [XmlRoot(ElementName = "ErrorResourceList")]
    public class YahooErrorResourceList
    {
        [XmlElement(ElementName = "Error")]
        public List<YahooError> Error { get; set; }
    }

    public class YahooError
    {
        [XmlElement(ElementName = "Code")]
        public long Code { get; set; }
        [XmlElement(ElementName = "Message")]
        public string Message { get; set; }
    }
}
