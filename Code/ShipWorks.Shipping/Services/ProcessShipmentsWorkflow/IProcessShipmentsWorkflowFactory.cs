using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services.ProcessShipmentsWorkflow
{
    /// <summary>
    /// Factory for creating the correct process shipments workflow
    /// </summary>
    public interface IProcessShipmentsWorkflowFactory
    {
        /// <summary>
        /// Create the correct workflow
        /// </summary>
        IProcessShipmentsWorkflow Create(IEnumerable<ShipmentEntity> shipments);
    }
}