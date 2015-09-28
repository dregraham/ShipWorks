using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.AddressValidation;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Labels;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using System.Threading.Tasks;
using ShipWorks.Core.Common.Threading;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net
{
    /// <summary>
    /// Central point where API stuff goes through for USPS
    /// </summary>
    public class UspsWebClient : IUspsWebClient
    {
        // This value came from USPS (the "standard" account value is 88)
        private const int ExpeditedPlanID = 0;

        // These lengths come from the error that USPS's API gives us when we send data that is too long
        private const int MaxCustomsContentDescriptionLength = 20;
        private const int MaxCustomsItemDescriptionLength = 60;

        private readonly ILog log;
        private readonly ILogEntryFactory logEntryFactory;
        private readonly ICarrierAccountRepository<UspsAccountEntity> accountRepository;

        static Guid integrationID = new Guid("F784C8BC-9CAD-4DAF-B320-6F9F86090032");

        // Cleansed address map so we don't do common addresses over and over again
        static Dictionary<PersonAdapter, Address> cleansedAddressMap = new Dictionary<PersonAdapter, Address>();

        private readonly ICertificateInspector certificateInspector;
        private readonly UspsResellerType uspsResellerType;

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsWebClient" /> class.
        /// </summary>
        /// <param name="uspsResellerType">Type of the USPS reseller.</param>
        public UspsWebClient(UspsResellerType uspsResellerType)
            : this(new UspsAccountRepository(), new LogEntryFactory(), new TrustingCertificateInspector(), uspsResellerType)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsWebClient" /> class.
        /// </summary>
        /// <param name="accountRepository">The account repository.</param>
        /// <param name="logEntryFactory">The log entry factory.</param>
        /// <param name="certificateInspector">The certificate inspector.</param>
        /// <param name="uspsResellerType">Type of the USPS reseller.</param>
        public UspsWebClient(ICarrierAccountRepository<UspsAccountEntity> accountRepository, ILogEntryFactory logEntryFactory, ICertificateInspector certificateInspector, UspsResellerType uspsResellerType)
        {
            this.accountRepository = accountRepository;
            this.logEntryFactory = logEntryFactory;
            this.log = LogManager.GetLogger(typeof(UspsWebClient));
            this.certificateInspector = certificateInspector;
            this.uspsResellerType = uspsResellerType;
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
        /// Gets the service URL to use when contacting the USPS API.
        /// </summary>
        private static string ServiceUrl
        {
            get { return UseTestServer ? "https://swsim.testing.stamps.com/swsim/SwsimV49.asmx" : "https://swsim.stamps.com/swsim/SwsimV49.asmx"; }
        }

        /// <summary>
        /// Create the web service instance with the appropriate URL
        /// </summary>
        private SwsimV49 CreateWebService(string logName)
        {
            return CreateWebService(logName, LogActionType.Other);
        }

        /// <summary>
        /// Returns the current type of the web service.  Used to remove the namespace in exceptions received from Stamps.
        /// </summary>
        public static Type WebServiceType
        {
            get { return typeof(SwsimV49); }
        }

        /// <summary>
        /// Create the web service instance with the appropriate URL
        /// </summary>
        private SwsimV49 CreateWebService(string logName, LogActionType logActionType)
        {
            SwsimV49 webService = new SwsimV49(logEntryFactory.GetLogEntry(ApiLogSource.Usps, logName, logActionType))
            {
                Url = ServiceUrl
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
                bool codewordsSet;

                using (SwsimV49 webService = CreateWebService("Authenticate"))
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
                throw new UspsApiException(ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(UspsException));
            }
        }

        /// <summary>
        /// Makes a request to the specified url, and determines it's CertificateSecurityLevel
        /// </summary>
        private void CheckCertificate(string url)
        {
            CertificateRequest certificateRequest = new CertificateRequest(new Uri(url), certificateInspector);
            CertificateSecurityLevel certificateSecurityLevel = certificateRequest.Submit();

            if (certificateSecurityLevel != CertificateSecurityLevel.Trusted)
            {
                string description = EnumHelper.GetDescription(ShipmentTypeCode.Usps);
                throw new UspsException(string.Format("ShipWorks is unable to make a secure connection to {0}.", description));
            }
        }

        /// <summary>
        /// Get the account info for the given USPS user name
        /// </summary>
        public object GetAccountInfo(UspsAccountEntity account)
        {
            return ExceptionWrapper(() => { return GetAccountInfoInternal(account); }, account);
        }

        /// <summary>
        /// The internal GetAccountInfo implementation that is intended to be wrapped by the exception wrapper
        /// </summary>
        private AccountInfo GetAccountInfoInternal(UspsAccountEntity account)
        {
            AccountInfo accountInfo;

            using (SwsimV49 webService = CreateWebService("GetAccountInfo"))
            {
                // Address and CustomerEmail are not returned by Express1, so do not use them.
                Address address;
                string email;

                webService.GetAccountInfo(GetCredentials(account), out accountInfo, out address, out email);
            }

            return accountInfo;
        }

        /// <summary>
        /// Get the USPS URL of the given urlType
        /// </summary>
        public string GetUrl(UspsAccountEntity account, UrlType urlType)
        {
            return ExceptionWrapper(() => { return GetUrlInternal(account, urlType); }, account);
        }

        /// <summary>
        /// The internal GetUrl implementation that is intended to be wrapped by the exception wrapper
        /// </summary>
        private string GetUrlInternal(UspsAccountEntity account, UrlType urlType)
        {
            string url;

            using (SwsimV49 webService = CreateWebService("GetURL"))
            {
                webService.GetURL(GetCredentials(account), urlType, string.Empty, out url);
            }

            return url;
        }

        /// <summary>
        /// Purchase postage for the given account for the specified amount.  ControlTotal is the ControlTotal value last retrieved from GetAccountInfo.
        /// </summary>
        public void PurchasePostage(UspsAccountEntity account, decimal amount, decimal controlTotal)
        {
            ExceptionWrapper(() => { PurchasePostageInternal(account, amount, controlTotal); return true; }, account);
        }

        /// <summary>
        /// The internal PurchasePostageInternal implementation intended to be wrapped by the exception wrapper
        /// </summary>
        private void PurchasePostageInternal(UspsAccountEntity account, decimal amount, decimal controlTotal)
        {
            PurchaseStatus purchaseStatus;
            int transactionID;
            Usps.WebServices.PostageBalance postageBalance;
            string rejectionReason;

            bool miRequired_Unused;

            using (SwsimV49 webService = CreateWebService("PurchasePostage"))
            {
                webService.PurchasePostage(GetCredentials(account), amount, controlTotal, null, null, out purchaseStatus, out transactionID, out postageBalance, out rejectionReason, out miRequired_Unused);
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

                foreach (RateV18 uspsRate in ExceptionWrapper(() => GetRatesInternal(shipment, account), account))
                {
                    PostalServiceType serviceType = UspsUtility.GetPostalServiceType(uspsRate.ServiceType);

                    RateResult baseRate;

                    // If its a rate that has sig\deliv, then you can's select the core rate itself
                    if (uspsRate.AddOns.Any(a => a.AddOnType == AddOnTypeV7.USADC))
                    {
                        baseRate = new RateResult(
                            PostalUtility.GetPostalServiceTypeDescription(serviceType),
                            uspsRate.DeliverDays.Replace("Days", ""))
                        {
                            Tag = new UspsPostalRateSelection(serviceType, PostalConfirmationType.None, account),
                            ProviderLogo = EnumHelper.GetImage((ShipmentTypeCode)shipment.ShipmentType)
                        };
                    }
                    else
                    {
                        baseRate = new RateResult(
                            PostalUtility.GetPostalServiceTypeDescription(serviceType),
                            uspsRate.DeliverDays.Replace("Days", ""),
                            uspsRate.Amount,
                            new UspsPostalRateSelection(serviceType, PostalConfirmationType.None, account))
                        {
                            ProviderLogo = EnumHelper.GetImage((ShipmentTypeCode)shipment.ShipmentType)
                        };
                    }

                    PostalUtility.SetServiceDetails(baseRate, serviceType, uspsRate.DeliverDays);

                    rates.Add(baseRate);

                    // Add a rate for each add-on
                    AddRatesForAddOns(uspsRate, serviceType, account, rates);
                }

                return rates;
            }
            catch (UspsApiException ex)
            {
                if (ex.Message.ToUpperInvariant().Contains("THE USERNAME OR PASSWORD ENTERED IS NOT CORRECT"))
                {
                    // Provide a little more context as to which user name/password was incorrect in the case
                    // where there's multiple accounts or Express1 for USPS is being used to compare rates
                    string message = string.Format("ShipWorks was unable to connect to {0} with account {1}.{2}{2}Check that your account credentials are correct.",
                                    UspsAccountManager.GetResellerName((UspsResellerType)account.UspsReseller),
                                    account.Username,
                                    Environment.NewLine);

                    throw new UspsException(message, ex);
                }

                // This isn't an authentication exception, so just throw the original exception
                throw;
            }
        }

        /// <summary>
        /// Iterates through each rate add on and creates a rate for each
        /// </summary>
        private static void AddRatesForAddOns(RateV18 uspsRate, PostalServiceType serviceType, UspsAccountEntity account, List<RateResult> rates)
        {
            // Add a rate for each add-on
            foreach (AddOnV7 addOn in uspsRate.AddOns)
            {
                string name = null;
                PostalConfirmationType confirmationType = PostalConfirmationType.None;

                switch (addOn.AddOnType)
                {
                    case AddOnTypeV7.USADC:
                        name = string.Format("       Delivery Confirmation ({0:c})", addOn.Amount);
                        confirmationType = PostalConfirmationType.Delivery;
                        break;

                    case AddOnTypeV7.USASC:
                        name = string.Format("       Signature Confirmation ({0:c})", addOn.Amount);
                        confirmationType = PostalConfirmationType.Signature;
                        break;

                    case AddOnTypeV7.USAASR:
                        name = string.Format("       Adult Signature Required ({0:c})", addOn.Amount);
                        confirmationType = PostalConfirmationType.AdultSignatureRequired;
                        break;

                    case AddOnTypeV7.USAASRD:
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
                        new UspsPostalRateSelection(serviceType, confirmationType, account));

                    PostalUtility.SetServiceDetails(addOnRate, serviceType, uspsRate.DeliverDays);

                    rates.Add(addOnRate);
                }
            }
        }

        /// <summary>
        /// The internal GetRates implementation intended to be wrapped by the exception wrapper
        /// </summary>
        private List<RateV18> GetRatesInternal(ShipmentEntity shipment, UspsAccountEntity account)
        {
            RateV18 rate = CreateRateForRating(shipment, account);

            List<RateV18> rateResults;

            using (SwsimV49 webService = CreateWebService("GetRates", LogActionType.GetRates))
            {
                CheckCertificate(webService.Url);

                RateV18[] ratesArray;

                webService.GetRates(GetCredentials(account), rate, out ratesArray);

                rateResults = ratesArray.ToList();
            }

            List<RateV18> noConfirmationServiceRates = new List<RateV18>();

            // If its a "Flat" then FirstClass and Priority can't have a confirmation
            PostalPackagingType packagingType = (PostalPackagingType)shipment.Postal.PackagingType;
            if (packagingType == PostalPackagingType.Envelope || packagingType == PostalPackagingType.LargeEnvelope)
            {
                noConfirmationServiceRates.AddRange(rateResults.Where(r => r.ServiceType == ServiceType.USFC || r.ServiceType == ServiceType.USPM));
            }

            // Remove the Delivery and Signature add ons from all those that shouldn't support it
            foreach (RateV18 noConfirmationServiceRate in noConfirmationServiceRates)
            {
                if (noConfirmationServiceRate != null && noConfirmationServiceRate.AddOns != null)
                {
                    noConfirmationServiceRate.AddOns = noConfirmationServiceRate.AddOns.Where(a => a.AddOnType != AddOnTypeV7.USASC && a.AddOnType != AddOnTypeV7.USADC).ToArray();
                }
            }

            return rateResults;
        }

        /// <summary>
        /// Cleans the address of the given person using the specified USPS account
        /// </summary>
        private Address CleanseAddress(UspsAccountEntity account, PersonAdapter person, bool requireFullMatch)
        {
            return ExceptionWrapper(() => { return CleanseAddressInternal(person, account, requireFullMatch); }, account);
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

            Task<UspsAddressValidationResults> task = ValidateAddressAsync(person, account);
            task.Wait();
            UspsAddressValidationResults results = task.Result;

            if (!results.IsSuccessfulMatch)
            {
                if (!results.IsCityStateZipOk)
                {
                    throw new UspsException(string.Format("The address for '{0}' is not a valid address.", new PersonName(person).FullName));
                }

                if (requireFullMatch)
                {
                    throw new UspsException(string.Format("The city, state, and postal code for '{0}' is valid, but the full address is not.", new PersonName(person).FullName));
                }
            }

            cleansedAddressMap[person] = results.MatchedAddress;

            return results.MatchedAddress;
        }

        /// <summary>
        /// Validates the address of the given person
        /// </summary>
        public Task<UspsAddressValidationResults> ValidateAddressAsync(PersonAdapter person)
        {
            return ValidateAddressAsync(person, null);
        }

        /// <summary>
        /// Validates the address of the given person using the specified stamps account
        /// </summary>
        private async Task<UspsAddressValidationResults> ValidateAddressAsync(PersonAdapter person, UspsAccountEntity account)
        {
            Address address = CreateAddress(person);

            address.Address1 = AdjustAddressLineForLength(address.Address1);
            address.Address2 = AdjustAddressLineForLength(address.Address2);
            address.Address3 = AdjustAddressLineForLength(address.Address3);

            address.State = PostalUtility.AdjustState(person.CountryCode, person.StateProvCode);
            address.Country = person.AdjustedCountryCode(ShipmentTypeCode.Usps);
            
            string badAddressMessage = null;

            CleanseAddressCompletedEventArgs result = null;

            try
            {
                await ActionRetry.ExecuteWithRetry<InvalidOperationException>(3, async () =>
                {
                    using (SwsimV49 webService = CreateWebService("CleanseAddress", LogActionType.ExtendedLogging))
                    {
                        using (new LoggedStopwatch(log, "UspsWebClient.ValidateAddress - webService.CleanseAddress"))
                        {
                            TaskCompletionSource<CleanseAddressCompletedEventArgs> taskCompletion = new TaskCompletionSource<CleanseAddressCompletedEventArgs>();
                            
                            webService.CleanseAddressCompleted += (s, e) =>
                            {
                                if (e.Error != null)
                                {
                                    taskCompletion.SetException(e.Error);
                                }
                                else
                                {
                                    taskCompletion.SetResult(e);
                                }
                            };

                            webService.CleanseAddressAsync(GetCredentials(account, true), address, null);
                            result = await taskCompletion.Task;
                        }
                    }
                });
            }
            catch (AggregateException ex)
            {
                SoapException soapException = ex.InnerExceptions.OfType<SoapException>().FirstOrDefault();
                if (soapException != null)
                {
                    log.Error(ex);

                    // Rethrow the exception, but filter out namespaces and information that isn't useful to customers
                    badAddressMessage = ex.Message.Replace("Invalid SOAP message due to XML Schema validation failure. ", string.Empty);
                    badAddressMessage = Regex.Replace(badAddressMessage, @"http://stamps.com/xml/namespace/\d{4}/\d{1,2}/swsim/swsimv\d*:", string.Empty);

                    return new UspsAddressValidationResults()
                    {
                        IsSuccessfulMatch = false,
                        BadAddressMessage = badAddressMessage,
                        Candidates = new List<Address>()
                    };
                }

                InvalidOperationException invalidOperationException = ex.InnerExceptions.OfType<InvalidOperationException>().FirstOrDefault();
                if (invalidOperationException != null)
                {
                    throw invalidOperationException;
                }
                
                throw WebHelper.TranslateWebException(ex.InnerException, typeof(AddressValidationException));
            }
            catch (SoapException ex)
            {
                log.Error(ex);

                // Rethrow the exception, but filter out namespaces and information that isn't useful to customers
                badAddressMessage = ex.Message.Replace("Invalid SOAP message due to XML Schema validation failure. ", string.Empty);
                badAddressMessage = Regex.Replace(badAddressMessage, @"http://stamps.com/xml/namespace/\d{4}/\d{1,2}/swsim/swsimv\d*:", string.Empty);

                return new UspsAddressValidationResults()
                {
                    IsSuccessfulMatch = false,
                    BadAddressMessage = badAddressMessage,
                    Candidates = new List<Address>()
                };
            }
            catch (Exception ex)
            {
                log.Error(ex);

                throw WebHelper.TranslateWebException(ex, typeof(AddressValidationException));
            }

            if (!result.AddressMatch)
            {
                badAddressMessage = result.CityStateZipOK ?
                    "City, State and ZIP Code are valid, but street address is not a match." :
                    "The address as submitted could not be found. Check for excessive abbreviations in the street address line or in the City name.";
            }

            return new UspsAddressValidationResults
            {
                IsSuccessfulMatch = result.AddressMatch,
                IsCityStateZipOk = result.CityStateZipOK,
                ResidentialIndicator = result.ResidentialDeliveryIndicator,
                IsPoBox = result.IsPOBox,
                MatchedAddress = address,
                Candidates = result.CandidateAddresses,
                BadAddressMessage = badAddressMessage
            };
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
        /// Registers a new account with USPS
        /// </summary>
        public UspsRegistrationResult RegisterAccount(UspsRegistration registration)
        {
            // Output parameters supplied to the request
            string suggestedUserName = string.Empty;
            int userId = 0;
            string promoUrl = string.Empty;

            try
            {
                RegistrationStatus registrationStatus = RegistrationStatus.Fail;

                using (SwsimV49 webService = CreateWebService("RegisterAccount"))
                {
                    // Note: API docs say the address must be cleansed prior to registering the account, but the API 
                    // for cleansing an address assumes there are existing credentials. Question is out to USPS
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

                return new UspsRegistrationResult(registrationStatus, suggestedUserName, promoUrl);
            }
            catch (Exception ex)
            {
                throw new UspsRegistrationException("Stamps.com encountered an error trying to register your account:\n\n" + ex.Message, ex);
            }
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
            ExceptionWrapper(() => { result = CreateScanFormInternal(shipments, uspsAccountEntity); return true; }, uspsAccountEntity);

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
            IEnumerable<UspsShipmentEntity> uspsShipmentEntities = shipments as IList<UspsShipmentEntity> ?? shipments.ToList();

            List<Guid> uspsTransactions = uspsShipmentEntities.Select(s => s.UspsTransactionID).ToList();
            PersonAdapter person = new PersonAdapter(uspsAccountEntity, string.Empty);
            Carrier carrier = GetScanFormCarrier(uspsShipmentEntities.ToList());

            string scanFormUspsId = string.Empty;
            string scanFormUrl = string.Empty;

            using (SwsimV49 webService = CreateWebService("ScanForm"))
            {
                webService.CreateScanForm
                    (
                        GetCredentials(uspsAccountEntity),
                        uspsTransactions.ToArray(),
                        CreateScanFormAddress(person),
                        ImageType.Png,
                        false, // Don't print instructions
                        carrier,
                        null,
                        false,
                        out scanFormUspsId,
                        out scanFormUrl
                    );
            }

            string responseXml = string.Format("<ScanForm><TransactionId>{0}</TransactionId><Url>{1}</Url></ScanForm>", scanFormUspsId, scanFormUrl);
            XDocument response = XDocument.Parse(responseXml);

            return response;
        }

        /// <summary>
        /// A helper method to determine which carrier a SCAN form is being generated for: USPS or DHL.
        /// </summary>
        /// <param name="shipments">The list of shipments going to be used to create the SCAN form.</param>
        /// <returns>Carrier.Usps if all shipments are USPS services; Carrier.DHL if all shipments are DHL services.</returns>
        /// <exception cref="UspsException">The Stamps.com API does not support creating a SCAN form containing a mixture of USPS and DHL shipments.</exception>
        private static Carrier GetScanFormCarrier(List<UspsShipmentEntity> shipments)
        {
            if (shipments.All(s => ShipmentTypeManager.IsDhl((PostalServiceType)s.PostalShipment.Service)))
            {
                return Carrier.Dhl;
            }

            if (shipments.All(s => !ShipmentTypeManager.IsDhl((PostalServiceType)s.PostalShipment.Service)))
            {
                return Carrier.Usps;
            }

            throw new UspsException("The Stamps.com API does not support creating a SCAN form containing a mixture of USPS and DHL shipments.");
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

            ExceptionWrapper(() => { VoidShipmentInternal(shipment, account); return true; }, account);
        }

        /// <summary>
        /// The internal VoidShipment implementation intended to be wrapped by the exception wrapper
        /// </summary>
        private void VoidShipmentInternal(ShipmentEntity shipment, UspsAccountEntity account)
        {
            using (SwsimV49 webService = CreateWebService("Void"))
            {
                webService.CancelIndicium(GetCredentials(account), shipment.Postal.Usps.UspsTransactionID);
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
                ExceptionWrapper(() =>
                {
                    ProcessShipmentInternal(shipment, account);
                    return true;
                }, account);
            }
            catch (UspsApiException ex)
            {
                string errorMessageUpper = ex.Message.ToUpperInvariant();

                if (errorMessageUpper.Contains("THE USERNAME OR PASSWORD ENTERED IS NOT CORRECT"))
                {
                    // Provide a little more context as to which user name/password was incorrect in the case
                    // where there's multiple accounts or Express1 for USPS is being used to compare rates
                    string message = string.Format("ShipWorks was unable to connect to {0} with account {1}.{2}{2}Check that your account credentials are correct.",
                        UspsAccountManager.GetResellerName(uspsResellerType),
                        account.Username,
                        Environment.NewLine);

                    throw new UspsException(message, ex);
                }

                if (ex.Code == 5636353 ||
                    errorMessageUpper.Contains("INSUFFICIENT FUNDS") || errorMessageUpper.Contains("not enough postage".ToUpperInvariant()) ||
                    errorMessageUpper.Contains("Insufficient Postage".ToUpperInvariant()))
                {
                    throw new UspsInsufficientFundsException(account, ex.Message);
                }

                if (errorMessageUpper.Contains("DHL") && errorMessageUpper.Contains("IS NOT ALLOWED"))
                {
                    throw new UspsException("Your Stamps.com account has not been enabled to use the selected DHL service.");
                }

                // This isn't an exception we can handle, so just throw the original exception
                throw;
            }
        }

        /// <summary>
        /// The internal ProcessShipment implementation intended to be wrapped by the exception wrapper
        /// </summary>
        private void ProcessShipmentInternal(ShipmentEntity shipment, UspsAccountEntity account)
        {
            Guid uspsGuid;
            string tracking = string.Empty;
            string labelUrl;

            Address fromAddress;
            Address toAddress;

            FixWebserviceAddresses(account, shipment, out toAddress, out fromAddress);

            RateV18 rate = CreateRateForProcessing(shipment, account);
            CustomsV3 customs = CreateCustoms(shipment);
            WebServices.PostageBalance postageBalance;

            // USPS requires that the address in the Rate match that of the request.  Makes sense - but could be different if they auto-cleansed the address.
            rate.ToState = toAddress.State;
            rate.ToZIPCode = toAddress.ZIPCode;

            ThermalLanguage? thermalType = GetThermalLanguage(shipment);

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

            if (shipment.Postal.PackagingType == (int)PostalPackagingType.Envelope && shipment.Postal.Service != (int)PostalServiceType.InternationalFirst)
            {
                // Envelopes don't support thermal
                thermalType = null;

                // A separate service call is used for processing envelope according to USPS as of v. 22
                using (SwsimV49 webService = CreateWebService("Process"))
                {
                    // Always use the personal envelope layout to generate the envelope label
                    rate.PrintLayout = "EnvelopePersonal";

                    webService.CreateEnvelopeIndicium(GetCredentials(account), ref integratorGuid,
                        ref rate,
                        fromAddress,
                        toAddress,
                        null,
                        CreateIndiciumModeV1.Normal,
                        ImageType.Png,
                        0, // cost code ID
                        false, // do not hide the facing identification mark (FIM) 
                        out tracking,
                        out uspsGuid,
                        out labelUrl,
                        out postageBalance,
                        out mac_Unused,
                        out postageHash);
                }
            }
            else
            {
                // Labels for all other package types other than envelope get created via the CreateIndicium method
                using (SwsimV49 webService = CreateWebService("Process"))
                {
                    webService.CreateIndicium(GetCredentials(account), ref integratorGuid,
                        ref tracking,
                        ref rate,
                        fromAddress,
                        toAddress,
                        null,
                        customs,
                        false,  // Sample only,
                        shipment.Postal.NoPostage ? PostageMode.NoPostage : PostageMode.Normal,
                        thermalType == null ? ImageType.Png : ((thermalType == ThermalLanguage.EPL) ? ImageType.Epl : ImageType.Zpl),
                        EltronPrinterDPIType.Default,
                        UspsUtility.BuildMemoField(shipment.Postal), // Memo
                        0, // Cost Code
                        false, // delivery notify
                        null, // shipment notification
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
                        true, true, // returnImageData,
                        null,
                        PaperSizeV1.Default,
                        null,
                        false, //PayOnPrint
                        null, //ReturnLabelExpirationDays
                        false, //ReturnLabelExpirationDaysSpecified
                        out uspsGuid,
                        out labelUrl,
                        out postageBalance,
                        out mac_Unused,
                        out postageHash,
                        out imageData);
                }
            }

            shipment.TrackingNumber = tracking;
            shipment.ShipmentCost = rate.Amount + (rate.AddOns != null ? rate.AddOns.Where(a => a.AddOnType != AddOnTypeV7.SCAINS).Sum(a => a.Amount) : 0);
            shipment.Postal.Usps.UspsTransactionID = uspsGuid;
            shipment.BilledWeight = rate.EffectiveWeightInOunces / 16D;

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

            // Per stamps - only send state for domestic - send province for Intl
            if (!toAddress.AsAddressAdapter().IsDomesticCountry())
            {
                toAddress.Province = toAddress.State;
                toAddress.State = string.Empty;
            }
        }

        /// <summary>
        /// Gets the thermal language based on the shipment type and the requested label format on the shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>The ThermalLanguage value.</returns>
        /// <exception cref="System.InvalidOperationException">Unknown USPS shipment type.</exception>
        private static ThermalLanguage? GetThermalLanguage(ShipmentEntity shipment)
        {
            ThermalLanguage? thermalType;

            // Determine what thermal type, if any to use.  If USPS, use it's setting. Otherwise, use the USPS
            // settings if it is a USPS shipment being auto-switched to an Express1 shipment
            if (shipment.ShipmentType == (int)ShipmentTypeCode.Usps)
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

            return thermalType;
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

            address.Country = person.AdjustedCountryCode(ShipmentTypeCode.Usps);

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
        private static RateV18 CreateRateForRating(ShipmentEntity shipment, UspsAccountEntity account)
        {
            RateV18 rate = new RateV18();

            string fromZipCode = !string.IsNullOrEmpty(account.MailingPostalCode) ? account.MailingPostalCode : shipment.OriginPostalCode;
            string toZipCode = shipment.ShipPostalCode;
            string toCountry = shipment.AdjustedShipCountryCode();

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

            rate.PackageType = UspsUtility.GetApiPackageType((PostalPackagingType)shipment.Postal.PackagingType, new DimensionsAdapter(shipment.Postal));
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
        private static RateV18 CreateRateForProcessing(ShipmentEntity shipment, UspsAccountEntity account)
        {
            PostalServiceType serviceType = (PostalServiceType)shipment.Postal.Service;
            PostalPackagingType packagingType = (PostalPackagingType)shipment.Postal.PackagingType;

            RateV18 rate = CreateRateForRating(shipment, account);
            rate.ServiceType = UspsUtility.GetApiServiceType(serviceType);
            rate.PrintLayout = "Normal4X6";

            // Get the confirmation type add ons
            List<AddOnV7> addOns = AddConfirmationTypeAddOnsForProcessing(shipment, serviceType, packagingType);

            // For express, apply the signature waiver if necessary
            if (serviceType == PostalServiceType.ExpressMail)
            {
                if (!shipment.Postal.ExpressSignatureWaiver)
                {
                    addOns.Add(new AddOnV7 { AddOnType = AddOnTypeV7.USASR });
                }
            }

            // Add in the hidden postage option (but not supported for envelopes)
            if (shipment.Postal.Usps.HidePostage && shipment.Postal.PackagingType != (int)PostalPackagingType.Envelope)
            {
                addOns.Add(new AddOnV7 { AddOnType = AddOnTypeV7.SCAHP });
            }

            // Add insurance if using SDC insurance
            if (shipment.Insurance && shipment.InsuranceProvider == (int)InsuranceProvider.Carrier)
            {
                rate.InsuredValue = shipment.Postal.InsuranceValue;
                addOns.Add(new AddOnV7 { AddOnType = AddOnTypeV7.SCAINS });
            }

            if (addOns.Count > 0)
            {
                rate.AddOns = addOns.ToArray();
            }

            // For APO/FPO, we have to specifically ask for customs docs
            if (PostalUtility.IsMilitaryState(shipment.ShipStateProvCode) || ShipmentTypeManager.GetType(shipment).IsCustomsRequired(shipment))
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
        /// Returns a list of confirmation type add ons for the shipment
        /// </summary>
        private static List<AddOnV7> AddConfirmationTypeAddOnsForProcessing(ShipmentEntity shipment, PostalServiceType serviceType, PostalPackagingType packagingType)
        {
            List<AddOnV7> addOns = new List<AddOnV7>();

            // For domestic, add in Delivery\Signature confirmation; delivery confirmation is not allowed on DHL services
            if (SupportsConfirmation(shipment))
            {
                PostalConfirmationType confirmation = (PostalConfirmationType)shipment.Postal.Confirmation;

                // TODO: This comment was in here, but no supporting code for it.  Determine if it's still valid.
                // If the service type is Parcel Select, Force DC, otherwise USPS throws an error

                switch (confirmation)
                {
                    case PostalConfirmationType.Delivery:
                        addOns.Add(new AddOnV7 { AddOnType = AddOnTypeV7.USADC });
                        break;
                    case PostalConfirmationType.Signature:
                        addOns.Add(new AddOnV7 { AddOnType = AddOnTypeV7.USASC });
                        break;
                    case PostalConfirmationType.AdultSignatureRequired:
                        addOns.Add(new AddOnV7 { AddOnType = AddOnTypeV7.USAASR });
                        break;
                    case PostalConfirmationType.AdultSignatureRestricted:
                        addOns.Add(new AddOnV7 { AddOnType = AddOnTypeV7.USAASRD });
                        break;
                }
            }
            else if (PostalUtility.IsFreeInternationalDeliveryConfirmation(shipment.ShipCountryCode, serviceType, packagingType))
            {
                // Check for the new (as of 01/27/13) international delivery service.  In that case, we have to explicitly turn on DC
                addOns.Add(new AddOnV7 { AddOnType = AddOnTypeV7.USADC });
            }
            return addOns;
        }

        /// <summary>
        /// Determines if the shipto address supports confirmation addons
        /// </summary>
        private static bool SupportsConfirmation(ShipmentEntity shipment)
        {
            if (!shipment.ShipPerson.IsDomesticCountry())
            {
                return false;
            }

            PostalServiceType serviceType = (PostalServiceType)shipment.Postal.Service;

            // The following services support confirmation types.
            if ((new[] { PostalServiceType.DhlBpmExpedited, PostalServiceType.DhlBpmGround, PostalServiceType.DhlMarketingGround, PostalServiceType.DhlMarketingExpedited }).Contains(serviceType))
            {
                return true;
            }

            // Other DHL shipments don't support confirmation types
            if (ShipmentTypeManager.IsDhl(serviceType))
            {
                return false;
            }

            // Per USPS: Excludes Palau, Marshall Islands, and the Federated States of Micronesia
            List<string> notSupportedZips = new List<string> { "96939", "96940", "96941", "96942", "96943", "96944", "96960", "96970" };
            if (notSupportedZips.Contains(shipment.ShipPostalCode))
            {
                return false;
            }

            return true;
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
            customs.ContentType = UspsUtility.GetApiContentType((PostalCustomsContentType)shipment.Postal.CustomsContentType);
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
        /// Makes request to USPS API to change plan associated with the account referenced by the authenticator to be 
        /// an Expedited plan. This requires an authentication call to the USPS API prior to changing the plan.
        /// </summary>
        public void ChangeToExpeditedPlan(UspsAccountEntity account, string promoCode)
        {
            ExceptionWrapper(() =>
            {
                InternalChangeToExpeditedPlan(GetCredentials(account), promoCode);
                return true;
            }, account);
        }

        /// <summary>
        /// Makes request to USPS API to change plan associated with the account referenced by the authenticator to be 
        /// an Expedited plan. This requires an authentication call to the USPS API prior to changing the plan.
        /// </summary>
        private void InternalChangeToExpeditedPlan(Credentials credentials, string promoCode)
        {
            // Output parameters for web service call
            int transactionID;
            PurchaseStatus purchaseStatus;
            string rejectionReason = string.Empty;

            try
            {
                using (SwsimV49 webService = CreateWebService("ChangePlan"))
                {
                    webService.Url = ServiceUrl;

                    // We send 0 as the plan id
                    webService.ChangePlan(credentials, 0, promoCode, out purchaseStatus, out transactionID, out rejectionReason);
                }
            }
            catch (UspsException exception)
            {
                log.ErrorFormat("ShipWorks was unable to change the USPS plan. {0}. {1}", rejectionReason ?? string.Empty, exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Checks with USPS to get the contract type of the account.
        /// </summary>
        public UspsAccountContractType GetContractType(UspsAccountEntity account)
        {
            return ExceptionWrapper(() => InternalGetContractType(account), account);
        }

        /// <summary>
        /// The internal GetContractType implementation that is intended to be wrapped by the auth wrapper
        /// </summary>
        private UspsAccountContractType InternalGetContractType(UspsAccountEntity account)
        {
            UspsAccountContractType contract = UspsAccountContractType.Unknown;
            AccountInfo accountInfo;

            using (SwsimV49 webService = CreateWebService("GetContractType"))
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
                        contract = UspsAccountContractType.Commercial;
                        break;

                    case RatesetType.CPP:
                    case RatesetType.NSA:
                        contract = UspsAccountContractType.CommercialPlus;
                        break;

                    case RatesetType.STMP:
                        contract = UspsAccountContractType.Reseller;
                        break;

                    default:
                        contract = UspsAccountContractType.Unknown;
                        break;
                }
            }

            return contract;
        }

        /// <summary>
        /// Handles exceptions when making calls to the USPS API
        /// </summary>
        private T ExceptionWrapper<T>(Func<T> executor, UspsAccountEntity account)
        {
            try
            {
                try
                {
                    return executor();
                }
                catch (AggregateException ex)
                {
                    throw ex.InnerException;
                }
            }
            catch (SoapException ex)
            {
                log.ErrorFormat("Failed connecting to USPS.  Account: {0}, Error Code: '{1}', Exception Message: {2}", account.UspsAccountID, UspsApiException.GetErrorCode(ex), ex.Message);

                throw new UspsApiException(ex);
            }
            catch (InvalidOperationException ex)
            {
                // We had a client that was seeing this exception, so rather than crash, we should fail the operation and 
                if (ex.Message.Contains("Response is not well-formed XML") || ex.Message.Contains("error in XML document"))
                {
                    throw new UspsException(ex.Message, ex);
                }

                throw;
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(UspsException));
            }
        }

        /// <summary>
        /// Get the Credentials for the given account
        /// </summary>
        private static Credentials GetCredentials(UspsAccountEntity account)
        {
            return GetCredentials(account, false);
        }

        /// <summary>
        /// Get the Credentials for the given account
        /// </summary>
        private static Credentials GetCredentials(UspsAccountEntity account, bool emptyCredentialsIfAccountNull)
        {
            if (account == null && !emptyCredentialsIfAccountNull)
            {
                throw new ArgumentNullException("account");
            }

            return new Credentials
            {
                IntegrationID = integrationID,
                Username = account == null ? "" : account.Username,
                Password = account == null ? "" : SecureText.Decrypt(account.Password, account.Username)
            };
        }
    }
}
