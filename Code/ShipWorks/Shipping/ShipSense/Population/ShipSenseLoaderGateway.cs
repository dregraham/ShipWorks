using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.ShipSense.Packaging;
using ShipWorks.SqlServer.Common.Data;
using ShipWorks.Stores.Content;

namespace ShipWorks.Shipping.ShipSense.Population
{
    /// <summary>
    ///  Implementation for the ShipSenseLoader to get/save database entries
    /// </summary>
    public class ShipSenseLoaderGateway : IShipSenseLoaderGateway
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ShipSenseLoaderGateway));
        private SqlConnection connection; 
        private IKnowledgebase knowledgebase;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipSenseLoaderGateway(IKnowledgebase knowledgebase)
        {
            this.knowledgebase = knowledgebase;
        }

        /// <summary>
        /// Opens the current connection if it is not already open
        /// </summary>
        private void OpenConnection()
        {
            if (connection == null)
            {
                connection = new SqlConnection(SqlAdapter.Default.ConnectionString);
            }

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        /// <summary>
        /// Gets the ShippingSettingsEntity
        /// </summary>
        public ShippingSettingsEntity FetchShippingSettingsEntity()
        {
            return ShippingSettings.Fetch();
        }

        /// <summary>
        /// Saves the ShippingSettingsEntity
        /// </summary>
        public void SaveShippingSettings(ShippingSettingsEntity shippingSettings)
        {
            ShippingSettings.Save(shippingSettings);
        }

        /// <summary>
        /// Gets the next shipment to process based on ShippingSettings
        /// </summary>
        public ShipmentEntity FetchNextShipmentToProcess()
        {
            ShipmentEntity shipment = null;
            /*         
            select max(s.shipmentID)
            from Shipment s with(nolock), [Order] o with(nolock)
            where s.shipmentID <= @endShipmentID 
              and s.ShipmentID > @processedShipmentID 
              and s.Processed = 1
              and s.OrderID = o.OrderID
            group by o.ShipSenseHashKey  
            order by max(s.shipmentID) asc       
             */

            ShippingSettingsEntity shippingSettings = FetchShippingSettingsEntity();
            long endShipmentID = shippingSettings.ShipSenseEndShipmentID;
            long lastProcessedShipmentID = shippingSettings.ShipSenseProcessedShipmentID;

            RelationPredicateBucket shipmentBucket = new RelationPredicateBucket();
            shipmentBucket.Relations.Add(new EntityRelation(ShipmentFields.OrderID, OrderFields.OrderID, RelationType.ManyToOne));

            shipmentBucket.PredicateExpression.Add(ShipmentFields.Processed == true);
            shipmentBucket.PredicateExpression.Add(ShipmentFields.ShipmentID <= endShipmentID);
            shipmentBucket.PredicateExpression.Add(ShipmentFields.ShipmentID > lastProcessedShipmentID);

            GroupByCollection groupByCollection = new GroupByCollection();
            groupByCollection.Add(OrderFields.ShipSenseHashKey);

            ResultsetFields resultFields = new ResultsetFields(1);
            resultFields.DefineField(ShipmentFields.ShipmentID, 0, "ShipmentID", "");
            resultFields[0].AggregateFunctionToApply = AggregateFunction.Max;

            IExpression datePart = new DbFunctionCall("MAX({0})", new object[] { ShipmentFields.ShipmentID });
            IEntityField2 datePartField = ShipmentFields.ShipmentID.SetExpression(datePart);

            ISortClause datePartSortClause = new SortClause(datePartField, null, SortOperator.Ascending);
            SortExpression sort = new SortExpression(datePartSortClause);

            OpenConnection();
            using (SqlAdapter sqlAdapter = new SqlAdapter(connection))
            {
                using (SqlDataReader sqlDataReader = (SqlDataReader) sqlAdapter.FetchDataReader(resultFields, shipmentBucket, CommandBehavior.SequentialAccess, 1, sort, groupByCollection, false, 1, 1))
                {
                    while (sqlDataReader.Read())
                    {
                        long shipmentID = sqlDataReader.GetInt64(0);

                        if (shipmentID != 0)
                        {
                            shipment = new ShipmentEntity(shipmentID);
                            SqlAdapter.Default.FetchEntity(shipment);
                            ShippingManager.EnsureShipmentLoaded(shipment);
                        }
                    }
                }
            }

            return shipment;
        }

        /// <summary>
        /// Saves the shipment data to the ShipSense knowledge base. All exceptions will be caught
        /// and logged and wrapped in a ShippingException.
        /// </summary>
        public void Save(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            try
            {
                ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);
                IEnumerable<IPackageAdapter> packageAdapters = shipmentType.GetPackageAdapters(shipment);

                // Make sure we have all of the order information
                OrderEntity order = (OrderEntity)DataProvider.GetEntity(shipment.OrderID);

                using (TransactionScope scope = new TransactionScope())
                {
                    OpenConnection();
                    using (SqlAdapter adapter = new SqlAdapter(connection))
                    {
                        adapter.FetchEntityCollection(order.OrderItems, new RelationPredicateBucket(OrderItemFields.OrderID == order.OrderID));
                    }

                    // Apply the data from the package adapters and the customs items to the knowledge base 
                    // entry, so the shipment data will get saved to the knowledge base; the knowledge base
                    // is smart enough to know when to save the customs items associated with an entry.
                    KnowledgebaseEntry entry = new KnowledgebaseEntry();
                    entry.ApplyFrom(packageAdapters, shipment.CustomsItems);

                    knowledgebase.Save(entry, order);

                    ShippingSettingsEntity shippingSettings = ShippingSettings.Fetch();
                    shippingSettings.ShipSenseProcessedShipmentID = shipment.ShipmentID;
                    ShippingSettings.Save(shippingSettings);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                // We may want to eat this exception entirely, so the user isn't impacted 
                log.ErrorFormat("An error occurred writing shipment ID {0} to the knowledge base: {1}", shipment.ShipmentID, ex.Message);
                throw new ShippingException("The shipment was processed successfully, but the data was not logged to the knowledge base.", ex);
            }
        }

        /// <summary>
        /// Gets an OrderEntity that doesn't have a ShipSenseHashKey
        /// </summary>
        public OrderEntity FetchNextOrderOrderToProcess()
        {
            // Create an order with an ID of zero to force the gateway to use the first order it
            // encounters
            OrderEntity simulatedPreviousOrder = new OrderEntity(0);
            return FetchNextOrderOrderToProcess(simulatedPreviousOrder);
        }


        /// <summary>
        /// Gets an OrderEntity that doesn't have a ShipSenseHashKey and is not the same as the previous order. 
        /// This overload is useful when you want to bypass the previous order if it is returned again, 
        /// ensure that you don't get into an infinite loop state
        /// </summary>
        /// <param name="previousOrder">The previous order.</param>
        public OrderEntity FetchNextOrderOrderToProcess(OrderEntity previousOrder)
        {
            OrderEntity orderEntity = null;

            RelationPredicateBucket orderBucket = new RelationPredicateBucket();
            orderBucket.PredicateExpression.Add(OrderFields.OrderID > previousOrder.OrderID);
            orderBucket.PredicateExpression.Add(OrderFields.ShipSenseHashKey == string.Empty);

            using (OrderCollection orders = new OrderCollection())
            {
                OpenConnection();
                using (SqlAdapter sqlAdapter = new SqlAdapter(connection))
                {
                    sqlAdapter.FetchEntityCollection(orders, orderBucket, 1);

                    if (orders.Any())
                    {
                        orderEntity = orders.First();
                        OrderUtility.PopulateOrderDetails(orderEntity);
                    }
                }
            }

            return orderEntity;
        }

        /// <summary>
        /// Saves an OrderEntity
        /// </summary>
        public void SaveOrder(OrderEntity order)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                OpenConnection();
                using (SqlAdapter sqlAdapter = new SqlAdapter(connection))
                {
                    sqlAdapter.SaveEntity(order, false, false);
                }

                scope.Complete();
            }
        }

        /// <summary>
        /// Gets a sql app lock for loading ShipSense data
        /// </summary>
        public bool GetAppLock(string appLockName)
        {
            OpenConnection();
            return SqlAppLockUtility.AcquireLock(connection, appLockName);
        }

        /// <summary>
        /// Releases the ShipSense loading sql app lock
        /// </summary>
        public void ReleaseAppLock(string appLockName)
        {
            OpenConnection();
            SqlAppLockUtility.ReleaseLock(connection, appLockName);
        }

        /// <summary>
        /// Dispose of any resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose of any resources
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                connection = null;
            }
        }
    }
}
