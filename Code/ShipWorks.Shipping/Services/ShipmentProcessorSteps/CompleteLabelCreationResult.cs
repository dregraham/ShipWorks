using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps
{
    /// <summary>
    /// Results of completing the label creation process
    /// </summary>
    public class CompleteLabelCreationResult : ICompleteLabelCreationResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CompleteLabelCreationResult(ILabelPersistenceResult result, bool worldshipExported, string errorMessage,
            IInsufficientFunds outOfFundsException, ITermsAndConditionsException termsAndConditionsException)
        {
            OriginalShipment = result.OriginalShipment;
            Canceled = result.Canceled;
            WorldshipExported = worldshipExported;
            ErrorMessage = errorMessage;
            OutOfFundsException = outOfFundsException;
            TermsAndConditionsException = termsAndConditionsException;
        }

        /// <summary>
        /// Shipment being processed
        /// </summary>
        public ShipmentEntity OriginalShipment { get; }

        /// <summary>
        /// Error message generated from processing
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// Out of funds exception, if there was one
        /// </summary>
        public IInsufficientFunds OutOfFundsException { get; }

        /// <summary>
        /// Terms and conditions exception, if there was one
        /// </summary>
        public ITermsAndConditionsException TermsAndConditionsException { get; }

        /// <summary>
        /// Is exported by WorldShip
        /// </summary>
        public bool WorldshipExported { get; }

        /// <summary>
        /// Was the process canceled
        /// </summary>
        public bool Canceled { get; }

        /// <summary>
        /// Was the process successful
        /// </summary>
        public bool Success => string.IsNullOrWhiteSpace(ErrorMessage) && !Canceled;
    }
}