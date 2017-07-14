using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using SD.LLBLGen.Pro.QuerySpec.Adapter;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Get download starting points
    /// </summary>
    [Component]
    public class DownloadStartingPoint : IDownloadStartingPoint
    {
        readonly ISqlAdapterFactory sqlAdapterFactory;
        readonly IDateTimeProvider dateTimeProvider;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public DownloadStartingPoint(ISqlAdapterFactory sqlAdapterFactory, IDateTimeProvider dateTimeProvider, Func<Type, ILog> createLogger)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.sqlAdapterFactory = sqlAdapterFactory;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Obtains the most recent order date.  If there is none, and the store has an InitialDaysBack policy, it
        /// will be used to calculate the initial number of days back to.
        /// </summary>
        public Task<DateTime?> OnlineLastModified(IStoreEntity store) =>
            GetDateStartingPoint(OrderFields.OnlineLastModified, store);

        /// <summary>
        /// Obtains the most recent order date.  If there is none, and the store has an InitialDaysBack policy, it
        /// will be used to calculate the initial number of days back to.
        /// </summary>
        public Task<DateTime?> OrderDate(IStoreEntity store) =>
            GetDateStartingPoint(OrderFields.OrderDate, store);

        /// <summary>
        /// Gets the largest OrderNumber we have in our database for non-manual orders for this store.  If no
        /// such orders exist, then if there is an InitialDownloadPolicy it is applied.  Otherwise, 0 is returned.
        /// </summary>
        public async Task<long> OrderNumber(IStoreEntity store)
        {
            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                long? orderNumber = await GetMaxOrderNumberFromDatabase(adapter, store.StoreID);

                if (!orderNumber.HasValue && store.InitialDownloadOrder != null)
                {
                    // We have to subtract one b\c the downloader expects the starting point to be the max order number in the db.  So what
                    // it does is download all orders AFTER it.  But for the initial download policy, we want to START with it.  So we have
                    // to back off by one to include it.
                    orderNumber = Math.Max(0, store.InitialDownloadOrder.Value - 1);
                    log.InfoFormat("Max(OrderNumber) - applying initial download policy.");
                }

                log.InfoFormat("MAX(OrderNumber) = {0}", orderNumber);

                return orderNumber.GetValueOrDefault(0);
            }
        }

        /// <summary>
        /// Get the maximum order number from the database
        /// </summary>
        private async Task<long?> GetMaxOrderNumberFromDatabase(ISqlAdapter adapter, long storeID)
        {
            QueryFactory factory = new QueryFactory();

            DynamicQuery maxOrderQuery = factory.Order
                .Select(OrderFields.OrderNumber.Max())
                .Where(OrderFields.StoreID == storeID)
                .AndWhere(OrderFields.IsManual == false);

            DynamicQuery maxOrderSearchQuery = factory.OrderSearch
                .Select(OrderSearchFields.OrderNumber.Max())
                .Where(OrderSearchFields.StoreID == storeID)
                .AndWhere(OrderSearchFields.IsManual == false);

            long? maxOrder = await adapter.FetchScalarAsync<long?>(maxOrderQuery);
            long? maxOrderSearch = await adapter.FetchScalarAsync<long?>(maxOrderSearchQuery);

            if (maxOrder.HasValue && maxOrderSearch.HasValue)
            {
                return Math.Max(maxOrder.Value, maxOrderSearch.Value);
            }

            return maxOrder.HasValue ? maxOrder : maxOrderSearch;
        }

        /// <summary>
        /// Get the date starting point
        /// </summary>
        private async Task<DateTime?> GetDateStartingPoint(EntityField2 dateField, IStoreEntity store)
        {
            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                QueryFactory factory = new QueryFactory();

                DynamicQuery maxOrderQuery = factory.Order
                    .Select(dateField.Max())
                    .Where(OrderFields.StoreID == store.StoreID)
                    .AndWhere(OrderFields.IsManual == false);

                DateTime? dateTime = await adapter.FetchScalarAsync<DateTime?>(maxOrderQuery);

                log.Info($"MAX({dateField.Name}) = {dateTime:u}");

                // If we don't have a max, but do have a days-back policy, use the days back
                if (dateTime == null && store.InitialDownloadDays != null)
                {
                    // Also add on 2 hours just to make sure we are in range
                    dateTime = dateTimeProvider.UtcNow.AddDays(-store.InitialDownloadDays.Value).AddHours(2);
                    log.Info($"MAX({dateField.Name}) adjusted by download policy = {dateTime:u}");
                }

                // Dates pulled from the database are always UTC
                if (dateTime != null && dateTime.Value.Kind == DateTimeKind.Unspecified)
                {
                    dateTime = DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc);
                }

                return dateTime;
            }
        }
    }
}
