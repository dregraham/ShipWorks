using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Users.Security
{
    /// <summary>
    /// Controls the scope of what a PermissionType locks down
    /// </summary>
    public enum PermissionScope
    {
        /// <summary>
        /// PermissionType's of this scope are either granted or not. Does not depend on what orders are selected.
        /// </summary>
        Global,

        /// <summary>
        /// PermissionType's of this scope are granted per-store, and depend on what is selected.
        /// </summary>
        Store,

        /// <summary>
        /// PermissionType's that cannot be directly determined until the actual object is examined.  For instance,
        /// the actual permission to be applied to a note depends on if the note is related to a customer or an order.
        /// </summary>
        IndirectRelatedObject,

        /// <summary>
        /// PermissionType's that automatically choose which actual permission to use based on the type of the entity 
        /// that has security requested on it.
        /// </summary>
        IndirectEntityType
    }
}
