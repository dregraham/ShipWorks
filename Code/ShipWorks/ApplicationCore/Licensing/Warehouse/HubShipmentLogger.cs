using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Warehouse;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Class for logging shipments to the hub
    /// </summary>
    [Component]
    public class HubShipmentLogger : IHubShipmentLogger
    {
        private readonly IWarehouseOrderClient warehouseOrderClient;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubShipmentLogger(IWarehouseOrderClient warehouseOrderClient, ISqlAdapterFactory sqlAdapterFactory)
        {
            this.warehouseOrderClient = warehouseOrderClient;
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Log processed shipments to the hub
        /// </summary>
        public async Task LogProcessedShipments(DbConnection connection, CancellationToken cancellationToken)
        {
            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create(connection))
            {
                // todo: add shipment.order.hubOrderID is not null
                var query = new QueryFactory().Shipment
                    .Where(ShipmentFields.Processed == true)
                    .AndWhere(ShipmentFields.Voided == false)
                    .AndWhere(ShipmentFields.LoggedShippedToHub == false)
                    .WithPath(ShipmentEntity.PrefetchPathOrder)
                    .OrderBy(ShipmentFields.ProcessedDate.Descending())
                    .Limit(20);

                EntityCollection<ShipmentEntity> shipmentCollection = new EntityCollection<ShipmentEntity>();

                await sqlAdapter.FetchQueryAsync(query, shipmentCollection, cancellationToken).ConfigureAwait(false);

                foreach (var shipmentToLog in shipmentCollection)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    await warehouseOrderClient.UploadShipment(shipmentToLog, shipmentToLog.Order.HubOrderID.Value,
                                                              shipmentToLog.OnlineShipmentID).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Log voided shipments to the hub
        /// </summary>
        public async Task LogVoidedShipments(DbConnection connection, CancellationToken cancellationToken)
        {
            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create(connection))
            {
                // todo: add shipment.order.hubOrderID is not null
                var query = new QueryFactory().Shipment
                    .Where(ShipmentFields.Voided == true)
                    .AndWhere(ShipmentFields.LoggedShippedToHub == true)
                    .AndWhere(ShipmentFields.LoggedVoidToHub == false)
                    .WithPath(ShipmentEntity.PrefetchPathOrder)
                    .OrderBy(ShipmentFields.ProcessedDate.Descending())
                    .Limit(20);

                EntityCollection<ShipmentEntity> shipmentCollection = new EntityCollection<ShipmentEntity>();

                await sqlAdapter.FetchQueryAsync(query, shipmentCollection, cancellationToken).ConfigureAwait(false);

                foreach (var shipmentToLog in shipmentCollection)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    await warehouseOrderClient.UploadVoid(shipmentToLog.ShipmentID, shipmentToLog.Order.HubOrderID.Value,
                                                          shipmentToLog.OnlineShipmentID).ConfigureAwait(false);
                }
            }
        }
    }
}
