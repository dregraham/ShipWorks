﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ActiproSoftware.Drawing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Stores;
using ShipWorks.Data;
using ShipWorks.Shipping.Settings.Origin;
using System.Text.RegularExpressions;
using ShipWorks.Shipping.Settings;
using ShipWorks.Editions;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Utility functions for dealing with USPS in general, not a specific USPS integration.
    /// </summary>
    public static class PostalUtility
    {
        /// <summary>
        /// This is a list of ranges used for military addresses
        /// http://pe.usps.gov/text/LabelingLists/L002.htm
        /// </summary>
        private static readonly List<string> militaryPostalCodePrefixes = Enumerable
            .Range(90, 9)
            .Concat(new[] {340})
            .Concat(Enumerable.Range(962, 5))
            .Select(x => x.ToString("000")).ToList();

        /// <summary>
        /// Indicates if the given state code is a military destination.  Only the USPS can ship to military, which is why this is in the PostalUtility
        /// </summary>
        public static bool IsMilitaryState(string stateCode)
        {
            return stateCode == "AA" || stateCode == "AE" || stateCode == "AP";
        }

        /// <summary>
        /// Indiciates if the given postal code is a military destination.
        /// </summary>
        public static bool IsMilitaryPostalCode(string postalCode)
        {
            return militaryPostalCodePrefixes.Any(postalCode.StartsWith);
        }

        /// <summary>
        /// Indicates if the shipment type is a postal shipment
        /// </summary>
        public static bool IsPostalShipmentType(ShipmentTypeCode shipmentTypeCode)
        {
            switch (shipmentTypeCode)
            {
                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.Stamps:
                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Stamps:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Indicates if the given foreign country code is considered a postal domestic shipment.
        /// Per http://www.usps.com/ncsc/lookups/abbr_state.txt
        /// </summary>
        public static bool IsDomesticCountry(string countryCode)
        {
            return

                // Of course US
                countryCode == "US" ||

                // American Samoa
                countryCode == "AS" ||

                // District of Columbia
                countryCode == "DC" ||

                // Federated States of Micronesia
                countryCode == "FM" ||

                // Guam
                countryCode == "GU" ||

                // Marshall Islands
                countryCode == "MH" ||

                // Northern Mariana Islands
                countryCode == "MP" ||

                // Palau
                countryCode == "PW" ||

                // Puerto Rico
                countryCode == "PR" ||

                // Virgin Islands
                countryCode == "VI" ||
                countryCode == "VL" ||
                countryCode == "UV";
        }

        /// <summary>
        /// Get the domestic services valid for the given shipment type
        /// </summary>
        public static List<PostalServiceType> GetDomesticServices(ShipmentTypeCode shipmentType)
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            if((shipmentType == ShipmentTypeCode.Express1Endicia && !settings.Express1EndiciaSingleSource) ||
               (shipmentType == ShipmentTypeCode.Express1Stamps && !settings.Express1StampsSingleSource))
            {
                return new List<PostalServiceType>
                    {
                        PostalServiceType.PriorityMail,
                        PostalServiceType.ExpressMail,
                    };
            }
            else
            {
                var services = new List<PostalServiceType>
                    {
                        PostalServiceType.PriorityMail,
                        PostalServiceType.ExpressMail,
                        PostalServiceType.FirstClass,
                        PostalServiceType.ParcelSelect,
                        PostalServiceType.MediaMail,
                        PostalServiceType.LibraryMail,
                        PostalServiceType.CriticalMail,
                        PostalServiceType.StandardPost
                    };

                if (shipmentType == ShipmentTypeCode.Stamps ||
                    shipmentType == ShipmentTypeCode.Usps ||
                    shipmentType == ShipmentTypeCode.Express1Stamps ||
                    shipmentType == ShipmentTypeCode.Express1Endicia)
                {
                    // As of the 01/28/2013 changes, Stamps.com does not support Parcel Post (now Standard Post)
                    services.Remove(PostalServiceType.StandardPost);
                }

                if (shipmentType == ShipmentTypeCode.Endicia)
                {
                    // If consolidation is supported, add it in
                    if (EditionManager.ActiveRestrictions.CheckRestriction(EditionFeature.EndiciaConsolidator).Level == EditionRestrictionLevel.None)
                    {
                        services.Add(PostalServiceType.ConsolidatorDomestic);
                    }

                    // If not restricted from Endicia DHL, add them in
                    if (EditionManager.ActiveRestrictions.CheckRestriction(EditionFeature.EndiciaDhl).Level == EditionRestrictionLevel.None)
                    {
                        services.AddRange(EnumHelper.GetEnumList<PostalServiceType>(service => ShipmentTypeManager.IsEndiciaDhl(service)).Select(entry => entry.Value));
                    }
                }

                return services;
            }
        }

        /// <summary>
        /// Get the international services supported by the given shipment type
        /// </summary>
        public static List<PostalServiceType> GetInternationalServices(ShipmentTypeCode shipmentType)
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            if((shipmentType == ShipmentTypeCode.Express1Endicia && !settings.Express1EndiciaSingleSource) ||
               (shipmentType == ShipmentTypeCode.Express1Stamps && !settings.Express1StampsSingleSource))
            {
                return new List<PostalServiceType>
                    {
                        PostalServiceType.InternationalPriority,
                        PostalServiceType.InternationalExpress,
                    };
            }
            else
            {
                var services = new List<PostalServiceType>
                    {
                        PostalServiceType.InternationalPriority,
                        PostalServiceType.InternationalFirst,
                        PostalServiceType.InternationalExpress,

                        // Deprecated values, these get filtered out before being bound to a ComboBox
                        PostalServiceType.ExpressMailPremium,
                        PostalServiceType.GlobalExpressGuaranteed,
                        PostalServiceType.GlobalExpressGuaranteedNonDocument
                    };

                if (shipmentType == ShipmentTypeCode.Endicia)
                {
                    // If consolidation is supported, add it in
                    if (EditionManager.ActiveRestrictions.CheckRestriction(EditionFeature.EndiciaConsolidator).Level == EditionRestrictionLevel.None)
                    {
                        services.Add(PostalServiceType.ConsolidatorInternational);
                        services.Add(PostalServiceType.ConsolidatorIpa);
                        services.Add(PostalServiceType.ConsolidatorIsal);
                        services.Add(PostalServiceType.CommercialePacket);
                    }
                }

                return services;
            }
        }

        /// <summary>
        /// Get the number of days it takes to deliver a service
        /// </summary>
        public static string GetServiceTransitDays(PostalServiceType serviceType)
        {
            switch (serviceType)
            {
                case PostalServiceType.PriorityMail:
                case PostalServiceType.FirstClass:
                case PostalServiceType.CriticalMail:
                    return "1-3";

                case PostalServiceType.StandardPost:
                case PostalServiceType.LibraryMail:
                case PostalServiceType.MediaMail:
                    return "2-9";

                case PostalServiceType.ExpressMail:
                    return "1-2";

                case PostalServiceType.InternationalExpress:
                    return "3 - 5 business days";

                case PostalServiceType.InternationalPriority:
                    return "6 - 10 business days";

                case PostalServiceType.InternationalFirst:
                    return "Varies by country";
            }

            return "";
        }

        /// <summary>
        /// Gets the BestRate service level associated with the specified postal service type
        /// </summary>
        /// <param name="serviceType">Service type for which to get the best rate service level</param>
        /// <returns></returns>
        public static ServiceLevelType GetServiceLevel(PostalServiceType serviceType)
        {
            switch (serviceType)
            {
                case PostalServiceType.ExpressMail:
                    return ServiceLevelType.TwoDays;

                case PostalServiceType.PriorityMail:
                case PostalServiceType.FirstClass:
                case PostalServiceType.CriticalMail:
                    return ServiceLevelType.ThreeDays;

                case PostalServiceType.InternationalExpress:
                    return ServiceLevelType.FourToSevenDays;

                default:
                    return ServiceLevelType.Anytime;
            }
        }

        /// <summary>
        /// Get the longest amount of delivery days for the specified best rate service level
        /// </summary>
        /// <param name="serviceLevel">Service level for which to get the worst case delivery days</param>
        /// <returns></returns>
        public static int GetWorstCaseDeliveryDaysFromServiceType(ServiceLevelType serviceLevel)
        {
            switch (serviceLevel)
            {
                case ServiceLevelType.OneDay:
                    return 1;
                case ServiceLevelType.TwoDays:
                    return 2;
                case ServiceLevelType.ThreeDays:
                    return 3;
                case ServiceLevelType.FourToSevenDays:
                    return 7;
                default:
                    return -1;
            }
        }

        /// <summary>
        /// The USPS does not allow punctuation characters in addresses
        /// </summary>
        public static string StripPunctuation(string value)
        {
            // Replace any non char\digit with a space
            value = Regex.Replace(value, "[^0-9a-zA-Z ]", " ");

            // Replace any resulting double-spaces with a single space
            value = Regex.Replace(value, "[ ]+", " ");

            return value;
        }

        /// <summary>
        /// Get the appropriate customs form to use for the given shipment
        /// </summary>
        public static PostalCustomsForm GetCustomsForm(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            if (IsMilitaryState(shipment.ShipStateProvCode))
            {
                return (shipment.TotalWeight >= 1 || shipment.CustomsValue >= 400) ?
                    PostalCustomsForm.CN72 :
                    PostalCustomsForm.CN22;
            }
            else if (IsDomesticCountry(shipment.ShipCountryCode))
            {
                return PostalCustomsForm.None;
            }
            else
            {
                return (shipment.CustomsValue >= 400)  ?
                    PostalCustomsForm.CN72 :
                    PostalCustomsForm.CN22;
            }
        }

        /// <summary>
        /// Indicates if the packaging type represents an envelope or flat
        /// </summary>
        public static bool IsEnvelopeOrFlat(PostalPackagingType packagingType)
        {
            switch (packagingType)
            {
                case PostalPackagingType.Envelope:
                case PostalPackagingType.FlatRateEnvelope:
                case PostalPackagingType.LargeEnvelope:
                case PostalPackagingType.FlatRateLegalEnvelope:
                case PostalPackagingType.FlatRatePaddedEnvelope:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Indicates if the combination of country, service, and packaging qualifies for the free international delivery confirmation
        /// </summary>
        public static bool IsFreeInternationalDeliveryConfirmation(string countryCode, PostalServiceType serviceType, PostalPackagingType packagingType)
        {
            // Has to be Canada
            if (IsCountryEligibleForFreeInternationalDeliveryConfirmation(countryCode))
            {
                if (packagingType == PostalPackagingType.FlatRateSmallBox)
                {
                    return true;
                }

                if (serviceType == PostalServiceType.InternationalPriority)
                {
                    switch (packagingType)
                    {
                        case PostalPackagingType.FlatRateEnvelope:
                        case PostalPackagingType.FlatRateLegalEnvelope:
                        case PostalPackagingType.FlatRatePaddedEnvelope:
                            return true;
                    }
                }

                if (serviceType == PostalServiceType.InternationalFirst)
                {
                    if (!PostalUtility.IsEnvelopeOrFlat(packagingType))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether [is country eligible for free international delivery confirmation] [the specified country code].
        /// </summary>
        /// <param name="countryCode">The country code.</param>
        /// <returns>
        ///   <c>true</c> if [is country eligible for free international delivery confirmation] [the specified country code]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsCountryEligibleForFreeInternationalDeliveryConfirmation(string countryCode)
        {
            // Allowable country codes include Australia, Belgium, Brazil, Canada, Croatia, Denmark, Estonia, Finland,
            // France, Germany, Gibraltar, Great Britain, Hungary, Northern Ireland, Israel, Italy, Latvia, Lithuania, Luxembourg, Malaysia, 
            // Malta, Netherlands, New Zealand, Portugal, Singapore, Spain, and Switzerland
            List<string> eligibleCountryCodes = new List<string>
            {
                "AU", "BE", "BR", "CA", "HR", "DK", "FR", "DE", "GB", "NB", "IL", "NL", "NZ", "ES", "CH", 
                "EE", "FI", "GI", "HU", "IT", "LV", "LT", "LU", "MY", "MT", "PT", "SG"
            };

            return eligibleCountryCodes.Contains(countryCode);
        }
        
        /// <summary>
        /// Helper method to get postal service description.
        /// </summary>
        public static string GetPostalServiceTypeDescription(PostalServiceType postalServiceType)
        {
            return EnumHelper.GetDescription(postalServiceType);
        }

        /// <summary>
        /// Indicates if the EntryFacility and SortType is required for a service
        /// </summary>
        public static bool IsEntryFacilityRequired(PostalServiceType serviceType)
        {
            return 
                serviceType == PostalServiceType.ParcelSelect || 
                ShipmentTypeManager.IsEndiciaDhl(serviceType);
        }

        /// <summary>
        /// Sets service level details on the specified rate
        /// </summary>
        /// <param name="baseRate">Rate on which service level details should be set</param>
        public static void SetServiceDetails(RateResult baseRate)
        {
            PostalRateSelection rateSelection = baseRate.OriginalTag as PostalRateSelection;

            if (rateSelection != null)
            {
                SetServiceDetails(baseRate, rateSelection.ServiceType, string.Empty);    
            }
        }

        /// <summary>
        /// Sets service level details on the specified rate
        /// </summary>
        /// <param name="baseRate">Rate on which service level details should be set</param>
        /// <param name="serviceType">Service type for the specified rate</param>
        /// <param name="deliverDays">How many days are expected for the package to be in delivery</param>
        public static void SetServiceDetails(RateResult baseRate, PostalServiceType serviceType, string deliverDays)
        {
            baseRate.ServiceLevel = GetServiceLevel(serviceType);

            int deliveryDays = -1;
            if (!int.TryParse(deliverDays.Split('-').LastOrDefault(), out deliveryDays))
            {
                deliveryDays = GetWorstCaseDeliveryDaysFromServiceType(baseRate.ServiceLevel);
            }

            if (deliveryDays > 0)
            {
                DateTime? deliveryDate = ShippingManager.CalculateExpectedDeliveryDate(deliveryDays, DayOfWeek.Sunday);

                if (deliveryDate.HasValue && deliveryDate.Value.DayOfWeek == DayOfWeek.Saturday)
                {
                    deliveryDate = deliveryDate.Value.AddDays(2);
                }

                baseRate.ExpectedDeliveryDate = deliveryDate;
            }
        }

        /// <summary>
        /// Gets a state adjusted for what the post office wants
        /// </summary>
        /// <param name="countryCode">Country code for the address</param>
        /// <param name="state">State for the address</param>
        /// <returns>State code that should be used for a postal address</returns>
        public static string AdjustState(string countryCode, string state)
        {
            // If the country is Puerto Rico, return PR regardless of what was entered
            return countryCode.Equals("PR", StringComparison.OrdinalIgnoreCase) ? "PR" : state;
        }

        /// <summary>
        /// Determines whether any postal account exists.
        /// </summary>
        public static bool IsPostalSetup()
        {
            return EndiciaAccountManager.EndiciaAccounts.Any() ||
                   EndiciaAccountManager.Express1Accounts.Any() ||
                   StampsAccountManager.StampsAccounts.Any() ||
                   StampsAccountManager.Express1Accounts.Any() ||
                   StampsAccountManager.StampsExpeditedAccounts.Any();
        }

        /// <summary>
        /// Returns the StampsResellerType for a ShipmentTypeCode
        /// </summary>
        public static StampsResellerType GetStampsResellerType(ShipmentTypeCode shipmentTypeCode)
        {
            switch (shipmentTypeCode)
            {
                case ShipmentTypeCode.Usps:
                    return StampsResellerType.StampsExpedited;
                case ShipmentTypeCode.Stamps:
                    return StampsResellerType.None;
                case ShipmentTypeCode.Express1Stamps:
                    return StampsResellerType.Express1;
                default:
                    throw new ArgumentException(string.Format("{0} has no associated StampsResellerType.", EnumHelper.GetDescription(shipmentTypeCode)), "shipmentTypeCode");
            }
        }

        /// <summary>
        /// Returns a new StampsShipmentType for a given StampsResellerType
        /// </summary>
        public static StampsShipmentType GetStampsShipmentTypeForStampsResellerType(StampsResellerType stampsResellerType)
        {
            switch (stampsResellerType)
            {
                case StampsResellerType.None:
                    return new StampsShipmentType();
                case StampsResellerType.StampsExpedited:
                    return new UspsShipmentType();
                case StampsResellerType.Express1:
                    return new Express1StampsShipmentType();
                default:
                    throw new ArgumentException(string.Format("{0} has no associated Shipment Type.", EnumHelper.GetDescription(stampsResellerType)), "stampsResellerType");
            }
        }
    }
}
