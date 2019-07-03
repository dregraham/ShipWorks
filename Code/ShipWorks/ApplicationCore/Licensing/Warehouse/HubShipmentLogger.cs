using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
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
                var queryFactory = new QueryFactory();
                var query = queryFactory.Shipment
                    .From(QueryTarget.InnerJoin(queryFactory.Order)
                        .On(ShipmentFields.OrderID == OrderFields.OrderID))
                    .Where(ShipmentFields.Processed == true)
                    .AndWhere(ShipmentFields.Voided == false)
                    .AndWhere(ShipmentFields.LoggedShippedToHub == false)
                    .AndWhere(OrderFields.HubOrderID.IsNotNull())
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
                }
            }
        }

        /// <summary>
        /// Logs a single system
        /// </summary>
        public async Task LogProcessedShipment(ShipmentEntity shipmentToLog, ISqlAdapter sqlAdapter)
        {
            if (shipmentToLog.Order.HubOrderID.HasValue)
            {
                Result uploadResult = await warehouseOrderClient.UploadShipment(
                    shipmentToLog, shipmentToLog.Order.HubOrderID.Value,
                    shipmentToLog.OnlineShipmentID).ConfigureAwait(false);

                if (uploadResult.Success)
                {
                    ShipmentEntity shipmentToUpdate = new ShipmentEntity {LoggedShippedToHub = true};
                    sqlAdapter.UpdateEntitiesDirectly(shipmentToUpdate,
                        new RelationPredicateBucket(
                            new PredicateExpression(
                                ShipmentFields.ShipmentID ==
                                shipmentToLog.ShipmentID)));
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

                    Result uploadResult = await warehouseOrderClient.UploadVoid(
                        shipmentToLog.ShipmentID, shipmentToLog.Order.HubOrderID.Value,
                        shipmentToLog.OnlineShipmentID).ConfigureAwait(false);

                    if (uploadResult.Success)
                    {
                        ShipmentEntity shipmentToUpdate = new ShipmentEntity { LoggedVoidToHub = true };
                        sqlAdapter.UpdateEntitiesDirectly(shipmentToUpdate,
                                                          new RelationPredicateBucket(
                                                              new PredicateExpression(
                                                                  ShipmentFields.ShipmentID ==
                                                                  shipmentToLog.ShipmentID)));
                    }
                }
            }
        }
    }
}
