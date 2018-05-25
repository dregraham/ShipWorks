using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Stores.Platforms.Overstock.OnlineUpdating
{
    /// <summary>
    /// Translate a shipment's carrier and service level into what Overstock wants
    /// </summary>
    public static class OverstockCarrierServiceTranslator
    {
        /// <summary>
        /// Get the carrier code for uploading
        /// </summary>
        public static (string carrierCode, string serviceLevel) GetCarrierValues(IShipmentEntity shipment)
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