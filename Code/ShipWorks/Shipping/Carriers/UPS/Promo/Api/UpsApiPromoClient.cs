using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Carriers.UPS.WebServices.Promo;
using System;

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
        private readonly LocaleType locale;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsApiPromoClient(IUpsPromo upsPromo, ILogEntryFactory logEntryFactory)
        {
            this.upsPromo = upsPromo;
            this.logEntryFactory = logEntryFactory;
            locale = new LocaleType()
            {
                CountryCode = upsPromo.CountryCode,
                LanguageCode = "en"
            };
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
                IApiLogEntry apiLogEntry = logEntryFactory.GetLogEntry(ApiLogSource.UPS, "GetPromoAgreement", LogActionType.Other);

                using (PromoDiscountService service = new PromoDiscountService(apiLogEntry))
                {
                    // Point the service to the correct endpoint
                    service.Url = UseTestServer ? TestingEndpoint : LiveEndpoint;
                    service.UPSSecurityValue = new UPSSecurity()
                    {
                        ServiceAccessToken = new UPSSecurityServiceAccessToken()
                        {
                            AccessLicenseNumber = upsPromo.AccessLicenseNumber
                        },
                        UsernameToken = new UPSSecurityUsernameToken()
                        {
                            Username = upsPromo.Username,
                            Password = upsPromo.Password
                        }
                    };

                    PromoDiscountAgreementResponse response = service.ProcessPromoDiscountAgreement(request);
                    return new PromoAcceptanceTerms(response);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(UpsPromoException));
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
                    service.UPSSecurityValue = new UPSSecurity()
                    {
                        ServiceAccessToken = new UPSSecurityServiceAccessToken()
                        {
                            AccessLicenseNumber = upsPromo.AccessLicenseNumber
                        },
                        UsernameToken = new UPSSecurityUsernameToken()
                        {
                            Username = upsPromo.Username,
                            Password = upsPromo.Password
                        }
                    };

                    PromoDiscountResponse response = service.ProcessPromoDiscount(request);
                    return new PromoActivation(response);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(UpsPromoException));
            }
        }
    }
}