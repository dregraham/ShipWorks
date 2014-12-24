﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Divelements.SandRibbon;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;
using ShipWorks.Shipping.Editing;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// Utility functions for working with Stamps.com
    /// </summary>
    public static class StampsUtility
    {
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
                    throw new InvalidOperationException(string.Format("Invalid Stamps.com packaging type {0}", packagingType));
            }
        }

        /// <summary>
        /// Get our internal service type that represents the given Stamps.com API service type
        /// </summary>
        public static PostalServiceType GetPostalServiceType(ServiceType stampsServiceType)
        {
            switch (stampsServiceType)
            {
                case ServiceType.USFC: return PostalServiceType.FirstClass;
                case ServiceType.USPM: return PostalServiceType.PriorityMail;
                case ServiceType.USXM: return PostalServiceType.ExpressMail;
                case ServiceType.USMM: return PostalServiceType.MediaMail;
                case ServiceType.USBP: return PostalServiceType.BoundPrintedMatter;
                case ServiceType.USLM: return PostalServiceType.LibraryMail;
                case ServiceType.USPS: return PostalServiceType.ParcelSelect;
                case ServiceType.USEMI: return PostalServiceType.InternationalExpress;
                case ServiceType.USPMI: return PostalServiceType.InternationalPriority;
                case ServiceType.USFCI: return PostalServiceType.InternationalFirst;
                case ServiceType.USCM: return PostalServiceType.CriticalMail;

                default:
                    throw new InvalidOperationException(string.Format("Invalid Stamps.com service type {0}", stampsServiceType));
            }
        }

        /// <summary>
        /// Get the API service type to use for the given postal service type
        /// </summary>
        public static ServiceType GetApiServiceType(PostalServiceType postalServiceType)
        {
            switch (postalServiceType)
            {
                case PostalServiceType.FirstClass: return ServiceType.USFC;
                case PostalServiceType.PriorityMail: return ServiceType.USPM;
                case PostalServiceType.ExpressMail: return ServiceType.USXM;
                case PostalServiceType.MediaMail: return ServiceType.USMM;
                case PostalServiceType.BoundPrintedMatter: return ServiceType.USBP;
                case PostalServiceType.LibraryMail: return ServiceType.USLM;
                case PostalServiceType.ParcelSelect: return ServiceType.USPS;
                case PostalServiceType.InternationalExpress: return ServiceType.USEMI;
                case PostalServiceType.InternationalPriority: return ServiceType.USPMI;
                case PostalServiceType.InternationalFirst: return ServiceType.USFCI;
                case PostalServiceType.CriticalMail: return ServiceType.USCM;
                case PostalServiceType.DhlParcelExpedited: return ServiceType.DHLPE;
                case PostalServiceType.DhlParcelStandard: return ServiceType.DHLPG;
                case PostalServiceType.DhlParcelPlusExpedited: return ServiceType.DHLPPE;
                case PostalServiceType.DhlParcelPlusStandard: return ServiceType.DHLPPE;
                case PostalServiceType.DhlBpmExpedited: return ServiceType.DHLBPME;
                case PostalServiceType.DhlBpmStandard: return ServiceType.DHLBPMG;
                case PostalServiceType.DhlMarketingExpedited: return ServiceType.DHLMPE;
                case PostalServiceType.DhlMarketingStandard:return ServiceType.DHLMPG;

                default:
                    throw new ShippingException(string.Format("Stamps.com does not support {0}.", EnumHelper.GetDescription(postalServiceType)));
            }
        }

        /// <summary>
        /// Get the Stamps.com ContentType api value from our own internal content type enum
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
    }
}
