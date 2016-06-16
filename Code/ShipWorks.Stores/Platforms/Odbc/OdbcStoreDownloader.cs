using Interapptive.Shared.Business;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Odbc.Loaders;
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

                int orderCount = GetOrderCount(orderGroups);

                if (orderCount > 0)
                {
                    EnsureRecordIdentifiersAreNotNull(orderGroups);

                    LoadOrders(orderGroups, orderCount);
                }
            }
            catch (ShipWorksOdbcException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Ensures the orders contain record identifier.
        /// </summary>
        /// <param name="orderGroups">The order groups.</param>
        /// <exception cref="DownloadException">$At least one order is missing a value in {fieldMap.RecordIdentifierSource}</exception>
        private void EnsureRecordIdentifiersAreNotNull(List<IGrouping<string, OdbcRecord>> orderGroups)
        {
            if (orderGroups.Any(groups => string.IsNullOrWhiteSpace(groups.Key)))
            {
                throw new DownloadException(
                    $"At least one order is missing a value in {fieldMap.RecordIdentifierSource}");
            }
        }

        /// <summary>
        /// Gets the order count.
        /// </summary>
        private int GetOrderCount(List<IGrouping<string, OdbcRecord>> orderGroups)
        {
            int orderCount = orderGroups.Count;

            Progress.Detail = orderCount == 0 ? "No orders to download." : $"{orderCount} orders found.";

            return orderCount;
        }

        /// <summary>
        /// Loads the order information into order entities
        /// </summary>
        private void LoadOrders(List<IGrouping<string, OdbcRecord>> orderGroups, int totalCount)
        {
            foreach (IGrouping<string, OdbcRecord> odbcRecordsForOrder in orderGroups)
            {
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                Progress.Detail = $"Processing order {QuantitySaved + 1}";

                OrderEntity downloadedOrder = LoadOrder(odbcRecordsForOrder);

                ResetAddressIfRequired(downloadedOrder, "Ship", OriginalShippingAddress);
                ResetAddressIfRequired(downloadedOrder, "Bill", OriginalBillingAddress);

                try
                {
                    SaveDownloadedOrder(downloadedOrder);
                }
                catch (ORMQueryExecutionException ex)
                {
                    
                    throw new DownloadException(ex.Message, ex);
                }


                Progress.PercentComplete = 100 * QuantitySaved / totalCount;
            }
        }

        /// <summary>
        /// Resets the address if required.
        /// </summary>
        /// <param name="order">The order that now has the downloaded address.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="addressBeforeDownload">The address before download.</param>
        /// <remarks>
        /// If address changed and the new address matches the address pre-address validation (the AV original address)
        /// from address validation, reset the address back to the original address.
        /// </remarks>
        private static void ResetAddressIfRequired(OrderEntity order, string prefix, AddressAdapter addressBeforeDownload)
        {
            AddressAdapter orderAddress = new AddressAdapter(order, prefix);
            if (addressBeforeDownload == orderAddress)
            {
                // Address hasn't changed
                return;
            }

            ValidatedAddressEntity addressBeforeValidation =
                ValidatedAddressManager.GetOriginalAddress(SqlAdapter.Default, order.OrderID, prefix);

            if (addressBeforeValidation != null)
            {
                AddressAdapter originalAddressAdapter = new AddressAdapter(addressBeforeValidation, string.Empty);

                if (originalAddressAdapter == orderAddress)
                {
                    AddressAdapter.Copy(addressBeforeDownload, orderAddress);
                }
            }
        }

        /// <summary>
        /// Downloads the order.
        /// </summary>
        /// <exception cref="DownloadException">Order number not found in map.</exception>
        private OrderEntity LoadOrder(IGrouping<string, OdbcRecord> odbcRecordsForOrder)
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
