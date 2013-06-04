using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model;

namespace ShipWorks.Users.Security
{
    /// <summary>
    /// Specifies the target entity type of a PermissionType
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class PermissionScopeAttribute : Attribute
    {
        PermissionScope scope;

        /// <summary>
        /// Constructor
        /// </summary>
        public PermissionScopeAttribute(PermissionScope scope)
        {
            this.scope = scope;
        }

        /// <summary>
        /// Type of scope that the permission secures
        /// </summary>
        public PermissionScope Scope
        {
            get { return scope; }
        }
    }
}
