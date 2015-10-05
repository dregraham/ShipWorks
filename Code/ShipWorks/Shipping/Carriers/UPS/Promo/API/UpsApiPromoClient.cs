using System;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Carriers.UPS.WebServices.Promo;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.API
{
    /// <summary>
    /// Client for interacting with UPS Promo API
    /// </summary>
    public class UpsApiPromoClient : IUpsApiPromoClient
    {
        private const string PromoCode = "SomePromoCode";
        private const string LiveEndpoing = "https://onlinetools.ups.com/webservices/PromoDiscount";
        private const string TestingEndpoint = "https://wwwcie.ups.com/webservices/PromoDiscount";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="upsPromo"></param>
        public UpsApiPromoClient(UpsPromo upsPromo)
        {
            this.UpsPromo = upsPromo;
            Locale = new LocaleType()
            {
                CountryCode = upsPromo.CountryCode,
                LanguageCode = "en"
            };

        }
        
        /// <summary>
        /// Requests Terms and Conditions for UPS Promo Api
        /// </summary>
        /// <returns></returns>
        public PromoAcceptanceTerms GetAgreement()
        {
            PromoDiscountAgreementRequest request = new PromoDiscountAgreementRequest
            {
                Locale = Locale,
                PromoCode = PromoCode
            };

            try
            {
                using (PromoDiscountService service = new PromoDiscountService())
                {
                    // Point the service to the correct endpoint
                    service.Url = UseTestServer ? TestingEndpoint : LiveEndpoing;
                    service.UPSSecurityValue = new UPSSecurity()
                    {
                        ServiceAccessToken = new UPSSecurityServiceAccessToken()
                        {
                            AccessLicenseNumber = UpsPromo.AccessLicenseNumber
                        },
                        UsernameToken = new UPSSecurityUsernameToken()
                        {
                            Username = UpsPromo.Username,
                            Password = UpsPromo.Password
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
                Locale = Locale,
                AgreementAcceptanceCode = acceptanceCode,
                PromoCode = PromoCode
            };

            try
            {
                using (PromoDiscountService service = new PromoDiscountService())
                {
                    // Point the service to the correct endpoint
                    service.Url = UseTestServer ? TestingEndpoint : LiveEndpoing;
                    service.UPSSecurityValue = new UPSSecurity()
                    {
                        ServiceAccessToken = new UPSSecurityServiceAccessToken()
                        {
                            AccessLicenseNumber = UpsPromo.AccessLicenseNumber
                        },
                        UsernameToken = new UPSSecurityUsernameToken()
                        {
                            Username = UpsPromo.Username,
                            Password = UpsPromo.Password
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

        /// <summary>
        /// Ups Promo code information
        /// </summary>
        private UpsPromo UpsPromo { get; }

        /// <summary>
        /// Locale information for the request
        /// </summary>
        private LocaleType Locale { get; }

        /// <summary>
        /// Indicates if the test server should be used instead of the live server
        /// </summary>
        public static bool UseTestServer
        {
            get { return InterapptiveOnly.Registry.GetValue("UpsOltTestServer", false); }
            set { InterapptiveOnly.Registry.SetValue("UpsOltTestServer", value); }
        }
    }
}