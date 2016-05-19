using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Xml;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Settings;
using ShipWorks.Editions;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// UPS utility functions
    /// </summary>
    public static class UpsUtility
    {
		private static Lazy<bool> hasSurePostShipments = new Lazy<bool>(SurePostShipmentsExist);
        private static IEnumerable<UpsServiceType> surePostShipmentTypes;

        /// <summary>
        /// Static Constructor
        /// </summary>
        static UpsUtility()
        {
        }

        /// <summary>
        /// Gets a value indicating whether this instance has sure post shipments.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has sure post shipments; otherwise, <c>false</c>.
        /// </value>
        public static bool HasSurePostShipments
        {
            get
            {
                return hasSurePostShipments.Value;
            }
        }

        /// <summary>
        /// Create a new package entity that has default values
        /// </summary>
        public static UpsPackageEntity CreateDefaultPackage()
        {
            UpsPackageEntity package = new UpsPackageEntity();

            package.PackagingType = (int) UpsPackagingType.Custom;
            package.Weight = 0;

            package.DimsProfileID = 0;
            package.DimsLength = 0;
            package.DimsWidth = 0;
            package.DimsHeight = 0;
            package.DimsWeight = 0;
            package.DimsAddWeight = true;

            package.Insurance = false;
            package.InsuranceValue = 0;
            package.InsurancePennyOne = false;
            package.DeclaredValue = 0;

            package.TrackingNumber = "";
            package.UspsTrackingNumber = "";

			package.AdditionalHandlingEnabled = false;

            package.DryIceEnabled = false;
            package.DryIceIsForMedicalUse = false;
            package.DryIceRegulationSet = (int) UpsDryIceRegulationSet.Cfr;
            package.DryIceWeight = 0;

			package.VerbalConfirmationEnabled = false;
            package.VerbalConfirmationName = "";
            package.VerbalConfirmationPhone = "";
            package.VerbalConfirmationPhoneExtension = "";

            return package;
        }

        /// <summary>
        /// Gets the user-facing description of the various UPS Returns service types
        /// </summary>
        public static string GetReturnServiceExplanation(UpsReturnServiceType returnServiceType)
        {
            switch (returnServiceType)
            {
                case UpsReturnServiceType.ElectronicReturnLabel:
                    return "UPS will email a return label to the customer.";
                case UpsReturnServiceType.PrintAndMail:
                    return "UPS will print and mail a return label to the customer.";
                case UpsReturnServiceType.PrintReturnLabel:
                    return "Print a return label to include in an outbound shipment.";
                case UpsReturnServiceType.ReturnPlus1:
                    return "UPS will make one attempt to pickup the package from your customer.";
                case UpsReturnServiceType.ReturnPlus3:
                    return "UPS will make three attempts to pickup the package from your customer.";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Determines if the Return Service specified will result in a label image ShipWorks must handle.
        /// </summary>
        public static bool ReturnServiceHasLabels(UpsReturnServiceType returnServiceType)
        {
            if (returnServiceType == UpsReturnServiceType.PrintReturnLabel)
            {
                return true;
            }
            else
            {
                // all other methods involve UPS handling the label image
                return false;
            }
        }

        /// <summary>
        /// Gets the packaging types that are conditionally avaiable for Mail Innovations
        /// </summary>
        private static List<UpsPackagingType> GetMailInnovationsPackagingTypes()
        {
            List<UpsPackagingType> types = new List<UpsPackagingType>();
            types.Add(UpsPackagingType.FirstClassMail);
            types.Add(UpsPackagingType.PriorityMail);
            types.Add(UpsPackagingType.BPMFlats);
            types.Add(UpsPackagingType.BPMParcels);
            types.Add(UpsPackagingType.Irregulars);
            types.Add(UpsPackagingType.Machinables);
            types.Add(UpsPackagingType.MediaMail);
            types.Add(UpsPackagingType.ParcelPost);
            types.Add(UpsPackagingType.StandardFlats);

            // Mail Innovaitons International, not going to filter out based on country
            types.Add(UpsPackagingType.Flats);
            types.Add(UpsPackagingType.BPM);
            types.Add(UpsPackagingType.Parcels);


            return types;
        }

        /// <summary>
        /// Get hte valid package types based on the shipment type
        /// </summary>
        /// <param name="shipmentTypeCode"></param>
        public static List<UpsPackagingType> GetValidPackagingTypes(ShipmentTypeCode shipmentTypeCode)
        {
            List<UpsPackagingType> packageTypes = new List<UpsPackagingType>();

            packageTypes.Add(UpsPackagingType.Custom);
            packageTypes.Add(UpsPackagingType.Letter);
            packageTypes.Add(UpsPackagingType.Tube);
            packageTypes.Add(UpsPackagingType.Pak);
            packageTypes.Add(UpsPackagingType.BoxExpressSmall);
            packageTypes.Add(UpsPackagingType.BoxExpressMedium);
            packageTypes.Add(UpsPackagingType.BoxExpressLarge);
            packageTypes.Add(UpsPackagingType.Box25Kg);
            packageTypes.Add(UpsPackagingType.Box10Kg);

            // Canadian package types
            packageTypes.Add(UpsPackagingType.BoxExpress);
            packageTypes.Add(UpsPackagingType.ExpressEnvelope);

            UpsShipmentType upsShipmentType = (UpsShipmentType)ShipmentTypeManager.GetType(shipmentTypeCode);
            if (upsShipmentType.IsMailInnovationsEnabled())
            {
                packageTypes.AddRange(GetMailInnovationsPackagingTypes());
            }

            return packageTypes.Distinct().ToList();
        }

        /// <summary>
        /// Indicates if the DocumentsOnly flag is required when shipping to the given country
        /// </summary>
        public static bool IsDocumentsOnlyRequired(string originCountryCode, string destinationCountryCode)
        {
            if (originCountryCode == "US")
            {
                return (
                           destinationCountryCode != "US" &&
                           destinationCountryCode != "CA" &&
                           destinationCountryCode != "PR" &&
                           destinationCountryCode != "MX");
            }

            return originCountryCode != destinationCountryCode;
        }

        /// <summary>
        /// Gets the currency for the country in the account.
        /// </summary>
        public static string GetCurrency(UpsAccountEntity account)
        {
            if (account==null)
            {
                throw new ArgumentNullException("account","UPS account passed into GetCurrency is null.");
            }

            return EnumHelper.GetApiValue(ShipmentType.GetCurrencyForCountryCode(account.CountryCode));
        }

        /// <summary>
        /// Indicates if the service\date combination is available for saturday delivery
        /// </summary>
        public static bool CanDeliverOnSaturday(UpsServiceType service, DateTime dateTime)
        {
            if (service == UpsServiceType.Ups2DayAir && dateTime.DayOfWeek == DayOfWeek.Thursday)
            {
                return true;
            }

            if ((service == UpsServiceType.UpsNextDayAir || service == UpsServiceType.UpsNextDayAirAM) && dateTime.DayOfWeek == DayOfWeek.Friday)
            {
                return true;
            }

            if (service == UpsServiceType.WorldwideExpress || service == UpsServiceType.WorldwideExpressPlus)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Indicates if UPS COD is available to the given country AND UPS Service Type
        /// </summary>
        public static bool IsCodAvailable(UpsServiceType service, string countryCode)
        {
            if (IsUpsMiOrSurePostService(service))
            {
                return false;
            }

            return IsCodAvailable(countryCode);
        }

        /// <summary>
        /// Indicates if UPS COD is available to the given country and service
        /// </summary>
        public static bool IsCodAvailable(ShipmentEntity shipment)
        {
            return IsCodAvailable((UpsServiceType) shipment.Ups.Service, shipment.AdjustedShipCountryCode());
        }

        /// <summary>
        /// Indicates if UPS COD is available to the given country
        /// </summary>
        public static bool IsCodAvailable(string countryCode)
        {
            return countryCode == "US" || countryCode == "PR";
        }

        /// <summary>
        /// Get the total weight of the package including the dimensional weight
        /// </summary>
        public static double GetPackageTotalWeight(UpsPackageEntity package)
        {
            double weight = package.Weight;

            if (package.DimsAddWeight)
            {
                weight += package.DimsWeight;
            }

            return weight;
        }

        /// <summary>
        /// Get the global instanced UPS access key
        /// </summary>
        [NDependIgnoreLongMethod]
        public static string FetchAndSaveUpsAccessKey(UpsAccountEntity upsAccount, string upsLicense)
        {
            // Create the client for connecting to the UPS server
            using (XmlTextWriter xmlWriter = UpsWebClient.CreateRequest(UpsOnLineToolType.AccessKey, null))
            {

                xmlWriter.WriteStartElement("AccessLicenseProfile");
                xmlWriter.WriteElementString("CountryCode", upsAccount.CountryCode);
                xmlWriter.WriteElementString("LanguageCode", "EN");
                xmlWriter.WriteElementString("AccessLicenseText", upsLicense);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteElementString("CompanyName", upsAccount.Company);
                xmlWriter.WriteElementString("CompanyURL", upsAccount.Website);
                xmlWriter.WriteElementString("ShipperNumber", upsAccount.AccountNumber);

                xmlWriter.WriteStartElement("Address");
                xmlWriter.WriteElementString("AddressLine1", upsAccount.Street1);
                xmlWriter.WriteElementString("AddressLine2", upsAccount.Street2);
                xmlWriter.WriteElementString("AddressLine3", upsAccount.Street3);
                xmlWriter.WriteElementString("City", upsAccount.City);
                xmlWriter.WriteElementString("StateProvinceCode", upsAccount.StateProvCode);
                xmlWriter.WriteElementString("PostalCode", upsAccount.PostalCode);
                xmlWriter.WriteElementString("CountryCode", upsAccount.CountryCode);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("PrimaryContact");
                xmlWriter.WriteElementString("Name", new PersonName(new PersonAdapter(upsAccount, "")).FullName);
                xmlWriter.WriteElementString("Title", "N\\A");
                xmlWriter.WriteElementString("EMailAddress", UpsUtility.GetCorrectedEmailAddress(upsAccount.Email));
                xmlWriter.WriteElementString("PhoneNumber", new PersonAdapter(upsAccount, "").Phone10Digits);
                xmlWriter.WriteEndElement();

                UpsWebClient.AppendToolList(xmlWriter);

                xmlWriter.WriteStartElement("ClientSoftwareProfile");
                xmlWriter.WriteElementString("SoftwareInstaller", "User");
                xmlWriter.WriteElementString("SoftwareProductName", "ShipWorks");
                xmlWriter.WriteElementString("SoftwareProvider", "Interapptive, Inc.");
                xmlWriter.WriteElementString("SoftwareVersionNumber", Assembly.GetExecutingAssembly().GetName().Version.ToString(2));
                xmlWriter.WriteEndElement();

                // Process the XML request
                XmlDocument upsResponse = UpsWebClient.ProcessRequest(xmlWriter);

                // Now we can get the Access License number
                string accessKey = (string) upsResponse.CreateNavigator().Evaluate("string(//AccessLicenseNumber)");

                ShippingSettingsEntity settings = ShippingSettings.Fetch();
                settings.UpsAccessKey = SecureText.Encrypt(accessKey, "UPS");

                ShippingSettings.Save(settings);

                return settings.UpsAccessKey;
            }
        }

        /// <summary>
        /// Gets a list of UpsServiceTypes that are SurePost
        /// </summary>
        public static IEnumerable<UpsServiceType> SurePostShipmentTypes
        {
            get
            {
                return surePostShipmentTypes ?? (surePostShipmentTypes = new ReadOnlyCollection<UpsServiceType>(new []
                {
                    UpsServiceType.UpsSurePost1LbOrGreater,
                    UpsServiceType.UpsSurePostBoundPrintedMatter,
                    UpsServiceType.UpsSurePostMedia,
                    UpsServiceType.UpsSurePostLessThan1Lb
                }));
            }
        }

        /// <summary>
        /// Checks to see if any SurePost shipments exist.
        /// </summary>
        /// <returns>True if there are SurePost shipments in the database; otherwise false.</returns>
        private static bool SurePostShipmentsExist()
        {
            int[] surePostServiceTypeValues = SurePostShipmentTypes.Cast<int>().ToArray();

            // Create the predicate for the query to determine which shipments are eligible
            RelationPredicateBucket bucket = new RelationPredicateBucket
            (
                // UPS Shipment service type  IN ([surePostServiceTypeValues])
                UpsShipmentFields.Service == surePostServiceTypeValues
            );

            using (UpsShipmentCollection upsShipmentCollection = new UpsShipmentCollection())
            {
                return SqlAdapter.Default.GetDbCount(upsShipmentCollection, bucket) > 0;
            }
        }

        /// <summary>
        /// Determines whether customer [can use sure post].
        /// </summary>
        public static bool CanUseSurePost()
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ILicenseService licenseService = lifetimeScope.Resolve<ILicenseService>();
                EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.UpsSurePost, null);

                return restrictionLevel == EditionRestrictionLevel.None;
            }
        }

        /// <summary>
        /// Helper method to detrmine if a service is a SurePost service
        /// </summary>
        public static bool IsUpsSurePostService(UpsServiceType upsServiceType)
        {
            return SurePostShipmentTypes.Contains(upsServiceType);
        }

        /// <summary>
        /// Helper method to detrmine if a service is an MI service
        /// </summary>
        public static bool IsUpsMiService(UpsServiceType upsServiceType)
        {
            return upsServiceType == UpsServiceType.UpsMailInnovationsExpedited ||
                   upsServiceType == UpsServiceType.UpsMailInnovationsFirstClass ||
                   upsServiceType == UpsServiceType.UpsMailInnovationsIntEconomy ||
                   upsServiceType == UpsServiceType.UpsMailInnovationsIntPriority ||
                   upsServiceType == UpsServiceType.UpsMailInnovationsPriority;
        }

        /// <summary>
        /// Helper method to detrmine if a service is MI or SurePost service
        /// </summary>
        public static bool IsUpsMiOrSurePostService(UpsServiceType upsServiceType)
        {
            return IsUpsMiService(upsServiceType) || IsUpsSurePostService(upsServiceType);
        }

        /// <summary>
        /// UPS only allows email addresses less than or equal to 50 characters.
        /// </summary>
        /// <returns>
        /// If the email address length is 50 or less, emailAddress is returned.
        /// Otherwise, string.Empty is returned.
        /// </returns>
        public static string GetCorrectedEmailAddress(string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                return string.Empty;
            }

            return emailAddress.Length <= 50 ? emailAddress : string.Empty;
        }

        /// <summary>
        /// Corrects the smart pickup error. Return null if address not changed.
        /// </summary>
        public static string CorrectSmartPickupError(string city)
        {
            List<KeyValuePair<string, string>> replacements = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("St ", "Saint "),
                new KeyValuePair<string, string>("St. ", "Saint "),
                new KeyValuePair<string, string>("Ste ", "Saint "),
                new KeyValuePair<string, string>("Ste. ", "Saint "),
                new KeyValuePair<string, string>("Saint G", "Ste. G"), // specifically St Genevieve.
                new KeyValuePair<string, string>("Saint ", "St "),
                new KeyValuePair<string, string>("Ft. ", "Fort "),
                new KeyValuePair<string, string>("Ft ", "Fort "),
                new KeyValuePair<string, string>("Fort ", "Ft "),
                new KeyValuePair<string, string>("MT ", "Mount "),
                new KeyValuePair<string, string>("Mount ", "MT ")
            };

            foreach (KeyValuePair<string, string> replacement in replacements)
            {
                if (city.StartsWith(replacement.Key, StringComparison.InvariantCultureIgnoreCase))
                {
                    return string.Format("{0}{1}", replacement.Value, city.Substring(replacement.Key.Length));
                }
            }

            return null;
        }
    }
}
