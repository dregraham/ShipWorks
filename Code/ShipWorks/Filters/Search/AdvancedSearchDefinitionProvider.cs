using System.Collections.Generic;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;

namespace ShipWorks.Filters.Search
{
    /// <summary>
    /// Generate FilterDefinitions for when using the advanced search in ShipWorks
    /// </summary>
    public class AdvancedSearchDefinitionProvider : ISearchDefinitionProvider
    {
        private readonly FilterDefinition advancedFilterDefinition;
        private readonly ISearchDefinitionProvider quickSearchDefinitionProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public AdvancedSearchDefinitionProvider(FilterDefinition advancedFilterDefinition,
            ISearchDefinitionProvider quickSearchDefinitionProvider,
            FilterDefinitionSourceType filterDefinitionSourceType)
        {
            this.advancedFilterDefinition = new FilterDefinition(advancedFilterDefinition.GetXml(), filterDefinitionSourceType);
            this.quickSearchDefinitionProvider = quickSearchDefinitionProvider;
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

            FilterDefinition quickFilterDefinition = quickSearchDefinitionProvider.GetDefinition(quickSearchString);

            List<Condition> quickSearchConditions = new List<Condition>();
            // Grap all of the conditions from the quick filter
            if (quickFilterDefinition?.RootContainer?.FirstGroup?.Conditions != null)
            {
                quickSearchConditions.AddRange(quickFilterDefinition.RootContainer.FirstGroup.Conditions);
            }

            if (quickFilterDefinition?.RootContainer?.SecondGroup?.FirstGroup?.Conditions != null)
            {
                // Quick search stores customer conditions in the secondGroups firstGroup, there is no second group.
                //
                //                                           - FirstGroup (ConditionGroup)
                //                                          /
                // RootContainer (ConditionGroupContainer) -                                           - FirstGroup (ConditionGroup)
                //                                          \                                         /
                //                                           - SecondGroup (ConditionGroupContainer) -
                //                                                                                    \
                //                                                                                     - SecondGroup (ConditionGroupContainer)

                quickSearchConditions.AddRange(quickFilterDefinition.RootContainer.SecondGroup.FirstGroup.Conditions);
            }

            ConditionGroup quickSearchConditionGroup = new ConditionGroup();
            quickSearchConditions.ForEach(quickSearchConditionGroup.Conditions.Add);
            quickSearchConditionGroup.JoinType = ConditionJoinType.Any;

            FilterDefinition combinedFilter = new FilterDefinition(advancedFilterDefinition.FilterTarget, FilterDefinitionSourceType.Search)
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