using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.UPS.ShipEngine
{
    /// <summary>
    /// Translate from Ups services to ShipEngine codes
    /// </summary>
    public static class UpsShipEngineServiceTypeUtility
    {
        private static readonly Dictionary<UpsServiceType, string> serviceCodeMap = new Dictionary<UpsServiceType, string>()
            {
                { UpsServiceType.UpsGround, "ups_ground" },
                { UpsServiceType.UpsGroundSaver, "ups_ground_saver"},
                { UpsServiceType.Ups3DaySelect, "ups_3_day_select" },
                { UpsServiceType.Ups2DayAir, "ups_2nd_day_air" },
                { UpsServiceType.Ups2DayAirAM, "ups_2nd_day_air_am" },
                { UpsServiceType.UpsNextDayAirSaver, "ups_next_day_air_saver" },
                { UpsServiceType.UpsNextDayAir, "ups_next_day_air" },
                { UpsServiceType.UpsNextDayAirAM, "ups_next_day_air_early_am" },
        };

        private static readonly Dictionary<UpsServiceType, string> internationalServiceCodeMap = new Dictionary<UpsServiceType, string>()
        {
            { UpsServiceType.UpsGround, "ups_ground_international" }
        };

        private static readonly Dictionary<UpsPackagingType, string> packageCodeMap = new Dictionary<UpsPackagingType, string>()
            {
                { UpsPackagingType.Custom, "" },
                { UpsPackagingType.Letter, "ups_letter" },
                { UpsPackagingType.Box10Kg, "ups_10_kg_box" },
                { UpsPackagingType.Box25Kg, "ups_25_kg_box" },
                { UpsPackagingType.Tube, "ups_tube" },
                { UpsPackagingType.Pak, "ups_express_pak" },
                { UpsPackagingType.BoxExpress, "ups_express_box" },
                { UpsPackagingType.BoxExpressSmall, "ups_express_box_small" },
                { UpsPackagingType.BoxExpressMedium, "ups_express_box_medium" },
                { UpsPackagingType.BoxExpressLarge, "ups__express_box_large" }
            };

        /// <summary>
        /// Get a service code for the given service type
        /// </summary>
        public static string GetServiceCode(UpsServiceType serviceType, string countryCode)
        {
            string serviceCode;

            if (countryCode.Equals("US", System.StringComparison.OrdinalIgnoreCase))
            {
                serviceCodeMap.TryGetValue(serviceType, out serviceCode);
            }
            else
            {
                internationalServiceCodeMap.TryGetValue(serviceType, out serviceCode);
            }
            
            if (string.IsNullOrEmpty(serviceCode))
            {
                throw new ShippingException($"{EnumHelper.GetDescription(serviceType)} is not supported from UPS from ShipWorks. Select a different service and try again.");
            }

            return serviceCode;
        }

        /// <summary>
        /// Get a service type for the given service code
        /// </summary>
        public static UpsServiceType GetServiceType(string serviceCode)
        {
            var serviceType = serviceCodeMap.Where(v => v.Value == serviceCode).Select(e => e.Key)
                .Cast<UpsServiceType?>().FirstOrDefault();

            if(!serviceType.HasValue)
            {
                serviceType = internationalServiceCodeMap.Where(v => v.Value == serviceCode).Select(e => e.Key)
                    .Cast<UpsServiceType?>()
                    .FirstOrDefault();
            }

            if (serviceType == null)
            {
                throw new ShippingException($"{serviceCode} is not supported from UPS from ShipWorks. Select a different service and try again.");
            }

            return serviceType.Value;
        }

        /// <summary>
        /// Get a package code for the given PackagingType
        /// </summary>
        public static string GetPackageCode(UpsPackagingType packageCode)
        {
            if (!packageCodeMap.ContainsKey(packageCode))
            {
                throw new ShippingException($"{EnumHelper.GetDescription(packageCode)} is not supported from UPS from ShipWorks. Select a different packaging type and try again.");
            }

            return packageCodeMap[packageCode];
        }

        /// <summary>
        /// Check to see if the service is supported for any country
        /// </summary>
        public static bool IsServiceSupported(UpsServiceType serviceType)
        {
            return serviceCodeMap.ContainsKey(serviceType) || internationalServiceCodeMap.ContainsKey(serviceType);
        }
        
        /// <summary>
        /// Check to see if the service is supported
        /// </summary>
        public static bool IsServiceSupported(UpsServiceType serviceType, string countryCode)
        {
            if (countryCode.Equals("US", System.StringComparison.OrdinalIgnoreCase))
            {
                return serviceCodeMap.ContainsKey(serviceType);
            }
            else
            {
                return internationalServiceCodeMap.ContainsKey(serviceType);
            }
        }

        /// <summary>
        /// Get all of the UPS services supported by ShipEngine
        /// </summary>
        public static IEnumerable<UpsServiceType> GetSupportedServices() => serviceCodeMap.Keys;
    }
}
