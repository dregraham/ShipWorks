﻿using System;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Account;
using ShipWorks.Shipping.Carriers.Postal.Express1.WebServices.CustomerService;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// Summary description for Express1Helper.
    /// </summary>
    static class Express1EndiciaUtility
    {
        // Express1
        public const string Express1ProductionUrl = "https://service.express1.com/Services/EwsLabelService.svc";
        public const string Express1DevelopmentUrl = "http://www.express1dev.com/Services/EwsLabelService.svc";

        /// <summary>
        /// Franchise ID
        /// </summary>
        public static string FranchiseID
        {
            get
            {
                return "306";
            }
        }

        /// <summary>
        /// Gets the encryption certificateID
        /// </summary>
        public static string CertificateID
        {
            get
            {
                if (UseTestServer)
                {
                    return "691F6E51-01E4-43BA-B5D4-643644C325DE";
                }
                else
                {
                    return "33DC2051-3645-4D65-B4DE-A5ECC3676EE1";
                }
            }
        }

        /// <summary>
        /// Gets the API Key, "partnerid" in Endicia parlance
        /// </summary>
        public static string ApiKey
        {
            get
            {
                if (UseTestServer)
                {
                    return "1A4E8B5F-99D4-422F-8782-7EBA12DDE312";
                }
                else
                {
                    return "2BFD18C9-EB52-4C64-867C-0E5328D264C8";
                }
            }
        }

        /// <summary>
        /// Determines if the Live Server should be used
        /// </summary>
        public static bool UseTestServer
        {
            get
            {
                return InterapptiveOnly.Registry.GetValue("Express1EndiciaTestServer", false);
            }
            set
            {
                InterapptiveOnly.Registry.SetValue("Express1EndiciaTestServer", value);
            }
        }

        /// <summary>
        /// Gets the Express1 Card type from the Endicia Card Type
        /// </summary>
        public static CreditCardTypeEnum GetExpress1CardType(EndiciaCreditCardType endiciaCreditCardType)
        {
            switch (endiciaCreditCardType)
            {
                case EndiciaCreditCardType.AmericanExpress:
                    return CreditCardTypeEnum.AmericanExpress;
                case EndiciaCreditCardType.Discover:
                    return CreditCardTypeEnum.Discover;
                case EndiciaCreditCardType.MasterCard:
                    return CreditCardTypeEnum.MasterCard;
                case EndiciaCreditCardType.Visa:
                    return CreditCardTypeEnum.Visa;

                default:
                    throw new InvalidOperationException("Unsupported Credit Card type.");
            }
        }


        /// <summary>
        /// Translates Express1 Card Type to Endicia Credit Card Type
        /// </summary>
        public static EndiciaCreditCardType GetEndiciaCardType(CreditCardTypeEnum express1CardType)
        {
            switch (express1CardType)
            {
                case CreditCardTypeEnum.AmericanExpress:
                    return EndiciaCreditCardType.AmericanExpress;
                case CreditCardTypeEnum.Discover:
                    return EndiciaCreditCardType.Discover;
                case CreditCardTypeEnum.Visa:
                    return EndiciaCreditCardType.Visa;
                case CreditCardTypeEnum.MasterCard:
                    return EndiciaCreditCardType.MasterCard;

                default:
                    throw new InvalidOperationException("Unknown Credit Card Type.");
            }
        }
    }
}
