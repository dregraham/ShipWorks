using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Common;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps.LabelRetrieval
{
    /// <summary>
    /// Validates a shipment before getting a label
    /// </summary>
    [Service]
    public interface ILabelRetrievalShipmentValidator : IValidator<ShipmentEntity>
    {
    }
}
