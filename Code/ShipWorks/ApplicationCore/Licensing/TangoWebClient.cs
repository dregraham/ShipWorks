using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Linq;
using Autofac;
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
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Account;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for working with the interapptive license server
    /// </summary>
    [NDependIgnoreLongTypes]
    public static class TangoWebClient
    {
        private const string ActivationUrl = "https://interapptive.com/ShipWorksNet/ActivationV1.svc";

        // Logger
        private static readonly ILog log = LogManager.GetLogger(typeof(TangoWebClient));

        private static InsureShipAffiliateProvider insureShipAffiliateProvider = new InsureShipAffiliateProvider();

        private readonly static Lazy<Version> version = new Lazy<Version>(() =>
        {
            // Tango requires a specific version in order to know when to return
            // legacy responses or new response for the customer license. This is
            // primarily for debug/internal versions of ShipWorks that have 0.0.0.x
            // version number.
            Version assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
            Version minimumVersion = new Version(5, 0, 0, 0);

            return assemblyVersion.Major == 0 ? minimumVersion : assemblyVersion;
        });

        private static readonly DateTime nextSecureConnectionValidation = DateTime.MinValue;

        /// <summary>
        /// Gets the version - If version is under 5.0.0.0, return 5.0.0.0
        /// </summary>
        public static string Version => version.Value.ToString(4);

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

            return ProcessAccountRequest(postRequest, store, license, true);
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
                XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "associateshipworkswithitself", true);
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
        public static ILicenseAccountDetail GetLicenseStatus(string licenseKey, StoreEntity store, bool collectTelemetry)
        {
            ShipWorksLicense license = new ShipWorksLicense(licenseKey);

            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();
            postRequest.Variables.Add("action", "getstatus");

            LicenseAccountDetail licenseAccountDetail = ProcessAccountRequest(postRequest, store, license, collectTelemetry);

            InsureShipAffiliate insureShipAffiliate = new InsureShipAffiliate(licenseAccountDetail.TangoStoreID, licenseAccountDetail.TangoCustomerID);
            insureShipAffiliateProvider.Add(store.StoreID, insureShipAffiliate);

            return licenseAccountDetail;
        }

        /// <summary>
        /// Gets the Tango customer id for a license.
        /// </summary>
        public static string GetTangoCustomerId()
        {
            StoreEntity store = StoreManager.GetEnabledStores()
                                    .FirstOrDefault(s => new ShipWorksLicense(s.License).IsTrial == false) ??
                                StoreManager.GetAllStores()
                                    .FirstOrDefault(s => new ShipWorksLicense(s.License).IsTrial == false);

            try
            {
                return store != null ? GetLicenseStatus(store.License, store, false).TangoCustomerID : string.Empty;
            }
            catch (TangoException ex)
            {
                log.Error(ex);
                return string.Empty;
            }
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
                GetLicenseStatus(store.License, store, true);
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

            XmlDocument nudgesDoc = ProcessXmlRequest(postRequest, "GetNudges", true);
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

            ProcessRequest(postRequest, "LogNudgeOption", true);
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
            XmlDocument responseXmlDocument = ProcessXmlRequest(postRequest, "GetCounterRatesCreds", true);

            // Pull the credentials from the response; none of the fields are encrypted in the response
            // so we can easily/quickly update them in Tango if they ever need to change
            // Use the correct version that ShipWorks should work with.
            string fedExBasePath = string.Format("/CounterRateCredentials/FedEx_v{0}/", FedExWebServiceVersions.Ship);

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
            XmlDocument responseXmlDocument = ProcessXmlRequest(postRequest, "CarrierCertificate", true);

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

            ProcessXmlRequest(postRequest, "SendAccountPassword", true);
        }

        /// <summary>
        /// Send the user their username using the specified email address
        /// </summary>s
        public static void SendAccountUsername(string email, string username)
        {
            HttpVariableRequestSubmitter postRequest = new HttpVariableRequestSubmitter();
            postRequest.Variables.Add("action", "email");
            postRequest.Variables.Add("emailtype", "username");
            postRequest.Variables.Add("emailaddress", email);
            postRequest.Variables.Add("username", username);

            ProcessXmlRequest(postRequest, "SendAccountUsername", true);
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

            ProcessXmlRequest(postRequest, "SubmitInsuranceClaim", true);
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

            XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "LogStampsAccount", true);

            // Check for error
            XmlNode errorNode = xmlResponse.SelectSingleNode("//Error");
            if (errorNode != null)
            {
                throw new TangoException(errorNode.InnerText);
            }
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

                ProcessXmlRequest(postRequest, "LogShipmentVoided", true);
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
            XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "UpicPolicy", true);

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
            XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "FreemiumStatus", true);

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

            XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "FreemiumCreate", true);
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

            LicenseAccountDetail accountDetail = ProcessAccountRequest(postRequest, store, new ShipWorksLicense(store.License), true);

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

                ProcessXmlRequest(postRequest, "UpdateTrialGenericModuleInfo", true);
            }
            else
            {
                postRequest.Variables.Add("action", "updateStoreGenericModuleInfo");

                ProcessAccountRequest(postRequest, store, license, true);
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

            LicenseAccountDetail accountDetail = ProcessAccountRequest(postRequest, store, new ShipWorksLicense(store.License), true);

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
        private static LicenseAccountDetail ProcessAccountRequest(HttpVariableRequestSubmitter postRequest, StoreEntity store, ShipWorksLicense license, bool collectTelemetry)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            postRequest.Variables.Add("machine", StoreTypeManager.GetType(store).LicenseIdentifier);
            postRequest.Variables.Add("license", license.Key);

            // Process the request
            XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "AccountRequest", collectTelemetry);

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
            XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "ProcessTrialRequest", true);

            // Grab the shipment type functionality node
            XmlNode shipmentTypeFunctionality = xmlResponse.SelectSingleNode("//License/ShipmentTypeFunctionality");
            // Grab the Best Rate shipment type functionality node
            XmlNode bestRateShipmentTypeFunctionality = shipmentTypeFunctionality?.SelectSingleNode("//ShipmentType[@TypeCode='14']");

            if (bestRateShipmentTypeFunctionality?.SelectSingleNode("Restriction")?.InnerText.ToLower() == "disabled")
            {
                // If it exists remove it
                shipmentTypeFunctionality.RemoveChild(bestRateShipmentTypeFunctionality);

                // add the new default which enables best rate but limits it to local rating only
                XmlDocumentFragment newBestRateFunctionality = xmlResponse.CreateDocumentFragment();
                newBestRateFunctionality.InnerXml =
                    @"<ShipmentType TypeCode='14'> 
                        <Feature>
                            <Type>BestRateUpsRestriction</Type>
                            <Config>True</Config>
                        </Feature>
                        <Feature>
                            <Type>RateResultCount</Type>
                            <Config>5</Config>
                        </Feature>
                    </ShipmentType>";

                shipmentTypeFunctionality.AppendChild(newBestRateFunctionality);
            }

            // Create the details
            TrialDetail trialDetail = new TrialDetail(xmlResponse, store);

            return trialDetail;
        }

        /// <summary>
        /// Process the given request against the interapptive license server
        /// </summary>
        private static XmlDocument ProcessXmlRequest(IHttpVariableRequestSubmitter postRequest, string logEntryName, bool collectTelemetry)
        {
            using (var lifetimeScope = IoC.BeginLifetimeScope())
            {
                var client = lifetimeScope.Resolve<ITangoWebRequestClient>();
                return client.ProcessXmlRequest(postRequest, logEntryName, collectTelemetry)
                    .Match(x => x,
                    ex =>
                    {
                        if (ex is XmlException xmlEx)
                        {
                            throw new TangoException(
                                "The ShipWorks server returned an invalid response. \n\n" +
                                "Details: " + xmlEx.Message, xmlEx);
                        }

                        if (WebHelper.IsWebException(ex))
                        {
                            throw new TangoException("An error occurred connecting to the ShipWorks server:\n\n" + ex.Message, ex);
                        }

                        throw ex;
                    });
            }
        }

        /// <summary>
        /// Process the given request against the interapptive license server
        /// </summary>
        private static string ProcessRequest(IHttpVariableRequestSubmitter postRequest, string logEntryName, bool collectTelemetry)
        {
            using (var lifetimeScope = IoC.BeginLifetimeScope())
            {
                var client = lifetimeScope.Resolve<ITangoWebRequestClient>();
                return client.ProcessRequest(postRequest, logEntryName, collectTelemetry)
                    .Match(x => x,
                    ex =>
                    {
                        if (WebHelper.IsWebException(ex))
                        {
                            throw new TangoException("An error occurred connecting to the ShipWorks server:\n\n" + ex.Message, ex);
                        }

                        throw ex;
                    });
            }
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

            XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "GetLicenseCapabilities", true);

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

            XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "AddStore", true);

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

            XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "GetActiveStores", true);

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

            XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "DeleteStore", true);

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

            XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "DeleteStores", true);

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
                XmlDocument xmlResponse = ProcessXmlRequest(postRequest, "AssociateStampsUsernameWithLicense", true);

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

            if (!string.IsNullOrEmpty(error))
            {
                throw new TangoException(error);
            }
        }
    }
}
