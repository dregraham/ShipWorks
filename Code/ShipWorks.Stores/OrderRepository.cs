using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Administration.Recovery;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Search;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Orders;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Repository for saving order related content
    /// </summary>
    public class OrderRepository : IOrderRepository
    {
        private readonly ISqlSession sqlSession;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly ISqlAdapterRetryFactory sqlAdapterRetryFactory;
        private readonly IOnDemandDownloaderFactory onDemandDownloaderFactory;
        private readonly ISingleScanSearchDefinitionProvider singleScanSearchDefinitionProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderRepository"/> class.
        /// </summary>
        public OrderRepository(ISqlSession sqlSession,
                               ISqlAdapterFactory sqlAdapterFactory, 
                               ISqlAdapterRetryFactory sqlAdapterRetryFactory,
                               IOnDemandDownloaderFactory onDemandDownloaderFactory,
                               ISingleScanSearchDefinitionProvider singleScanSearchDefinitionProvider)
        {
            this.sqlSession = sqlSession;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.sqlAdapterRetryFactory = sqlAdapterRetryFactory;
            this.onDemandDownloaderFactory = onDemandDownloaderFactory;
            this.singleScanSearchDefinitionProvider = singleScanSearchDefinitionProvider;
        }

        /// <summary>
        /// Determines whether the specified order has matching note in database.
        /// </summary>
        public bool ContainsNote(OrderEntity order, string noteText, NoteSource source)
        {
            IRelationPredicateBucket relationPredicateBucket = order.GetRelationInfoNotes();
            relationPredicateBucket.PredicateExpression.AddWithAnd(new FieldCompareValuePredicate(NoteFields.Text,
                null, ComparisonOperator.Equal, noteText));
            relationPredicateBucket.PredicateExpression.AddWithAnd(new FieldCompareValuePredicate(
                NoteFields.Source, null, ComparisonOperator.Equal, (int) source));

            using (EntityCollection<NoteEntity> notes = new EntityCollection<NoteEntity>())
            {
                int matchingNotes = SqlAdapter.Default.GetDbCount(notes, relationPredicateBucket);

                if (matchingNotes > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Populates the order, order items, order charges, and order item attribute for the given order.
        /// </summary>
        /// <param name="order">The order.</param>
        public void PopulateOrderDetails(OrderEntity order) => OrderUtility.PopulateOrderDetails(order);

        /// <summary>
        /// Saves an order to the database
        /// </summary>
        public void Save(OrderEntity order)
        {
            ISqlAdapterRetry sqlAdapterRetry = sqlAdapterRetryFactory.Create<SqlException>(5, 0,
                "OrderRepository.PopulateOrderDetails");
            sqlAdapterRetry.ExecuteWithRetry(() => InternalSave(order));
        }

        /// <summary>
        /// Actual save command
        /// </summary>
        private void InternalSave(OrderEntity order)
        {
            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                sqlAdapter.SaveAndRefetch(order);
                sqlAdapter.Commit();
            }
        }
        
        /// <summary>
        /// Given scanned text, find the associated order
        /// </summary>
        public async Task<OrderEntity> FindOrder(string scanMsgScannedText)
        {
            await DownloadOrderOnDemand(scanMsgScannedText).ConfigureAwait(false);

            return await FetchOrder(scanMsgScannedText).ConfigureAwait(false);
        }

        /// <summary>
        /// Downloads order from customer's database
        /// </summary>
        private Task DownloadOrderOnDemand(string scanMsgScannedText)
        {
            // Downloads the order if needed
            IOnDemandDownloader singleScanOnDemandDownloader = onDemandDownloaderFactory.CreateSingleScanOnDemandDownloader();
            return singleScanOnDemandDownloader.Download(scanMsgScannedText);
        }

        /// <summary>
        /// Find corresponding order
        /// </summary>
        private async Task<OrderEntity> FetchOrder(string scannedText)
        {
            using (DbConnection conn = sqlSession.OpenConnection())
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = singleScanSearchDefinitionProvider.GenerateSql(scannedText);

                    long? orderId = (long?) cmd.ExecuteScalar();

                    using (ISqlAdapter adapter = sqlAdapterFactory.Create(conn))
                    {
                        EntityQuery<OrderEntity> orderQuery = new QueryFactory().Order.Where(OrderFields.OrderID == orderId);
                        return await adapter.FetchFirstAsync(orderQuery);
                    }
                }
            }
        }
    }
}
