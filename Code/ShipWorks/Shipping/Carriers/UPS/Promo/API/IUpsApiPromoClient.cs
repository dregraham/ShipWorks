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
}