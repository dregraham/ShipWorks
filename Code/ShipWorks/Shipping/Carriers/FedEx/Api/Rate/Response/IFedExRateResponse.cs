using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Response
{
    /// <summary>
    /// Rate response for FedEx
    /// </summary>
    public interface IFedExRateResponse
    {
        /// <summary>
        /// Performs any processing required based on the response from the carrier.
        /// </summary>
        RateReply Process();
    }
}