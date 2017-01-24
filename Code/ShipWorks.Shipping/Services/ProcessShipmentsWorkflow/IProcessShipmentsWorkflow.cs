using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Services.ProcessShipmentsWorkflow
{
    /// <summary>
    /// Workflow for processing shipments
    /// </summary>
    public interface IProcessShipmentsWorkflow
    {
        /// <summary>
        /// Process the shipments
        /// </summary>
        Task<IProcessShipmentsWorkflowResult> Process(IEnumerable<ShipmentEntity> shipments,
            RateResult chosenRateResult, Action counterRateCarrierConfiguredWhileProcessingAction);
    }
}