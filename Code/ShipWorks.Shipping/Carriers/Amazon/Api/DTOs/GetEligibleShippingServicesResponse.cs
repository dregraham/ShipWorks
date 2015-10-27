using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ShipWorks.Shipping.Carriers.Amazon.Enums;

namespace ShipWorks.Shipping.Carriers.Amazon.Api.DTOs
{
    [XmlRoot(ElementName = "GetEligibleShippingServicesResponse", Namespace = "https://mws.amazonservices.com/MerchantFulfillment/2015-06-01")]
    public class GetEligibleShippingServicesResponse
    {
        [XmlElement(ElementName = "GetEligibleShippingServicesResult")]
        public GetEligibleShippingServicesResult GetEligibleShippingServicesResult { get; set; }
    }

    [XmlRoot(ElementName = "GetEligibleShippingServicesResult")]
    public class GetEligibleShippingServicesResult
    {
        [XmlElement(ElementName = "ShippingServiceList")]
        public ShippingServiceList ShippingServiceList { get; set; }

        [XmlElement(ElementName = "TemporarilyUnavailableCarrierList")]
        public TemporarilyUnavailableCarrierList TemporarilyUnavailableCarrierList { get; set; }

        [XmlElement(ElementName = "TermsAndConditionsNotAcceptedCarrierList")]
        public List<TermsAndConditionsNotAcceptedCarrier> TermsAndConditionsNotAcceptedCarrierList { get; set; }
    }

    [XmlRoot(ElementName = "Rate")]
    public class Rate
    {
        [XmlElement(ElementName = "Amount")]
        public decimal Amount { get; set; }

        [XmlElement(ElementName = "CurrencyCode")]
        public string CurrencyCode { get; set; }
    }

    [XmlRoot(ElementName = "ShippingServiceList")]
    public class ShippingServiceList
    {
        [XmlElement(ElementName = "ShippingService")]
        public List<ShippingService> ShippingService { get; set; }
    }

    [XmlRoot(ElementName = "ShippingService")]
    public class ShippingService
    {
        [XmlElement(ElementName = "CarrierName")]
        public string CarrierName { get; set; }

        [XmlElement(ElementName = "ShippingServiceOptions")]
        public ShippingServiceOptions ShippingServiceOptions { get; set; }

        [XmlElement(ElementName = "ShippingServiceId")]
        public string ShippingServiceId { get; set; }

        [XmlElement(ElementName = "Rate")]
        public Rate Rate { get; set; }

        [XmlElement(ElementName = "LatestEstimatedDeliveryDate")]
        public string LatestEstimatedDeliveryDate { get; set; }

        [XmlElement(ElementName = "EarliestEstimatedDeliveryDate")]
        public string EarliestEstimatedDeliveryDate { get; set; }
        
        [XmlElement(ElementName = "ShippingServiceOfferId")]
        public string ShippingServiceOfferId { get; set; }
        
        [XmlElement(ElementName = "ShipDate")]
        public string ShipDate { get; set; }
        
        [XmlElement(ElementName = "ShippingServiceName")]
        public string ShippingServiceName { get; set; }
    }

    [XmlRoot(ElementName = "ShippingServiceOptions")]
    public class ShippingServiceOptions
    {
        [XmlElement(ElementName = "CarrierWillPickUp")]
        public bool CarrierWillPickUp { get; set; }

        [XmlElement(ElementName = "DeclaredValue")]
        public DeclaredValue DeclaredValue { get; set; }

        [XmlElement(ElementName = "DeliveryExperience")]
        public string DeliveryExperience { get; set; }
    }

    [XmlRoot(ElementName = "DeclaredValue")]
    public class DeclaredValue
    {
        [XmlElement(ElementName = "CurrencyCode")]
        public string CurrencyCode { get; set; }

        [XmlElement(ElementName = "Amount")]
        public decimal Amount { get; set; }
    }

    [XmlRoot(ElementName = "TemporarilyUnavailableCarrier")]
    public class TemporarilyUnavailableCarrier
    {
        [XmlElement(ElementName = "CarrierName")]
        public string CarrierName { get; set; }
    }

    [XmlRoot(ElementName = "TemporarilyUnavailableCarrierList")]
    public class TemporarilyUnavailableCarrierList
    {
        [XmlElement(ElementName = "TemporarilyUnavailableCarrier")]
        public List<TemporarilyUnavailableCarrier> TemporarilyUnavailableCarrier { get; set; }
    }

    [XmlRoot(ElementName = "TermsAndConditionsNotAcceptedCarrier")]
    public class TermsAndConditionsNotAcceptedCarrier
    {
        [XmlElement(ElementName = "CarrierName")]
        public List<string> CarrierName { get; set; }
    }

    [XmlRoot(ElementName = "TermsAndConditionsNotAcceptedCarrierList")]
    public class TermsAndConditionsNotAcceptedCarrierList
    {
        [XmlElement(ElementName = "TermsAndConditionsNotAcceptedCarrier")]
        public TermsAndConditionsNotAcceptedCarrier TermsAndConditionsNotAcceptedCarrier { get; set; }
    }
}