using System.Collections.Generic;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Overstock.DTO
{
    [XmlRoot(ElementName = "warehouseName")]
    public class WarehouseName
    {
        [XmlElement(ElementName = "code")]
        public string Code { get; set; }
    }

    [XmlRoot(ElementName = "shipToAddress")]
    public class ShipToAddress
    {
        [XmlElement(ElementName = "contactName")]
        public string ContactName { get; set; }
        [XmlElement(ElementName = "address1")]
        public string Address1 { get; set; }
        [XmlElement(ElementName = "address2")]
        public string Address2 { get; set; }
        [XmlElement(ElementName = "city")]
        public string City { get; set; }
        [XmlElement(ElementName = "stateOrProvince")]
        public string StateOrProvince { get; set; }
        [XmlElement(ElementName = "postalCode")]
        public string PostalCode { get; set; }
        [XmlElement(ElementName = "countryCode")]
        public string CountryCode { get; set; }
        [XmlElement(ElementName = "phone")]
        public string Phone { get; set; }
    }

    [XmlRoot(ElementName = "returnAddress")]
    public class ReturnAddress
    {
        [XmlElement(ElementName = "contactName")]
        public string ContactName { get; set; }
        [XmlElement(ElementName = "address1")]
        public string Address1 { get; set; }
        [XmlElement(ElementName = "city")]
        public string City { get; set; }
        [XmlElement(ElementName = "stateOrProvince")]
        public string StateOrProvince { get; set; }
        [XmlElement(ElementName = "postalCode")]
        public string PostalCode { get; set; }
        [XmlElement(ElementName = "countryCode")]
        public string CountryCode { get; set; }
        [XmlElement(ElementName = "phone")]
        public string Phone { get; set; }
    }

    [XmlRoot(ElementName = "shippingServiceLevel")]
    public class ShippingServiceLevel
    {
        [XmlElement(ElementName = "code")]
        public string Code { get; set; }
    }

    [XmlRoot(ElementName = "smallParcelShipment")]
    public class SmallParcelShipment
    {
        [XmlElement(ElementName = "shippingServiceLevel")]
        public ShippingServiceLevel ShippingServiceLevel { get; set; }
    }

    [XmlRoot(ElementName = "shippingSpecifications")]
    public class ShippingSpecifications
    {
        [XmlElement(ElementName = "isThirdPartyBilling")]
        public string IsThirdPartyBilling { get; set; }
        [XmlElement(ElementName = "isSignatureRequired")]
        public string IsSignatureRequired { get; set; }
        [XmlElement(ElementName = "isDeclaredValueRequired")]
        public string IsDeclaredValueRequired { get; set; }
        [XmlElement(ElementName = "smallParcelShipment")]
        public SmallParcelShipment SmallParcelShipment { get; set; }
        [XmlElement(ElementName = "isExport")]
        public string IsExport { get; set; }
    }

    [XmlRoot(ElementName = "processedSalesOrderLine")]
    public class ProcessedSalesOrderLine
    {
        [XmlElement(ElementName = "salesChannelLineId")]
        public string SalesChannelLineId { get; set; }
        [XmlElement(ElementName = "salesChannelLineNumber")]
        public string SalesChannelLineNumber { get; set; }
        [XmlElement(ElementName = "partnerSKU")]
        public string PartnerSKU { get; set; }
        [XmlElement(ElementName = "barcode")]
        public string Barcode { get; set; }
        [XmlElement(ElementName = "salesChannelSKU")]
        public string SalesChannelSKU { get; set; }
        [XmlElement(ElementName = "quantity")]
        public string Quantity { get; set; }
        [XmlElement(ElementName = "itemPrice")]
        public string ItemPrice { get; set; }
        [XmlElement(ElementName = "lineId")]
        public string LineId { get; set; }
        [XmlElement(ElementName = "itemId")]
        public string ItemId { get; set; }
        [XmlElement(ElementName = "itemName")]
        public string ItemName { get; set; }
        [XmlElement(ElementName = "lineStatus")]
        public string LineStatus { get; set; }
        [XmlElement(ElementName = "unitCost")]
        public string UnitCost { get; set; }
        [XmlElement(ElementName = "unitCostCurrencyCode")]
        public string UnitCostCurrencyCode { get; set; }
        [XmlElement(ElementName = "firstCost")]
        public string FirstCost { get; set; }
        [XmlElement(ElementName = "firstCostCurrencyCode")]
        public string FirstCostCurrencyCode { get; set; }
        [XmlElement(ElementName = "additionalShippingCost")]
        public string AdditionalShippingCost { get; set; }
        [XmlElement(ElementName = "additionalShippingCostCurrencyCode")]
        public string AdditionalShippingCostCurrencyCode { get; set; }
    }

    [XmlRoot(ElementName = "list")]
    public class OverstockOrderDto
    {
        [XmlElement(ElementName = "salesChannelOrderNumber")]
        public string SalesChannelOrderNumber { get; set; }
        [XmlElement(ElementName = "salesChannelName")]
        public string SalesChannelName { get; set; }
        [XmlElement(ElementName = "orderDate")]
        public string OrderDate { get; set; }
        [XmlElement(ElementName = "sofsCreatedDate")]
        public string SofsCreatedDate { get; set; }
        [XmlElement(ElementName = "warehouseName")]
        public WarehouseName WarehouseName { get; set; }
        [XmlElement(ElementName = "shipToAddress")]
        public ShipToAddress ShipToAddress { get; set; }
        [XmlElement(ElementName = "returnAddress")]
        public ReturnAddress ReturnAddress { get; set; }
        [XmlElement(ElementName = "shippingSpecifications")]
        public ShippingSpecifications ShippingSpecifications { get; set; }
        [XmlElement(ElementName = "branding")]
        public string Branding { get; set; }
        [XmlElement(ElementName = "orderFulfillment")]
        public string OrderFulfillment { get; set; }
        [XmlElement(ElementName = "orderId")]
        public string OrderId { get; set; }
        [XmlElement(ElementName = "status")]
        public string Status { get; set; }
        [XmlElement(ElementName = "retailChannelCode")]
        public string RetailChannelCode { get; set; }
        [XmlElement(ElementName = "retailerOrderCode")]
        public string RetailerOrderCode { get; set; }
        [XmlElement(ElementName = "retailOrderNumber")]
        public string RetailOrderNumber { get; set; }
        [XmlElement(ElementName = "actionRequired")]
        public string ActionRequired { get; set; }
        [XmlElement(ElementName = "processedSalesOrderLine")]
        public ProcessedSalesOrderLine ProcessedSalesOrderLine { get; set; }
    }

    [XmlRoot(ElementName = "processedSalesOrderTypeList")]
    public class ProcessedSalesOrderTypeList
    {
        [XmlElement(ElementName = "totalCount")]
        public string TotalCount { get; set; }

        [XmlElement(ElementName = "list")]
        public List<OverstockOrderDto> List { get; set; }

        [XmlAttribute(AttributeName = "ns2", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Ns2 { get; set; }
    }
}
