using System;
using System.Web.Services.Protocols;
using Interapptive.Shared.Net;
using ShipWorks.Shipping.Carriers.UPS.WebServices.Promo;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.API
{
    /// <summary>
    /// Client for interacting with UPS Promo API
    /// </summary>
    public class UpsApiPromoClient : IUpsApiPromoClient
    {
        private readonly UpsPromo upsPromo;
        private static readonly string promoCode = "SomePromoCode";
        private static readonly string endpoint = "https://onlinetools.ups.com/webservices/PromoDiscount";
        private static readonly string testingEndpoint = "https://wwwcie.ups.com/webservices/PromoDiscount";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="upsPromo"></param>
        public UpsApiPromoClient(UpsPromo upsPromo)
        {
            this.upsPromo = upsPromo;
        }

        /// <summary>
        /// Requests Terms and Conditions for UPS Promo Api
        /// </summary>
        /// <returns></returns>
        public PromoAcceptanceTerms GetAgreement()
        {
            PromoDiscountAgreementRequest request = new PromoDiscountAgreementRequest
            {
                Locale = new LocaleType()
                {
                    CountryCode = upsPromo.CountryCode,
                    LanguageCode = "en"
                },
                PromoCode = promoCode
            };

            try
            {
                using (PromoDiscountService service = new PromoDiscountService())
                {
                    // Point the service to the correct endpoint
                    service.Url = endpoint;
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
        public PromoActivation Activate(string acceptanceCode)
        {
            PromoDiscountRequest request = new PromoDiscountRequest()
            {
                Locale = new LocaleType()
                {
                    CountryCode = upsPromo.CountryCode,
                    LanguageCode = "en"
                },
                AgreementAcceptanceCode = acceptanceCode,
                PromoCode = promoCode
            };

            try
            {
                using (PromoDiscountService service = new PromoDiscountService())
                {
                    // Point the service to the correct endpoint
                    service.Url = endpoint;
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
                    return new PromoActivation(upsPromo, response);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(UpsPromoException));
            }
        }
    }
}