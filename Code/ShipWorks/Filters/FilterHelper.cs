using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Transactions;
using Interapptive.Shared.Data;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Editions;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Orders.Address;
using ShipWorks.Filters.Management;
using ShipWorks.Properties;
using ShipWorks.Users;

namespace ShipWorks.Filters
{
    /// <summary>
    /// Utility class for filters
    /// </summary>
    public static class FilterHelper
    {
        static readonly ILog log = LogManager.GetLogger(typeof(FilterHelper));

        const string myFiltersName = "My Filters";

        static readonly Dictionary<FilterImageType, Image> filterImageCache = new Dictionary<FilterImageType, Image>();

        enum FilterImageType
        {
            MyFilters,
            Orders,
            Customers,
            Search,
            HardLinkFolder,
            HardLinkFodlerWithCondition,
            HardLinkFilter,
            SoftLinkFolder,
            SoftLinkFodlerWithCondition,
            SoftLinkFilter,
            Folder,
            FolderWithCondition,
            Filter
        }

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

        /// <summary>
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
            filter.State = (int) FilterState.Enabled;

            FilterSequenceEntity sequence = new FilterSequenceEntity();
            sequence.Parent = null;
            sequence.Filter = filter;
            sequence.Position = 0;

            FilterNodeContentEntity content = new FilterNodeContentEntity();
            content.Status = (int) FilterCountStatus.Ready;
            content.InitialCalculation = "";
            content.UpdateCalculation = "";
            content.EntityExistsQuery = "";
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
            // When copying, we add " (Copy)" to the filter name which can push the filter's name over the max length.
            // Check here and throw if the requested name is too long.
            if (name.Length > filter.Fields[(int) FilterFieldIndex.Name].MaxLength)
            {
                throw new FilterException(
                    string.Format(
                        "Filter names may only be {0} characters or less.  Please shorten the requested filter name '{1}'.",
                        filter.Fields[(int) FilterFieldIndex.Name].MaxLength, name));
            }

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
        public static Image GetFilterImage(FilterNodeEntity node, bool createCopy = true)
        {
            FilterEntity filter = node.Filter;

            if (IsBuiltin(filter))
            {
                if (filter.Name == myFiltersName)
                {
                    return EditionManager.ActiveRestrictions.CheckRestriction(EditionFeature.MyFilters).Level == EditionRestrictionLevel.None ?
                        GetFilterImage(FilterImageType.MyFilters, createCopy) :
                        EditionGuiHelper.GetLockImage(16);
                }

                switch ((FilterTarget) filter.FilterTarget)
                {
                    case FilterTarget.Orders: return GetFilterImage(FilterImageType.Orders, createCopy);
                    case FilterTarget.Customers: return GetFilterImage(FilterImageType.Customers, createCopy);
                    default:
                        throw new InvalidOperationException("Unhandled FilterTarget in GetFilterImage");
                }
            }

            if (node.Purpose == (int) FilterNodePurpose.Search)
            {
                return GetFilterImage(FilterImageType.Search, createCopy);
            }

            if (IsFilterHardLinked(filter))
            {
                if (filter.IsFolder)
                {
                    return filter.Definition == null ? GetFilterImage(FilterImageType.HardLinkFolder, createCopy) : GetFilterImage(FilterImageType.HardLinkFodlerWithCondition, createCopy);
                }
                else
                {
                    return GetFilterImage(FilterImageType.HardLinkFilter, createCopy);
                }
            }
            else if (IsFilterSoftLinked(filter))
            {
                if (filter.IsFolder)
                {
                    return filter.Definition == null ? GetFilterImage(FilterImageType.SoftLinkFolder, createCopy) : GetFilterImage(FilterImageType.SoftLinkFodlerWithCondition, createCopy);
                }
                else
                {
                    return GetFilterImage(FilterImageType.SoftLinkFilter, createCopy);
                }
            }
            else
            {
                if (filter.IsFolder)
                {
                    return filter.Definition == null ? GetFilterImage(FilterImageType.Folder, createCopy) : GetFilterImage(FilterImageType.FolderWithCondition, createCopy);
                }
                else
                {
                    return GetFilterImage(FilterImageType.Filter, createCopy);
                }
            }
        }

        /// <summary>
        /// Get the filter image of the given type, either from our a cache of reusable images or creating a new copy
        /// </summary>
        private static Image GetFilterImage(FilterImageType filterImageType, bool createCopy)
        {
            if (createCopy)
            {
                switch (filterImageType)
                {
                    case FilterImageType.MyFilters: return Resources.my_filters;
                    case FilterImageType.Orders: return Resources.order16;
                    case FilterImageType.Customers: return Resources.customer16;
                    case FilterImageType.Search: return Resources.view;
                    case FilterImageType.HardLinkFolder: return Resources.folderclosed_linked_infinity;
                    case FilterImageType.HardLinkFodlerWithCondition: return Resources.folderfilter_linked_infinity;
                    case FilterImageType.HardLinkFilter: return Resources.funnel_linked_infinity;
                    case FilterImageType.SoftLinkFolder: return Resources.folderclosed_linked_arrow;
                    case FilterImageType.SoftLinkFodlerWithCondition: return Resources.folderfilter_linked_arrow;
                    case FilterImageType.SoftLinkFilter: return Resources.funnel_linked_arrow;
                    case FilterImageType.Folder: return Resources.folderclosed;
                    case FilterImageType.FolderWithCondition: return Resources.folderfilter;
                    case FilterImageType.Filter: return Resources.filter;
                }

                throw new NotFoundException("Could not find filter image for " + filterImageType);
            }
            else
            {
                Image image;
                if (!filterImageCache.TryGetValue(filterImageType, out image))
                {
                    image = GetFilterImage(filterImageType, true);
                    filterImageCache[filterImageType] = image;
                }

                return image;
            }
        }

        /// <summary>
        /// Get the image to use for the given type
        /// </summary>
        public static Image GetFilterImage(FilterTarget target)
        {
            return EntityUtility.GetEntityImage(GetEntityType(target));
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
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
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
        /// Get a list of template IDs that apply to an entity ID, based on template/filter rules.
        /// </summary>
        private static bool DoesFilterNodeApplyToEntity(long entityID, long filterNodeID)
        {
            List<long> results = new List<long>();

            using (new LoggedStopwatch(log, $"FilterHelper.DoesFilterNodeApplyToEntity Running: {entityID}, {filterNodeID}"))
            {
                using (DbConnection conn = SqlSession.Current.OpenConnection())
                {
                    using (DbCommand command = DbCommandProvider.Create(conn))
                    {
                        command.CommandText = "DoesFilterNodeApplyToEntity";
                        command.CommandType = CommandType.StoredProcedure;
                        command.AddParameterWithValue("@entityID", entityID);
                        command.AddParameterWithValue("@filterNodeID", filterNodeID);

                        using (DbDataReader reader = DbCommandProvider.ExecuteReader(command))
                        {
                            while (reader.Read())
                            {
                                results.Add(reader.GetInt64(0));
                            }
                        }
                    }
                }
            }

            return results.Contains(filterNodeID);
        }

        /// <summary>
        /// Indicates if the given object is in the filter contents of the specified filter node id
        /// </summary>
        public static bool IsObjectInFilterContent(long objectID, long filterNodeID)
        {
            // If its a top-level, like "Orders", assume the object is in it
            if (BuiltinFilter.IsTopLevelKey(filterNodeID))
            {
                return true;
            }

            if (EntityUtility.GetEntitySeed(filterNodeID) != 7)
            {
                throw new InvalidOperationException($"{filterNodeID} is not a valid FilterNodeID");
            }

            // If we are within a transaction right now, we don't want the FilterNode table locked into that
            // transaction.  We may block if filter are being updated, but at least we won't cause blocking.
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
            {
                return DoesFilterNodeApplyToEntity(objectID, filterNodeID);
            }
        }

        /// <summary>
        /// Creates filter for address validation statuses.
        /// </summary>
        public static FilterDefinition CreateAddressValidationDefinition(IEnumerable<AddressValidationStatusType> statusesToInclude)
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);

            // If [Any]
            definition.RootContainer.FirstGroup.JoinType = ConditionJoinType.Any;

            AddressValidationStatusCondition statusCondition = new OrderAddressValidationStatusCondition()
            {
                Operator = EqualityOperator.Equals,
                StatusTypes = statusesToInclude.ToList(),
                AddressOperator = BillShipAddressOperator.Ship
            };

            definition.RootContainer.FirstGroup.Conditions.Add(statusCondition);

            return definition;
        }

        /// <summary>
        /// Create a FilterEntity object of the given name from the specified definition
        /// </summary>
        public static FilterEntity CreateFilterEntity(string name, FilterDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException("definition");
            }

            FilterEntity filter = new FilterEntity();
            filter.Name = name;
            filter.FilterTarget = (int) definition.FilterTarget;
            filter.IsFolder = false;
            filter.Definition = definition.GetXml();
            filter.State = (int) FilterState.Enabled;

            return filter;
        }

        /// <summary>
        /// Create a FilterEntity object of the given name with no definition that represents a folder
        /// </summary>
        public static FilterEntity CreateFilterFolderEntity(string name, FilterTarget target)
        {
            FilterEntity folder = new FilterEntity();
            folder.Name = name;
            folder.FilterTarget = (int) target;
            folder.IsFolder = true;
            folder.Definition = null;
            folder.State = (int) FilterState.Enabled;

            return folder;
        }

    }
}
