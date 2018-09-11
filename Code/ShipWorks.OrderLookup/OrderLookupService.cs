using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Filters.Search;
using ShipWorks.Stores.Communication;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Service used by OrderLookupSingleScanPipeline to assist in finding an order
    /// </summary>
    [Component]
    public class OrderLookupService : IOrderLookupService
    {
        private readonly IOnDemandDownloaderFactory onDemandDownloaderFactory;
        private readonly ISearchDefinitionProviderFactory searchDefinitionProviderFactory;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly ISqlSession sqlSession;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupService(IOnDemandDownloaderFactory onDemandDownloaderFactory,
            ISearchDefinitionProviderFactory searchDefinitionProviderFactory,
            ISqlAdapterFactory sqlAdapterFactory,
            ISqlSession sqlSession)
        {
            this.onDemandDownloaderFactory = onDemandDownloaderFactory;
            this.searchDefinitionProviderFactory = searchDefinitionProviderFactory;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.sqlSession = sqlSession;
        }

        /// <summary>
        /// Given scanned text, find the associated order
        /// </summary>
        public async Task<OrderEntity> FindOrder(string scanMsgScannedText)
        {
            // Download order from store database when applicable
            await DownloadOrderOnDemand(scanMsgScannedText);

            // Get the orderID of the scanned order
            long? orderId = FetchOrderId(scanMsgScannedText);

            // Download the order
            OrderEntity order = FetchOrder(orderId);

            return order;
        }
        
        /// <summary>
        /// Downloads order from customer's database
        /// </summary>
        private async Task DownloadOrderOnDemand(string scanMsgScannedText)
        {
            // Downloads the order if needed
            IOnDemandDownloader singleScanOnDemandDownloader = onDemandDownloaderFactory.CreateSingleScanOnDemandDownloader();
            await singleScanOnDemandDownloader.Download(scanMsgScannedText);
        }

        /// <summary>
        /// Find corresponding order
        /// </summary>
        private long? FetchOrderId(string scannedText)
        {
            string sql = GenerateSql(scannedText);
            if (string.IsNullOrEmpty(sql))
            {
                return null;
            }

            long? orderId = null;
            using (IDbConnection sqlConnection = sqlSession.OpenConnection())
            {
                using (IDbCommand cmd = sqlConnection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;

                    using (IDataReader dbDataReader = cmd.ExecuteReader())
                    {
                        while (dbDataReader.Read())
                        {
                            orderId = dbDataReader.GetInt64(0);
                            break;
                        }
                    }
                }
            }

            return orderId;
        }

        /// <summary>
        /// Generate sql to fetch order
        /// </summary>
        private string GenerateSql(string scanMsgScannedText)
        {
            ISearchDefinitionProvider searchDefinitionProvider = searchDefinitionProviderFactory.Create(FilterTarget.Orders, true);

            IFilterDefinition filterDefinition = searchDefinitionProvider.GetDefinition(scanMsgScannedText);

            string whereClause = filterDefinition.GenerateRootSql(FilterTarget.Orders);
            return $"Select OrderId from [Order] o where {whereClause}";
        }

        /// <summary>
        /// Get order based on order id, return null if not found or orderId is null
        /// </summary>
        private OrderEntity FetchOrder(long? orderId)
        {
            OrderEntity order = null;
            if (orderId.HasValue)
            {
                order = new OrderEntity(orderId.Value);
                using (var sqlAdapter = sqlAdapterFactory.Create())
                {
                    sqlAdapter.FetchEntity(order);
                }
            }

            // If the order is null or new, return null as an order was not found
            return order?.IsNew ?? true ? null : order;
        }
    }
}