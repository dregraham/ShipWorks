using ShipWorks.Shipping.Carriers.Amazon.SFP.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP.Api
{
    /// <summary>
    /// Interface for creating Amazon RateGroup
    /// </summary>
    public interface IAmazonSFPRateGroupFactory
    {
        /// <summary>
        /// Creates a RateGroup from GetEligibleShippingServicesResponse
        /// </summary>
        RateGroup GetRateGroupFromResponse(GetEligibleShippingServicesResponse response);
    }
}