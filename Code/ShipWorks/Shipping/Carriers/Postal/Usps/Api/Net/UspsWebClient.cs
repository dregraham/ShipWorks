﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Autofac;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.AddressValidation;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Tracking;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net
{
    /// <summary>
    /// Central point where API stuff goes through for USPS
    /// </summary>
    public class UspsWebClient : IUspsWebClient
    {
        // We don't include delivery confirmation because we want to treat that like None, because it is
        // included at no charge for services to which it applies.
        private readonly static IDictionary<PostalConfirmationType, AddOnTypeV16> confirmationLookup =
            new Dictionary<PostalConfirmationType, AddOnTypeV16>
            {
                { PostalConfirmationType.Signature, AddOnTypeV16.USASC },
                { PostalConfirmationType.AdultSignatureRequired, AddOnTypeV16.USAASR },
                { PostalConfirmationType.AdultSignatureRestricted, AddOnTypeV16.USAASRD }
            };

        // This value came from USPS (the "standard" account value is 88)
        private const int ExpeditedPlanID = 0;

        // These lengths come from the error that USPS's API gives us when we send data that is too long
        private const int MaxCustomsContentDescriptionLength = 20;
        private const int MaxCustomsSkuLength = 20;
        private const int MaxCustomsItemDescriptionLength = 60;

        private readonly ILog log;
        private readonly IUspsWebServiceFactory webServiceFactory;
        private readonly ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> accountRepository;
        private static readonly Guid integrationID = new Guid("F784C8BC-9CAD-4DAF-B320-6F9F86090032");

        // Cleansed address map so we don't do common addresses over and over again
        private static readonly Dictionary<PersonAdapter, Address> cleansedAddressMap = new Dictionary<PersonAdapter, Address>();

        private readonly ICertificateInspector certificateInspector;
        private readonly UspsResellerType uspsResellerType;

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsWebClient" /> class.
        /// </summary>
        /// <param name="uspsResellerType">Type of the USPS reseller.</param>
        public UspsWebClient(ILifetimeScope lifetimeScope, UspsResellerType uspsResellerType)
            : this(new UspsAccountRepository(),
                  lifetimeScope.Resolve<Owned<IUspsWebServiceFactory>>().Value,
                  new TrustingCertificateInspector(),
                  uspsResellerType)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsWebClient(ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> accountRepository,
            IUspsWebServiceFactory webServiceFactory, Func<string, ICertificateInspector> certificateInspectorFactory,
            UspsResellerType uspsResellerType)
            : this(accountRepository, webServiceFactory, certificateInspectorFactory(string.Empty), uspsResellerType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsWebClient" /> class.
        /// </summary>
        /// <param name="accountRepository">The account repository.</param>
        /// <param name="webServiceFactory">The web service factory.</param>
        /// <param name="certificateInspector">The certificate inspector.</param>
        /// <param name="uspsResellerType">Type of the USPS reseller.</param>
        public UspsWebClient(ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> accountRepository,
            IUspsWebServiceFactory webServiceFactory, ICertificateInspector certificateInspector,
            UspsResellerType uspsResellerType)
        {
            this.accountRepository = accountRepository;
            this.webServiceFactory = webServiceFactory;
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
        /// Create the web service instance with the appropriate URL
        /// </summary>
        private ISwsimV90 CreateWebService(string logName)
        {
            return CreateWebService(logName, LogActionType.Other);
        }

        /// <summary>
        /// Returns the current type of the web service.  Used to remove the namespace in exceptions received from Stamps.
        /// </summary>
        public static Type WebServiceType
        {
            get { return typeof(SwsimV90); }
        }

        /// <summary>
        /// Create the web service instance with the appropriate URL
        /// </summary>
        private ISwsimV90 CreateWebService(string logName, LogActionType logActionType) =>
           webServiceFactory.Create(logName, logActionType);

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

                using (ISwsimV90 webService = CreateWebService("Authenticate"))
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
#if !DEBUG
            CertificateRequest certificateRequest = new CertificateRequest(new Uri(url), certificateInspector);
            CertificateSecurityLevel certificateSecurityLevel = certificateRequest.Submit();

            if (certificateSecurityLevel != CertificateSecurityLevel.Trusted)
            {
                string description = EnumHelper.GetDescription(ShipmentTypeCode.Usps);
                throw new UspsException(string.Format("ShipWorks is unable to make a secure connection to {0}.", description));
            }
#endif
        }

        /// <summary>
        /// Get the account info for the given USPS user name
        /// </summary>
        public object GetAccountInfo(IUspsAccountEntity account)
        {
            return ExceptionWrapper(() => { return GetAccountInfoInternal(account); }, account);
        }

        /// <summary>
        /// The internal GetAccountInfo implementation that is intended to be wrapped by the exception wrapper
        /// </summary>
        private AccountInfoV41 GetAccountInfoInternal(IUspsAccountEntity account)
        {
            using (ISwsimV90 webService = CreateWebService("GetAccountInfo"))
            {
                return webService.GetAccountInfo(GetCredentials(account)).AccountInfo;
            }
        }

        /// <summary>
        /// Get the tracking result for the given shipment
        /// </summary>
        public TrackingResult TrackShipment(ShipmentEntity shipment)
        {
            UspsAccountEntity account = accountRepository.GetAccount(shipment.Postal.Usps.UspsAccountID);

            if (account == null)
            {
                // We weren't able to get the account, so the user must have deleted it.
                // Just try PostalWebTools instead.
                return new PostalWebShipmentType().TrackShipment(shipment);
            }

            try
            {
                return ExceptionWrapper(() => TrackShipmentInternal(shipment, account), account);
            }
            catch (UspsApiException ex)
            {
                throw new ShippingException("ShipWorks was unable to get tracking information.", ex);
            }
        }

        /// <summary>
        /// Get the tracking result for the given shipment
        /// </summary>
        private TrackingResult TrackShipmentInternal(ShipmentEntity shipment, UspsAccountEntity account)
        {
            TrackingResult result = new TrackingResult();
            TrackingEvent[] trackingEvents;
            DateTime? guaranteedDeliveryDate;
            DateTime? expectedDeliveryDate;
            string serviceDescription;
            string carrier;
            DestinationInfo destinationInfo;

            using (ISwsimV90 webService = CreateWebService("TrackShipment"))
            {
                webService.TrackShipment(GetCredentials(account), shipment.TrackingNumber,
                    out trackingEvents, out guaranteedDeliveryDate,
                    out expectedDeliveryDate, out serviceDescription, out carrier, out destinationInfo);
            }

            if (trackingEvents.Any())
            {
                foreach (TrackingEvent trackingEvent in trackingEvents)
                {
                    result.Details.Add(new TrackingResultDetail()
                    {
                        Date = trackingEvent.Timestamp.ToString("M/dd/yyy"),
                        Time = trackingEvent.Timestamp.ToString("h:mm tt"),
                        Activity = trackingEvent.Event,
                        Location = GetTrackEventLocation(trackingEvent)
                    });
                }

                result.Summary = trackingEvents.OrderBy(te => te.Timestamp).Last().Event;
            }

            return result;
        }

        /// <summary>
        /// Get descriptive location text for the given track event
        /// </summary>
        private string GetTrackEventLocation(TrackingEvent trackEvent)
        {
            var location = new[] { AddressCasing.Apply(trackEvent.City), trackEvent.State, Geography.GetCountryName(trackEvent.Country) };

            return string.Join(", ", location.Where(x => !string.IsNullOrEmpty(x)));
        }

        /// <summary>
        /// Populates a usps account entity.
        /// </summary>
        /// <param name="account">The account.</param>
        public void PopulateUspsAccountEntity(UspsAccountEntity account)
        {
            ExceptionWrapper(() => PopulateUspsAccountEntityInternal(account), account);
        }

        /// <summary>
        /// The internal PopulateUspsAccountEntity implementation that is intended to be wrapped by the exception wrapper
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns></returns>
        public UspsAccountEntity PopulateUspsAccountEntityInternal(UspsAccountEntity account)
        {
            using (ISwsimV90 webService = CreateWebService("GetAccountInfo"))
            {
                AccountInfoV41 accountInfo;
                // Address and CustomerEmail are not returned by Express1, so do not use them.
                Address address;
                string email;

                webService.GetAccountInfo(GetCredentials(account), out accountInfo, out address, out email, out string accountStatus, out DateAdvance dataAdvanceConfig, out string verificationPhoneNumber, out string verificationPhoneExtension);

                account.UspsAccountID = accountInfo.AccountId;
                account.Description = UspsAccountManager.GetDefaultDescription(account) ?? string.Empty;

                Address accountAddress = accountInfo.MeterPhysicalAddress ?? address;

                account.FirstName = accountAddress.FirstName ?? string.Empty;
                account.MiddleName = accountAddress.MiddleName ?? string.Empty;
                account.LastName = accountAddress.LastName ?? string.Empty;
                account.Company = accountAddress.Company ?? string.Empty;
                account.Street1 = accountAddress.Address1 ?? string.Empty;
                account.Street2 = accountAddress.Address2 ?? string.Empty;
                account.Street3 = accountAddress.Address3 ?? string.Empty;
                account.City = accountAddress.City ?? string.Empty;
                account.StateProvCode = Geography.GetStateProvCode(accountAddress.State) ?? string.Empty;

                account.PostalCode = accountAddress.ZIPCode ?? accountAddress.PostalCode ?? string.Empty;
                account.MailingPostalCode = accountAddress.ZIPCode ?? accountAddress.PostalCode ?? string.Empty;

                account.CountryCode = Geography.GetCountryCode(accountAddress.Country) ?? string.Empty;
                account.Phone = accountAddress.PhoneNumber ?? string.Empty;
                account.Email = email ?? string.Empty;
                account.Website = string.Empty;
                account.UspsReseller = (int) UspsResellerType.None;
                account.ContractType = (int) GetUspsAccountContractType(accountInfo.RatesetType);
                account.CreatedDate = DateTime.UtcNow;
                account.PendingInitialAccount = (int) UspsPendingAccountType.Existing;
                account.GlobalPostAvailability = (int) GetGlobalPostServiceAvailability(accountInfo.Capabilities);
            }

            return account;
        }

        /// <summary>
        /// Get GlobalPost service availability from the account info
        /// </summary>
        public static GlobalPostServiceAvailability GetGlobalPostServiceAvailability(CapabilitiesV27 capabilities)
        {
            return HasCapability(capabilities.CanPrintGP, GlobalPostServiceAvailability.GlobalPost) |
                HasCapability(capabilities.CanPrintGPSmartSaver, GlobalPostServiceAvailability.SmartSaver) |
                GetGlobalPostInternationalPresortAvailability(capabilities) |
                GetGlobalPostPlusAvailability(capabilities);
        }

        /// <summary>
        /// Get GlobalPostPlus
        /// </summary>
        private static GlobalPostServiceAvailability GetGlobalPostPlusAvailability(CapabilitiesV27 capabilities)
        {
            if (capabilities.CanPrintGPSmartSaver && capabilities.CanPrintGlobalPostPlus && capabilities.CanCleanseIntlAddress)
            {
                return GlobalPostServiceAvailability.GlobalPostPlus;
            }

            return GlobalPostServiceAvailability.None;
        }

        /// <summary>
        /// Get international presort availability for Global Post
        /// </summary>
        private static GlobalPostServiceAvailability GetGlobalPostInternationalPresortAvailability(CapabilitiesV18 capabilities)
        {
            if (!capabilities.CanPrintIntlPresortSinglePiece)
            {
                return GlobalPostServiceAvailability.None;
            }

            return HasCapability(capabilities.CanPrintFCIPresort, GlobalPostServiceAvailability.InternationalFirst) |
                HasCapability(capabilities.CanPrintPMIPresort, GlobalPostServiceAvailability.InternationalPriority) |
                HasCapability(capabilities.CanPrintPMEIPresort, GlobalPostServiceAvailability.InternationalExpress);
        }

        /// <summary>
        /// Gets a capability based on whether a user has it or not
        /// </summary>
        private static GlobalPostServiceAvailability HasCapability(bool hasCapability, GlobalPostServiceAvailability capability) =>
            hasCapability ? capability : GlobalPostServiceAvailability.None;

        /// <summary>
        /// Get the USPS URL of the given urlType
        /// </summary>
        public string GetUrl(IUspsAccountEntity account, UrlType urlType)
        {
            return ExceptionWrapper(() => { return GetUrlInternal(account, urlType); }, account);
        }

        /// <summary>
        /// The internal GetUrl implementation that is intended to be wrapped by the exception wrapper
        /// </summary>
        private string GetUrlInternal(IUspsAccountEntity account, UrlType urlType)
        {
            string url;

            using (ISwsimV90 webService = CreateWebService("GetURL"))
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
            PurchaseRejectionCode? purchaseRejectionCode_Unused;

            using (ISwsimV90 webService = CreateWebService("PurchasePostage"))
            {
                webService.PurchasePostage(
                    GetCredentials(account),
                    amount,
                    controlTotal,
                    null, // MI
                    null, // IntegratorTxID
                    null, // SendEmail
                    false, //SendEmailSpecified
                    out purchaseStatus,
                    out transactionID,
                    out postageBalance,
                    out rejectionReason,
                    out miRequired_Unused,
                    out purchaseRejectionCode_Unused);
            }

            if (purchaseStatus == PurchaseStatus.Rejected)
            {
                throw new UspsException(rejectionReason);
            }
        }

        /// <summary>
        /// Get the rates for the given shipment based on its settings
        /// </summary>
        public (IEnumerable<RateResult> rates, IEnumerable<Exception> errors) GetRates(ShipmentEntity shipment)
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
                return ExceptionWrapper(() => GetRatesInternal(shipment, account), account)
                    .Select(uspsRate => UspsUtility.GetPostalServiceType(uspsRate.ServiceType)
                        .Map(x => BuildRateResult(shipment, account, uspsRate, x)))
                    .Aggregate((rates: Enumerable.Empty<RateResult>(), errors: Enumerable.Empty<Exception>()),
                        (accumulator, x) => x.Match(
                                rate => (accumulator.rates.Append(rate), accumulator.errors),
                                ex => (accumulator.rates, accumulator.errors.Append(ex))));
            }
            catch (UspsApiException ex)
            {
                if (ex.Message.ToUpperInvariant().Contains("THE USERNAME OR PASSWORD ENTERED IS NOT CORRECT"))
                {
                    // Provide a little more context as to which user name/password was incorrect in the case
                    // where there's multiple accounts or Express1 for USPS is being used to compare rates
                    string message = string.Format("ShipWorks was unable to connect to {0} with account {1}.{2}{2}Check that your account credentials are correct.",
                                    UspsAccountManager.GetResellerName((UspsResellerType) account.UspsReseller),
                                    account.Username,
                                    Environment.NewLine);

                    throw new UspsException(message, ex);
                }

                // This isn't an authentication exception, so just throw the original exception
                throw;
            }
        }

        /// <summary>
        /// Build a RateResult from a USPS rate
        /// </summary>
        private static RateResult BuildRateResult(ShipmentEntity shipment, UspsAccountEntity account, RateV33 uspsRate, PostalServiceType serviceType)
        {
            var (description, amount) = GetRateAddOnDetails((PostalConfirmationType) shipment.Postal.Confirmation, uspsRate.AddOns);

            var baseRate = new RateResult(
                PostalUtility.GetPostalServiceTypeDescription(serviceType) + description,
                PostalUtility.GetDaysForRate(uspsRate.DeliverDays, uspsRate.DeliveryDate),
                uspsRate.Amount + amount,
                new UspsPostalRateSelection(serviceType, account))
            {
                ProviderLogo = EnumHelper.GetImage((ShipmentTypeCode) shipment.ShipmentType)
            };

            return PostalUtility.SetServiceDetails(baseRate, serviceType, uspsRate.DeliverDays, ShipmentTypeCode.Usps);
        }

        /// <summary>
        /// Get details for required rate addons
        /// </summary>
        private static (string description, decimal amount) GetRateAddOnDetails(PostalConfirmationType confirmation, IEnumerable<AddOnV16> addOns) =>
            addOns
                .Where(x => confirmationLookup.TryGetValue(confirmation, out AddOnTypeV16 type) && type == x.AddOnType)
                .Select(x => (description: " (" + EnumHelper.GetDescription(confirmation) + ")", amount: x.Amount))
                .FirstOrDefault();

        /// <summary>
        /// The internal GetRates implementation intended to be wrapped by the exception wrapper
        /// </summary>
        private IEnumerable<RateV33> GetRatesInternal(ShipmentEntity shipment, UspsAccountEntity account)
        {
            RateV33 rate = CreateRateForRating(shipment, account);

            RateV33[] rateResults;

            using (ISwsimV90 webService = CreateWebService("GetRates", LogActionType.GetRates))
            {
                CheckCertificate(webService.Url);
                rateResults = webService.GetRates(GetCredentials(account), rate);
            }

            List<RateV33> noConfirmationServiceRates = new List<RateV33>();

            // If its a "Flat" then FirstClass and Priority can't have a confirmation
            PostalPackagingType packagingType = (PostalPackagingType) shipment.Postal.PackagingType;
            if (packagingType == PostalPackagingType.Envelope || packagingType == PostalPackagingType.LargeEnvelope)
            {
                noConfirmationServiceRates.AddRange(rateResults.Where(r => r.ServiceType == ServiceType.USFC || r.ServiceType == ServiceType.USPM));
            }

            // Remove the Delivery and Signature add ons from all those that shouldn't support it
            foreach (RateV33 noConfirmationServiceRate in noConfirmationServiceRates)
            {
                if (noConfirmationServiceRate != null && noConfirmationServiceRate.AddOns != null)
                {
                    noConfirmationServiceRate.AddOns = noConfirmationServiceRate.AddOns.Where(a => a.AddOnType != AddOnTypeV16.USASC && a.AddOnType != AddOnTypeV16.USADC).ToArray();
                }
            }

            // remove services that are unknown
            return rateResults.Where(r => r.ServiceType != ServiceType.Unknown);
        }

        /// <summary>
        /// Cleans the address of the given person using the specified USPS account
        /// </summary>
        private Task<Address> CleanseAddress(UspsAccountEntity account, PersonAdapter person, bool requireFullMatch)
        {
            return ExceptionWrapperAsync(() => CleanseAddressInternal(person, account, requireFullMatch), account);
        }

        /// <summary>
        /// Internal CleanseAddress implementation intended to be wrapped by the auth wrapper
        /// </summary>
        private async Task<Address> CleanseAddressInternal(PersonAdapter person, UspsAccountEntity account, bool requireFullMatch)
        {
            if (cleansedAddressMap.TryGetValue(person, out Address address))
            {
                return address;
            }

            UspsAddressValidationResults results = await ValidateAddressAsync(person, account).ConfigureAwait(false);

            if (!results.IsSuccessfulMatch)
            {
                if (!results.IsCityStateZipOk)
                {
                    throw new UspsException($"The address for '{new PersonName(person).FullName}' is not a valid address. {Environment.NewLine}{Environment.NewLine}" +
                        $"Address validation returned the following error: {Environment.NewLine}" +
                        $"{results.BadAddressMessage}");
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
        /// Validates the address of the given person using the specified stamps account
        /// </summary>
        public async Task<UspsAddressValidationResults> ValidateAddressAsync(PersonAdapter person, UspsAccountEntity account)
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
                result = await ActionRetry.ExecuteWithRetry<InvalidOperationException, CleanseAddressCompletedEventArgs>(2, () => CleanseAddressAsync(account, address)).ConfigureAwait(false);
            }
            catch (AggregateException ex)
            {
                SoapException soapException = ex.InnerExceptions.OfType<SoapException>().FirstOrDefault();
                if (soapException != null)
                {
                    return BuildSoapSchemaExceptionMessage(ex, log);
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
                return BuildSoapSchemaExceptionMessage(ex, log);
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
                MatchedAddress = result.Address,
                Candidates = result.CandidateAddresses,
                BadAddressMessage = badAddressMessage,
                StatusCodes = result.StatusCodes,
                VerificationLevel = result.VerificationLevel,
                AddressCleansingResult = result.AddressCleansingResult
            };
        }

        /// <summary>
        /// Cleanse an address asynchronously
        /// </summary>
        private Task<CleanseAddressCompletedEventArgs> CleanseAddressAsync(UspsAccountEntity account, Address address)
        {
            using (ISwsimV90 webService = CreateWebService("CleanseAddress", LogActionType.ExtendedLogging))
            {
                CheckCertificate(webService.Url);

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
                    return taskCompletion.Task;
                }
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

                using (ISwsimV90 webService = CreateWebService("RegisterAccount"))
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
                            (object) registration.CreditCard ?? registration.AchAccount,
                            null,  // SendEmail
                            false, // SendEmailSpecified
                            false, // ResetPasswordAfterRegistration
                            true, // ResetPasswordAfterRegistrationSpecified
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
            List<string> trackingNumbers = uspsShipmentEntities.Select(s => s.PostalShipment.Shipment.TrackingNumber).ToList();
            PersonAdapter person = new PersonAdapter(uspsAccountEntity, string.Empty);
            Carrier carrier = GetScanFormCarrier(uspsShipmentEntities.ToList());

            string transactionID = Guid.NewGuid().ToString();

            EndOfDayManifest[] endOfDayManifests;

            using (ISwsimV90 webService = CreateWebService("ScanForm"))
            {
                webService.CreateManifest
                    (
                        GetCredentials(uspsAccountEntity),
                        ref transactionID,
                        uspsTransactions.ToArray(),
                        null, // TrackingNumbers (not needed because we send the uspsTransactions)
                        null, // ShipDate
                        false, // ShipDateSpecified
                        null, // PrintLayout
                        CreateScanFormAddress(person),
                        ImageType.Png,
                        false, // Don't print instructions
                        ManifestType.All,
                        0, // NumberOfLabels (wsdl shows it as optional and default is 0)
                        out endOfDayManifests
                    );
            }

            StringBuilder innerXml = new StringBuilder();
            foreach (var item in endOfDayManifests)
            {
                innerXml.AppendLine($"<TransactionId>{item.ManifestId}</TransactionId><Url>{item.ManifestUrl}</Url>");
            }
            string responseXml = $"<ScanForm>{innerXml.ToString()}</ScanForm>";

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
            if (shipments.All(s => ShipmentTypeManager.IsDhl((PostalServiceType) s.PostalShipment.Service)))
            {
                return Carrier.DHLExpress;
            }

            if (shipments.All(s => !ShipmentTypeManager.IsDhl((PostalServiceType) s.PostalShipment.Service)))
            {
                return Carrier.USPS;
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

            ExceptionWrapper(() => { VoidShipmentInternal(account, shipment.Postal.Usps.UspsTransactionID); return true; }, account);
        }

        /// <summary>
        /// The internal VoidShipment implementation intended to be wrapped by the exception wrapper
        /// </summary>
        protected void VoidShipmentInternal(UspsAccountEntity account, Guid uspsTransactionID)
        {
            using (ISwsimV90 webService = CreateWebService("Void"))
            {
                webService.CancelIndicium(
                    GetCredentials(account),
                    uspsTransactionID,
                    null, // SendEmail
                    false); // SendsEmailSpecified
            }
        }

        /// <summary>
        /// Process the given shipment, downloading label images and tracking information
        /// </summary>
        public async Task<TelemetricResult<StampsLabelResponse>> ProcessShipment(ShipmentEntity shipment)
        {
            UspsAccountEntity account = accountRepository.GetAccount(shipment.Postal.Usps.UspsAccountID);
            if (account == null)
            {
                throw new UspsException("No USPS account is selected for the shipment.");
            }

            try
            {
                // Each request needs to get a new requestID.  If USPS sees a duplicate, it thinks its the same request.
                // So if you had an error (like weight was too much) and then changed the weight and resubmitted, it would still
                // be in error if you used the same ID again.
                shipment.Postal.Usps.IntegratorTransactionID = Guid.NewGuid();

                bool envelope = shipment.Postal.PackagingType == (int) PostalPackagingType.Envelope &&
                    shipment.Postal.Service != (int) PostalServiceType.InternationalFirst;

                return await ExceptionWrapperAsync(() => ProcessShipmentInternal(shipment, account, shipment.Postal.Usps.RequireFullAddressValidation, shipment.Postal.Usps.IntegratorTransactionID, envelope), account).ConfigureAwait(false);

            }
            catch (UspsApiException ex)
            {
                TranslateProcessShipmentException(account, ex);

                // This isn't an exception we can handle, so just throw the original exception
                throw;
            }
        }

        /// <summary>
        /// Translate an exception caught while processing into something more usable if possible
        /// </summary>
        private void TranslateProcessShipmentException(UspsAccountEntity account, UspsApiException ex)
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
        }

        /// <summary>
        /// The internal ProcessShipment implementation intended to be wrapped by the exception wrapper
        /// </summary>
        protected async Task<TelemetricResult<StampsLabelResponse>> ProcessShipmentInternal(
            ShipmentEntity shipment,
            UspsAccountEntity account, 
            bool requireFullAddressValidation, 
            Guid integratorTransactionID, 
            bool createEnvelopeRequest = false)
        {
            TelemetricResult<StampsLabelResponse> telemetricResult = new TelemetricResult<StampsLabelResponse>(TelemetricResultBaseName.ApiResponseTimeInMilliseconds);
            (Address toAddress, Address fromAddress) = await FixWebserviceAddresses(account, shipment, telemetricResult, requireFullAddressValidation).ConfigureAwait(false);

            RateV33 rate = CreateRateForProcessing(shipment, account);
            CustomsV5 customs = CreateCustoms(shipment);

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

            CreateIndiciumResult result = null;

            using (ISwsimV90 webService = CreateWebService("Process"))
            {
                if (createEnvelopeRequest)
                {
                    // Envelopes don't support thermal
                    thermalType = null;

                    // Always use the personal envelope layout to generate the envelope label
                    rate.PrintLayout = "EnvelopePersonal";

                    // A separate service call is used for processing envelope according to USPS as of v. 22
                    telemetricResult.RunTimedEvent(TelemetricEventType.GetLabel, () =>
                        result = webService.CreateEnvelopeIndicium(
                            new CreateEnvelopeIndiciumParameters
                            {
                                Item = GetCredentials(account),
                                IntegratorTxID = integratorTransactionID.ToString(),
                                Rate = rate,
                                From = fromAddress,
                                To = toAddress,
                                CustomerID = null,
                                Mode = CreateIndiciumModeV1.Normal,
                                ImageType = ImageType.Png,
                                CostCodeId = 0, // cost code ID
                                HideFIM = false, // do not hide the facing identification mark (FIM)
                                RateToken = null, // RateToken
                                OrderId = null
                            }));
                }
                else
                {
                    // Labels for all other package types other than envelope get created via the CreateIndicium method
                    telemetricResult.RunTimedEvent(TelemetricEventType.GetLabel, () =>
                        result = webService.CreateIndicium(
                            new CreateIndiciumParameters
                            {
                                Item = GetCredentials(account),
                                IntegratorTxID = integratorTransactionID.ToString(),
                                TrackingNumber = string.Empty,
                                Rate = rate,
                                From = fromAddress,
                                To = toAddress,
                                CustomerID = null,
                                Customs = customs,
                                SampleOnly = false, // Sample only,
                                PostageMode = GetPostageMode(shipment),
                                ImageType = thermalType == null
                                    ? ImageType.Png
                                    : ((thermalType == ThermalLanguage.EPL) ? ImageType.Epl : ImageType.Zpl),
                                EltronPrinterDPIType = EltronPrinterDPIType.Default,
                                Memo = GetMemoText(shipment), // Memo
                                CostCodeId = 0, // Cost Code
                                DeliveryNotification = false, // delivery notify
                                ShipmentNotification = null, // shipment notification
                                RotationDegrees = 0, // Rotation
                                HorizontalOffset = null,
                                HorizontalOffsetSpecified = false, // horizontal offset
                                VerticalOffset = null,
                                VerticalOffsetSpecified = false, // vertical offset
                                PrintDensity = null,
                                PrintDensitySpecified = false, // print density
                                PrintMemo = null,
                                PrintMemoSpecified = false, // print memo
                                PrintInstructions = false,
                                PrintInstructionsSpecified = true, // print instructions
                                RequestPostageHash = false, // request postage hash
                                NonDeliveryOption = NonDeliveryOption.Return, // return to sender
                                RedirectTo = null, // redirectTo
                                OriginalPostageHash = null, // OriginalPostageHash
                                ReturnImageData = true,
                                ReturnImageDataSpecified = true, // returnImageData,
                                InternalTransactionNumber = null,
                                PaperSize = PaperSizeV1.Default,
                                EmailLabelTo = null,
                                PayOnPrint = false, // PayOnPrint
                                ReturnLabelExpirationDays = null, // ReturnLabelExpirationDays
                                ReturnLabelExpirationDaysSpecified = false, // ReturnLabelExpirationDaysSpecified,
                                ImageDpi = ImageDpi.ImageDpi203, // ImageDpi
                                RateToken = null, // RateToken
                                OrderId = null, // OrderId
                            }));
                }
            }

            SaveLabelInformation(shipment, result, thermalType);

            StampsLabelResponse stampsLabelResponse = new StampsLabelResponse
            {
                Shipment = shipment,
                ImageData = result.ImageData,
                LabelUrl = result.URL
            };
            telemetricResult.SetValue(stampsLabelResponse);
            return telemetricResult;
        }

        /// <summary>
        /// Get memo text for the given shipment
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        protected virtual string GetMemoText(ShipmentEntity shipment) => 
            UspsUtility.BuildMemoField(shipment.Postal);

        /// <summary>
        /// Get the postage mode for the given shipment
        /// </summary>
        protected virtual PostageMode GetPostageMode(ShipmentEntity shipment) =>
            shipment.Postal.NoPostage ? PostageMode.NoPostage : PostageMode.Normal;

        /// <summary>
        /// Save the label information to the shipment
        /// </summary>
        protected virtual void SaveLabelInformation(ShipmentEntity shipment, CreateIndiciumResult result, ThermalLanguage? thermalType)
        {
            shipment.TrackingNumber = result.TrackingNumber;
            shipment.ShipmentCost = result.ShipmentCost;
            shipment.Postal.Usps.UspsTransactionID = result.StampsTxID;
            shipment.BilledWeight = result.Rate.EffectiveWeightInOunces / 16D;

            // Set the thermal type for the shipment
            shipment.ActualLabelFormat = (int?) thermalType;
        }

        /// <summary>
        /// Updates addresses based on shipment properties like ReturnShipment, etc
        /// </summary>
        private async Task<(Address to, Address from)> FixWebserviceAddresses(UspsAccountEntity account,
            ShipmentEntity shipment,
            TelemetricResult<StampsLabelResponse> telemetricResult,
            bool requireFullAddressValidation)
        {
            Address toAddress = null;
            Address fromAddress;

            // If this is a return shipment, swap the to/from addresses
            if (shipment.ReturnShipment)
            {
                toAddress = await telemetricResult.RunTimedEventAsync(
                        TelemetricEventType.CleanseAddress,
                        () => CleanseAddress(account, shipment.OriginPerson, false))
                    .ConfigureAwait(false);
                fromAddress = CreateAddress(shipment.ShipPerson);
            }
            else
            {
                fromAddress = CreateAddress(shipment.OriginPerson);
                toAddress = await telemetricResult.RunTimedEventAsync(
                        TelemetricEventType.CleanseAddress,
                        () => CleanseAddress(account, shipment.ShipPerson, requireFullAddressValidation))
                    .ConfigureAwait(false);
            }

            if (shipment.ReturnShipment && !(toAddress.AsAddressAdapter().IsDomesticCountry() && fromAddress.AsAddressAdapter().IsDomesticCountry()))
            {
                throw new UspsException("Return shipping labels can only be used to send packages to and from domestic addresses.");
            }

            // Per stamps - only send state for domestic - send province for international
            if (!toAddress.AsAddressAdapter().IsDomesticCountry())
            {
                // Only overwrite the Province if the State is not blank
                if (!string.IsNullOrWhiteSpace(toAddress.State))
                {
                    toAddress.Province = toAddress.State;
                    toAddress.State = string.Empty;
                }
            }

            return (toAddress, fromAddress);
        }

        /// <summary>
        /// Gets the thermal language based on the shipment type and the requested label format on the shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>The ThermalLanguage value.</returns>
        private static ThermalLanguage? GetThermalLanguage(ShipmentEntity shipment) =>
            shipment.RequestedLabelFormat == (int) ThermalLanguage.None ? null : (ThermalLanguage?) shipment.RequestedLabelFormat;

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

            address.EmailAddress = person.Email;

            return address;
        }

        /// <summary>
        /// Create a Rate object used as the rate info for the GetRates method
        /// </summary>
        protected virtual RateV33 CreateRateForRating(ShipmentEntity shipment, UspsAccountEntity account)
        {
            RateV33 rate = new RateV33();

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

            rate.PackageType = UspsUtility.GetApiPackageType((PostalPackagingType) shipment.Postal.PackagingType, new DimensionsAdapter(shipment.Postal));
            rate.NonMachinable = shipment.Postal.NonMachinable;

            rate.Length = shipment.Postal.DimsLength;
            rate.Width = shipment.Postal.DimsWidth;
            rate.Height = shipment.Postal.DimsHeight;

            rate.ShipDate = shipment.ShipDate;
            rate.DeclaredValue = shipment.CustomsValue;

            if (CustomsManager.IsCustomsRequired(shipment))
            {
                rate.ContentTypeSpecified = true;
                rate.ContentType = UspsUtility.GetApiContentType((PostalCustomsContentType) shipment.Postal.CustomsContentType);
            }

            return rate;
        }

        /// <summary>
        /// Create the rate object for the given shipment
        /// </summary>
        protected virtual RateV33 CreateRateForProcessing(ShipmentEntity shipment, UspsAccountEntity account)
        {
            PostalServiceType serviceType = (PostalServiceType) shipment.Postal.Service;
            PostalPackagingType packagingType = (PostalPackagingType) shipment.Postal.PackagingType;

            RateV33 rate = CreateRateForRating(shipment, account);
            rate.ServiceType = UspsUtility.GetApiServiceType(serviceType);
            rate.PrintLayout = "Normal4X6";

            // Get the confirmation type add ons
            List<AddOnV16> addOns = AddConfirmationTypeAddOnsForProcessing(shipment, serviceType, packagingType);

            // For express, apply the signature waiver if necessary
            if (serviceType == PostalServiceType.ExpressMail)
            {
                if (!shipment.Postal.ExpressSignatureWaiver)
                {
                    addOns.Add(new AddOnV16 { AddOnType = AddOnTypeV16.USASR });
                }
            }

            // Add in the hidden postage option (but not supported for envelopes)
            if (shipment.Postal.Usps.HidePostage && shipment.Postal.PackagingType != (int) PostalPackagingType.Envelope)
            {
                addOns.Add(new AddOnV16 { AddOnType = AddOnTypeV16.SCAHP });
            }

            // Add insurance if using SDC insurance
            if (shipment.Insurance && shipment.InsuranceProvider == (int) InsuranceProvider.Carrier)
            {
                rate.InsuredValue = shipment.Postal.InsuranceValue;
                addOns.Add(new AddOnV16 { AddOnType = AddOnTypeV16.SCAINS });
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
        private static List<AddOnV16> AddConfirmationTypeAddOnsForProcessing(ShipmentEntity shipment, PostalServiceType serviceType, PostalPackagingType packagingType)
        {
            List<AddOnV16> addOns = new List<AddOnV16>();

            // For domestic, add in Delivery\Signature confirmation; delivery confirmation is not allowed on DHL services
            if (SupportsConfirmation(shipment))
            {
                PostalConfirmationType confirmation = (PostalConfirmationType) shipment.Postal.Confirmation;

                // TODO: This comment was in here, but no supporting code for it.  Determine if it's still valid.
                // If the service type is Parcel Select, Force DC, otherwise USPS throws an error

                switch (confirmation)
                {
                    case PostalConfirmationType.Delivery:
                        addOns.Add(new AddOnV16 { AddOnType = AddOnTypeV16.USADC });
                        break;
                    case PostalConfirmationType.Signature:
                        addOns.Add(new AddOnV16 { AddOnType = AddOnTypeV16.USASC });
                        break;
                    case PostalConfirmationType.AdultSignatureRequired:
                        addOns.Add(new AddOnV16 { AddOnType = AddOnTypeV16.USAASR });
                        break;
                    case PostalConfirmationType.AdultSignatureRestricted:
                        addOns.Add(new AddOnV16 { AddOnType = AddOnTypeV16.USAASRD });
                        break;
                }
            }
            else if (((PostalShipmentType) ShipmentTypeManager.GetType(shipment)).IsFreeInternationalDeliveryConfirmation(shipment.ShipCountryCode, serviceType, packagingType))
            {
                // Check for the new (as of 01/27/13) international delivery service.  In that case, we have to explicitly turn on DC
                addOns.Add(new AddOnV16 { AddOnType = AddOnTypeV16.USADC });
            }
            return addOns;
        }

        /// <summary>
        /// Determines if the ship-to address supports confirmation addons
        /// </summary>
        private static bool SupportsConfirmation(ShipmentEntity shipment)
        {
            if (!shipment.ShipPerson.IsDomesticCountry())
            {
                return false;
            }

            PostalServiceType serviceType = (PostalServiceType) shipment.Postal.Service;

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
        protected virtual CustomsV5 CreateCustoms(ShipmentEntity shipment)
        {
            if (!CustomsManager.IsCustomsRequired(shipment))
            {
                return null;
            }

            CustomsV5 customs = new CustomsV5();

            // Content type
            customs.ContentType = UspsUtility.GetApiContentType((PostalCustomsContentType) shipment.Postal.CustomsContentType);
            if (customs.ContentType == ContentTypeV2.Other)
            {
                if (shipment.Postal.CustomsContentType == (int) PostalCustomsContentType.Merchandise)
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

                line.sku = customsItem.Description.Truncate(MaxCustomsSkuLength);

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
                using (ISwsimV90 webService = CreateWebService("ChangePlan"))
                {
                    // We send 0 as the plan id
                    webService.ChangePlan(
                        credentials,
                        0,
                        promoCode,
                        null, // ChangeEmail,
                        false, // ChangeEmailSpecified
                        out purchaseStatus,
                        out transactionID,
                        out rejectionReason);
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
            using (ISwsimV90 webService = CreateWebService("GetContractType"))
            {
                CheckCertificate(webService.Url);

                AccountInfoResult result = webService.GetAccountInfo(GetCredentials(account));

                return GetUspsAccountContractType(result.AccountInfo?.RatesetType);
            }
        }

        /// <summary>
        /// Gets the UspsAccountContractType enum value
        /// </summary>
        /// <param name="rateset">The rateset.</param>
        /// <returns></returns>
        private UspsAccountContractType GetUspsAccountContractType(RatesetType? rateset)
        {
            UspsAccountContractType contract = UspsAccountContractType.Unknown;

            if (!rateset.HasValue)
            {
                return contract;
            }

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

            return contract;
        }

        /// <summary>
        /// Handles exceptions when making calls to the USPS API
        /// </summary>
        protected async Task<T> ExceptionWrapperAsync<T>(Func<Task<T>> executor, UspsAccountEntity account)
        {
            try
            {
                return await executor().ConfigureAwait(false);
            }
            catch (SoapException ex)
            {
                log.ErrorFormat("Failed connecting to USPS.  Account: {0}, Error Code: '{1}', Exception Message: {2}",
                    account.UspsAccountID, UspsApiException.GetErrorCode(ex), ex.Message);

                throw new UspsApiException(ex);
            }
            catch (WebException ex)
            {
                if (ex.Message.Contains("Unable to connect to the remote server") ||
                    ex.Message.Contains("The underlying connection was closed") ||
                    ex.Message.Contains("Bad gateway"))
                {
                    throw new UspsException("ShipWorks is unable to connect to USPS.");
                }

                throw WebHelper.TranslateWebException(ex, typeof(UspsException));
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
        /// Handles exceptions when making calls to the USPS API
        /// </summary>
        private T ExceptionWrapper<T>(Func<T> executor, IUspsAccountEntity account)
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
                log.ErrorFormat("Failed connecting to USPS.  Account: {0}, Error Code: '{1}', Exception Message: {2}",
                    account.UspsAccountID, UspsApiException.GetErrorCode(ex), ex.Message);

                throw new UspsApiException(ex);
            }
            catch (WebException ex)
            {
                if (ex.Message.Contains("Unable to connect to the remote server") ||
                    ex.Message.Contains("The underlying connection was closed") ||
                    ex.Message.Contains("Bad gateway"))
                {
                    throw new UspsException("ShipWorks is unable to connect to USPS.");
                }

                throw WebHelper.TranslateWebException(ex, typeof(UspsException));
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
        private static Credentials GetCredentials(IUspsAccountEntity account) => GetCredentials(account, false);

        /// <summary>
        /// Get the Credentials for the given account
        /// </summary>
        private static Credentials GetCredentials(IUspsAccountEntity account, bool emptyCredentialsIfAccountNull)
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

        /// <summary>
        /// Build a soap schema exception message
        /// </summary>
        private static UspsAddressValidationResults BuildSoapSchemaExceptionMessage(Exception ex, ILog log)
        {
            log.Error(ex);

            // Re-throw the exception, but filter out namespaces and information that isn't useful to customers
            string badAddressMessage = ex.Message.Replace("Invalid SOAP message due to XML Schema validation failure. ", string.Empty);
            badAddressMessage = Regex.Replace(badAddressMessage, @"http://stamps.com/xml/namespace/\d{4}/\d{1,2}/swsim/swsimv\d*:", string.Empty);

            return new UspsAddressValidationResults()
            {
                IsSuccessfulMatch = false,
                BadAddressMessage = badAddressMessage,
                Candidates = new List<Address>()
            };
        }

        /// <summary>
        /// Add DHL Express to the given Stamps.com account
        /// </summary>
        public string AddDhlExpress(IUspsAccountEntity account) =>
            ExceptionWrapper(() => AddDhlExpressInternal(account), account);

        /// <summary>
        /// The internal AddDhlExpress implemenation that is intended to be wrapped by the exception wrapper
        /// </summary>
        private string AddDhlExpressInternal(IUspsAccountEntity account)
        {
            using (ISwsimV90 webService = CreateWebService("AddCarrier"))
            {
                return webService.AddCarrier(GetCredentials(account), false, Carrier.DHLExpress, string.Empty, string.Empty, string.Empty, null, false, null, false);
            }
        }

        /// <summary>
        /// Set automatic funding settings
        /// </summary>
        public string SetAutoBuy(IUspsAccountEntity account, AutoBuySettings autoBuySettings) =>
            ExceptionWrapper(() => SetAutoBuyInternal(account, autoBuySettings), account);


        /// <summary>
        /// The internal SetAutoBuy implementation that is intended to be wrapped by the exception wrapper
        /// </summary>
        private string SetAutoBuyInternal(IUspsAccountEntity account, AutoBuySettings autoBuySettings)
        {
            using (ISwsimV90 webService = CreateWebService("SetAutoBuy"))
            {
                return webService.SetAutoBuy(GetCredentials(account), autoBuySettings);
            }
        }
    }
}
