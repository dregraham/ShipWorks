using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.Stores.Platforms.Sears
{
    /// <summary>
    /// Utility functions for working with sears
    /// </summary>
    public static class SearsUtility
    {
        /// <summary>
        /// Get the carrier code to be used for the given shipment upload
        /// </summary>
        public static string GetShipmentCarrierCode(ShipmentEntity shipment)
        {
            ShipmentTypeCode shipmentTypeCode = (ShipmentTypeCode) shipment.ShipmentType;

            SearsOrderEntity searsOrder = shipment.Order as SearsOrderEntity;
            if (searsOrder != null && searsOrder.CustomerPickup)
            {
                return "OTH";
            }

            // FedEx
            if (ShipmentTypeManager.IsFedEx(shipmentTypeCode))
            {
                FedExServiceType fedExService = ((FedExServiceType) shipment.FedEx.Service);
                if (fedExService == FedExServiceType.SmartPost)
                {
                    return "SMRT";
                }

                if (FedExUtility.IsFreightService(fedExService))
                {
                    return "FXFT";
                }

                return "FDE";
            }

            // UPS
            if (ShipmentTypeManager.IsUps(shipmentTypeCode))
            {
                return UpsUtility.IsUpsMiService((UpsServiceType) shipment.Ups.Service) ? "UPSM" : "UPS";
            }

            // USPS
            if (ShipmentTypeManager.IsPostal(shipmentTypeCode))
            {
                return "USPS";
            }

            // I-Parcel
            if (ShipmentTypeManager.IsiParcel(shipmentTypeCode))
            {
                return "UPSI";
            }

            return "OTH";
        }

        /// <summary>
        /// Get the service method code to be used for the given shipment upload
        /// </summary>
        public static string GetShipmentServiceCode(ShipmentEntity shipment)
        {
            SearsOrderEntity searsOrder = shipment.Order as SearsOrderEntity;
            if (searsOrder != null && searsOrder.CustomerPickup)
            {
                return "PICKUP";
            }

            ShipmentTypeCode shipmentTypeCode = (ShipmentTypeCode)shipment.ShipmentType;

            // FedEx
            if (ShipmentTypeManager.IsFedEx(shipmentTypeCode))
            {
                FedExServiceType fedExServiceType = (FedExServiceType)shipment.FedEx.Service;

                return GetFedExServiceName(fedExServiceType);
            }

            // UPS
            if (ShipmentTypeManager.IsUps(shipmentTypeCode))
            {
                UpsServiceType upsServiceType = (UpsServiceType)shipment.Ups.Service;

                return GetUpsServiceName(upsServiceType);
            }

            // Postal
            if (ShipmentTypeManager.IsPostal(shipmentTypeCode))
            {
                PostalServiceType uspsServiceType = (PostalServiceType)shipment.Postal.Service;
                return GetPostalServiceName(uspsServiceType);
            }

            // I-Parcel
            if (ShipmentTypeManager.IsiParcel(shipmentTypeCode))
            {
                return "Parcel";
            }

            return "PRIORITY";
        }

        /// <summary>
        /// Returns the Sears service name for the given fedex service type
        /// </summary>
        private static string GetFedExServiceName(FedExServiceType fedExServiceType)
        {
            switch (fedExServiceType)
            {
                case FedExServiceType.PriorityOvernight:
                case FedExServiceType.StandardOvernight:
                case FedExServiceType.FirstOvernight:
                case FedExServiceType.OneRateFirstOvernight:
                case FedExServiceType.OneRatePriorityOvernight:
                case FedExServiceType.OneRateStandardOvernight:
                    return "Next Day";

                case FedExServiceType.FedEx2Day:
                case FedExServiceType.FedEx2DayAM:
                case FedExServiceType.OneRate2Day:
                case FedExServiceType.OneRate2DayAM:
                    return "Second Day";

                case FedExServiceType.FedEx1DayFreight:
                case FedExServiceType.FedEx2DayFreight:
                case FedExServiceType.FedEx3DayFreight:
                case FedExServiceType.InternationalPriorityFreight:
                case FedExServiceType.InternationalEconomyFreight:
                case FedExServiceType.FirstFreight:
                    return "LTL";

                case FedExServiceType.SmartPost:
                    return "Smart Post";
            }

            return "Ground";
        }

        /// <summary>
        /// Returns the Sears service name for the given ups service type
        /// </summary>
        private static string GetUpsServiceName(UpsServiceType upsServiceType)
        {
            if (UpsUtility.IsUpsMiService(upsServiceType))
            {
                return "Parcel";
            }

            if (UpsUtility.IsUpsSurePostService(upsServiceType))
            {
                return "SurePost";
            }

            if (upsServiceType == UpsServiceType.UpsNextDayAirSaver)
            {
                return "Next Day Saver";
            }

            if (upsServiceType == UpsServiceType.UpsNextDayAir || upsServiceType == UpsServiceType.UpsNextDayAirAM)
            {
                return "Next Day";
            }

            if (upsServiceType == UpsServiceType.Ups2DayAir || upsServiceType == UpsServiceType.Ups2DayAirAM || upsServiceType == UpsServiceType.Ups2ndDayAirIntra)
            {
                return "Second Day";
            }

            return "Ground";
        }

        /// <summary>
        /// Returns the Sears service name for the given postal service type
        /// </summary>
        private static string GetPostalServiceName(PostalServiceType postalServiceType)
        {
            if (postalServiceType == PostalServiceType.ParcelSelect)
            {
                return "Parcel Post";
            }

            if (postalServiceType == PostalServiceType.FirstClass)
            {
                return "First Class";
            }

            if (postalServiceType == PostalServiceType.StandardPost)
            {
                return "Standard Mail";
            }

            return "Priority";
        }

        /// <summary>
        /// The TimeZone that all Sears dates come down in, and are expected in
        /// </summary>
        public static TimeZoneInfo SearsTimeZone
        {
            get { return TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"); }
        }

        /// <summary>
        /// Convert the given UTC date to the Sears timezone
        /// </summary>
        public static DateTime ConvertUtcToSearsTimeZone(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Utc, SearsUtility.SearsTimeZone);
        }

        /// <summary>
        /// Convert the given date, in the Sears timezone, to UTC
        /// </summary>
        public static DateTime ConvertSearsTimeZoneToUTC(DateTime searsDate)
        {
            return TimeZoneInfo.ConvertTime(searsDate, SearsUtility.SearsTimeZone, TimeZoneInfo.Utc);
        }
    }
}
