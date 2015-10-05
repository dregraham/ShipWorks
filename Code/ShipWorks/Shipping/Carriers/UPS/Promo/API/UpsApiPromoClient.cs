using System;
using ShipWorks.Stores.Platforms.PayPal.WebServices;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.API
{
    /// <summary>
    /// Client for interacting with UPS Promo API
    /// </summary>
    public class UpsApiPromoClient
    {
        public UpsApiPromoClient()
        {
                
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