using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

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
        /// Log voided shipments to the hub
        /// </summary>
        Task LogVoidedShipments(DbConnection connection, CancellationToken cancellationToken);
    }
}
