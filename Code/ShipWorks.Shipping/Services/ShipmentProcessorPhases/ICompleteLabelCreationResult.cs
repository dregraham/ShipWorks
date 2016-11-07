using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;

namespace ShipWorks.Shipping.Services.ShipmentProcessorPhases
{
    /// <summary>
    /// Results of completing the label creation process
    /// </summary>
    public interface ICompleteLabelCreationResult
    {
        /// <summary>
        /// Shipment being processed
        /// </summary>
        ShipmentEntity OriginalShipment { get; }

        /// <summary>
        /// Error message generated from processing
        /// </summary>
        string ErrorMessage { get; }

        /// <summary>
        /// Out of funds exception, if there was one
        /// </summary>
        IInsufficientFunds OutOfFundsException { get; }

        /// <summary>
        /// Terms and conditions exception, if there was one
        /// </summary>
        ITermsAndConditionsException TermsAndConditionsException { get; }

        /// <summary>
        /// Is exported by WorldShip
        /// </summary>
        bool WorldshipExported { get; }

        /// <summary>
        /// Was the process canceled
        /// </summary>
        bool Canceled { get; }

        /// <summary>
        /// Was the process successful
        /// </summary>
        bool Success { get; }
    }
}