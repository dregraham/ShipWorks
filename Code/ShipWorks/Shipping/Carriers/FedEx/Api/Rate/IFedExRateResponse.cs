using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate
{
    /// <summary>
    /// Rate response for FedEx
    /// </summary>
    public interface IFedExRateResponse
    {
        /// <summary>
        /// Performs any processing required based on the response from the carrier.
        /// </summary>
        GenericResult<RateReply> Process();
    }
}