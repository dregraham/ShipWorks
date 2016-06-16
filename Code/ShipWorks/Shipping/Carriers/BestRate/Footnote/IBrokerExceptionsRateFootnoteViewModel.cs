using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.BestRate.Footnote
{
    /// <summary>
    /// Footnote to display broker exceptions
    /// </summary>
    public interface IBrokerExceptionsRateFootnoteViewModel
    {
        /// <summary>
        /// List of exceptions to display
        /// </summary>
        IEnumerable<BrokerException> BrokerExceptions { get; set; }

        /// <summary>
        /// Severity level of the exceptions
        /// </summary>
        BrokerExceptionSeverityLevel SeverityLevel { get; set; }
    }
}