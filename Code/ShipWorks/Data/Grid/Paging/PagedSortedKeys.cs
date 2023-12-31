﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Administration.Recovery;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Utility;
using static Interapptive.Shared.Utility.Functional;
using static ShipWorks.Data.Administration.Recovery.SqlAdapterRetry;

namespace ShipWorks.Data.Grid.Paging
{
    /// <summary>
    /// Provides an IEnumerable facade over an asynchronous fetch pattern to be able to return keys to the requester as
    /// soon as they are available from the database.
    /// </summary>
    public class PagedSortedKeys : IEnumerable<long>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PagedSortedKeys));
        private List<long> loadedKeys;
        private int loadedKeyCount;
        private bool loadingComplete;
        private bool canceled;

        // Don't allow too many to be fetching at once or we can end up just stalling out completely
        private static SemaphoreSlim semaphore = new SemaphoreSlim(2);

        // Special single insance of an empty key set
        private static readonly PagedSortedKeys emptyKeys = new PagedSortedKeys(new List<long>());

        /// <summary>
        /// Constructs a new instance that will load the keys based on the given key field, query, and sort
        /// </summary>
        public PagedSortedKeys(EntityField2 keyField, RelationPredicateBucket queryBucket, SortExpression sortExpression)
        {
            loadedKeys = new List<long>();
            loadedKeyCount = 0;
            loadingComplete = false;

            ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem((object state) => AsyncExecuteQuery(keyField, queryBucket, sortExpression), "loading data"));
        }

        /// <summary>
        /// Constructs a new instance that will load keys as related to the source keys based on the given relation type, using the standard DataProvider
        /// </summary>
        public PagedSortedKeys(List<long> sourceKeys, EntityType relatedKeyType, SortDefinition sortDefinition)
        {
            // See if the keys exist already
            loadedKeys = DataProvider.GetRelatedKeys(sourceKeys, relatedKeyType, false, sortDefinition);

            // If we got them, go ahead and mark us complete
            if (loadedKeys != null)
            {
                loadedKeyCount = loadedKeys.Count;
                loadingComplete = true;
            }
            else
            {
                loadedKeys = new List<long>();
                loadedKeyCount = 0;
                loadingComplete = false;

                ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem((object state) => AsyncGetRelatedKeys(sourceKeys, relatedKeyType, sortDefinition)));
            }
        }

        /// <summary>
        /// Constructs a new instance based on the given known keyset.  No querying is done.
        /// </summary>
        public PagedSortedKeys(List<long> keys)
        {
            loadedKeys = keys.ToList();
            loadedKeyCount = keys.Count;
            loadingComplete = true;
        }

        /// <summary>
        /// Special single instance of the "empty key set"
        /// </summary>
        public static PagedSortedKeys Empty
        {
            get { return emptyKeys; }
        }

        /// <summary>
        /// Get the key from the given index.  If still loading, it waits for the data in the index to come available.  If the index ends up being out of range,
        /// then null is returned.
        /// </summary>
        public long? GetKeyFromIndex(int index)
        {
            return GetKeyFromIndex(index, null);
        }

        /// <summary>
        /// Get the key from the given index.  If still loading, it waits for the data in the index to come available.  If the index ends up being out of range,
        /// then null is returned.
        /// </summary>
        public long? GetKeyFromIndex(int index, TimeSpan? timeout)
        {
            if (index < 0)
            {
                return null;
            }

            Stopwatch timer = Stopwatch.StartNew();

            SpinWait.SpinUntil(() =>
                loadingComplete ||
                index < loadedKeyCount ||
                (timeout != null && timer.Elapsed > timeout));

            // If we've already loaded enough, go ahead and return it directly from our list
            if (index < loadedKeyCount)
            {
                return loadedKeys[index];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get the total count of all rows
        /// </summary>
        public PagedRowCount GetCount()
        {
            SpinWait.SpinUntil(() => !PagedRowCount.IsStillWaitingForComplete || loadingComplete);

            return new PagedRowCount(loadedKeyCount, loadingComplete);
        }

        /// <summary>
        /// IEnumerable implementation
        /// </summary>
        public IEnumerator<long> GetEnumerator()
        {
            if (loadingComplete)
            {
                return loadedKeys.GetEnumerator();
            }
            else
            {
                return new InternalEnumerable(this).GetEnumerator();
            }
        }

        /// <summary>
        /// Private class to facilitate 'delay' / waiting enumeration
        /// </summary>
        private class InternalEnumerable : IEnumerable<long>
        {
            private PagedSortedKeys owner;

            /// <summary>
            /// Constructor
            /// </summary>
            public InternalEnumerable(PagedSortedKeys owner)
            {
                this.owner = owner;
            }

            /// <summary>
            /// Returns the enumerator for the paged key set
            /// </summary>
            public IEnumerator<long> GetEnumerator()
            {
                int index = 0;
                long? key;

                while ((key = owner.GetKeyFromIndex(index++)) != null)
                {
                    yield return key.Value;
                }
            }

            /// <summary>
            /// Explicit generic version
            /// </summary>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        /// <summary>
        /// Explicit implementation for generic version
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Indicates if the load was canceled and was never completed
        /// </summary>
        public bool IsCanceled
        {
            get
            {
                return canceled;
            }
        }

        /// <summary>
        /// If the loading is still incomplete, cancel any further loading
        /// </summary>
        public void CancelLoadingIfIncomplete()
        {
            if (!loadingComplete)
            {
                canceled = true;
            }
        }

        /// <summary>
        /// Fetch the keys defined by the given field, query, and sort
        /// </summary>
        private void AsyncExecuteQuery(EntityField2 keyField, RelationPredicateBucket queryBucket, SortExpression sortExpression) =>
            Using(
                semaphore.DisposableWait(),
                _ => ExecuteWithRetry(
                        "PagedSortedKeys.AsyncExecuteQuery",
                        new SqlAdapterRetryOptions(log: log),
                        () => PerformQuery(keyField, queryBucket, sortExpression),
                        CanHandleQueryException));

        /// <summary>
        /// Perform the query
        /// </summary>
        private void PerformQuery(EntityField2 keyField, RelationPredicateBucket queryBucket, SortExpression sortExpression)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                if (!canceled)
                {
                    ResultsetFields resultFields = new ResultsetFields(1);
                    resultFields.DefineField(keyField, 0, "EntityID", "");

                    using (IDataReader reader = adapter.FetchDataReader(resultFields, queryBucket, CommandBehavior.Default, PagedEntityGrid.MaxVirtualRowCount, sortExpression, true))
                    {
                        while (!canceled && reader.Read())
                        {
                            loadedKeys.Add(reader.GetInt64(0));
                            loadedKeyCount = loadedKeys.Count;
                        }
                    }
                }

                loadingComplete = true;
            }
        }

        /// <summary>
        /// Can the query exception be handled
        /// </summary>
        private static bool CanHandleQueryException(Exception ex) =>
            ex.GetAllExceptions().OfType<SqlException>().Any() ||
            ex.GetAllExceptions().OfType<InvalidOperationException>()
                .Any(x => x.Message.IndexOf("Internal connection fatal error", StringComparison.OrdinalIgnoreCase) >= 0);

        /// <summary>
        /// Fetch the keys related to the given sourceKeys, based on the specified definition
        /// </summary>
        private void AsyncGetRelatedKeys(List<long> sourceKeys, EntityType relatedKeyType, SortDefinition sortDefinition)
        {
            semaphore.Wait();

            try
            {
                if (!canceled)
                {
                    var keys = DataProvider.GetRelatedKeys(sourceKeys, relatedKeyType, true, sortDefinition);

                    foreach (long key in keys)
                    {
                        loadedKeys.Add(key);
                    }

                    loadedKeyCount = loadedKeys.Count;
                }

                loadingComplete = true;
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}