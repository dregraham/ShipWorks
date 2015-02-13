using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Api.Labels;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;
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
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.UI;
using ShipWorks.Shipping.Editing;
using Interapptive.Shared.Business;
using ShipWorks.ApplicationCore;
using log4net;
using ShipWorks.Templates.Tokens;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration;
using System.Xml.Linq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Shipping.Carriers.BestRate;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Api
{
    /// <summary>
    /// Central point where API stuff goes through for stamps.com
    /// </summary>
    public class StampsWebClient : IStampsWebClient
    {
        // This value came from Stamps.com (the "standard" account value is 88)
        private const int ExpeditedPlanID = 236;

        // These lengths come from the error that Stamps' API gives us when we send data that is too long
        private const int MaxCustomsContentDescriptionLength = 20;
        private const int MaxCustomsItemDescriptionLength = 60;

        private readonly ILog log;
        private readonly ILogEntryFactory logEntryFactory;
        private readonly ICarrierAccountRepository<StampsAccountEntity> accountRepository;

        static Guid integrationID = new Guid("F784C8BC-9CAD-4DAF-B320-6F9F86090032");

        // Cleansed address map so we don't do common addresses over and over again
        static Dictionary<PersonAdapter, Address> cleansedAddressMap = new Dictionary<PersonAdapter, Address>();

        private readonly ICertificateInspector certificateInspector;
        private readonly StampsResellerType stampsResellerType;

        /// <summary>
        /// Initializes a new instance of the <see cref="StampsWebClient" /> class.
        /// </summary>
        /// <param name="stampsResellerType">Type of the stamps reseller.</param>
        public StampsWebClient(StampsResellerType stampsResellerType)
            : this(new StampsAccountRepository(), new LogEntryFactory(), new TrustingCertificateInspector(), stampsResellerType)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StampsWebClient" /> class.
        /// </summary>
        /// <param name="accountRepository">The account repository.</param>
        /// <param name="logEntryFactory">The log entry factory.</param>
        /// <param name="certificateInspector">The certificate inspector.</param>
        /// <param name="stampsResellerType">Type of the stamps reseller.</param>
        public StampsWebClient(ICarrierAccountRepository<StampsAccountEntity> accountRepository, ILogEntryFactory logEntryFactory, ICertificateInspector certificateInspector, StampsResellerType stampsResellerType)
        {
            this.accountRepository = accountRepository;
            this.logEntryFactory = logEntryFactory;
            this.log = LogManager.GetLogger(typeof(StampsWebClient));
            this.certificateInspector = certificateInspector;
            this.stampsResellerType = stampsResellerType;
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
        /// Gets the service URL to use when contacting the Stamps.com API.
        /// </summary>
        private static string ServiceUrl
        {
            get { return UseTestServer ? "https://swsim.testing.stamps.com/swsim/SwsimV40.asmx" : "https://swsim.stamps.com/swsim/SwsimV40.asmx"; }
        }

        /// <summary>
        /// Create the web service instance with the appropriate URL
        /// </summary>
        private SwsimV40 CreateWebService(string logName)
        {
            return CreateWebService(logName, LogActionType.Other);
        }

        /// <summary>
        /// Create the web service instance with the appropriate URL
        /// </summary>
        private SwsimV40 CreateWebService(string logName, LogActionType logActionType)
        {
            SwsimV40 webService = new SwsimV40(logEntryFactory.GetLogEntry(ApiLogSource.UspsStamps, logName, logActionType))
            {
                Url = ServiceUrl
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
                bool codewordsSet;

                using (SwsimV40 webService = CreateWebService("Authenticate"))
                {
                    CheckCertificate(webService.Url);

                    webService.AuthenticateUser(new Credentials
                    {
                        IntegrationID = integrationID,
                        Username = username,
                        Password = password
                    }, out lastLoginTime, out clearCredential, out bannerText, out passwordExpired, out codewordsSet);
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
                string description = EnumHelper.GetDescription(ShipmentTypeCode.Stamps);
                throw new StampsException(string.Format("ShipWorks is unable to make a secure connection to {0}.", description));
            }
        }

        /// <summary>
        /// Get the account info for the given Stamps.com user name
        /// </summary>
        public object GetAccountInfo(StampsAccountEntity account)
        {
            return ExceptionWrapper(() => { return GetAccountInfoInternal(account); }, account);
        }

        /// <summary>
        /// The internal GetAccountInfo implementation that is intended to be wrapped by the exception wrapper
        /// </summary>
        private AccountInfo GetAccountInfoInternal(StampsAccountEntity account)
        {
            AccountInfo accountInfo;

            using (SwsimV40 webService = CreateWebService("GetAccountInfo"))
            {
                // Address and CustomerEmail are not returned by Express1, so do not use them.
                Address address;
                string email;

                webService.GetAccountInfo(GetCredentials(account), out accountInfo, out address, out email);
            }

            return accountInfo;
        }

        /// <summary>
        /// Get the Stamps.com URL of the given urlType
        /// </summary>
        public string GetUrl(StampsAccountEntity account, UrlType urlType)
        {
            return ExceptionWrapper(() => { return GetUrlInternal(account, urlType); }, account);
        }

        /// <summary>
        /// The internal GetUrl implementation that is intended to be wrapped by the exception wrapper
        /// </summary>
        private string GetUrlInternal(StampsAccountEntity account, UrlType urlType)
        {
            string url;

            using (SwsimV40 webService = CreateWebService("GetURL"))
            {
                webService.GetURL(GetCredentials(account), urlType, string.Empty, out url);
            }

            return url;
        }

        /// <summary>
        /// Purchase postage for the given account for the specified amount.  ControlTotal is the ControlTotal value last retrieved from GetAccountInfo.
        /// </summary>
        public void PurchasePostage(StampsAccountEntity account, decimal amount, decimal controlTotal)
        {
            ExceptionWrapper(() => { PurchasePostageInternal(account, amount, controlTotal); return true; }, account);
        }

        /// <summary>
        /// The internal PurchasePostageInternal implementation intended to be wrapped by the exception wrapper
        /// </summary>
        private void PurchasePostageInternal(StampsAccountEntity account, decimal amount, decimal controlTotal)
        {
            PurchaseStatus purchaseStatus;
            int transactionID;
            WebServices.PostageBalance postageBalance;
            string rejectionReason;

            bool miRequired_Unused;

            using (SwsimV40 webService = CreateWebService("PurchasePostage"))
            {
                webService.PurchasePostage(GetCredentials(account), amount, controlTotal, null, null, out purchaseStatus, out transactionID, out postageBalance, out rejectionReason, out miRequired_Unused);
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

                foreach (RateV15 stampsRate in ExceptionWrapper(() => GetRatesInternal(shipment, account), account))
                {
                    PostalServiceType serviceType = StampsUtility.GetPostalServiceType(stampsRate.ServiceType);

                    RateResult baseRate;

                    // If its a rate that has sig\deliv, then you can's select the core rate itself
                    if (stampsRate.AddOns.Any(a => a.AddOnType == AddOnTypeV6.USADC))
                    {
                        baseRate = new RateResult(
                            PostalUtility.GetPostalServiceTypeDescription(serviceType),
                            stampsRate.DeliverDays.Replace("Days", ""))
                        {
                            Tag = new UspsPostalRateSelection(serviceType, PostalConfirmationType.None, account),
                            ProviderLogo = EnumHelper.GetImage((ShipmentTypeCode)shipment.ShipmentType)
                        };
                    }
                    else
                    {
                        baseRate = new RateResult(
                            PostalUtility.GetPostalServiceTypeDescription(serviceType),
                            stampsRate.DeliverDays.Replace("Days", ""),
                            stampsRate.Amount,
                            new UspsPostalRateSelection(serviceType, PostalConfirmationType.None, account))
                        {
                            ProviderLogo = EnumHelper.GetImage((ShipmentTypeCode)shipment.ShipmentType)
                        };
                    }

                    PostalUtility.SetServiceDetails(baseRate, serviceType, stampsRate.DeliverDays);

                    rates.Add(baseRate);

                    // Add a rate for each add-on
                    foreach (AddOnV6 addOn in stampsRate.AddOns)
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
                        }

                        if (name != null)
                        {
                            RateResult addOnRate = new RateResult(
                                name,
                                string.Empty,
                                stampsRate.Amount + addOn.Amount,
                                new UspsPostalRateSelection(serviceType, confirmationType, account));

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
                    string message = string.Format("ShipWorks was unable to connect to {0} with account {1}.{2}{2}Check that your account credentials are correct.",
                                    StampsAccountManager.GetResellerName((StampsResellerType)account.StampsReseller),
                                    account.Username,
                                    Environment.NewLine);

                    throw new StampsException(message, ex);
                }

                // This isn't an authentication exception, so just throw the original exception
                throw;
            }
        }

        /// <summary>
        /// The internal GetRates implementation intended to be wrapped by the exception wrapper
        /// </summary>
        private List<RateV15> GetRatesInternal(ShipmentEntity shipment, StampsAccountEntity account)
        {
            RateV15 rate = CreateRateForRating(shipment, account);

            List<RateV15> rateResults;

            using (SwsimV40 webService = CreateWebService("GetRates", LogActionType.GetRates))
            {
                CheckCertificate(webService.Url);

                RateV15[] ratesArray;

                webService.GetRates(GetCredentials(account), rate, out ratesArray);

                rateResults = ratesArray.ToList();
            }

            List<RateV15> noConfirmationServiceRates = new List<RateV15>();

            // If its a "Flat" then FirstClass and Priority can't have a confirmation
            PostalPackagingType packagingType = (PostalPackagingType)shipment.Postal.PackagingType;
            if (packagingType == PostalPackagingType.Envelope || packagingType == PostalPackagingType.LargeEnvelope)
            {
                noConfirmationServiceRates.AddRange(rateResults.Where(r => r.ServiceType == ServiceType.USFC || r.ServiceType == ServiceType.USPM));
            }

            // Remove the Delivery and Signature add ons from all those that shouldn't support it
            foreach (RateV15 noConfirmationServiceRate in noConfirmationServiceRates)
            {
                if (noConfirmationServiceRate != null && noConfirmationServiceRate.AddOns != null)
                {
                    noConfirmationServiceRate.AddOns = noConfirmationServiceRate.AddOns.Where(a => a.AddOnType != AddOnTypeV6.USASC && a.AddOnType != AddOnTypeV6.USADC).ToArray();
                }
            }

            return rateResults;
        }

        /// <summary>
        /// Cleans the address of the given person using the specified stamps account
        /// </summary>
        private Address CleanseAddress(StampsAccountEntity account, PersonAdapter person, bool requireFullMatch)
        {
            return ExceptionWrapper(() => { return CleanseAddressInternal(person, account, requireFullMatch); }, account);
        }

        /// <summary>
        /// Internal CleanseAddress implementation intended to be warpped by the exception wrapper
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
            RateV15[] rates;
            string fromZipCode = account.PostalCode;

            using (SwsimV40 webService = CreateWebService("CleanseAddress"))
            {
                webService.CleanseAddress(GetCredentials(account), ref address, fromZipCode, out addressMatch, out cityStateZipOK, out residentialIndicator, out isPoBox, out isPoBoxSpecified, out candidates, out statusCodes, out rates);

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
        /// Registers a new account with Stamps.com.
        /// </summary>
        public StampsRegistrationResult RegisterAccount(StampsRegistration registration)
        {
            // Output parameters supplied to the request
            string suggestedUserName = string.Empty;
            int userId = 0;
            string promoUrl = string.Empty;

            try
            {
                RegistrationStatus registrationStatus = RegistrationStatus.Fail;

                using (SwsimV40 webService = CreateWebService("RegisterAccount"))
                {
                    // Note: API docs say the address must be cleansed prior to registering the account, but the API 
                    // for cleansing an address assumes there are existing credentials. Question is out to Stamps.com 
                    // on this, but haven't heard anything back as of 11/8/2012.
                    registrationStatus = webService.RegisterAccount
                        (
                            integrationID,
                            registration.UserName,
                            registration.Password,
                            registration.FirstCodewordType,
                            true,
                            registration.FirstCodewordValue,
                            registration.SecondCodewordType,
                            true,
                            registration.SecondCodewordValue,
                            registration.PhysicalAddress,
                            null,
                            registration.MachineInfo,
                            registration.Email,
                            registration.UsageType,
                            registration.PromoCode,
                            (object)registration.CreditCard ?? registration.AchAccount,
                            out suggestedUserName,
                            out userId,
                            out promoUrl
                        );
                }

                return new StampsRegistrationResult(registrationStatus, suggestedUserName, promoUrl);
            }
            catch (Exception ex)
            {
                throw new StampsRegistrationException("Stamps.com encountered an error trying to register your account:\n\n" + ex.Message, ex);
            }
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
            ExceptionWrapper(() => { result = CreateScanFormInternal(shipments, stampsAccountEntity); return true; }, stampsAccountEntity);

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
            ScanForm scanForm = new ScanForm();

            using (SwsimV40 webService = CreateWebService("ScanForm"))
            {
                webService.CreateScanForm
                    (
                        GetCredentials(stampsAccountEntity),
                        stampsTransactions.ToArray(),
                        CreateScanFormAddress(person),
                        ImageType.Png,
                        false, // Don't print instructions
                        scanForm,
                        null,
                        false,
                        out scanFormStampsId,
                        out scanFormUrl
                    );
            }


            if (scanFormUrl.Contains(" "))
            {
                // According to the docs, there is a chance that there could be multiple URLs; the first
                // URL contains the actual SCAN form though
                scanFormUrl = scanFormUrl.Split(new char[] { ' ' })[0];
            }

            string responseXml = string.Format("<ScanForm><TransactionId>{0}</TransactionId><Url>{1}</Url></ScanForm>", scanFormStampsId, scanFormUrl);
            XDocument response = XDocument.Parse(responseXml);

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

            ExceptionWrapper(() => { VoidShipmentInternal(shipment, account); return true; }, account);
        }

        /// <summary>
        /// The internal VoidShipment implementation intended to be wrapped by the exception wrapper
        /// </summary>
        private void VoidShipmentInternal(ShipmentEntity shipment, StampsAccountEntity account)
        {
            using (SwsimV40 webService = CreateWebService("Void"))
            {
                webService.CancelIndicium(GetCredentials(account), shipment.Postal.Stamps.StampsTransactionID);
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
                ExceptionWrapper(() => { ProcessShipmentInternal(shipment, account); return true; }, account);
            }
            catch (StampsApiException ex)
            {
                if (ex.Message.ToUpperInvariant().Contains("THE USERNAME OR PASSWORD ENTERED IS NOT CORRECT"))
                {
                    // Provide a little more context as to which user name/password was incorrect in the case
                    // where there's multiple accounts or Express1 for Stamps is being used to compare rates
                    string message = string.Format("ShipWorks was unable to connect to {0} with account {1}.{2}{2}Check that your account credentials are correct.",
                                    StampsAccountManager.GetResellerName(stampsResellerType),
                                    account.Username,
                                    Environment.NewLine);

                    throw new StampsException(message, ex);
                }

                if (ex.Code == 5636353 || 
                    ex.Message.ToUpperInvariant().Contains("INSUFFICIENT FUNDS") || ex.Message.ToUpperInvariant().Contains("not enough postage".ToUpperInvariant()) ||
                    ex.Message.ToUpperInvariant().Contains("Insufficient Postage".ToUpperInvariant()))
                {
                    throw new StampsInsufficientFundsException(account, ex.Message);
                }

                // This isn't an exception we can handle, so just throw the original exception
                throw;
            }
        }

        /// <summary>
        /// The internal ProcessShipment implementation intended to be wrapped by the exception wrapper
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

            RateV15 rate = CreateRateForProcessing(shipment, account);
            CustomsV3 customs = CreateCustoms(shipment);
            WebServices.PostageBalance postageBalance;
            string memo = StringUtility.Truncate(TemplateTokenProcessor.ProcessTokens(shipment.Postal.Stamps.Memo, shipment.ShipmentID), 200);

            // Stamps requires that the address in the Rate match that of the request.  Makes sense - but could be different if they auto-cleansed the address.
            rate.ToState = toAddress.State;
            rate.ToZIPCode = toAddress.ZIPCode;

            ThermalLanguage? thermalType = GetThermalLanguage(shipment);

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
                using (SwsimV40 webService = CreateWebService("Process"))
                {
                    // Always use the personal envelope layout to generate the envelope label
                    rate.PrintLayout = "EnvelopePersonal";

                    webService.CreateEnvelopeIndicium(GetCredentials(account), ref integratorGuid,
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
                }
            }
            else
            {
                // Labels for all other package types other than envelope get created via the CreateIndicium method
                using (SwsimV40 webService = CreateWebService("Process"))
                {
                    webService.CreateIndicium(GetCredentials(account), ref integratorGuid,
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
                        false, // delivery notify
                        null,  // shipment notification
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
                        null, false, // returnImageData,
                        null,
                        PaperSizeV1.Default,
                        null,
                        out stampsGuid,
                        out labelUrl,
                        out postageBalance,
                        out mac_Unused,
                        out postageHash,
                        out imageData);
                }
            }

            shipment.TrackingNumber = tracking;
            shipment.ShipmentCost = rate.Amount + (rate.AddOns != null ? rate.AddOns.Sum(a => a.Amount) : 0);
            shipment.Postal.Stamps.StampsTransactionID = stampsGuid;
            shipment.BilledWeight = rate.EffectiveWeightInOunces / 16D;

            // Set the thermal type for the shipment
            shipment.ActualLabelFormat = (int?)thermalType;

            // Interapptive users have an unprocess button.  If we are reprocessing we need to clear the old images
            ObjectReferenceManager.ClearReferences(shipment.ShipmentID);

            string[] labelUrls = labelUrl.Split(' ');
            SaveLabels(shipment, labelUrls);
        }

        /// <summary>
        /// Gets the thermal language based on the shipment type and the requested label format on the shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>The ThermalLanguage value.</returns>
        /// <exception cref="System.InvalidOperationException">Unknown Stamps.com shipment type.</exception>
        private static ThermalLanguage? GetThermalLanguage(ShipmentEntity shipment)
        {
            ThermalLanguage? thermalType;

            // Determine what thermal type, if any to use.  If USPS, use it's setting. Otherwise, use the Stamps 
            // settings if it is a Stamps shipment being auto-switched to an Express1 shipment
            if (shipment.ShipmentType == (int) ShipmentTypeCode.Usps)
            {
                thermalType = shipment.RequestedLabelFormat == (int) ThermalLanguage.None ? null : (ThermalLanguage?) shipment.RequestedLabelFormat;
            }
            else if (shipment.ShipmentType == (int) ShipmentTypeCode.Stamps || shipment.Postal.Stamps.OriginalStampsAccountID != null)
            {
                thermalType = shipment.RequestedLabelFormat == (int) ThermalLanguage.None ? null : (ThermalLanguage?) shipment.RequestedLabelFormat;
            }
            else if (shipment.ShipmentType == (int) ShipmentTypeCode.Express1Stamps)
            {
                thermalType = shipment.RequestedLabelFormat == (int) ThermalLanguage.None ? null : (ThermalLanguage?) shipment.RequestedLabelFormat;
            }
            else
            {
                throw new InvalidOperationException("Unknown Stamps.com shipment type.");
            }

            return thermalType;
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
        private static RateV15 CreateRateForRating(ShipmentEntity shipment, StampsAccountEntity account)
        {
            RateV15 rate = new RateV15();

            string fromZipCode = !string.IsNullOrEmpty(account.MailingPostalCode) ? account.MailingPostalCode : shipment.OriginPostalCode;
            string toZipCode = shipment.ShipPostalCode;
            string toCountry = CountryCodeCleanser.CleanseCountryCode(shipment.ShipCountryCode);

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

            rate.PackageType = StampsUtility.GetApiPackageType((PostalPackagingType)shipment.Postal.PackagingType, new DimensionsAdapter(shipment.Postal));
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
        private static RateV15 CreateRateForProcessing(ShipmentEntity shipment, StampsAccountEntity account)
        {
            PostalServiceType serviceType = (PostalServiceType)shipment.Postal.Service;
            PostalPackagingType packagingType = (PostalPackagingType)shipment.Postal.PackagingType;

            RateV15 rate = CreateRateForRating(shipment, account);
            rate.ServiceType = StampsUtility.GetApiServiceType(serviceType);
            rate.PrintLayout = "Normal";

            List<AddOnV6> addOns = new List<AddOnV6>();

            // For domestic, add in Delivery\Signature confirmation
            if (PostalUtility.IsDomesticCountry(shipment.ShipCountryCode))
            {
                PostalConfirmationType confirmation = (PostalConfirmationType)shipment.Postal.Confirmation;

                // If the service type is Parcel Select, Force DC, otherwise stamps throws an error
                if (confirmation == PostalConfirmationType.Delivery)
                {
                    addOns.Add(new AddOnV6 { AddOnType = AddOnTypeV6.USADC });
                }

                if (confirmation == PostalConfirmationType.Signature)
                {
                    addOns.Add(new AddOnV6 { AddOnType = AddOnTypeV6.USASC });
                }
            }

            // Check for the new (as of 01/27/13) international delivery service.  In that case, we have to explicitly turn on DC
            else if (PostalUtility.IsFreeInternationalDeliveryConfirmation(shipment.ShipCountryCode, serviceType, packagingType))
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
            if (shipment.Postal.Stamps.HidePostage && shipment.Postal.PackagingType != (int)PostalPackagingType.Envelope)
            {
                addOns.Add(new AddOnV6 { AddOnType = AddOnTypeV6.SCAHP });
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
        private static CustomsV3 CreateCustoms(ShipmentEntity shipment)
        {
            if (!CustomsManager.IsCustomsRequired(shipment))
            {
                return null;
            }

            CustomsV3 customs = new CustomsV3();

            // Content type
            customs.ContentType = StampsUtility.GetApiContentType((PostalCustomsContentType)shipment.Postal.CustomsContentType);
            if (customs.ContentType == ContentTypeV2.Other)
            {
                if (shipment.Postal.CustomsContentType == (int)PostalCustomsContentType.Merchandise)
                {
                    customs.OtherDescribe = "Merchandise";
                }
                else
                {
                    customs.OtherDescribe = shipment.Postal.CustomsContentDescription.Truncate(MaxCustomsContentDescriptionLength); ;
                }
            }

            List<CustomsLine> lines = new List<CustomsLine>();

            // Go through each of the customs contents to create  the stamps.com custom line entity
            foreach (ShipmentCustomsItemEntity customsItem in shipment.CustomsItems)
            {
                WeightValue weightValue = new WeightValue(customsItem.Weight);

                CustomsLine line = new CustomsLine();
                line.Description = customsItem.Description.Truncate(MaxCustomsItemDescriptionLength); ;
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
        /// Makes request to Stamps.com API to change plan associated with the account referenced by the authenticator to be 
        /// an Expedited plan. This requires an authentication call to the Stamps.com API prior to changing the plan.
        /// </summary>
        public void ChangeToExpeditedPlan(StampsAccountEntity account, string promoCode)
        {
            ExceptionWrapper(() =>
            {
                InternalChangeToExpeditedPlan(GetCredentials(account), promoCode);
                return true;
            }, account);
        }

        /// <summary>
        /// Makes request to Stamps.com API to change plan associated with the account referenced by the authenticator to be 
        /// an Expedited plan. This requires an authentication call to the Stamps.com API prior to changing the plan.
        /// </summary>
        private void InternalChangeToExpeditedPlan(Credentials credentials, string promoCode)
        {
            // Output parameters for web service call
            int transactionID;
            PurchaseStatus purchaseStatus;
            string rejectionReason = string.Empty;

            try
            {
                using (SwsimV40 webService = CreateWebService("ChangePlan"))
                {
                    webService.Url = ServiceUrl;
                    webService.ChangePlan(credentials, ExpeditedPlanID, promoCode, out purchaseStatus, out transactionID, out rejectionReason);
                }
            }
            catch (StampsException exception)
            {
                log.ErrorFormat("ShipWorks was unable to change the Stamps.com plan. {0}. {1}", rejectionReason ?? string.Empty, exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Checks with Stamps.com to get the contract type of the account.
        /// </summary>
        public StampsAccountContractType GetContractType(StampsAccountEntity account)
        {
            return ExceptionWrapper(() => InternalGetContractType(account), account);
        }

        /// <summary>
        /// The internal GetContractType implementation that is intended to be wrapped by the auth wrapper
        /// </summary>
        private StampsAccountContractType InternalGetContractType(StampsAccountEntity account)
        {
            StampsAccountContractType contract = StampsAccountContractType.Unknown;
            AccountInfo accountInfo;

            using (SwsimV40 webService = CreateWebService("GetContractType"))
            {
                CheckCertificate(webService.Url);

                // Address and CustomerEmail are not returned by Express1, so do not use them.
                Address address;
                string email;

                webService.GetAccountInfo(GetCredentials(account), out accountInfo, out address, out email);
            }

            RatesetType? rateset = accountInfo.RatesetType;
            if (rateset.HasValue)
            {
                switch (rateset)
                {
                    case RatesetType.CBP:
                    case RatesetType.Retail:
                        contract = StampsAccountContractType.Commercial;
                        break;

                    case RatesetType.CPP:
                    case RatesetType.NSA:
                        contract = StampsAccountContractType.CommercialPlus;
                        break;

                    case RatesetType.Reseller:
                        contract = StampsAccountContractType.Reseller;
                        break;

                    default:
                        contract = StampsAccountContractType.Unknown;
                        break;
                }
            }

            return contract;
        }

        /// <summary>
        /// Handles exceptions when making calls to the Stamps API
        /// </summary>
        private T ExceptionWrapper<T>(Func<T> executor, StampsAccountEntity account)
        {
            try
            {
                return executor();
            }
            catch (SoapException ex)
            {
                log.ErrorFormat("Failed connecting to Stamps.com.  Account: {0}, Error Code: '{1}', Exception Message: {2}", account.StampsAccountID, StampsApiException.GetErrorCode(ex), ex.Message);

                throw new StampsApiException(ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(StampsException));
            }
        }

        /// <summary>
        /// Get the Credentials for the given account
        /// </summary>
        private static Credentials GetCredentials(StampsAccountEntity account)
        {
            return new Credentials
            {
                IntegrationID = integrationID,
                Username = account.Username,
                Password = SecureText.Decrypt(account.Password, account.Username)
            };
        }
    }
}
