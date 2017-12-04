using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions.QuickSearch;

namespace ShipWorks.Filters.Search
{
    /// <summary>
    /// Creates Order Filter Definitions for Quick Search
    /// </summary>
    public class OrderQuickSearchDefinitionProvider : ISearchDefinitionProvider
    {
        /// <summary>
        /// Gets a filter definition based on the provided quick search string.
        /// </summary>
        public FilterDefinition GetDefinition(string quickSearchString)
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders, FilterDefinitionSourceType.Search);
            definition.RootContainer.FirstGroup.JoinType = ConditionJoinType.Any;

            QuickSearchCondition quickSearchCondition = new QuickSearchCondition(quickSearchString);
            definition.RootContainer.FirstGroup.Conditions.Add(quickSearchCondition);

            return definition;
        }
    }
}
