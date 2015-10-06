using ShipWorks.Shipping.Carriers.UPS.WebServices.Promo;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.API
{
    /// <summary>
    /// Interface for UPS Promo Client
    /// </summary>
    public interface IUpsApiPromoClient
    {
        /// <summary>
        /// Activates the UPS Promo using the given code
        /// </summary>
        PromoActivation Activate(string acceptanceCode);

        /// <summary>
        /// Gets the Promo Acceptance Terms
        /// </summary>
        PromoAcceptanceTerms GetAgreement();
    }

    // Todo: get rid of this
    class FakeUpsApiPromoClient : IUpsApiPromoClient
    {
        public PromoActivation Activate(string acceptanceCode)
        {
            return new PromoActivation(new PromoDiscountResponse()
            {
                Response = new ResponseType()
                {
                    ResponseStatus = new CodeDescriptionType()
                    {
                        Code = "1"
                    }
                }
            });
        }

        public PromoAcceptanceTerms GetAgreement()
        {
            return new PromoAcceptanceTerms(new PromoDiscountAgreementResponse()
            {
                PromoAgreement = new PromoAgreementType()
                {
                    AgreementURL = "www.google.com",
                    AcceptanceCode = "42"
                },
                PromoDescription = "Mocked Description"
            });
        }
    }
}