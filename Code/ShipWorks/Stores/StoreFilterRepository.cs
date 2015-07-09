using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Orders;

namespace ShipWorks.Stores
{
    public class StoreFilterRepository
    {
        private readonly StoreEntity store;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreFilterRepository"/> class.
        /// </summary>
        /// <param name="store">The store filters are being created for.</param>
        public StoreFilterRepository(StoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Uses the store provided in the constructor to save the store filters to the database.
        /// </summary>
        /// <param name="abortOnFolderNameCollision">if set to <c>true</c> [abort on folder name collision].</param>
        /// <returns>StoreFilterRepositorySaveResult.</returns>
        public StoreFilterRepositorySaveResult Save(bool abortOnFolderNameCollision)
        {
            FilterEntity storeFilterFolder = CreateStoreFilterFolder();

            StoreFilterRepositorySaveResult result = new StoreFilterRepositorySaveResult
            {
                FolderCreated = false, 
                StoreFolderName = storeFilterFolder.Name
            };

            // See if the store has needs to create any store-specific filters
            List<FilterEntity> storeFilters = StoreTypeManager.GetType(store).CreateInitialFilters();

            // Find the root orders node
            FilterNodeEntity ordersNode = FilterLayoutContext.Current.FindNode(BuiltinFilter.GetTopLevelKey(FilterTarget.Orders));

            result.StoreFolderName = storeFilterFolder.Name;

            // If a folder has the same name and abortOnFolderNameCollision is on, abort
            bool hasFolderWithStoreName = HasChildNodeWithName(ordersNode, storeFilterFolder.Name, true);
            if (hasFolderWithStoreName && abortOnFolderNameCollision)
            {
                return result;
            }

            List<FilterNodeEntity> foldersWithMatchingNameAndStoreCondition = FindMatchingChildNodesWithMatchingStoreCondition(ordersNode, storeFilterFolder.Name, true);

            // If there are folders with the same name, but none of them have a matching store condition, this is a collision that cannot be resolved from
            if (hasFolderWithStoreName && !foldersWithMatchingNameAndStoreCondition.Any())
            {
                return result;
            }

            // If there is a match, use this as the store node, else create a new store node
            FilterNodeEntity storeNode = foldersWithMatchingNameAndStoreCondition.Any() ?
                foldersWithMatchingNameAndStoreCondition.First() : 
                FilterLayoutContext.Current.AddFilter(storeFilterFolder, ordersNode, 0)[0];
            
            // Always create an "All Orders" filter so that the folder count show's the full store orders's count.  Otherwise our users would
            // likely be confused.
            FilterEntity allOrders = CreateStoreFilterAllOrders();
            AddUniquelyNamedFilter(storeNode, allOrders, result, 0);
            result.FolderCreated = true;

            if (storeFilters != null && storeFilters.Count > 0)
            {
                // Create each filter under the store node
                for (int i = 0; i < storeFilters.Count; i++)
                {
                    AddUniquelyNamedFilter(storeNode, storeFilters[i], result, i + 1);
                }
            }

            return result;
        }

        /// <summary>
        /// Adds the uniquely named filter and updates the result with the result of this call.
        /// </summary>
        private static void AddUniquelyNamedFilter(FilterNodeEntity folder, FilterEntity filter, StoreFilterRepositorySaveResult result, int position)
        {
            if (!HasChildNodeWithName(folder, filter.Name, false))
            {
                result.CreatedFilters.Add(filter);
                FilterLayoutContext.Current.AddFilter(filter, folder, position);
            }
            else
            {
                result.CollisionFilters.Add(filter);
            }
        }

        /// <summary>
        /// Determines whether the filter has children that match the name and folder criteria
        /// </summary>
        private static bool HasChildNodeWithName(FilterNodeEntity parentFilterNode, string nameToFind, bool isFolder)
        {
            return parentFilterNode.ChildNodes.Any(n => n.Filter.Name == nameToFind && n.Filter.IsFolder == isFolder);
        }

        /// <summary>
        /// Finds the matching child nodes with matching store condition.
        /// </summary>
        private List<FilterNodeEntity> FindMatchingChildNodesWithMatchingStoreCondition(FilterNodeEntity parentFilterNode, string nameToFind, bool isFolder)
        {
            List<FilterNodeEntity> matchingChildren = new List<FilterNodeEntity>();

            if (parentFilterNode.ChildNodes == null)
            {
                return new List<FilterNodeEntity>();
            }

            foreach (FilterNodeEntity node in parentFilterNode.ChildNodes)
            {
                // The name must match
                if (node.Filter.Name != nameToFind)
                {
                    continue;
                }

                // The node's folder property must match
                if (node.Filter.IsFolder != isFolder)
                {
                    continue;
                }

                FilterDefinition filterDefinition = new FilterDefinition(node.Filter.Definition);

                // There is only one group
                if (filterDefinition.RootContainer.SecondGroup != null)
                {
                    continue;
                }

                // There is only one condition
                if (filterDefinition.RootContainer.FirstGroup.Conditions.Count() != 1)
                {
                    continue;
                }

                StoreCondition storeCondition = filterDefinition.RootContainer.FirstGroup.Conditions.Single() as StoreCondition;
                // The condition is a store condition
                if (storeCondition == null)
                {
                    continue;
                }
                
                // The condition is "Equal to this store"
                if (storeCondition.Value != store.StoreID || storeCondition.Operator != EqualityOperator.Equals)
                {
                    continue;
                }

                matchingChildren.Add(node);
            }

            return matchingChildren;
        }

        /// <summary>
        /// Create a filter folder entity that has a definition that filters for orders only in the current store
        /// </summary>
        private FilterEntity CreateStoreFilterFolder()
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
            definition.RootContainer.FirstGroup.Conditions.Add(new StoreCondition { Operator = EqualityOperator.Equals, Value = store.StoreID });

            FilterEntity folder = new FilterEntity();
            folder.Name = string.Format("{0} ({1})", StoreTypeManager.GetType(store).StoreTypeName, store.StoreName);
            folder.FilterTarget = (int)FilterTarget.Orders;
            folder.IsFolder = true;
            folder.Definition = definition.GetXml();

            return folder;
        }

        /// <summary>
        /// Create a filter that filter's for all orders of this store
        /// </summary>
        private FilterEntity CreateStoreFilterAllOrders()
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
            definition.RootContainer.FirstGroup.Conditions.Add(new StoreCondition { Operator = EqualityOperator.Equals, Value = store.StoreID });

            FilterEntity filter = new FilterEntity();
            filter.Name = "All Orders";
            filter.FilterTarget = (int)FilterTarget.Orders;
            filter.IsFolder = false;
            filter.Definition = definition.GetXml();

            return filter;
        }
    }
}
