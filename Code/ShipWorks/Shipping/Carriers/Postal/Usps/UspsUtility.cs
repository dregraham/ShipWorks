using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using ShipWorks.Shipping.Editing;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Utility functions for working with USPS
    /// </summary>
    public static class UspsUtility
    {
        private static readonly Dictionary<ServiceType, PostalServiceType> uspsServiceTypeTranslation = new Dictionary<ServiceType, PostalServiceType>
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
			{ServiceType.DHLPG, PostalServiceType.DhlParcelStandard},
            {ServiceType.DHLPPE, PostalServiceType.DhlParcelPlusExpedited},
            {ServiceType.DHLPPE, PostalServiceType.DhlParcelPlusStandard},
            {ServiceType.DHLBPME, PostalServiceType.DhlBpmExpedited},
            {ServiceType.DHLBPMG, PostalServiceType.DhlBpmStandard},
            {ServiceType.DHLMPE, PostalServiceType.DhlMarketingExpedited},
            {ServiceType.DHLMPG, PostalServiceType.DhlMarketingStandard},
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
                .Select(pair => (ServiceType?)pair.Key)
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

    }
}
