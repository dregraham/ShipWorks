﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Interface for the order lookup confirmation service
    /// </summary>
    [Service]
    public interface IOrderLookupConfirmationService
    {
        /// <summary>
        /// Confirm a list of orders. Return the selected order or null
        /// </summary>
        Task<long?> ConfirmOrder(string searchText, IEnumerable<long> list);
    }
}