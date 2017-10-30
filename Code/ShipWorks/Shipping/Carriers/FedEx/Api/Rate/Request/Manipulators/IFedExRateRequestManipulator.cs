using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators
{
    /// <summary>
    /// Manipulators for a FedEx rate request
    /// </summary>
    [Service]
    public interface IFedExRateRequestManipulator
    {
        /// <summary>
        /// Manipulate the given request
        /// </summary>
        RateRequest Manipulate(IShipmentEntity shipment, RateRequest request);

        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        bool ShouldApply(IShipmentEntity shipment, FedExRateRequestOptions options);
    }
}
