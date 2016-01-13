using System.Xml.Serialization;


namespace ShipWorks.Shipping.Carriers.Amazon.Api.DTOs
{
    [XmlRoot(ElementName = "CreateShipmentResponse", Namespace = "https://mws.amazonservices.com/MerchantFulfillment/2015-06-01")]
    public class CreateShipmentResponse
    {
        [XmlElement(ElementName = "CreateShipmentResult")]
        public CreateShipmentResult CreateShipmentResult { get; set; }
        [XmlElement(ElementName = "ResponseMetadata")]
        public ResponseMetadata ResponseMetadata { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
    
    [XmlRoot(ElementName = "CreateShipmentResult")]
    public class CreateShipmentResult
    {
        [XmlElement(ElementName = "Shipment")]
        public AmazonShipment AmazonShipment { get; set; }
    }
    
    [XmlRoot(ElementName = "Shipment")]
    public class AmazonShipment
    {
        [XmlElement(ElementName = "ShipmentId")]
        public string ShipmentId { get; set; }
        [XmlElement(ElementName = "AmazonOrderId")]
        public string AmazonOrderId { get; set; }
        [XmlElement(ElementName = "SellerOrderId")]
        public string SellerOrderId { get; set; }
        [XmlElement(ElementName = "ItemList")]
        public ItemList ItemList { get; set; }
        [XmlElement(ElementName = "ShipFromAddress")]
        public ShipFromAddress ShipFromAddress { get; set; }
        [XmlElement(ElementName = "ShipToAddress")]
        public ShipToAddress ShipToAddress { get; set; }
        [XmlElement(ElementName = "PackageDimensions")]
        public PackageDimensions PackageDimensions { get; set; }
        [XmlElement(ElementName = "Weight")]
        public Weight Weight { get; set; }
        [XmlElement(ElementName = "Insurance")]
        public Insurance Insurance { get; set; }
        [XmlElement(ElementName = "ShippingService")]
        public ShippingService ShippingService { get; set; }
        [XmlElement(ElementName = "Label")]
        public Label Label { get; set; }
        [XmlElement(ElementName = "Status")]
        public string Status { get; set; }
        [XmlElement(ElementName = "TrackingId")]
        public string TrackingId { get; set; }
        [XmlElement(ElementName = "CreatedDate")]
        public string CreatedDate { get; set; }
        [XmlElement(ElementName = "LastUpdatedDate")]
        public string LastUpdatedDate { get; set; }
    }

    [XmlRoot(ElementName = "ItemList")]
    public class ItemList
    {
        [XmlElement(ElementName = "Item")]
        public Item Item { get; set; }
    }

    [XmlRoot(ElementName = "ShipFromAddress")]
    public class ShipFromAddress
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "AddressLine1")]
        public string AddressLine1 { get; set; }
        [XmlElement(ElementName = "AddressLine2")]
        public string AddressLine2 { get; set; }
        [XmlElement(ElementName = "AddressLine3")]
        public string AddressLine3 { get; set; }
        [XmlElement(ElementName = "DistrictOrCounty")]
        public string DistrictOrCounty { get; set; }
        [XmlElement(ElementName = "Email")]
        public string Email { get; set; }
        [XmlElement(ElementName = "City")]
        public string City { get; set; }
        [XmlElement(ElementName = "StateOrProvinceCode")]
        public string StateOrProvinceCode { get; set; }
        [XmlElement(ElementName = "PostalCode")]
        public string PostalCode { get; set; }
        [XmlElement(ElementName = "CountryCode")]
        public string CountryCode { get; set; }
        [XmlElement(ElementName = "Phone")]
        public string Phone { get; set; }
    }

    [XmlRoot(ElementName = "ShipToAddress")]
    public class ShipToAddress
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "AddressLine1")]
        public string AddressLine1 { get; set; }
        [XmlElement(ElementName = "AddressLine2")]
        public string AddressLine2 { get; set; }
        [XmlElement(ElementName = "AddressLine3")]
        public string AddressLine3 { get; set; }
        [XmlElement(ElementName = "DistrictOrCounty")]
        public string DistrictOrCounty { get; set; }
        [XmlElement(ElementName = "Email")]
        public string Email { get; set; }
        [XmlElement(ElementName = "City")]
        public string City { get; set; }
        [XmlElement(ElementName = "StateOrProvinceCode")]
        public string StateOrProvinceCode { get; set; }
        [XmlElement(ElementName = "PostalCode")]
        public string PostalCode { get; set; }
        [XmlElement(ElementName = "CountryCode")]
        public string CountryCode { get; set; }
        [XmlElement(ElementName = "Phone")]
        public string Phone { get; set; }
    }

    [XmlRoot(ElementName = "Weight")]
    public class Weight
    {
        [XmlElement(ElementName = "Value")]
        public string Value { get; set; }
        [XmlElement(ElementName = "Unit")]
        public string Unit { get; set; }
    }

    [XmlRoot(ElementName = "Insurance")]
    public class Insurance
    {
        [XmlElement(ElementName = "CurrencyCode")]
        public string CurrencyCode { get; set; }
        [XmlElement(ElementName = "Amount")]
        public string Amount { get; set; }
    }

    [XmlRoot(ElementName = "Dimensions")]
    public class Dimensions
    {
        [XmlElement(ElementName = "Length")]
        public string Length { get; set; }
        [XmlElement(ElementName = "Width")]
        public string Width { get; set; }
        [XmlElement(ElementName = "Unit")]
        public string Unit { get; set; }
    }

    [XmlRoot(ElementName = "FileContents")]
    public class FileContents
    {
        [XmlElement(ElementName = "Contents")]
        public string Contents { get; set; }
        [XmlElement(ElementName = "FileType")]
        public string FileType { get; set; }
        [XmlElement(ElementName = "Checksum")]
        public string Checksum { get; set; }
    }

    [XmlRoot(ElementName = "Label")]
    public class Label
    {
        [XmlElement(ElementName = "Dimensions")]
        public Dimensions Dimensions { get; set; }
        [XmlElement(ElementName = "FileContents")]
        public FileContents FileContents { get; set; }
    }
    
    [XmlRoot(ElementName = "ResponseMetadata")]
    public class ResponseMetadata
    {
        [XmlElement(ElementName = "RequestId")]
        public string RequestId { get; set; }
    }
}
