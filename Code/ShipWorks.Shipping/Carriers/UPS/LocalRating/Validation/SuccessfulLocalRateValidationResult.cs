using ShipWorks.Shipping.Services.ProcessShipmentsWorkflow;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    /// <summary>
    /// Result of a successful validation. HandleValidationFailure does nothing.
    /// </summary>
    public class SuccessfulLocalRateValidationResult : ILocalRateValidationResult
    {
        /// <summary>
        /// Success!
        /// </summary>
        public void HandleValidationFailure(IProcessShipmentsWorkflowResult workflowResult)
        {
            // no-op
        }
    }
}
