﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
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

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Utility functions for working with USPS
    /// </summary>
    public static class UspsUtility
    {
        private static readonly Dictionary<ServiceType, PostalServiceType> uspsServiceTypeTranslation =
            new Dictionary<ServiceType, PostalServiceType>
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
            {ServiceType.SCGPE, PostalServiceType.GlobalPostEconomyIntl},
            {ServiceType.SCGPP, PostalServiceType.GlobalPostStandardIntl},
            {ServiceType.SCGPESS, PostalServiceType.GlobalPostSmartSaverEconomyIntl},
            {ServiceType.SCGPPSS, PostalServiceType.GlobalPostSmartSaverStandardIntl},
            {ServiceType.SCGPLSS, PostalServiceType.GlobalPostPlusSmartSaver },
            {ServiceType.SCGPL, PostalServiceType.GlobalPostPlus },
            {ServiceType.USRETURN, PostalServiceType.UspsPayOnUseReturn }
        };

        /// <summary>
        /// Get the API value for the given packaging type
        /// </summary>
        public static PackageTypeV11 GetApiPackageType(PostalPackagingType packagingType, DimensionsAdapter dimensions)
        {
            switch (packagingType)
            {
                case PostalPackagingType.FlatRateSmallBox: return PackageTypeV11.SmallFlatRateBox;
                case PostalPackagingType.FlatRateMediumBox: return PackageTypeV11.FlatRateBox;
                case PostalPackagingType.FlatRateLargeBox: return PackageTypeV11.LargeFlatRateBox;
                case PostalPackagingType.FlatRateEnvelope: return PackageTypeV11.FlatRateEnvelope;
                case PostalPackagingType.LargeEnvelope: return PackageTypeV11.LargeEnvelopeorFlat;
                case PostalPackagingType.Envelope: return PackageTypeV11.Letter;
                case PostalPackagingType.Package:
                    if (dimensions.Length + dimensions.Girth > 108)
                    {
                        return PackageTypeV11.OversizedPackage;
                    }
                    else
                    {
                        return PackageTypeV11.Package;
                    }

                case PostalPackagingType.FlatRatePaddedEnvelope: return PackageTypeV11.FlatRatePaddedEnvelope;
                case PostalPackagingType.FlatRateLegalEnvelope: return PackageTypeV11.LegalFlatRateEnvelope;
                case PostalPackagingType.RateRegionalBoxA: return PackageTypeV11.RegionalRateBoxA;
                case PostalPackagingType.RateRegionalBoxB: return PackageTypeV11.RegionalRateBoxB;
                case PostalPackagingType.RateRegionalBoxC: return PackageTypeV11.RegionalRateBoxC;
                case PostalPackagingType.CubicSoftPack: return PackageTypeV11.LargeEnvelopeorFlat;

                default:
                    throw new InvalidOperationException(string.Format("Invalid USPS packaging type {0}", packagingType));
            }
        }

        /// <summary>
        /// Get our internal service type that represents the given USPS API service type
        /// </summary>
        public static GenericResult<PostalServiceType> GetPostalServiceType(ServiceType uspsServiceType)
        {
            if (uspsServiceTypeTranslation.ContainsKey(uspsServiceType))
            {
                return uspsServiceTypeTranslation[uspsServiceType];
            }

            return new InvalidOperationException(string.Format("Invalid USPS service type {0}", uspsServiceType));
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
        public static bool IsStampsInsuranceActive => IsStampsInsuranceAllowed && ShippingSettings.FetchReadOnly().UspsInsuranceProvider == (int) InsuranceProvider.Carrier;

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
        public static string StampsInsuranceDisplayName => "Stamps.com Insurance";

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
