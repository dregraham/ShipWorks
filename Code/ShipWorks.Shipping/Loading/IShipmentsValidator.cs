using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Loading
{
    /// <summary>
    /// Validation component for the shipments loader
    /// </summary>
    public interface IShipmentsValidator
    {
        /// <summary>
        /// Start validating shipments as they are queued
        /// </summary>
        Task<bool> StartTask(IProgressProvider progressProvider,
            IDictionary<long, ShipmentEntity> globalShipments, BlockingCollection<ShipmentEntity> shipmentsQueue);
    }
}