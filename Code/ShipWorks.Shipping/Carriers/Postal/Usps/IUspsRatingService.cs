using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Rating service for the Usps carrier
    /// </summary>
    public interface IUspsRatingService
    {
        /// <summary>
        /// Gets rates for the given shipment
        /// </summary>
        RateGroup GetRates(ShipmentEntity shipment, TelemetricResult<IDownloadedLabelData> retrieveExpress1Rates);

        /// <summary>
        /// Get rates includes Express1 rates if specified
        /// </summary>
        RateGroup GetRates(ShipmentEntity shipment, bool retrieveExpress1Rates);
    }
}