using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.DTO
{
    [XmlRoot(ElementName = "BillToInfo")]
    public class YahooBillToInfo
    {
        [XmlElement(ElementName = "GeneralInfo")]
        public YahooGeneralInfo GeneralInfo { get; set; }

        [XmlElement(ElementName = "AddressInfo")]
        public YahooAddressInfo AddressInfo { get; set; }
    }
}
