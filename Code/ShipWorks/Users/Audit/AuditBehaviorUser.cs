using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Users.Audit
{
    /// <summary>
    /// Indicates if the normal user should be used, or the super user
    /// </summary>
    public enum AuditBehaviorUser
    {
        /// <summary>
        /// Runs under whatever user is currently running, either the logged in user, or the SuperUser if its already in scope.
        /// </summary>
        Default,

        /// <summary>
        /// Runs everything under the SuperUser
        /// </summary>
        SuperUser
    }
}
