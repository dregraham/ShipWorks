using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Filters.Management;
using ShipWorks.Data.Adapter.Custom;
using System.Data.SqlClient;
using ShipWorks.Data;
using ShipWorks.Data.Model.HelperClasses;
using System.Drawing;
using ShipWorks.Properties;
using ShipWorks.Data.Adapter;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions.Special;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Model;
using ShipWorks.Data.Grid.Columns;
using System.Data;
using ShipWorks.Users;
using System.Transactions;
using ShipWorks.SqlServer.Filters;
using log4net;
using System.Diagnostics;
using System.Threading;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Data;
using ShipWorks.Editions;

namespace ShipWorks.Filters
{
    /// <summary>
    /// Utility class for filters
    /// </summary>
    public static class FilterHelper
    {
        static readonly ILog log = LogManager.GetLogger(typeof(FilterHelper));

        const string myFiltersName = "My Filters";

        /// <summary>
        /// Indicates that the filter is a hard-coded filter required by ShipWorks
        /// </summary>
        public static bool IsBuiltin(FilterEntity filter)
        {
            if (!filter.IsFolder)
            {
                return false;
            }

            if (IsMyFiltersRoot(filter))
            {
                return true;
            }

            switch ((FilterTarget) filter.FilterTarget)
            {
                case FilterTarget.Orders:
                case FilterTarget.Customers:
                    return BuiltinFilter.IsTopLevelKey(filter.FilterID);

                default:
                    throw new InvalidOperationException("Unhandled FilterTarget in IsBuiltin");
            }
        }

        /// <summary>
        /// Indicates that the node is a hard-coded required node of ShipWorks
        /// </summary>
        public static bool IsBuiltin(FilterNodeEntity node)
        {
            return IsBuiltin(node.Filter);
        }

        /// <summary>
        /// Indicates if the given filter is a uneditable "My Filters"
        /// </summary>
        public static bool IsMyFiltersRoot(FilterEntity filter)
        {
            if (!filter.IsFolder)
            {
                return false;
            }

            if (filter.Name == myFiltersName)
            {
                if (filter.UsedBySequences.Count == 1)
                {
                    if (filter.UsedBySequences[0].ParentFilterID == null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// Indicates if the given filter is a uneditable "My Filters"
        /// </summary>
        public static bool IsMyFiltersRoot(FilterNodeEntity node)
        {
            return IsMyFiltersRoot(node.Filter);
        }

        /// <summary>
        /// Indicates if a rename operation is allowed on the given node
        /// </summary>
        public static bool CanRename(FilterNodeEntity node)
        {
            return !IsBuiltin(node);
        }

        /// <summary>
        /// Indicates if the node is a filter that is in the My Filters collection of the current user
        /// </summary>
        public static bool IsMyFilter(FilterNodeEntity node)
        {
            FilterLayoutEntity layout = FilterLayoutContext.Current.GetNodeLayout(node);

            return layout != null && layout.UserID == UserSession.User.UserID;
        }

        /// <summary>
        /// Translate the given exception to a FilterException, if we understand it.
        /// </summary>
        public static bool TranslateException(Exception ex)
        {
            if (ex is ORMConcurrencyException)
            {
                throw new FilterConcurrencyException("Another user has recently made changes. ShipWorks cannot save your changes since they would overwrite the other changes.", ex);
            }

            return false;
        }

        /// <summary>
        /// Ensure that the filter layout tables are in a valid state.  If they are not, its assumed because another user made edits,
        /// and so a concurrency exception is thrown.
        /// </summary>
        public static void ValidateFilterLayouts(SqlAdapter adapter)
        {
            try
            {
                ActionProcedures.ValidateFilterLayouts(adapter);
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("corrupt"))
                {
                    string message = "Another user has recently made changes. ShipWorks cannot save your changes since they would overwrite the other changes.";

#if DEBUG
                    message += "\n(DEBUG: InvalidFilterLayout)";
#endif

                    throw new FilterConcurrencyException(message, ex);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Create a new, unsaved layout entity that is initialized as the "My Filters" for the given user and target
        /// </summary>
        public static FilterLayoutEntity CreateMyLayout(UserEntity user, FilterTarget target)
        {
            FilterEntity filter = new FilterEntity();
            filter.Name = myFiltersName;
            filter.FilterTarget = (int) target;
            filter.IsFolder = true;

            FilterSequenceEntity sequence = new FilterSequenceEntity();
            sequence.Parent = null;
            sequence.Filter = filter;
            sequence.Position = 0;

            FilterNodeContentEntity content = new FilterNodeContentEntity();
            content.Status = (int) FilterCountStatus.Ready;
            content.InitialCalculation = "";
            content.UpdateCalculation = "";
            content.ColumnMask = new byte[0];
            content.JoinMask = 0;
            content.Cost = 0;
            content.Count = 0;
            content.CountVersion = 0;

            FilterNodeEntity node = new FilterNodeEntity();
            node.ParentNode = null;
            node.FilterSequence = sequence;
            node.FilterNodeContent = content;
            node.Created = DateTime.UtcNow;
            node.Purpose = (int) FilterNodePurpose.Standard;

            FilterLayoutEntity layout = new FilterLayoutEntity();
            layout.User = user;
            layout.FilterTarget = (int) target;
            layout.FilterNode = node;

            return layout;
        }

        /// <summary>
        /// Determines if the given name is a valid and unique filter name
        /// </summary>
        public static bool IsValidName(FilterEntity filter, string name)
        {
            int count = FilterCollection.GetCount(SqlAdapter.Default,
                FilterFields.Name == name & FilterFields.IsFolder == filter.IsFolder & FilterFields.FilterTarget == filter.FilterTarget);

            return count == 0;
        }

        /// <summary>
        /// Convert the given filter target to a grid target
        /// </summary>
        public static GridColumnDefinitionSet ConvertToGridColumnDefinitionSet(FilterTarget filterTarget)
        {
            switch (filterTarget)
            {
                case FilterTarget.Orders: return GridColumnDefinitionSet.Orders;
                case FilterTarget.Customers: return GridColumnDefinitionSet.Customers;
            }

            throw new InvalidOperationException("Unhandled filter target.");
        }

        /// <summary>
        /// Get the image to use for the given node
        /// </summary>
        public static Image GetFilterImage(FilterNodeEntity node)
        {
            FilterEntity filter = node.Filter;

            if (IsBuiltin(filter))
            {
                if (filter.Name == myFiltersName)
                {
                    return EditionManager.ActiveRestrictions.CheckRestriction(EditionFeature.MyFilters).Level == EditionRestrictionLevel.None ?
                        Resources.my_filters :
                        EditionGuiHelper.GetLockImage(16);
                }

                switch ((FilterTarget) filter.FilterTarget)
                {
                    case FilterTarget.Orders: return Resources.order16;
                    case FilterTarget.Customers: return Resources.customer16;
                    default:
                        throw new InvalidOperationException("Unhandled FilterTarget in GetFilterImage");
                }
            }

            if (node.Purpose == (int) FilterNodePurpose.Search)
            {
                return Resources.view;
            }

            if (IsFilterHardLinked(filter))
            {
                if (filter.IsFolder)
                {
                    return filter.Definition == null ? Resources.folderclosed_linked_infinity : Resources.folderfilter_linked_infinity;
                }
                else
                {
                    return Resources.funnel_linked_infinity;
                }
            }
            else if (IsFilterSoftLinked(filter))
            {
                if (filter.IsFolder)
                {
                    return filter.Definition == null ? Resources.folderclosed_linked_arrow : Resources.folderfilter_linked_arrow;
                }
                else
                {
                    return Resources.funnel_linked_arrow;
                }
            }
            else
            {
                if (filter.IsFolder)
                {
                    return filter.Definition == null ? Resources.folderclosed : Resources.folderfilter;
                }
                else
                {
                    return Resources.filter;
                }
            }
        }

        /// <summary>
        /// Get the image to use for the given type
        /// </summary>
        public static Image GetFilterImage(FilterTarget target)
        {
            return EntityUtility.GetEntityImage(FilterHelper.GetEntityType(target));
        }

        /// <summary>
        /// Indicates if the given filter is linked to multiple locations
        /// </summary>
        public static bool IsFilterHardLinked(FilterEntity filter)
        {
            // Using nodes rather than sequences would give false positives.  If you link a folder,
            // then the child filters will all have multiple nodes, but still one sequence.
            return GetSequencesUsingFilter(filter).Count > 1;
        }

        /// <summary>
        /// Indicates if the given filter is a child of a linked folder
        /// </summary>
        public static bool IsFilterSoftLinked(FilterEntity filter)
        {
            List<FilterNodeEntity> nodes = GetNodesUsingFilter(filter);

            // See how many sequences use it
            List<long> uniqueSequences = new List<long>();

            // Make the list of sequences
            foreach (FilterNodeEntity node in nodes)
            {
                if (!uniqueSequences.Contains(node.FilterSequenceID))
                {
                    uniqueSequences.Add(node.FilterSequenceID);
                }
            }

            // If there are more nodes, that means at least 2 nodes use 1 sequence, which is a soft link.
            return nodes.Count > uniqueSequences.Count;
        }

        /// <summary>
        /// Get all the nodes that use a given filter.
        /// </summary>
        public static List<FilterNodeEntity> GetNodesUsingFilter(FilterEntity filter)
        {
            // All the ones that match
            List<FilterNodeEntity> foundNodes = new List<FilterNodeEntity>();

            // Find them
            foreach (FilterSequenceEntity sequence in GetSequencesUsingFilter(filter))
            {
                foundNodes.AddRange(sequence.NodesUsingSequence);
            }

            return foundNodes;
        }

        /// <summary>
        /// Get a list of all the sequences that use a given filter
        /// </summary>
        public static List<FilterSequenceEntity> GetSequencesUsingFilter(FilterEntity filter)
        {
            return new List<FilterSequenceEntity>(filter.UsedBySequences);
        }

        /// <summary>
        /// Get a list of all nodes that are attached to the given sequence
        /// </summary>
        public static List<FilterNodeEntity> GetNodesUsingSequence(FilterSequenceEntity sequence)
        {
            return new List<FilterNodeEntity>(sequence.NodesUsingSequence);
        }

        /// <summary>
        /// Return the passed in node, as well as all its descendants (if its a folder)
        /// </summary>
        public static List<FilterNodeEntity> GetNodeAndDescendants(FilterNodeEntity filterNode)
        {
            List<FilterNodeEntity> nodes = new List<FilterNodeEntity>();
            nodes.Add(filterNode);

            foreach (FilterNodeEntity child in filterNode.ChildNodes)
            {
                nodes.AddRange(GetNodeAndDescendants(child));
            }

            return nodes;
        }

        /// <summary>
        /// Get the EntityType that the Target addresses
        /// </summary>
        public static EntityType GetEntityType(FilterTarget target)
        {
            switch (target)
            {
                case FilterTarget.Orders:
                    return EntityType.OrderEntity;

                case FilterTarget.Customers:
                    return EntityType.CustomerEntity;

                case FilterTarget.Shipments:
                    return EntityType.ShipmentEntity;

                case FilterTarget.Items:
                    return EntityType.OrderItemEntity;
            }

            throw new InvalidOperationException("Invalid FilterTarget value.");
        }

        /// <summary>
        /// Get the FilterNodeContentID for the given node
        /// </summary>
        public static long? GetFilterNodeContentID(long nodeID)
        {
            // If we are within a transaction right now, we don't want the FilterNodeContent table locked into that
            // transaction.  The FilterNodeContent table should actually never be in a transaction, so just suppressing
            // the transaction should make it so we never block or cause blocking.
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ResultsetFields resultFields = new ResultsetFields(1);
                resultFields.DefineField(FilterNodeFields.FilterNodeContentID, 0, "FilterNodeContentID", "");

                RelationPredicateBucket bucket = new RelationPredicateBucket(FilterNodeFields.FilterNodeID == nodeID);

                // Do the fetch
                DataTable result = new DataTable();

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.FetchTypedList(resultFields, result, bucket, 1, true);
                }

                if (result.Rows.Count == 1)
                {
                    return (long) result.Rows[0][0];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Indicates if the given object is in the filter contents of the specified filter content id
        /// </summary>
        public static bool IsObjectInFilterContent(long objectID, long filterContentID)
        {
            // If its a top-level, like "Orders", assume the object is in it
            if (BuiltinFilter.IsTopLevelKey(filterContentID))
            {
                return true;
            }

            if (EntityUtility.GetEntitySeed(filterContentID) != 14)
            {
                throw new InvalidOperationException(string.Format("{0} is not a valid FilterNodeContentID", filterContentID));
            }

            // If we are within a transaction right now, we don't want the FilterNodeContent table locked into that
            // transaction.  We may block if filter are being updated, but at least we won't cause blocking.
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    int count = FilterNodeContentDetailCollection.GetCount(adapter,
                        FilterNodeContentDetailFields.FilterNodeContentID == filterContentID &
                        FilterNodeContentDetailFields.ObjectID == objectID);

                    return count != 0;
                }
            }
        }

        /// <summary>
        /// Ensures that there are no pending filter calculations that need done.  If the timeout expires before we are able to begin
        /// doing a calculation, the method returns false.  If the timeout expires during the calculation, the method finishes and returns
        /// true. This method is different from FilterContentManager.CalculateUpdateCounts because it blocks until the counts are updated.
        /// </summary>
        public static bool EnsureFiltersUpToDate(TimeSpan timeout)
        {
            byte[] rowVersion;

            // Can't calculate filters within a transaction - would hose everything up
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                // We need to capture the DBTS on entry - we only care if filters get as up-to-date as what the dirty mark is now.
                // If changes just keep coming in and coming in, then update counts would always be needed, and we'd never consider filters
                // up-to-date
                using (SqlConnection con = SqlSession.Current.OpenConnection())
                {
                    rowVersion = (byte[]) SqlCommandProvider.ExecuteScalar(con, "SELECT @@DBTS");
                }
            }

            return EnsureFiltersUpToDate(timeout, rowVersion);
        }

        /// <summary>
        /// Ensures that there are no pending filter calculations that need done.  If the timeout expires before we are able to begin
        /// doing a calculation, the method returns false.  If the timeout expires during the calculation, the method finishes and returns
        /// true. This method is different from FilterContentManager.CalculateUpdateCounts because it blocks until the counts are updated.
        /// </summary>
        public static bool EnsureFiltersUpToDate(TimeSpan timeout, byte[] rowVersion)
        {
            Stopwatch timer = Stopwatch.StartNew();

            // Can't calculate filters within a transaction - would hose everything up
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                log.InfoFormat("Ensuring filters are at least as up to date as {0}", SqlUtility.GetTimestampValue(rowVersion));

                // Keep going until we run out of time
                while (timer.Elapsed < timeout)
                {
                    using (SqlConnection con = SqlSession.Current.OpenConnection())
                    {
                        object result = SqlCommandProvider.ExecuteScalar(con, "SELECT MIN(RowVersion) FROM FilterNodeContentDirty WITH (NOLOCK)");

                        // No dirty means we're up to date
                        if (result == null || ((byte[]) result).Length == 0)
                        {
                            log.InfoFormat("There are no dirty objects, filters are up to date.");
                            return true;
                        }

                        // Still some dirty - but they are all edited after we first entered this method, which means they are
                        // at least as up-to-date as when we were called - which is all we can hope for
                        if (SqlUtility.GetTimestampValue((byte[]) result) > SqlUtility.GetTimestampValue(rowVersion))
                        {
                            log.InfoFormat("Still some dirty objets, but all are up-to-date from the time of the request.");
                            return true;
                        }

                        log.InfoFormat("Still not ensured up-to-date ({0} < {1}), calculating and waiting...", SqlUtility.GetTimestampValue((byte[]) result), SqlUtility.GetTimestampValue(rowVersion));
                    }

                    // Ensure counts are running
                    FilterContentManager.CalculateUpdateCounts();

                    // Wait a little before looping again
                    Thread.Sleep(TimeSpan.FromSeconds(.5));
                }
            }

            log.InfoFormat("Timed out trying to ensure filters were updated.");
            return false;
        }
    }
}
