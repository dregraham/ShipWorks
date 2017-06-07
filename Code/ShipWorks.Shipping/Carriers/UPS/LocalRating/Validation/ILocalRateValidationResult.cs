using ShipWorks.Shipping.Services.ProcessShipmentsWorkflow;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    /// <summary>
    /// The result of a validation
    /// </summary>
    public interface ILocalRateValidationResult
    {
        /// <summary>
        /// Handles a validation failure.
        /// </summary>
        void HandleValidationFailure(IProcessShipmentsWorkflowResult workflowResult);
    }
}