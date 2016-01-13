using System.Xml.Serialization;

namespace ShipWorks.Shipping.Carriers.Amazon.Api.DTOs
{
    [XmlRoot(ElementName = "CancelShipmentResponse", Namespace = "https://mws.amazonservices.com/MerchantFulfillment/2015-06-01")]
    public class CancelShipmentResponse
    {
        [XmlElement(ElementName = "CancelShipmentResult")]
        public CancelShipmentResult CancelShipmentResult { get; set; }
        [XmlElement(ElementName = "ResponseMetadata")]
        public ResponseMetadata ResponseMetadata { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
    
    [XmlRoot(ElementName = "CancelShipmentResult")]
    public class CancelShipmentResult
    {
        [XmlElement(ElementName = "Shipment")]
        public AmazonShipment AmazonShipment { get; set; }
    }

}
