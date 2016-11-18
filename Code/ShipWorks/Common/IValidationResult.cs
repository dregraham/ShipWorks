using System.Collections.Generic;

namespace ShipWorks.Common
{
    /// <summary>
    /// Results of applying validators to an object
    /// </summary>
    public interface IApplyValidatorsResult
    {
        /// <summary>
        /// Was validation successful
        /// </summary>
        bool Success { get; }

        /// <summary>
        /// Did validation fail
        /// </summary>
        bool Failure { get; }

        /// <summary>
        /// Collection of errors if validation failed
        /// </summary>
        IEnumerable<string> Errors { get; }
    }
}
