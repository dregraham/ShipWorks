using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Utility;

namespace ShipWorks.Filters.Grid
{
    /// <summary>
    /// Specializes EntityGateway to incorporate filtering
    /// </summary>
    class FilterEntityGateway : PagedEntityGateway
    {
        static readonly ILog log = LogManager.GetLogger(typeof(FilterEntityGateway));

        // BN: Caching turned off due to FB113212.  Re-enable if we can make dirty detection more sophisticated.
        // static LruCache<string, FilterContentCacheEntry> filterContentCache = new LruCache<string, FilterContentCacheEntry>(20);

        // Active filter count
        FilterCount filterCount;

        class FilterContentCacheEntry
        {
            public PagedSortedKeys SortedKeys { get; set; }
            public long FilterCountVersion { get; set; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterEntityGateway(EntityType entityType, FilterCount filterCount)
            : base(entityType)
        {
            this.filterCount = filterCount;
        }

        /// <summary>
        /// Copy constructor used for cloning
        /// </summary>
        protected FilterEntityGateway(FilterEntityGateway copy)
            : base(copy)
        {
            this.filterCount = copy.filterCount;
        }

        /// <summary>
        /// Create a copy of the gateway
        /// </summary>
        public override IEntityGateway Clone()
        {
            return new FilterEntityGateway(this);
        }

        /// <summary>
        /// Get the row count for the filter gateway
        /// </summary>
        public override PagedRowCount GetRowCount()
        {
            // Get the row count from the base, to know if the actual keys are done loading yet
            PagedRowCount baseCount = base.GetRowCount();

            // We'll provide our known count from the filter data, but we still need to accurately report if the
            // keys are done loading in the background or not.
            return new PagedRowCount(filterCount.Count, baseCount.LoadingComplete);
        }

        /// <summary>
        /// Get the sorted key set for the configured gateway
        /// </summary>
        protected override PagedSortedKeys QuerySortedKeys(SortDefinition sortDefinition)
        {
            if (filterCount != null)
            {
                // If known to be zero rows, skip the querying altogether
                if (filterCount.Count == 0)
                {
                    return PagedSortedKeys.Empty;
                }

                // BN: Caching turned off due to FB113212.  Re-enable if we can make dirty detection more sophisticated.
                /* FilterContentCacheEntry cacheEntry = filterContentCache[GetCacheKey(sortDefinition)];
                if (cacheEntry != null)
                {
                    // If the count version is the same, and the result set wasn't terminated before it finished, then we can use it
                    if (cacheEntry.FilterCountVersion == filterCount.CountVersion)
                    {
                        if (!cacheEntry.SortedKeys.IsCanceled)
                        {
                            return cacheEntry.SortedKeys;
                        }
                    }
                }*/
            }

            PagedSortedKeys sortedKeys = base.QuerySortedKeys(sortDefinition);

            // BN: Caching turned off due to FB113212.  Re-enable if we can make dirty detection more sophisticated.
            /* if (filterCount != null)
            {
                // Cache for next time
                filterContentCache[GetCacheKey(sortDefinition)] = new FilterContentCacheEntry { SortedKeys = sortedKeys, FilterCountVersion = filterCount.CountVersion };
            }*/

            return sortedKeys;
        }

        /// <summary>
        /// Get the query relations\predicate used to filter on the configured filter node
        /// </summary>
        protected override RelationPredicateBucket GetQueryFilter()
        {
            bool isTopLevelFilter = false;

            // See if its a top-level Orders\Customers filter.
            if (filterCount != null && BuiltinFilter.IsTopLevelKey(filterCount.FilterNodeID))
            {
                isTopLevelFilter = true;
            }

            if (filterCount != null && !isTopLevelFilter)
            {
                RelationPredicateBucket bucket = new RelationPredicateBucket();

                // PrimaryKey in (SELECT ObjectID FROM FilterNodeContentDetail WHERE FilterNodeContentID = @filterNodeContentID)
                bucket.PredicateExpression.Add(new FieldCompareSetPredicate(
                    PrimaryKeyField, null, FilterNodeContentDetailFields.EntityID, null,
                    SetOperator.In, (FilterNodeContentDetailFields.FilterNodeContentID == filterCount.FilterNodeContentID)));

                return bucket;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Generate the key into our content cache, based on the filternodecontentid and the current sort
        /// </summary>
        private string GetCacheKey(SortDefinition sortDefinition)
        {
            return string.Format("{0}: {1}", filterCount.FilterNodeContentID, sortDefinition.GetDescription());
        }
    }
}
