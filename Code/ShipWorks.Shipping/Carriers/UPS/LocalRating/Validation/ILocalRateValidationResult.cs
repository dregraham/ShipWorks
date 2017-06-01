using ShipWorks.Shipping.Services.ProcessShipmentsWorkflow;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    /// <summary>
    /// The result of a validation
    /// </summary>
    public interface ILocalRateValidationResult
    {
        /// <summary>
        /// Inserts a message into the 
        /// </summary>
        void PrependMessageToWorkflowResultErrors(IProcessShipmentsWorkflowResult workflowResult);

        /// <summary>
        /// Shows the validation message.
        /// </summary>
        void ShowMessage();
    }
}