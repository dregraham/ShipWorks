using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ShipWorks.Shipping.Carriers.Amazon.Enums;

namespace ShipWorks.Shipping.Carriers.Amazon.Api.DTOs
{
    [Serializable]
    [XmlRoot("GetEligibleShippingServicesResponse", Namespace = "https://mws.amazonservices.com/MerchantFulfillment/2015-06-01")]
    public class GetEligibleShippingServicesResponse
    {
        [XmlElement("GetEligibleShippingServicesResult")]
        public GetEligibleShippingServicesResult GetEligibleShippingServicesResult { get; set; }
    }

    [Serializable]
    [XmlRoot("GetEligibleShippingServicesResult")]
    public class GetEligibleShippingServicesResult
    {
        [XmlElement("ShippingServiceList")]
        public ShippingServiceList ShippingServiceList { get; set; }

        [XmlElement("TemporarilyUnavailableCarrierList")]
        public TemporarilyUnavailableCarrierList TemporarilyUnavailableCarrierList { get; set; }

        [XmlElement("TermsAndConditionsNotAcceptedCarrierList")]
        public TermsAndConditionsNotAcceptedCarrierList TermsAndConditionsNotAcceptedCarrierList { get; set; }
    }

    [Serializable]
    [XmlRoot("Rate")]
    public class Rate
    {
        [XmlElement("Amount")]
        public decimal Amount { get; set; }

        [XmlElement("CurrencyCode")]
        public string CurrencyCode { get; set; }
    }

    [Serializable]
    [XmlRoot("ShippingServiceList")]
    public class ShippingServiceList
    {
        [XmlElement("ShippingService")]
        public List<ShippingService> ShippingService { get; set; }
    }

    [Serializable]
    [XmlRoot("ShippingService")]
    public class ShippingService
    {
        [XmlElement("CarrierName")]
        public string CarrierName { get; set; }

        [XmlElement("ShippingServiceOptions")]
        public ShippingServiceOptions ShippingServiceOptions { get; set; }

        [XmlElement("ShippingServiceId")]
        public string ShippingServiceId { get; set; }

        [XmlElement("Rate")]
        public Rate Rate { get; set; }

        [XmlElement("LatestEstimatedDeliveryDate")]
        public string LatestEstimatedDeliveryDate { get; set; }

        [XmlElement("EarliestEstimatedDeliveryDate")]
        public string EarliestEstimatedDeliveryDate { get; set; }
        
        [XmlElement("ShippingServiceOfferId")]
        public string ShippingServiceOfferId { get; set; }
        
        [XmlElement("ShipDate")]
        public string ShipDate { get; set; }
        
        [XmlElement("ShippingServiceName")]
        public string ShippingServiceName { get; set; }
    }

    [Serializable]
    [XmlRoot("ShippingServiceOptions")]
    public class ShippingServiceOptions
    {
        [XmlElement("CarrierWillPickUp")]
        public bool CarrierWillPickUp { get; set; }

        [XmlElement("DeclaredValue")]
        public DeclaredValue DeclaredValue { get; set; }

        [XmlElement("DeliveryExperience")]
        public string DeliveryExperience { get; set; }
    }

    [Serializable]
    [XmlRoot("DeclaredValue")]
    public class DeclaredValue
    {
        [XmlElement("CurrencyCode")]
        public string CurrencyCode { get; set; }

        [XmlElement("Amount")]
        public decimal Amount { get; set; }
    }

    [Serializable]
    [XmlRoot("TemporarilyUnavailableCarrier")]
    public class TemporarilyUnavailableCarrier
    {
        [XmlElement("CarrierName")]
        public string CarrierName { get; set; }
    }

    [Serializable]
    [XmlRoot("TemporarilyUnavailableCarrierList")]
    public class TemporarilyUnavailableCarrierList
    {
        [XmlElement("TemporarilyUnavailableCarrier")]
        public List<TemporarilyUnavailableCarrier> TemporarilyUnavailableCarrier { get; set; }
    }

    [Serializable]
    [XmlRoot("TermsAndConditionsNotAcceptedCarrier")]
    public class TermsAndConditionsNotAcceptedCarrier
    {
        [XmlElement("CarrierName")]
        public List<string> CarrierName { get; set; }
    }

    [Serializable]
    [XmlRoot("TermsAndConditionsNotAcceptedCarrierList")]
    public class TermsAndConditionsNotAcceptedCarrierList
    {
        [XmlElement("TermsAndConditionsNotAcceptedCarrier")]
        public TermsAndConditionsNotAcceptedCarrier TermsAndConditionsNotAcceptedCarrier { get; set; }
    }
}