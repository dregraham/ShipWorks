using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using System.Data;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Model;

namespace ShipWorks.Filters.Management
{
    /// <summary>
    /// Utility functions for working with quick filters
    /// </summary>
    public static class QuickFilterHelper
    {
        static readonly ILog log = LogManager.GetLogger(typeof(QuickFilterHelper));

        /// <summary>
        /// Create a new quick filter.  It will not have been saved to the database.  The node\sequence\count will all be new and attached.
        /// </summary>
        public static FilterNodeEntity CreateQuickFilter(FilterTarget target)
        {
            FilterEntity filter = new FilterEntity();
            filter.Name = "Quick Filter";
            filter.FilterTarget = (int) target;
            filter.IsFolder = false;
            filter.Definition = null;
            filter.State = (int) FilterState.Enabled;

            FilterSequenceEntity sequence = new FilterSequenceEntity();
            sequence.Parent = null;
            sequence.Filter = filter;
            sequence.Position = 0;

            FilterNodeContentEntity content = new FilterNodeContentEntity();
            content.Count = 0;
            content.CountVersion = 0;
            content.Cost = 0;
            content.Status = (int) FilterCountStatus.Ready;
            content.InitialCalculation = "";
            content.UpdateCalculation = "";
            content.ColumnMask = new byte[0];
            content.JoinMask = 0;

            FilterNodeEntity node = new FilterNodeEntity();
            node.ParentNode = null;
            node.FilterSequence = sequence;
            node.FilterNodeContent = content;
            node.Created = DateTime.UtcNow;
            node.Purpose = (int)FilterNodePurpose.Quick;

            return node;
        }

        /// <summary>
        /// Delete the given quick filter node
        /// </summary>
        public static void DeleteQuickFilter(FilterNodeEntity filterNode)
        {
            if (filterNode == null)
            {
                throw new ArgumentNullException("filterNode");
            }

            if (filterNode.IsNew)
            {
                return;
            }

            FilterSequenceEntity sequence = filterNode.FilterSequence;
            FilterEntity filter = sequence.Filter;

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                adapter.DeleteEntity(filterNode);
                adapter.DeleteEntity(sequence);
                adapter.DeleteEntity(filter);

                adapter.Commit();
            }
        }

        /// <summary>
        /// Get all the quick filter nodes that are for the given filter target
        /// </summary>
        public static List<FilterNodeEntity> GetQuickFilters(FilterTarget target)
        {
            // Grab the sequence and filter of each node
            PrefetchPath2 prefetch = new PrefetchPath2(EntityType.FilterNodeEntity);
            prefetch.Add(FilterNodeEntity.PrefetchPathFilterSequence).SubPath.Add(FilterSequenceEntity.PrefetchPathFilter);

            // We want to grab all quick filters that are of the given target type
            RelationPredicateBucket bucket = new RelationPredicateBucket(
                FilterNodeFields.Purpose == (int) FilterNodePurpose.Quick &
                FilterFields.FilterTarget == (int) target);

            // Add the relationships for the bucket to get from FilterNode -> Filter
            bucket.Relations.Add(FilterNodeEntity.Relations.FilterSequenceEntityUsingFilterSequenceID);
            bucket.Relations.Add(FilterSequenceEntity.Relations.FilterEntityUsingFilterID);

            using (SqlAdapter adapter = new SqlAdapter())
            {
                FilterNodeCollection nodes = new FilterNodeCollection();
                adapter.FetchEntityCollection(nodes, bucket, prefetch);

                return nodes.ToList();
            }
        }

        /// <summary>
        /// Delete all quick filters that are no longer being used
        /// </summary>
        public static void DeleteAbandonedFilters()
        {
            log.InfoFormat("Delete abandoned quick filters....");

            FilterNodeCollection nodesToDelete = new FilterNodeCollection();

            using (SqlAdapter adapter = new SqlAdapter())
            {
                // Query all quick filters that are not referenced, and have were created at least 48 hours ago.  This is b\c when a local filter
                // is first created, its not referenced until the user eventually saves.  So that gives ample time for the user to do so without 
                // the filter being deleted from underneath.  If they had the window open over 48 hours without saving, I think then they'd crash, 
                // but then that's just stupid of them.
                RelationPredicateBucket bucket = new RelationPredicateBucket(
                    FilterNodeFields.Purpose == (int) FilterNodePurpose.Quick &
                    FilterNodeFields.Created < (DateTime.UtcNow - TimeSpan.FromDays(2)));

                bucket.PredicateExpression.AddWithAnd(
                    new FieldCompareSetPredicate(
                        FilterNodeFields.FilterNodeID, null,
                        ObjectReferenceFields.ObjectID, null,
                        SetOperator.In, null,
                        true));

                PrefetchPath2 prefetch = new PrefetchPath2(EntityType.FilterNodeEntity);
                prefetch.Add(FilterNodeEntity.PrefetchPathFilterSequence);

                // Do the fetch
                adapter.FetchEntityCollection(nodesToDelete, bucket, prefetch);
            }

            if (nodesToDelete.Count > 0)
            {
                foreach (FilterNodeEntity node in nodesToDelete)
                {
                    using (SqlAdapter adapter = new SqlAdapter(true))
                    {
                        // Load the sequence
                        log.InfoFormat("Deleting abandoned quick filter node {0}", node.FilterNodeID);

                        // Delete the node
                        adapter.DeleteEntitiesDirectly(typeof(FilterNodeEntity), new RelationPredicateBucket(FilterNodeFields.FilterNodeID == node.FilterNodeID));

                        // Delete the sequence
                        adapter.DeleteEntitiesDirectly(typeof(FilterSequenceEntity), new RelationPredicateBucket(FilterSequenceFields.FilterSequenceID == node.FilterSequenceID));

                        // Delete the filter
                        adapter.DeleteEntitiesDirectly(typeof(FilterEntity), new RelationPredicateBucket(FilterFields.FilterID == node.FilterSequence.FilterID));

                        adapter.Commit();
                    }

                    // If the user exits idle, then stop
                    if (!IdleWatcher.IsIdle)
                    {
                        return;
                    }
                }
            }
        }
    }
}
