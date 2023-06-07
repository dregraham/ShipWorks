﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Autofac;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Utility functions for dealing with USPS in general, not a specific USPS integration.
    /// </summary>
    public static class PostalUtility
    {
        /// <summary>
        /// Lookup of presort service types
        /// </summary>
        private static readonly ImmutableHashSet<PostalServiceType> presortServiceTypes =
            new[]
            {
                PostalServiceType.InternationalFirst,
                PostalServiceType.InternationalPriority,
                PostalServiceType.InternationalExpress
            }.ToImmutableHashSet();

        /// <summary>
        /// Lookup of what service types are associated with which edition features
        /// </summary>
        private static readonly Dictionary<EditionFeature, List<PostalServiceType>> uspsConsolidatorServiceTypes =
            new Dictionary<EditionFeature, List<PostalServiceType>>
        {
            {
                EditionFeature.StampsAscendiaConsolidator, new List<PostalServiceType>
                {
                    PostalServiceType.AsendiaGeneric,
                    PostalServiceType.AsendiaIpa,
                    PostalServiceType.AsendiaIsal,
                    PostalServiceType.AsendiaePacket
                }
            },
            {
                EditionFeature.StampsDhlConsolidator, new List<PostalServiceType>
                {
                    PostalServiceType.DhlPacketIpa,
                    PostalServiceType.DhlPacketIsal
                }
            },
            {
                EditionFeature.StampsGlobegisticsConsolidator, new List<PostalServiceType>
                {
                    PostalServiceType.GlobegisticsIpa,
                    PostalServiceType.GlobegisticsIsal,
                    PostalServiceType.GlobegisticsePacket,
                    PostalServiceType.GlobegisticsGeneric
                }
            },
            {
                EditionFeature.StampsIbcConsolidator, new List<PostalServiceType>
                {
                    PostalServiceType.InternationalBondedCouriersIpa,
                    PostalServiceType.InternationalBondedCouriersIsal,
                    PostalServiceType.InternationalBondedCouriersePacket
                }
            },
            {
                EditionFeature.StampsRrDonnelleyConsolidator, new List<PostalServiceType>
                {
                    PostalServiceType.RrdIpa,
                    PostalServiceType.RrdIsal,
                    PostalServiceType.RrdEpsePacketService,
                    PostalServiceType.RrdGeneric
                }
            }
        };

        /// <summary>
        /// This is a list of ranges used for military addresses
        /// http://pe.usps.gov/text/LabelingLists/L002.htm
        /// </summary>
        private static readonly List<string> militaryPostalCodePrefixes = Enumerable
            .Range(90, 9)
            .Concat(new[] { 340 })
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
        /// Indicates if the given postal code is a military destination.
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
                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Indicates if the given foreign country code is considered a postal domestic shipment.
        /// Per http://www.usps.com/ncsc/lookups/abbr_state.txt
        /// </summary>
        public static bool IsDomesticCountry(this IAddressAdapter address)
        {
            MethodConditions.EnsureArgumentIsNotNull(address, nameof(address));

            return address.CountryCode == "US" ||
                address.CountryCode == "DC" ||
                address.IsUSInternationalTerritory();
        }

        /// <summary>
        /// Get the domestic services valid for the given shipment type
        /// </summary>
        public static List<PostalServiceType> GetDomesticServices(ShipmentTypeCode shipmentType)
        {
            IShippingSettingsEntity settings = ShippingSettings.FetchReadOnly();

            if ((shipmentType == ShipmentTypeCode.Express1Endicia && !settings.Express1EndiciaSingleSource) ||
               (shipmentType == ShipmentTypeCode.Express1Usps && !settings.Express1UspsSingleSource))
            {
                return new List<PostalServiceType>
                    {
                        PostalServiceType.PriorityMail,
                        PostalServiceType.ExpressMail
                    };
            }

            var services = new List<PostalServiceType>
            {
                PostalServiceType.PriorityMail,
                PostalServiceType.ExpressMail,
                PostalServiceType.GroundAdvantage,
                PostalServiceType.FirstClass,
                PostalServiceType.ParcelSelect,
                PostalServiceType.MediaMail,
                PostalServiceType.LibraryMail,
                PostalServiceType.CriticalMail,
                PostalServiceType.StandardPost,
            };

            if (shipmentType == ShipmentTypeCode.Usps || shipmentType == ShipmentTypeCode.Express1Usps ||
                shipmentType == ShipmentTypeCode.Express1Endicia)
            {
                // As of the 01/28/2013 changes, Stamps.com does not support Parcel Post (now Standard Post)
                services.Remove(PostalServiceType.StandardPost);
            }

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ILicenseService licenseService = lifetimeScope.Resolve<ILicenseService>();

                if (shipmentType == ShipmentTypeCode.Endicia)
                {
                    // If consolidation is supported, add it in
                    if (licenseService.CheckRestriction(EditionFeature.EndiciaConsolidator, null) == EditionRestrictionLevel.None)
                    {
                        services.Add(PostalServiceType.ConsolidatorDomestic);
                    }

                    // If not restricted from Endicia DHL, add them in
                    if (licenseService.CheckRestriction(EditionFeature.EndiciaDhl, null) == EditionRestrictionLevel.None)
                    {
                        services.AddRange(EnumHelper.GetEnumList<PostalServiceType>(ShipmentTypeManager.IsEndiciaDhl)
                            .Select(entry => entry.Value));
                    }
                }

                if (shipmentType == ShipmentTypeCode.Usps)
                {
                    // If not restricted from Stamps DHL, add them in
                    if (licenseService.CheckRestriction(EditionFeature.StampsDhl, null) == EditionRestrictionLevel.None)
                    {
                        services.AddRange(EnumHelper.GetEnumList<PostalServiceType>(ShipmentTypeManager.IsStampsDhl)
                            .Select(entry => entry.Value));
                    }
                    services.Add(PostalServiceType.PayOnUseReturn);
                }
            }

            return services;
        }

        /// <summary>
        /// Get the international services supported by the given shipment type
        /// </summary>
        public static List<PostalServiceType> GetInternationalServices(ShipmentTypeCode shipmentType)
        {
            IShippingSettingsEntity settings = ShippingSettings.FetchReadOnly();

            if ((shipmentType == ShipmentTypeCode.Express1Endicia && !settings.Express1EndiciaSingleSource) ||
               (shipmentType == ShipmentTypeCode.Express1Usps && !settings.Express1UspsSingleSource))
            {
                return new List<PostalServiceType>
                    {
                        PostalServiceType.InternationalPriority,
                        PostalServiceType.InternationalExpress
                    };
            }

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

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ILicenseService licenseService = lifetimeScope.Resolve<ILicenseService>();

                if (shipmentType == ShipmentTypeCode.Endicia)
                {
                    // If consolidation is supported, add it in
                    if (licenseService.CheckRestriction(EditionFeature.EndiciaConsolidator, null) == EditionRestrictionLevel.None)
                    {
                        services.Add(PostalServiceType.ConsolidatorInternational);
                        services.Add(PostalServiceType.ConsolidatorIpa);
                        services.Add(PostalServiceType.ConsolidatorIsal);
                        services.Add(PostalServiceType.CommercialePacket);
                    }
                }

                if (shipmentType == ShipmentTypeCode.Usps)
                {
                    // Get a list of any consolidators that should be available to customers
                    IEnumerable<PostalServiceType> accesibleConsolidatorTypes = uspsConsolidatorServiceTypes
                        .Where(x => licenseService.CheckRestriction(x.Key, null) == EditionRestrictionLevel.None)
                        .SelectMany(x => x.Value)
                        .ToList();

                    services.AddRange(accesibleConsolidatorTypes);

                    IGlobalPostAvailabilityService globalPostAvailabilityService = lifetimeScope.Resolve<IGlobalPostAvailabilityService>();
                    // Add global post services
                    services.InsertRange(0, globalPostAvailabilityService.Services);
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
                // As per Stamps.com, envelopes under 16 oz do not require customs.
                // https://stamps.custhelp.com/app/answers/detail/a_id/406/related/1
                if ((PostalPackagingType) shipment.Postal.PackagingType == PostalPackagingType.Envelope &&
                    shipment.TotalWeight < 1)
                {
                    return PostalCustomsForm.None;
                }

                return (shipment.TotalWeight >= 1 || shipment.CustomsValue >= 400 || shipment.CustomsItems.Count >= 6) ?
                    PostalCustomsForm.CN72 :
                    PostalCustomsForm.CN22;
            }

            if (shipment.ShipPerson.IsDomesticCountry() && !ShipmentTypeManager.GetType(shipment).IsCustomsRequired(shipment))
            {
                return PostalCustomsForm.None;
            }

            return (shipment.CustomsValue >= 400 || shipment.CustomsItems.Count >= 6) ?
                    PostalCustomsForm.CN72 :
                    PostalCustomsForm.CN22;
        }

        /// <summary>
        /// Indicates if the packaging type represents an envelope or flat
        /// </summary>
        public static bool IsEnvelopeOrFlat(PostalPackagingType? packagingType)
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
        /// Set service details and shipment type for rate
        /// </summary>
        public static RateResult SetServiceDetails(RateResult rate, PostalServiceType serviceType,
                                             string deliveryDays, ShipmentTypeCode shipmentTypeCode)
        {
            SetServiceDetails(rate, serviceType, deliveryDays);
            rate.ShipmentType = shipmentTypeCode;
            return rate;
        }

        /// <summary>
        /// Gets a state adjusted for what the post office wants
        /// </summary>
        /// <param name="countryCode">Country code for the address</param>
        /// <param name="state">State for the address</param>
        /// <returns>State code that should be used for a postal address</returns>
        public static string AdjustState(string countryCode, string state)
        {
            switch (countryCode.ToUpper())
            {
                case "PR":
                    return "PR";
                case "VI":
                    return "VI";
                default:
                    return state;
            }
        }

        /// <summary>
        /// Determines whether any postal account exists.
        /// </summary>
        public static bool IsPostalSetup()
        {
            return EndiciaAccountManager.EndiciaAccounts.Any() ||
                   EndiciaAccountManager.Express1Accounts.Any() ||
                   UspsAccountManager.Express1Accounts.Any() ||
                   UspsAccountManager.UspsAccounts.Any();
        }

        /// <summary>
        /// Returns the UspsResellerType for a ShipmentTypeCode
        /// </summary>
        public static UspsResellerType GetUspsResellerType(ShipmentTypeCode shipmentTypeCode)
        {
            switch (shipmentTypeCode)
            {
                case ShipmentTypeCode.Usps:
                    return UspsResellerType.None;
                case ShipmentTypeCode.Express1Usps:
                    return UspsResellerType.Express1;
                default:
                    throw new ArgumentException(string.Format("{0} has no associated UspsResellerType.", EnumHelper.GetDescription(shipmentTypeCode)), "shipmentTypeCode");
            }
        }

        /// <summary>
        /// Returns a new UspsShipmentType for a given UspsResellerType
        /// </summary>
        public static UspsShipmentType GetUspsShipmentTypeForUspsResellerType(UspsResellerType uspsResellerType)
        {
            switch (uspsResellerType)
            {
                case UspsResellerType.None:
                    return new UspsShipmentType();
                case UspsResellerType.Express1:
                    return new Express1UspsShipmentType();
                default:
                    throw new ArgumentException(string.Format("{0} has no associated Shipment Type.", EnumHelper.GetDescription(uspsResellerType)), "uspsResellerType");
            }
        }

        /// <summary>
        /// Gets a list of postal service types that are for Usps consolidators
        /// </summary>
        public static IEnumerable<PostalServiceType> UspsConsolidatorTypes
        {
            get
            {
                return uspsConsolidatorServiceTypes.SelectMany(x => x.Value);
            }
        }

        /// <summary>
        /// Initialize the package picker control
        /// </summary>
        [CLSCompliant(false)]
        public static void InitializePackagePicker(PackageTypePickerControl<PostalPackagingType> packagePicker, ShipmentType shipmentType)
        {
            IEnumerable<PostalPackagingType> excludedServices = shipmentType.GetExcludedPackageTypes()
                .Cast<PostalPackagingType>();

            IEnumerable<PostalPackagingType> postalServices = EnumHelper.GetEnumList<PostalPackagingType>()
                .Select(p => p.Value)
                .Where(p => p != PostalPackagingType.Cubic || shipmentType.ShipmentTypeCode == ShipmentTypeCode.Express1Endicia);

            packagePicker.Initialize(postalServices, excludedServices);
        }

        /// <summary>
        /// Initialize the service picker control
        /// </summary>
        [CLSCompliant(false)]
        public static void InitializeServicePicker(ServicePickerControl<PostalServiceType> servicePicker, ShipmentType shipmentType)
        {
            IEnumerable<PostalServiceType> excludedServices = shipmentType.GetExcludedServiceTypes()
                .Cast<PostalServiceType>()
                .ToList();

            IEnumerable<PostalServiceType> postalServices = GetDomesticServices(shipmentType.ShipmentTypeCode)
                .Union(GetInternationalServices(shipmentType.ShipmentTypeCode))
                .ToList();

            servicePicker.Initialize(postalServices, excludedServices);
        }

        /// <summary>
        /// Determines whether the specified service type is GlobalPost.
        /// </summary>
        public static bool IsGlobalPost(PostalServiceType serviceType)
        {
            PostalServiceType[] postalServiceTypes = {
                PostalServiceType.GlobalPostEconomyIntl, PostalServiceType.GlobalPostStandardIntl,
                PostalServiceType.GlobalPostSmartSaverEconomyIntl, PostalServiceType.GlobalPostSmartSaverStandardIntl,
                PostalServiceType.GlobalPostPlus, PostalServiceType.GlobalPostPlusSmartSaver
            };

            return postalServiceTypes.Contains(serviceType);
        }

        /// <summary>
        /// Is the shipment a GAP label
        /// </summary>
        public static bool IsGapLabel(IPostalShipmentEntity shipment) =>
            shipment.Service == (int) PostalServiceType.InternationalFirst &&
                shipment.CustomsContentType != (int) PostalCustomsContentType.Documents &&
                (shipment.PackagingType == (int) PostalPackagingType.Envelope ||
                    shipment.PackagingType == (int) PostalPackagingType.LargeEnvelope);

        /// <summary>
        /// Is the service a Presort service
        /// </summary>
        public static bool IsPresort(IPostalShipmentEntity postalShipment) =>
            !IsGapLabel(postalShipment) && presortServiceTypes.Contains((PostalServiceType) postalShipment.Service);

        /// <summary>
        /// Get the days string to display in the rate grid - 4 (Monday)
        /// </summary>
        public static string GetDaysForRate(string deliverDays, DateTime deliveryDate)
        {
            return deliveryDate == DateTime.MinValue ? deliverDays : $"{deliverDays} ({deliveryDate.DayOfWeek})";
        }
    }
}
