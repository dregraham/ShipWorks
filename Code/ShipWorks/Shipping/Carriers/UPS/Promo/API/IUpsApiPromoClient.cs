using log4net;
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
        private ILog log;
        private UpsPromo promo;

        public FakeUpsApiPromoClient(UpsPromo promo)
        {
            log = LogManager.GetLogger(typeof(FakeUpsApiPromoClient));
            log.Info($"FakeUpsApiPromoClient recieved a promo with account {promo.AccountNumber}.");

            this.promo = promo;
        }

        public PromoActivation Activate(string acceptanceCode)
        {
            log.Info($"FakeUpsApiPromoClient.Activate({acceptanceCode})");

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
            string acceptanceCode = "42";
            log.Info($"FakeUpsApiPromoClient.GetAgreement() returning acceptance code of {acceptanceCode}, AgreementURL of www.google.com and PromoDescription of \"Mocked Descriptoin\"");

            return new PromoAcceptanceTerms(new PromoDiscountAgreementResponse()
            {
                PromoAgreement = new PromoAgreementType()
                {
                    AgreementURL = "www.google.com",
                    AcceptanceCode = acceptanceCode
                },
                PromoDescription = "Mocked Description"
            });
        }
    }
}