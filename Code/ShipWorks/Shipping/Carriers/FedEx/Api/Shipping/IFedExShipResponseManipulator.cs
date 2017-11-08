using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping
{
    /// <summary>
    /// Manipulator for a FedEx label response
    /// </summary>
    [Service]
    public interface IFedExShipResponseManipulator
    {
        /// <summary>
        /// Manipulate the given shipment
        /// </summary>
        GenericResult<ShipmentEntity> Manipulate(ProcessShipmentReply response, ProcessShipmentRequest request, ShipmentEntity shipment);
    }
}
