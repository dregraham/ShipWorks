﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Interapptive.Shared.Business;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Editions;
using ShipWorks.Editions.Freemium;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Account;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Endicia.WebServices.LabelService;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Wraps access to the Endicia API
    /// </summary>
    public class EndiciaApiClient : IEndiciaApiClient
    {
        // We don't include delivery confirmation because we want to treat that like None, because it is
        // included at no charge for services to which it applies.
        private readonly static IDictionary<PostalConfirmationType, Func<PostagePrice, decimal>> confirmationLookup =
            new Dictionary<PostalConfirmationType, Func<PostagePrice, decimal>>
            {
                { PostalConfirmationType.Signature, x => x.Fees.SignatureConfirmation},
                { PostalConfirmationType.AdultSignatureRequired, x => x.Fees.AdultSignature },
                { PostalConfirmationType.AdultSignatureRestricted, x => x.Fees.AdultSignatureRestrictedDelivery }
            };

        private readonly ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity> accountRepository;
        private readonly ILogEntryFactory logEntryFactory;
        private readonly ICertificateInspector certificateInspector;
        private readonly ILog log = LogManager.GetLogger(typeof(EndiciaApiClient));

        private const string productionUrl = "https://LabelServer.Endicia.com/LabelService/EwsLabelService.asmx";

        private const string standardEndiciaPartnerID = "lswk";
        private const string freemiumEndiciaPartnerID = "lseb";
        private const string TestEndiciaPartnerID = "lxxx";

        /// <summary>
        /// Initializes a new instance of the <see cref="EndiciaApiClient"/> class.
        /// </summary>
        public EndiciaApiClient()
            : this(new EndiciaAccountRepository(), new LogEntryFactory(), new TrustingCertificateInspector())
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EndiciaApiClient"/> class.
        /// </summary>
        /// <param name="accountRepository">The account repository.</param>
        /// <param name="logEntryFactory">The log entry factory.</param>
        /// <param name="certificateInspector">The certificate inspector.</param>
        public EndiciaApiClient(ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity> accountRepository, ILogEntryFactory logEntryFactory, ICertificateInspector certificateInspector)
        {
            this.accountRepository = accountRepository;
            this.logEntryFactory = logEntryFactory;
            this.certificateInspector = certificateInspector;
        }

        /// <summary>
        /// Indicates if the test server should be used instead of the live server
        /// </summary>
        public static bool UseTestServer
        {
            get
            {
                return InterapptiveOnly.Registry.GetValue("EndiciaTestServer", false);
            }
            set
            {
                InterapptiveOnly.Registry.SetValue("EndiciaTestServer", value);
            }
        }

        /// <summary>
        /// If set to use a test server, this is the URL for the test server to use.
        /// </summary>
        public static EndiciaTestServer UseTestServerUrl
        {
            get
            {
                int useTestServerUrl = InterapptiveOnly.Registry.GetValue("EndiciaUseTestServerUrl", (int) EndiciaTestServer.Envmgr);

                // Make sure it's a valid enum.  If not, default to old test server.
                if (!Enum.IsDefined(typeof(EndiciaTestServer), useTestServerUrl))
                {
                    useTestServerUrl = (int) EndiciaTestServer.Envmgr;
                }

                return (EndiciaTestServer) useTestServerUrl;
            }
            set
            {
                InterapptiveOnly.Registry.SetValue("EndiciaUseTestServerUrl", (int) value);
            }
        }

        /// <summary>
        /// The PartnerID ShipWorks is using to communicate with Endicia as.
        /// </summary>
        public static string GetInterapptivePartnerID(EndiciaReseller reseller)
        {
            switch (reseller)
            {
                case EndiciaReseller.Express1:
                {
                    return Express1EndiciaUtility.ApiKey;
                }

                case EndiciaReseller.None:
                default:
                {
                    if (UseTestServer)
                    {
                        return TestEndiciaPartnerID;
                    }

                    if (FreemiumFreeEdition.IsActive)
                    {
                        return freemiumEndiciaPartnerID;
                    }

                    // non-freemium, and freemium-paid use our standard partnerID
                    return standardEndiciaPartnerID;
                }
            }
        }

        /// <summary>
        /// Gets the PartnerID for ShipWorks to use when communicating with Endicia, based on
        /// the type of Endicia account
        /// </summary>
        public static string GetInterapptivePartnerID(EndiciaAccountType accountType)
        {
            if (accountType == EndiciaAccountType.Freemium)
            {
                return freemiumEndiciaPartnerID;
            }
            else
            {
                return standardEndiciaPartnerID;
            }
        }

        /// <summary>
        /// Create the web service instance with the appropriate URL
        /// </summary>
        private EwsLabelService CreateWebService(string logName, EndiciaReseller reseller)
        {
            return CreateWebService(logName, reseller, LogActionType.Other);
        }

        /// <summary>
        /// Create the web service instance with the appropriate URL
        /// </summary>
        private EwsLabelService CreateWebService(string logName, EndiciaReseller reseller, LogActionType logActionType)
        {
            EwsLabelService webService = null;
            switch (reseller)
            {
                // Express1
                case EndiciaReseller.Express1:
                {
                    IApiLogEntry apiLogEntry = logEntryFactory.GetLogEntry(ApiLogSource.UspsExpress1Endicia, logName, logActionType);

                    webService = new Express1EndiciaServiceWrapper(apiLogEntry);

                    webService.Url = Express1EndiciaUtility.UseTestServer ? Express1EndiciaUtility.Express1DevelopmentUrl : Express1EndiciaUtility.Express1ProductionUrl;
                    break;
                }

                // Endicia Label Server
                default:
                {
                    IApiLogEntry apiLogEntry = logEntryFactory.GetLogEntry(ApiLogSource.UspsEndicia, logName, logActionType);

                    webService = new EwsLabelService(apiLogEntry);
                    webService.Url = UseTestServer ? EnumHelper.GetApiValue(UseTestServerUrl) : productionUrl;
                    break;
                }
            }

            return webService;
        }

        /// <summary>
        /// Get the account to use for the given shipment
        /// </summary>
        private EndiciaAccountEntity GetAccount(PostalShipmentEntity postal)
        {
            EndiciaAccountEntity account = accountRepository.GetAccount(postal.Endicia.EndiciaAccountID);
            if (account == null)
            {
                throw new EndiciaException($"No {EnumHelper.GetDescription((ShipmentTypeCode) postal.Shipment.ShipmentType)} account is selected for the shipment.");
            }

            if (postal.Shipment.ShipmentType == (int) ShipmentTypeCode.Endicia)
            {
                using (var lifetimeScope = IoC.BeginLifetimeScope())
                {
                    ILicenseService licenseService = lifetimeScope.Resolve<ILicenseService>();
                    EditionRestrictionLevel accountRestriction = licenseService.CheckRestriction(EditionFeature.EndiciaAccountNumber, account.AccountNumber);

                    if (accountRestriction != EditionRestrictionLevel.None)
                    {
                        throw new ShippingException(EnumHelper.GetDescription(EditionFeature.EndiciaAccountNumber));
                    }

                    EditionRestrictionLevel quantityRestriction = licenseService.CheckRestriction(EditionFeature.EndiciaAccountLimit, accountRepository.Accounts.Count());
                    if (quantityRestriction != EditionRestrictionLevel.None)
                    {
                        throw new ShippingException(EnumHelper.GetDescription(EditionFeature.EndiciaAccountLimit));
                    }
                }
            }

            return account;
        }

        /// <summary>
        /// Process the token for use in an endicia reference\stamp field.
        /// </summary>
        private static string ProcessTokenizedField(string token, PostalShipmentEntity postal)
        {
            return TemplateTokenProcessor.ProcessTokens(token, postal.ShipmentID);
        }

        /// <summary>
        /// Apply all the customs stuff from the shipment to the request
        /// </summary>
        private static void ApplyCustoms(ShipmentEntity shipment, LabelRequest request)
        {
            // If international we need customs
            if (!CustomsManager.IsCustomsRequired(shipment))
            {
                return;
            }

            PostalShipmentEntity postal = shipment.Postal;
            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            request.Value = (float) shipment.CustomsValue;
            request.Description = postal.CustomsContentDescription;

            // If it's an Endicia shipment - OR - if it was an endicia shipment that is being automatically processed as Express1 instead
            if (shipment.ShipmentType == (int) ShipmentTypeCode.Endicia || shipment.Postal.Endicia.OriginalEndiciaAccountID != null)
            {
                request.CustomsCertify = settings.EndiciaCustomsCertify ? "TRUE" : "FALSE";
                request.CustomsSigner = settings.EndiciaCustomsSigner;
            }
            else if (shipment.ShipmentType == (int) ShipmentTypeCode.Express1Endicia)
            {
                request.CustomsCertify = settings.Express1EndiciaCustomsCertify ? "TRUE" : "FALSE";
                request.CustomsSigner = settings.Express1EndiciaCustomsSigner;
            }
            else
            {
                throw new InvalidOperationException("Unknown endicia reseller");
            }

			if (!shipment.Postal.InternalTransactionNumber.IsNullOrWhiteSpace())
			{
				request.EelPfc = shipment.Postal.InternalTransactionNumber;
			}

            CustomsInfo customs = new CustomsInfo();
            request.CustomsInfo = customs;

            customs.NonDeliveryOption = "Return";

            customs.ContentsType = EndiciaApiTransforms.GetCustomsContentTypeCode((PostalCustomsContentType) postal.CustomsContentType);
            customs.ContentsExplanation = postal.CustomsContentDescription;
            customs.SendersCustomsReference = StringUtility.Truncate(postal.CustomsRecipientTin, 24);

            List<CustomsItem> customsItems = new List<CustomsItem>();
            for (int i = 0; i < Math.Min(30, shipment.CustomsItems.Count); i++)
            {
                ShipmentCustomsItemEntity customsItem = shipment.CustomsItems[i];

                int weightOz = (int) Math.Floor(new WeightValue(customsItem.Weight).TotalOunces * customsItem.Quantity);
                decimal value = customsItem.UnitValue * (decimal) customsItem.Quantity;

                // Round up to 1oz if we floored down to zero - but if the original value was zero, leave it zero.
                if (weightOz == 0 && customsItem.Weight > 0)
                {
                    weightOz = 1;
                }

                customsItems.Add(new CustomsItem
                {
                    Description = StringUtility.Truncate(customsItem.Description, 50),
                    Quantity = (int) Math.Ceiling(customsItem.Quantity),
                    Weight = weightOz,
                    Value = value,
                    CountryOfOrigin = customsItem.CountryOfOrigin,
                    HSTariffNumber = customsItem.HarmonizedCode
                });
            }

            customs.CustomsItems = customsItems.ToArray();
        }

        /// <summary>
        /// Get postal rates for the given shipment for all possible mail classes and rates.
        /// </summary>
        public List<RateResult> GetRates(ShipmentEntity shipment, EndiciaShipmentType endiciaShipmentType)
        {
            PostalShipmentEntity postal = shipment.Postal;

            EndiciaAccountEntity account = GetAccount(postal);

            PostalPackagingType packagingType = (PostalPackagingType) postal.PackagingType;

            bool isDomestic = shipment.ShipPerson.IsDomesticCountry();

            // There are no rates for regional boxes for international
            if (!isDomestic && (packagingType == PostalPackagingType.RateRegionalBoxA ||
                                packagingType == PostalPackagingType.RateRegionalBoxB ||
                                packagingType == PostalPackagingType.RateRegionalBoxC))
            {
                return new List<RateResult>();
            }

            PostageRatesRequest request = PrepareGetRatesRequest(shipment, account, isDomestic, packagingType, postal);

            try
            {
                using (EwsLabelService service = CreateWebService("GetRates", GetReseller(account, shipment), LogActionType.GetRates))
                {
                    PostageRatesResponse response = ProcessGetRatesRequest(shipment, service, request, account);

                    // No rates available for this service/class/type
                    if (response.PostagePrice == null)
                    {
                        return new List<RateResult>();
                    }

                    IEnumerable<RateResult> rates = response.PostagePrice
                        .Select(GetRatingDetails)
                        .Where(IsKnownService)
                        .Where(x => IsStandardService(x, shipment))
                        .Select(x => BuildRateFromResponse(x, shipment, endiciaShipmentType))
                        .ToList();

                    if (isDomestic)
                    {
                        var services = rates.Select(x => x.Tag).OfType<PostalRateSelection>().ToList();

                        rates = rates
                            .Prepend(GetFirstClassEnvelopeRates(shipment, endiciaShipmentType, packagingType, services))
                            .Append(GetGroundAdvantageRates(shipment, endiciaShipmentType, account, services));
                    }

                    return rates
                        .Where(x => x != null)
                        .Do(x =>
                        {
                            PostalUtility.SetServiceDetails(x);
                            x.ShipmentType = ShipmentTypeCode.Endicia;
                        })
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(EndiciaException));
            }
        }

        /// <summary>
        /// Get rating details from a price
        /// </summary>
        private (PostalServiceType? service, PostagePrice postage) GetRatingDetails(PostagePrice price) =>
            (EndiciaApiTransforms.GetServiceTypeFromRateMailService(price.MailClass), price);

        /// <summary>
        /// Does ShipWorks know about the service
        /// </summary>
        private bool IsKnownService((PostalServiceType? service, PostagePrice postage) value) =>
            value.service.HasValue;

        /// <summary>
        /// Is this a standard service that should be shown
        /// </summary>
        private bool IsStandardService((PostalServiceType? service, PostagePrice postage) value, IShipmentEntity shipment)
        {
            PostalServiceType serviceType = value.service.Value;

            // If the person has selected Parcel Select as their shipment type, don't include Parcel Select rates here
            // We'll pick it up below by getting individual rates so we can include the Parcel Select specific fields
            // and therefore get a more accurate rate
            if (serviceType == PostalServiceType.ParcelSelect &&
                shipment.Postal.Service == (int) PostalServiceType.ParcelSelect)
            {
                return false;
            }

            if (serviceType == PostalServiceType.GroundAdvantage &&
                shipment.Postal.Service == (int) PostalServiceType.GroundAdvantage)
            {
                return false;
            }

            // Don't confuse people by showing them Standard Post - almost no one will qualify for it.  If they do qualify, they can still manually select it.
            if (serviceType == PostalServiceType.StandardPost)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Add a rate to the rate result list from the response
        /// </summary>
        private static RateResult BuildRateFromResponse((PostalServiceType? service, PostagePrice postage) value, ShipmentEntity shipment, EndiciaShipmentType endiciaShipmentType)
        {
            var serviceType = value.service.Value;

            var (description, getAmount) = GetRateAddOnDetails(
                (PostalConfirmationType) shipment.Postal.Confirmation,
                endiciaShipmentType.GetAvailableConfirmationTypes(shipment.ShipCountryCode, serviceType, (PostalPackagingType) shipment.Postal.PackagingType));

            return new RateResult(PostalUtility.GetPostalServiceTypeDescription(serviceType) + description,
                PostalUtility.GetServiceTransitDays(serviceType),
                value.postage.Postage.TotalAmount + getAmount(value.postage) + value.postage.Surcharge.TotalAmount,
                new PostalRateSelection(serviceType))
            {
                ProviderLogo = EnumHelper.GetImage(ShipmentTypeCode.Endicia)
            };
        }

        /// <summary>
        /// Get details for required rate addons
        /// </summary>
        private static (string description, Func<PostagePrice, decimal> getAmount) GetRateAddOnDetails(PostalConfirmationType confirmation, IEnumerable<PostalConfirmationType> addOns) =>
            addOns
                .Where(x => x == confirmation && confirmationLookup.ContainsKey(x))
                .Select(x => confirmationLookup[x])
                .Select(x => (description: " (" + EnumHelper.GetDescription(confirmation) + ")", getAmount: x))
                .DefaultIfEmpty((description: string.Empty, x => 0))
                .First();

        /// <summary>
        /// Get first class envelope rates and add them to rate result list, if needed
        /// </summary>
        private RateResult GetFirstClassEnvelopeRates(ShipmentEntity shipment, EndiciaShipmentType endiciaShipmentType,
                                                PostalPackagingType packagingType, IEnumerable<PostalRateSelection> services)
        {
            // Special case - endicia not returning a rate for first class envelopes
            if ((packagingType == PostalPackagingType.Envelope || packagingType == PostalPackagingType.LargeEnvelope) &&
                services.None(r => r.ServiceType == PostalServiceType.FirstClass))
            {
                try
                {
                    return GetRate(shipment, endiciaShipmentType, PostalServiceType.FirstClass);
                }
                catch (EndiciaException ex)
                {
                    log.Error("Failed getting first class destination confirm rate: " + ex.Message, ex);
                }
            }

            return null;
        }

        /// <summary>
        /// Get parcel select rates and add them to rate result list, if needed
        /// </summary>
        private RateResult GetGroundAdvantageRates(ShipmentEntity shipment, EndiciaShipmentType endiciaShipmentType,
                                          EndiciaAccountEntity account, IEnumerable<PostalRateSelection> services)
        {
            // As of 01/28/2013 Endicia is not returning Parcel Select in the GetAllRates call - they are returning
            // Standard Post instead. If we can't find Parcel Select, try to get those rates manually. In the future
            // if Endicia updates/fixes it we may be able to remove this.
            // 07/2023 - Parcel Select is replaced by GroundAdvantage
            if (account.EndiciaReseller == (int) EndiciaReseller.None &&
                services.None(r => r.ServiceType == PostalServiceType.GroundAdvantage))
            {
                try
                {
                    return GetRate(shipment, endiciaShipmentType, PostalServiceType.GroundAdvantage, (PostalConfirmationType) shipment.Postal.Confirmation);
                }
                catch (EndiciaException ex)
                {
                    log.Error("Failed getting first class destination confirm rate: " + ex.Message, ex);
                }
            }

            return null;
        }

        /// <summary>
        /// Process the get rates request, check for errors, and return the response
        /// </summary>
        private PostageRatesResponse ProcessGetRatesRequest(ShipmentEntity shipment, EwsLabelService service,
                                                            PostageRatesRequest request, EndiciaAccountEntity account)
        {
            EnsureSecureRequest(service, shipment.ShipmentType);

            PostageRatesResponse response = service.CalculatePostageRates(request);

            // Check for errors and throw EndiciaApiException if error is found
            CheckGetRatesResponseForErrors(response, account);

            return response;
        }

        /// <summary>
        /// Check the get rates response for errors
        /// </summary>
        private static void CheckGetRatesResponseForErrors(PostageRatesResponse response, EndiciaAccountEntity account)
        {
            // Check for errors
            if (response.Status != 0)
            {
                string errorMessage = response.ErrorMessage;

                if (response.Status == 55001 && errorMessage != null)
                {
                    // We know error code 55001 maps to cubic pricing not being supported, but it also could mask other messages such as
                    // an authentication error message in the response; do some fuzzy logic to determine what the error actually is
                    if (errorMessage.ToUpperInvariant().Contains("CUBIC") && !errorMessage.ToUpperInvariant().StartsWith("DIMENSIONS"))
                    {
                        // The error is in reference to cubic packaging; use our own error message, here so we can
                        // direct the user to contact Express1 to try to reduce ShipWorks call volume
                        errorMessage = "The selected Express1 account does not support cubic pricing. Please contact Express1 to apply.";
                    }
                    else if (errorMessage.ToUpperInvariant().Contains("UNABLE TO AUTHENTICATE"))
                    {
                        // Use an error message that is slightly more informative, to let the user know which of their accounts
                        // had the problem in the event that they have multiple accounts for Endicia and/or have an Express1 account
                        errorMessage = $"ShipWorks was unable to connect to {(account.EndiciaReseller == 0 ? "Endicia" : "Express1")}" +
                                       $" with account {account.AccountNumber}.{Environment.NewLine}Check that your account credentials are correct.";
                    }
                }

                throw new EndiciaApiException(response.Status, errorMessage ?? "ShipWorks was unable to get rates at this time.");
            }
        }

        /// <summary>
        /// Prepare the get rates request with the necessary properties
        /// </summary>
        private static PostageRatesRequest PrepareGetRatesRequest(ShipmentEntity shipment, EndiciaAccountEntity account,
                                                                  bool isDomestic, PostalPackagingType packagingType,
                                                                  PostalShipmentEntity postal)
        {
            // Create the request
            PostageRatesRequest request = new PostageRatesRequest();

            // Our requester ID
            request.RequesterID = GetInterapptivePartnerID(GetReseller(account, shipment));

            // Used to rate shipments having a future ship date; seven days is the upper bound
            request.DateAdvance = Math.Max(0, (int) Math.Min((shipment.ShipDate.Date - DateTime.Now.Date).TotalDays, 7));

            // Domestic/International
            request.MailClass = isDomestic ? "Domestic" : "International";

            // Machinable
            request.Machinable = postal.NonMachinable ? "FALSE" : "TRUE";

            AddAccountDetailsToRatesRequest(account, request);
            AddPackageDetailsToRatesRequest(shipment, packagingType, postal, request);
            AddAddressDetailsToRatesRequest(shipment, account, request);
            AddConfirmationDetailsToRatesRequest(isDomestic, request, postal);
            AddInsuranceDetailsToRatesRequest(shipment, request);
            AddCustomsDetailsToRatesRequest(shipment, postal, request);

            return request;
        }

        /// <summary>
        /// Add customs details from the given shipment to the rate request
        /// </summary>
        private static void AddCustomsDetailsToRatesRequest(ShipmentEntity shipment, PostalShipmentEntity postal,
                                                           PostageRatesRequest request)
        {
            if (CustomsManager.IsCustomsRequired(shipment))
            {
                request.ContentsType = EndiciaApiTransforms.GetCustomsContentTypeCode((PostalCustomsContentType) postal.CustomsContentType);
            }
        }

        /// <summary>
        /// Add insurance details from the given shipment to the rate request
        /// </summary>
        private static void AddInsuranceDetailsToRatesRequest(ShipmentEntity shipment, PostageRatesRequest request)
        {
            // If they want insurance... (Endicia only - not Express1)
            // If they have Endicia selected as the insurance provider - AND that's enabled for their account
            if (shipment.Insurance && shipment.ShipmentType == (int) ShipmentTypeCode.Endicia &&
                EndiciaUtility.IsEndiciaInsuranceActive &&
                shipment.InsuranceProvider == (int) InsuranceProvider.Carrier)
            {
                // Ensure Services is created
                request.Services = request.Services ?? new SpecialServices();
                request.Services.InsuredMail = "Endicia";
                request.InsuredValue = (double) shipment.Postal.InsuranceValue;
            }
        }

        /// <summary>
        /// Add address details from the given shipment to the rate request
        /// </summary>
        private static void AddAddressDetailsToRatesRequest(ShipmentEntity shipment, EndiciaAccountEntity account,
                                                           PostageRatesRequest request)
        {
            PersonAdapter from = new PersonAdapter(shipment, "Origin");
            PersonAdapter recipient = new PersonAdapter(shipment, "Ship");

            // Address stuff
            if (!string.IsNullOrWhiteSpace(account.MailingPostalCode) && !shipment.ReturnShipment)
            {
                request.FromPostalCode = PersonUtility.GetZip5(account.MailingPostalCode);
            }
            else
            {
                request.FromPostalCode = @from.PostalCode5;
            }

            request.ToPostalCode = recipient.PostalCode5;
            request.ToCountryCode = recipient.AdjustedCountryCode((ShipmentTypeCode) shipment.ShipmentType);
        }

        /// <summary>
        /// Add confirmation details from the given shipment to the rate request
        /// </summary>
        private static void AddConfirmationDetailsToRatesRequest(bool isDomestic, PostageRatesRequest request, PostalShipmentEntity postal)
        {
            // We show rate matrix based on delivery and signature options
            if (isDomestic)
            {
                request.Services = new SpecialServices();
                request.Services.DeliveryConfirmation = "ON";
                request.Services.SignatureConfirmation = "ON";
                request.Services.AdultSignatureRestrictedDelivery = postal.Confirmation == (int) PostalConfirmationType.AdultSignatureRestricted ? "ON" : "OFF";
                request.Services.AdultSignature = postal.Confirmation == (int) PostalConfirmationType.AdultSignatureRequired ? "ON" : "OFF";
            }
        }

        /// <summary>
        /// Add account details from the given shipment to the rate request
        /// </summary>
        private static void AddAccountDetailsToRatesRequest(EndiciaAccountEntity account, PostageRatesRequest request)
        {
            // Account information
            request.CertifiedIntermediary = new CertifiedIntermediary();
            request.CertifiedIntermediary.AccountID = account.AccountNumber;
            request.CertifiedIntermediary.PassPhrase = SecureText.Decrypt(account.ApiUserPassword, "Endicia");
        }

        /// <summary>
        /// Add package details from the given shipment to the rate request
        /// </summary>
        private static void AddPackageDetailsToRatesRequest(ShipmentEntity shipment, PostalPackagingType packagingType,
                                                           PostalShipmentEntity postal, PostageRatesRequest request)
        {
            // Default the weight to 14oz for best rate if it is 0, so we can get a rate without needing the user to provide a value.  We do 14oz so it kicks it into a Priority shipment, which
            // is the category that most of our users will be in.
            request.WeightOz = shipment.TotalWeight > 0 ? CalculateWeight(shipment) : BestRateScope.IsActive ? 14 : .1;

            request.MailpieceShape = EndiciaApiTransforms.GetMailpieceShapeCode(packagingType);

            // Dims
            if (packagingType == PostalPackagingType.Package)
            {
                request.MailpieceDimensions = new Dimensions();

                // Force at least 1x1x1 here so we at least get an answer back
                request.MailpieceDimensions.Length = Math.Max(1, postal.DimsLength);
                request.MailpieceDimensions.Width = Math.Max(1, postal.DimsWidth);
                request.MailpieceDimensions.Height = Math.Max(1, postal.DimsHeight);
            }
            else if (packagingType == PostalPackagingType.Cubic || packagingType == PostalPackagingType.CubicSoftPack)
            {
                // To be eligible for cubic rates, the package must be 1/2 cubic foot or less, so don't
                // round up to 1 like is done with the Package type
                request.MailpieceDimensions = new Dimensions();

                request.MailpieceDimensions.Length = postal.DimsLength;
                request.MailpieceDimensions.Width = postal.DimsWidth;
                request.MailpieceDimensions.Height = postal.DimsHeight;
            }
        }

        /// <summary>
        /// Calculates the weight to use when sending a rating and label request to Endicia. The Endicia API only supports
        /// ounces weights to one decimal place, so we need to round up to the next tenth of an ounce if the weight calls
        /// for more than one decimal place being sent (i.e. this will always round up to the next tenth of an ounce unless
        /// the total weight is divisible by 0.1). For example, 3.11 would result in 3.2 being returned; 3.1 would result
        /// in 3.1 being returned.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>System.Double.</returns>
        private static double CalculateWeight(ShipmentEntity shipment)
        {
            return Math.Round(new WeightValue(shipment.TotalWeight).TotalOunces + .04, 1, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Get the postal rate for the given shipment, service, and confirmation selection.
        /// </summary>
        private RateResult GetRate(ShipmentEntity shipment, EndiciaShipmentType endiciaShipmentType, PostalServiceType serviceType) =>
            GetRate(shipment, endiciaShipmentType, serviceType, PostalConfirmationType.None);

        /// <summary>
        /// Get the postal rate for the given shipment, service, and confirmation selection.
        /// </summary>
        private RateResult GetRate(ShipmentEntity shipment, EndiciaShipmentType endiciaShipmentType, PostalServiceType serviceType, PostalConfirmationType confirmation)
        {
            PostalShipmentEntity postal = shipment.Postal;

            EndiciaAccountEntity account = GetAccount(postal);

            PostalPackagingType packagingType = (PostalPackagingType) postal.PackagingType;

            // Create the request
            PostageRateRequest request = new PostageRateRequest();

            // Our requester ID
            request.RequesterID = GetInterapptivePartnerID(GetReseller(account, shipment));

            // Used to rate shipments having a future ship date; seven days is the upper bound
            request.DateAdvance = Math.Max(0, (int) Math.Min((shipment.ShipDate.Date - DateTime.Now.Date).TotalDays, 7));
            request.ShipDate = shipment.ShipDate.ToString("MM/dd/yyyy");

            // Service
            request.MailClass = endiciaShipmentType.GetMailClassCode(serviceType, packagingType);

            // Parcel Select
            if (serviceType == PostalServiceType.ParcelSelect || serviceType == PostalServiceType.GroundAdvantage)
            {
                // Just hard code these to make sure we get rates back
                request.SortType = "Presorted";
                request.EntryFacility = "Other";
            }

            // Machinable
            request.Machinable = postal.NonMachinable ? "FALSE" : "TRUE";

            // Postage vs. Pricing
            request.ResponseOptions = new ResponseOptions();
            request.ResponseOptions.PostagePrice = "TRUE";

            // Parcel Select, or DHL
            if (PostalUtility.IsEntryFacilityRequired(serviceType))
            {
                request.SortType = EndiciaApiTransforms.GetSortTypeCode((PostalSortType) shipment.Postal.SortType);
                request.EntryFacility = EndiciaApiTransforms.GetEntryFacilityCode((PostalEntryFacility) shipment.Postal.EntryFacility);
            }

            AddAccountDetailsToRateRequest(request, account);
            AddPackageDetailsToGetRateRequest(shipment, request, packagingType, postal);
            AddAddressDetailsToGetRateRequest(shipment, account, request);
            AddConfirmationDetailsToGetRateRequest(confirmation, request);
            AddInsuranceDetailsToGetRateRequest(shipment, request);

            return ProcessRateRequest(shipment, serviceType, account, request);
        }

        /// <summary>
        /// Add confirmation details to the get rate request
        /// </summary>
        private static void AddConfirmationDetailsToGetRateRequest(PostalConfirmationType confirmation, PostageRateRequest request)
        {
            // Service options
            request.Services = new SpecialServices();
            request.Services.DeliveryConfirmation = confirmation == PostalConfirmationType.Delivery ? "ON" : "OFF";
            request.Services.SignatureConfirmation = confirmation == PostalConfirmationType.Signature ? "ON" : "OFF";
        }

        /// <summary>
        /// Add insurance details from the given shipment to the get rate request
        /// </summary>
        private static void AddInsuranceDetailsToGetRateRequest(ShipmentEntity shipment, PostageRateRequest request)
        {
            // If they want insurance... (Endicia only - not Express1)
            // If they have Endicia selected as the insurance provider - AND that's enabled for their account
            if (shipment.Insurance && shipment.ShipmentType == (int) ShipmentTypeCode.Endicia &&
                EndiciaUtility.IsEndiciaInsuranceActive && shipment.InsuranceProvider == (int) InsuranceProvider.Carrier)
            {
                request.Services.InsuredMail = "ENDICIA";
                request.InsuredValue = shipment.Postal.InsuranceValue.ToString("0.00");
            }
        }

        /// <summary>
        /// Add address details from the given shipment to the get rate request
        /// </summary>
        private static void AddAddressDetailsToGetRateRequest(ShipmentEntity shipment, EndiciaAccountEntity account, PostageRateRequest request)
        {
            PersonAdapter from = new PersonAdapter(shipment, "Origin");
            PersonAdapter recipient = new PersonAdapter(shipment, "Ship");

            // Address stuff
            request.FromPostalCode = !string.IsNullOrWhiteSpace(account.MailingPostalCode) ? account.MailingPostalCode : from.PostalCode;

            // Address stuff
            request.ToPostalCode = recipient.PostalCode5;
            request.ToCountryCode = recipient.AdjustedCountryCode((ShipmentTypeCode) shipment.ShipmentType);
        }

        /// <summary>
        /// Add package details to the get rate request
        /// </summary>
        /// <remarks>
        /// While this code looks like a duplicate of AddPackageDetailsToGetRatesRequest, it cannot be a single method
        /// because PostageRateRequest and PostageRatesRequest are 2 unique objects in the wsdl that don't share an
        /// interface or base class.
        /// </remarks>
        private static void AddPackageDetailsToGetRateRequest(ShipmentEntity shipment, PostageRateRequest request,
                                                              PostalPackagingType packagingType, PostalShipmentEntity postal)
        {
            // Default the weight to 14oz for best rate if it is 0, so we can get a rate without needing the user to provide a value.  We do 14oz so it kicks it into a Priority shipment, which
            // is the category that most of our users will be in.
            request.WeightOz = shipment.TotalWeight > 0 ? CalculateWeight(shipment) : BestRateScope.IsActive ? 14 : .1;

            request.MailpieceShape = EndiciaApiTransforms.GetMailpieceShapeCode(packagingType);

            // Dims
            if (packagingType == PostalPackagingType.Package)
            {
                request.MailpieceDimensions = new Dimensions();

                // Force at least 1x1x1 here so we at least get an answer back
                request.MailpieceDimensions.Length = Math.Max(1, postal.DimsLength);
                request.MailpieceDimensions.Width = Math.Max(1, postal.DimsWidth);
                request.MailpieceDimensions.Height = Math.Max(1, postal.DimsHeight);
            }
            else if (packagingType == PostalPackagingType.Cubic || packagingType == PostalPackagingType.CubicSoftPack)
            {
                // To be eligible for cubic rates, the package must be 1/2 cubic foot or less, so don't
                // round up to 1 like is done with the Package type
                request.MailpieceDimensions = new Dimensions();

                request.MailpieceDimensions.Length = postal.DimsLength;
                request.MailpieceDimensions.Width = postal.DimsWidth;
                request.MailpieceDimensions.Height = postal.DimsHeight;
            }
        }

        /// <summary>
        /// Add account details to the get rate request
        /// </summary>
        /// <remarks>
        /// While this code looks like a duplicate of AddPackageDetailsToGetRatesRequest, it cannot be a single method
        /// because PostageRateRequest and PostageRatesRequest are 2 unique objects in the wsdl that don't share an
        /// interface or base class.
        /// </remarks>
        private static void AddAccountDetailsToRateRequest(PostageRateRequest request, EndiciaAccountEntity account)
        {
            // Account information
            request.CertifiedIntermediary = new CertifiedIntermediary();
            request.CertifiedIntermediary.AccountID = account.AccountNumber;
            request.CertifiedIntermediary.PassPhrase = SecureText.Decrypt(account.ApiUserPassword, "Endicia");
        }

        /// <summary>
        /// Process RateRequest
        /// </summary>
        private RateResult ProcessRateRequest(ShipmentEntity shipment,
            PostalServiceType serviceType,
            EndiciaAccountEntity account,
            PostageRateRequest request)
        {
            try
            {
                using (EwsLabelService service = CreateWebService("GetRates", GetReseller(account, shipment), LogActionType.GetRates))
                {
                    EnsureSecureRequest(service, shipment.ShipmentType);

                    PostageRateResponse response = service.CalculatePostageRate(request);

                    // Check for errors
                    if (response.Status != 0)
                    {
                        throw new EndiciaApiException(response.Status, response.ErrorMessage);
                    }

                    string days = "";

                    if (PostalUtility.GetDomesticServices(ShipmentTypeCode.Endicia).Contains(serviceType))
                    {
                        days = PostalUtility.GetServiceTransitDays(serviceType);
                    }

                    return new RateResult(
                        PostalUtility.GetPostalServiceTypeDescription(serviceType),
                        days,
                        response.PostagePrice[0].TotalAmount,
                        new PostalRateSelection(serviceType))
                    {
                        ProviderLogo = EnumHelper.GetImage(ShipmentTypeCode.Endicia)
                    };
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(EndiciaException));
            }
        }

        /// <summary>
        /// Get the EndiciaReseller value for the given shipment and account
        /// </summary>
        private static EndiciaReseller GetReseller(IEndiciaAccountEntity account, ShipmentEntity shipment = null)
        {
            EndiciaReseller endiciaReseller = (EndiciaReseller) account.EndiciaReseller;

            // We just use the shipment to verify
            if (shipment != null)
            {
                if (endiciaReseller != (shipment.ShipmentType == (int) ShipmentTypeCode.Endicia ? EndiciaReseller.None : EndiciaReseller.Express1))
                {
                    throw new ShippingException(string.Format("The selected account is not for use with {0}.", ShipmentTypeManager.GetType((ShipmentTypeCode) shipment.ShipmentType).ShipmentTypeName));
                }
            }

            return endiciaReseller;
        }

        /// <summary>
        /// Purchase postage for the given amount
        /// </summary>
        public void BuyPostage(EndiciaAccountEntity account, decimal amount)
        {
            if (amount < 10)
            {
                throw new EndiciaException("At least $10.00 of postage must be purchased at a time.");
            }

            if (amount >= 100000)
            {
                throw new EndiciaException("The postage amount requested is too large.");
            }

            try
            {
                using (EwsLabelService service = CreateWebService("BuyPostage", GetReseller(account)))
                {
                    RecreditRequest request = new RecreditRequest();
                    request.RequesterID = GetInterapptivePartnerID(GetReseller(account));
                    request.RequestID = Guid.NewGuid().ToString();
                    request.CertifiedIntermediary = new CertifiedIntermediary
                    {
                        AccountID = account.AccountNumber,
                        PassPhrase = SecureText.Decrypt(account.ApiUserPassword, "Endicia")
                    };

                    request.RecreditAmount = amount.ToString("0.00");
                    RecreditRequestResponse response = service.BuyPostage(request);

                    // Check for errors
                    if (response.Status != 0)
                    {
                        throw new EndiciaApiException(response.Status, response.ErrorMessage);
                    }

                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(EndiciaException));
            }
        }

        /// <summary>
        /// Get the account status of the account, including the current postage balance.
        /// </summary>
        public EndiciaAccountStatus GetAccountStatus(EndiciaAccountEntity account)
        {
            try
            {
                using (EwsLabelService service = CreateWebService("AccountStatus", GetReseller(account)))
                {
                    AccountStatusRequest request = new AccountStatusRequest();
                    request.RequesterID = GetInterapptivePartnerID(GetReseller(account));
                    request.RequestID = Guid.NewGuid().ToString();
                    request.CertifiedIntermediary = new CertifiedIntermediary
                    {
                        AccountID = account.AccountNumber,
                        PassPhrase = SecureText.Decrypt(account.ApiUserPassword, "Endicia")
                    };

                    AccountStatusResponse response = service.GetAccountStatus(request);

                    // Check for errors
                    if (response.Status != 0)
                    {
                        throw new EndiciaApiException(response.Status, response.ErrorMessage);
                    }

                    // Save the new password to the account
                    return new EndiciaAccountStatus(response);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(EndiciaException));
            }
        }

        /// <summary>
        /// Change the api passphrase for the given account.  Returns the encrypted updated password if successful
        /// </summary>
        public string ChangeApiPassphrase(string accountNumber, EndiciaReseller reseller, string oldPassword, string newPassword)
        {
            try
            {
                using (EwsLabelService service = CreateWebService("ChangePassphrase", reseller))
                {
                    ChangePassPhraseRequest request = new ChangePassPhraseRequest();
                    request.RequesterID = GetInterapptivePartnerID(reseller);
                    request.RequestID = Guid.NewGuid().ToString();

                    request.CertifiedIntermediary = new CertifiedIntermediary
                    {
                        AccountID = accountNumber,
                        PassPhrase = oldPassword
                    };

                    request.NewPassPhrase = newPassword;

                    ChangePassPhraseRequestResponse response = service.ChangePassPhrase(request);

                    // Check for errors
                    if (response.Status != 0)
                    {
                        throw new EndiciaApiException(response.Status, response.ErrorMessage);
                    }

                    // Save the new password to the account
                    return SecureText.Encrypt(newPassword, "Endicia");
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(EndiciaException));
            }
        }

        /// <summary>
        /// Ensures that the request is not being intercepted
        /// </summary>
        /// <param name="service">Service call that should be checked</param>
        /// <param name="shipmentType">Type of shipment that will be used in the description of the exception, if one is thrown</param>
        private void EnsureSecureRequest(EwsLabelService service, int shipmentType)
        {
            CertificateRequest certificateRequest = new CertificateRequest(new Uri(service.Url), certificateInspector);
            if (certificateRequest.Submit() != CertificateSecurityLevel.Trusted)
            {
                string description = EnumHelper.GetDescription((ShipmentTypeCode) shipmentType);
                throw new EndiciaException(string.Format("ShipWorks is unable to make a secure connection to {0}.", description));
            }
        }

        /// <summary>
        /// request a refund for the given shipment
        /// </summary>
        public void RequestRefund(ShipmentEntity shipment)
        {
            if (ShipmentTypeManager.IsEndiciaDhl((PostalServiceType) shipment.Postal.Service))
            {
                log.InfoFormat("DHL shipments do not support refunds. {0}", shipment.ShipmentID);
                return;
            }

            EndiciaAccountEntity account = GetAccount(shipment.Postal);

            try
            {
                using (EwsLabelService service = CreateWebService("Refund", GetReseller(account, shipment)))
                {
                    RefundRequest request = new RefundRequest()
                    {
                        RequesterID = GetInterapptivePartnerID(GetReseller(account, shipment)),
                        RequestID = Guid.NewGuid().ToString("N"),
                        CertifiedIntermediary = new CertifiedIntermediary()
                        {
                            AccountID = account.AccountNumber,
                            PassPhrase = SecureText.Decrypt(account.ApiUserPassword, "Endicia")
                        },
                        PicNumbers = new[] { shipment.TrackingNumber }
                    };

                    RefundResponse response = service.GetRefund(request);
                    IEnumerable<LabelResponse> errors = response.Refund.Where(r => r.RefundStatus != RefundStatus.Approved);

                    if (errors.Any())
                    {
                        throw new EndiciaException(errors.First().RefundStatusMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(EndiciaException));
            }
        }

        /// <summary>
        /// Process the given shipment
        /// </summary>
        public LabelRequestResponse ProcessShipment(ShipmentEntity shipment, EndiciaShipmentType endiciaShipmentType,
            TelemetricResult<IDownloadedLabelData> telemetricResult)
        {
            PostalShipmentEntity postal = shipment.Postal;
            EndiciaAccountEntity account = GetAccount(postal);
            PostalServiceType serviceType = (PostalServiceType) postal.Service;

            LabelRequest request = GenerateLabelRequest(shipment, endiciaShipmentType, account, serviceType);

            try
            {
                using (EwsLabelService service = CreateWebService("Process", GetReseller(account, shipment)))
                {
                    EnsureSecureRequest(service, shipment.ShipmentType);

                    LabelRequestResponse response = null;
                    telemetricResult.RunTimedEvent(TelemetricEventType.GetLabel, () => response = service.GetPostageLabel(request));

                    // Check for errors
                    if (response.Status != 0)
                    {
                        if ((response.Status == 12503 || response.Status == 12104) ||
                            (response.Status == 100002 && response.ErrorMessage.Contains("not enough money in the account to produce the indicium")) ||
                            (response.Status == -1 && response.ErrorMessage.Contains("insufficient funds")))

                        {
                            throw new EndiciaInsufficientFundsException(account, response.Status, response.ErrorMessage);
                        }
                        else
                        {
                            string errorMessage = response.ErrorMessage;

                            // We know error code 55001 maps to cubic pricing not being supported, but it also could mask other messages such as
                            // an authentication error message in the response
                            if (response.Status == 55001)
                            {
                                errorMessage = "The selected Express1 account does not support cubic pricing. Please contact Express1 to apply.";
                            }

                            throw new EndiciaApiException(response.Status, errorMessage, serviceType);
                        }
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(EndiciaException));
            }
        }

        /// <summary>
        /// Create a label request for the given shipment.
        /// </summary>
        private LabelRequest GenerateLabelRequest(ShipmentEntity shipment, EndiciaShipmentType endiciaShipmentType, EndiciaAccountEntity account, PostalServiceType serviceType)
        {
            PostalShipmentEntity postal = shipment.Postal;
            PostalPackagingType packagingType = (PostalPackagingType) postal.PackagingType;

            // Create the request
            LabelRequest request = new LabelRequest();

            ApplyCredentials(shipment, request, account);

            ConfigureEnvironment(shipment, request, account);

            ApplyLabelFormat(shipment, request, serviceType, packagingType);

            ApplyDateInfo(shipment, request);

            ApplyWeightAndDims(request, postal, packagingType);

            ApplyServiceOptions(shipment, request, postal, endiciaShipmentType);

            ApplyPricing(request);

            ApplyAddresses(shipment, request, account);

            ApplyReferenceAndRubberStamps(request, postal);

            ApplyInsurance(shipment, request);

            ApplyCustoms(shipment, request);

            ApplyReturns(shipment, request, serviceType);

            return request;
        }

        /// <summary>
        /// Apply ship date info to the request.
        /// </summary>
        private static void ApplyDateInfo(ShipmentEntity shipment, LabelRequest request)
        {
            // Date advance
            request.ShipDate = shipment.ShipDate.ToString("MM/dd/yyyy");
            request.DateAdvance = (int) Math.Max(0, ((TimeSpan) (shipment.ShipDate.Date - DateTime.Now.Date)).TotalDays);
        }

        /// <summary>
        /// Apply account, requester, and partner credentials to the request.
        /// </summary>
        private static void ApplyCredentials(ShipmentEntity shipment, LabelRequest request, EndiciaAccountEntity account)
        {
            // Our partner id
            request.RequesterID = GetInterapptivePartnerID(GetReseller(account, shipment));

            // Account credentials
            request.AccountID = account.AccountNumber;
            request.PassPhrase = SecureText.Decrypt(account.ApiUserPassword, "Endicia");

            // Not sure why these are required fields - I don't think they show up anywhere
            request.PartnerCustomerID = shipment.Order.CustomerID.ToString();
            request.PartnerTransactionID = shipment.ShipmentID.ToString();
        }

        /// <summary>
        /// Apply weight and dimensions to the request.
        /// </summary>
        private static void ApplyWeightAndDims(LabelRequest request, PostalShipmentEntity postal, PostalPackagingType packagingType)
        {
            // Weight
            request.WeightOz = CalculateWeight(postal.Shipment);

            // Dims
            if (packagingType == PostalPackagingType.Package)
            {
                request.MailpieceDimensions = new Dimensions();

                // Force at least 1x1x1 here so we at least get an answer back
                request.MailpieceDimensions.Length = Math.Max(1, postal.DimsLength);
                request.MailpieceDimensions.Width = Math.Max(1, postal.DimsWidth);
                request.MailpieceDimensions.Height = Math.Max(1, postal.DimsHeight);
            }
            else if (packagingType == PostalPackagingType.Cubic || packagingType == PostalPackagingType.CubicSoftPack)
            {
                // To be eligible for cubic rates, the package must be 1/2 cubic foot or less, so don't
                // round up to 1 like is done with the Package type
                request.MailpieceDimensions = new Dimensions();

                request.MailpieceDimensions.Length = postal.DimsLength;
                request.MailpieceDimensions.Width = postal.DimsWidth;
                request.MailpieceDimensions.Height = postal.DimsHeight;
            }
        }

        /// <summary>
        /// Apply postage price to the request.
        /// </summary>
        private static void ApplyPricing(LabelRequest request)
        {
            // Postage vs. Pricing
            request.ResponseOptions = new ResponseOptions();
            request.ResponseOptions.PostagePrice = "TRUE";
        }

        /// <summary>
        /// Apply service options to the request.
        /// </summary>
        private static void ApplyServiceOptions(ShipmentEntity shipment, LabelRequest request, PostalShipmentEntity postal, EndiciaShipmentType endiciaShipmentType)
        {
            PostalServiceType serviceType = (PostalServiceType) postal.Service;
            PostalPackagingType packagingType = (PostalPackagingType) postal.PackagingType;

            // Service and packaging
            request.MailClass = endiciaShipmentType.GetMailClassCode(serviceType, packagingType);
            request.MailpieceShape = EndiciaApiTransforms.GetMailpieceShapeCode(packagingType);

            // Machinable
            request.Machinable = postal.NonMachinable ? "FALSE" : "TRUE";

            // Hidden postage
            request.IncludePostage = postal.NoPostage ? "FALSE" : "TRUE";
            request.Stealth = postal.Endicia.StealthPostage ? "TRUE" : "FALSE";

            // Service options
            request.Services = new SpecialServices();
            request.Services.SignatureConfirmation = postal.Confirmation == (int) PostalConfirmationType.Signature ? "ON" : "OFF";
            request.Services.AdultSignature = postal.Confirmation == (int) PostalConfirmationType.AdultSignatureRequired ? "ON" : "OFF";
            request.Services.AdultSignatureRestrictedDelivery = postal.Confirmation == (int) PostalConfirmationType.AdultSignatureRestricted ? "ON" : "OFF";

            // Check for the new (as of 01/27/13) international delivery service.  In that case, we have to explicitly turn on DC
            if (endiciaShipmentType.IsFreeInternationalDeliveryConfirmation(shipment.ShipCountryCode, serviceType, packagingType) && request.Services.SignatureConfirmation != "ON")
            {
                request.Services.DeliveryConfirmation = "ON";
            }

            // For express mail, see if the user is waiving signature
            if (serviceType == PostalServiceType.ExpressMail)
            {
                request.SignatureWaiver = shipment.Postal.ExpressSignatureWaiver ? "TRUE" : "FALSE";
            }

            // Parcel Select, or DHL
            if (PostalUtility.IsEntryFacilityRequired(serviceType))
            {
                request.SortType = EndiciaApiTransforms.GetSortTypeCode((PostalSortType) shipment.Postal.SortType);
                request.EntryFacility = EndiciaApiTransforms.GetEntryFacilityCode((PostalEntryFacility) shipment.Postal.EntryFacility);
            }

            // DHL, or any Consolidator
            if (ShipmentTypeManager.IsEndiciaDhl(serviceType) || ShipmentTypeManager.IsConsolidator(serviceType))
            {
                // Per documentation, IncludePostage MUST be false
                request.IncludePostage = "FALSE";

                // And this must be true
                request.PrintConsolidatorLabel = "true";
            }
        }

        /// <summary>
        /// Apply reference and rubber stamps to the request.
        /// </summary>
        private static void ApplyReferenceAndRubberStamps(LabelRequest request, PostalShipmentEntity postal)
        {
            // Reference and rubber
            postal.Memo1 = ProcessTokenizedField(postal.Memo1, postal).Truncate(50);
            postal.Memo2 = ProcessTokenizedField(postal.Memo2, postal).Truncate(50);
            postal.Memo3 = ProcessTokenizedField(postal.Memo3, postal).Truncate(50);
            postal.Endicia.ReferenceID = ProcessTokenizedField(postal.Endicia.ReferenceID, postal).Truncate(50);
            postal.Endicia.ReferenceID2 = ProcessTokenizedField(postal.Endicia.ReferenceID2, postal).Truncate(50);
            postal.Endicia.ReferenceID3 = ProcessTokenizedField(postal.Endicia.ReferenceID3, postal).Truncate(50);
            postal.Endicia.ReferenceID4 = ProcessTokenizedField(postal.Endicia.ReferenceID4, postal).Truncate(50);
            postal.Endicia.GroupCode = ProcessTokenizedField(postal.Endicia.GroupCode, postal).Truncate(50);

            request.RubberStamp1 = postal.Memo1;
            request.RubberStamp2 = postal.Memo2;
            request.RubberStamp3 = postal.Memo3;
            request.ReferenceID = postal.Endicia.ReferenceID;
            request.ReferenceID2 = postal.Endicia.ReferenceID2;
            request.ReferenceID3 = postal.Endicia.ReferenceID3;
            request.ReferenceID4 = postal.Endicia.ReferenceID4;
            request.CostCenterAlphaNumeric = postal.Endicia.GroupCode;
        }

        /// <summary>
        /// Apply returns to the request if the shipment is a return.
        /// </summary>
        private static void ApplyReturns(ShipmentEntity shipment, LabelRequest request, PostalServiceType serviceType)
        {
            // Set if this is reply postage
            if (!shipment.ReturnShipment)
            {
                return;
            }

            request.ReplyPostage = "TRUE";

            // If this is a scan based return, we need to modify the properties we are sending.
            if (EndiciaShipmentType.IsScanBasedReturnsAllowed(shipment))
            {
                // As per Nandita, ReplyPostage should be set to false for a scan based return.
                request.ReplyPostage = "FALSE";
                request.PrintScanBasedPaymentLabel = "TRUE";

                if (serviceType == PostalServiceType.ParcelSelect || serviceType == PostalServiceType.GroundAdvantage)
                {
                    // Only none is allowed for parcel select, so set these to OFF
                    request.Services.SignatureConfirmation = "OFF";
                    request.Services.DeliveryConfirmation = "OFF";
                    request.Services.AdultSignatureRestrictedDelivery = "OFF";
                    request.Services.AdultSignature = "OFF";
                }
                else if (serviceType == PostalServiceType.ExpressMail)
                {
                    // SignatureWaiver must be TRUE or null.  False will return an error.
                    request.SignatureWaiver = shipment.Postal.ExpressSignatureWaiver ? request.SignatureWaiver : null;
                }
            }
        }

        /// <summary>
        /// Apply insurance to the request.
        /// </summary>
        private static void ApplyInsurance(ShipmentEntity shipment, LabelRequest request)
        {
            // If it's supposed to be Endicia insurance
            if (shipment.InsuranceProvider == (int) InsuranceProvider.Carrier)
            {
                // Make sure that even makes sense
                if (shipment.ShipmentType == (int) ShipmentTypeCode.Endicia && EndiciaUtility.IsEndiciaInsuranceActive)
                {
                    // Then if it's insured, do it
                    if (shipment.Insurance)
                    {
                        // Ensure Services is created
                        request.Services = request.Services ?? new SpecialServices();
                        request.Services.InsuredMail = "Endicia";
                        request.InsuredValue = shipment.Postal.InsuranceValue.ToString("0.00");
                    }
                }
                // If it doesn't make sense, reset the carrier
                else
                {
                    shipment.InsuranceProvider = (int) InsuranceProvider.ShipWorks;
                }
            }
        }

        /// <summary>
        /// Apply the to and from addresses to the request.
        /// </summary>
        private void ApplyAddresses(ShipmentEntity shipment, LabelRequest request, EndiciaAccountEntity account)
        {
            PersonAdapter from = new PersonAdapter(shipment, "Origin");
            PersonAdapter recipient = new PersonAdapter(shipment, "Ship");

            ApplyFromAddress(shipment, request, from, account);

            ApplyToAddress(shipment, request, from, recipient);
        }

        /// <summary>
        /// Apply the to address to the request.
        /// </summary>
        private static void ApplyToAddress(ShipmentEntity shipment, LabelRequest request, PersonAdapter from, PersonAdapter recipient)
        {
            PostalServiceType serviceType = (PostalServiceType) shipment.Postal.Service;

            // POZipCode is always required for ParcelSelect if EntryFacility is not 'Other'.
            if (string.IsNullOrWhiteSpace(request.POZipCode) && PostalUtility.IsEntryFacilityRequired(serviceType))
            {
                request.POZipCode = shipment.ReturnShipment ? recipient.PostalCode5 : from.PostalCode5;
            }

            // To
            request.ToName = new PersonName(recipient).FullName;
            request.ToCompany = recipient.Company;
            request.ToAddress1 = recipient.Street1;
            request.ToAddress2 = recipient.Street2;
            request.ToAddress3 = recipient.Street3;
            request.ToCity = PostalUtility.StripPunctuation(recipient.City);
            request.ToState = PostalUtility.AdjustState(recipient.CountryCode, recipient.StateProvCode);
            request.ToPostalCode = recipient.IsDomesticCountry() ? recipient.PostalCode5 : recipient.PostalCode;
            request.ToZIP4 = recipient.IsDomesticCountry() ? recipient.PostalCode4 : "";
            request.ToCountryCode = recipient.AdjustedCountryCode((ShipmentTypeCode) shipment.ShipmentType);
            request.ToPhone = recipient.Phone10Digits;
            request.ToEMail = recipient.Email;

            // Special case for US territories, that Endicia wants the state code filled out
            if (string.IsNullOrWhiteSpace(request.ToState) && recipient.IsDomesticCountry())
            {
                request.ToState = request.ToCountryCode;
            }
        }

        /// <summary>
        /// Apply the from address to the request.
        /// </summary>
        private void ApplyFromAddress(ShipmentEntity shipment, LabelRequest request, PersonAdapter from, EndiciaAccountEntity account)
        {
            // From
            request.FromName = new PersonName(from).FullName;
            request.FromCompany = from.Company;
            request.ReturnAddress1 = from.Street1;
            request.ReturnAddress2 = from.Street2;
            request.ReturnAddress3 = from.Street3;
            request.FromCity = from.City;
            request.FromState = from.StateProvCode;
            request.FromPostalCode = from.PostalCode;
            request.FromPhone = from.Phone10Digits;
            request.FromEMail = from.Email;

            var mailingPostOfficeAccount = account;

            // If this is an Express1 shipment that was really an Endicia shipment auto-using Express1, we need to use the originally selected Endicia account setting,
            // as that is what the user will expect.
            if (shipment.ShipmentType == (int) ShipmentTypeCode.Express1Endicia && shipment.Postal.Endicia.OriginalEndiciaAccountID != null)
            {
                mailingPostOfficeAccount = accountRepository.GetAccount(shipment.Postal.Endicia.OriginalEndiciaAccountID.Value) ?? account;
            }

            // We can only using the MailingPostalCode configured for the account if it's not a Return Shipment - since it's not coming from this account if its a return
            if (!shipment.ReturnShipment && !string.IsNullOrWhiteSpace(mailingPostOfficeAccount.MailingPostalCode))
            {
                request.POZipCode = PersonUtility.GetZip5(mailingPostOfficeAccount.MailingPostalCode);
            }
        }

        /// <summary>
        /// Apply appropriate label format.
        /// </summary>
        private static void ApplyLabelFormat(ShipmentEntity shipment, LabelRequest request, PostalServiceType serviceType, PostalPackagingType packagingType)
        {
            ThermalLanguage? thermalType;

            // Determine what thermal type, if any to use.  Use the Endicia settings if it is an Endicia shipment being auto-switched to an Express1 shipment
            if (shipment.ShipmentType == (int) ShipmentTypeCode.Endicia || shipment.Postal.Endicia.OriginalEndiciaAccountID != null)
            {
                thermalType = shipment.RequestedLabelFormat == (int) ThermalLanguage.None ? null : (ThermalLanguage?) shipment.RequestedLabelFormat;
            }
            else if (shipment.ShipmentType == (int) ShipmentTypeCode.Express1Endicia)
            {
                thermalType = shipment.RequestedLabelFormat == (int) ThermalLanguage.None ? null : (ThermalLanguage?) shipment.RequestedLabelFormat;
            }
            else
            {
                throw new InvalidOperationException("Unknown Endicia shipment type.");
            }

            // Critical mail does not support thermal
            if (serviceType == PostalServiceType.CriticalMail)
            {
                thermalType = null;
            }

            bool isInternational = PostalUtility.GetInternationalServices(ShipmentTypeCode.Endicia).Contains(serviceType);

            // International is always marked international
            if (isInternational)
            {
                thermalType = ApplyInternationalLabelFormat(shipment, request, thermalType);
            }
            // APO/FPO gets marked as "Domestic"
            else if (CustomsManager.IsCustomsRequired(shipment) && shipment.ShipPerson.IsDomesticCountry())
            {
                thermalType = ApplyApoLabelFormat(shipment, request);
            }
            // Check for special FirstClass envelopes that are "Destination Confirm"
            else if (serviceType == PostalServiceType.FirstClass && (packagingType == PostalPackagingType.LargeEnvelope || packagingType == PostalPackagingType.Envelope))
            {
                request.LabelType = "DestinationConfirm";
                request.LabelSize = "6x4";

                // Thermal is unsupported
                thermalType = null;
            }
            // Otherwise a 'standard' (Default) domestic label
            else
            {
                request.LabelType = "Default";
                request.LabelSize = "4x6";
            }

            // Set the thermal type for the shipment
            shipment.ActualLabelFormat = (int?) thermalType;
            request.ImageFormat = thermalType == null ? "PNG" : thermalType == ThermalLanguage.EPL ? "EPL2" : "ZPLII";
        }

        /// <summary>
        /// For APO/FPO shipments, add appropriate label format.
        /// </summary>
        private static ThermalLanguage? ApplyApoLabelFormat(ShipmentEntity shipment, LabelRequest request)
        {
            request.LabelType = "Domestic";
            request.LabelSubtype = "Integrated";

            // For APO/FPO customs - the non-A (CN22) form is the only supported for thermal.  So if we are doing thermal we'll just use that. USPS site says that's ok
            // if the "A" (CP72) is too big for the package.  Whatever - we need thermal to work.
            // BN: Reverted back to 'A' due to customers complaining USPS wont accept
            request.IntegratedFormType = "Form2976A";

            if (shipment.CustomsItems.Count > 5)
            {
                request.LabelSize = "";
            }
            else
            {
                request.LabelSize = "4x6";
            }

            return null;
        }

        /// <summary>
        /// For international shipments, add appropriate label format.
        /// </summary>
        private static ThermalLanguage? ApplyInternationalLabelFormat(ShipmentEntity shipment, LabelRequest request, ThermalLanguage? thermalType)
        {
            PostalServiceType serviceType = (PostalServiceType) shipment.Postal.Service;
            PostalPackagingType packagingType = (PostalPackagingType) shipment.Postal.PackagingType;

            request.LabelType = "International";
            request.LabelSubtype = "Integrated";
            request.LabelSize = "4x6";

            // First class is always the simple form CN22
            if (serviceType == PostalServiceType.InternationalFirst)
            {
                request.IntegratedFormType = "Form2976";

                if (shipment.CustomsItems.Count > 5)
                {
                    throw new ShippingException("International First Class only supports up to 5 customs items.");
                }
            }
            else if (serviceType == PostalServiceType.InternationalPriority)
            {
                // Flat envelopes, and Flat small box, can have the small form
                if (packagingType == PostalPackagingType.FlatRateEnvelope ||
                    packagingType == PostalPackagingType.FlatRatePaddedEnvelope ||
                    packagingType == PostalPackagingType.FlatRateLegalEnvelope ||
                    packagingType == PostalPackagingType.FlatRateSmallBox)
                {
                    request.IntegratedFormType = "Form2976";

                    if (shipment.CustomsItems.Count > 5)
                    {
                        throw new ShippingException(string.Format("International Priority with {0} only supports up to 5 customs items.", EnumHelper.GetDescription(packagingType)));
                    }
                }
                else
                {
                    request.IntegratedFormType = "Form2976A";
                }
            }
            // Otherwise the big form will be required CP72
            else
            {
                request.IntegratedFormType = "Form2976A";
            }

            if (shipment.CustomsItems.Count > 5)
            {
                request.IntegratedFormType = "Form2976A";
                request.LabelSize = "";

                // Can't be thermal
                thermalType = null;
            }
            else
            {
                if (request.IntegratedFormType == "Form2976A" && thermalType != null)
                {
                    request.LabelSize = "4x6c";
                }
            }

            return thermalType;
        }

        /// <summary>
        /// Set test/production environment info on the request.
        /// </summary>
        private static void ConfigureEnvironment(ShipmentEntity shipment, LabelRequest request, EndiciaAccountEntity account)
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            PostalServiceType serviceType = (PostalServiceType) shipment.Postal.Service;
            PostalPackagingType packagingType = (PostalPackagingType) shipment.Postal.PackagingType;

            // Express1
            if (shipment.ShipmentType == (int) ShipmentTypeCode.Express1Endicia)
            {
                request.Test = Express1EndiciaUtility.UseTestServer || account.TestAccount ? "YES" : "NO";

                // If this is an Express1 shipment, and Single-Source is turned on, we need to make sure it is a packaging type Express1 supports or
                if (settings.Express1EndiciaSingleSource)
                {
                    // If its not a postage saving service, express1 would automatically reroute to Endicia.
                    // It's only when the service could save, but the packaging is goofed that there's a problem.
                    if (Express1Utilities.IsPostageSavingService(serviceType) &&
                        !Express1Utilities.IsValidPackagingType(serviceType, packagingType))
                    {
                        request.Provider = "Endicia";
                    }
                }
            }
            // Endicia
            else
            {
                // Per Wing, if we are sending to test servers, always set Test = "NO"
                if (UseTestServer)
                {
                    request.Test = "NO";
                }
                else
                {
                    request.Test = account.TestAccount ? "YES" : "NO";
                }
            }
        }

        /// <summary>
        /// Track the given shipment
        /// </summary>
        public Tracking.TrackingResult TrackShipment(ShipmentEntity shipment)
        {
            PostalShipmentEntity postal = shipment.Postal;
            EndiciaAccountEntity account;

            try
            {
                account = GetAccount(postal);
            }
            catch (Exception e) when (e is EndiciaException || e is ShippingException)
            {
                // We weren't able to get the account, so the user must have deleted it.
                // Just try PostalWebTools instead.
                return new PostalWebShipmentType().TrackShipment(shipment);
            }

            PackageStatusRequest packageStatusRequest = new PackageStatusRequest()
            {
                PicNumbers = new[] { shipment.TrackingNumber },
                RequesterID = GetInterapptivePartnerID(GetReseller(account, shipment)),
                RequestID = Guid.NewGuid().ToString("N"),
                CertifiedIntermediary = new CertifiedIntermediary()
                {
                    AccountID = account.AccountNumber,
                    PassPhrase = SecureText.Decrypt(account.ApiUserPassword, "Endicia")
                }
            };

            try
            {
                using (EwsLabelService service = CreateWebService("Track", GetReseller(account, shipment)))
                {
                    EnsureSecureRequest(service, shipment.ShipmentType);

                    PackageStatusResponse packageStatusResponse = service.StatusRequest(packageStatusRequest);

                    // Check for errors
                    if (packageStatusResponse.Status != 0)
                    {
                        log.Error($@"An error was returned while getting tracking info for ShipmentID: '{shipment.ShipmentID}', tracking number: '{shipment.TrackingNumber}'.  
                                     The error number was {packageStatusResponse.Status}");
                    }

                    Tracking.TrackingResult trackingResult = new Tracking.TrackingResult();

                    IEnumerable<StatusEventList> statusEvents = packageStatusResponse.PackageStatus.SelectMany(ps => ps.PackageStatusEventList);

                    if (statusEvents.Any())
                    {
                        foreach (StatusEventList statusResponse in statusEvents)
                        {
                            trackingResult.Details.Add(new TrackingResultDetail()
                            {
                                Activity = statusResponse.StatusDescription,
                                Date = DateTime.Parse(statusResponse.EventDateTime).ToString("M/dd/yyy"),
                                Time = DateTime.Parse(statusResponse.EventDateTime).ToString("h:mm tt")
                            });
                        }

                        trackingResult.Summary = statusEvents.OrderBy(te => DateTime.Parse(te.EventDateTime)).Last().TrackingSummary;
                    }

                    return trackingResult;
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(ShippingException));
            }
        }

        /// <summary>
        /// Get a scan form for the given shipments
        /// </summary>
        public SCANResponse GetScanForm(IEndiciaAccountEntity account, IEnumerable<IShipmentEntity> shipments)
        {
            try
            {
                EndiciaReseller reseller = GetReseller(account);

                using (EwsLabelService service = CreateWebService("ScanForm", reseller))
                {
                    SCANRequest request = new SCANRequest()
                    {
                        GetSCANRequestParameters = new GetSCANParameters() { ImageFormat = "PNG" },
                        RequesterID = GetInterapptivePartnerID(reseller),
                        RequestID = Guid.NewGuid().ToString("N"),
                        CertifiedIntermediary = new CertifiedIntermediary()
                        {
                            AccountID = account.AccountNumber,
                            PassPhrase = SecureText.Decrypt(account.ApiUserPassword, "Endicia")
                        },
                        PicNumbers = shipments.Select(s => s.TrackingNumber).ToArray()
                    };

                    return service.GetSCAN(request);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(EndiciaException));
            }
        }

        /// <summary>
        /// Signup for a new Endicia account
        /// </summary>
        public void Signup(EndiciaAccountEntity account,
            EndiciaAccountType accountType,
            PersonAdapter address,
            EndiciaNewAccountCredentials credentials,
            EndiciaPaymentInfo paymentInfo)
        {
            try
            {
                UserSignUpRequest request = GetSignupRequest(accountType, address, credentials, paymentInfo);
                UserSignUpResponse signupResponse = SendSignupRequest(request);
                ProcessUserSignupResponse(account, accountType, address, credentials, signupResponse);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(EndiciaException));
            }
        }

        /// <summary>
        /// Sends the signup request
        /// </summary>
        private UserSignUpResponse SendSignupRequest(UserSignUpRequest request)
        {
            UserSignUpResponse userSignUpResponse;
            using (EwsLabelService service = CreateWebService("Signup", EndiciaReseller.None))
            {
                userSignUpResponse = service.GetUserSignUp(request);
            }

            return userSignUpResponse;
        }

        /// <summary>
        /// Processes the signup response
        /// </summary>
        private void ProcessUserSignupResponse(EndiciaAccountEntity account, EndiciaAccountType accountType, PersonAdapter address, EndiciaNewAccountCredentials credentials, UserSignUpResponse userSignUpResponse)
        {
            long confirmation = userSignUpResponse.ConfirmationNumber;
            CheckResponseForErrors(userSignUpResponse, confirmation);

            // Save the address
            PersonAdapter.Copy(address, new PersonAdapter(account, ""));
            account.MailingPostalCode = account.PostalCode;

            credentials.PopulateAccountEntity(account);

            // Account type and passwords
            account.SignupConfirmation = confirmation.ToString();
            account.AccountType = (int) accountType;
            account.CreatedByShipWorks = true;
            account.ScanFormAddressSource = (int) EndiciaScanFormAddressSource.Provider;

            account.TestAccount = UseTestServer;
            account.Description = string.Empty;
            accountRepository.Save(account);
        }

        /// <summary>
        /// Checks the response for errors. If an error is found, EndiciaException is thrown
        /// </summary>
        private static void CheckResponseForErrors(UserSignUpResponse userSignUpResponse, long confirmation)
        {
            string errorMessage = userSignUpResponse.ErrorMessage;
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                if (!errorMessage.EndsWith("."))
                {
                    errorMessage += ".";
                }

                throw new EndiciaException(errorMessage);
            }

            if (confirmation <= 0)
            {
                throw new EndiciaException("The Endicia server returned an invalid confirmation number.");
            }
        }

        /// <summary>
        /// Gets a UserSignupRequest
        /// </summary>
        private UserSignUpRequest GetSignupRequest(EndiciaAccountType accountType,
            PersonAdapter address,
            EndiciaNewAccountCredentials credentials,
            EndiciaPaymentInfo paymentInfo)
        {
            return new UserSignUpRequest()
            {
                RequesterID = GetInterapptivePartnerID(EndiciaReseller.None),
                RequestID = Guid.NewGuid().ToString("N"),

                AccountCredentials = credentials.GetApiAccountCredentials(),
                PhysicalAddress = new PhysicalPickupAddress()
                {
                    CompanyName = address.Company,
                    FirstName = address.FirstName,
                    LastName = address.LastName,
                    Phone = address.Phone,
                    Address = address.StreetAll,
                    City = address.City,
                    State = address.StateProvCode,
                    Zip5 = address.PostalCode5,
                    Zip4 = address.PostalCode4
                },

                FirstName = address.FirstName,
                MiddleName = address.MiddleName,
                LastName = address.LastName,
                EmailAddress = address.Email,
                PhoneNumber = address.Phone,
                ICertify = true,
                FaxNumber = address.Fax,

                BillingType = GetBillingType(accountType),
                PartnerID = GetInterapptivePartnerID(accountType),

                CreditCard = GetCreditCard(paymentInfo),
                CheckingAccount = GetCheckingAccount(paymentInfo),
                PaymentDetailsDeferred = false
            };
        }

        /// <summary>
        /// Gets a checking account for a signup request
        /// </summary>
        /// <returns>Null if paymentInfo.UseCheckingForPostage is false</returns>
        private CheckingAccount GetCheckingAccount(EndiciaPaymentInfo paymentInfo)
        {
            CheckingAccount checkingAccount = null;
            if (paymentInfo.UseCheckingForPostage)
            {
                checkingAccount = new CheckingAccount()
                {
                    AccountNumber = paymentInfo.CheckingAccount,
                    RoutingNumber = paymentInfo.CheckingRouting
                };
            }

            return checkingAccount;
        }

        /// <summary>
        /// Gets the CreditCard for a signup request
        /// </summary>
        private CreditCard GetCreditCard(EndiciaPaymentInfo paymentInfo)
        {
            return new CreditCard()
            {
                CreditCardNumber = paymentInfo.CardNumber,
                CreditCardAddress = paymentInfo.CardBillingAddress.StreetAll,
                CreditCardCity = paymentInfo.CardBillingAddress.City,
                CreditCardState = paymentInfo.CardBillingAddress.StateProvCode,
                CreditCardZip5 = paymentInfo.CardBillingAddress.PostalCode5,
                CreditCardCountryCode = paymentInfo.CardBillingAddress.CountryCode,
                CreditCardType = GetCreditCardType(paymentInfo.CardType),
                CreditCardCVV = paymentInfo.CVV,
                CreditCardMonth = GetCreditCardMonth(paymentInfo.CardExpirationMonth),
                CreditCardYear = paymentInfo.CardExpirationYear
            };
        }

        /// <summary>
        /// Gets the CreditCardMonth for a signup request
        /// </summary>
        private CreditCardMonth GetCreditCardMonth(int numericMonth)
        {
            switch (numericMonth)
            {
                case 1:
                    return CreditCardMonth.January;
                case 2:
                    return CreditCardMonth.February;
                case 3:
                    return CreditCardMonth.March;
                case 4:
                    return CreditCardMonth.April;
                case 5:
                    return CreditCardMonth.May;
                case 6:
                    return CreditCardMonth.June;
                case 7:
                    return CreditCardMonth.July;
                case 8:
                    return CreditCardMonth.August;
                case 9:
                    return CreditCardMonth.September;
                case 10:
                    return CreditCardMonth.October;
                case 11:
                    return CreditCardMonth.November;
                case 12:
                    return CreditCardMonth.December;
                default:
                    throw new ArgumentOutOfRangeException($"Invalid month:{numericMonth}", "numericMonth");
            }
        }

        /// <summary>
        /// Gets the CreditCardType for a signup request
        /// </summary>
        private CreditCardType GetCreditCardType(EndiciaCreditCardType cardType)
        {
            switch (cardType)
            {
                case EndiciaCreditCardType.Visa:
                    return CreditCardType.Visa;
                case EndiciaCreditCardType.MasterCard:
                    return CreditCardType.Mastercard;
                case EndiciaCreditCardType.AmericanExpress:
                    return CreditCardType.AmericanExpress;
                case EndiciaCreditCardType.CarteBlanche:
                    return CreditCardType.CarteBlanche;
                case EndiciaCreditCardType.Discover:
                    return CreditCardType.Discover;
                case EndiciaCreditCardType.DinersClub:
                    return CreditCardType.DinersClub;
                default:
                    throw new InvalidOperationException("Invalid endicia card type: " + cardType);
            }
        }

        /// <summary>
        /// Gets the billing type for a signup request
        /// </summary>
        private string GetBillingType(EndiciaAccountType accountType)
        {
            switch (accountType)
            {
                case EndiciaAccountType.Premium:
                    return "T8";
                case EndiciaAccountType.Standard:
                    return "T7";
                default:
                    return "TP";
            }
        }
    }
}
