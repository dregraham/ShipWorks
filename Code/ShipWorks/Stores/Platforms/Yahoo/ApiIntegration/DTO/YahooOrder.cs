using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    [XmlRoot(ElementName = "Order")]
    public class YahooOrder
    {
        [XmlElement(ElementName = "OrderID")]
        public string OrderIDTransport { get; set; }

        [XmlIgnore]
        public long OrderID {
            get
            {
                long result;
                long.TryParse(OrderIDTransport, out result);
                return result;
            }
        }

        [XmlElement(ElementName = "CreationTime")]
        public string CreationTime { get; set; }

        [XmlElement(ElementName = "StatusList")]
        public YahooStatusList StatusList { get; set; }

        [XmlElement(ElementName = "ShipMethod")]
        public string ShipMethod { get; set; }

        [XmlElement(ElementName = "CartShipmentInfo")]
        public YahooCartShipmentInfo CartShipmentInfo { get; set; }

        [XmlElement(ElementName = "ShipToInfo")]
        public YahooShipToInfo ShipToInfo { get; set; }

        [XmlElement(ElementName = "BillToInfo")]
        public YahooBillToInfo BillToInfo { get; set; }

        [XmlElement(ElementName = "ItemList")]
        public YahooItemList ItemList { get; set; }

        [XmlElement(ElementName = "OrderTotals")]
        public YahooOrderTotals OrderTotals { get; set; }

        [XmlElement(ElementName = "MerchantNotes")]
        public string MerchantNotes { get; set; }

        [XmlElement(ElementName = "BuyerComments")]
        public string BuyerComments { get; set; }

        [XmlElement(ElementName = "GiftMessage")]
        public string GiftMessage { get; set; }

        [XmlElement(ElementName = "PaymentType")]
        public string PaymentType { get; set; }

        [XmlElement(ElementName = "LastUpdatedTime")]
        public string LastUpdatedTime { get; set; }
    }
}
