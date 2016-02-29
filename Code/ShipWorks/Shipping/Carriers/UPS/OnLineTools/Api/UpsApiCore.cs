using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using System.Xml;
using Autofac;
using Interapptive.Shared;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api.ElementWriters;
using ShipWorks.Shipping.Carriers.UPS.ServiceManager;
using ShipWorks.Templates.Tokens;
using Interapptive.Shared.Business;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Editions;
using ShipWorks.Shipping.Api;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api
{
    /// <summary>
    /// Useful UPS api functions
    /// </summary>
    public static class UpsApiCore
    {
        /// <summary>
        /// Packging codes for the UPS API
        /// </summary>
        static Dictionary<UpsPackagingType, string> upsPackagingCodes = new Dictionary<UpsPackagingType, string>();

        /// <summary>
        /// Constructor
        /// </summary>
        static UpsApiCore()
        {
            upsPackagingCodes[UpsPackagingType.Letter] = "01";
            upsPackagingCodes[UpsPackagingType.Custom] = "02";
            upsPackagingCodes[UpsPackagingType.Tube] = "03";
            upsPackagingCodes[UpsPackagingType.Pak] = "04";
            upsPackagingCodes[UpsPackagingType.BoxExpressSmall] = "2a";
            upsPackagingCodes[UpsPackagingType.BoxExpressMedium] = "2b";
            upsPackagingCodes[UpsPackagingType.BoxExpressLarge] = "2c";
            upsPackagingCodes[UpsPackagingType.Box25Kg] = "24";
            upsPackagingCodes[UpsPackagingType.Box10Kg] = "25";

            // Domestic MI package types
            upsPackagingCodes[UpsPackagingType.FirstClassMail] = "59";
            upsPackagingCodes[UpsPackagingType.PriorityMail] = "60";
            upsPackagingCodes[UpsPackagingType.BPMFlats] = "66";
            upsPackagingCodes[UpsPackagingType.BPMParcels] = "64";
            upsPackagingCodes[UpsPackagingType.Irregulars] = "62";
            upsPackagingCodes[UpsPackagingType.Machinables] = "61";
            upsPackagingCodes[UpsPackagingType.MediaMail] = "65";
            upsPackagingCodes[UpsPackagingType.ParcelPost] = "63";
            upsPackagingCodes[UpsPackagingType.StandardFlats] = "67";

            // International MI package types
            upsPackagingCodes[UpsPackagingType.Flats] = "56";
            upsPackagingCodes[UpsPackagingType.BPM] = "58";
            upsPackagingCodes[UpsPackagingType.Parcels] = "57";

            // Code for Canada - according to UPS support Express Envelope & Letter service codes 
            // are interchangeable
            upsPackagingCodes[UpsPackagingType.BoxExpress] = "21";
            upsPackagingCodes[UpsPackagingType.ExpressEnvelope] = "01";
        }

        /// <summary>
        /// Interapptive's UPS developer license number
        /// </summary>
        public static string DeveloperLicenseNumber
        {
            get { return "DB9B05CE131F5D24"; }
        }

        /// <summary>
        /// Get the UPS account associated with the given shipment.  Throws an exception if it does not exist.
        /// </summary>
        public static UpsAccountEntity GetUpsAccount(ShipmentEntity shipment, ICarrierAccountRepository<UpsAccountEntity> accountRepository)
        {
            UpsAccountEntity account = accountRepository.GetAccount(shipment.Ups.UpsAccountID);
            if (account == null)
            {
                throw new UpsException("No UPS account is selected for the shipment.");
            }

            var accountRestriction = EditionManager.ActiveRestrictions.CheckRestriction(EditionFeature.UpsAccountNumbers, UpsAccountManager.Accounts);
            if (accountRestriction.Level != EditionRestrictionLevel.None)
            {
                throw new UpsException(accountRestriction.GetDescription());
            }

            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                // The window handler is null because A: we are in a static method and B: the 
                // UpsAccountLimitFeatureRestriction doesn't use it at all.
                ILicenseService licenseService = scope.Resolve<ILicenseService>();
                licenseService.HandleRestriction(EditionFeature.UpsAccountLimit, UpsAccountManager.Accounts.Count, null);
            }

            return account;
        }

        /// <summary>
        /// Get the reate type code for the given rate type
        /// </summary>
        public static string GetPickupTypeCode(UpsRateType rateType)
        {
            switch (rateType)
            {
                case UpsRateType.Negotiated:
                case UpsRateType.DailyPickup:
                    return "01";

                case UpsRateType.Occasional:
                    return "03";

                case UpsRateType.Retail:
                default:
                    return "11";
            }
        }

        /// <summary>
        /// Get the customer classification code for the UPS API for the given rate type
        /// </summary>
        public static string GetCustomerClassificationCode(UpsRateType rateType)
        {
            string pickupCode = GetPickupTypeCode(rateType);

            if (pickupCode == "11")
            {
                return "04";
            }

            return pickupCode;
        }

        /// <summary>
        /// Write the address block for the given person adapter to the given writer
        /// </summary>
        public static void WriteAddressXml(XmlTextWriter xmlWriter, PersonAdapter person)
        {
            WriteAddressXml(xmlWriter, person, null);
        }

        /// <summary>
        /// Write the address block for the given person adapter.  If residentialFlag is true, then that flag will be written as well.
        /// </summary>
        public static void WriteAddressXml(XmlTextWriter xmlWriter, PersonAdapter person, string residentialFlag)
        {
            xmlWriter.WriteStartElement("Address");
            xmlWriter.WriteElementString("AddressLine1", person.Street1);
            xmlWriter.WriteElementString("AddressLine2", person.Street2);
            xmlWriter.WriteElementString("AddressLine3", person.Street3);
            xmlWriter.WriteElementString("City", person.City);
            xmlWriter.WriteElementString("StateProvinceCode", AdjustUpsStateProvinceCode(person.CountryCode, person.StateProvCode));
            xmlWriter.WriteElementString("PostalCode", person.PostalCode);
            xmlWriter.WriteElementString("CountryCode", person.AdjustedCountryCode(ShipmentTypeCode.UpsOnLineTools));

            if (!string.IsNullOrWhiteSpace(residentialFlag))
            {
                xmlWriter.WriteElementString(residentialFlag, null);
            }

            xmlWriter.WriteEndElement();
        }

        /// <summary>
        /// Translate the state if necessary
        /// </summary>
        /// <param name="countryCode">Country associated with the address</param>
        /// <param name="stateProvCode">State or province code associated with the address</param>
        /// <returns></returns>
        public static string AdjustUpsStateProvinceCode(string countryCode, string stateProvCode)
        {
            // If Puerto Rico is the country, we'll just use it as the state as well
            return countryCode.Equals("PR", StringComparison.OrdinalIgnoreCase) ? "PR" : stateProvCode;
        }

        /// <summary>
        /// Get the UPS API code to use for the giving packaging type
        /// </summary>
        private static string GetPackagingCode(UpsPackagingType packagingType)
        {
            return upsPackagingCodes[packagingType];
        }

        /// <summary>
        /// Generate the packages XML for either rating or label generation.
        /// </summary>
        [NDependIgnoreLongMethod]
        public static void WritePackagesXml(UpsShipmentEntity ups, XmlTextWriter xmlWriter, bool forLabels, 
            UpsPackageWeightElementWriter weightElementWriter, UpsPackageServiceOptionsElementWriter serviceOptionsElementWriter)
        {
            bool isSurePost = UpsUtility.IsUpsSurePostService((UpsServiceType)ups.Service);
            
            // All packages in the shipment
            foreach (UpsPackageEntity package in ups.Packages.ToList())
            {
                xmlWriter.WriteStartElement("Package");

                // return shipments require a package description - returns not applicable for SurePost
                if (ups.Shipment.ReturnShipment && !isSurePost)
                {
                    // contents are tokenizable
                    string contents = TemplateTokenProcessor.ProcessTokens(ups.ReturnContents, ups.ShipmentID);
                    if (contents.Length > 35)
                    {
                        contents = contents.Substring(0, 35);
                    }

                    xmlWriter.WriteElementString("Description", contents);
                }

                string packagingCode = GetPackagingCode((UpsPackagingType) package.PackagingType);

                // Type
                xmlWriter.WriteStartElement("PackagingType");
                xmlWriter.WriteElementString("Code", packagingCode);
                xmlWriter.WriteEndElement();

                // Dimensions
                if (packagingCode == "02")
                {
                    xmlWriter.WriteStartElement("Dimensions");

                    xmlWriter.WriteStartElement("UnitOfMeasurement");
                    xmlWriter.WriteElementString("Code", "IN");
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteElementString("Length", package.DimsLength.ToString("##0.00"));
                    xmlWriter.WriteElementString("Width", package.DimsWidth.ToString("##0.00"));
                    xmlWriter.WriteElementString("Height", package.DimsHeight.ToString("##0.00"));
                    xmlWriter.WriteEndElement();
                }

                // Package Weight
                weightElementWriter.WriteWeightElement(ups, package);

                // Mail Innovations doesn't support ReferenceNumbers
                if (!UpsUtility.IsUpsMiService((UpsServiceType) ups.Service))
                {
                    WritePackageReference(ups.ReferenceNumber, ups, xmlWriter, forLabels);
                    WritePackageReference(ups.ReferenceNumber2, ups, xmlWriter, forLabels);
                }

                // Additional handling
                if (package.AdditionalHandlingEnabled)
                {
                    xmlWriter.WriteElementString("AdditionalHandling", null);
                }

                // Service options
                UpsServicePackageTypeSetting servicePackageSettings = UpsServicePackageTypeSetting.ServicePackageValidationSettings.FirstOrDefault(s => s.ServiceType == (UpsServiceType)ups.Service && s.PackageType == (UpsPackagingType)package.PackagingType);
                serviceOptionsElementWriter.WriteServiceOptionsElement(ups, package, servicePackageSettings);
                
                // End package
                xmlWriter.WriteEndElement();
            }
        }

        /// <summary>
        /// Writes the reference for Package
        /// </summary>
        /// <param name="referenceNumber">The reference number.</param>
        /// <param name="ups">The ups.</param>
        /// <param name="xmlWriter">The XML writer.</param>
        /// <param name="forLabels">if set to <c>true</c> [for labels].</param>
        private static void WritePackageReference(string referenceNumber, UpsShipmentEntity ups, XmlTextWriter xmlWriter, bool forLabels)
        {
            // Package ref # only included if we are doing the actual ShipAPI, and not just rates and origin/destination is US/US or PR/PR
            if (forLabels && IsDomesticUnitedStatesOrPuertoRico(ups.Shipment))
            {
                WriteReference(referenceNumber, ups, xmlWriter, "01");
            }
        }

        /// <summary>
        /// Writes the reference for Shipment
        /// </summary>
        /// <param name="referenceNumber">The reference number.</param>
        /// <param name="ups">The ups.</param>
        /// <param name="xmlWriter">The XML writer.</param>
        /// <param name="forLabels">if set to <c>true</c> [for labels].</param>
        public static void WriteShipmentReference(string referenceNumber, UpsShipmentEntity ups, XmlTextWriter xmlWriter, bool forLabels)
        {
            // Shipment ref # only included if we are doing the actual ShipAPI, and not just rates and origin/destination is NOT US/US or PR/PR
            if (forLabels && !IsDomesticUnitedStatesOrPuertoRico(ups.Shipment))
            {
                WriteReference(referenceNumber, ups, xmlWriter, "XX");
            }
        }

        /// <summary>
        /// Writes the reference no matter what. This should be called by WritePackageReference or WriteShipmentReference
        /// </summary>
        /// <param name="referenceNumber">The reference number.</param>
        /// <param name="ups">The ups shipment entity.</param>
        /// <param name="xmlWriter">The XML writer.</param>
        /// <param name="code">The code.</param>
        private static void WriteReference(string referenceNumber, UpsShipmentEntity ups, XmlTextWriter xmlWriter, string code)
        {
            string reference = ProcessReferenceNumber(referenceNumber, ups.ShipmentID);

            // Shipment Reference #
            if (!string.IsNullOrEmpty(reference))
            {
                xmlWriter.WriteStartElement("ReferenceNumber");
                xmlWriter.WriteElementString("Code", code);
                xmlWriter.WriteElementString("Value", reference);
                xmlWriter.WriteEndElement();
            }
        }

        /// <summary>
        /// Process for tokens and trim to allowable length
        /// </summary>
        public static string ProcessReferenceNumber(string reference, long shipmentID)
        {
            string referenceValue = TemplateTokenProcessor.ProcessTokens(reference, shipmentID);

            if (!string.IsNullOrEmpty(referenceValue))
            {
                referenceValue = referenceValue.Trim();

                if (referenceValue.Length > 35)
                {
                    referenceValue = referenceValue.Substring(0, 35);
                }
            }

            return referenceValue;
        }

        /// <summary>
        /// Process for tokens and trim to allowable length
        /// </summary>
        public static string ProcessUspsTokenField(string tokenFieldValue, long shipmentID, string defaultValue)
        {
            // Set the max length to 35 for MI shipments
            int maxLength = 30;

            // Process the reference as a token first
            tokenFieldValue = TemplateTokenProcessor.ProcessTokens(tokenFieldValue.Trim(), shipmentID);

            tokenFieldValue = Regex.Replace(tokenFieldValue, "[^a-zA-Z0-9]", string.Empty);

            // Fix the defaultValue in case we need to use it below.
            defaultValue = Regex.Replace(defaultValue, "[^a-zA-Z0-9]", string.Empty);

            // Also, ref 1/2 cannot be blank
            if (string.IsNullOrWhiteSpace(tokenFieldValue))
            {
                tokenFieldValue = defaultValue;
            }

            // Also, the field must be less than max length
            tokenFieldValue = tokenFieldValue.Substring(0, tokenFieldValue.Length >= maxLength ? maxLength : tokenFieldValue.Length);

            return tokenFieldValue;
        }

        /// <summary>
        /// Get the code to use for the given delivery confirmation type
        /// </summary>
        public static string GetShipmentLevelDeliveryConfirmationCode(UpsDeliveryConfirmationType confirmationType)
        {
            switch (confirmationType)
            {
                case UpsDeliveryConfirmationType.NoSignature: return "";
                case UpsDeliveryConfirmationType.Signature: return "1";
                case UpsDeliveryConfirmationType.AdultSignature: return "2";
                case UpsDeliveryConfirmationType.UspsDeliveryConfirmation: return "";
            }

            throw new InvalidOperationException("Invalid UPS DC Type: " + confirmationType);
        }

        /// <summary>
        /// Get the code to use for the given delivery confirmation type
        /// </summary>
        public static string GetPackageLevelDeliveryConfirmationCode(UpsDeliveryConfirmationType confirmationType)
        {
            switch (confirmationType)
            {
                case UpsDeliveryConfirmationType.NoSignature: return "1";
                case UpsDeliveryConfirmationType.Signature: return "2";
                case UpsDeliveryConfirmationType.AdultSignature: return "3";
                case UpsDeliveryConfirmationType.UspsDeliveryConfirmation: return "4";
            }

            throw new InvalidOperationException("Invalid UPS DC Type: " + confirmationType);
        }

        /// <summary>
        /// Get the code to use for the given Return Service Type
        /// </summary>
        public static string GetReturnServiceCode(UpsReturnServiceType returnServiceType)
        {
            switch (returnServiceType)
            {
                case UpsReturnServiceType.PrintAndMail: return "2";
                case UpsReturnServiceType.ReturnPlus1: return "3";
                case UpsReturnServiceType.ReturnPlus3: return "5";
                case UpsReturnServiceType.ElectronicReturnLabel: return "8";
                case UpsReturnServiceType.PrintReturnLabel: return "9";
            }

            throw new InvalidOperationException("Invalid UPS Return Service Type: " + returnServiceType);
        }

        /// <summary>
        /// Get the code to use for the given Endorsement Type
        /// </summary>
        public static string GetUspsEndorsementTypeCode(UspsEndorsementType endorsementType)
        {
            return EnumHelper.GetApiValue(endorsementType);
        }

        /// <summary>
        /// Gets whether a shipment is a domestic US to US or PR to PR
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        public static bool IsDomesticUnitedStatesOrPuertoRico(ShipmentEntity shipment)
        {
            return !ShipmentType.IsShipmentBetweenUnitedStatesAndPuertoRico(shipment) &&
                   ((shipment.AdjustedShipCountryCode() == "US" && shipment.AdjustedOriginCountryCode() == "US") ||
                    (ShipmentType.IsPuertoRicoAddress(shipment, "Ship") && ShipmentType.IsPuertoRicoAddress(shipment, "Origin")));
        }
    }
}
