using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Metrics;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Service to automatically apply weight from an external scale to a shipment
    /// </summary>
    
    public interface IAutoWeighService
    {
        /// <summary>
        /// Applies the weight on the scale to the specified shipments
        /// </summary>
        Task<bool> ApplyWeights(IEnumerable<ShipmentEntity> shipments, ITrackedDurationEvent trackedDurationEvent);
    }
}