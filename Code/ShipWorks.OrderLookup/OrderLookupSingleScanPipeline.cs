using System;
using System.Data;
using System.Data.Common;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Filters.Search;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Settings;
using ShipWorks.Stores.Communication;
using ShipWorks.Users;

namespace ShipWorks.OrderLookup
{
    public class OrderLookupSingleScanPipeline : IInitializeForCurrentUISession
    {
        private readonly IMessenger messenger;
        private readonly IOnDemandDownloaderFactory onDemandDownloaderFactory;
        private readonly SearchDefinitionProviderFactory searchDefinitionProviderFactory;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IMainForm mainForm;
        private readonly ICurrentUserSettings userSettings;
        private readonly ISqlSession sqlSession;

        private IDisposable subscription;

        public OrderLookupSingleScanPipeline(IMessenger messenger,
            IOnDemandDownloaderFactory onDemandDownloaderFactory,
            SearchDefinitionProviderFactory searchDefinitionProviderFactory,
            ISqlAdapterFactory sqlAdapterFactory,
            IMainForm mainForm,
            ICurrentUserSettings userSettings, 
            ISqlSession sqlSession)
        {
            this.messenger = messenger;
            this.onDemandDownloaderFactory = onDemandDownloaderFactory;
            this.searchDefinitionProviderFactory = searchDefinitionProviderFactory;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.mainForm = mainForm;
            this.userSettings = userSettings;
            this.sqlSession = sqlSession;
        }

        public void InitializeForCurrentSession()
        {
            EndSession();

            subscription = messenger.OfType<SingleScanMessage>()
                .Where(x => !mainForm.AdditionalFormsOpen() && userSettings.GetUIMode() == UIMode.OrderLookup)
                .Subscribe(async scanMsg => await FindOrder(scanMsg.ScannedText));
        }

        /// <summary>
        /// Finds an order
        /// </summary>
        private async Task<object> FindOrder(string scanMsgScannedText)
        {
            // Download order from store database when applicable
            await DownloadOrderOnDemand(scanMsgScannedText);

            // Get the orderID of the scanned order
            long? orderId = await FetchOrderId(scanMsgScannedText);

            // Download the order
            OrderEntity order = FetchOrder(orderId);

            SendOrderMessage(order);

            return order;
        }

        /// <summary>
        /// SendOrderMessage
        /// </summary>
        /// <param name="order"></param>
        private void SendOrderMessage(OrderEntity order)
        {
            if (!(order?.IsNew ?? false))
            {
                messenger.Send(new OrderFoundMessage(this, order));
            }
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

            return order;
        }

        /// <summary>
        /// Find cooresponding order
        /// </summary>
        private async Task<long?> FetchOrderId(string scannedText)
        {
            string sql = GenerateSql(scannedText);
            if (string.IsNullOrEmpty(sql))
            {
                return null;
            }

            long? orderId = null;
            using (DbConnection sqlConnection = sqlSession.OpenConnection())
            {
                using (DbCommand cmd = sqlConnection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;

                    using (DbDataReader dbDataReader = await cmd.ExecuteReaderAsync())
                    {
                        while (await dbDataReader.ReadAsync())
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
            ISearchDefinitionProvider searchDefinitionProvider= searchDefinitionProviderFactory.Create(FilterTarget.Orders, true);

            var def = searchDefinitionProvider.GetDefinition(scanMsgScannedText);

            string whereClause = def.RootContainer.GenerateSql(new SqlGenerationContext(FilterTarget.Orders));
            return $"Select OrderId from [Order] o where {whereClause}";
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
        /// End the session
        /// </summary>
        public void Dispose() => EndSession();

        /// <summary>
        /// End the session
        /// </summary>
        public void EndSession() => subscription?.Dispose();
    }
}
