﻿using ShipWorks.Filters.Content;
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
        private readonly ISingleScanOrderPrefix singleScanPrefix;

        /// <summary>
        /// Constructor
        /// </summary>
        public SingleScanSearchDefinitionProvider(ISingleScanOrderPrefix singleScanPrefix)
        {
            this.singleScanPrefix = singleScanPrefix;
        }

        /// <summary>
        /// Gets a filter definition that searches for an exact order number or OrderId based on the prefix
        /// </summary>
        public FilterDefinition GetDefinition(string quickSearchString)
        {
            NumericCondition<long> condition;
            if (singleScanPrefix.Contains(quickSearchString))
            {
                // The search string contains the OrderId
                condition = new OrderIDCondition
                {
                    Operator = NumericOperator.Equal,
                    Value1 = singleScanPrefix.GetOrderID(quickSearchString)
                };
            }
            else
            {
                // The search string must be an Order Number
                long value;
                if (long.TryParse(quickSearchString, out value))
                {
                    condition = new OrderNumberCondition
                    {
                        IsNumeric = true,
                        Operator = NumericOperator.Equal,
                        Value1 = value
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
            }

            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
            definition.RootContainer.FirstGroup.Conditions.Add(condition);

            return definition;
        }
    }
}