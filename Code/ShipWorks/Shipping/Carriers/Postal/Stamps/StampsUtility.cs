using System;
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
                case ServiceType.USBP: return PostalServiceType.LibraryMail;
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
                case PostalServiceType.LibraryMail: return ServiceType.USBP;
                case PostalServiceType.ParcelSelect: return ServiceType.USPS;
                case PostalServiceType.InternationalExpress: return ServiceType.USEMI;
                case PostalServiceType.InternationalPriority: return ServiceType.USPMI;
                case PostalServiceType.InternationalFirst: return ServiceType.USFCI;
                case PostalServiceType.CriticalMail: return ServiceType.USCM;

                default:
                    throw new ShippingException(string.Format("Stamps.com does not support {0}.", EnumHelper.GetDescription(postalServiceType)));
            }
        }

        /// <summary>
        /// Get the Stamps.com ContentType api value from our own internal content type enum
        /// </summary>
        public static ContentType GetApiContentType(PostalCustomsContentType contentType)
        {
            switch (contentType)
            {
                case PostalCustomsContentType.Documents: return ContentType.Document;
                case PostalCustomsContentType.Gift: return ContentType.Gift;
                case PostalCustomsContentType.Merchandise: return ContentType.Other;
                case PostalCustomsContentType.Other: return ContentType.Other;
                case PostalCustomsContentType.ReturnedGoods: return ContentType.ReturnedGoods;
                case PostalCustomsContentType.Sample: return ContentType.CommercialSample;

                default:
                    throw new ShippingException("Unsupported customs content type.");
            }
        }
    }
}
