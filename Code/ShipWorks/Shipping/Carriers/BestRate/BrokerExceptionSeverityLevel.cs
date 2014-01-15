using Interapptive.Shared.Utility;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.BestRate
{

    /// <summary>
    /// An enumeration intended to be used in conjunction with broker exceptions
    /// to indicate the severity level of the underlying carrier/shipping exception.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum BrokerExceptionSeverityLevel
    {

        /// <summary>
        /// An informational message, no severity
        /// </summary>
        [Description("Information")]
        [ImageResourceAttribute("information16")]
        Information,

        /// <summary>
        /// A low severity
        /// </summary>
        [Description("Warning")]
        [ImageResourceAttribute("warning16")]
        Warning,

        /// <summary>
        /// A high severity
        /// </summary>
        [Description("Error")]
        [ImageResourceAttribute("error16")]
        Error,
    }
}
