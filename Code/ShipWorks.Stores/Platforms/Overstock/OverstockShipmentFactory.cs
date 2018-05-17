using System.Linq;
using System.Xml.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Stores.Platforms.Overstock
{
    /// <summary>
    /// Generates XML for uploading shipment details to Overstock
    /// </summary>
    [Component]
    public class OverstockShipmentFactory : IOverstockShipmentFactory
    {
        /// <summary>
        /// Create the XML for shipment upload to Overstock
        /// </summary>
        public XDocument CreateShipmentDetails(IShipmentEntity shipment)
        {
            OverstockOrderEntity overstockOrder = shipment.Order as OverstockOrderEntity;

            (string CarrierCode, string ServiceLevel) carrierValues = GetCarrierValues(shipment);

            XNamespace ns = "api.supplieroasis.com";

            XDocument shipmentDetails = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement(ns + "supplierShipmentMessage",
                    overstockOrder?.OrderItems.Select(oi => new XElement("supplierShipment",
                            new XElement("salesChannelName", overstockOrder.SalesChannelName),
                            new XElement("salesChannelOrderNumber", overstockOrder.OrderNumberComplete),
                            new XElement("salesChannelLineNumber", ((OverstockOrderItemEntity) oi).SalesChannelLineNumber),
                            new XElement("warehouse", new XElement("code", overstockOrder.WarehouseCode)),
                            new XElement("supplierShipConfirmation",
                                new XElement("quantity", oi.Quantity),
                                new XElement("carrier", new XElement("code", carrierValues.CarrierCode)),
                                new XElement("trackingNumber", shipment.TrackingNumber),
                                new XElement("shipDate", shipment.ShipDate.ToString("o")),
                                new XElement("serviceLevel", new XElement("code", carrierValues.ServiceLevel)))
                        )
                    )
                )
            );

            return shipmentDetails;
        }

        /// <summary>
        /// Get the carrier code for uploading
        /// </summary>
        private static (string carrierCode, string serviceLevel) GetCarrierValues(IShipmentEntity shipment)
        {
            switch (shipment.ShipmentTypeCode)
            {
                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    return GetUpsValues(shipment);

                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.Usps:
                    return GetUspsValues(shipment);

                case ShipmentTypeCode.FedEx:
                    FedExServiceType fedExServiceType = (FedExServiceType) shipment.FedEx.Service;
                    if (FedExUtility.IsFreightAnyService(fedExServiceType))
                    {
                        return ("FXFE", "DOCK");
                    }

                    if (fedExServiceType == FedExServiceType.SmartPost)
                    {
                        return ("FEDSP", "GROUND");
                    }

                    return GetFedExValues(fedExServiceType);

                case ShipmentTypeCode.OnTrac:
                    return ("ONTRAC", "GROUND");

                case ShipmentTypeCode.DhlExpress:
                    return ("DHL", "GROUND");

                case ShipmentTypeCode.Other:
                    return (shipment.Other.Carrier.ToUpper(), shipment.Other.Service.ToUpper());

                default:
                    return ("UNKNOWN", "GROUND");
            }
        }

        /// <summary>
        /// Get carrier code and service level for UPS
        /// </summary>
        private static (string carrierCode, string serviceLevel) GetUpsValues(IShipmentEntity shipment)
        {
            UpsServiceType upsServiceType = (UpsServiceType) shipment.Ups.Service;
            if (UpsUtility.IsUpsSurePostService(upsServiceType))
            {
                return ("UPSSP", "FIRSTCLASS");
            }
            else if (UpsUtility.IsUpsMiService(upsServiceType))
            {
                return ("UPSMI", "FIRSTCLASS");
            }
            else
            {
                switch (upsServiceType)
                {
                    case UpsServiceType.UpsNextDayAir:
                    case UpsServiceType.UpsNextDayAirSaver:
                    case UpsServiceType.UpsNextDayAirAM:
                        return ("UPS", "NEXT_DAY");

                    case UpsServiceType.Ups2DayAir:
                    case UpsServiceType.Ups2DayAirAM:
                    case UpsServiceType.Ups2ndDayAirIntra:
                        return ("UPS", "TWO_DAY");

                    case UpsServiceType.Ups3DaySelect:
                    case UpsServiceType.Ups3DaySelectFromCanada:
                        return ("UPS", "THREE_DAY");

                    case UpsServiceType.UpsMailInnovationsFirstClass:
                    case UpsServiceType.UpsSurePostLessThan1Lb:
                    case UpsServiceType.UpsSurePostBoundPrintedMatter:
                        return ("UPS", "FIRSTCLASS");

                    case UpsServiceType.UpsMailInnovationsPriority:
                    case UpsServiceType.UpsMailInnovationsExpedited:
                    case UpsServiceType.UpsMailInnovationsIntEconomy:
                    case UpsServiceType.UpsMailInnovationsIntPriority:
                    case UpsServiceType.UpsSurePost1LbOrGreater:
                        return ("UPS", "PRIORITY");

                    case UpsServiceType.UpsSurePostMedia:
                        return ("UPS", "MEDIA_MAIL");
                }

                return ("UPS", "GROUND");
            }
        }

        /// <summary>
        /// Get carrier code and service level for USPS
        /// </summary>
        private static (string carrierCode, string serviceLevel) GetUspsValues(IShipmentEntity shipment)
        {
            if (shipment.Postal.Service == (int) PostalServiceType.FirstClass)
            {
                return ("USPSFC", "FIRSTCLASS");
            }
            else
            {
                return ("USPS", "PRIORITY");
            }
        }

        /// <summary>
        /// Get carrier code and service level for FedEx
        /// </summary>
        private static (string carrierCode, string serviceLevel) GetFedExValues(FedExServiceType fedExServiceType)
        {
            
            switch (fedExServiceType)
            {
                case FedExServiceType.PriorityOvernight:
                case FedExServiceType.StandardOvernight:
                case FedExServiceType.FirstOvernight:
                case FedExServiceType.OneRateFirstOvernight:
                case FedExServiceType.OneRatePriorityOvernight:
                case FedExServiceType.OneRateStandardOvernight:
                case FedExServiceType.FedExNextDayAfternoon:
                case FedExServiceType.FedExNextDayEarlyMorning:
                case FedExServiceType.FedExNextDayMidMorning:
                case FedExServiceType.FedExNextDayEndOfDay:
                    return ("FEDEX", "NEXT_DAY");

                case FedExServiceType.FedEx2Day:
                case FedExServiceType.FedEx2DayAM:
                case FedExServiceType.OneRate2Day:
                case FedExServiceType.OneRate2DayAM:
                    return ("FEDEX", "TWO_DAY");

                case FedExServiceType.InternationalFirst:
                    return ("FEDEX", "FIRSTCLASS");

                case FedExServiceType.InternationalPriority:
                case FedExServiceType.InternationalPriorityExpress:
                case FedExServiceType.FedExEuropeFirstInternationalPriority:
                    return ("FEDEX", "PRIORITY");
            }

            return ("FEDEX", "GROUND");
        }
    }
}
