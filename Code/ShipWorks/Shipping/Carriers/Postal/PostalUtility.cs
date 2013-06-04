﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
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
        /// Indicates if the given state code is a military destination.  Only the USPS can ship to military, which is why this is in the PostalUtility
        /// </summary>
        public static bool IsMilitaryState(string stateCode)
        {
            return stateCode == "AA" || stateCode == "AE" || stateCode == "AP";
        }

        /// <summary>
        /// Indicates if the shipment type is a postal shipment
        /// </summary>
        public static bool IsPostalShipmentType(ShipmentTypeCode shipmentTypeCode)
        {
            switch (shipmentTypeCode)
            {
                case ShipmentTypeCode.Stamps:
                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.PostalExpress1:
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

            if (shipmentType == ShipmentTypeCode.PostalExpress1 && !settings.Express1SingleSource)
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

                if (shipmentType == ShipmentTypeCode.Stamps || shipmentType == ShipmentTypeCode.PostalExpress1)
                {
                    // As of the 01/28/2013 changes, Stamps.com does not support Parcel Post (now Standard Post)
                    services.Remove(PostalServiceType.StandardPost);
                }

                if (shipmentType == ShipmentTypeCode.Endicia)
                {
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

            if (shipmentType == ShipmentTypeCode.PostalExpress1 && !settings.Express1SingleSource)
            {
                return new List<PostalServiceType>
                    {
                        PostalServiceType.InternationalPriority,
                        PostalServiceType.InternationalExpress,
                    };
            }
            else
            {
                return new List<PostalServiceType>
                    {
                        PostalServiceType.InternationalPriority,
                        PostalServiceType.InternationalFirst,
                        PostalServiceType.InternationalExpress,

                        // Deprecated values, these get filtered out before being bound to a ComboBox
                        PostalServiceType.ExpressMailPremium,
                        PostalServiceType.GlobalExpressGuaranteed,
                        PostalServiceType.GlobalExpressGuaranteedNonDocument
                    };
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
            if (countryCode == "CA")
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
    }
}
