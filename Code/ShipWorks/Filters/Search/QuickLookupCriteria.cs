using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Customers;
using ShipWorks.Filters.Content.Conditions.Orders;

namespace ShipWorks.Filters.Search
{
    /// <summary>
    /// Loads the search criteria for quick-lookups
    /// </summary>
    public static class QuickLookupCriteria
    {
        /// <summary>
        /// Create the definition needed to lookup the given order's customer.
        /// </summary>
        public static FilterDefinition CreateCustomerLookupDefinition(long customerID)
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Customers);
            ConditionGroup group = definition.RootContainer.FirstGroup;

            group.Conditions.Add(new CustomerIDCondition { Operator = NumericOperator.Equal, Value1 = customerID });

            return definition;
        }

        /// <summary>
        /// Create the definition needed to lookup the given customer's orders.
        /// </summary>
        public static FilterDefinition CreateOrderLookupDefinition(CustomerEntity customer)
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
            ConditionGroup group = definition.RootContainer.FirstGroup;

            CustomerCondition customerCondition = new CustomerCondition();
            customerCondition.Container.FirstGroup.Conditions.Add(
                new CustomerIDCondition { Operator = NumericOperator.Equal, Value1 = customer.CustomerID });

            group.Conditions.Add(customerCondition);

            return definition;
        }

        /// <summary>
        /// Create the definition needed to lookup the specific order
        /// </summary>
        public static FilterDefinition CreateOrderLookupDefinition(long orderID)
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
            ConditionGroup group = definition.RootContainer.FirstGroup;

            group.Conditions.Add(new OrderIDCondition { Operator = NumericOperator.Equal, Value1 = orderID });

            return definition;
        }

        /// <summary>
        /// Create the definition needed to lookup the specific orders
        /// </summary>
        public static FilterDefinition CreateOrderLookupDefinition(IEnumerable<long> orderIDs)
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
            ConditionGroup group = definition.RootContainer.FirstGroup;
            group.JoinType = ConditionJoinType.Any;

            foreach (long orderID in orderIDs)
            {
                group.Conditions.Add(new OrderIDCondition { Operator = NumericOperator.Equal, Value1 = orderID });
            }

            return definition;
        }
    }
}
