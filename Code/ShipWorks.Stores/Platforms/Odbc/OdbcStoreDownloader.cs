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
            IOdbcCommand downloadCommand = commandFactory.CreateDownloadCommand(store);

            IEnumerable<IGrouping<string, OdbcRecord>> odbcOrders = downloadCommand.Execute().GroupBy(o => o.RecordIdentifier);

            if (odbcOrders.Any(groups=>string.IsNullOrWhiteSpace(groups.Key)))
            {
                throw new DownloadException(
                    $"At least one order is missing a value in {fieldMap.RecordIdentifierSource}");
            }

            foreach (IGrouping<string, OdbcRecord> odbcRecordsForOrder in odbcOrders)
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
                OrderEntity orderEntity = InstantiateOrder(new OrderNumberIdentifier((long) odbcFieldMapEntry.ShipWorksField.Value));
               
                orderLoader.Load(fieldMap, orderEntity, odbcRecordsForOrder);

                SaveDownloadedOrder(orderEntity);
            }
        }
    }
}
