﻿using Interapptive.Shared.Threading;
using ShipWorks.Stores.Platforms.ShipEngine.Apollo;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.ShipEngine
{
    public interface IPlatformOrderWebClient
    {
        /// <summary>
        /// Executes a request for more orders
        /// </summary>
        Task<PaginatedPlatformServiceResponseOfOrderSourceApiSalesOrder> GetOrders(string orderSourceId, string continuationToken);

        /// <summary>
        /// Progress reporter that will be used for requests
        /// </summary>
        IProgressReporter Progress { get; set; }
    }
}