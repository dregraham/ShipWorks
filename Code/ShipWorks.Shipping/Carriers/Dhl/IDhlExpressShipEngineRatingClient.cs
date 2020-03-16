using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Dhl Express ShipEngine rating client
    /// </summary>
    public interface IDhlExpressShipEngineRatingClient
    {
        /// <summary>
        /// Get rates from DHL Express via ShipEngine
        /// </summary>
        RateGroup GetRates(ShipmentEntity shipment);
    }
}
