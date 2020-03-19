using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Dhl Express Stamps rating client
    /// </summary>
    public interface IDhlExpressStampsRatingClient
    {
        /// <summary>
        /// Get rates from DHL Express via Stamps.com
        /// </summary>
        RateGroup GetRates(ShipmentEntity shipment);
    }
}
