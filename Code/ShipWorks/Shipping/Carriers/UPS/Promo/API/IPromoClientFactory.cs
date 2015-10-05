namespace ShipWorks.Shipping.Carriers.UPS.Promo.API
{
    /// <summary>
    /// A factory interface for creating IUpsApiPromoClient instances.
    /// </summary>
    public interface IPromoClientFactory
    {
        /// <summary>
        /// Uses the UpsPromo provided to create a web client for communicating with 
        /// the UPS Promo API.
        /// </summary>
        /// <param name="promo">The UPS promo that the client is being created for.</param>
        /// <returns>An IUpsApiPromoClient instance.</returns>
        IUpsApiPromoClient CreatePromoClient(UpsPromo promo);
    }
}