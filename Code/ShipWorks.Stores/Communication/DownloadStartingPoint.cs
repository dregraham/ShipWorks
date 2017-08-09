using System;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;
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
        public DateTime? OnlineLastModified(IStoreEntity store) =>
            GetStartingPoint(OrderFields.OnlineLastModified, store);

        /// <summary>
        /// Obtains the most recent order date.  If there is none, and the store has an InitialDaysBack policy, it
        /// will be used to calculate the initial number of days back to.
        /// </summary>
        public DateTime? OrderDate(IStoreEntity store) =>
            GetStartingPoint(OrderFields.OrderDate, store);

        private DateTime? GetStartingPoint(EntityField2 dateField, IStoreEntity store)
        {
            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                object result = adapter.GetScalar(
                    dateField,
                    null, AggregateFunction.Max,
                    OrderFields.StoreID == store.StoreID & OrderFields.IsManual == false);

                DateTime? dateTime = result as DateTime?;

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
