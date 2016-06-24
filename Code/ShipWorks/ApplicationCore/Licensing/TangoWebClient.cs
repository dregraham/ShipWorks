using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Licensing.Activation.WebServices;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.ApplicationCore.Nudges;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Account;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Linq;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for working with the interapptive license server
    /// </summary>
    [NDependIgnoreLongTypesAttribute]
    public static class TangoWebClient
    {
        private const string ActivationUrl = "https://interapptive.com/ShipWorksNet/ActivationV1.svc";

        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(TangoWebClient));

        private static InsureShipAffiliateProvider insureShipAffiliateProvider = new InsureShipAffiliateProvider();

        private static Version version;

        /// <summary>
        /// Gets the version - If version is under 5.0.0.0, return 5.0.0.0
        /// </summary>
        private static string Version
        {
            get
            {
                if (version == null)
                {
                    // Tango requires a specific version in order to know when to return
                    // legacy responses or new response for the customer license. This is
                    // primarily for debug/internal versions of ShipWorks that have 0.0.0.x
                    // version number.
                    Version assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
                    Version minimumVersion = new Version(5, 0, 0, 0);

                    version = assemblyVersion.Major == 0 ? minimumVersion : assemblyVersion;
                }

                return version.ToString(4);
            }
        }

        /// <summary>
        /// Activate the given license key to the specified store identifier
        /// </summary>
        public static LicenseAccountDetail ActivateLicense(string licenseKey, StoreEntity store)
        {
            ShipWorksLicense license = new ShipWorksLicense(licenseKey);

            if (!license.IsValid)
            {
                throw new ShipWorksLicenseException("The license key is not valid.");
            }

            if (!license.IsMetered)
            {
                throw new ShipWorksLicenseException("The license is not valid for this version of ShipWorks.");
            }

            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();
            postRequest.Variables.Add("action", "activate");

            return ProcessAccountRequest(postRequest, store, license);
        }

        /// <summary>
        /// Associates a Usps account created in ShipWorks as the users free Stamps.com account
        /// </summary>
        internal static AssociateShipWorksWithItselfResponse AssociateShipworksWithItself(AssociateShipworksWithItselfRequest request)
        {
            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();

            postRequest.Variables.Add("action", "associateshipworkswithitself");
            postRequest.Variables.Add("customerlicense", request.CustomerKey);

            postRequest.Variables.Add("cc_holder", request.CardHolderName);
            postRequest.Variables.Add("cc_cardType", ((int) request.CardType).ToString());
            postRequest.Variables.Add("cc_account_number", request.CardNumber);
            postRequest.Variables.Add("cc_cvn", request.CardCvn);
            postRequest.Variables.Add("cc_expiration_month", request.CardExpirationMonth.ToString());
            postRequest.Variables.Add("cc_expiration_year", request.CardExpirationYear.ToString());
            postRequest.Variables.Add("cc_billing_street_1", request.CardBillingAddress.Street1);
            postRequest.Variables.Add("cc_billing_city", request.CardBillingAddress.City);
            postRequest.Variables.Add("cc_billing_state", request.CardBillingAddress.StateProvCode);
            postRequest.Variables.Add("cc_billing_zipcode", request.CardBillingAddress.PostalCode.Truncate(5));
            postRequest.Variables.Add("sendMarketingInfo", "false");
            postRequest.Variables.Add("version", Version);

            if (request.MatchedPhysicalAddress != null)
            {
                Address matchedAddress = request.MatchedPhysicalAddress;
                postRequest.Variables.Add("pStreet", matchedAddress.Address1);
                postRequest.Variables.Add("pCity", matchedAddress.City);
                postRequest.Variables.Add("pState", matchedAddress.State);
                postRequest.Variables.Add("pZipcode", matchedAddress.ZIPCode);
                postRequest.Variables.Add("pAddOn", matchedAddress.ZIPCodeAddOn);
                postRequest.Variables.Add("pDPCode", matchedAddress.DPB);
                postRequest.Variables.Add("pChkDigit", matchedAddress.CheckDigit);
                postRequest.Variables.Add("pZipStandard", "zip_standardized");
            }
            try
            {
                XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "associateshipworkswithitself");
                return new AssociateShipWorksWithItselfResponse(xmlResponse);
            }
            catch (TangoException ex)
            {
                return new AssociateShipWorksWithItselfResponse(AssociateShipWorksWithItselfResponseType.UnknownError, ex.Message);
            }
        }

        /// <summary>
        /// Get the status of the specified license
        /// </summary>
        public static ILicenseAccountDetail GetLicenseStatus(string licenseKey, StoreEntity store)
        {
            ShipWorksLicense license = new ShipWorksLicense(licenseKey);

            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();
            postRequest.Variables.Add("action", "getstatus");

            LicenseAccountDetail licenseAccountDetail = ProcessAccountRequest(postRequest, store, license);

            InsureShipAffiliate insureShipAffiliate = new InsureShipAffiliate(licenseAccountDetail.TangoStoreID, licenseAccountDetail.TangoCustomerID);
            insureShipAffiliateProvider.Add(store.StoreID, insureShipAffiliate);

            return licenseAccountDetail;
        }

        /// <summary>
        /// Gets the Tango customer id for a license.
        /// </summary>
        public static string GetTangoCustomerId()
        {
            StoreEntity store = StoreManager.GetEnabledStores().FirstOrDefault() ??
                                StoreManager.GetAllStores().FirstOrDefault();

            return store != null ? GetLicenseStatus(store.License, store).TangoCustomerID : string.Empty;
        }

        /// <summary>
        /// Returns an InsureShipAffiliate for the specified store.
        /// If one cannot be found, an InsureShipException is thrown.
        /// </summary>
        public static InsureShipAffiliate GetInsureShipAffiliate(StoreEntity store)
        {
            InsureShipAffiliate insureShipAffiliate = insureShipAffiliateProvider.GetInsureShipAffiliate(store.StoreID);

            // If it's null, try one more time to populate it.
            if (insureShipAffiliate == null)
            {
                GetLicenseStatus(store.License, store);
                insureShipAffiliate = insureShipAffiliateProvider.GetInsureShipAffiliate(store.StoreID);

                // If it's still null, throw
                if (insureShipAffiliate == null)
                {
                    throw new InsureShipException(string.Format("ShipWorks was unable to determine the Insurance Affiliate for store '{0}'", store.StoreName));
                }
            }

            return insureShipAffiliate;
        }

        /// <summary>
        /// Gets the nudges for the specified store.
        /// </summary>
        public static List<Nudge> GetNudges(StoreEntity store)
        {
            List<Nudge> nudges = new List<Nudge>();

            ShipWorksLicense license = new ShipWorksLicense(store.License);

            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();
            postRequest.Variables.Add("action", "getnudges");
            postRequest.Variables.Add("license", license.Key);

            XmlDocument nudgesDoc = ProcessXmlRequest(postRequest, "GetNudges");
            XElement xNudges = XElement.Parse(nudgesDoc.OuterXml);

            foreach (XElement xNudge in xNudges.Elements("Nudge"))
            {
                try
                {
                    Nudge nudge = NudgeDeserializer.Deserialize(xNudge);
                    nudges.Add(nudge);
                }
                catch (NudgeException ex)
                {
                    log.ErrorFormat("Unable to deserialize a nudge from Tango.  Exception message: {0}", ex.Message);
                }
            }

            return nudges;
        }

        /// <summary>
        /// Logs the nudge option back to Tango to indicate that the option was selected
        /// by the user.
        /// </summary>
        public static void LogNudgeOption(NudgeOption option)
        {
            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();

            postRequest.Variables.Add("action", "lognudgeresponse");
            postRequest.Variables.Add("nudgeid", option.Owner.NudgeID.ToString(CultureInfo.InvariantCulture));
            postRequest.Variables.Add("nudgeoptionid", option.NudgeOptionID.ToString(CultureInfo.InvariantCulture));
            postRequest.Variables.Add("result", option.Result);

            ProcessRequest(postRequest, "LogNudgeOption");
        }

        /// <summary>
        /// Get the status of the specified license
        /// </summary>
        public static Dictionary<string, string> GetCounterRatesCredentials(StoreEntity store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            Dictionary<string, string> results = new Dictionary<string, string>();
            string action = "getcounterratecredentials";

            // Get the license from the store so we know how to log
            ShipWorksLicense license = new ShipWorksLicense(store.License);

            // Get the store type
            StoreType storeType = StoreTypeManager.GetType(store);

            // Create our http variable request submitter
            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();

            // Both methods use action
            postRequest.Variables.Add("action", action);

            // Trial shipment logging
            if (license.IsTrial)
            {
                postRequest.Variables.Add("storecode", storeType.TangoCode);
                postRequest.Variables.Add("identifier", storeType.LicenseIdentifier);
            }
            else
            {
                postRequest.Variables.Add("license", license.Key);
            }

            // Get the credentials from Tango
            XmlDocument responseXmlDocument = ProcessXmlRequest(postRequest, "GetCounterRatesCreds");

            // Pull the credentials from the response; none of the fields are encrypted in the response
            // so we can easily/quickly update them in Tango if they ever need to change
            // Use the correct version that ShipWorks should work with.
            string fedExBasePath = string.Format("/CounterRateCredentials/FedEx_v{0}/", FedExShippingClerk.ShipWebServiceVersion);

            // FedEx fields - password needs to encrypted
            AddCounterRateDictionaryEntry(responseXmlDocument, "FedExAccountNumber", fedExBasePath + "AccountNumber", results);
            AddCounterRateDictionaryEntry(responseXmlDocument, "FedExMeterNumber", fedExBasePath + "MeterNumber", results);
            AddCounterRateDictionaryEntry(responseXmlDocument, "FedExUsername", fedExBasePath + "Username", results);
            AddEncryptedCounterRateDictionaryEntry(responseXmlDocument, "FedExPassword", fedExBasePath + "Password", results, "FedEx");

            // UPS fields - access key needs to be encrypted
            AddCounterRateDictionaryEntry(responseXmlDocument, "UpsUserId", "/CounterRateCredentials/UPS/UserID", results);
            AddCounterRateDictionaryEntry(responseXmlDocument, "UpsPassword", "/CounterRateCredentials/UPS/Password", results);
            AddEncryptedCounterRateDictionaryEntry(responseXmlDocument, "UpsAccessKey", "/CounterRateCredentials/UPS/AccessKey", results, "UPS");

            // Express1 for Endicia fields - passphrase needs to be encrypted
            AddCounterRateDictionaryEntry(responseXmlDocument, "Express1EndiciaAccountNumber", "/CounterRateCredentials/Express1[@provider='Endicia']/AccountNumber", results);
            AddEncryptedCounterRateDictionaryEntry(responseXmlDocument, "Express1EndiciaPassPhrase", "/CounterRateCredentials/Express1[@provider='Endicia']/Password", results, "Endicia");

            // Express1 for USPS fields - password needs to be encrypted
            AddCounterRateDictionaryEntry(responseXmlDocument, "Express1StampsUsername", "/CounterRateCredentials/Express1[@provider='Stamps']/AccountNumber", results);
            AddEncryptedCounterRateDictionaryEntry(responseXmlDocument, "Express1StampsPassword", "/CounterRateCredentials/Express1[@provider='Stamps']/Password", results, results["Express1StampsUsername"]);

            // USPS fields - password needs to be encrypted
            AddCounterRateDictionaryEntry(responseXmlDocument, "StampsUsername", "/CounterRateCredentials/Stamps/Username", results);
            AddEncryptedCounterRateDictionaryEntry(responseXmlDocument, "StampsPassword", "/CounterRateCredentials/Stamps/Password", results, results["StampsUsername"]);

            return results;
        }

        /// <summary>
        /// Get the status of the specified license
        /// </summary>
        public static Dictionary<string, string> GetCarrierCertificateVerificationData(StoreEntity store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            Dictionary<string, string> results = new Dictionary<string, string>();
            string action = "getcertificateverificationdata";

            // Get the license from the store so we know how to log
            ShipWorksLicense license = new ShipWorksLicense(store.License);

            // Get the store type
            StoreType storeType = StoreTypeManager.GetType(store);

            // Create our http variable request submitter
            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();

            // Both methods use action
            postRequest.Variables.Add("action", action);

            // Trial shipment logging
            if (license.IsTrial)
            {
                postRequest.Variables.Add("storecode", storeType.TangoCode);
                postRequest.Variables.Add("identifier", storeType.LicenseIdentifier);
            }
            else
            {
                postRequest.Variables.Add("license", license.Key);
            }

            // Get the certificate verification data from Tango
            XmlDocument responseXmlDocument = ProcessXmlRequest(postRequest, "CarrierCertificate");

            // Pull certificate verification data from the response; none of the fields are encrypted in the response
            // so we can easily/quickly update them in Tango if they ever need to change
            AddCarrierCertificateVerificationDataDictionaryEntries(responseXmlDocument, "FedEx", TangoCredentialStore.FedExCertificateVerificationDataKeyName, results);
            AddCarrierCertificateVerificationDataDictionaryEntries(responseXmlDocument, "UPS", TangoCredentialStore.UpsCertificateVerificationDataKeyName, results);
            AddCarrierCertificateVerificationDataDictionaryEntries(responseXmlDocument, "InsureShip", TangoCredentialStore.InsureShipCertificateVerificationDataKeyName, results);
            AddCarrierCertificateVerificationDataDictionaryEntries(responseXmlDocument, "Stamps", TangoCredentialStore.UspsCertificateVerificationDataKeyName, results);

            return results;
        }

        /// <summary>
        /// Helper method to pull counter rate credentials from the tango response.
        /// </summary>
        private static void AddCarrierCertificateVerificationDataDictionaryEntries(XmlDocument responseXmlDocument, string serviceName, string keyName, Dictionary<string, string> dictionary)
        {
            string subjectXPath = string.Format("/CertificateVerificationData/Service[@name='{0}'][@enabled='true']", serviceName);
            XmlNode subjectNode = responseXmlDocument.SelectSingleNode(subjectXPath);

            if (subjectNode != null)
            {
                dictionary.Add(keyName, subjectNode.OuterXml);
            }
            else
            {
                dictionary.Add(keyName, string.Empty);
            }
        }

        /// <summary>
        /// Helper method to pull counter rate credentials from the tango response.
        /// </summary>
        private static void AddCounterRateDictionaryEntry(XmlDocument responseXmlDocument, string keyName, string xPathToNode, Dictionary<string, string> dictionary)
        {
            XmlNode node = responseXmlDocument.SelectSingleNode(xPathToNode);
            if (node != null)
            {
                dictionary.Add(keyName, node.InnerText.Trim());
            }
        }

        /// <summary>
        /// Helper method to pull counter rate credentials from the tango response and encrypt the value. This is necessary
        /// for a couple of the password and access key values since the API web clients are expecting these values to
        /// be encrypted.
        /// </summary>
        private static void AddEncryptedCounterRateDictionaryEntry(XmlDocument responseXmlDocument, string keyName, string xPathToNode, Dictionary<string, string> dictionary, string salt)
        {
            XmlNode node = responseXmlDocument.SelectSingleNode(xPathToNode);
            if (node != null)
            {
                string encryptedValue = SecureText.Encrypt(node.InnerText.Trim(), salt);
                dictionary.Add(keyName, encryptedValue);
            }
        }

        /// <summary>
        /// Request a trial for use with the specified store. If a trial already exists, a new one will not be created.
        /// </summary>
        public static TrialDetail GetTrial(StoreEntity store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();
            postRequest.Variables.Add("action", "gettrial");
            postRequest.Variables.Add("license", store.License);
            postRequest.Variables.Add("edition", EnumHelper.GetDescription(TrialDetail.EffectiveTrialEditionType));

            // Process the request
            TrialDetail trialDetail = ProcessTrialRequest(postRequest, store);

            // This will happen when the user changes the identifier of the store, and the identifier they
            // change to is one that is already used by an existing trial.  We update the store to use
            // the license from the trial of the identifier they changed to.
            if (trialDetail.License.Key != store.License)
            {
                bool alreadyDirty = store.IsDirty || store.IsNew;

                store.License = trialDetail.License.Key;

                // Dont save if its already dirty - whoever already made it dirty will save it.
                if (!alreadyDirty)
                {
                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        adapter.SaveAndRefetch(store);
                    }
                }
            }

            return trialDetail;
        }

        /// <summary>
        /// Extend the trial for the given store
        /// </summary>
        public static TrialDetail ExtendTrial(StoreEntity store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();
            postRequest.Variables.Add("action", "extendtrial");

            return ProcessTrialRequest(postRequest, store);
        }

        /// <summary>
        /// Send the new password to the given user
        /// </summary>
        public static void SendAccountPassword(string email, string password)
        {
            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();
            postRequest.Variables.Add("action", "email");
            postRequest.Variables.Add("emailtype", "password");
            postRequest.Variables.Add("emailaddress", email);
            postRequest.Variables.Add("password", password);

            ProcessXmlRequest(postRequest, "SendAccountPassword");
        }

        /// <summary>
        /// Send the user their username using the specified email address
        /// </summary>
        public static void SendAccountUsername(string email, string username)
        {
            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();
            postRequest.Variables.Add("action", "email");
            postRequest.Variables.Add("emailtype", "username");
            postRequest.Variables.Add("emailaddress", email);
            postRequest.Variables.Add("username", username);

            ProcessXmlRequest(postRequest, "SendAccountUsername");
        }

        /// <summary>
        /// Log the given insurance claim to Tango.
        /// </summary>
        public static void LogSubmitInsuranceClaim(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            // The shipment wasn't insured or it's carrier declared value, just return.
            if (!shipment.Insurance || shipment.InsuranceProvider != (int) InsuranceProvider.ShipWorks)
            {
                return;
            }

            // Load the insurance policy if it's null.
            if (shipment.InsurancePolicy == null)
            {
                ShipmentTypeDataService.LoadInsuranceData(shipment);
            }

            // If there is no insurance policy, just return.
            if (shipment.InsurancePolicy == null)
            {
                throw new InsureShipException(string.Format("No insurance policy was found for ShipmentID: {0}.", shipment.ShipmentID));
            }

            InsurancePolicyEntity insurancePolicy = shipment.InsurancePolicy;
            if (!insurancePolicy.ClaimID.HasValue || !insurancePolicy.ClaimType.HasValue || !insurancePolicy.DamageValue.HasValue || !insurancePolicy.SubmissionDate.HasValue)
            {
                throw new InsureShipException(string.Format("Missing insurance claim values for ShipmentID: {0}.", shipment.ShipmentID));
            }

            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();
            postRequest.Variables.Add("action", "submitinsuranceclaim");
            postRequest.Variables.Add("insureshipstorename", insurancePolicy.InsureShipStoreName);
            postRequest.Variables.Add("createdwithapi", insurancePolicy.CreatedWithApi.ToString());
            postRequest.Variables.Add("itemname", insurancePolicy.ItemName);
            postRequest.Variables.Add("claimtype", insurancePolicy.ClaimType.Value.ToString(CultureInfo.InvariantCulture));
            postRequest.Variables.Add("damagevalue", insurancePolicy.DamageValue.Value.ToString(CultureInfo.InvariantCulture));
            postRequest.Variables.Add("submissiondate", insurancePolicy.SubmissionDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            postRequest.Variables.Add("claimid", insurancePolicy.ClaimID.Value.ToString(CultureInfo.InvariantCulture));

            ProcessXmlRequest(postRequest, "SubmitInsuranceClaim");
        }

        /// <summary>
        /// Sends Postal balances for postal services.
        /// </summary>
        public static void LogPostageEvent(ILicenseAccountDetail license, decimal balance, decimal purchaseAmount, ShipmentTypeCode shipmentTypeCode, string accountIdentifier)
        {
            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();

            postRequest.Variables.Add("action", "logpostagebalance");
            postRequest.Variables.Add("license", license.Key);
            postRequest.Variables.Add("balance", balance.ToString(CultureInfo.InvariantCulture));
            postRequest.Variables.Add("purchaseamount", purchaseAmount.ToString(CultureInfo.InvariantCulture));
            postRequest.Variables.Add("swtype", ((int) shipmentTypeCode).ToString(CultureInfo.InvariantCulture));
            postRequest.Variables.Add("accountidentifier", accountIdentifier);

            XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "CarrierBalance");

            // Check for error
            XmlNode errorNode = xmlResponse.SelectSingleNode("//Error");
            if (errorNode != null)
            {
                throw new TangoException(errorNode.InnerText);
            }
        }

        /// <summary>
        /// Sends USPS contract type to Tango.
        /// </summary>
        public static void LogStampsAccount(ILicenseAccountDetail license, ShipmentTypeCode shipmentTypeCode, string accountIdentifier, UspsAccountContractType uspsAccountContractType)
        {
            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();

            postRequest.Variables.Add("action", "logstampsaccount");
            postRequest.Variables.Add("license", license.Key);
            postRequest.Variables.Add("accountidentifier", accountIdentifier);
            postRequest.Variables.Add("swtype", ((int) shipmentTypeCode).ToString(CultureInfo.InvariantCulture));
            postRequest.Variables.Add("stampscontracttype", ((int) uspsAccountContractType).ToString(CultureInfo.InvariantCulture));

            XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "LogStampsAccount");

            // Check for error
            XmlNode errorNode = xmlResponse.SelectSingleNode("//Error");
            if (errorNode != null)
            {
                throw new TangoException(errorNode.InnerText);
            }
        }

        /// <summary>
        /// Log the given processed shipment to Tango.  isRetry is only for internal interapptive purposes to handle rare cases where shipments a customer
        /// insured did not make it up into tango, but the shipment did actually process.
        /// </summary>
        /// <returns>OnlineShipmentID from Tango.</returns>
        /// <exception cref="System.ArgumentNullException">store</exception>
        /// <exception cref="TangoException"></exception>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethod]
        public static string LogShipment(StoreEntity store, ShipmentEntity shipment, bool isRetry = false)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            string onlineShipmentID = string.Empty;

            // Get the license from the store so we know how to log
            ShipWorksLicense license = new ShipWorksLicense(store.License);

            // Create the request
            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();

            // Get the shipment and store types
            ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);
            StoreType storeType = StoreTypeManager.GetType(store);

            // Both methods use the license key
            postRequest.Variables.Add("license", license.Key);

            // Trial shipment logging
            if (license.IsTrial)
            {
                postRequest.Variables.Add("action", "logtrialshipments");
                postRequest.Variables.Add("shipments", "1");
                postRequest.Variables.Add("service", shipmentType.ShipmentTypeName);
                postRequest.Variables.Add("storecode", storeType.TangoCode);
                postRequest.Variables.Add("identifier", storeType.LicenseIdentifier);

                ProcessXmlRequest(postRequest, "LogTrialShipments");
            }
            // Regular shipment logging
            else
            {
                string tracking = shipment.TrackingNumber;

                // For the purposes of U-PIC logging, CustomsNumber cannot be counted as a true TrackingNumber
                if (PostalUtility.IsPostalShipmentType(shipmentType.ShipmentTypeCode) && !shipment.ShipPerson.IsDomesticCountry())
                {
                    tracking = "";
                }

                List<IInsuranceChoice> insuredPackages =
                    Enumerable.Range(0, shipmentType.GetParcelCount(shipment))
                    .Select(parcelIndex => shipmentType.GetParcelDetail(shipment, parcelIndex).Insurance)
                    .Where(choice => choice.Insured && choice.InsuranceProvider == InsuranceProvider.ShipWorks && choice.InsuranceValue > 0)
                    .ToList();

                bool shipWorksInsured = false;
                bool carrierInsured = false;
                bool pennyOne = false;
                decimal insuredValue = 0;

                if (insuredPackages.Count > 0)
                {
                    IInsuranceChoice insuranceChoice = insuredPackages[0];

                    shipWorksInsured = true;
                    pennyOne = insuranceChoice.InsurancePennyOne ?? false;
                    insuredValue = insuranceChoice.InsuranceValue;
                }
                else
                {
                    carrierInsured = Enumerable.Range(0, shipmentType.GetParcelCount(shipment))
                        .Select(parcelIndex => shipmentType.GetParcelDetail(shipment, parcelIndex).Insurance)
                        .Any(
                            choice =>
                                choice.Insured && choice.InsuranceProvider == InsuranceProvider.Carrier &&
                                choice.InsuranceValue > 0);
                }

                if (isRetry)
                {
                    postRequest.Variables.Add("isretry", "1");
                }

                postRequest.Variables.Add("action", "logshipmentdetails");
                postRequest.Variables.Add("swshipmentid", shipment.ShipmentID.ToString());
                postRequest.Variables.Add("swordernumber", shipment.Order.OrderNumberComplete);
                postRequest.Variables.Add("shipdate", shipment.ShipDate.ToString("yyyy-MM-dd HH:mm:ss"));
                postRequest.Variables.Add("declaredvalue", insuredValue.ToString());
                postRequest.Variables.Add("swtype", shipment.ShipmentType.ToString());
                postRequest.Variables.Add("swinsurance", shipWorksInsured ? "1" : "0");

                if (shipment.InsurancePolicy == null)
                {
                    postRequest.Variables.Add("insuredwith", EnumHelper.GetApiValue(InsuredWith.NotWithApi));
                }
                else
                {
                    InsuredWith insuredWith = shipment.InsurancePolicy.CreatedWithApi ? InsuredWith.SuccessfullyInsuredViaApi : InsuredWith.FailedToInsureViaApi;
                    postRequest.Variables.Add("insuredwith", EnumHelper.GetApiValue(insuredWith));
                }

                postRequest.Variables.Add("pennyone", pennyOne ? "1" : "0");
                postRequest.Variables.Add("carrier", ShippingManager.GetCarrierName(shipmentType.ShipmentTypeCode));
                postRequest.Variables.Add("service", ShippingManager.GetServiceUsed(shipment));
                postRequest.Variables.Add("country", shipment.ShipPerson.AdjustedCountryCode(ShipmentTypeCode.None));
                postRequest.Variables.Add("tracking", tracking);
                postRequest.Variables.Add("firstname", shipment.ShipFirstName);
                postRequest.Variables.Add("lastname", shipment.ShipLastName);
                postRequest.Variables.Add("email", shipment.ShipEmail);

                // Send best rate usage data to Tango
                BestRateEventsDescription bestRateEventsDescription = new BestRateEventsDescription((BestRateEventTypes) shipment.BestRateEvents);
                postRequest.Variables.Add("bestrateevents", bestRateEventsDescription.ToString());

                ShipmentCommonDetail shipmentDetail = shipmentType.GetShipmentCommonDetail(shipment);

                // Added to prepare for Tango2...
                postRequest.Variables.Add("orderSubTotal", OrderUtility.CalculateTotal(shipment.OrderID, false).ToString());
                postRequest.Variables.Add("orderTotal", shipment.Order.OrderTotal.ToString());
                postRequest.Variables.Add("originAccount", shipmentDetail.OriginAccount);
                postRequest.Variables.Add("originPostalCode", shipment.OriginPostalCode);
                postRequest.Variables.Add("originCountry", shipment.OriginCountryCode);
                postRequest.Variables.Add("swtypeOriginal", shipmentDetail.OriginalShipmentType != null ? ((int) shipmentDetail.OriginalShipmentType).ToString() : "");
                postRequest.Variables.Add("swServiceType", shipmentDetail.ServiceType.ToString());
                postRequest.Variables.Add("packageCount", shipmentType.GetParcelCount(shipment).ToString());
                postRequest.Variables.Add("swPackagingType", shipmentDetail.PackagingType.ToString());
                postRequest.Variables.Add("weight", shipment.TotalWeight.ToString());
                postRequest.Variables.Add("packageLength", shipmentDetail.PackageLength.ToString());
                postRequest.Variables.Add("packageWidth", shipmentDetail.PackageWidth.ToString());
                postRequest.Variables.Add("packageHeight", shipmentDetail.PackageHeight.ToString());
                postRequest.Variables.Add("recipientCompany", shipment.ShipCompany);
                postRequest.Variables.Add("recipientPhone", shipment.ShipPhone);
                postRequest.Variables.Add("recipientPostalCode", shipment.ShipPostalCode);
                postRequest.Variables.Add("labelFormat", shipment.ActualLabelFormat == null ? "9" : shipment.ActualLabelFormat.Value.ToString());
                postRequest.Variables.Add("returnShipment", shipment.ReturnShipment ? "1" : "0");
                postRequest.Variables.Add("carrierCost", shipment.ShipmentCost.ToString());
                postRequest.Variables.Add("carrierInsured", carrierInsured ? "1" : "0");

                XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "LogShipmentDetails");

                // Check for error
                XmlNode errorNode = xmlResponse.SelectSingleNode("//Error");
                if (errorNode != null)
                {
                    throw new TangoException(errorNode.InnerText);
                }

                XmlNode shipmentIDNode = xmlResponse.SelectSingleNode("//OnlineShipmentID");
                if (shipmentIDNode != null)
                {
                    onlineShipmentID = shipmentIDNode.InnerText;
                }
            }

            return onlineShipmentID;
        }

        /// <summary>
        /// Void the given processed shipment to Tango
        /// </summary>
        public static void VoidShipment(StoreEntity store, ShipmentEntity shipment)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            // Get the license from the store so we know how to log
            ShipWorksLicense license = new ShipWorksLicense(store.License);

            if (!license.IsTrial)
            {
                // Create the request
                HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();

                postRequest.Variables.Add("action", "logshipmentvoided");
                postRequest.Variables.Add("swshipmentid", shipment.ShipmentID.ToString());
                postRequest.Variables.Add("license", license.Key);

                ProcessXmlRequest(postRequest, "LogShipmentVoided");
            }
        }

        /// <summary>
        /// Records License agreement and generates an Interapptive-tracked policy number.
        /// </summary>
        public static string GenerateInsurancePolicyNumber(StoreEntity store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            // Get the license from the store so we know how to log
            ShipWorksLicense license = new ShipWorksLicense(store.License);

            if (license.IsTrial)
            {
                throw new InvalidOperationException("Should not get here for trials.");
            }

            // Create the request
            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();
            postRequest.Variables.Add("action", "upicpolicy");

            postRequest.Variables.Add("machine", StoreTypeManager.GetType(store).LicenseIdentifier);
            postRequest.Variables.Add("license", license.Key);

            // Process the request
            XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "UpicPolicy");

            // check for errors
            XmlNode errorNode = xmlResponse.SelectSingleNode("//Error");
            if (errorNode != null)
            {
                throw new TangoException(errorNode.InnerText);
            }

            return XPathUtility.Evaluate(xmlResponse.CreateNavigator(), "//Policy", "");
        }

        /// <summary>
        /// Get the freemium status of the given store
        /// </summary>
        public static LicenseAccountDetail GetFreemiumStatus(EbayStoreEntity store)
        {
            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();
            postRequest.Variables.Add("action", "freemiumstatus");
            postRequest.Variables.Add("identifier", store.EBayUserID);

            // Process the request
            XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "FreemiumStatus");

            return new LicenseAccountDetail(xmlResponse, store);
        }

        /// <summary>
        /// Create a new freemium store in tango
        /// </summary>
        [NDependIgnoreLongMethod]
        public static void CreateFreemiumStore(StoreEntity store, PersonAdapter accountAddress, EndiciaPaymentInfo paymentInfo, string dazzleAccount, bool validateOnly)
        {
            EbayStoreEntity ebayStore = store as EbayStoreEntity;
            if (ebayStore == null)
            {
                throw new InvalidOperationException("Freemium status only applies to eBay stores.");
            }

            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();
            postRequest.Variables.Add("action", "freemiumcreate");
            postRequest.Variables.Add("identifier", ebayStore.EBayUserID);
            postRequest.Variables.Add("dazzleAccount", dazzleAccount);
            postRequest.Variables.Add("validateOnly", validateOnly ? "true" : "false");
            postRequest.Variables.Add("email", accountAddress.Email);
            postRequest.Variables.Add("company", accountAddress.Company);
            postRequest.Variables.Add("firstname", accountAddress.FirstName);
            postRequest.Variables.Add("lastname", accountAddress.LastName);
            postRequest.Variables.Add("address1", accountAddress.Street1);
            postRequest.Variables.Add("address2", accountAddress.Street2);
            postRequest.Variables.Add("city", accountAddress.City);
            postRequest.Variables.Add("state", accountAddress.StateProvCode);
            postRequest.Variables.Add("zip", accountAddress.PostalCode);
            postRequest.Variables.Add("country", "US");
            postRequest.Variables.Add("phone", accountAddress.Phone);
            postRequest.Variables.Add("pay_ccnumber", paymentInfo.CardNumber);
            postRequest.Variables.Add("pay_cctype", GetTangoCardType(paymentInfo.CardType).ToString());
            postRequest.Variables.Add("pay_ccexpmo", paymentInfo.CardExpirationMonth.ToString());
            postRequest.Variables.Add("pay_ccexpyr", paymentInfo.CardExpirationYear.ToString());
            postRequest.Variables.Add("pay_nameoncard", new PersonName(accountAddress).FullName);
            postRequest.Variables.Add("pay_address1", paymentInfo.CardBillingAddress.Street1);
            postRequest.Variables.Add("pay_address2", paymentInfo.CardBillingAddress.Street2);
            postRequest.Variables.Add("pay_city", paymentInfo.CardBillingAddress.City);
            postRequest.Variables.Add("pay_state", paymentInfo.CardBillingAddress.StateProvCode);
            postRequest.Variables.Add("pay_zip", paymentInfo.CardBillingAddress.PostalCode);
            postRequest.Variables.Add("pay_country", "US");

            XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "FreemiumCreate");
            LicenseAccountDetail accountDetail = new LicenseAccountDetail(xmlResponse, store);

            UpdateLicense(store, accountDetail);
        }

        /// <summary>
        /// Get the tango card type value corresponding to the given enum
        /// </summary>
        private static int GetTangoCardType(EndiciaCreditCardType cardType)
        {
            switch (cardType)
            {
                case EndiciaCreditCardType.AmericanExpress: return 1;
                case EndiciaCreditCardType.Discover: return 2;
                case EndiciaCreditCardType.MasterCard: return 3;
                case EndiciaCreditCardType.Visa: return 4;
            }

            return 0;
        }

        /// <summary>
        /// Associate the given freemium account# with the given store
        /// </summary>
        public static void SetFreemiumAccountNumber(StoreEntity store, string freemiumAccount)
        {
            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();
            postRequest.Variables.Add("action", "freemiumsetaccount");
            postRequest.Variables.Add("freemiumaccount", freemiumAccount);

            LicenseAccountDetail accountDetail = ProcessAccountRequest(postRequest, store, new ShipWorksLicense(store.License));

            UpdateLicense(store, accountDetail);
        }

        /// <summary>
        /// Update the platform, developer, and version info for the given generic store
        /// </summary>
        public static void UpdateGenericModuleInfo(GenericModuleStoreEntity store, string platform, string developer, string version)
        {
            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();
            postRequest.Variables.Add("platform", platform);
            postRequest.Variables.Add("developer", developer);
            postRequest.Variables.Add("version", version);

            ShipWorksLicense license = new ShipWorksLicense(store.License);

            if (license.IsTrial)
            {
                postRequest.Variables.Add("action", "updateTrialGenericModuleInfo");
                postRequest.Variables.Add("license", store.License);

                ProcessXmlRequest(postRequest, "UpdateTrialGenericModuleInfo");
            }
            else
            {
                postRequest.Variables.Add("action", "updateStoreGenericModuleInfo");

                ProcessAccountRequest(postRequest, store, license);
            }
        }

        /// <summary>
        /// Update the license for the store to be what it is in the given account detail
        /// </summary>
        private static void UpdateLicense(StoreEntity store, ILicenseAccountDetail accountDetail)
        {
            bool wasDirty = store.IsDirty;

            store.License = accountDetail.Key;
            store.Edition = accountDetail.Edition.Serialize();

            if (!wasDirty)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.SaveAndRefetch(store);
                }

                StoreManager.CheckForChanges();
            }
        }

        /// <summary>
        /// Upgrade the given store to the 'Paid' version of freemium with the given endicia service plan.
        /// </summary>
        public static void UpgradeFreemiumStore(StoreEntity store, int endiciaServicePlan)
        {
            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();
            postRequest.Variables.Add("action", "freemiumupgrade");
            postRequest.Variables.Add("elsplan", endiciaServicePlan.ToString());

            LicenseAccountDetail accountDetail = ProcessAccountRequest(postRequest, store, new ShipWorksLicense(store.License));

            UpdateLicense(store, accountDetail);
        }

        /// <summary>
        /// Upgrade the given trial to not be in an 'Edition' mode
        /// </summary>
        public static void UpgradeEditionTrial(StoreEntity store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();
            postRequest.Variables.Add("action", "trialeditionupgrade");
            postRequest.Variables.Add("license", store.License);

            TrialDetail trialDetail = ProcessTrialRequest(postRequest, store);

            bool wasDirty = store.IsDirty;

            store.Edition = EditionSerializer.Serialize(trialDetail.Edition);

            if (!wasDirty)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.SaveAndRefetch(store);
                }

                StoreManager.CheckForChanges();
            }
        }

        /// <summary>
        /// Process a request against a signed up customers interapptive account
        /// </summary>
        private static LicenseAccountDetail ProcessAccountRequest(HttpVariableRequestSubmitter postRequest, StoreEntity store, ShipWorksLicense license)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            postRequest.Variables.Add("machine", StoreTypeManager.GetType(store).LicenseIdentifier);
            postRequest.Variables.Add("license", license.Key);

            // Process the request
            XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "AccountRequest");

            return new LicenseAccountDetail(xmlResponse, store);
        }

        /// <summary>
        /// Process a trial creation \ information request.
        /// </summary>
        private static TrialDetail ProcessTrialRequest(HttpVariableRequestSubmitter postRequest, StoreEntity store)
        {
            StoreType storeType = StoreTypeManager.GetType(store);

            // These go with every trial request
            postRequest.Variables.Add("storecode", storeType.TangoCode);
            postRequest.Variables.Add("identifier", storeType.LicenseIdentifier);

            // Process the request
            XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "ProcessTrialRequest");

            // Create the details
            TrialDetail trialDetail = new TrialDetail(xmlResponse, store);

            return trialDetail;
        }

        /// <summary>
        /// Process the given request against the interapptive license server
        /// </summary>
        private static XmlDocument ProcessXmlRequest(HttpVariableRequestSubmitter postRequest, string logEntryName)
        {
            XmlDocument xmlResponse = new XmlDocument();

            try
            {
                string resultXml = ProcessRequest(postRequest, logEntryName);
                xmlResponse.LoadXml(resultXml);
                return xmlResponse;
            }
            catch (XmlException ex)
            {
                throw new TangoException(
                    "The ShipWorks server returned an invalid response. \n\n" +
                    "Details: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Process the given request against the interapptive license server
        /// </summary>
        private static string ProcessRequest(HttpVariableRequestSubmitter postRequest, string logEntryName)
        {
            // Timeout
            postRequest.Timeout = TimeSpan.FromSeconds(60);

            // Get the url
            string tangoUrl = string.Format("https://www.interapptive.com/{0}/shipworks.php", UseTestServer ? "tango_private" : "account");

            // Set the uri
            postRequest.Uri = new Uri(tangoUrl);

            // Logging
            ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.ShipWorks, logEntryName);
            logEntry.LogRequest(postRequest);

            // Setup parameters
            postRequest.RequestSubmitting += delegate (object sender, HttpRequestSubmittingEventArgs e)
            {
                e.HttpWebRequest.KeepAlive = false;

                e.HttpWebRequest.UserAgent = "shipworks";
                e.HttpWebRequest.Headers.Add("X-SHIPWORKS-VERSION", Version);

                e.HttpWebRequest.Headers.Add("X-SHIPWORKS-USER", SecureText.Decrypt("C5NOiKdNaM/324R7sIjFUA==", "interapptive"));
                e.HttpWebRequest.Headers.Add("X-SHIPWORKS-PASS", SecureText.Decrypt("lavEgsQoKGM=", "interapptive"));
            };

            try
            {
                // First validate that we are connecting to interapptive, and not a fake redirect to steal passwords and such.  Doing this pre-call
                // also prevents stealing the headers user\pass with fiddler
                ValidateSecureConnection(postRequest.Uri);

                using (IHttpResponseReader postResponse = postRequest.GetResponse())
                {
                    // Ensure the site has a valid interapptive secure certificate
                    ValidateInterapptiveCertificate(postResponse.HttpWebRequest);

                    string result = postResponse.ReadResult().Trim();

                    logEntry.LogResponse(result);

                    return result;
                }
            }
            catch (Exception ex)
            {
                if (WebHelper.IsWebException(ex))
                {
                    throw new TangoException("An error occurred connecting to the ShipWorks server:\n\n" + ex.Message, ex);
                }

                throw;
            }
        }

        /// <summary>
        /// Ensure the connection to the given URI is a valid interapptive secure connection
        /// </summary>
        public static void ValidateSecureConnection(Uri uri)
        {
#if !DEBUG
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);
            request.KeepAlive = false;
            request.UserAgent = "shipworks";

            using (WebResponse response = request.GetResponse())
            {
                ValidateInterapptiveCertificate(request);
            }
#endif
        }

        /// <summary>
        /// Validate that there is an accurate interapptive certificate attached to the web request
        /// </summary>
        private static void ValidateInterapptiveCertificate(HttpWebRequest httpWebRequest)
        {
#pragma warning disable 168
            X509Certificate certificate;
#pragma warning restore 168

#if !DEBUG
            if (httpWebRequest.ServicePoint == null)
            {
                throw new TangoException("The SSL certificate on the server is invalid.");
            }

            certificate = httpWebRequest.ServicePoint.Certificate;

            if (certificate == null)
            {
                throw new TangoException("The SSL certificate on the server is invalid.");
            }

            if (certificate.Subject.IndexOf("www.interapptive.com") == -1 ||
                certificate.Subject.IndexOf("Interapptive, Inc") == -1)
            {
                throw new TangoException("The SSL certificate on the server is invalid.");
            }
#endif
        }


        /// <summary>
        /// Indicates if the test server should be used instead of the live server
        /// </summary>
        public static bool UseTestServer
        {
            get { return InterapptiveOnly.Registry.GetValue("TangoTestServer", false); }
            set { InterapptiveOnly.Registry.SetValue("TangoTestServer", value); }
        }

        /// <summary>
        /// Gets license information for the given email and password
        /// </summary>
        public static GenericResult<IActivationResponse> ActivateLicense(string email, string password)
        {
            try
            {
                Activation.WebServices.Activation activationService = new Activation.WebServices.Activation(new ApiLogEntry(ApiLogSource.ShipWorks, "Activation")) { Url = ActivationUrl };
                CustomerLicenseInfoV1 customerLicenseInfo = activationService.GetCustomerLicenseInfo(email, password);

                return GenericResult.FromSuccess<IActivationResponse>(new ActivationResponse(customerLicenseInfo));
            }
            catch (SoapException ex)
            {
                return GenericResult.FromError<IActivationResponse>(ex.Message);
            }
        }

        /// <summary>
        /// Gets the license capabilities.
        /// </summary>
        public static ILicenseCapabilities GetLicenseCapabilities(ICustomerLicense license)
        {
            XmlDocument xmlResponse = GetLicenseCapabilitiesXmlDocument(license);

            try
            {
                return new LicenseCapabilities(xmlResponse);
            }
            catch (ShipWorksLicenseException ex)
            {
                throw new TangoException(ex);
            }
        }

        /// <summary>
        /// Gets the license capabilities.
        /// </summary>
        private static XmlDocument GetLicenseCapabilitiesXmlDocument(ICustomerLicense license)
        {
            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();

            postRequest.Variables.Add("action", "login");
            postRequest.Variables.Add("customerlicense", license.Key);
            postRequest.Variables.Add("version", Version);

            XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "GetLicenseCapabilities");

            return xmlResponse;
        }

        /// <summary>
        /// Makes a request to Tango to add a store
        /// </summary>
        public static IAddStoreResponse AddStore(ICustomerLicense license, StoreEntity store)
        {
            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();

            StoreType storeType = StoreTypeManager.GetType(store);

            postRequest.Variables.Add("action", "createstore");
            postRequest.Variables.Add("customerlicense", license.Key);
            postRequest.Variables.Add("storecode", storeType.TangoCode);
            postRequest.Variables.Add("identifier", storeType.LicenseIdentifier);
            postRequest.Variables.Add("version", Version);
            postRequest.Variables.Add("storeinfo", store.StoreName);

            XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "AddStore");

            try
            {
                return new AddStoreResponse(xmlResponse);
            }
            catch (ShipWorksLicenseException ex)
            {
                throw new TangoException(ex);
            }
        }

        /// <summary>
        /// Gets the active stores from Tango
        /// </summary>
        public static IEnumerable<ActiveStore> GetActiveStores(ICustomerLicense license)
        {
            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();

            postRequest.Variables.Add("action", "getactivestores");
            postRequest.Variables.Add("customerlicense", license.Key);
            postRequest.Variables.Add("version", Version);

            XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "GetActiveStores");

            CheckResponseForErrors(xmlResponse);
            List<ActiveStore> activeStores = new GetActiveStoresResponse(xmlResponse).ActiveStores;

            return activeStores;
        }

        /// <summary>
        /// Deletes a store from Tango
        /// </summary>
        public static void DeleteStore(ICustomerLicense customerLicense, string storeLicenseKey)
        {
            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();

            postRequest.Variables.Add("action", "deletestore");
            postRequest.Variables.Add("customerlicense", customerLicense.Key);
            postRequest.Variables.Add("storelicensekey[]", storeLicenseKey);
            postRequest.Variables.Add("version", Version);

            XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "DeleteStore");

            try
            {
                CheckResponseForErrors(xmlResponse);
            }
            catch (TangoException ex)
            {
                // Tango returned an error while deleting the store
                // at this point the store has been removed from
                // the shipworks database, log the error and move on
                log.Error(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a stores from Tango
        /// </summary>
        public static void DeleteStores(ICustomerLicense customerLicense, IEnumerable<string> storeLicenseKeys)
        {
            string licenseKeyParam = string.Join(",", storeLicenseKeys);

            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();

            postRequest.Variables.Add("action", "deletestore");
            postRequest.Variables.Add("customerlicense", customerLicense.Key);
            postRequest.Variables.Add("storelicensekey[]", licenseKeyParam);
            postRequest.Variables.Add("version", Version);

            XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "DeleteStores");

            try
            {
                CheckResponseForErrors(xmlResponse);
            }
            catch (TangoException ex)
            {
                // Tango returned an error while deleting the store
                // at this point the store has been removed from
                // the shipworks database, log the error and move on
                log.Error(ex.Message);
            }
        }

        /// <summary>
        /// Associates a free Stamps.com account with a customer license.
        /// </summary>
        public static void AssociateStampsUsernameWithLicense(string licenseKey, string stampsUsername, string stampsPassword)
        {
            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();

            postRequest.Variables.Add("action", "associateexistingstampswithshipworks");
            postRequest.Variables.Add("customerlicense", licenseKey);
            postRequest.Variables.Add("version", Version);
            postRequest.Variables.Add("stampsusername", stampsUsername);
            postRequest.Variables.Add("stampspassword", stampsPassword);

            try
            {
                XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "AssociateStampsUsernameWithLicense");

                CheckResponseForErrors(xmlResponse);
            }
            catch (TangoException ex)
            {
                log.Error(ex.Message);
            }
        }

        /// <summary>
        /// Checks the response for errors.
        /// </summary>
        private static void CheckResponseForErrors(XmlDocument xmlResponse)
        {
            XPathNamespaceNavigator navigator = new XPathNamespaceNavigator(xmlResponse);

            string error = XPathUtility.Evaluate(navigator, "//Error", string.Empty);

            if (!error.Equals(string.Empty))
            {
                throw new TangoException(error);
            }
        }
    }
}
