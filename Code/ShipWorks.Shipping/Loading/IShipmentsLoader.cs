using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Loading
{
    /// <summary>
    /// Interface for loading shipments
    /// </summary>
    public interface IShipmentsLoader
    {
        /// <summary>
        /// Start the task to load shipments
        /// </summary>
        Task<bool> StartTask(IProgressProvider progressProvider, List<long> orderIDs,
            IDictionary<long, ShipmentEntity> globalShipments, BlockingCollection<ShipmentEntity> shipmentsToValidate);
    }
}
