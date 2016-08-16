using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Interface for carriers that support getting rates from Express1
    /// </summary>
    public interface ISupportExpress1Rates
    {
        /// <summary>
        /// Gets rate and includes express1 if shouldRetrieveExpress1Rates is true
        /// </summary>
        RateGroup GetRates(ShipmentEntity shipment, bool shouldRetrieveExpress1Rates);
    }
}