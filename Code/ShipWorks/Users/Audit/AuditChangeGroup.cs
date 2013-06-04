using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Users.Audit
{
    /// <summary>
    /// When multiple changes are filed under a single Audit entry, all the changes must belong to a single group.
    /// For instance, you should be able to change a Template in the same transaction as you changed an Order.
    /// </summary>
    public enum AuditChangeGroup
    {
        /// <summary>
        /// Change group for things that change only on there own, and shouldnt have more than one change detail per-audit.
        /// </summary>
        Standalone,

        /// <summary>
        /// Change group for all things related to an order, such as Customer, Shipments, Items, etc.
        /// </summary>
        Orders
    }
}
