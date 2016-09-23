﻿using Autofac;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Settings;
using ShipWorks.Templates.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Utility functions for working with USPS
    /// </summary>
    public static class UspsUtility
    {
        private static readonly Dictionary<ServiceType, PostalServiceType> uspsServiceTypeTranslation = new Dictionary
            <ServiceType, PostalServiceType>
        {
            {ServiceType.USFC, PostalServiceType.FirstClass},
            {ServiceType.USPM, PostalServiceType.PriorityMail},
            {ServiceType.USXM, PostalServiceType.ExpressMail},
            {ServiceType.USMM, PostalServiceType.MediaMail},
            {ServiceType.USBP, PostalServiceType.BoundPrintedMatter},
            {ServiceType.USLM, PostalServiceType.LibraryMail},
            {ServiceType.USPS, PostalServiceType.ParcelSelect},
            {ServiceType.USEMI, PostalServiceType.InternationalExpress},
            {ServiceType.USPMI, PostalServiceType.InternationalPriority},
            {ServiceType.USFCI, PostalServiceType.InternationalFirst},
            {ServiceType.USCM, PostalServiceType.CriticalMail},
            {ServiceType.ASIPA, PostalServiceType.AsendiaIpa},
            {ServiceType.ASISAL, PostalServiceType.AsendiaIsal},
            {ServiceType.ASEPKT, PostalServiceType.AsendiaePacket},
            {ServiceType.ASGNRC, PostalServiceType.AsendiaGeneric},
            {ServiceType.DHLPIPA, PostalServiceType.DhlPacketIpa},
            {ServiceType.DHLPISAL, PostalServiceType.DhlPacketIsal},
            {ServiceType.GGIPA, PostalServiceType.GlobegisticsIpa},
            {ServiceType.GGISAL, PostalServiceType.GlobegisticsIsal},
            {ServiceType.GGEPKT, PostalServiceType.GlobegisticsePacket},
            {ServiceType.GGGNRC, PostalServiceType.GlobegisticsGeneric},
            {ServiceType.IBCIPA, PostalServiceType.InternationalBondedCouriersIpa},
            {ServiceType.IBCISAL, PostalServiceType.InternationalBondedCouriersIsal},
            {ServiceType.IBCEPKT, PostalServiceType.InternationalBondedCouriersePacket},
            {ServiceType.RRDIPA, PostalServiceType.RrdIpa},
            {ServiceType.RRDISAL, PostalServiceType.RrdIsal},
            {ServiceType.RRDEPKT, PostalServiceType.RrdEpsePacketService},
            {ServiceType.RRDGNRC, PostalServiceType.RrdGeneric},
            {ServiceType.DHLPG, PostalServiceType.DhlParcelGround},
            {ServiceType.DHLPE, PostalServiceType.DhlParcelExpedited},
            {ServiceType.DHLPPE, PostalServiceType.DhlParcelPlusExpedited},
            {ServiceType.DHLPPG, PostalServiceType.DhlParcelPlusGround},
            {ServiceType.DHLBPME, PostalServiceType.DhlBpmExpedited},
            {ServiceType.DHLBPMG, PostalServiceType.DhlBpmGround},
            {ServiceType.DHLMPE, PostalServiceType.DhlMarketingExpedited},
            {ServiceType.DHLMPG, PostalServiceType.DhlMarketingGround},
            {ServiceType.SCGPE, PostalServiceType.GlobalPostEconomy},
            {ServiceType.SCGPP, PostalServiceType.GlobalPostPriority},
            {ServiceType.SCGPESS, PostalServiceType.GlobalPostSmartSaverEconomy},
            {ServiceType.SCGPPSS, PostalServiceType.GlobalPostSmartSaverPriority}
        };

        /// <summary>
        /// Get the API value for the given packaging type
        /// </summary>
        public static PackageTypeV6 GetApiPackageType(PostalPackagingType packagingType, DimensionsAdapter dimensions)
        {
            switch (packagingType)
            {
                case PostalPackagingType.FlatRateSmallBox: return PackageTypeV6.SmallFlatRateBox;
                case PostalPackagingType.FlatRateMediumBox: return PackageTypeV6.FlatRateBox;
                case PostalPackagingType.FlatRateLargeBox: return PackageTypeV6.LargeFlatRateBox;
                case PostalPackagingType.FlatRateEnvelope: return PackageTypeV6.FlatRateEnvelope;
                case PostalPackagingType.LargeEnvelope: return PackageTypeV6.LargeEnvelopeorFlat;
                case PostalPackagingType.Envelope: return PackageTypeV6.Letter;
                case PostalPackagingType.Package:
                    if (dimensions.Length + dimensions.Girth > 108)
                    {
                        return PackageTypeV6.OversizedPackage;
                    }
                    else
                    {
                        return PackageTypeV6.Package;
                    }

                case PostalPackagingType.FlatRatePaddedEnvelope: return PackageTypeV6.FlatRatePaddedEnvelope;
                case PostalPackagingType.FlatRateLegalEnvelope: return PackageTypeV6.LegalFlatRateEnvelope;
                case PostalPackagingType.RateRegionalBoxA: return PackageTypeV6.RegionalRateBoxA;
                case PostalPackagingType.RateRegionalBoxB: return PackageTypeV6.RegionalRateBoxB;
                case PostalPackagingType.RateRegionalBoxC: return PackageTypeV6.RegionalRateBoxC;

                default:
                    throw new InvalidOperationException(string.Format("Invalid USPS packaging type {0}", packagingType));
            }
        }

        /// <summary>
        /// Get our internal service type that represents the given USPS API service type
        /// </summary>
        public static PostalServiceType GetPostalServiceType(ServiceType uspsServiceType)
        {
            if (uspsServiceTypeTranslation.ContainsKey(uspsServiceType))
            {
                return uspsServiceTypeTranslation[uspsServiceType];
            }

            throw new InvalidOperationException(string.Format("Invalid USPS service type {0}", uspsServiceType));
        }

        /// <summary>
        /// Get the API service type to use for the given postal service type
        /// </summary>
        public static ServiceType GetApiServiceType(PostalServiceType postalServiceType)
        {
            ServiceType? serviceType = uspsServiceTypeTranslation
                .Where(pair => pair.Value == postalServiceType)
                .Select(pair => (ServiceType?) pair.Key)
                .FirstOrDefault();

            if (serviceType.HasValue)
            {
                return serviceType.Value;
            }
            throw new ShippingException(string.Format("USPS does not support {0}.", EnumHelper.GetDescription(postalServiceType)));
        }

        /// <summary>
        /// Get the USPS ContentType api value from our own internal content type enum
        /// </summary>
        public static ContentTypeV2 GetApiContentType(PostalCustomsContentType contentType)
        {
            switch (contentType)
            {
                case PostalCustomsContentType.Documents: return ContentTypeV2.Document;
                case PostalCustomsContentType.Gift: return ContentTypeV2.Gift;
                case PostalCustomsContentType.Merchandise: return ContentTypeV2.Merchandise;
                case PostalCustomsContentType.Other: return ContentTypeV2.Other;
                case PostalCustomsContentType.ReturnedGoods: return ContentTypeV2.ReturnedGoods;
                case PostalCustomsContentType.Sample: return ContentTypeV2.CommercialSample;
                case PostalCustomsContentType.HumanitarianDonation: return ContentTypeV2.HumanitarianDonation;
                case PostalCustomsContentType.DangerousGoods: return ContentTypeV2.DangerousGoods;

                default:
                    throw new ShippingException("Unsupported customs content type.");
            }
        }

        /// <summary>
        /// Builds the string to send to USPS as the memo.  If multiple memo fields have content, the wrap character is injected
        /// between each.
        /// </summary>
        public static string BuildMemoField(PostalShipmentEntity postalShipment)
        {
            string memo1 = TemplateTokenProcessor.ProcessTokens(postalShipment.Memo1, postalShipment.ShipmentID).Truncate(200);
            string memo2 = TemplateTokenProcessor.ProcessTokens(postalShipment.Memo2, postalShipment.ShipmentID).Truncate(200);
            string memo3 = TemplateTokenProcessor.ProcessTokens(postalShipment.Memo3, postalShipment.ShipmentID).Truncate(200);

            var memo = string.Format("\x09{0}\r\n{1}\r\n{2}", memo1, memo2, memo3).Truncate(200);

            return memo;
        }

        /// <summary>
        /// Indicates if Stamps insurance is allowed, turned on, and activated
        /// </summary>
        public static bool IsStampsInsuranceActive
        {
            get
            {
                return IsStampsInsuranceAllowed && ShippingSettings.FetchReadOnly().UspsInsuranceProvider == (int) InsuranceProvider.Carrier;
            }
        }

        /// <summary>
        /// Indicates if Stamps.com insurance is allowed
        /// </summary>
        public static bool IsStampsInsuranceAllowed
        {
            get
            {
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    ILicenseService licenseService = lifetimeScope.Resolve<ILicenseService>();
                    EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.StampsInsurance, null);

                    // If scan based returns is not allowed, show the the default returns control
                    return restrictionLevel == EditionRestrictionLevel.None;
                }
            }
        }

        /// <summary>
        /// Gets the display name of Stamps.com insurance
        /// </summary>
        public static string StampsInsuranceDisplayName
        {
            get
            {
                return "Stamps.com Insurance";
            }
        }

        /// <summary>
        /// Determines whether the specified service type is for one of the international consolidators supported by USPS.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns><c>true</c> if the service is an international consolidator service type; otherwise, <c>false</c>.</returns>
        public static bool IsInternationalConsolidatorServiceType(PostalServiceType serviceType)
        {
            return IsAsendiaServiceType(serviceType) ||
                IsGlobegisticsServiceType(serviceType) ||
                IsInternationalBondedCouriersServiceType(serviceType) ||
                IsRrDonnellyServiceType(serviceType) ||
                IsInternationalDhlServiceType(serviceType);
        }

        /// <summary>
        /// Determines whether the specified service type is an Asendia service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns><c>true</c> if the service is an Asendia service type; otherwise, <c>false</c>.</returns>
        public static bool IsAsendiaServiceType(PostalServiceType serviceType)
        {
            return serviceType == PostalServiceType.AsendiaIsal ||
                serviceType == PostalServiceType.AsendiaIpa ||
                serviceType == PostalServiceType.AsendiaGeneric ||
                serviceType == PostalServiceType.AsendiaePacket;
        }

        /// <summary>
        /// Determines whether the specified service type is a Globegistics service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns><c>true</c> if the serivce is a Globegistics service type; otherwise, <c>false</c>.</returns>
        public static bool IsGlobegisticsServiceType(PostalServiceType serviceType)
        {
            return serviceType == PostalServiceType.GlobegisticsGeneric ||
                serviceType == PostalServiceType.GlobegisticsIpa ||
                serviceType == PostalServiceType.GlobegisticsIsal ||
                serviceType == PostalServiceType.GlobegisticsePacket;
        }

        /// <summary>
        /// Determines whether the specified service type is a InternationalBondedCouriers service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns><c>true</c> if the serivce is a InternationalBondedCouriers service type; otherwise, <c>false</c>.</returns>
        public static bool IsInternationalBondedCouriersServiceType(PostalServiceType serviceType)
        {
            return serviceType == PostalServiceType.InternationalBondedCouriersIpa ||
                serviceType == PostalServiceType.InternationalBondedCouriersIsal ||
                serviceType == PostalServiceType.InternationalBondedCouriersePacket;
        }

        /// <summary>
        /// Determines whether the specified service type is a RRDonnelly service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns><c>true</c> if the serivce is a RRDonnelly service type; otherwise, <c>false</c>.</returns>
        public static bool IsRrDonnellyServiceType(PostalServiceType serviceType)
        {
            return serviceType == PostalServiceType.RrdIpa ||
                serviceType == PostalServiceType.RrdIsal ||
                serviceType == PostalServiceType.RrdEpsePacketService ||
                serviceType == PostalServiceType.RrdGeneric;
        }

        /// <summary>
        /// Determines whether the specified service type is a DHL international service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns><c>true</c> if the serivce is a DHL international service type; otherwise, <c>false</c>.</returns>
        public static bool IsInternationalDhlServiceType(PostalServiceType serviceType)
        {
            return serviceType == PostalServiceType.DhlPacketIpa ||
                serviceType == PostalServiceType.DhlPacketIsal;
        }
    }
}
