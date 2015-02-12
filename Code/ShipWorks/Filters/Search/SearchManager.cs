using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Data.Adapter.Custom;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data;
using ShipWorks.Data.Model;

namespace ShipWorks.Filters.Search
{
    /// <summary>
    /// Helps with various search stuff
    /// </summary>
    public static class SearchManager
    {
        static Dictionary<FilterTarget, FilterNodeEntity> searchPlaceholders;

        /// <summary>
        /// Initialize our search stuff 
        /// </summary> 
        public static void InitializeForCurrentSession()
        {
            searchPlaceholders = new Dictionary<FilterTarget, FilterNodeEntity>();

            using (SqlAdapter adapter = new SqlAdapter())
            {
                FilterNodeCollection nodes = new FilterNodeCollection();

                // Load the search nodes
                RelationPredicateBucket bucket = new RelationPredicateBucket(
                    FilterNodeFields.FilterNodeID == BuiltinFilter.GetSearchPlaceholderKey(FilterTarget.Orders) |
                    FilterNodeFields.FilterNodeID == BuiltinFilter.GetSearchPlaceholderKey(FilterTarget.Customers));

                // Get the sequence and filter with it
                PrefetchPath2 prefetch = new PrefetchPath2(EntityType.FilterNodeEntity);
                prefetch.Add(FilterNodeEntity.PrefetchPathFilterSequence).SubPath.Add(FilterSequenceEntity.PrefetchPathFilter);

                // Do the fetch
                adapter.FetchEntityCollection(nodes, bucket, prefetch);

                // Save the results
                searchPlaceholders[FilterTarget.Orders] = FindPlaceholder(nodes, FilterTarget.Orders);
                searchPlaceholders[FilterTarget.Customers] = FindPlaceholder(nodes, FilterTarget.Customers);
            }
        }

        /// <summary>
        /// Get the placeholder for the specified target
        /// </summary>
        public static FilterNodeEntity GetPlaceholder(FilterTarget target)
        {
            return searchPlaceholders[target];
        }

        /// <summary>
        /// Find the placeholder for the given target within the specified node set
        /// </summary>
        private static FilterNodeEntity FindPlaceholder(FilterNodeCollection nodes, FilterTarget target)
        {
            foreach (FilterNodeEntity node in nodes)
            {
                if (node.Filter.FilterTarget == (int) target)
                {
                    return node;
                }
            }

            throw new InvalidOperationException("No search placeholder was found for target " + target);
        }

        /// <summary>
        /// Create the single-instance search filter in the database.
        /// </summary>
        public static void CreateSearchPlaceholder(FilterTarget target)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                // We will be specifying the pk values
                adapter.IdentityInsert = true;

                try
                {
                    long pkValue = BuiltinFilter.GetSearchPlaceholderKey(target);

                    FilterEntity filter = new FilterEntity();
                    filter.FilterID = pkValue;
                    filter.Name = "Search";
                    filter.FilterTarget = (int) target;
                    filter.IsFolder = false;
                    filter.Definition = null;
                    filter.State = (int) FilterState.Enabled;
                    adapter.SaveAndRefetch(filter);

                    FilterSequenceEntity sequence = new FilterSequenceEntity();
                    sequence.FilterSequenceID = pkValue;
                    sequence.Parent = null;
                    sequence.Filter = filter;
                    sequence.Position = 0;
                    adapter.SaveAndRefetch(sequence);

                    FilterNodeContentEntity content = new FilterNodeContentEntity();
                    content.FilterNodeContentID = pkValue;
                    content.Count = 0;
                    content.CountVersion = 0;
                    content.Cost = 0;
                    content.Status = (int) FilterCountStatus.Ready;
                    content.InitialCalculation = "";
                    content.UpdateCalculation = "";
                    content.ColumnMask = new byte[0];
                    content.JoinMask = 0;
                    adapter.SaveAndRefetch(content);

                    FilterNodeEntity node = new FilterNodeEntity();
                    node.FilterNodeID = pkValue;
                    node.ParentNode = null;
                    node.FilterSequence = sequence;
                    node.FilterNodeContent = content;
                    node.Created = DateTime.UtcNow;
                    node.Purpose = (int) FilterNodePurpose.Search;
                    adapter.SaveAndRefetch(node);
                }
                finally
                {
                    adapter.IdentityInsert = false;
                }
            }
        }
    }
}
