using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Response;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request
{
    /// <summary>
    /// An implementation of the CarrierRequest that issues FedEx RateRequest request types.
    /// </summary>
    public interface IFedExRateRequest
    {
        /// <summary>
        /// Submits the request to the carrier API
        /// </summary>
        IFedExRateResponse Submit(IShipmentEntity shipment);

        /// <summary>
        /// Submits the request to the carrier API
        /// </summary>
        IFedExRateResponse Submit(IShipmentEntity shipment, FedExRateRequestOptions options);
    }
}