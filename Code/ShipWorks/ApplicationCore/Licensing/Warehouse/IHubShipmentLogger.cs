using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using ShipWorks.Data.Connection;
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
        Task LogProcessedShipments(DbConnection connection, CancellationToken cancellationToken);

        /// <summary>
        /// Log processed shipment to the hub
        /// </summary>
        Task LogProcessedShipment(ShipmentEntity shipmentToLog, string resultValue, ISqlAdapter sqlAdapter);

        /// <summary>
        /// Log voided shipments to the hub
        /// </summary>
        Task LogVoidedShipments(DbConnection connection, CancellationToken cancellationToken);

        /// <summary>
        /// Log voided shipment to the hub
        /// </summary>
        Task LogVoidedShipment(ShipmentEntity shipmentToLog, ISqlAdapter sqlAdapter);
    }
}
