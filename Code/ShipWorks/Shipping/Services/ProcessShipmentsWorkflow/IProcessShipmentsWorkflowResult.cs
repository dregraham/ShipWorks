using System.Collections.Generic;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation;

namespace ShipWorks.Shipping.Services.ProcessShipmentsWorkflow
{
    /// <summary>
    /// Result of performing the process shipment workflow
    /// </summary>
    public interface IProcessShipmentsWorkflowResult
    {
        /// <summary>
        /// Local Rate ValidationResult
        /// </summary>
        ILocalRateValidationResult LocalRateValidationResult { get; set; }

        /// <summary>
        /// List of new errors
        /// </summary>
        IList<string> NewErrors { get; }

        /// <summary>
        /// Order hashes
        /// </summary>
        IList<string> OrderHashes { get; }

        /// <summary>
        /// Out of funds exception that may have been generated while processing
        /// </summary>
        IInsufficientFunds OutOfFundsException { get; }

        /// <summary>
        /// Terms and conditions exception that may have been generated while processing
        /// </summary>
        ITermsAndConditionsException TermsAndConditionsException { get; }

        /// <summary>
        /// Is this for Worldship
        /// </summary>
        bool WorldshipExported { get; }
    }
}