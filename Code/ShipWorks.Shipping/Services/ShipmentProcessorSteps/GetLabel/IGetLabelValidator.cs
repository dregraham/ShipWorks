using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Common;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps.GetLabel
{
    /// <summary>
    /// Validates a shipment before getting a label
    /// </summary>
    [Service]
    public interface IGetLabelValidator : IValidator<ShipmentEntity>
    {
    }
}
