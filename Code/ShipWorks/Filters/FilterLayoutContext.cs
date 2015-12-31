using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Interapptive.Shared;
using Interapptive.Shared.Data;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Editions;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Filters.Management;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Filters
{
    /// <summary>
    /// Provides functionality for effeciently loading filter layouts
    /// </summary>
    [NDependIgnoreLongTypes]
    public class FilterLayoutContext
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(FilterLayoutContext));

        // The loaded layouts
        List<FilterLayoutEntity> layouts = new List<FilterLayoutEntity>();

        // The user used to load My Layout
        UserEntity user;

        // Backing LLBLGen context to make sure everything has identity correctly
        Context context = new Context();

        // Scope stack
        static List<FilterLayoutContext> instanceScope = new List<FilterLayoutContext>();

        /// <summary>
        /// Initialize the global layout context instance for the currently logged on user
        /// </summary>
        public static void InitializeForCurrentSession()
        {
            if (instanceScope.Count > 1)
            {
                throw new InvalidOperationException("Someone forgot to pop a scope.");
            }

            if (instanceScope.Count == 1)
            {
                PopScope();
            }

            instanceScope.Add(new FilterLayoutContext(UserSession.User));
            instanceScope[0].Reload();
        }

        /// <summary>
        /// Pushes a new FilterLayoutContext into scope, which will be the one returned by the Instance method until PopScope is called.  This
        /// method was put here for the FilterOrganizerDlg, so that editing of filter can happen outside of the main FilterLayoutContext, to avoid
        /// havoc with the MainForm as it tries to read deleted nodes and stuff.
        /// </summary>
        public static void PushScope()
        {
            instanceScope.Insert(0, new FilterLayoutContext(UserSession.User));
            instanceScope[0].Reload();
        }

        /// <summary>
        /// Pops a scope pushed by PushScope
        /// </summary>
        public static void PopScope()
        {
            instanceScope[0].ClearContext();
            instanceScope.RemoveAt(0);
        }

        /// <summary>
        /// The single global FilterLayoutContext instance
        /// </summary>
        public static FilterLayoutContext Current
        {
            get
            {
                if (instanceScope.Any())
                {
                    return instanceScope[0];
                }

                log.Warn("Could not get current filter layout context");
                return null;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private FilterLayoutContext(UserEntity user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            this.user = user;
        }

        #region Loading

        /// <summary>
        /// Clear the context of all loaded filters and layouts
        /// </summary>
        private void ClearContext()
        {
            layouts.Clear();
            context = new Context();
        }

        /// <summary>
        /// Reload the layout information from the database
        /// </summary>
        public void Reload()
        {
            ClearContext();

            ResultsetFields resultFields = new ResultsetFields(1);
            resultFields.DefineField(FilterLayoutFields.FilterLayoutID, 0, "FilterLayoutID", "");

            // Do the fetch
            DataTable result = new DataTable();

            ExistingConnectionScope.ExecuteWithAdapter(sqlAdapter => sqlAdapter.FetchTypedList(resultFields, result, null));

            // Load all the layouts
            foreach (long layoutID in result.Rows.Cast<DataRow>().Select(r => (long) r[0]))
            {
                LoadLayout(layoutID);
            }
        }

        /// <summary>
        /// Load the layout with the specified ID, ensureing its for the given target.
        /// </summary>
        private void LoadLayout(long layoutID)
        {
            FilterLayoutEntity layout = new FilterLayoutEntity(layoutID);
            context.Add(layout);

            ExistingConnectionScope.ExecuteWithAdapter(sqlAdapter => sqlAdapter.FetchEntity(layout, BuildLayoutPrefetchPath(layoutID)));

            // Ensure we got it
            if (layout.Fields.State != EntityState.Fetched)
            {
                throw new InvalidOperationException(string.Format("Could not fetch layout {0}.", layoutID));
            }

            layouts.Add(layout);
        }

        /// <summary>
        /// Get the shared layout for the given target.
        /// </summary>
        public FilterLayoutEntity GetSharedLayout(FilterTarget target)
        {
            FilterLayoutEntity layout = layouts.SingleOrDefault(l => l.FilterTarget == (int) target && l.UserID == null);

            if (layout == null)
            {
                throw new NotFoundException(string.Format("Shared layout for target {0} not found.", target));
            }

            return layout;
        }

        /// <summary>
        /// Get the custom layout of personal filters for the given user
        /// </summary>
        public FilterLayoutEntity GetMyLayout(FilterTarget target)
        {
            FilterLayoutEntity layout;

            // Special case for the SuperUser - who won't have his own actual personal layouts.  This code runs during the initial filter load when
            // creating a new database.  There are no users yet - but we need the LayoutContext in order to create filters.
            if (user.UserID == SuperUser.UserID)
            {
                layout = new FilterLayoutEntity { UserID = user.UserID, FilterTarget = (int) target };
                layout.IsDirty = false;
                layout.IsNew = false;
            }
            else
            {
                layout = layouts.SingleOrDefault(l => l.FilterTarget == (int) target && l.UserID == user.UserID);

                if (layout == null)
                {
                    throw new NotFoundException(string.Format("Shared layout for target {0} not found.", target));
                }
            }

            return layout;
        }

        /// <summary>
        /// Build a prefetch path that will get every child node, sequence, and filter for this layout.
        /// </summary>
        private IPrefetchPath2 BuildLayoutPrefetchPath(long layoutID)
        {
            PrefetchPath2 prefetchPath = new PrefetchPath2((int) EntityType.FilterLayoutEntity);
            IPrefetchPathElement2 nodePath = prefetchPath.Add(FilterLayoutEntity.PrefetchPathFilterNode);

            int levelsDeep = GetFilterNodeLevels(layoutID);

            // We actually have to go another level deeper in the prefetch, so that the object model _knows_ there is nothing deeper, and the child
            // collections are prefilled with zero entries, rather than being null and needing lazy loaded.
            levelsDeep++;

            while (levelsDeep > 0)
            {
                // We need to get it's sequence, and its filter
                nodePath.SubPath.Add(FilterNodeEntity.PrefetchPathFilterSequence).SubPath.Add(FilterSequenceEntity.PrefetchPathFilter);

                // If its not the last level
                if (levelsDeep > 1)
                {
                    RelationCollection nodeRelationToSequence = new RelationCollection();
                    nodeRelationToSequence.Add(FilterNodeEntity.Relations.FilterSequenceEntityUsingFilterSequenceID);

                    // We need the children sorted by the position of there corresponding sequenc entities
                    ISortExpression sorter = new SortExpression(FilterSequenceFields.Position | SortOperator.Ascending);

                    // We need to get all the children of this node.  This is where the recursion happens,
                    // where we reassign nodepath to be the children.
                    nodePath = nodePath.SubPath.Add(FilterNodeEntity.PrefetchPathChildNodes, 0, null, nodeRelationToSequence, sorter);
                }

                levelsDeep--;
            }

            return prefetchPath;
        }

        /// <summary>
        /// Get the number of filter node levels below the root in the given layout
        /// </summary>
        private int GetFilterNodeLevels(long layoutID)
        {
            return ExistingConnectionScope.ExecuteWithCommand(cmd =>
            {
                cmd.CommandText = "SELECT dbo.GetFilterNodeLevels(@FilterLayoutID)";
                cmd.Parameters.AddWithValue("@FilterLayoutID", layoutID);

                return (int) SqlCommandProvider.ExecuteScalar(cmd);
            });
        }

        #endregion

        #region Layout Information \ Refreshing

        /// <summary>
        /// Get the filter layout that ultimately contains this node.  Null if the node is a quick filter.
        /// </summary>
        public FilterLayoutEntity GetNodeLayout(FilterNodeEntity node)
        {
            while (node.ParentNode != null)
            {
                node = node.ParentNode;
            }

            // Local nodes don't have layouts
            if (node.Purpose == (int) FilterNodePurpose.Quick)
            {
                return null;
            }

            FilterLayoutEntity sharedLayout = GetSharedLayout((FilterTarget) node.Filter.FilterTarget);
            if (sharedLayout.FilterNode == node)
            {
                return sharedLayout;
            }

            FilterLayoutEntity myLayout = GetMyLayout((FilterTarget) node.Filter.FilterTarget);
            if (myLayout.FilterNode == node)
            {
                return myLayout;
            }

            return null;
        }

        /// <summary>
        /// Find the node with the given ID with the layout.  Returns null if not found.  If the nodeID represents a Quick Filter, it will
        /// be returned regardless of whether it is contained in this layout.
        /// </summary>
        public FilterNodeEntity FindNode(long nodeID)
        {
            foreach (FilterNodeEntity node in context.GetAll(typeof(FilterNodeEntity)))
            {
                if (node.Fields.State == EntityState.Deleted)
                {
                    continue;
                }

                if (node.FilterNodeID == nodeID)
                {
                    return node;
                }
            }

            // No local nodes will have a negative ID.  Though our builtins do, which is why we don't do this check first thing.
            if (nodeID <= 0)
            {
                return null;
            }

            // For the sake of making this method do what is usually expected, we also go outside of what is actually loaded and try to
            // load local nodes
            // See if we can load it directly if its a quick filter
            using (SqlAdapter adapter = SqlAdapter.Create(false))
            {
                PrefetchPath2 prefetch = new PrefetchPath2(EntityType.FilterNodeEntity);
                prefetch.Add(FilterNodeEntity.PrefetchPathFilterSequence).SubPath.Add(FilterSequenceEntity.PrefetchPathFilter);

                FilterNodeEntity node = new FilterNodeEntity(nodeID);
                adapter.FetchEntity(node, prefetch);

                if (node.Fields.State == EntityState.Fetched && node.Purpose == (int) FilterNodePurpose.Quick)
                {
                    return node;
                }
            }

            return null;
        }

        /// <summary>
        /// Indicates if this in-memory version of the layout differs from the database.
        /// </summary>
        public bool IsLayoutDirty()
        {
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                // We need to detect any changes to sequences or nodes
                SqlCommand cmd = SqlCommandProvider.Create(con);
                cmd.CommandText = @"
                    SELECT CAST(MAX(RowVersion) as bigint)
                      FROM FilterLayout";

                // We're using this version of ExecuteScalar because it will throw a better exception if the returned value cannot be cast.
                // A customer was crashing on a NullReferenceException and the original cast to long was one possible location of the crash.
                long dbTimestamp = SqlCommandProvider.ExecuteScalar<long>(cmd);

                // Get the local timestamp
                long localTimestamp = layouts.Max(l => SqlUtility.GetTimestampValue(l.RowVersion));

                // See if its dirty
                return dbTimestamp != localTimestamp;
            }
        }

        /// <summary>
        /// Refreshes all filters with latest content from the database.  Returns the list of any that changed.
        /// </summary>
        public List<FilterEntity> RefreshFilters()
        {
            EntityCollection<FilterEntity> filters = new EntityCollection<FilterEntity>();
            context.Add(filters);

            using (SqlAdapter adapter = new SqlAdapter())
            {
                RelationPredicateBucket bucket = new RelationPredicateBucket(FilterFields.RowVersion > GetLocalFilterTimestamp());

                // Get only those filters that have changed
                adapter.FetchEntityCollection(filters, bucket);

                List<FilterEntity> changed = new List<FilterEntity>(filters.Items);

                return changed;
            }
        }

        /// <summary>
        /// Get the max timestamp that we have locally loaded for filters
        /// </summary>
        private byte[] GetLocalFilterTimestamp()
        {
            long timestamp = 0;
            byte[] rowversion = new byte[8];

            foreach (FilterEntity filter in context.GetAll(typeof(FilterEntity)))
            {
                long filterTimestamp = SqlUtility.GetTimestampValue(filter.RowVersion);
                if (filterTimestamp > timestamp)
                {
                    timestamp = filterTimestamp;
                    rowversion = filter.RowVersion;
                }
            }

            return rowversion;
        }

        /// <summary>
        /// Get the number of filters (not including folders) contained by top-level filters layouts, not including 'My' fiters
        /// </summary>
        public int GetTopLevelFilterCount()
        {
            int count = 0;

            foreach (FilterTarget target in new FilterTarget[] { FilterTarget.Orders, FilterTarget.Customers })
            {
                FilterLayoutEntity layout = GetSharedLayout(target);

                count += GetDescendantFilterCount(layout.FilterNode);
            }

            return count;
        }

        /// <summary>
        /// Get the count of descendant filters of the given node.  Should only be used by GetTopLevelFilterCount function
        /// </summary>
        private int GetDescendantFilterCount(FilterNodeEntity node)
        {
            if (node.Fields.State == EntityState.Deleted)
            {
                return 0;
            }

            FilterEntity filter = node.Filter;

            if (filter == null)
            {
                return 0;
            }

            if (filter.IsFolder)
            {
                return node.ChildNodes.Sum(child => GetDescendantFilterCount(child));
            }
            else
            {
                return (node.Purpose == (int) FilterNodePurpose.Standard) ? 1 : 0;
            }
        }

        #endregion

        #region Editing

        /// <summary>
        /// Save the given filter
        /// </summary>
        public void SaveFilter(FilterEntity filter)
        {
            // Execute in a transaction
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                SaveFilter(filter, adapter);

                adapter.Commit();
            }
        }

        /// <summary>
        /// Save the given filter
        /// </summary>
        public void SaveFilter(FilterEntity filter, SqlAdapter adapter)
        {
            try
            {
                bool filterDefinitionChanged = filter.Fields[(int) FilterFieldIndex.Definition].IsChanged || filter.Fields[(int) FilterFieldIndex.State].IsChanged;

                // Fetch list of nodes befor refetching.
                List<FilterNodeEntity> nodesToUpdate = GetNodesAffectedByDefinition(filter);
                //IEnumerable<long> filterNodesIDsToUpdate = nodesToUpdate
                //    //.Where(nodeToUpdate => nodeToUpdate.Fields[(int)FilterNodeFieldIndex.State].IsChanged || nodeToUpdate.Filter.IsFolder)
                //    .Where(nodeToUpdate => nodeToUpdate.Filter.IsFolder)
                //    .Select(n => n.FilterNodeID)
                //    .ToList();

                // First try to save the filter itself
                adapter.SaveAndRefetch(filter);

                // Grab the nodes again after refetch.
                nodesToUpdate = GetNodesAffectedByDefinition(filter);

                // Go through each node that needs its sql updated
                foreach (FilterNodeEntity node in nodesToUpdate)
                {
                    //if (filterDefinitionChanged || filterNodesIDsToUpdate.Any(noteToUpdate => noteToUpdate == node.FilterNodeID))
                    if (filterDefinitionChanged)
                    {
                        node.FilterNodeContent = FilterContentManager.CreateNewFilterContent(new FilterContentCalculation(node), adapter);
                        adapter.SaveAndRefetch(node);
                    }
                }
            }
            catch (Exception ex)
            {
                FilterHelper.TranslateException(ex);

                // Rethrow, if the translate didnt
                throw;
            }
        }

        /// <summary>
        /// Regenerate all filter nodes that are affected by a DateCondition that contains a relative date.
        /// </summary>
        public void RegenerateDateFilters(SqlAdapter adapter)
        {
            List<FilterEntity> dateFilters = new List<FilterEntity>();

            // Look at all filters
            foreach (FilterEntity filter in context.GetAll(typeof(FilterEntity)))
            {
                if (filter.Definition != null)
                {
                    // This filter is based on a relative date - all its nodes will need recalculated
                    FilterDefinition definition = new FilterDefinition(filter.Definition);
                    if (definition.HasRelativeDateCondition())
                    {
                        log.DebugFormat("Filter {0} uses relative dates", filter.Name);

                        dateFilters.Add(filter);
                    }
                }
            }

            // We have to make sure and do "Quick Filters" too, which will not be already in our context
            PrefetchPath2 prefetch = new PrefetchPath2(EntityType.FilterNodeEntity);
            prefetch.Add(FilterNodeEntity.PrefetchPathFilterSequence).SubPath.Add(FilterSequenceEntity.PrefetchPathFilter);

            FilterNodeCollection localNodes = FilterNodeCollection.Fetch(adapter, FilterNodeFields.Purpose == (int) FilterNodePurpose.Quick, prefetch);
            foreach (FilterNodeEntity localNode in localNodes)
            {
                FilterEntity filter = localNode.Filter;

                if (filter.Definition != null)
                {
                    // This filter is based on a relative date - its nodes will need recalculated
                    FilterDefinition definition = new FilterDefinition(filter.Definition);
                    if (definition.HasRelativeDateCondition())
                    {
                        log.DebugFormat("Quick filter {0} uses relative dates", filter.Name);

                        dateFilters.Add(filter);
                    }
                }
            }

            // Regenerate all the date filters
            RegenerateFilterSql(dateFilters, adapter);
        }

        /// <summary>
        /// Regenerates the filter sql for all filters in the database.  This is used by the database updater anytime there is a schema change.
        ///
        /// If there are outstanding "Search" filters they will be regnerated but not calculated.  This is b\c when regenerating the will be marked as needing initial counts,
        /// but the search engine and not the filter update engine is supposed to be responsible for search filter initial counts.  So in that case, the search results will just
        /// always be spinning until the user canceled or updated the search.  That said, given we only use this during upgrade, that shouldn't even really be possible.
        /// </summary>
        public void RegenerateAllFilters(SqlAdapter adapter)
        {
            List<FilterEntity> allFilters = new List<FilterEntity>();

            // Look at all filters
            foreach (FilterEntity filter in context.GetAll(typeof(FilterEntity)))
            {
                // Don't regenerate the top-level (Orders, Customers), or Search - b\c they don't use filter sql calculations
                if (!BuiltinFilter.IsTopLevelKey(filter.FilterID) && !BuiltinFilter.IsSearchPlaceholderKey(filter.FilterID))
                {
                    allFilters.Add(filter);
                }
            }

            PrefetchPath2 prefetch = new PrefetchPath2(EntityType.FilterNodeEntity);
            prefetch.Add(FilterNodeEntity.PrefetchPathFilterSequence).SubPath.Add(FilterSequenceEntity.PrefetchPathFilter);

            // We have to make sure and do "Quick Filters" and "Search" too, which will not be already in our context.  See the comments on the function summary
            // for information about the Search filters
            FilterNodeCollection localNodes = FilterNodeCollection.Fetch(adapter,

                // Quick filters
                FilterNodeFields.Purpose == (int) FilterNodePurpose.Quick |

                // Or search filters that are real (not the placeholders)
                (FilterNodeFields.Purpose == (int) FilterNodePurpose.Search & FilterNodeFields.FilterNodeID > 0), prefetch);

            foreach (FilterNodeEntity localNode in localNodes)
            {
                FilterEntity filter = localNode.Filter;

                allFilters.Add(filter);
            }

            // Regenerate all the date filters
            RegenerateFilterSql(allFilters, adapter);
        }

        /// <summary>
        /// Regenerate the given list of filters.  All affected nodes (even those due to folder conditions) will be regnerated automatically as well.
        /// </summary>
        private void RegenerateFilterSql(List<FilterEntity> filters, SqlAdapter adapter)
        {
            // Ordered list of affected nodes, from children up, so folders are accurate
            List<FilterNodeEntity> affectedNodes = new List<FilterNodeEntity>();

            // Look at all filters
            foreach (FilterEntity filter in filters)
            {
                foreach (FilterNodeEntity node in GetNodesAffectedByDefinition(filter))
                {
                    log.DebugFormat("  Adding node {0} ({1})", node.FilterNodeID, node.Filter.Name);

                    // The nodes are ordered from children -> parent.  We need to make sure
                    // all parents stay to the right of children for the counts to work.
                    if (affectedNodes.Contains(node))
                    {
                        log.DebugFormat("    already present, moving to end");
                        affectedNodes.Remove(node);
                    }

                    affectedNodes.Add(node);
                }
            }

            // Now we have a unique list of affected nodes, ordered from children -> parent
            foreach (FilterNodeEntity node in affectedNodes)
            {
                log.InfoFormat("Regenerating sql for node {0} ({1})", node.FilterNodeID, node.Filter.Name);

                node.FilterNodeContent = FilterContentManager.CreateNewFilterContent(new FilterContentCalculation(node), adapter);
                adapter.SaveAndRefetch(node);
            }
        }

        /// <summary>
        /// Get all the nodes that are affected by a definition change of the given filter
        /// </summary>
        private List<FilterNodeEntity> GetNodesAffectedByDefinition(FilterEntity filter)
        {
            List<FilterNodeEntity> nodesToUpdate = new List<FilterNodeEntity>();
            List<FilterNodeEntity> nodesUsingFilter = FilterHelper.GetNodesUsingFilter(filter);

            // Note: The ordering matters.  The order the nodes are processed in does matter due to
            // folder dependencies on their child FilterNodeContentID's.

            // 1) For a folder, we need to update every child node that is a filter
            if (filter.IsFolder)
            {
                foreach (FilterNodeEntity node in nodesUsingFilter)
                {
                    // Add the children
                    nodesToUpdate.AddRange(GetChildFilterNodes(node));
                }
            }

            // 2) Add all the nodes used by the filter
            nodesToUpdate.AddRange(nodesUsingFilter);

            // 3) Add in all ancestor folders
            nodesToUpdate.AddRange(GetAncestorsAffectedByDefinition(filter));

            return nodesToUpdate;
        }

        /// <summary>
        /// Get all ancestor folder nodes that would be affected by a change to the given filter definition
        /// </summary>
        private List<FilterNodeEntity> GetAncestorsAffectedByDefinition(FilterEntity filter)
        {
            List<FilterNodeEntity> nodesToUpdate = new List<FilterNodeEntity>();
            List<FilterNodeEntity> nodesUsingFilter = FilterHelper.GetNodesUsingFilter(filter);

            // We have to update all parent nodes
            foreach (FilterNodeEntity node in nodesUsingFilter)
            {
                nodesToUpdate.AddRange(GetAncestorsAffectedByDefinition(node));
            }

            return nodesToUpdate;
        }

        /// <summary>
        /// Get all the ancestor folder nodes affected by a change to the given node's filter definition
        /// </summary>
        private List<FilterNodeEntity> GetAncestorsAffectedByDefinition(FilterNodeEntity node)
        {
            List<FilterNodeEntity> nodesToUpdate = new List<FilterNodeEntity>();

            FilterNodeEntity parent = node.ParentNode;
            while (parent != null && (!FilterHelper.IsBuiltin(parent) || FilterHelper.IsMyFiltersRoot(parent)))
            {
                nodesToUpdate.Add(parent);
                parent = parent.ParentNode;
            }

            return nodesToUpdate;
        }

        /// <summary>
        /// Recursively get every child filter node for the given folder node.  Children are added before their parents.
        /// </summary>
        private List<FilterNodeEntity> GetChildFilterNodes(FilterNodeEntity folderNode)
        {
            List<FilterNodeEntity> childNodes = new List<FilterNodeEntity>();

            foreach (FilterNodeEntity childNode in folderNode.ChildNodes)
            {
                // If its a folder, add its children first
                if (childNode.Filter.IsFolder)
                {
                    childNodes.AddRange(GetChildFilterNodes(childNode));
                }

                // Then add the folder itself
                childNodes.Add(childNode);
            }

            return childNodes;
        }

        /// <summary>
        /// Add the specified filter as a child of the given parent in the given position.  Returns the list of nodes that were created.
        /// Multiple nodes may be created if the parent is linked.
        /// </summary>
        public List<FilterNodeEntity> AddFilter(FilterEntity filter, FilterNodeEntity parentNode, int position)
        {
            using (SqlAdapter adapter = SqlAdapter.Create(true))
            {
                List<FilterNodeEntity> result = AddFilter(filter, parentNode, position, adapter);

                adapter.Commit();

                return result;
            }
        }

        /// <summary>
        /// Add the specified filter as a child of the given parent in the given position.  Returns the list of nodes that were created.
        /// Multiple nodes may be created if the parent is linked.
        /// </summary>
        public List<FilterNodeEntity> AddFilter(FilterEntity filter, FilterNodeEntity parentNode, int position, SqlAdapter adapter)
        {
            // Every node has at least one sequence describing its position
            FilterSequenceEntity sequence = new FilterSequenceEntity();
            sequence.Filter = filter;

            // Now we need a new FilterNode to represent it in the hiearchy.  We may need more than one - we'll need one for
            // each link its parent has.  But Move will take care of that.
            FilterNodeEntity filterNode = new FilterNodeEntity();
            filterNode.FilterSequence = sequence;
            filterNode.Filter.State = (int) FilterState.Enabled;

            // Since we don't exist anywhere yet, this doesnt actually add a link, its the first one
            return AddNodeToParent(filterNode, parentNode, position, adapter);
        }

        /// <summary>
        /// Add a link between the itemToLink and the new parent folder
        /// </summary>
        public List<FilterNodeEntity> AddLink(FilterNodeEntity nodeToLink, FilterNodeEntity parentNode, int position)
        {
            return AddNodeToParent(nodeToLink, parentNode, position);
        }

        /// <summary>
        /// Adds the given node as a child of the specified parent.  The original node is not moved or changed.  A link is created in
        /// the destination parent. Multiple nodes may be created if the parent is linked.
        /// </summary>
        private List<FilterNodeEntity> AddNodeToParent(FilterNodeEntity filterNode, FilterNodeEntity parentNode, int position)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                List<FilterNodeEntity> result = AddNodeToParent(filterNode, parentNode, position, adapter);

                adapter.Commit();

                return result;
            }
        }

        /// <summary>
        /// Adds the given node as a child of the specified parent.  The original node is not moved or changed.  A link is created in
        /// the destination parent. Multiple nodes may be created if the parent is linked.
        /// </summary>
        private List<FilterNodeEntity> AddNodeToParent(FilterNodeEntity filterNode, FilterNodeEntity parentNode, int position, SqlAdapter adapter)
        {
            List<FilterNodeEntity> result = SetNodeToParent(filterNode, parentNode, position, false, adapter);

            FilterHelper.ValidateFilterLayouts(adapter);

            return result;
        }

        /// <summary>
        /// Adds the given node as a child of the specified parent.  If move is true, then the node is moved to the target location.  Any soft-links from the
        /// original location are deleted.  If move is false, a copy is made, and all original nodes are left as they were.
        /// </summary>
        [NDependIgnoreLongMethod]
        private List<FilterNodeEntity> SetNodeToParent(FilterNodeEntity filterNode, FilterNodeEntity parentNode, int position, bool move, SqlAdapter adapter)
        {
            FilterEntity filter = filterNode.Filter;
            FilterEntity parentFilter = parentNode.Filter;

            // Make sure the move is valid
            if (filter.FilterTarget != parentFilter.FilterTarget)
            {
                throw new FilterInvalidLocationException(string.Format("Cannot add a {0} filter to a {1} layout.", filter.FilterTarget, parentFilter.FilterTarget));
            }

            // Make sure we arent linking ourselves to somewhere we already are
            string reason;
            if (!CanAddChild(filter, parentNode, out reason))
            {
                throw new FilterInvalidLocationException(reason);
            }

            // Special case for new nodes that arent already in the hierarchy.  If we didn't do this, then due to the recursive
            // saves, this new node, which is basically just a prototype telling us what to add, would be actually added to
            // the database.
            if (filterNode.IsNew)
            {
                if (move)
                {
                    throw new InvalidOperationException("Cannot move an element that is still new.");
                }

                filterNode.FilterSequence.Filter = null;
                filterNode.FilterSequence = null;
            }

            List<FilterNodeEntity> affectedNodes = new List<FilterNodeEntity>();

            // If moving, after the move we have to be sure to update the counts of any ancestors that the node used to be in
            List<FilterNodeEntity> preMoveAncestors = null;
            if (move)
            {
                // Getting all the ancestores that could be potentially affected by this move, so we can update them after the move
                preMoveAncestors = GetAncestorsAffectedByDefinition(filter);
            }

            try
            {
                // First make sure we can save the filter
                adapter.SaveAndRefetch(filter);

                // Have to adjust following sibling sequences
                for (int i = position; i < parentFilter.ChildSequences.Count; i++)
                {
                    FilterSequenceEntity sibling = parentFilter.ChildSequences[i];
                    sibling.Position++;

                    adapter.SaveAndRefetch(sibling);
                }

                // We only need one sequence - every new node references that sequence, and will thus
                // be positioned the same within the parent.
                FilterSequenceEntity sequenceToInsert = new FilterSequenceEntity();
                parentFilter.ChildSequences.Insert(position, sequenceToInsert);
                sequenceToInsert.Filter = filter;
                sequenceToInsert.Position = position;

                // If we are moving, we will have soft-links to delete.
                FilterSequenceEntity softLinkToDelete = null;
                if (move)
                {
                    softLinkToDelete = filterNode.FilterSequence;
                }

                // Must be added to every node that represents the parent.
                foreach (FilterNodeEntity parentLink in FilterHelper.GetNodesUsingFilter(parentFilter))
                {
                    FilterNodeEntity childNode;

                    // If we are moving, and this particular parent link is the one that was actually requested to be moved to,
                    // than this is the one where we do the real move.  Any other links just get copies.
                    if (move && parentLink == parentNode)
                    {
                        filterNode.FilterSequence = null;
                        filterNode.ParentNode = null;

                        filterNode.FilterSequence = sequenceToInsert;

                        childNode = filterNode;
                    }
                    else
                    {
                        FilterNodeEntity nodeToInsert = new FilterNodeEntity();
                        nodeToInsert.FilterSequence = sequenceToInsert;
                        nodeToInsert.Created = DateTime.UtcNow;
                        nodeToInsert.Purpose = (int) FilterNodePurpose.Standard;

                        childNode = nodeToInsert;
                    }

                    // Add it to the correct location in the parent
                    parentLink.ChildNodes.Insert(position, childNode);

                    // Add to the list we created
                    affectedNodes.Add(childNode);

                    // We also have to create child nodes for any children of the node being copied, if its a new (not being moved) node.
                    if (childNode.IsNew)
                    {
                        CreateSoftLinks(filterNode.ChildNodes, childNode, adapter);
                    }
                    // When moving, all the children need to have their count's updated
                    else
                    {
                        // Update all the children counts, since we are now in a different folder.
                        foreach (FilterNodeEntity descendantNode in GetChildFilterNodes(childNode))
                        {
                            descendantNode.FilterNodeContent = FilterContentManager.CreateNewFilterContent(new FilterContentCalculation(descendantNode), adapter);
                            adapter.SaveAndRefetch(descendantNode);
                        }
                    }

                    // The count has to be done after generating the children, since folders depend on child nodes in place
                    childNode.FilterNodeContent = FilterContentManager.CreateNewFilterContent(new FilterContentCalculation(childNode), adapter);

                    // Save the filter node and its sequence
                    adapter.SaveAndRefetch(childNode);

                    // NOTE: We used to copy grid settings from the existing node to the new node here, but we decided that its probably 50/50 what
                    // the user would even expect, so we just don't do it now.
                }

                // If we were moving the original node, there are likely soft-links that need to be deleted
                if (softLinkToDelete != null)
                {
                    DeleteLink(softLinkToDelete, adapter);
                }

                // Update the filter counts for all new ancestor folders
                foreach (FilterNodeEntity anscestor in GetAncestorsAffectedByDefinition(filter))
                {
                    anscestor.FilterNodeContent = FilterContentManager.CreateNewFilterContent(new FilterContentCalculation(anscestor), adapter);
                    adapter.SaveAndRefetch(anscestor);
                }

                // Update the filter counts for all previous anscestor folders for moves
                if (preMoveAncestors != null)
                {
                    foreach (FilterNodeEntity anscestor in preMoveAncestors)
                    {
                        anscestor.FilterNodeContent = FilterContentManager.CreateNewFilterContent(new FilterContentCalculation(anscestor), adapter);
                        adapter.SaveAndRefetch(anscestor);
                    }
                }

                return affectedNodes;
            }
            catch (Exception ex)
            {
                FilterHelper.TranslateException(ex);

                // Rethrow, if the translate didnt
                throw;
            }
        }

        /// <summary>
        /// Make a copy of the selected filter as a child of the specified filter
        /// </summary>
        public List<FilterNodeEntity> Copy(FilterEntity filter, FilterNodeEntity parentNode, int position)
        {
            if (filter.IsFolder)
            {
                throw new InvalidOperationException("Cannot copy a folder.");
            }

            // First we need the copy
            FilterEntity copy = new FilterEntity();
            copy.Name = GetCopyName(filter);
            copy.FilterTarget = filter.FilterTarget;
            copy.IsFolder = false;
            copy.Definition = filter.Definition;
            copy.State = filter.State;

            // Every node has at least one sequence describing its position
            FilterSequenceEntity sequence = new FilterSequenceEntity();
            sequence.Filter = copy;

            // Now we need a new FilterNode to represent it in the hiearchy.  We may need more than one - we'll need one for
            // each link its parent has.  But AddNodeToParent will take care of that.
            FilterNodeEntity filterNode = new FilterNodeEntity();
            filterNode.FilterSequence = sequence;

            // Since we don't exist anywhere yet, this doesnt actually add a link, its the first one
            return AddNodeToParent(filterNode, parentNode, position);
        }

        /// <summary>
        /// Get a unqiue name we can use as the name of a copy of this filter
        /// </summary>
        private string GetCopyName(FilterEntity filter)
        {
            string name = filter.Name + " (Copy)";

            if (FilterHelper.IsValidName(filter, name))
            {
                return name;
            }

            string format = filter.Name + " (Copy{0})";
            int i = 1;

            while (true)
            {
                name = string.Format(format, i);

                if (FilterHelper.IsValidName(filter, name))
                {
                    return name;
                }

                i++;
            }
        }

        /// <summary>
        /// Delete the filter and all its children in every location that its linked.
        /// </summary>
        public void DeleteFilter(FilterEntity filter)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // Each sequence using the filter represents a hard link.  Soft-links are taken care of
                // by the DeleteLink function.
                foreach (FilterSequenceEntity sequence in FilterHelper.GetSequencesUsingFilter(filter))
                {
                    // The sequence may be being used by more than one node.  Those are soft-links.  All
                    // soft-links are deleted automatically, so we can just pick one.
                    DeleteLink(sequence, adapter);
                }

                FilterHelper.ValidateFilterLayouts(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Delete the node and all its children.  If the node is linked, the other linked nodes are left alone.
        /// </summary>
        public void DeleteLink(FilterNodeEntity node)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                DeleteLink(node.FilterSequence, adapter);

                FilterHelper.ValidateFilterLayouts(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Delete the specified link using the given adapter
        /// </summary>
        [NDependIgnoreLongMethod]
        private void DeleteLink(FilterSequenceEntity sequence, SqlAdapter adapter)
        {
            FilterEntity filter = sequence.Filter;

            // We may have to delete the filter when we are done if this is the last sequence using the filter
            bool deleteFilter = filter.UsedBySequences.Count == 1;

            // Get the position of the sequence we are deleting
            int deletedPosition = sequence.Position;

            // Make a list of all the sequences siblings
            EntityCollection<FilterSequenceEntity> siblings = new EntityCollection<FilterSequenceEntity>();
            foreach (FilterSequenceEntity siblingSequence in sequence.Parent.ChildSequences)
            {
                siblings.Add(siblingSequence);
            }

            try
            {
                List<FilterNodeEntity> ancestorsToRecount = new List<FilterNodeEntity>();

                // We have to delete all soft links - which are all nodes using this same sequence
                foreach (FilterNodeEntity nodeToDelete in new List<FilterNodeEntity>(sequence.NodesUsingSequence))
                {
                    ancestorsToRecount.AddRange(GetAncestorsAffectedByDefinition(nodeToDelete));

                    // First delete all children, from bottom to top.
                    foreach (FilterNodeEntity childNode in new List<FilterNodeEntity>(nodeToDelete.ChildNodes))
                    {
                        DeleteChildLink(childNode, adapter);
                    }

                    nodeToDelete.FilterSequence = null;
                    nodeToDelete.ParentNode = null;
                    adapter.DeleteEntity(nodeToDelete);
                }

                // We have to move all the following siblings up
                for (int i = deletedPosition + 1; i < siblings.Count; i++)
                {
                    siblings[i].Position--;
                    adapter.SaveAndRefetch(siblings[i]);
                }

                // No more nodes refering to it, we can delete the sequence
                sequence.Filter = null;
                sequence.Parent = null;
                adapter.DeleteEntity(sequence);

                // If this was the last sequence refering to the filter, we can delete the filter
                if (deleteFilter)
                {
                    log.InfoFormat("Deleting filter {0}", filter.Name);
                    adapter.DeleteEntity(filter);
                }

                // All ancestors will need to be recounted
                foreach (FilterNodeEntity ancestor in ancestorsToRecount)
                {
                    ancestor.FilterNodeContent = FilterContentManager.CreateNewFilterContent(new FilterContentCalculation(ancestor), adapter);
                    adapter.SaveAndRefetch(ancestor);
                }
            }
            catch (Exception ex)
            {
                FilterHelper.TranslateException(ex);

                // Rethrow, if the translate didnt
                throw;
            }
        }

        /// <summary>
        /// Delete a link that is a child of a ancestor that was the target for deletion.
        /// </summary>
        private void DeleteChildLink(FilterNodeEntity node, SqlAdapter adapter)
        {
            // First delete all children, from bottom to top.
            foreach (FilterNodeEntity childNode in new List<FilterNodeEntity>(node.ChildNodes))
            {
                DeleteChildLink(childNode, adapter);
            }

            FilterSequenceEntity sequence = node.FilterSequence;
            FilterEntity filter = sequence.Filter;

            // We may have to delete the filter when we are done if this is the last node using the filter.  That
            // will only be the case if this node is not linked at all.
            bool deleteFilter = FilterHelper.GetNodesUsingFilter(filter).Count == 1;

            // We may have to delete the sequence if this is the last node using the sequence
            bool deleteSequence = sequence.NodesUsingSequence.Count == 1;

            node.FilterSequence = null;
            node.ParentNode = null;
            adapter.DeleteEntity(node);

            if (deleteSequence)
            {
                // No more nodes refering to it, we can delete the sequence
                sequence.Filter = null;
                sequence.Parent = null;
                adapter.DeleteEntity(sequence);
            }

            // If this was the last sequence refering to the filter, we can delete the filter
            if (deleteFilter)
            {
                log.InfoFormat("Deleting filter {0}", filter.Name);
                adapter.DeleteEntity(filter);
            }
        }

        /// <summary>
        /// Move the given node to the new parent
        /// </summary>
        public List<FilterNodeEntity> Move(FilterNodeEntity node, FilterNodeEntity newParent, int position)
        {
            List<FilterNodeEntity> affected = new List<FilterNodeEntity>();

            // Already there, just change positions
            if (node.ParentNode == newParent)
            {
                affected.Add(node);

                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    while (position > node.FilterSequence.Position && CanMoveDown(node))
                    {
                        MoveDown(node, adapter);
                    }

                    while (position < node.FilterSequence.Position && CanMoveUp(node))
                    {
                        MoveUp(node, adapter);
                    }

                    FilterHelper.ValidateFilterLayouts(adapter);
                    adapter.Commit();
                }
            }

            // Changing parents
            else
            {
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    // Add it to the new parent
                    affected = SetNodeToParent(node, newParent, position, true, adapter);

                    FilterHelper.ValidateFilterLayouts(adapter);
                    adapter.Commit();
                }
            }

            return affected;
        }

        /// <summary>
        /// Can the given node be moved down amongst its siblings
        /// </summary>
        public bool CanMoveDown(FilterNodeEntity node)
        {
            return !FilterHelper.IsBuiltin(node) && node.FilterSequence.Position + 1 < node.ParentNode.ChildNodes.Count;
        }

        /// <summary>
        /// Can the given node be moved up amongst its siblings
        /// </summary>
        public bool CanMoveUp(FilterNodeEntity node)
        {
            return !FilterHelper.IsBuiltin(node) && node.FilterSequence.Position > 0;
        }

        /// <summary>
        /// Move the filter node up amongst its siblings
        /// </summary>
        public void MoveUp(FilterNodeEntity node)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                MoveUp(node, adapter);

                FilterHelper.ValidateFilterLayouts(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Move the node up, using teh specified adapter
        /// </summary>
        private void MoveUp(FilterNodeEntity node, SqlAdapter adapter)
        {
            if (!CanMoveUp(node))
            {
                throw new FilterInvalidLocationException("The filter cannot be moved up.  It is the first sibling.");
            }

            // Get the sequence and the sibling above it
            FilterSequenceEntity sequence = node.FilterSequence;
            FilterSequenceEntity sibling = node.ParentNode.ChildNodes[sequence.Position - 1].FilterSequence;

            // Move them
            sibling.Position++;
            sequence.Position--;

            try
            {
                adapter.SaveAndRefetch(sequence);
                adapter.SaveAndRefetch(sibling);

                // Need to rearrange the child nodes into correct position
                foreach (FilterNodeEntity movedNode in FilterHelper.GetNodesUsingSequence(sequence))
                {
                    EntityCollection<FilterNodeEntity> nodes = movedNode.ParentNode.ChildNodes;
                    nodes.Remove(movedNode);
                    nodes.Insert(movedNode.FilterSequence.Position, movedNode);
                }
            }
            catch (Exception ex)
            {
                FilterHelper.TranslateException(ex);

                // Rethrow, if the translate didnt
                throw;
            }
        }

        /// <summary>
        /// Move the filter node down amongst its siblings
        /// </summary>
        public void MoveDown(FilterNodeEntity node)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                MoveDown(node, adapter);

                FilterHelper.ValidateFilterLayouts(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Move the filter down amongst its siblings, using the specified adapter
        /// </summary>
        private void MoveDown(FilterNodeEntity node, SqlAdapter adapter)
        {
            if (!CanMoveDown(node))
            {
                throw new FilterInvalidLocationException("The filter cannot be moved down.  It is the last sibling.");
            }

            // Get the sequence and the sibling below it
            FilterSequenceEntity sequence = node.FilterSequence;
            FilterSequenceEntity sibling = node.ParentNode.ChildNodes[sequence.Position + 1].FilterSequence;

            // Move them
            sibling.Position--;
            sequence.Position++;

            try
            {
                // Save
                adapter.SaveAndRefetch(sequence);
                adapter.SaveAndRefetch(sibling);

                // Need to rearrange the child nodes into correct position
                foreach (FilterNodeEntity movedNode in FilterHelper.GetNodesUsingSequence(sequence))
                {
                    EntityCollection<FilterNodeEntity> nodes = movedNode.ParentNode.ChildNodes;
                    nodes.Remove(movedNode);
                    nodes.Insert(movedNode.FilterSequence.Position, movedNode);
                }
            }
            catch (Exception ex)
            {
                FilterHelper.TranslateException(ex);

                // Rethrow, if the translate didnt
                throw;
            }
        }

        /// <summary>
        /// Indicates if the given filter can be a child of the specified filter.
        /// </summary>
        public bool CanAddChild(FilterEntity child, FilterNodeEntity parentNode, out string reason)
        {
            FilterEntity parent = parentNode.Filter;

            reason = null;

            if (!parent.IsFolder)
            {
                reason = string.Format("A {0} cannot be the child of a filter.", child.IsFolder ? "folder" : "filter");
                return false;
            }

            if (child == parent)
            {
                reason = string.Format("A {0} cannot be added as a child of itself.", child.IsFolder ? "folder" : "filter");
                return false;
            }

            // Go through all the child sequences of the parent - if it already contains the child, thats not ok
            foreach (FilterSequenceEntity existingSequence in parent.ChildSequences)
            {
                // This means the child is already a child of the parent
                if (existingSequence.Filter == child)
                {
                    reason = string.Format("A {0} cannot be added to a folder it is already in.", child.IsFolder ? "folder" : "filter");
                    return false;
                }
            }

            // If the parent has the child for its anscestor at some point, then the child cant really be a child
            if (HasAncestor(parent, child))
            {
                reason = string.Format("A {0} cannot be added as a descendant of itself.", child.IsFolder ? "folder" : "filter");
                return false;
            }

            // Edition restriction check on My Filters
            EditionRestrictionIssue myFiltersRestriction = EditionManager.ActiveRestrictions.CheckRestriction(EditionFeature.MyFilters);
            if (myFiltersRestriction.Level != EditionRestrictionLevel.None && FilterHelper.IsMyFilter(parentNode))
            {
                reason = myFiltersRestriction.GetDescription();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Recursively sort the children of the given filter
        /// </summary>
        public void Sort(FilterEntity filter)
        {
            try
            {
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    Sort(filter, adapter);

                    FilterHelper.ValidateFilterLayouts(adapter);
                    adapter.Commit();
                }
            }
            catch (Exception ex)
            {
                FilterHelper.TranslateException(ex);

                throw;
            }
        }

        /// <summary>
        /// Recursively sort the children of the given filter using the given adapter
        /// </summary>
        private void Sort(FilterEntity filter, SqlAdapter adapter)
        {
            // First sort them in memory
            filter.ChildSequences.Sort("FilterName", ListSortDirection.Ascending, null);

            // Have to make our own copy
            List<FilterSequenceEntity> childSequences = new List<FilterSequenceEntity>(filter.ChildSequences);

            // Then update their positions
            for (int i = 0; i < childSequences.Count; i++)
            {
                FilterSequenceEntity sequence = childSequences[i];
                sequence.Position = i;
                adapter.SaveAndRefetch(sequence);

                // Have to update all the node positions
                foreach (FilterNodeEntity node in new List<FilterNodeEntity>(sequence.NodesUsingSequence))
                {
                    FilterNodeEntity parent = node.ParentNode;
                    parent.ChildNodes.Remove(node);
                    parent.ChildNodes.Insert(i, node);
                }
            }

            // Now do the child folders
            foreach (FilterSequenceEntity sequence in filter.ChildSequences)
            {
                if (sequence.Filter.IsFolder)
                {
                    Sort(sequence.Filter, adapter);
                }
            }
        }

        #endregion

        #region Utility

        /// <summary>
        /// Determines if the given child has the given filter as an ancestor
        /// </summary>
        private bool HasAncestor(FilterEntity child, FilterEntity ancestor)
        {
            // Check all of the potential ancestores immediate children
            foreach (FilterSequenceEntity childSequence in ancestor.ChildSequences)
            {
                // The potential anscestor has this child as an immediate child.
                if (childSequence.Filter == child)
                {
                    return true;
                }

                // Check if this sequence would be an ancestor
                if (HasAncestor(child, childSequence.Filter))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Create "soft" links between the existing nodes, linking them into the specified parent.
        /// </summary>
        private void CreateSoftLinks(EntityCollection<FilterNodeEntity> existingNodes, FilterNodeEntity linkedParentNode, SqlAdapter adapter)
        {
            foreach (FilterNodeEntity node in existingNodes)
            {
                FilterNodeEntity linkedNode = new FilterNodeEntity();
                linkedNode.ParentNode = linkedParentNode;
                linkedNode.FilterSequence = node.FilterSequence;
                linkedNode.Created = DateTime.UtcNow;
                linkedNode.Purpose = (int) FilterNodePurpose.Standard;

                CreateSoftLinks(node.ChildNodes, linkedNode, adapter);

                // The count has to be done after generating the children, since folders depend on child nodes in place
                linkedNode.FilterNodeContent = FilterContentManager.CreateNewFilterContent(new FilterContentCalculation(linkedNode), adapter);
            }
        }

        #endregion
    }
}
