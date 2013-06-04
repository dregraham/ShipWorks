using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Users.Audit
{
    /// <summary>
    /// Indicates if auditing should be disabled
    /// </summary>
    public enum AuditBehaviorDisabledState
    {
        /// <summary>
        /// Auditing state (enabled\disabled) does not change
        /// </summary>
        Default,

        /// <summary>
        /// Auditing is disabled for everything under the scope
        /// </summary>
        Disabled
    }
}
