using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Class for transforming ShipWorks values into Endicia API values
    /// </summary>
    public static class EndiciaApiTransforms
    {
        /// <summary>
        /// Get the mail piece shape code to use for the given packaging type
        /// </summary>
        public static string GetMailpieceShapeCode(PostalPackagingType packagingType)
        {
            switch (packagingType)
            {
                case PostalPackagingType.Envelope: return "Letter";
                case PostalPackagingType.LargeEnvelope: return "Flat";
                case PostalPackagingType.FlatRateEnvelope: return "FlatRateEnvelope";
                case PostalPackagingType.FlatRatePaddedEnvelope: return "FlatRatePaddedEnvelope";
                case PostalPackagingType.FlatRateLegalEnvelope: return "FlatRateLegalEnvelope";
                case PostalPackagingType.FlatRateSmallBox: return "SmallFlatRateBox";
                case PostalPackagingType.FlatRateMediumBox: return "MediumFlatRateBox";
                case PostalPackagingType.FlatRateLargeBox: return "LargeFlatRateBox";
                case PostalPackagingType.Package: return "Parcel";
                case PostalPackagingType.RateRegionalBoxA: return "RegionalRateBoxA";
                case PostalPackagingType.RateRegionalBoxB: return "RegionalRateBoxB";
                case PostalPackagingType.RateRegionalBoxC: return "RegionalRateBoxC";
                case PostalPackagingType.Cubic: return "Cubic";
            }

            throw new InvalidOperationException("Invalid packagingType: " + packagingType);
        }

        /// <summary>
        /// Get the label server value to use for the given customs content type
        /// </summary>
        public static string GetCustomsContentTypeCode(PostalCustomsContentType contentType)
        {
            switch (contentType)
            {
                case PostalCustomsContentType.Documents: return "Documents";
                case PostalCustomsContentType.Gift: return "Gift";
                case PostalCustomsContentType.Merchandise: return "Merchandise";
                case PostalCustomsContentType.Other: return "Other";
                case PostalCustomsContentType.ReturnedGoods: return "ReturnedGoods";
                case PostalCustomsContentType.Sample: return "Sample";
                case PostalCustomsContentType.DangerousGoods: return "Other";
                case PostalCustomsContentType.HumanitarianDonation: return "Other";
            }

            throw new InvalidOperationException("Invalid postal content type: " + contentType);
        }

        /// <summary>
        /// Get the Endicia API sort type code for the given PostalSortType value
        /// </summary>
        public static string GetSortTypeCode(PostalSortType sortType)
        {
            switch (sortType)
            {
                case PostalSortType.SinglePiece: return "SinglePiece";
                case PostalSortType.BMC: return "BMC";
                case PostalSortType.FiveDigit: return "FiveDigit";
                case PostalSortType.MixedBMC: return "MixedBMC";
                case PostalSortType.Nonpresorted: return "Nonpresorted";
                case PostalSortType.Presorted: return "Presorted";
                case PostalSortType.SCF: return "SCF";
                case PostalSortType.ThreeDigit: return "ThreeDigit";
            }

            throw new InvalidOperationException("Invalid postal sort type: " + sortType);
        }

        /// <summary>
        /// Get the Endicia API entry facility value for the given PostalEntryFacility value
        /// </summary>
        public static string GetEntryFacilityCode(PostalEntryFacility entryFacility)
        {
            switch (entryFacility)
            {
                case PostalEntryFacility.Other: return "Other";
                case PostalEntryFacility.DBMC: return "DBMC";
                case PostalEntryFacility.DDU: return "DDU";
                case PostalEntryFacility.DSCF: return "DSCF";
                case PostalEntryFacility.OBMC: return "OBMC";
            }

            throw new InvalidOperationException("Invalid postal entry facility: " + entryFacility);
        }

        /// <summary>
        /// Get the PostalServiceType value representing the returned mail class from the rates request
        /// </summary>
        public static PostalServiceType? GetServiceTypeFromRateMailService(string mailClass)
        {
            switch (mailClass)
            {
                case "Express": return PostalServiceType.ExpressMail;
                case "PriorityExpress": return PostalServiceType.ExpressMail;
                case "First": return PostalServiceType.FirstClass;
                case "LibraryMail": return PostalServiceType.LibraryMail;
                case "MediaMail": return PostalServiceType.MediaMail;
                case "StandardPost": return PostalServiceType.StandardPost;
                case "ParcelSelect": return PostalServiceType.ParcelSelect;
                case "Priority": return PostalServiceType.PriorityMail;
                case "CriticalMail": return PostalServiceType.CriticalMail;

                case "ExpressMailInternational": return PostalServiceType.InternationalExpress;
                case "PriorityMailInternational": return PostalServiceType.InternationalPriority;

                // Endicia changed the value to First Class Package International Service; keeping the original
                // case statement for First Class Mail International per Brian in case Endicia changes it back
                case "FirstClassMailInternational":
                case "FirstClassPackageInternational":
                case "FirstClassPackageInternationalService": return PostalServiceType.InternationalFirst;
            }

            // Known values we ignore
            switch (mailClass)
            {
                case "GXG":
                    return null;
            }

            Debug.Fail("Unknown mailClass value while getting rates: " + mailClass);

            return null;
        }
    }
}
