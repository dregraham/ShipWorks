using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [XmlRoot(ElementName = "ShipToInfo")]
    public class YahooShipToInfo
    {
        [XmlElement(ElementName = "GeneralInfo")]
        public YahooGeneralInfo GeneralInfo { get; set; }

        [XmlElement(ElementName = "AddressInfo")]
        public YahooAddressInfo AddressInfo { get; set; }
    }
}
