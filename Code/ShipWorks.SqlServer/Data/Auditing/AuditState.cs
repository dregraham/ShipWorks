using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Users.Audit
{
    /// <summary>
    /// Indicates if auditing should be disabled
    /// </summary>
    public enum AuditState
    {
        /// <summary>
        /// Auditing state (enabled\disabled) does not change
        /// </summary>
        Default,

        /// <summary>
        /// Auditing should be full enabled
        /// </summary>
        Enabled,

        /// <summary>
        /// Auditing is disabled for everything under the scope
        /// </summary>
        Disabled,

        /// <summary>
        /// Auditing is enabled, but doesn't include any comlumn data
        /// </summary>
        NoDetails
    }
}
