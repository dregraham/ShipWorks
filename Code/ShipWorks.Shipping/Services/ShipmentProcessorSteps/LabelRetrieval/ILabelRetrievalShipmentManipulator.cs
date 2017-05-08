using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Common;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps.LabelRetrieval
{
    /// <summary>
    /// Modify the shipment before attempting to get a label
    /// </summary>
    [Service]
    public interface ILabelRetrievalShipmentManipulator : IManipulator<ShipmentEntity>
    {
    }
}
