using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Common;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps.GetLabel
{
    /// <summary>
    /// Modify the shipment before attempting to get a label
    /// </summary>
    [Service]
    public interface IGetLabelManipulator : IManipulator<ShipmentEntity>
    {
    }
}
