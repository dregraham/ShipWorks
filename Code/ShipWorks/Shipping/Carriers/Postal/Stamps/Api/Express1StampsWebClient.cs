using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Api.Labels;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices.v29;
using Interapptive.Shared.Utility;
using System.Web.Services.Protocols;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Net;
using System.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data;
using System.Diagnostics;
using ShipWorks.Data.Connection;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.UI;
using ShipWorks.Shipping.Editing;
using Interapptive.Shared.Business;
using ShipWorks.ApplicationCore;
using log4net;
using ShipWorks.Templates.Tokens;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration;
using System.Xml.Linq;
using Microsoft.Web.Services3;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;
using AccountInfo = ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices.v29.AccountInfo;
using Address = ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices.v29.Address;
using ContentTypeV2 = ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices.v29.ContentTypeV2;
using CreateIndiciumModeV1 = ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices.v29.CreateIndiciumModeV1;
using Credentials = ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices.v29.Credentials;
using CustomsLine = ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices.v29.CustomsLine;
using CustomsV2 = ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices.v29.CustomsV2;
using EltronPrinterDPIType = ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices.v29.EltronPrinterDPIType;
using ImageType = ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices.v29.ImageType;
using NonDeliveryOption = ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices.v29.NonDeliveryOption;
using PackageTypeV6 = ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices.PackageTypeV6;
using PurchaseStatus = ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices.v29.PurchaseStatus;
using RegistrationStatus = ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices.v29.RegistrationStatus;
using ResidentialDeliveryIndicatorType = ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices.v29.ResidentialDeliveryIndicatorType;
using ServiceType = ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices.v29.ServiceType;
using StatusCodes = ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices.v29.StatusCodes;
using UrlType = ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices.v29.UrlType;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Api
{
    /// <summary>
    /// Central point where API stuff goes through for stamps.com
    /// </summary>
    public class Express1StampsWebClient : IStampsWebClient
    {
        // These lengths come from the error that Stamps' API gives us when we send data that is too long
        private const int MaxCustomsContentDescriptionLength = 20;
        private const int MaxCustomsItemDescriptionLength = 60;

        private readonly ILog log;
        private readonly LogEntryFactory logEntryFactory;
        private readonly ICarrierAccountRepository<StampsAccountEntity> accountRepository;

        // Maps stamps.com usernames to their latest authenticator tokens
        static Dictionary<string, string> usernameAuthenticatorMap = new Dictionary<string, string>();

        // Maps stamps.com usernames to the object lock used to make sure only one thread is trying to authenticate at a time
        static Dictionary<string, object> authenticationLockMap = new Dictionary<string, object>();

        // Cleansed address map so we don't do common addresses over and over again
        static Dictionary<PersonAdapter, Address> cleansedAddressMap = new Dictionary<PersonAdapter, Address>();

        // Express1 API service connection info 
        static Express1StampsConnectionDetails express1StampsConnectionDetails = new Express1StampsConnectionDetails();

        private readonly ICertificateInspector certificateInspector;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="StampsWebClient"/> class.
        /// </summary>
        public Express1StampsWebClient()
            : this(new Express1StampsAccountRepository(), new LogEntryFactory(), new TrustingCertificateInspector())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StampsWebClient" /> class.
        /// </summary>
        public Express1StampsWebClient(ICarrierAccountRepository<StampsAccountEntity> accountRepository, LogEntryFactory logEntryFactory, ICertificateInspector certificateInspector)
        {
            this.accountRepository = accountRepository;
            this.logEntryFactory = logEntryFactory;
            this.log = LogManager.GetLogger(typeof(Express1StampsWebClient));
            this.certificateInspector = certificateInspector;
        }

        /// <summary>
        /// Indicates if the test server should be used instead of the live server
        /// </summary>
        public static bool UseTestServer
        {
            get { return InterapptiveOnly.Registry.GetValue("StampsTestServer", false); }
            set { InterapptiveOnly.Registry.SetValue("StampsTestServer", value); }
        }

        /// <summary>
        /// Create the web service instance with the appropriate URL
        /// </summary>
        private SwsimV29 CreateWebService(string logName)
        {
            return CreateWebService(logName, LogActionType.Other);
        }

        /// <summary>
        /// Create the web service instance with the appropriate URL
        /// </summary>
        private SwsimV29 CreateWebService(string logName, LogActionType logActionType)
        {
            SwsimV29 webService = new Express1StampsServiceWrapper(logEntryFactory.GetLogEntry(ApiLogSource.UspsExpress1Stamps, logName, logActionType))
            {
                Url = express1StampsConnectionDetails.ServiceUrl
            };

            return webService;
        }

        /// <summary>
        /// Authenticate the given user with Stamps.com.  If 
        /// </summary>
        public void AuthenticateUser(string username, string password)
        {
            try
            {
                // Output parameters from stamps.com
                DateTime lastLoginTime = new DateTime();
                bool clearCredential = false;

                string bannerText = string.Empty;
                bool passwordExpired = false;

                using (SwsimV29 webService = CreateWebService("Authenticate"))
                {
                    CheckCertificate(webService.Url);

                    string auth = webService.AuthenticateUser(new Credentials
                    {
                        IntegrationID = new Guid(express1StampsConnectionDetails.ApiKey),
                        Username = username,
                        Password = password
                    }, out lastLoginTime, out clearCredential, out bannerText, out passwordExpired);

                    usernameAuthenticatorMap[username] = auth;
                }
            }
            catch (SoapException ex)
            {
                throw new StampsApiException(ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(StampsException));
            }
        }

        /// <summary>
        /// Changes the contract associated with the given account based on the contract type provided.
        /// </summary>
        public void ChangeToExpeditedPlan(StampsAccountEntity account, string promoCode)
        {
            throw new InvalidOperationException("An Express1 contract cannot be changed.");
        }

        /// <summary>
        /// Checks with Stamps.com API to get the contract type of the account.
        /// </summary>
        public StampsAccountContractType GetContractType(StampsAccountEntity account)
        {
            throw new InvalidOperationException("The contract type is not applicable for Express1.");
        }

        /// <summary>
        /// Makes a request to the specified url, and determines it's CertificateSecurityLevel
        /// </summary>
        private void CheckCertificate(string url)
        {
            CertificateRequest certificateRequest = new CertificateRequest(new Uri(url), certificateInspector);

            CertificateSecurityLevel certificateSecurityLevel = certificateRequest.Submit();

            // If we are using the test server, it's not https, so none will be returned.
            // Also, our test server credentials should not be using "real money", so not such a terrible thing if someone
            // hi-jacked them.
            if (Express1StampsConnectionDetails.UseTestServer && certificateSecurityLevel == CertificateSecurityLevel.None)
            {
                return;
            }

            if (certificateSecurityLevel != CertificateSecurityLevel.Trusted)
            {
                string description = EnumHelper.GetDescription(ShipmentTypeCode.Express1Stamps);
                throw new StampsException(string.Format("ShipWorks is unable to make a secure connection to {0}.", description));
            }
        }

        /// <summary>
        /// Get the account info for the given Stamps.com user name
        /// </summary>
        public object GetAccountInfo(StampsAccountEntity account)
        {
            return AuthenticationWrapper(() => { return GetAccountInfoInternal(account); }, account);
        }

        /// <summary>
        /// The internal GetAccountInfo implementation that is intended to be wrapped by the auth wrapper
        /// </summary>
        private AccountInfo GetAccountInfoInternal(StampsAccountEntity account)
        {
            AccountInfo accountInfo;

            using (SwsimV29 webService = CreateWebService("GetAccountInfo"))
            {
                // Address and CustomerEmail are not returned by Express1, so do not use them.
                Address address;
                string email;

                string auth = webService.GetAccountInfo(GetAuthenticator(account), out accountInfo, out address, out email);
                usernameAuthenticatorMap[account.Username] = auth;
            }

            return accountInfo;
        }

        /// <summary>
        /// Get the Stamps.com URL of the given urlType
        /// </summary>
        public string GetUrl(StampsAccountEntity account, UrlType urlType)
        {
            return AuthenticationWrapper(() => { return GetUrlInternal(account, urlType); }, account);
        }

        /// <summary>
        /// The internal GetUrl implementation that is intended to be wrapped by the auth wrapper
        /// </summary>
        private string GetUrlInternal(StampsAccountEntity account, UrlType urlType)
        {
            string url;

            using (SwsimV29 webService = CreateWebService("GetURL"))
            {
                string auth = webService.GetURL(GetAuthenticator(account), urlType, string.Empty, out url);
                usernameAuthenticatorMap[account.Username] = auth;
            }

            return url;
        }

        /// <summary>
        /// Purchase postage for the given account for the specified amount.  ControlTotal is the ControlTotal value last retrieved from GetAccountInfo.
        /// </summary>
        public void PurchasePostage(StampsAccountEntity account, decimal amount, decimal controlTotal)
        {
            AuthenticationWrapper(() => { PurchasePostageInternal(account, amount, controlTotal); return true; }, account);
        }

        /// <summary>
        /// The internal PurchasePostageInternal implementation intended to be wrapped by the auth wrapper
        /// </summary>
        private void PurchasePostageInternal(StampsAccountEntity account, decimal amount, decimal controlTotal)
        {
            PurchaseStatus purchaseStatus;
            int transactionID;
            WebServices.v29.PostageBalance postageBalance;
            string rejectionReason;

            bool miRequired_Unused;

            using (SwsimV29 webService = CreateWebService("PurchasePostage"))
            {
                string auth = webService.PurchasePostage(GetAuthenticator(account), amount, controlTotal, null, null, out purchaseStatus, out transactionID, out postageBalance, out rejectionReason, out miRequired_Unused);
                usernameAuthenticatorMap[account.Username] = auth;
            }

            if (purchaseStatus == PurchaseStatus.Rejected)
            {
                throw new StampsException(rejectionReason);
            }
        }

        /// <summary>
        /// Get the rates for the given shipment based on its settings
        /// </summary>
        public List<RateResult> GetRates(ShipmentEntity shipment)
        {
            StampsAccountEntity account = accountRepository.GetAccount(shipment.Postal.Stamps.StampsAccountID);
            if (account == null)
            {
                throw new StampsException("No Stamps.com account is selected for the shipment.");
            }

            if (shipment.ReturnShipment && !(PostalUtility.IsDomesticCountry(shipment.OriginCountryCode) && PostalUtility.IsDomesticCountry(shipment.ShipCountryCode)))
            {
                throw new StampsException("Return shipping labels can only be used to send packages to and from domestic addresses.");
            }

            try
            {
                List<RateResult> rates = new List<RateResult>();

                foreach (RateV11 stampsRate in AuthenticationWrapper(() => { return GetRatesInternal(shipment, account); }, account))
                {
                    PostalServiceType serviceType = StampsUtility.GetPostalServiceType(ConvertServiceType(stampsRate.ServiceType));

                    RateResult baseRate = null;

                    // If its a rate that has sig\deliv, then you can's select the core rate itself
                    if (stampsRate.AddOns.Any(a => a.AddOnType == AddOnTypeV4.USADC))
                    {
                        baseRate = new RateResult(
                            PostalUtility.GetPostalServiceTypeDescription(serviceType),
                            stampsRate.DeliverDays.Replace("Days", ""))
                        {
                            Tag = new PostalRateSelection(serviceType, PostalConfirmationType.None),
                            ProviderLogo = EnumHelper.GetImage((ShipmentTypeCode)shipment.ShipmentType)
                        };
                    }
                    else
                    {
                        baseRate = new RateResult(
                            PostalUtility.GetPostalServiceTypeDescription(serviceType),
                            stampsRate.DeliverDays.Replace("Days", ""),
                            stampsRate.Amount,
                            new PostalRateSelection(serviceType, PostalConfirmationType.None))
                            {
                                ProviderLogo = EnumHelper.GetImage((ShipmentTypeCode)shipment.ShipmentType)
                            };
                    }

                    PostalUtility.SetServiceDetails(baseRate, serviceType, stampsRate.DeliverDays);

                    rates.Add(baseRate);

                    // Add a rate for each add-on
                    foreach (AddOnV4 addOn in stampsRate.AddOns)
                    {
                        string name = null;
                        PostalConfirmationType confirmationType = PostalConfirmationType.None;

                        switch (addOn.AddOnType)
                        {
                            case AddOnTypeV4.USADC:
                                name = string.Format("       Delivery Confirmation ({0:c})", addOn.Amount);
                                confirmationType = PostalConfirmationType.Delivery;
                                break;

                            case AddOnTypeV4.USASC:
                                name = string.Format("       Signature Confirmation ({0:c})", addOn.Amount);
                                confirmationType = PostalConfirmationType.Signature;
                                break;
                        }

                        if (name != null)
                        {
                            RateResult addOnRate = new RateResult(
                                name,
                                string.Empty,
                                stampsRate.Amount + addOn.Amount,
                                new PostalRateSelection(serviceType, confirmationType));

                            PostalUtility.SetServiceDetails(addOnRate, serviceType, stampsRate.DeliverDays);

                            rates.Add(addOnRate);
                        }
                    }
                }

                return rates;
            }
            catch (StampsApiException ex)
            {
                if (ex.Message.ToUpperInvariant().Contains("THE USERNAME OR PASSWORD ENTERED IS NOT CORRECT"))
                {
                    // Provide a little more context as to which user name/password was incorrect in the case
                    // where there's multiple accounts or Express1 for Stamps is being used to compare rates
                    string message = string.Format("ShipWorks was unable to connect to {0} with account {1}.{2}Check that your account credentials are correct.",
                                    StampsAccountManager.GetResellerName(StampsResellerType.Express1),
                                    account.Username,
                                    Environment.NewLine);

                    throw new StampsException(message, ex);
                }

                // This isn't an authentication exception, so just throw the original exception
                throw;
            }
        }


        /// <summary>
        /// The internal GetRates implementation intended to be wrapped by the auth wrapper
        /// </summary>
        private List<RateV11> GetRatesInternal(ShipmentEntity shipment, StampsAccountEntity account)
        {
            RateV11 rate = CreateRateForRating(shipment, account);

            List<RateV11> rateResults = new List<RateV11>();

            using (SwsimV29 webService = CreateWebService("GetRates", LogActionType.GetRates))
            {
                CheckCertificate(webService.Url);

                RateV11[] ratesArray;

                string auth = webService.GetRates(GetAuthenticator(account), rate, out ratesArray);
                usernameAuthenticatorMap[account.Username] = auth;

                rateResults = ratesArray.ToList();
            }

            List<RateV11> noConfirmationServiceRates = new List<RateV11>();

            // If its a "Flat" then FirstClass and Priority can't have a confirmation
            PostalPackagingType packagingType = (PostalPackagingType)shipment.Postal.PackagingType;
            if (packagingType == PostalPackagingType.Envelope || packagingType == PostalPackagingType.LargeEnvelope)
            {
                noConfirmationServiceRates.AddRange(rateResults.Where(r => r.ServiceType == ServiceType.USFC || r.ServiceType == ServiceType.USPM));
            }

            // Remove the Delivery and Signature add ons from all those that shouldn't support it
            foreach (RateV11 noConfirmationServiceRate in noConfirmationServiceRates)
            {
                if (noConfirmationServiceRate != null && noConfirmationServiceRate.AddOns != null)
                {
                    noConfirmationServiceRate.AddOns = noConfirmationServiceRate.AddOns.Where(a => a.AddOnType != AddOnTypeV4.USASC && a.AddOnType != AddOnTypeV4.USADC).ToArray();
                }
            }

            return rateResults;
        }

        /// <summary>
        /// Cleans the address of the given person using the specified stamps account
        /// </summary>
        private Address CleanseAddress(StampsAccountEntity account, PersonAdapter person, bool requireFullMatch)
        {
            return AuthenticationWrapper(() => { return CleanseAddressInternal(person, account, requireFullMatch); }, account);
        }

        /// <summary>
        /// Internal CleanseAddress implementation intended to be warpped by the auth wrapper
        /// </summary>
        private Address CleanseAddressInternal(PersonAdapter person, StampsAccountEntity account, bool requireFullMatch)
        {
            Address address;
            if (cleansedAddressMap.TryGetValue(person, out address))
            {
                return address;
            }

            address = CreateAddress(person);

            bool addressMatch;
            bool cityStateZipOK;
            ResidentialDeliveryIndicatorType residentialIndicator;
            bool? isPoBox;
            bool isPoBoxSpecified;
            Address[] candidates;
            StatusCodes statusCodes;

            using (SwsimV29 webService = CreateWebService("CleanseAddress"))
            {
                string auth = webService.CleanseAddress(GetAuthenticator(account), ref address, out addressMatch, out cityStateZipOK, out residentialIndicator, out isPoBox, out isPoBoxSpecified, out candidates, out statusCodes);
                usernameAuthenticatorMap[account.Username] = auth;

                if (!addressMatch)
                {
                    if (!cityStateZipOK)
                    {
                        throw new StampsException(string.Format("The address for '{0}' is not a valid address.", new PersonName(person).FullName));
                    }
                    else if (requireFullMatch)
                    {
                        throw new StampsException(string.Format("The city, state, and postal code for '{0}' is valid, but the full address is not.", new PersonName(person).FullName));
                    }
                }
            }

            cleansedAddressMap[person] = address;

            return address;
        }
        
        /// <summary>
        /// Creates the scan form.
        /// </summary>
        /// <param name="shipments">The shipments.</param>
        /// <param name="stampsAccountEntity">The stamps account entity.</param>
        /// <returns>An XDocument having a ScanForm node as the root which contains a TransactionId and Url nodes to 
        /// identify results from Stamps.com</returns>
        public XDocument CreateScanForm(IEnumerable<StampsShipmentEntity> shipments, StampsAccountEntity stampsAccountEntity)
        {
            if (stampsAccountEntity == null)
            {
                throw new StampsException("No Stamps.com account is selected for the SCAN form.");
            }

            XDocument result = new XDocument();
            AuthenticationWrapper(() => { result = CreateScanFormInternal(shipments, stampsAccountEntity); return true; }, stampsAccountEntity);

            return result;
        }

        /// <summary>
        /// Creates the scan form via the Stamps.com API and intended to be wrapped by the authentication wrapper.
        /// </summary>
        /// <param name="shipments">The shipments.</param>
        /// <param name="stampsAccountEntity">The stamps account entity.</param>
        /// <returns>An XDocument having a ScanForm node as the root which contains a TransactionId and Url nodes to 
        /// identify results from Stamps.com</returns>
        private XDocument CreateScanFormInternal(IEnumerable<StampsShipmentEntity> shipments, StampsAccountEntity stampsAccountEntity)
        {
            List<Guid> stampsTransactions = shipments.Select(s => s.StampsTransactionID).ToList();
            PersonAdapter person = new PersonAdapter(stampsAccountEntity, string.Empty);

            string scanFormStampsId = string.Empty;
            string scanFormUrl = string.Empty;

            using (SwsimV29 webService = CreateWebService("ScanForm"))
            {
                webService.CreateScanForm
                    (
                        GetAuthenticator(stampsAccountEntity),
                        stampsTransactions.ToArray(),
                        CreateScanFormAddress(person),
                        ImageType.Png,
                        false, // Don't print instructions
                        null,
                        false,
                        out scanFormStampsId,
                        out scanFormUrl
                    );
            }

            XDocument response = XDocument.Parse("<ScanForm/>");
            response.Root.Add(scanFormStampsId.Split(new[] { ' ' }).Select(x => new XElement("TransactionId", x)));
            response.Root.Add(scanFormUrl.Split(new[] { ' ' }).Select(x => new XElement("Url", x)));
            return response;
        }

        /// <summary>
        /// Void the given already processed shipment
        /// </summary>
        public void VoidShipment(ShipmentEntity shipment)
        {
            StampsAccountEntity account = accountRepository.GetAccount(shipment.Postal.Stamps.StampsAccountID);
            if (account == null)
            {
                throw new StampsException("No Stamps.com account is selected for the shipment.");
            }

            AuthenticationWrapper(() => { VoidShipmentInternal(shipment, account); return true; }, account);
        }

        /// <summary>
        /// The internal VoidShipment implementation intended to be wrapped by the auth wrapper
        /// </summary>
        private void VoidShipmentInternal(ShipmentEntity shipment, StampsAccountEntity account)
        {
            using (SwsimV29 webService = CreateWebService("Void"))
            {
                string auth = webService.CancelIndicium(GetAuthenticator(account), shipment.Postal.Stamps.StampsTransactionID);
                usernameAuthenticatorMap[account.Username] = auth;
            }
        }

        /// <summary>
        /// Process the given shipment, downloading label images and tracking information
        /// </summary>
        public void ProcessShipment(ShipmentEntity shipment)
        {
            StampsAccountEntity account = accountRepository.GetAccount(shipment.Postal.Stamps.StampsAccountID);
            if (account == null)
            {
                throw new StampsException("No Stamps.com account is selected for the shipment.");
            }

            try
            {
                AuthenticationWrapper(() => { ProcessShipmentInternal(shipment, account); return true; }, account);
            }
            catch (StampsApiException ex)
            {
                if (ex.Message.ToUpperInvariant().Contains("THE USERNAME OR PASSWORD ENTERED IS NOT CORRECT"))
                {
                    // Provide a little more context as to which user name/password was incorrect in the case
                    // where there's multiple accounts or Express1 for Stamps is being used to compare rates
                    string message = string.Format("ShipWorks was unable to connect to {0} with account {1}.{2}Check that your account credentials are correct.",
                                    StampsAccountManager.GetResellerName((StampsResellerType.Express1)),
                                    account.Username,
                                    Environment.NewLine);

                    throw new StampsException(message, ex);
                }

                if (ex.Code == 5636353 || ex.Message.ToUpperInvariant().Contains("INSUFFICIENT FUNDS"))
                {
                    throw new StampsInsufficientFundsException(account, ex.Message);
                }

                // This isn't an exception we can handle, so just throw the original exception
                throw;
            }
        }

        /// <summary>
        /// The internal ProcessShipment implementation intended to be wrapped by the auth wrapper
        /// </summary>
        private void ProcessShipmentInternal(ShipmentEntity shipment, StampsAccountEntity account)
        {
            Guid stampsGuid;
            string tracking = string.Empty;
            string labelUrl;

            Address fromAddress = CleanseAddress(account, new PersonAdapter(shipment, "Origin"), false);
            Address toAddress = CleanseAddress(account, new PersonAdapter(shipment, "Ship"), shipment.Postal.Stamps.RequireFullAddressValidation);

            // If this is a return shipment, swap the to/from addresses
            if (shipment.ReturnShipment)
            {
                Address tmpAddress = toAddress;
                toAddress = fromAddress;
                fromAddress = tmpAddress;
            }

            if (shipment.ReturnShipment && !(PostalUtility.IsDomesticCountry(toAddress.Country) && PostalUtility.IsDomesticCountry(fromAddress.Country)))
            {
                throw new StampsException("Return shipping labels can only be used to send packages to and from domestic addresses.");
            }

            RateV11 rate = CreateRateForProcessing(shipment, account);
            CustomsV2 customs = CreateCustoms(shipment);
            WebServices.v29.PostageBalance postageBalance;
            string memo = StringUtility.Truncate(TemplateTokenProcessor.ProcessTokens(shipment.Postal.Stamps.Memo, shipment.ShipmentID), 200);

            // Stamps requires that the address in the Rate match that of the request.  Makes sense - but could be different if they auto-cleansed the address.
            rate.ToState = toAddress.State;
            rate.ToZIPCode = toAddress.ZIPCode;

            ThermalLanguage? thermalType;

            // Determine what thermal type, if any to use.  
            // If USPS, use it's setting.  
            // Otherwise, use the Stamps settings if it is a Stamps shipment being auto-switched to an Express1 shipment
            if (shipment.ShipmentType == (int) ShipmentTypeCode.Usps)
            {
                thermalType = shipment.RequestedLabelFormat == (int)ThermalLanguage.None ? null : (ThermalLanguage?)shipment.RequestedLabelFormat;
            }
            else if (shipment.ShipmentType == (int)ShipmentTypeCode.Stamps || shipment.Postal.Stamps.OriginalStampsAccountID != null)
            {
                thermalType = shipment.RequestedLabelFormat == (int)ThermalLanguage.None ? null : (ThermalLanguage?)shipment.RequestedLabelFormat;
            }
            else if (shipment.ShipmentType == (int)ShipmentTypeCode.Express1Stamps)
            {
                thermalType = shipment.RequestedLabelFormat == (int)ThermalLanguage.None ? null : (ThermalLanguage?)shipment.RequestedLabelFormat;
            }
            else
            {
                throw new InvalidOperationException("Unknown Stamps.com shipment type.");
            }

            // For international thermal labels, we need to set the print layout or else most service/package type combinations
            // will fail with a "does not support Zebra printers" error
            if (thermalType.HasValue &&
                !PostalUtility.IsDomesticCountry(shipment.ShipCountryCode) &&
                !PostalUtility.IsMilitaryState(shipment.ShipStateProvCode))
            {
                rate.PrintLayout = "Normal4X6CN22";
            }

            // Each request needs to get a new requestID.  If Stamps.com sees a duplicate, it thinks its the same request.  
            // So if you had an error (like weight was too much) and then changed the weight and resubmitted, it would still 
            // be in error if you used the same ID again.
            shipment.Postal.Stamps.IntegratorTransactionID = Guid.NewGuid();
            string integratorGuid = shipment.Postal.Stamps.IntegratorTransactionID.ToString();

            string mac_Unused;
            string postageHash;
            byte[][] imageData;

            // If we're using Express1, we don't want to use the SampleOnly flag since this will not
            // create shipments and cause subsequent calls (like SCAN form creation) to fail
            bool isSampleOnly = UseTestServer && account.StampsReseller != (int)StampsResellerType.Express1;

            if (shipment.Postal.PackagingType == (int)PostalPackagingType.Envelope && shipment.Postal.Service != (int)PostalServiceType.InternationalFirst)
            {
                // Envelopes don't support thermal
                thermalType = null;

                // A separate service call is used for processing envelope according to Stamps.com as of v. 22
                using (SwsimV29 webService = CreateWebService("Process"))
                {
                    // Always use the personal envelope layout to generate the envelope label
                    rate.PrintLayout = "EnvelopePersonal";

                    string envelopeAuth = webService.CreateEnvelopeIndicium(GetAuthenticator(account), ref integratorGuid,
                        ref rate,
                        fromAddress,
                        toAddress,
                        null,
                        isSampleOnly ? CreateIndiciumModeV1.Sample : CreateIndiciumModeV1.Normal,
                        ImageType.Png,
                        0, // cost code ID
                        false, // do not hide the facing identification mark (FIM) 
                        out tracking,
                        out stampsGuid,
                        out labelUrl,
                        out postageBalance,
                        out mac_Unused,
                        out postageHash);

                    usernameAuthenticatorMap[account.Username] = envelopeAuth;
                }
            }
            else
            {
                // Labels for all other package types other than envelope get created via the CreateIndicium method
                using (SwsimV29 webService = CreateWebService("Process"))
                {
                    string auth = webService.CreateIndicium(GetAuthenticator(account), ref integratorGuid,
                        ref tracking,
                        ref rate,
                        fromAddress,
                        toAddress,
                        null,
                        customs,
                        isSampleOnly,
                        thermalType == null ? ImageType.Png : ((thermalType == ThermalLanguage.EPL) ? ImageType.Epl : ImageType.Zpl),
                        EltronPrinterDPIType.Default,
                        memo, // Memo
                        0, // Cost Code
                        "", // recipient email
                        false, // delivery notify
                        false, // shipment notify
                        false, // shipment notify cc to main
                        false, // shipment notify from company
                        false, // shipment notify company in subject
                        0, // Rotation
                        null, false, // horizontal offset
                        null, false, // vertical offset
                        null, false, // print density
                        null, false, // print memo 
                        null, false, // print instructions
                        false, // request postage hash
                        NonDeliveryOption.Return, // return to sender
                        null, // redirectTo
                        null, // OriginalPostageHash 
                        null, false, // returnImageData
                        out stampsGuid,
                        out labelUrl,
                        out postageBalance,
                        out mac_Unused,
                        out postageHash,
                        out imageData);

                    usernameAuthenticatorMap[account.Username] = auth;
                }
            }

            shipment.TrackingNumber = tracking;
            shipment.ShipmentCost = rate.Amount + (rate.AddOns != null ? rate.AddOns.Sum(a => a.Amount) : 0);
            shipment.Postal.Stamps.StampsTransactionID = stampsGuid;

            // Set the thermal type for the shipment
            shipment.ActualLabelFormat = (int?)thermalType;

            // Interapptive users have an unprocess button.  If we are reprocessing we need to clear the old images
            ObjectReferenceManager.ClearReferences(shipment.ShipmentID);

            string[] labelUrls = labelUrl.Split(' ');
            SaveLabels(shipment, labelUrls);
        }

        /// <summary>
        /// Uses the label URLs to saves the label(s) for the given shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="labelUrls">The URLs that labels need to be requested from.</param>
        private void SaveLabels(ShipmentEntity shipment, IEnumerable<string> labelUrls)
        {
            List<Label> labels = new List<Label>();
            try
            {
                labels = new LabelFactory().CreateLabels(shipment, labelUrls.ToList()).ToList();
                labels.ForEach(l => l.Save());
            }
            finally
            {
                labels.ForEach(l => l.Dispose());
            }
        }
        
        /// <summary>
        /// Creates a scan form address (which is entirely different that the address object the rest of the API uses).
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns>A from_address_v1 object.</returns>
        private static Address CreateScanFormAddress(PersonAdapter person)
        {
            Address fromAddress = new Address();

            PersonName originName = new PersonName(person);
            fromAddress.FullName = originName.FullName;
            fromAddress.Company = person.Company;

            fromAddress.Address1 = person.Street1;
            fromAddress.Address2 = person.Street2;

            fromAddress.City = person.City;

            fromAddress.State = person.StateProvCode;
            fromAddress.ZIPCode = person.PostalCode5;
            fromAddress.ZIPCodeAddOn = person.PostalCode4;

            fromAddress.PhoneNumber = person.Phone;

            return fromAddress;
        }

        /// <summary>
        /// Create a Stamps.com Address API object based on the given person address
        /// </summary>
        private static Address CreateAddress(PersonAdapter person)
        {
            Address address = new Address();

            PersonName shippingName = new PersonName(person);

            address.FullName = shippingName.FullName;
            address.Company = person.Company;

            address.Address1 = person.Street1;
            address.Address2 = person.Street2;
            address.Address3 = person.Street3;

            address.City = person.City;

            if (PostalUtility.IsDomesticCountry(person.CountryCode))
            {
                address.State = PostalUtility.AdjustState(person.CountryCode, person.StateProvCode);
                address.ZIPCode = person.PostalCode5;
                address.ZIPCodeAddOn = person.PostalCode4;
            }
            else
            {
                address.Province = person.StateProvCode;
                address.PostalCode = person.PostalCode;
            }

            address.Country = CountryCodeCleanser.CleanseCountryCode(person.CountryCode);

            if (person.CountryCode == "US")
            {
                address.PhoneNumber = person.Phone10Digits;
            }
            else
            {
                address.PhoneNumber = person.Phone;
            }

            return address;
        }

        /// <summary>
        /// Create a Rate object used as the rate info for the GetRates method
        /// </summary>
        private static RateV11 CreateRateForRating(ShipmentEntity shipment, StampsAccountEntity account)
        {
            RateV11 rate = new RateV11();

            string fromZipCode = string.Empty;
            string toZipCode = string.Empty;
            string toCountry = string.Empty;

            fromZipCode = !string.IsNullOrEmpty(account.MailingPostalCode) ? account.MailingPostalCode : shipment.OriginPostalCode;
            toZipCode = shipment.ShipPostalCode;
            toCountry = CountryCodeCleanser.CleanseCountryCode(shipment.ShipCountryCode);

            // Swap the to/from for return shipments.
            if (shipment.ReturnShipment)
            {
                rate.FromZIPCode = toZipCode;
                rate.ToZIPCode = fromZipCode;
                rate.ToCountry = CountryCodeCleanser.CleanseCountryCode(shipment.OriginCountryCode);
            }
            else
            {
                rate.FromZIPCode = fromZipCode;
                rate.ToZIPCode = toZipCode;
                rate.ToCountry = toCountry;
            }

            // Default the weight to 14oz for best rate if it is 0, so we can get a rate without needing the user to provide a value.  We do 14oz so it kicks it into a Priority shipment, which
            // is the category that most of our users will be in.
            double defaultPounds = BestRateScope.IsActive ? 0.88 : .1;
            WeightValue weightValue = new WeightValue(shipment.TotalWeight > 0 ? shipment.TotalWeight : defaultPounds);
            rate.WeightLb = weightValue.PoundsOnly;
            rate.WeightOz = weightValue.OuncesOnly;

            WebServices.PackageTypeV6 packageTypeV6 = StampsUtility.GetApiPackageType((PostalPackagingType)shipment.Postal.PackagingType, new DimensionsAdapter(shipment.Postal));
            rate.PackageType = ConvertPackageType(packageTypeV6);
            rate.NonMachinable = shipment.Postal.NonMachinable;

            rate.Length = shipment.Postal.DimsLength;
            rate.Width = shipment.Postal.DimsWidth;
            rate.Height = shipment.Postal.DimsHeight;

            rate.ShipDate = shipment.ShipDate;
            rate.DeclaredValue = shipment.CustomsValue;

            return rate;
        }

        /// <summary>
        /// Create the rate object for the given shipment
        /// </summary>
        private static RateV11 CreateRateForProcessing(ShipmentEntity shipment, StampsAccountEntity account)
        {
            PostalServiceType serviceType = (PostalServiceType)shipment.Postal.Service;
            PostalPackagingType packagingType = (PostalPackagingType)shipment.Postal.PackagingType;

            RateV11 rate = CreateRateForRating(shipment, account);
            rate.ServiceType = ConvertServiceType(StampsUtility.GetApiServiceType(serviceType));
            rate.PrintLayout = "Normal";

            List<AddOnV4> addOns = new List<AddOnV4>();

            // For domestic, add in Delivery\Signature confirmation
            if (PostalUtility.IsDomesticCountry(shipment.ShipCountryCode))
            {
                PostalConfirmationType confirmation = (PostalConfirmationType)shipment.Postal.Confirmation;

                // If the service type is Parcel Select, Force DC, otherwise stamps throws an error
                if (confirmation == PostalConfirmationType.Delivery)
                {
                    addOns.Add(new AddOnV4 { AddOnType = AddOnTypeV4.USADC });
                }

                if (confirmation == PostalConfirmationType.Signature)
                {
                    addOns.Add(new AddOnV4 { AddOnType = AddOnTypeV4.USASC });
                }
            }

            // Check for the new (as of 01/27/13) international delivery service.  In that case, we have to explicitly turn on DC
            else if (PostalUtility.IsFreeInternationalDeliveryConfirmation(shipment.ShipCountryCode, serviceType, packagingType))
            {
                addOns.Add(new AddOnV4 { AddOnType = AddOnTypeV4.USADC });
            }

            // For express, apply the signature waiver if necessary
            if (serviceType == PostalServiceType.ExpressMail)
            {
                if (!shipment.Postal.ExpressSignatureWaiver)
                {
                    addOns.Add(new AddOnV4 { AddOnType = AddOnTypeV4.USASR });
                }
            }

            // Add in the hidden postage option (but not supported for envelopes)
            if (shipment.Postal.Stamps.HidePostage && shipment.Postal.PackagingType != (int)PostalPackagingType.Envelope)
            {
                addOns.Add(new AddOnV4 { AddOnType = AddOnTypeV4.SCAHP });
            }

            if (addOns.Count > 0)
            {
                rate.AddOns = addOns.ToArray();
            }

            // For APO/FPO, we have to specifically ask for customs docs
            if (PostalUtility.IsMilitaryState(shipment.ShipStateProvCode) || ShipmentTypeManager.GetType(shipment).IsCustomsRequired(shipment))
            {
                rate.PrintLayout = (PostalUtility.GetCustomsForm(shipment) == PostalCustomsForm.CN72) ? "NormalCP72" : "NormalCN22";
            }

            if (shipment.ReturnShipment)
            {
                // Swapping out Normal with Return indicates a return label
                rate.PrintLayout = rate.PrintLayout.Replace("Normal", "Return");
            }

            return rate;
        }

        /// <summary>
        /// Create the customs information for the given shipment
        /// </summary>
        private static CustomsV2 CreateCustoms(ShipmentEntity shipment)
        {
            if (!CustomsManager.IsCustomsRequired(shipment))
            {
                return null;
            }

            CustomsV2 customs = new CustomsV2();

            // Content type
            customs.ContentType = ConvertContentType(StampsUtility.GetApiContentType((PostalCustomsContentType)shipment.Postal.CustomsContentType));
            if (customs.ContentType == ContentTypeV2.Other)
            {
                if (shipment.Postal.CustomsContentType == (int)PostalCustomsContentType.Merchandise)
                {
                    customs.OtherDescribe = "Merchandise";
                }
                else
                {
                    customs.OtherDescribe = shipment.Postal.CustomsContentDescription.Truncate(MaxCustomsContentDescriptionLength);
                }
            }

            List<CustomsLine> lines = new List<CustomsLine>();

            // Go through each of the customs contents to create  the stamps.com custom line entity
            foreach (ShipmentCustomsItemEntity customsItem in shipment.CustomsItems)
            {
                WeightValue weightValue = new WeightValue(customsItem.Weight);

                CustomsLine line = new CustomsLine();
                line.Description = customsItem.Description.Truncate(MaxCustomsItemDescriptionLength);
                line.Quantity = customsItem.Quantity;
                line.Value = customsItem.UnitValue;

                line.WeightLb = weightValue.PoundsOnly;
                line.WeightOz = weightValue.OuncesOnly;

                line.HSTariffNumber = customsItem.HarmonizedCode;
                line.CountryOfOrigin = customsItem.CountryOfOrigin;

                lines.Add(line);
            }

            customs.CustomsLines = lines.ToArray();

            return customs;
        }

        /// <summary>
        /// Wraps the given executor in methods that ensure the appropriate authentication for the account
        /// </summary>
        private T AuthenticationWrapper<T>(Func<T> executor, StampsAccountEntity account)
        {
            object authenticationLock;

            lock (authenticationLockMap)
            {
                if (!authenticationLockMap.TryGetValue(account.Username, out authenticationLock))
                {
                    authenticationLock = new object();
                    authenticationLockMap[account.Username] = authenticationLock;
                }
            }

            // We have to lockout authentication of this account to make sure only one thread is trying to authenticate at a time,
            // otherwise there will be race conditions try to get the latest authenticator.
            lock (authenticationLock)
            {
                int triesLeft = 5;

                while (true)
                {
                    triesLeft--;

                    try
                    {
                        return executor();
                    }
                    catch (SoapException ex)
                    {
                        log.ErrorFormat("Failed connecting to Stamps.com: {0}, {1}", StampsApiException.GetErrorCode(ex), ex.Message);

                        if (triesLeft > 0 && IsStaleAuthenticator(ex))
                        {
                            AuthenticateUser(account.Username, SecureText.Decrypt(account.Password, account.Username));
                        }
                        else
                        {
                            throw new StampsApiException(ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw WebHelper.TranslateWebException(ex, typeof(StampsException));
                    }
                }
            }
        }

        /// <summary>
        /// Get the authenticator for the given account
        /// </summary>
        private string GetAuthenticator(StampsAccountEntity account)
        {
            string auth;
            if (!usernameAuthenticatorMap.TryGetValue(account.Username, out auth))
            {
                AuthenticateUser(account.Username, SecureText.Decrypt(account.Password, account.Username));

                auth = usernameAuthenticatorMap[account.Username];
            }

            return auth;
        }

        /// <summary>
        /// Indicates if the exception represents an authenticator that has gone stale
        /// </summary>
        private static bool IsStaleAuthenticator(SoapException ex)
        {
            // Express1 does not return error codes...
            switch (ex.Message)
            {
                case "Invalid authentication info":
                case "Unable to authenticate user.":
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Converts a current ServiceType to a v29 ServiceType
        /// </summary>
        private static WebServices.v29.ServiceType ConvertServiceType(WebServices.ServiceType serviceType)
        {
            switch (serviceType)
            {
                case WebServices.ServiceType.USFC:
                    return WebServices.v29.ServiceType.USFC;
                case WebServices.ServiceType.USPM:
                    return WebServices.v29.ServiceType.USPM;
                case WebServices.ServiceType.USXM:
                    return WebServices.v29.ServiceType.USXM;
                case WebServices.ServiceType.USMM:
                    return WebServices.v29.ServiceType.USMM;
                case WebServices.ServiceType.USBP:
                    return WebServices.v29.ServiceType.USBP;
                case WebServices.ServiceType.USLM:
                    return WebServices.v29.ServiceType.USLM;
                case WebServices.ServiceType.USEMI:
                    return WebServices.v29.ServiceType.USEMI;
                case WebServices.ServiceType.USPMI:
                    return WebServices.v29.ServiceType.USPMI;
                case WebServices.ServiceType.USFCI:
                    return WebServices.v29.ServiceType.USFCI;
                case WebServices.ServiceType.USCM:
                    return WebServices.v29.ServiceType.USCM;
                case WebServices.ServiceType.USPS:
                    return WebServices.v29.ServiceType.USPS;

                case WebServices.ServiceType.DHLPE:
                case WebServices.ServiceType.DHLPG:
                case WebServices.ServiceType.DHLPPE:
                case WebServices.ServiceType.DHLPPG:
                case WebServices.ServiceType.DHLBPME:
                case WebServices.ServiceType.DHLBPMG:
                default:
                    throw new ArgumentOutOfRangeException("serviceType");
            }
        }

        /// <summary>
        /// Converts a v29 ServiceType to the ServiceType
        /// </summary>
        private static WebServices.ServiceType ConvertServiceType(WebServices.v29.ServiceType serviceType)
        {
            switch (serviceType)
            {
                case WebServices.v29.ServiceType.USFC:
                    return WebServices.ServiceType.USFC;
                case WebServices.v29.ServiceType.USPM:
                    return WebServices.ServiceType.USPM;
                case WebServices.v29.ServiceType.USXM:
                    return WebServices.ServiceType.USXM;
                case WebServices.v29.ServiceType.USMM:
                    return WebServices.ServiceType.USMM;
                case WebServices.v29.ServiceType.USBP:
                    return WebServices.ServiceType.USBP;
                case WebServices.v29.ServiceType.USLM:
                    return WebServices.ServiceType.USLM;
                case WebServices.v29.ServiceType.USEMI:
                    return WebServices.ServiceType.USEMI;
                case WebServices.v29.ServiceType.USPMI:
                    return WebServices.ServiceType.USPMI;
                case WebServices.v29.ServiceType.USFCI:
                    return WebServices.ServiceType.USFCI;
                case WebServices.v29.ServiceType.USCM:
                    return WebServices.ServiceType.USCM;
                case WebServices.v29.ServiceType.USPS:
                    return WebServices.ServiceType.USPS;
                default:
                    throw new ArgumentOutOfRangeException("serviceType");
            }
        }
        
        /// <summary>
        /// Gets the v29 version of the CodewordType
        /// </summary>
        private static WebServices.v29.PackageTypeV6 ConvertPackageType(WebServices.PackageTypeV6 packageType)
        {
            switch (packageType)
            {
                case PackageTypeV6.Postcard:
                    return WebServices.v29.PackageTypeV6.Postcard;
                case PackageTypeV6.Letter:
                    return WebServices.v29.PackageTypeV6.Letter;
                case PackageTypeV6.LargeEnvelopeorFlat:
                    return WebServices.v29.PackageTypeV6.LargeEnvelopeorFlat;
                case PackageTypeV6.ThickEnvelope:
                    return WebServices.v29.PackageTypeV6.ThickEnvelope;
                case PackageTypeV6.Package:
                    return WebServices.v29.PackageTypeV6.Package;
                case PackageTypeV6.FlatRateBox:
                    return WebServices.v29.PackageTypeV6.FlatRateBox;
                case PackageTypeV6.SmallFlatRateBox:
                    return WebServices.v29.PackageTypeV6.SmallFlatRateBox;
                case PackageTypeV6.LargeFlatRateBox:
                    return WebServices.v29.PackageTypeV6.LargeFlatRateBox;
                case PackageTypeV6.FlatRateEnvelope:
                    return WebServices.v29.PackageTypeV6.FlatRateEnvelope;
                case PackageTypeV6.FlatRatePaddedEnvelope:
                    return WebServices.v29.PackageTypeV6.FlatRatePaddedEnvelope;
                case PackageTypeV6.LargePackage:
                    return WebServices.v29.PackageTypeV6.LargePackage;
                case PackageTypeV6.OversizedPackage:
                    return WebServices.v29.PackageTypeV6.OversizedPackage;
                case PackageTypeV6.RegionalRateBoxA:
                    return WebServices.v29.PackageTypeV6.RegionalRateBoxA;
                case PackageTypeV6.RegionalRateBoxB:
                    return WebServices.v29.PackageTypeV6.RegionalRateBoxB;
                case PackageTypeV6.LegalFlatRateEnvelope:
                    return WebServices.v29.PackageTypeV6.LegalFlatRateEnvelope;
                case PackageTypeV6.RegionalRateBoxC:
                    return WebServices.v29.PackageTypeV6.RegionalRateBoxC;
                default:
                    throw new ArgumentOutOfRangeException("packageType");
            }
        }

        /// <summary>
        /// Gets the v29 version of the ContentType
        /// </summary>
        private static WebServices.v29.ContentTypeV2 ConvertContentType(WebServices.ContentTypeV2 contentType)
        {
            switch (contentType)
            {
                case WebServices.ContentTypeV2.CommercialSample:
                    return WebServices.v29.ContentTypeV2.CommercialSample;
                case WebServices.ContentTypeV2.Gift:
                    return WebServices.v29.ContentTypeV2.Gift;
                case WebServices.ContentTypeV2.Document:
                    return WebServices.v29.ContentTypeV2.Document;
                case WebServices.ContentTypeV2.ReturnedGoods:
                    return WebServices.v29.ContentTypeV2.ReturnedGoods;
                case WebServices.ContentTypeV2.Other:
                    return WebServices.v29.ContentTypeV2.Other;
                case WebServices.ContentTypeV2.Merchandise:
                    return WebServices.v29.ContentTypeV2.Merchandise;
                case WebServices.ContentTypeV2.HumanitarianDonation:
                    return WebServices.v29.ContentTypeV2.HumanitarianDonation;
                case WebServices.ContentTypeV2.DangerousGoods:
                    return WebServices.v29.ContentTypeV2.DangerousGoods;
                default:
                    throw new ArgumentOutOfRangeException("contentType");
            }
        }
    }
}
