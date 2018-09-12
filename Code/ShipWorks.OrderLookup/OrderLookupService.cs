using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
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
        
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupService(IOnDemandDownloaderFactory onDemandDownloaderFactory,
            ISearchDefinitionProviderFactory searchDefinitionProviderFactory,
            ISqlAdapterFactory sqlAdapterFactory)
        {
            this.onDemandDownloaderFactory = onDemandDownloaderFactory;
            this.searchDefinitionProviderFactory = searchDefinitionProviderFactory;
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Given scanned text, find the associated order
        /// </summary>
        public async Task<OrderEntity> FindOrder(string scanMsgScannedText)
        {
            await DownloadOrderOnDemand(scanMsgScannedText).ConfigureAwait(false);

            return (await FetchOrder(scanMsgScannedText).ConfigureAwait(false)).FirstOrDefault();
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
        private Task<List<OrderEntity>> FetchOrder(string scannedText)
        {
            string sql = GenerateSql(scannedText);

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                return sqlAdapter.FetchQueryAsync<OrderEntity>(sql);
            }
        }

        /// <summary>
        /// Generate sql to fetch order
        /// </summary>
        private string GenerateSql(string scanMsgScannedText)
        {
            ISearchDefinitionProvider searchDefinitionProvider =
                searchDefinitionProviderFactory.Create(FilterTarget.Orders, true);

            IFilterDefinition filterDefinition = searchDefinitionProvider.GetDefinition(scanMsgScannedText);

            string whereClause = filterDefinition.GenerateRootSql(FilterTarget.Orders);
            return $"Select * from [Order] o where {whereClause}";
        }
    }
}