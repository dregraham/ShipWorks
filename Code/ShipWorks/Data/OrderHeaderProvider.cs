using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Caching;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Data
{
    /// <summary>
    /// Responsible for loading, caching, and providing order headers
    /// </summary>
    public sealed class OrderHeaderProvider
    {
        long lastOrderID = 0;

        // 'Cache' of orderID's to there header (StoreID, IsManual) info.  Once an OrderID goes in, its in to stay, since StoreID, IsManual can never change
        Dictionary<long, OrderHeader> headerCache = new Dictionary<long, OrderHeader>();

        // Order header loading sync stuff
        ManualResetEvent headersReadyEvent = new ManualResetEvent(true);

        // Can be used to grab header info from cached entity if we don't know about the header yet
        EntityCache entityCache;
        private readonly ExecutionMode executionMode;

        // non-null when we are busy fetching headers
        object asyncBusyLock = new object();
        ApplicationBusyToken busyToken;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderHeaderProvider(EntityCache entityCache)
            : this(entityCache, Program.ExecutionMode)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderHeaderProvider(EntityCache entityCache, ExecutionMode executionMode)
        {
            if (entityCache == null)
            {
                throw new ArgumentNullException("entityCache");
            }

            this.entityCache = entityCache;
            this.executionMode = executionMode;
        }

        /// <summary>
        /// Clear the cache entries
        /// </summary>
        public void Clear()
        {
            lock (headerCache)
            {
                headerCache.Clear();
                lastOrderID = 0;
            }
        }

        /// <summary>
        /// Get the order header corresponding to the given ID
        /// </summary>
        public OrderHeader GetHeader(long orderID)
        {
            OrderHeader header = GetHeaderFromCache(orderID);
            if (header != null)
            {
                return header;
            }

            // Next see if the order is in the entity cache, we can build a header from that
            OrderEntity order = (OrderEntity) entityCache.GetEntity(orderID, false);
            if (order != null)
            {
                header = new OrderHeader(orderID, order.StoreID, order.IsManual);

                lock (headerCache)
                {
                    headerCache[orderID] = header;
                }

                // We know we need some, so kickoff the load - but we don't need to wait for it, we already have what we came for
                InitiateHeaderLoading();

                return header;
            }

            // If we're on the UI, show the wait cursor, in case this is going to take a while
            if (executionMode.IsUIDisplayed && !Program.MainForm.InvokeRequired)
            {
                Cursor.Current = Cursors.WaitCursor;
            }

            // It may already be loading... if it is, wait
            headersReadyEvent.WaitOne();

            // Now that its done loading, try it again
            header = GetHeaderFromCache(orderID);
            if (header != null)
            {
                return header;
            }

            // Even if it was loading before when we waited, it could be that this OrderID came in so recently in a race condition such that
            // it wasn't picked up by the previous load attempt.  Kick off another load attempt that will get the very latest, and we know
            // if it exists we'd get it this time.
            InitiateHeaderLoading();

            // Wait for the loading to be done
            headersReadyEvent.WaitOne();

            // May still return null if the orderID has been deleted
            return GetHeaderFromCache(orderID);
        }

        /// <summary>
        /// Returns the header that is currently cached if any, or null if its not
        /// </summary>
        private OrderHeader GetHeaderFromCache(long orderID)
        {
            lock (headerCache)
            {
                OrderHeader header;
                return headerCache.TryGetValue(orderID, out header) ? header : null;
            }
        }

        /// <summary>
        /// Initiate the process of loading the latest headers from the database
        /// </summary>
        public void InitiateHeaderLoading()
        {
            lock (asyncBusyLock)
            {
                // If this is non-null, we are already working on it
                if (busyToken != null)
                {
                    return;
                }

                // Kick off the async loading.  If we are in a context sensitive scope, we have to wait until next time.  If we are on the UI, we'll always get it.
                // We only may not if we are running in the background.
                if (!ApplicationBusyManager.TryOperationStarting("loading data", out busyToken))
                {
                    return;
                }

                headersReadyEvent.Reset();

                ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(AsyncLoadHeaders));
            }
        }

        /// <summary>
        /// Runs on a background thread for async loading of headers
        /// </summary>
        private void AsyncLoadHeaders(object state)
        {
            List<OrderHeader> newHeaders = new List<OrderHeader>();

            ResultsetFields resultFields = new ResultsetFields(3);
            resultFields.DefineField(OrderFields.OrderID, 0, "OrderID", "");
            resultFields.DefineField(OrderFields.StoreID, 1, "StoreID", "");
            resultFields.DefineField(OrderFields.IsManual, 2, "IsManual", "");

            RelationPredicateBucket bucket = null;

            if (lastOrderID > 0)
            {
                bucket = new RelationPredicateBucket(OrderFields.OrderID > lastOrderID);
            }

            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                using (IDataReader reader = adapter.FetchDataReader(resultFields, bucket, CommandBehavior.Default, 0, true))
                {
                    while (reader.Read())
                    {
                        newHeaders.Add(new OrderHeader(reader.GetInt64(0), reader.GetInt64(1), reader.GetBoolean(2)));
                    }
                }
            }

            lock (headerCache)
            {
                foreach (OrderHeader header in newHeaders)
                {
                    headerCache[header.OrderID] = header;

                    lastOrderID = Math.Max(header.OrderID, lastOrderID);
                }
            }

            lock (asyncBusyLock)
            {
                busyToken.Dispose();
                busyToken = null;

                headersReadyEvent.Set();
            }
        }
    }
}
