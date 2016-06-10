using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Downloader for an OdbcStoreDownloader
    /// </summary>
    public class OdbcStoreDownloader : StoreDownloader
    {
        private readonly OdbcCommandFactory commandFactory;
        private readonly IOdbcFieldMap fieldMap;
        private readonly IOdbcOrderLoader orderLoader;
        private readonly OdbcStoreEntity store;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcStoreDownloader"/> class.
        /// </summary>
        public OdbcStoreDownloader(StoreEntity store,
            OdbcCommandFactory commandFactory,
            IOdbcFieldMap fieldMap,
            IOdbcOrderLoader orderLoader) : base(store)
        {
            this.commandFactory = commandFactory;
            this.fieldMap = fieldMap;
            this.orderLoader = orderLoader;
            this.store = (OdbcStoreEntity) store;

            fieldMap.Load(this.store.Map);
        }

        /// <summary>
        /// Import ODBC Orders from external datasource.
        /// </summary>
        protected override void Download()
        {
            Progress.Detail = "Querying data source...";
            try
            {

                IOdbcCommand downloadCommand = commandFactory.CreateDownloadCommand(store);

                IEnumerable<OdbcRecord> downloadedOrders = downloadCommand.Execute();
                List<IGrouping<string, OdbcRecord>> orderGroups =
                    downloadedOrders.GroupBy(o => o.RecordIdentifier).ToList();

                int totalCount = orderGroups.Count;

                if (totalCount == 0)
                {
                    Progress.Detail = "No orders to download.";
                    return;
                }

                Progress.Detail = $"{totalCount} orders found.";

                if (orderGroups.Any(groups => string.IsNullOrWhiteSpace(groups.Key)))
                {
                    throw new DownloadException(
                        $"At least one order is missing a value in {fieldMap.RecordIdentifierSource}");
                }

                foreach (IGrouping<string, OdbcRecord> odbcRecordsForOrder in orderGroups)
                {
                    if (Progress.IsCancelRequested)
                    {
                        return;
                    }

                    Progress.Detail = $"Processing order {QuantitySaved + 1}";
                    SaveDownloadedOrder(DownloadOrder(odbcRecordsForOrder));
                    Progress.PercentComplete = 100*QuantitySaved/totalCount;
                }
            }
            catch (ShipWorksOdbcException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Downloads the order.
        /// </summary>
        /// <exception cref="DownloadException">Order number not found in map.</exception>
        private OrderEntity DownloadOrder(IGrouping<string, OdbcRecord> odbcRecordsForOrder)
        {
            OdbcRecord firstRecord = odbcRecordsForOrder.First();

            fieldMap.ApplyValues(firstRecord);

            // Find the OrderNumber Entry
            IOdbcFieldMapEntry odbcFieldMapEntry = fieldMap.FindEntriesBy(OrderFields.OrderNumber).FirstOrDefault();

            if (odbcFieldMapEntry == null)
            {
                throw new DownloadException("Order number not found in map.");
            }

            // Create an order using the order number
            OrderEntity orderEntity = InstantiateOrder(new OrderNumberIdentifier((long)odbcFieldMapEntry.ShipWorksField.Value));

            orderLoader.Load(fieldMap, orderEntity, odbcRecordsForOrder);
            return orderEntity;
        }
    }
}
