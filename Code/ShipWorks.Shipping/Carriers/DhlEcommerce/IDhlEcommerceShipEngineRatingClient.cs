using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce
{
    /// <summary>
    /// Dhl Ecommerce ShipEngine rating client
    /// </summary>
    public interface IDhlEcommerceShipEngineRatingClient
    {
        /// <summary>
        /// Get rates from DHL Ecommerce via ShipEngine
        /// </summary>
        RateGroup GetRates(ShipmentEntity shipment);
    }
}
