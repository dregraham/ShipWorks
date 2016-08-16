using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon.Api
{
    /// <summary>
    /// Interface for creating Amazon RateGroup
    /// </summary>
    public interface IAmazonRateGroupFactory
    {
        /// <summary>
        /// Creates a RateGroup from GetEligibleShippingServicesResponse
        /// </summary>
        RateGroup GetRateGroupFromResponse(GetEligibleShippingServicesResponse response);
    }
}