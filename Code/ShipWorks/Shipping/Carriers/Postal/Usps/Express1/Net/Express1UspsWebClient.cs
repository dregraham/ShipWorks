using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Labels;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.v36;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Order;
using ShipWorks.Templates.Tokens;
using AccountInfo = ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.v36.AccountInfo;
using Address = ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.v36.Address;
using ContentTypeV2 = ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.v36.ContentTypeV2;
using CreateIndiciumModeV1 = ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.v36.CreateIndiciumModeV1;
using Credentials = ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.v36.Credentials;
using CustomsLine = ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.v36.CustomsLine;
using CustomsV2 = ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.v36.CustomsV2;
using EltronPrinterDPIType = ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.v36.EltronPrinterDPIType;
using ImageType = ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.v36.ImageType;
using NonDeliveryOption = ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.v36.NonDeliveryOption;
using PackageTypeV6 = ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.PackageTypeV6;
using PurchaseStatus = ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.v36.PurchaseStatus;
using ResidentialDeliveryIndicatorType = ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.v36.ResidentialDeliveryIndicatorType;
using ServiceType = ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.v36.ServiceType;
using StatusCodes = ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.v36.StatusCodes;
using UrlType = ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.v36.UrlType;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Net
{
    /// <summary>
    /// Central point where API stuff goes through for USPS
    /// </summary>
    public class Express1UspsWebClient : IUspsWebClient
    {
        // These lengths come from the error that USPS' API gives us when we send data that is too long
        private const int MaxCustomsContentDescriptionLength = 20;
        private const int MaxCustomsItemDescriptionLength = 60;

        private readonly ILog log;
        private readonly LogEntryFactory logEntryFactory;
        private readonly ICarrierAccountRepository<UspsAccountEntity> accountRepository;

        // Maps USPS usernames to their latest authenticator tokens
        static Dictionary<string, string> usernameAuthenticatorMap = new Dictionary<string, string>();

        // Maps USPS usernames to the object lock used to make sure only one thread is trying to authenticate at a time
        static Dictionary<string, object> authenticationLockMap = new Dictionary<string, object>();

        // Cleansed address map so we don't do common addresses over and over again
        static Dictionary<PersonAdapter, Address> cleansedAddressMap = new Dictionary<PersonAdapter, Address>();

        // Express1 API service connection info 
        static Express1UspsConnectionDetails express1UspsConnectionDetails = new Express1UspsConnectionDetails();

        private readonly ICertificateInspector certificateInspector;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Express1UspsWebClient"/> class.
        /// </summary>
        public Express1UspsWebClient()
            : this(new Express1UspsAccountRepository(), new LogEntryFactory(), new TrustingCertificateInspector())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Express1UspsWebClient" /> class.
        /// </summary>
        public Express1UspsWebClient(ICarrierAccountRepository<UspsAccountEntity> accountRepository, LogEntryFactory logEntryFactory, ICertificateInspector certificateInspector)
        {
            this.accountRepository = accountRepository;
            this.logEntryFactory = logEntryFactory;
            this.log = LogManager.GetLogger(typeof(Express1UspsWebClient));
            this.certificateInspector = certificateInspector;
        }

        /// <summary>
        /// Indicates if the test server should be used instead of the live server
        /// </summary>
        public static bool UseTestServer
        {
            get { return InterapptiveOnly.Registry.GetValue("UspsTestServer", false); }
            set { InterapptiveOnly.Registry.SetValue("UspsTestServer", value); }
        }

        /// <summary>
        /// Create the web service instance with the appropriate URL
        /// </summary>
        private SwsimV36 CreateWebService(string logName)
        {
            return CreateWebService(logName, LogActionType.Other);
        }

        /// <summary>
        /// Create the web service instance with the appropriate URL
        /// </summary>
        private SwsimV36 CreateWebService(string logName, LogActionType logActionType)
        {
            SwsimV36 webService = new Express1UspsServiceWrapper(logEntryFactory.GetLogEntry(ApiLogSource.UspsExpress1, logName, logActionType))
            {
                Url = express1UspsConnectionDetails.ServiceUrl
            };

            return webService;
        }

        /// <summary>
        /// Authenticate the given user with USPS
        /// </summary>
        public void AuthenticateUser(string username, string password)
        {
            try
            {
                // Output parameters from USPS
                DateTime lastLoginTime = new DateTime();
                bool clearCredential = false;

                string bannerText = string.Empty;
                bool passwordExpired = false;

                using (SwsimV36 webService = CreateWebService("Authenticate"))
                {
                    CheckCertificate(webService.Url);

                    string auth = webService.AuthenticateUser(new Credentials
                    {
                        IntegrationID = new Guid(express1UspsConnectionDetails.ApiKey),
                        Username = username,
                        Password = password
                    }, out lastLoginTime, out clearCredential, out bannerText, out passwordExpired);

                    usernameAuthenticatorMap[username] = auth;
                }
            }
            catch (SoapException ex)
            {
                throw new UspsApiException(ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(UspsException));
            }
        }

        /// <summary>
        /// Changes the contract associated with the given account based on the contract type provided.
        /// </summary>
        public void ChangeToExpeditedPlan(UspsAccountEntity account, string promoCode)
        {
            throw new InvalidOperationException("An Express1 contract cannot be changed.");
        }

        /// <summary>
        /// Checks with USPS API to get the contract type of the account.
        /// </summary>
        public UspsAccountContractType GetContractType(UspsAccountEntity account)
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
            if (Express1UspsConnectionDetails.UseTestServer && certificateSecurityLevel == CertificateSecurityLevel.None)
            {
                return;
            }

            if (certificateSecurityLevel != CertificateSecurityLevel.Trusted)
            {
                string description = EnumHelper.GetDescription(ShipmentTypeCode.Express1Usps);
                throw new UspsException(string.Format("ShipWorks is unable to make a secure connection to {0}.", description));
            }
        }

        /// <summary>
        /// Get the account info for the given USPS user name
        /// </summary>
        public object GetAccountInfo(UspsAccountEntity account)
        {
            return AuthenticationWrapper(() => GetAccountInfoInternal(account), account);
        }

        /// <summary>
        /// The internal GetAccountInfo implementation that is intended to be wrapped by the auth wrapper
        /// </summary>
        private AccountInfo GetAccountInfoInternal(UspsAccountEntity account)
        {
            AccountInfo accountInfo;

            using (SwsimV36 webService = CreateWebService("GetAccountInfo"))
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
        /// The internal GetUrl implementation that is intended to be wrapped by the auth wrapper
        /// </summary>
        private string GetUrlInternal(UspsAccountEntity account, UrlType urlType)
        {
            string url;

            using (SwsimV36 webService = CreateWebService("GetURL"))
            {
                string auth = webService.GetURL(GetAuthenticator(account), urlType, string.Empty, out url);
                usernameAuthenticatorMap[account.Username] = auth;
            }

            return url;
        }

        /// <summary>
        /// Purchase postage for the given account for the specified amount.  ControlTotal is the ControlTotal value last retrieved from GetAccountInfo.
        /// </summary>
        public void PurchasePostage(UspsAccountEntity account, decimal amount, decimal controlTotal)
        {
            AuthenticationWrapper(() => { PurchasePostageInternal(account, amount, controlTotal); return true; }, account);
        }

        /// <summary>
        /// The internal PurchasePostageInternal implementation intended to be wrapped by the auth wrapper
        /// </summary>
        private void PurchasePostageInternal(UspsAccountEntity account, decimal amount, decimal controlTotal)
        {
            PurchaseStatus purchaseStatus;
            int transactionID;
            Usps.WebServices.v36.PostageBalance postageBalance;
            string rejectionReason;

            bool miRequired_Unused;

            using (SwsimV36 webService = CreateWebService("PurchasePostage"))
            {
                string auth = webService.PurchasePostage(GetAuthenticator(account), amount, controlTotal, null, null, out purchaseStatus, out transactionID, out postageBalance, out rejectionReason, out miRequired_Unused);
                usernameAuthenticatorMap[account.Username] = auth;
            }

            if (purchaseStatus == PurchaseStatus.Rejected)
            {
                throw new UspsException(rejectionReason);
            }
        }

        /// <summary>
        /// Get the rates for the given shipment based on its settings
        /// </summary>
        public List<RateResult> GetRates(ShipmentEntity shipment)
        {
            UspsAccountEntity account = accountRepository.GetAccount(shipment.Postal.Usps.UspsAccountID);

            if (account == null)
            {
                throw new UspsException("No USPS account is selected for the shipment.");
            }

            if (shipment.ReturnShipment && !(shipment.OriginPerson.IsDomesticCountry() && shipment.ShipPerson.IsDomesticCountry()))
            {
                throw new UspsException("Return shipping labels can only be used to send packages to and from domestic addresses.");
            }

            try
            {
                List<RateResult> rates = new List<RateResult>();

                foreach (RateV14 uspsRate in AuthenticationWrapper(() => { return GetRatesInternal(shipment, account); }, account))
                {
                    PostalServiceType serviceType = UspsUtility.GetPostalServiceType(ConvertServiceType(uspsRate.ServiceType));

                    RateResult baseRate = null;

                    // If its a rate that has sig\deliv, then you can's select the core rate itself
                    if (uspsRate.AddOns.Any(a => a.AddOnType == AddOnTypeV6.USADC))
                    {
                        baseRate = new RateResult(
                            PostalUtility.GetPostalServiceTypeDescription(serviceType),
                            uspsRate.DeliverDays.Replace("Days", ""))
                        {
                            Tag = new PostalRateSelection(serviceType, PostalConfirmationType.None),
                            ProviderLogo = EnumHelper.GetImage(ShipmentTypeCode.Express1Usps)
                        };
                    }
                    else
                    {
                        baseRate = new RateResult(
                            PostalUtility.GetPostalServiceTypeDescription(serviceType),
                            uspsRate.DeliverDays.Replace("Days", ""),
                            uspsRate.Amount,
                            new PostalRateSelection(serviceType, PostalConfirmationType.None))
                        {
                            ProviderLogo = EnumHelper.GetImage(ShipmentTypeCode.Express1Usps)
                        };
                    }

                    PostalUtility.SetServiceDetails(baseRate, serviceType, uspsRate.DeliverDays);

                    rates.Add(baseRate);

                    // Add a rate for each add-on
                    foreach (AddOnV6 addOn in uspsRate.AddOns)
                    {
                        string name = null;
                        PostalConfirmationType confirmationType = PostalConfirmationType.None;

                        switch (addOn.AddOnType)
                        {
                            case AddOnTypeV6.USADC:
                                name = string.Format("       Delivery Confirmation ({0:c})", addOn.Amount);
                                confirmationType = PostalConfirmationType.Delivery;
                                break;

                            case AddOnTypeV6.USASC:
                                name = string.Format("       Signature Confirmation ({0:c})", addOn.Amount);
                                confirmationType = PostalConfirmationType.Signature;
                                break;

                            case AddOnTypeV6.USAASR:
                                name = string.Format("       Adult Signature Required ({0:c})", addOn.Amount);
                                confirmationType = PostalConfirmationType.AdultSignatureRequired;
                                break;

                            case AddOnTypeV6.USAASRD:
                                name = string.Format("       Adult Signature Restricted Delivery ({0:c})", addOn.Amount);
                                confirmationType = PostalConfirmationType.AdultSignatureRestricted;
                                break;
                        }

                        if (name != null)
                        {
                            RateResult addOnRate = new RateResult(
                                name,
                                string.Empty,
                                uspsRate.Amount + addOn.Amount,
                                new PostalRateSelection(serviceType, confirmationType));

                            PostalUtility.SetServiceDetails(addOnRate, serviceType, uspsRate.DeliverDays);

                            rates.Add(addOnRate);
                        }
                    }
                }

                return rates;
            }
            catch (UspsApiException ex)
            {
                if (ex.Message.ToUpperInvariant().Contains("THE USERNAME OR PASSWORD ENTERED IS NOT CORRECT"))
                {
                    // Provide a little more context as to which user name/password was incorrect in the case
                    // where there's multiple accounts or Express1 for USPS is being used to compare rates
                    string message = string.Format("ShipWorks was unable to connect to {0} with account {1}.{2}Check that your account credentials are correct.",
                                    UspsAccountManager.GetResellerName(UspsResellerType.Express1),
                                    account.Username,
                                    Environment.NewLine);

                    throw new UspsException(message, ex);
                }

                // This isn't an authentication exception, so just throw the original exception
                throw;
            }
        }

        /// <summary>
        /// The internal GetRates implementation intended to be wrapped by the auth wrapper
        /// </summary>
        private List<RateV14> GetRatesInternal(ShipmentEntity shipment, UspsAccountEntity account)
        {
            RateV14 rate = CreateRateForRating(shipment, account);

            List<RateV14> rateResults = new List<RateV14>();

            using (SwsimV36 webService = CreateWebService("GetRates", LogActionType.GetRates))
            {
                CheckCertificate(webService.Url);

                RateV14[] ratesArray;

                string auth = webService.GetRates(GetAuthenticator(account), rate, out ratesArray);
                usernameAuthenticatorMap[account.Username] = auth;

                rateResults = ratesArray.ToList();
            }

            List<RateV14> noConfirmationServiceRates = new List<RateV14>();

            // If its a "Flat" then FirstClass and Priority can't have a confirmation
            PostalPackagingType packagingType = (PostalPackagingType)shipment.Postal.PackagingType;
            if (packagingType == PostalPackagingType.Envelope || packagingType == PostalPackagingType.LargeEnvelope)
            {
                noConfirmationServiceRates.AddRange(rateResults.Where(r => r.ServiceType == ServiceType.USFC || r.ServiceType == ServiceType.USPM));
            }

            // Remove the Delivery and Signature add ons from all those that shouldn't support it
            foreach (RateV14 noConfirmationServiceRate in noConfirmationServiceRates)
            {
                if (noConfirmationServiceRate != null && noConfirmationServiceRate.AddOns != null)
                {
                    noConfirmationServiceRate.AddOns = noConfirmationServiceRate.AddOns.Where(a => a.AddOnType != AddOnTypeV6.USASC && a.AddOnType != AddOnTypeV6.USADC).ToArray();
                }
            }

            return rateResults;
        }

        /// <summary>
        /// Cleans the address of the given person using the specified USPS account
        /// </summary>
        private Address CleanseAddress(UspsAccountEntity account, PersonAdapter person, bool requireFullMatch)
        {
            return AuthenticationWrapper(() => { return CleanseAddressInternal(person, account, requireFullMatch); }, account);
        }

        /// <summary>
        /// Internal CleanseAddress implementation intended to be warpped by the auth wrapper
        /// </summary>
        private Address CleanseAddressInternal(PersonAdapter person, UspsAccountEntity account, bool requireFullMatch)
        {
            Address address;
            if (cleansedAddressMap.TryGetValue(person, out address))
            {
                return address;
            }

            address = CreateAddress(person);

            address.Address1 = AdjustAddressLineForLength(address.Address1);
            address.Address2 = AdjustAddressLineForLength(address.Address2);
            address.Address3 = AdjustAddressLineForLength(address.Address3);

            bool addressMatch;
            bool cityStateZipOK;
            ResidentialDeliveryIndicatorType residentialIndicator;
            bool? isPoBox;
            bool isPoBoxSpecified;
            Address[] candidates;
            StatusCodes statusCodes;

            using (SwsimV36 webService = CreateWebService("CleanseAddress"))
            {
                string auth = webService.CleanseAddress(GetAuthenticator(account), ref address, out addressMatch, out cityStateZipOK, out residentialIndicator, out isPoBox, out isPoBoxSpecified, out candidates, out statusCodes);
                usernameAuthenticatorMap[account.Username] = auth;

                if (!addressMatch)
                {
                    if (!cityStateZipOK)
                    {
                        throw new UspsException(string.Format("The address for '{0}' is not a valid address.", new PersonName(person).FullName));
                    }
                    else if (requireFullMatch)
                    {
                        throw new UspsException(string.Format("The city, state, and postal code for '{0}' is valid, but the full address is not.", new PersonName(person).FullName));
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
        /// <param name="uspsAccountEntity">The USPS account entity.</param>
        /// <returns>An XDocument having a ScanForm node as the root which contains a TransactionId and Url nodes to 
        /// identify results from USPS</returns>
        public XDocument CreateScanForm(IEnumerable<UspsShipmentEntity> shipments, UspsAccountEntity uspsAccountEntity)
        {
            if (uspsAccountEntity == null)
            {
                throw new UspsException("No USPS account is selected for the SCAN form.");
            }

            XDocument result = new XDocument();
            AuthenticationWrapper(() => { result = CreateScanFormInternal(shipments, uspsAccountEntity); return true; }, uspsAccountEntity);

            return result;
        }

        /// <summary>
        /// Creates the scan form via the USPS API and intended to be wrapped by the authentication wrapper.
        /// </summary>
        /// <param name="shipments">The shipments.</param>
        /// <param name="uspsAccountEntity">The USPS account entity.</param>
        /// <returns>An XDocument having a ScanForm node as the root which contains a TransactionId and Url nodes to 
        /// identify results from USPS</returns>
        private XDocument CreateScanFormInternal(IEnumerable<UspsShipmentEntity> shipments, UspsAccountEntity uspsAccountEntity)
        {
            List<Guid> uspsTransactions = shipments.Select(s => s.UspsTransactionID).ToList();
            PersonAdapter person = new PersonAdapter(uspsAccountEntity, string.Empty);

            string scanFormUspsId = string.Empty;
            string scanFormUrl = string.Empty;

            using (SwsimV36 webService = CreateWebService("ScanForm"))
            {
                webService.CreateScanForm
                    (
                        GetAuthenticator(uspsAccountEntity),
                        uspsTransactions.ToArray(),
                        CreateScanFormAddress(person),
                        ImageType.Png,
                        false, // Don't print instructions
                        null,
                        false,
                        out scanFormUspsId,
                        out scanFormUrl
                    );
            }

            XDocument response = XDocument.Parse("<ScanForm/>");
            response.Root.Add(scanFormUspsId.Split(new[] { ' ' }).Select(x => new XElement("TransactionId", x)));
            response.Root.Add(scanFormUrl.Split(new[] { ' ' }).Select(x => new XElement("Url", x)));
            return response;
        }

        /// <summary>
        /// Void the given already processed shipment
        /// </summary>
        public void VoidShipment(ShipmentEntity shipment)
        {
            UspsAccountEntity account = accountRepository.GetAccount(shipment.Postal.Usps.UspsAccountID);
            if (account == null)
            {
                throw new UspsException("No USPS account is selected for the shipment.");
            }

            AuthenticationWrapper(() => { VoidShipmentInternal(shipment, account); return true; }, account);
        }

        /// <summary>
        /// The internal VoidShipment implementation intended to be wrapped by the auth wrapper
        /// </summary>
        private void VoidShipmentInternal(ShipmentEntity shipment, UspsAccountEntity account)
        {
            using (SwsimV36 webService = CreateWebService("Void"))
            {
                string auth = webService.CancelIndicium(GetAuthenticator(account), shipment.Postal.Usps.UspsTransactionID);
                usernameAuthenticatorMap[account.Username] = auth;
            }
        }

        /// <summary>
        /// Process the given shipment, downloading label images and tracking information
        /// </summary>
        public void ProcessShipment(ShipmentEntity shipment)
        {
            UspsAccountEntity account = accountRepository.GetAccount(shipment.Postal.Usps.UspsAccountID);
            if (account == null)
            {
                throw new UspsException("No USPS account is selected for the shipment.");
            }

            try
            {
                AuthenticationWrapper(() => { ProcessShipmentInternal(shipment, account); return true; }, account);
            }
            catch (UspsApiException ex)
            {
                if (ex.Message.ToUpperInvariant().Contains("THE USERNAME OR PASSWORD ENTERED IS NOT CORRECT"))
                {
                    // Provide a little more context as to which user name/password was incorrect in the case
                    // where there's multiple accounts or Express1 for USPS is being used to compare rates
                    string message = string.Format("ShipWorks was unable to connect to {0} with account {1}.{2}Check that your account credentials are correct.",
                                    UspsAccountManager.GetResellerName((UspsResellerType.Express1)),
                                    account.Username,
                                    Environment.NewLine);

                    throw new UspsException(message, ex);
                }

                if (ex.Code == 5636353 || ex.Message.ToUpperInvariant().Contains("INSUFFICIENT FUNDS"))
                {
                    throw new UspsInsufficientFundsException(account, ex.Message);
                }

                // This isn't an exception we can handle, so just throw the original exception
                throw;
            }
        }

        /// <summary>
        /// Adjusts the length of the address line to be not more than 50 characters.
        /// </summary>
        private static string AdjustAddressLineForLength(string addressLine)
        {
            const int maxLength = 50;
            return addressLine.Length > maxLength ? addressLine.Substring(0, maxLength) : addressLine;
        }

        /// <summary>
        /// The internal ProcessShipment implementation intended to be wrapped by the auth wrapper
        /// </summary>
        private void ProcessShipmentInternal(ShipmentEntity shipment, UspsAccountEntity account)
        {
            Guid uspsGuid;
            string tracking = string.Empty;
            string labelUrl;

            Address fromAddress;
            Address toAddress;

            FixWebserviceAddresses(account, shipment, out toAddress, out fromAddress);

            RateV14 rate = CreateRateForProcessing(shipment, account);
            CustomsV2 customs = CreateCustoms(shipment);
            WebServices.v36.PostageBalance postageBalance;

            // USPS requires that the address in the Rate match that of the request.  Makes sense - but could be different if they auto-cleansed the address.
            rate.ToState = toAddress.State;
            rate.ToZIPCode = toAddress.ZIPCode;

            ThermalLanguage? thermalType;

            // Determine what thermal type, if any to use.  
            // If USPS, use it's setting.  
            // Otherwise, use the USPS settings if it is a USPS shipment being auto-switched to an Express1 shipment
            if (shipment.ShipmentType == (int) ShipmentTypeCode.Usps)
            {
                thermalType = shipment.RequestedLabelFormat == (int)ThermalLanguage.None ? null : (ThermalLanguage?)shipment.RequestedLabelFormat;
            }
            else if (shipment.ShipmentType == (int)ShipmentTypeCode.Express1Usps)
            {
                thermalType = shipment.RequestedLabelFormat == (int)ThermalLanguage.None ? null : (ThermalLanguage?)shipment.RequestedLabelFormat;
            }
            else
            {
                throw new InvalidOperationException("Unknown USPS shipment type.");
            }

            // For international thermal labels, we need to set the print layout or else most service/package type combinations
            // will fail with a "does not support Zebra printers" error
            if (thermalType.HasValue &&
                !shipment.ShipPerson.IsDomesticCountry() &&
                !PostalUtility.IsMilitaryState(shipment.ShipStateProvCode))
            {
                rate.PrintLayout = "Normal4X6CN22";
            }

            // Each request needs to get a new requestID.  If USPS sees a duplicate, it thinks its the same request.  
            // So if you had an error (like weight was too much) and then changed the weight and resubmitted, it would still 
            // be in error if you used the same ID again.
            shipment.Postal.Usps.IntegratorTransactionID = Guid.NewGuid();
            string integratorGuid = shipment.Postal.Usps.IntegratorTransactionID.ToString();

            string mac_Unused;
            string postageHash;
            byte[][] imageData = null;

            // If we're using Express1, we don't want to use the SampleOnly flag since this will not
            // create shipments and cause subsequent calls (like SCAN form creation) to fail
            bool isSampleOnly = UseTestServer && account.UspsReseller != (int)UspsResellerType.Express1;

            if (shipment.Postal.PackagingType == (int)PostalPackagingType.Envelope && shipment.Postal.Service != (int)PostalServiceType.InternationalFirst)
            {
                // Envelopes don't support thermal
                thermalType = null;

                // A separate service call is used for processing envelope according to USPS as of v. 22
                using (SwsimV36 webService = CreateWebService("Process"))
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
                        out uspsGuid,
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
                using (SwsimV36 webService = CreateWebService("Process"))
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
                        UspsUtility.BuildMemoField(shipment.Postal), // Memo
                        0, // Cost Code
                        false,
                        null,
                        0, // Rotation
                        null, false, // horizontal offset
                        null, false, // vertical offset
                        null, false, // print density
                        null, false, // print memo 
                        false, true, // print instructions
                        false, // request postage hash
                        NonDeliveryOption.Return, // return to sender
                        null, // redirectTo
                        null, // OriginalPostageHash 
                        true, true, // returnImageData
                        null,
                        PaperSizeV1.Default, 
                        null,
                        out uspsGuid,
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
            shipment.Postal.Usps.UspsTransactionID = uspsGuid;

            // Set the thermal type for the shipment
            shipment.ActualLabelFormat = (int?)thermalType;

            // Interapptive users have an unprocess button.  If we are reprocessing we need to clear the old images
            ObjectReferenceManager.ClearReferences(shipment.ShipmentID);

            SaveLabels(shipment, imageData, labelUrl);
        }

        /// <summary>
        /// Updates addresses based on shipment properties like ReturnShipment, etc
        /// </summary>
        private void FixWebserviceAddresses(UspsAccountEntity account, ShipmentEntity shipment, out Address toAddress, out Address fromAddress)
        {
            // If this is a return shipment, swap the to/from addresses
            if (shipment.ReturnShipment)
            {
                toAddress = CleanseAddress(account, shipment.OriginPerson, false);
                fromAddress = CreateAddress(shipment.ShipPerson);
            }
            else
            {
                fromAddress = CreateAddress(shipment.OriginPerson);
                toAddress = CleanseAddress(account, shipment.ShipPerson, shipment.Postal.Usps.RequireFullAddressValidation);
            }

            if (shipment.ReturnShipment && !(toAddress.AsAddressAdapter().IsDomesticCountry() && fromAddress.AsAddressAdapter().IsDomesticCountry()))
            {
                throw new UspsException("Return shipping labels can only be used to send packages to and from domestic addresses.");
            }
        }

        /// <summary>
        /// Uses the label URLs to saves the label(s) for the given shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="imageData">The base 64 binary data of each label image.</param>
        /// <param name="labelUrl">For envelopes, we need the labelUrl</param>
        private static void SaveLabels(ShipmentEntity shipment, byte[][] imageData, string labelUrl)
        {
            List<Label> labels = new List<Label>();

            try
            {
                LabelFactory labelFactory = new LabelFactory();

                if (imageData != null && imageData.Length > 0)
                {
                    labels.AddRange(labelFactory.CreateLabels(shipment, imageData).ToList());
                }

                if (!string.IsNullOrWhiteSpace(labelUrl))
                {
                    labels.Add(labelFactory.CreateLabel(shipment, labelUrl));
                }

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
        /// Create a USPS Address API object based on the given person address
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

            if (person.IsDomesticCountry())
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

            address.Country = person.AdjustedCountryCode(ShipmentTypeCode.Express1Usps);

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
        private static RateV14 CreateRateForRating(ShipmentEntity shipment, UspsAccountEntity account)
        {
            RateV14 rate = new RateV14();

            string fromZipCode = string.Empty;
            string toZipCode = string.Empty;
            string toCountry = string.Empty;

            fromZipCode = !string.IsNullOrEmpty(account.MailingPostalCode) ? account.MailingPostalCode : shipment.OriginPostalCode;
            toZipCode = shipment.ShipPostalCode;
            toCountry = shipment.AdjustedShipCountryCode();

            // Swap the to/from for return shipments.
            if (shipment.ReturnShipment)
            {
                rate.FromZIPCode = toZipCode;
                rate.ToZIPCode = fromZipCode;
                rate.ToCountry = shipment.AdjustedOriginCountryCode();
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

            Usps.WebServices.PackageTypeV6 packageTypeV6 = UspsUtility.GetApiPackageType((PostalPackagingType)shipment.Postal.PackagingType, new DimensionsAdapter(shipment.Postal));
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
        private static RateV14 CreateRateForProcessing(ShipmentEntity shipment, UspsAccountEntity account)
        {
            PostalServiceType serviceType = (PostalServiceType)shipment.Postal.Service;

            RateV14 rate = CreateRateForRating(shipment, account);
            rate.ServiceType = ConvertServiceType(UspsUtility.GetApiServiceType(serviceType));
            rate.PrintLayout = "Normal4x6";
            
            PostalShipmentType shipmentType = (PostalShipmentType)ShipmentTypeManager.GetType(shipment);

            List<AddOnV6> addOns = GetAddons(shipmentType, shipment);
            if (addOns.Count > 0)
            {
                rate.AddOns = addOns.ToArray();
            }

            // For APO/FPO, we have to specifically ask for customs docs
            if (PostalUtility.IsMilitaryState(shipment.ShipStateProvCode) || shipmentType.IsCustomsRequired(shipment))
            {
                rate.PrintLayout = (PostalUtility.GetCustomsForm(shipment) == PostalCustomsForm.CN72) ? "Normal4X6CP72" : "Normal4X6CN22";
            }

            if (shipment.ReturnShipment)
            {
                // Swapping out Normal with Return indicates a return label
                rate.PrintLayout = rate.PrintLayout.Replace("Normal", "Return");
            }

            return rate;
        }

        /// <summary>
        /// Gets the addons
        /// </summary>
        private static List<AddOnV6> GetAddons(PostalShipmentType shipmentType, ShipmentEntity shipment)
        {
            PostalServiceType serviceType = (PostalServiceType)shipment.Postal.Service;
            PostalPackagingType packagingType = (PostalPackagingType)shipment.Postal.PackagingType;

            List<AddOnV6> addOns = new List<AddOnV6>();

            // For domestic, add in Delivery\Signature confirmation
            if (shipment.ShipPerson.IsDomesticCountry())
            {
                PostalConfirmationType confirmation = (PostalConfirmationType)shipment.Postal.Confirmation;

                // If the service type is Parcel Select, Force DC, otherwise USPS throws an error
                if (confirmation == PostalConfirmationType.Delivery)
                {
                    addOns.Add(new AddOnV6 { AddOnType = AddOnTypeV6.USADC });
                }

                if (confirmation == PostalConfirmationType.Signature)
                {
                    addOns.Add(new AddOnV6 { AddOnType = AddOnTypeV6.USASC });
                }

                if (confirmation == PostalConfirmationType.AdultSignatureRequired)
                {
                    addOns.Add(new AddOnV6 { AddOnType = AddOnTypeV6.USAASR });
                }

                if (confirmation == PostalConfirmationType.AdultSignatureRestricted)
                {
                    addOns.Add(new AddOnV6 { AddOnType = AddOnTypeV6.USAASRD });
                }
            }

            // Check for the new (as of 01/27/13) international delivery service.  In that case, we have to explicitly turn on DC
            else if (shipmentType.IsFreeInternationalDeliveryConfirmation(shipment.ShipCountryCode, serviceType, packagingType))
            {
                addOns.Add(new AddOnV6 { AddOnType = AddOnTypeV6.USADC });
            }

            // For express, apply the signature waiver if necessary
            if (serviceType == PostalServiceType.ExpressMail)
            {
                if (!shipment.Postal.ExpressSignatureWaiver)
                {
                    addOns.Add(new AddOnV6 { AddOnType = AddOnTypeV6.USASR });
                }
            }

            // Add in the hidden postage option (but not supported for envelopes)
            if (shipment.Postal.Usps.HidePostage && shipment.Postal.PackagingType != (int)PostalPackagingType.Envelope)
            {
                addOns.Add(new AddOnV6 { AddOnType = AddOnTypeV6.SCAHP });
            }

            return addOns;
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
            customs.ContentType = ConvertContentType(UspsUtility.GetApiContentType((PostalCustomsContentType)shipment.Postal.CustomsContentType));
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

            // Go through each of the customs contents to create  the USPS custom line entity
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
        private T AuthenticationWrapper<T>(Func<T> executor, UspsAccountEntity account)
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
                        log.ErrorFormat("Failed connecting to USPS: {0}, {1}", UspsApiException.GetErrorCode(ex), ex.Message);

                        if (triesLeft > 0 && IsStaleAuthenticator(ex))
                        {
                            AuthenticateUser(account.Username, SecureText.Decrypt(account.Password, account.Username));
                        }
                        else
                        {
                            throw new UspsApiException(ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw WebHelper.TranslateWebException(ex, typeof(UspsException));
                    }
                }
            }
        }

        /// <summary>
        /// Get the authenticator for the given account
        /// </summary>
        private string GetAuthenticator(UspsAccountEntity account)
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
        /// Converts a current ServiceType to a v36 ServiceType
        /// </summary>
        private static ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.v36.ServiceType ConvertServiceType(ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.ServiceType serviceType)
        {
            switch (serviceType)
            {
                case Usps.WebServices.ServiceType.USFC:
                    return Usps.WebServices.v36.ServiceType.USFC;
                case Usps.WebServices.ServiceType.USPM:
                    return Usps.WebServices.v36.ServiceType.USPM;
                case Usps.WebServices.ServiceType.USXM:
                    return Usps.WebServices.v36.ServiceType.USXM;
                case Usps.WebServices.ServiceType.USMM:
                    return Usps.WebServices.v36.ServiceType.USMM;
                case Usps.WebServices.ServiceType.USBP:
                    return Usps.WebServices.v36.ServiceType.USBP;
                case Usps.WebServices.ServiceType.USLM:
                    return Usps.WebServices.v36.ServiceType.USLM;
                case Usps.WebServices.ServiceType.USEMI:
                    return Usps.WebServices.v36.ServiceType.USEMI;
                case Usps.WebServices.ServiceType.USPMI:
                    return Usps.WebServices.v36.ServiceType.USPMI;
                case Usps.WebServices.ServiceType.USFCI:
                    return Usps.WebServices.v36.ServiceType.USFCI;
                case Usps.WebServices.ServiceType.USCM:
                    return Usps.WebServices.v36.ServiceType.USCM;
                case Usps.WebServices.ServiceType.USPS:
                    return Usps.WebServices.v36.ServiceType.USPS;

                case Usps.WebServices.ServiceType.DHLPE:
                case Usps.WebServices.ServiceType.DHLPG:
                case Usps.WebServices.ServiceType.DHLPPE:
                case Usps.WebServices.ServiceType.DHLPPG:
                case Usps.WebServices.ServiceType.DHLBPME:
                case Usps.WebServices.ServiceType.DHLBPMG:
                default:
                    throw new ArgumentOutOfRangeException("serviceType");
            }
        }

        /// <summary>
        /// Converts a v36 ServiceType to the ServiceType
        /// </summary>
        private static ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.ServiceType ConvertServiceType(ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.v36.ServiceType serviceType)
        {
            switch (serviceType)
            {
                case Usps.WebServices.v36.ServiceType.USFC:
                    return Usps.WebServices.ServiceType.USFC;
                case Usps.WebServices.v36.ServiceType.USPM:
                    return Usps.WebServices.ServiceType.USPM;
                case Usps.WebServices.v36.ServiceType.USXM:
                    return Usps.WebServices.ServiceType.USXM;
                case Usps.WebServices.v36.ServiceType.USMM:
                    return Usps.WebServices.ServiceType.USMM;
                case Usps.WebServices.v36.ServiceType.USBP:
                    return Usps.WebServices.ServiceType.USBP;
                case Usps.WebServices.v36.ServiceType.USLM:
                    return Usps.WebServices.ServiceType.USLM;
                case Usps.WebServices.v36.ServiceType.USEMI:
                    return Usps.WebServices.ServiceType.USEMI;
                case Usps.WebServices.v36.ServiceType.USPMI:
                    return Usps.WebServices.ServiceType.USPMI;
                case Usps.WebServices.v36.ServiceType.USFCI:
                    return Usps.WebServices.ServiceType.USFCI;
                case Usps.WebServices.v36.ServiceType.USCM:
                    return Usps.WebServices.ServiceType.USCM;
                case Usps.WebServices.v36.ServiceType.USPS:
                    return Usps.WebServices.ServiceType.USPS;
                default:
                    throw new ArgumentOutOfRangeException("serviceType");
            }
        }
        
        /// <summary>
        /// Gets the v36 version of the CodewordType
        /// </summary>
        private static ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.v36.PackageTypeV6 ConvertPackageType(ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.PackageTypeV6 packageType)
        {
            switch (packageType)
            {
                case PackageTypeV6.Postcard:
                    return Usps.WebServices.v36.PackageTypeV6.Postcard;
                case PackageTypeV6.Letter:
                    return Usps.WebServices.v36.PackageTypeV6.Letter;
                case PackageTypeV6.LargeEnvelopeorFlat:
                    return Usps.WebServices.v36.PackageTypeV6.LargeEnvelopeorFlat;
                case PackageTypeV6.ThickEnvelope:
                    return Usps.WebServices.v36.PackageTypeV6.ThickEnvelope;
                case PackageTypeV6.Package:
                    return Usps.WebServices.v36.PackageTypeV6.Package;
                case PackageTypeV6.FlatRateBox:
                    return Usps.WebServices.v36.PackageTypeV6.FlatRateBox;
                case PackageTypeV6.SmallFlatRateBox:
                    return Usps.WebServices.v36.PackageTypeV6.SmallFlatRateBox;
                case PackageTypeV6.LargeFlatRateBox:
                    return Usps.WebServices.v36.PackageTypeV6.LargeFlatRateBox;
                case PackageTypeV6.FlatRateEnvelope:
                    return Usps.WebServices.v36.PackageTypeV6.FlatRateEnvelope;
                case PackageTypeV6.FlatRatePaddedEnvelope:
                    return Usps.WebServices.v36.PackageTypeV6.FlatRatePaddedEnvelope;
                case PackageTypeV6.LargePackage:
                    return Usps.WebServices.v36.PackageTypeV6.LargePackage;
                case PackageTypeV6.OversizedPackage:
                    return Usps.WebServices.v36.PackageTypeV6.OversizedPackage;
                case PackageTypeV6.RegionalRateBoxA:
                    return Usps.WebServices.v36.PackageTypeV6.RegionalRateBoxA;
                case PackageTypeV6.RegionalRateBoxB:
                    return Usps.WebServices.v36.PackageTypeV6.RegionalRateBoxB;
                case PackageTypeV6.LegalFlatRateEnvelope:
                    return Usps.WebServices.v36.PackageTypeV6.LegalFlatRateEnvelope;
                case PackageTypeV6.RegionalRateBoxC:
                    return Usps.WebServices.v36.PackageTypeV6.RegionalRateBoxC;
                default:
                    throw new ArgumentOutOfRangeException("packageType");
            }
        }

        /// <summary>
        /// Gets the v36 version of the ContentType
        /// </summary>
        private static ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.v36.ContentTypeV2 ConvertContentType(ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.ContentTypeV2 contentType)
        {
            switch (contentType)
            {
                case Usps.WebServices.ContentTypeV2.CommercialSample:
                    return Usps.WebServices.v36.ContentTypeV2.CommercialSample;
                case Usps.WebServices.ContentTypeV2.Gift:
                    return Usps.WebServices.v36.ContentTypeV2.Gift;
                case Usps.WebServices.ContentTypeV2.Document:
                    return Usps.WebServices.v36.ContentTypeV2.Document;
                case Usps.WebServices.ContentTypeV2.ReturnedGoods:
                    return Usps.WebServices.v36.ContentTypeV2.ReturnedGoods;
                case Usps.WebServices.ContentTypeV2.Other:
                    return Usps.WebServices.v36.ContentTypeV2.Other;
                case Usps.WebServices.ContentTypeV2.Merchandise:
                    return Usps.WebServices.v36.ContentTypeV2.Merchandise;
                case Usps.WebServices.ContentTypeV2.HumanitarianDonation:
                    return Usps.WebServices.v36.ContentTypeV2.HumanitarianDonation;
                case Usps.WebServices.ContentTypeV2.DangerousGoods:
                    return Usps.WebServices.v36.ContentTypeV2.DangerousGoods;
                default:
                    throw new ArgumentOutOfRangeException("contentType");
            }
        }
    }
}
