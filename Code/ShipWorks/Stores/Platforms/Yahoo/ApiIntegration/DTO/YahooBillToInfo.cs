using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    [XmlRoot(ElementName = "BillToInfo")]
    public class YahooBillToInfo
    {
        [XmlElement(ElementName = "GeneralInfo")]
        public YahooGeneralInfo GeneralInfo { get; set; }

        [XmlElement(ElementName = "AddressInfo")]
        public YahooAddressInfo AddressInfo { get; set; }
    }
}
