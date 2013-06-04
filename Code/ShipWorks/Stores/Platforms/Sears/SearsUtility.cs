using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

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

            if (ShipmentTypeManager.IsFedEx(shipmentTypeCode))
            {
                return "FDE";
            }
            else if (ShipmentTypeManager.IsUps(shipmentTypeCode))
            {
                return "UPS";
            }
            else if (ShipmentTypeManager.IsPostal(shipmentTypeCode))
            {
                return "USPS";
            }
            else
            {
                return "OTH";
            }
        }

        /// <summary>
        /// Get the service method code to be used for the given shipment upload
        /// </summary>
        public static string GetShpmentServiceCode(ShipmentEntity shipment)
        {
            SearsOrderEntity searsOrder = shipment.Order as SearsOrderEntity;
            if (searsOrder != null && searsOrder.CustomerPickup)
            {
                return "PICKUP";
            }
            else
            {
                // Hardcoded for now - options would be GROUND, PRIORITY, EXPRESS
                return "PRIORITY";
            }
        }

        /// <summary>
        /// The TimeZone that all Sears dates come down in, and are expected in
        /// </summary>
        public static TimeZoneInfo SearsTimeZone
        {
            get
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            }
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
