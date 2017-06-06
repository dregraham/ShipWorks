using System.Collections.Generic;
using ShipWorks.Shipping.Services.ProcessShipmentsWorkflow;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    /// <summary>
    /// Result of a successful validation. HandleValidationFailure does nothing.
    /// </summary>
    public class SuccessfulLocalRateValidationResult : ILocalRateValidationResult
    {
        /// <summary>
        /// Rate discrepancies associated with this result
        /// </summary>
        public IEnumerable<UpsLocalRateDiscrepancy> RateDiscrepancies => new List<UpsLocalRateDiscrepancy>();

        /// <summary>
        /// Success!
        /// </summary>
        public void HandleValidationFailure(IProcessShipmentsWorkflowResult workflowResult)
        {
            // no-op
        }

        /// <summary>
        /// Gets the user friendly message.
        /// </summary>
        public string GetUserFriendlyMessage() => string.Empty;
    }
}
