using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Orders;

namespace ShipWorks.Filters.Search
{
    /// <summary>
    /// Provides the search definition used when single scan is enabled
    /// </summary>
    /// <seealso cref="ShipWorks.Filters.Search.ISearchDefinitionProvider" />
    public class SingleScanSearchDefinitionProvider : ISearchDefinitionProvider
    {
        /// <summary>
        /// Gets a filter definition that searches for an exact order number
        /// </summary>
        public FilterDefinition GetDefinition(string quickSearchString)
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders, FilterDefinitionSourceType.Scan);

            OrderNumberCondition condition;
            long orderNumber;
            if (long.TryParse(quickSearchString, out orderNumber))
            {
                condition = new OrderNumberCondition
                {
                    IsNumeric = true,
                    Operator = NumericOperator.Equal,
                    Value1 = orderNumber
                };
            }
            else
            {
                condition = new OrderNumberCondition
                {
                    IsNumeric = false,
                    StringOperator = StringOperator.Equals,
                    StringValue = quickSearchString,
                };
            }
            
            definition.RootContainer.FirstGroup.Conditions.Add(condition);

            return definition;
        }
    }
}