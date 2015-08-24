using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ShipWorks.Shipping.Carriers.Amazon.Enums;

namespace ShipWorks.Shipping.Carriers.Amazon.Api.DTOs
{
    [XmlRoot(ElementName = "GetEligibleShippingServices")]
    public class GetEligibleShippingServices
    {
        [XmlElement(ElementName = "GetEligibleShippingServicesResponse")]
        public GetEligibleShippingServicesResponse GetEligibleShippingServicesResponse { get; set; }
    }

    [XmlRoot(ElementName = "GetEligibleShippingServicesResponse")]
    public class GetEligibleShippingServicesResponse
    {
        [XmlElement(ElementName = "shippingServiceList")]
        public List<ShippingService> ShippingServiceList { get; set; }

        [XmlElement(ElementName = "temporarilyUnavailableCarrierList")]
        public TemporarilyUnavailableCarrierList TemporarilyUnavailableCarrierList { get; set; }

        [XmlElement(ElementName = "TermsAndConditionsNotAcceptedCarrierList")]
        public TermsAndConditionsNotAcceptedCarrierList TermsAndConditionsNotAcceptedCarrierList { get; set; }
    }

    [XmlRoot(ElementName = "rate")]
    public class Rate
    {
        [XmlElement(ElementName = "amount")]
        public decimal Amount { get; set; }

        [XmlElement(ElementName = "currencyCode")]
        public string CurrencyCode { get; set; }
    }

    [XmlRoot(ElementName = "shippingService")]
    public class ShippingService
    {
        [XmlElement(ElementName = "carrierName")]
        public string CarrierName { get; set; }

        [XmlElement(ElementName = "earliestEstimatedDeliveryDate")]
        public string EarliestEstimatedDeliveryDate { get; set; }

        [XmlElement(ElementName = "latestEstimatedDeliveryDate")]
        public string LatestEstimatedDeliveryDate { get; set; }

        [XmlElement(ElementName = "rate")]
        public Rate Rate { get; set; }

        [XmlElement(ElementName = "shipDate")]
        public string ShipDate { get; set; }

        [XmlElement(ElementName = "shippingServiceId")]
        public string ShippingServiceId { get; set; }

        [XmlElement(ElementName = "shippingServiceName")]
        public string ShippingServiceName { get; set; }

        [XmlElement(ElementName = "shippingServiceOfferId")]
        public string ShippingServiceOfferId { get; set; }

        [XmlElement(ElementName = "shippingServiceOptions")]
        public ShippingServiceOptions ShippingServiceOptions { get; set; }
    }

    [XmlRoot(ElementName = "shippingServiceOptions")]
    public class ShippingServiceOptions
    {
        [XmlElement(ElementName = "CarrierWillPickUp")]
        public bool CarrierWillPickUp { get; set; }

        [XmlElement(ElementName = "deliveryExperience")]
        public string DeliveryExperience { get; set; }
    }

    [XmlRoot(ElementName = "temporarilyUnavailableCarrier")]
    public class TemporarilyUnavailableCarrier
    {
        [XmlElement(ElementName = "carrierName")]
        public string CarrierName { get; set; }
    }

    [XmlRoot(ElementName = "temporarilyUnavailableCarrierList")]
    public class TemporarilyUnavailableCarrierList
    {
        [XmlElement(ElementName = "temporarilyUnavailableCarrier")]
        public List<TemporarilyUnavailableCarrier> TemporarilyUnavailableCarrier { get; set; }
    }

    [XmlRoot(ElementName = "TermsAndConditionsNotAcceptedCarrier")]
    public class TermsAndConditionsNotAcceptedCarrier
    {
        [XmlElement(ElementName = "carrierName")]
        public List<string> CarrierName { get; set; }
    }

    [XmlRoot(ElementName = "TermsAndConditionsNotAcceptedCarrierList")]
    public class TermsAndConditionsNotAcceptedCarrierList
    {
        [XmlElement(ElementName = "TermsAndConditionsNotAcceptedCarrier")]
        public TermsAndConditionsNotAcceptedCarrier TermsAndConditionsNotAcceptedCarrier { get; set; }
    }
}