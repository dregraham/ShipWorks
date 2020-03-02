using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.Ups.ShipEngine
{
    /// <summary>
    /// Translate from Ups services to ShipEngine codes
    /// </summary>
    public static class UpsShipEngineTranslation
    {
        private static Dictionary<UpsServiceType, string> serviceCodeMap = new Dictionary<UpsServiceType, string>()
            {
                { UpsServiceType.UpsGround, "ups_ground" },
                { UpsServiceType.Ups3DaySelect, "ups_3_day_select" },
                { UpsServiceType.Ups2DayAir, "ups_2nd_day_air" },
                { UpsServiceType.Ups2DayAirAM, "ups_2nd_day_air_am" },
                { UpsServiceType.UpsNextDayAirSaver, "ups_next_day_air_saver" },
                { UpsServiceType.UpsNextDayAir, "ups_next_day_air" },
                { UpsServiceType.UpsNextDayAirAM, "ups_next_day_air_early_am" },
                { UpsServiceType.UpsStandard, "ups_standard_international" },
                { UpsServiceType.WorldwideExpedited, "ups_worldwide_expedited" },
                { UpsServiceType.WorldwideSaver, "ups_worldwide_saver" },
                { UpsServiceType.WorldwideExpress, "ups_worldwide_express" }
            }
        ;

        private static Dictionary<UpsPackagingType, string> packageCodeMap = new Dictionary<UpsPackagingType, string>()
            {
                { UpsPackagingType.Custom, "" },
                { UpsPackagingType.Letter, "ups_letter" },
                { UpsPackagingType.Box10Kg, "ups_10_kg_box" },
                { UpsPackagingType.Box25Kg, "ups_25_kg_box	" },
                { UpsPackagingType.Tube, "ups_tube" },
                { UpsPackagingType.Pak, "ups_express_pak" },
                { UpsPackagingType.BoxExpress, "ups_express_box" },
                { UpsPackagingType.BoxExpressSmall, "ups_express_box_small" },
                { UpsPackagingType.BoxExpressMedium, "ups_express_box_medium" },
                { UpsPackagingType.BoxExpressLarge, "ups_express_box_large" },
                { UpsPackagingType.Flats, "mi_bpm_flat" },
                { UpsPackagingType.BPMParcels, "mi_bpm_parcel" },
                { UpsPackagingType.FirstClassMail, "mi_first_class" },
                { UpsPackagingType.Irregulars, "mi_irregulars" },
                { UpsPackagingType.Machinables, "mi_machinables" },
                { UpsPackagingType.MediaMail, "mi_media_mail" },
                { UpsPackagingType.ParcelPost, "mi_parcel_post" },
                { UpsPackagingType.PriorityMail, "mi_priority" },
                { UpsPackagingType.StandardFlats, "mi_standard_flat" },
            };

        /// <summary>
        /// Get a service code for the given service type
        /// </summary>
        public static string GetServiceCode(UpsServiceType serviceType) => 
            serviceCodeMap[serviceType];

        /// <summary>
        /// Get a service type for the given service code
        /// </summary>
        public static UpsServiceType GetServiceType(string serviceCode) =>
            serviceCodeMap.FirstOrDefault(x => x.Value == serviceCode).Key;

        /// <summary>
        /// Get a package code for the given service type
        /// </summary>
        public static string GetPackageCode(UpsPackagingType serviceType) =>
            packageCodeMap[serviceType];

        /// <summary>
        /// Check to see if the service is supported
        /// </summary>
        public static bool IsServiceSupported(UpsServiceType serviceType) =>
            serviceCodeMap.ContainsKey(serviceType);
    }
}
