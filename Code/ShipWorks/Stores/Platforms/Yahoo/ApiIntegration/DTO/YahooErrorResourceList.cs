using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [XmlRoot(ElementName = "ErrorResourceList")]
    public class YahooErrorResourceList
    {
        [XmlElement(ElementName = "Error")]
        public Error Error { get; set; }
    }

    public class Error
    {
        [XmlElement(ElementName = "Code")]
        public string Code { get; set; }
        [XmlElement(ElementName = "Message")]
        public string Message { get; set; }
    }
}
