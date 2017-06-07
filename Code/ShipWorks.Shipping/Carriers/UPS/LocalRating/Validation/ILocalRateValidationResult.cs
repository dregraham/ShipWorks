using System.Collections.Generic;
using ShipWorks.Shipping.Services.ProcessShipmentsWorkflow;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    /// <summary>
    /// The result of a validation
    /// </summary>
    public interface ILocalRateValidationResult
    {
        /// <summary>
        /// Rate discrepancies associated with this result
        /// </summary>
        IEnumerable<UpsLocalRateDiscrepancy> RateDiscrepancies { get; }

        /// <summary>
        /// Handles a validation failure.
        /// </summary>
        void HandleValidationFailure(IProcessShipmentsWorkflowResult workflowResult);
    }
}