using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping
{
    /// <summary>
    /// Manipulator for a FedEx label request
    /// </summary>
    [Service]
    public interface IFedExShipRequestManipulator
    {
        /// <summary>
        /// Manipulate the given request
        /// </summary>
        GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber);

        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        bool ShouldApply(IShipmentEntity shipment);
    }
}
