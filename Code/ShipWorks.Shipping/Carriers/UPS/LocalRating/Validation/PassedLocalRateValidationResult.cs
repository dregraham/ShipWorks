using ShipWorks.Shipping.Services.ProcessShipmentsWorkflow;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    /// <summary>
    /// 
    /// </summary>
    public class PassedLocalRateValidationResult : ILocalRateValidationResult
    {
        /// <summary>
        /// Success!
        /// </summary>
        public void HandleValidationFailure(IProcessShipmentsWorkflowResult workflowResult)
        {
            // no-opp
        }
    }
}
