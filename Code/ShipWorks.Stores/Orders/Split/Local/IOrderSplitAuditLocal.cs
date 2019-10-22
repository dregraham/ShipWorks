﻿using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Orders.Split.Local
{
    /// <summary>
    /// Interface for auditing split orders
    /// </summary>
    public interface IOrderSplitAuditLocal
    {
        /// <summary>
        /// Audit the original order and split order.
        /// </summary>
        Task Audit(IOrderEntity originalOrder, IOrderEntity splitOrder);
    }
}
