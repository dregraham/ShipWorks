using System;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.API
{
    /// <summary>
    /// Client for interacting with UPS Promo API
    /// </summary>
    public class UpsApiPromoClient : IUpsApiPromoClient
    {
        private readonly UpsPromo promo;

        public UpsApiPromoClient(UpsPromo promo)
        {
            this.promo = promo;
        }

        public PromoAcceptanceTerms GetAgreement()
        {

            throw new NotImplementedException();
        }

        public PromoActivation Activate(string acceptanceCode)
        {
            throw new NotImplementedException();
        }
    }
}