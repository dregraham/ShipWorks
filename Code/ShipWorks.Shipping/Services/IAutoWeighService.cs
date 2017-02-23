using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Metrics;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Service to automatically apply weight from an external scale to a shipment
    /// </summary>
    public interface IAutoWeighService
    {
        /// <summary>
        /// Applies the weight on the scale to the specified shipments
        /// </summary>
        Task<bool> Apply(IEnumerable<ShipmentEntity> shipments, ITrackedDurationEvent trackedDurationEvent);
    }
}