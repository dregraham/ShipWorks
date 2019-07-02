using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Interface for logging shipments to the hub
    /// </summary>
    public interface IHubShipmentLogger
    {
        /// <summary>
        /// Log processed shipments to the hub
        /// </summary>
        Task LogProcessedShipments(IEnumerable<ShipmentEntity> shipments);

        /// <summary>
        /// Log voided shipments to the hub
        /// </summary>
        Task LogVoidedShipments(IEnumerable<ShipmentEntity> shipments);
    }
}
