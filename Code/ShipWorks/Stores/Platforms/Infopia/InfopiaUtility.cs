using System;
using System.Collections.Generic;
using System.Globalization;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;

namespace ShipWorks.Stores.Platforms.Infopia
{
    /// <summary>
    /// Utility methods for interacting with Infopia's web service
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    public static class InfopiaUtility
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(InfopiaUtility));

        // list of supported Infopia statuses
        static IList<string> statusValues = new List<string>()
            {
                "New",
                "Processed",
                "Incomplete",
                "Failed",
                "Manually Processed",
                "Pending",
                "Pending From Incomplete",
                "To Ship",
                "Flagged",
                "Temp",
                "Fraud",
                "Returns"
            }.AsReadOnly();

        /// <summary>
        /// Gets the predefined status values for Infopia
        /// </summary>
        public static IList<string> StatusValues
        {
            get { return statusValues; }
        }

        /// <summary>
        /// Convert the given date to the infopia store timezone
        /// </summary>
        public static DateTime ConvertToInfopiaTimeZone(DateTime dateTime)
        {
            // Always mountain time, apparently
            int utcOffset = -7;

            // Account for DST
            if (TimeZone.CurrentTimeZone.IsDaylightSavingTime(dateTime))
            {
                utcOffset += 1;
            }

            return dateTime - TimeSpan.FromHours(-utcOffset);
        }

        /// <summary>
        /// Convert the given date from the infopia store timezone
        /// </summary>
        public static DateTime ConvertFromInfopiaTimeZone(DateTime dateTime)
        {
            // Always mountain time, apparently
            int utcOffset = -7;

            // Account for DST
            if (TimeZone.CurrentTimeZone.IsDaylightSavingTime(dateTime))
            {
                utcOffset += 1;
            }

            return dateTime + TimeSpan.FromHours(-utcOffset);
        }

        /// <summary>
        /// Format the given date in the store-configured ca format.
        /// </summary>
        public static string FormatDate(DateTime dateTime)
        {
            return dateTime.ToString("yyyy/MM/dd HH:mm:ss");
        }

        /// <summary>
        /// Parse the given date using the store-configured ca date format
        /// </summary>
        public static DateTime ParseDate(string dateText)
        {
            try
            {
                string[] formats = new string[]
                    {
                        "MM-dd-yyyy HH:mm:ss",
                        "MM/dd/yyyy HH:mm:ss"
                    };

                return DateTime.ParseExact(dateText, formats, null, DateTimeStyles.AllowWhiteSpaces);
            }
            catch (FormatException ex)
            {
                throw new FormatException("Could not parse date '" + dateText + "'", ex);
            }
        }

        /// <summary>
        /// The Page size when returning orders from Infopia
        /// </summary>
        public static int DownloadBatchSize
        {
            get
            {
                int size = InterapptiveOnly.Registry.GetValue("InfopiaBatchSize", 200);

                // Restrict between 1 and 500
                return Math.Min(Math.Max(size, 1), 500);
            }
            set
            {
                InterapptiveOnly.Registry.SetValue("InfopiaBatchSize", value);
            }
        }

        /// <summary>
        /// Gets the Infopia Shipper Code from a given shipment
        /// </summary>
        public static string GetInfopiaShipperCode(IShipmentEntity shipment)
        {
            switch ((ShipmentTypeCode) shipment.ShipmentType)
            {
                case ShipmentTypeCode.FedEx:
                    return "FedEx";

                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    return "UPS";

                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.PostalWebTools:
                    return "USPS";

                case ShipmentTypeCode.Other:
                    return "Other";
            }

            log.ErrorFormat("Unhandled shipping type in GetInfopiaShipperCode: {0}", shipment.ShipmentType);

            return "Other";
        }

        /// <summary>
        /// Returns the values to upload to Infopia
        /// </summary>
        public static void GetShipmentUploadValues(ShipmentEntity shipment, out string shipper, out string trackingNumber)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            string tempTrackingNumber = shipment.TrackingNumber;
            string tempShipper = GetInfopiaShipperCode(shipment);

            // Handle Mail Innovations et. al.
            WorldShipUtility.DetermineAlternateTracking(shipment, (track, service) =>
               {
                   if (track.Length > 0)
                   {
                       tempShipper = "USPS";
                       tempTrackingNumber = track;
                   }
                   else
                   {
                       tempShipper = "Other";
                   }
               });

            trackingNumber = tempTrackingNumber;
            shipper = tempShipper;
        }
    }
}
