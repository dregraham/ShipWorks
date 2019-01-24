using System.Collections.Generic;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions.QuickSearch;

namespace ShipWorks.Filters.Search
{
    /// <summary>
    /// Creates Order Filter Definitions for Quick Search
    /// </summary>
    public class OrderQuickSearchDefinitionProvider : ISearchDefinitionProvider
    {
        private readonly IEnumerable<IQuickSearchStoreSql> storeSqls;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderQuickSearchDefinitionProvider(IEnumerable<IQuickSearchStoreSql> storeSqls)
        {
            this.storeSqls = storeSqls;
        }

        /// <summary>
        /// Gets a filter definition based on the provided quick search string.
        /// </summary>
        public IFilterDefinition GetDefinition(string quickSearchString)
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders, FilterDefinitionSourceType.Search);
            definition.RootContainer.FirstGroup.JoinType = ConditionJoinType.Any;

            QuickSearchCondition quickSearchCondition = new QuickSearchCondition(quickSearchString, storeSqls);
            definition.RootContainer.FirstGroup.Conditions.Add(quickSearchCondition);

            return definition;
        }
    }
}
