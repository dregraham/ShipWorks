using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using ShipWorks.Data;
using Interapptive.Shared.Business;
using Interapptive.Shared.Win32;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// API entry point for FedEx webservices
    /// </summary>
    public static class FedExApiCore
    {
        static string testingUrl = "https://wsbeta.fedex.com:443/web-services/";
        static string liveUrl = "https://gateway.fedex.com/web-services";

        // Values provided by fedex
        static string cspCredentialKey = "U17ZWjkTkNxIFLhT";
        static string cspCredentialPassword = SecureText.Decrypt("q4GaWogM20TrRxtAmYbTktTNQqvcG7vOyfNDIynBAug=", "apptive");
        static string clientProductID = "ITSW";
        static string clientProductVersion = "9558";

        /// <summary>
        /// Indicates if the test server should be used instead of hte live server
        /// </summary>
        public static bool UseTestServer
        {
            get { return InterapptiveOnly.Registry.GetValue("FedExTestServer", false); }
            set { InterapptiveOnly.Registry.SetValue("FedExTestServer", value); }
        }

        /// <summary>
        /// Get the FedEx server URL to use
        /// </summary>
        public static string ServerUrl
        {
            get { return UseTestServer ? testingUrl : liveUrl; }
        }

        /// <summary>
        /// Indicates if LIST rates are in effect, instead of the standard ACCOUNT rates
        /// </summary>
        public static bool UseListRates
        {
            get { return InterapptiveOnly.Registry.GetValue("FedExListRates", false); }
            set { InterapptiveOnly.Registry.SetValue("FedExListRates", value); }
        }

        /// <summary>
        /// Get the CSP provider credentials
        /// </summary>
        public static T GetCspCredentials<T>() where T: new()
        {
            T credential = new T();

            FillCspCredentials(credential);

            return credential;
        }

        /// <summary>
        /// Fill the given csp credentials object
        /// </summary>
        private static object FillCspCredentials(object credential)
        {
            Duck(credential, "Key", cspCredentialKey);
            Duck(credential, "Password", cspCredentialPassword);

            return credential;
        }

        /// <summary>
        /// Fill the given user credentials object
        /// </summary>
        private static object FillUserCredentials(object credential)
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            Duck(credential, "Key", settings.FedExUsername);
            Duck(credential, "Password", SecureText.Decrypt(settings.FedExPassword, "FedEx"));

            return credential;
        }

        /// <summary>
        /// Create a FedEx API address object from the given person
        /// </summary>
        public static T CreateAddress<T>(PersonAdapter person) where T: new()
        {
            T address = new T();

            List<string> streetLines = new List<string>();
            streetLines.Add(person.Street1);

            if (!string.IsNullOrEmpty(person.Street2))
            {
                streetLines.Add(person.Street2);
            }

            if (!string.IsNullOrEmpty(person.Street3))
            {
                streetLines.Add(person.Street3);
            }

            Duck(address, "StreetLines", streetLines.ToArray());
            Duck(address, "City", person.City);
            Duck(address, "PostalCode", person.PostalCode);
            Duck(address, "StateOrProvinceCode", person.StateProvCode);
            Duck(address, "CountryCode", AdjustFedExCountryCode(person.CountryCode));

            return address;
        }
        
        /// <summary>
        /// Adjust the country code for what FedEx requires expects
        /// </summary>
        private static object AdjustFedExCountryCode(string code)
        {
            // FedEx wants GB
            if (code == "UK")
            {
                code = "GB";
            }

            return code;
        }

        /// <summary>
        /// Create a FedEx API contact object from teh givne person
        /// </summary>
        public static T CreateContact<T>(PersonAdapter person) where T: new()
        {
            T contact = new T();

            Duck(contact, "PersonName", new PersonName(person).FullName);
            Duck(contact, "CompanyName", person.Company);
            Duck(contact, "EMailAddress", person.Email);
            Duck(contact, "FaxNumber", person.Fax);
            Duck(contact, "PhoneNumber", person.Phone);

            return contact;
        }

        /// <summary>
        /// Create a FedEx API "Parsed" contact
        /// </summary>
        public static T CreateParsedContact<T>(PersonAdapter person) where T: new ()
        {
            T contact = new T();

            // We need to know what "ParsedPersonName" type to create
            object parsedPerson = Activator.CreateInstance(typeof(T).GetProperty("PersonName").PropertyType);

            Duck(parsedPerson, "FirstName", person.FirstName);
            Duck(parsedPerson, "LastName", person.LastName);
            Duck(contact, "PersonName", parsedPerson);

            Duck(contact, "CompanyName", person.Company);
            Duck(contact, "EMailAddress", person.Email);
            Duck(contact, "FaxNumber", person.Fax);
            Duck(contact, "PhoneNumber", person.Phone);

            return contact;
        }

        /// <summary>
        /// Get the common WebAuthenticationDetail info
        /// </summary>
        public static T GetWebAuthenticationDetail<T>() where T: new()
        {
            T credential = new T();

            Duck(credential, "CspCredential", FillCspCredentials(Activator.CreateInstance(typeof(T).GetProperty("CspCredential").PropertyType)));
            Duck(credential, "UserCredential", FillUserCredentials(Activator.CreateInstance(typeof(T).GetProperty("UserCredential").PropertyType)));

            return credential;
        }

        /// <summary>
        /// Get the common client detail info
        /// </summary>
        public static T GetClientDetail<T>(FedExAccountEntity account) where T: new()
        {
            T clientDetail = new T();

            Duck(clientDetail, "AccountNumber", account.AccountNumber);
            Duck(clientDetail, "ClientProductId", clientProductID);
            Duck(clientDetail, "ClientProductVersion", clientProductVersion);
            Duck(clientDetail, "MeterNumber", account.MeterNumber);

            return clientDetail;
        }

        /// <summary>
        /// Our psuedo duck typing that sets the given property to the specified value on the given object
        /// </summary>
        public static void Duck(object duck, string property, object value)
        {
            duck.GetType().GetProperty(property).SetValue(duck, value, null);
        }

        /// <summary>
        /// Our psuedo duck typing that gets the given property of the given object
        /// </summary>
        public static object DuckGetProperty(object duck, string property)
        {
            return duck.GetType().GetProperty(property).GetValue(duck, null);
        }
    }
}
