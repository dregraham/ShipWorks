using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Interapptive.Shared.Data;
using ShipWorks.Users.Audit;
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
        private readonly IKnowledgebase knowledgebase;

        private OrderEntity previousProcessedOrder;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipSenseLoaderGateway(IKnowledgebase knowledgebase)
        {
            this.knowledgebase = knowledgebase;
            previousProcessedOrder = new OrderEntity(0);
        }

        /// <summary>
        /// Opens the current connection if it is not already open. This is primarily 
        /// intended for maintaining the connection to the applock.
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
        /// Gets the total shipments to analyze when building the knowledge base.
        /// </summary>
        public int TotalShipmentsToAnalyze
        {
            get
            {
                try
                {
                    ShippingSettingsEntity shippingSettings = ShippingSettings.Fetch();
                    int total = 0;

                    using (SqlConnection conn = SqlSession.Current.OpenConnection())
                    {
                        using (SqlCommand command = SqlCommandProvider.Create(conn))
                        {
                            command.CommandText = @"                                        
                                        WITH UniqueOrderShipments (Shipments)
                                        AS
                                        (
                                            SELECT count(0) as ShipmentCount FROM shipment WITH (NOLOCK)
                                            INNER JOIN [Order] WITH (NOLOCK)
                                            ON [Order].OrderID = Shipment.orderID
                                            WHERE ShipmentID > @LastProcessedShipmentID	AND ShipmentID < @EndShipmentID AND Processed = 1
                                            GROUP BY [Order].ShipSenseHashKey
                                        )

                                        SELECT COUNT(0) FROM UniqueOrderShipments";

                            command.Parameters.Add(new SqlParameter("LastProcessedShipmentID", shippingSettings.ShipSenseProcessedShipmentID));
                            command.Parameters.Add(new SqlParameter("EndShipmentID", shippingSettings.ShipSenseEndShipmentID));

                            using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(command))
                            {
                                if (reader.Read())
                                {
                                    total = reader.GetInt32(0);
                                }
                            }
                        }
                    }

                    return total;
                }
                catch (Exception e)
                {
                    log.Error(e.Message, e);
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the total orders to analyze when populating the hash key of orders.
        /// </summary>
        public int TotalOrdersToAnalyze
        {
            get
            {
                try
                {
                    int total = 0;
                    using (SqlConnection conn = SqlSession.Current.OpenConnection())
                    {
                        using (SqlCommand command = SqlCommandProvider.Create(conn))
                        {
                            command.CommandText = @"                                        
                                        SELECT COUNT(0) 
                                        FROM [Order] WITH (NOLOCK)
                                        WHERE 
	                                        OnlineLastModified >= DATEADD(day, -30, GETUTCDATE()) 
	                                        AND	LEN(ShipSenseHashKey) = 0";

                            using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(command))
                            {
                                if (reader.Read())
                                {
                                    total = reader.GetInt32(0);
                                }
                            }
                        }
                    }

                    return total;
                }
                catch (Exception e)
                {
                    log.Error(e.Message, e);
                    throw;
                }
            }
        }

        /// <summary>
        /// Resets the hash key of all orders to an empty string.
        /// </summary>
        public void ResetOrderHashKeys()
        {
            try
            {
                using (AuditBehaviorScope scope = new AuditBehaviorScope(AuditState.Disabled))
                {
                    using (SqlConnection conn = SqlSession.Current.OpenConnection())
                    {
                        using (SqlCommand command = SqlCommandProvider.Create(conn))
                        {
                            command.CommandText = "UPDATE [Order] SET ShipSenseHashKey = ''";

                            SqlCommandProvider.ExecuteNonQuery(command);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
                throw;
            }
        }

        /// <summary>
        /// Updates the shipment range of the shipping settings that will be used when rebuilding 
        /// the ShipSense knowledge base.
        /// </summary>
        public void ResetShippingSettingsForLoading()
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            settings.ShipSenseProcessedShipmentID = GetStartingShipmentID();
            settings.ShipSenseEndShipmentID = GetEndingShipmentID();

            ShippingSettings.Save(settings);
        }


        /// <summary>
        /// Gets the shipment ID to start from when rebuilding the ShipSense knowledge base.
        /// </summary>
        private static long GetStartingShipmentID()
        {
            long startingShipmentID = 0;

            using (SqlConnection connection = SqlSession.Current.OpenConnection())
            {
                using (SqlCommand command = SqlCommandProvider.Create(connection))
                {
                    command.CommandText = @"
                                        DECLARE @ShipSenseProcessedShipmentID BIGINT
                                        WITH Shipments AS                                        (	                                        SELECT TOP 25000 ShipmentID FROM Shipment WITH (NOLOCK) WHERE Processed = 1 ORDER BY ShipmentID DESC                                        )                                        SELECT MIN(ShipmentID) FROM Shipments";

                    using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(command))
                    {
                        if (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                startingShipmentID = reader.GetInt64(0);
                            }
                        }
                    }
                }
            }

            return startingShipmentID;
        }

        /// <summary>
        /// Gets the maximum shipment ID of all the processed shipments.
        /// </summary>
        private static long GetEndingShipmentID()
        {
            long endingShipmentID = 0;

            using (SqlConnection connection = SqlSession.Current.OpenConnection())
            {
                using (SqlCommand command = SqlCommandProvider.Create(connection))
                {
                    command.CommandText = @"SELECT MAX(ShipmentID) FROM SHIPMENT WITH (NOLOCK) WHERE Processed = 1";

                    using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(command))
                    {
                        if (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                endingShipmentID = reader.GetInt64(0);
                            }
                        }
                    }
                }
            }

            return endingShipmentID;
        }

        /// <summary>
        /// Gets the next shipment to process based on ShippingSettings
        /// </summary>
        public ShipmentEntity FetchNextShipmentToAnalyze()
        {
            ShipmentEntity shipment = null;
            ShippingSettingsEntity shippingSettings = ShippingSettings.Fetch();

            long endShipmentID = shippingSettings.ShipSenseEndShipmentID;
            long lastProcessedShipmentID = shippingSettings.ShipSenseProcessedShipmentID;

            /*         
            SELECT MAX(s.shipmentID)
            FROM 
                Shipment s WITH(NOLOCK)
            INNER JOIN 
                [Order] o WITH(NOLOCK)
            ON s.OrderID = o.OrderID
            WHERE s.shipmentID <= @endShipmentID 
              AND s.ShipmentID > @processedShipmentID 
              AND s.Processed = 1
            GROUP BY o.ShipSenseHashKey  
            ORDER BY MAX(s.shipmentID) ASC       
             */
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

            IExpression shipmentPart = new DbFunctionCall("MAX({0})", new object[] { ShipmentFields.ShipmentID });
            IEntityField2 shipmentPartField = ShipmentFields.ShipmentID.SetExpression(shipmentPart);

            ISortClause shipmentPartSortClause = new SortClause(shipmentPartField, null, SortOperator.Ascending);
            SortExpression sort = new SortExpression(shipmentPartSortClause);

            using (SqlAdapter sqlAdapter = new SqlAdapter())
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
        /// and logged and wrapped in a ShipSenseException.
        /// </summary>
        public void Save(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            try
            {
                using (new AuditBehaviorScope(AuditState.Disabled))
                {
                    ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);
                    IEnumerable<IPackageAdapter> packageAdapters = shipmentType.GetPackageAdapters(shipment);

                    // Make sure we have all of the order information
                    OrderEntity order = (OrderEntity)DataProvider.GetEntity(shipment.OrderID);
                    OrderUtility.PopulateOrderDetails(order);

                    // Apply the data from the package adapters and the customs items to the knowledge base 
                    // entry, so the shipment data will get saved to the knowledge base; the knowledge base
                    // is smart enough to know when to save the customs items associated with an entry.
                    KnowledgebaseEntry entry = new KnowledgebaseEntry();
                    entry.ApplyFrom(packageAdapters, shipment.CustomsItems);

                    knowledgebase.Save(entry, order);

                    ShippingSettingsEntity shippingSettings = ShippingSettings.Fetch();
                    shippingSettings.ShipSenseProcessedShipmentID = shipment.ShipmentID;
                    ShippingSettings.Save(shippingSettings);
                }
            }
            catch (Exception ex)
            {
                // We may want to eat this exception entirely, so the user isn't impacted 
                log.ErrorFormat("An error occurred writing shipment ID {0} to the knowledge base: {1}", shipment.ShipmentID, ex.Message);
                throw new ShipSenseException("Shipment ID {0} was not logged to the knowledge base during data population.", ex);
            }
        }

        /// <summary>        
        /// Gets an OrderEntity that doesn't have a ShipSenseHashKey. 
        /// </summary>
        public OrderEntity FetchNextOrderToAnalyze()
        {
            OrderEntity orderEntity = null;

            RelationPredicateBucket orderBucket = new RelationPredicateBucket();

            // Grab all orders from the last 30 days that don't have a ShipSenseHashKey set
            orderBucket.PredicateExpression.Add(OrderFields.OrderID > previousProcessedOrder.OrderID);
            orderBucket.PredicateExpression.Add(OrderFields.ShipSenseHashKey == string.Empty);
            orderBucket.PredicateExpression.Add(OrderFields.OnlineLastModified >= DateTime.UtcNow.Subtract(TimeSpan.FromDays(30)));

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

            previousProcessedOrder = orderEntity;

            return orderEntity;
        }

        /// <summary>
        /// Saves an OrderEntity
        /// </summary>
        public void SaveOrder(OrderEntity order)
        {
            using (new AuditBehaviorScope(AuditState.Disabled))
            {
                using (SqlAdapter sqlAdapter = new SqlAdapter(true))
                {
                    sqlAdapter.SaveEntity(order, false, false);
                    sqlAdapter.Commit();
                }
            }
        }

        /// <summary>
        /// Gets a SQL app lock for loading ShipSense data
        /// </summary>
        public bool GetAppLock(string appLockName)
        {
            OpenConnection();
            return SqlAppLockUtility.AcquireLock(connection, appLockName);
        }

        /// <summary>
        /// Releases the ShipSense loading sql app lock if it is currently locked.
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
