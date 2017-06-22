using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Carriers.UPS.WebServices.Promo;
using System;
using System.Web.Services.Protocols;
using Interapptive.Shared.Security;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.Api
{
    /// <summary>
    /// Client for interacting with UPS Promo API
    /// </summary>
    public class UpsApiPromoClient : IUpsApiPromoClient
    {
        private const string LiveEndpoint = "https://onlinetools.ups.com/webservices/PromoDiscount";
        private const string TestingEndpoint = "https://wwwcie.ups.com/webservices/PromoDiscount";
        private readonly IUpsPromo upsPromo;
        private readonly ILogEntryFactory logEntryFactory;
        private readonly ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository;
        private readonly IUpsUtility upsUtility;
        private readonly LocaleType locale;
        private readonly IEncryptionProvider secureText;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsApiPromoClient(IUpsPromo upsPromo, 
            ILogEntryFactory logEntryFactory, 
            ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository, 
            IUpsUtility upsUtility, 
            IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.upsPromo = upsPromo;
            this.logEntryFactory = logEntryFactory;
            this.accountRepository = accountRepository;
            this.upsUtility = upsUtility;
            locale = new LocaleType
            {
                CountryCode = upsPromo.CountryCode,
                LanguageCode = "en"
            };

            secureText = encryptionProviderFactory.CreateSecureTextEncryptionProvider("UPS");
        }

        /// <summary>
        /// Indicates if the test server should be used instead of the live server
        /// </summary>
        public static bool UseTestServer
        {
            get { return InterapptiveOnly.Registry.GetValue("UpsOltTestServer", false); }
            set { InterapptiveOnly.Registry.SetValue("UpsOltTestServer", value); }
        }

        /// <summary>
        /// Requests Terms and Conditions for UPS Promo Api
        /// </summary>
        /// <returns></returns>
        public PromoAcceptanceTerms GetAgreement()
        {
            PromoDiscountAgreementRequest request = new PromoDiscountAgreementRequest
            {
                Locale = locale,
                PromoCode = upsPromo.PromoCode
            };

            try
            {
                UPSSecurity security = GetUpsSecurity(upsPromo.AccessLicenseNumber, upsPromo.Username,
                    upsPromo.Password);
                return new PromoAcceptanceTerms(ProcessGetAgreementRequest(request, security));
            }
            catch (SoapException ex) when (ex.Detail.OuterXml.Contains("Invalid Access License number"))
            {
                // The access license number is not valid, get a new license number and retry getting the agreement 
                try
                {
                    UpsAccountEntity account = accountRepository.GetAccount(upsPromo.AccountId);

                    string accessKey = secureText.Decrypt(upsUtility.FetchAndSaveUpsAccessKey(account, upsUtility.GetAccessLicenseText()));
                    UPSSecurity security = GetUpsSecurity(accessKey, upsPromo.Username, upsPromo.Password);
                    return new PromoAcceptanceTerms(ProcessGetAgreementRequest(request, security));
                }
                catch (Exception e)
                {
                    throw WebHelper.TranslateWebException(e, typeof(UpsPromoException));
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(UpsPromoException));
            }
        }

        /// <summary>
        /// Process the get agreement request
        /// </summary>
        private PromoDiscountAgreementResponse ProcessGetAgreementRequest(PromoDiscountAgreementRequest request, UPSSecurity security)
        {
            IApiLogEntry apiLogEntry = logEntryFactory.GetLogEntry(ApiLogSource.UPS, "GetPromoAgreement", LogActionType.Other);

            using (PromoDiscountService service = new PromoDiscountService(apiLogEntry))
            {
                // Point the service to the correct endpoint
                service.Url = UseTestServer ? TestingEndpoint : LiveEndpoint;
                service.UPSSecurityValue = security;

                PromoDiscountAgreementResponse response = service.ProcessPromoDiscountAgreement(request);
                return response;
            }
        }

        /// <summary>
        /// Activates the UPS Promo with the given acceptance code
        /// </summary>
        public PromoActivation Activate(string acceptanceCode, string upsAccountNumber)
        {
            PromoDiscountRequest request = new PromoDiscountRequest()
            {
                AccountInfo = new AccountInfoType()
                {
                    AccountNumber = upsAccountNumber
                },
                Locale = locale,
                AgreementAcceptanceCode = acceptanceCode,
                PromoCode = upsPromo.PromoCode
            };

            try
            {
                IApiLogEntry apiLogEntry = logEntryFactory.GetLogEntry(ApiLogSource.UPS, "AcceptPromoAgreement", LogActionType.Other);
                using (PromoDiscountService service = new PromoDiscountService(apiLogEntry))
                {
                    // Point the service to the correct endpoint
                    service.Url = UseTestServer ? TestingEndpoint : LiveEndpoint;

                    service.UPSSecurityValue =
                        GetUpsSecurity(upsPromo.AccessLicenseNumber, upsPromo.Username, upsPromo.Password);
                    PromoDiscountResponse response = service.ProcessPromoDiscount(request);
                    return new PromoActivation(response);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(UpsPromoException));
            }
        }

        /// <summary>
        /// Get the UpsSecurity object
        /// </summary>
        private static UPSSecurity GetUpsSecurity(string licenseNumber, string username, string password)
        {
            return new UPSSecurity
            {
                ServiceAccessToken = new UPSSecurityServiceAccessToken
                {
                    AccessLicenseNumber = licenseNumber
                },
                UsernameToken = new UPSSecurityUsernameToken
                {
                    Username = username,
                    Password = password
                }
            };
        }
    }
}