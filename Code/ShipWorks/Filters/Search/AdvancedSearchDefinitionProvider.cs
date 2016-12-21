using System.Collections.Generic;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;

namespace ShipWorks.Filters.Search
{
    /// <summary>
    /// Advanced Search Definition Provider
    /// </summary>
    public class AdvancedSearchDefinitionProvider : IFilterDefinitionProvider
    {
        private readonly FilterDefinition advancedFilterDefinition;
        private readonly IFilterDefinitionProvider quickFilterDefinitionProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public AdvancedSearchDefinitionProvider(FilterDefinition advancedFilterDefinition,
            IFilterDefinitionProvider quickFilterDefinitionProvider)
        {
            this.advancedFilterDefinition = new FilterDefinition(advancedFilterDefinition.GetXml());
            this.quickFilterDefinitionProvider = quickFilterDefinitionProvider;
        }

        /// <summary>
        /// Get the Advanced AND Quick search definition combined in a single FilterDefinition
        /// </summary>
        public FilterDefinition GetDefinition(string quickSearchString)
        {
            // If the filter definition is empty (no criteria has been selected) don't return
            // the definition. An empty definition returns all orders or customers, which is
            // irrelevant
            if (advancedFilterDefinition.IsEmpty())
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(quickSearchString))
            {
                return advancedFilterDefinition;
            }

            FilterDefinition quickFilterDefinition = quickFilterDefinitionProvider.GetDefinition(quickSearchString);

            List<Condition> quickSearchConditions = new List<Condition>();
            if (quickFilterDefinition?.RootContainer?.FirstGroup?.Conditions != null)
            {
                quickSearchConditions.AddRange(quickFilterDefinition.RootContainer.FirstGroup.Conditions);
            }

            // When quick searching the customer grid, there is no second group.
            if (quickFilterDefinition?.RootContainer?.SecondGroup?.FirstGroup?.Conditions != null)
            {
                quickSearchConditions.AddRange(quickFilterDefinition.RootContainer.SecondGroup.FirstGroup.Conditions);
            }

            ConditionGroup quickSearchConditionGroup = new ConditionGroup();
            quickSearchConditions.ForEach(quickSearchConditionGroup.Conditions.Add);
            quickSearchConditionGroup.JoinType = ConditionJoinType.Any;

            FilterDefinition combinedFilter = new FilterDefinition(advancedFilterDefinition.FilterTarget)
            {
                RootContainer =
                {
                    FirstGroup = quickSearchConditionGroup,
                    SecondGroup = advancedFilterDefinition.RootContainer,
                    JoinType = ConditionGroupJoinType.And
                }
            };

            return combinedFilter;
        }
    }
}