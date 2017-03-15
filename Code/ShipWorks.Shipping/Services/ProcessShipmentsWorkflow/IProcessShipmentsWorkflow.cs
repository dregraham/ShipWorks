﻿using System;
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
        /// Get the name of the workflow
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Process the shipments
        /// </summary>
        Task<IProcessShipmentsWorkflowResult> Process(IEnumerable<ShipmentEntity> shipments,
            RateResult chosenRateResult, Action counterRateCarrierConfiguredWhileProcessingAction);

        /// <summary>
        /// Concurrent number of tasks used for processing shipments
        /// </summary>
        int ConcurrencyCount { get; }
    }
}