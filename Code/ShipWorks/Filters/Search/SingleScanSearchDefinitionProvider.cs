using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Orders;

namespace ShipWorks.Filters.Search
{
    /// <summary>
    /// Provides the search definition used when single scan is enabled
    /// </summary>
    public class SingleScanSearchDefinitionProvider : ISearchDefinitionProvider
    {
        private readonly ISingleScanOrderShortcut singleScanShortcut;

        /// <summary>
        /// Constructor
        /// </summary>
        public SingleScanSearchDefinitionProvider(ISingleScanOrderShortcut singleScanShortcut)
        {
            this.singleScanShortcut = singleScanShortcut;
        }

        /// <summary>
        /// Gets a filter definition that searches for an exact order number or OrderId based on the prefix
        /// </summary>
        public IFilterDefinition GetDefinition(string quickSearchString)
        {
            Condition condition = GetCondition(quickSearchString);

            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders, FilterDefinitionSourceType.Scan);
            definition.RootContainer.FirstGroup.Conditions.Add(condition);

            return definition;
        }

        /// <summary>
        /// Gets the search condition
        /// </summary>
        private Condition GetCondition(string quickSearchString)
        {
            Condition condition;
            if (singleScanShortcut.AppliesTo(quickSearchString))
            {
                // The search string contains the OrderId
                condition = new OrderIDCondition
                {
                    Operator = NumericOperator.Equal,
                    Value1 = singleScanShortcut.GetOrderID(quickSearchString)
                };
            }
            else
            {
                condition = new SingleScanSearchCondition(quickSearchString);
            }

            return condition;
        }
    }
}