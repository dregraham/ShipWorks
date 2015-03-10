using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content;
using ShipWorks.Filters;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Orders.PersonName;
using ShipWorks.Filters.Content.Conditions.Customers.PersonName;
using Interapptive.Shared.Utility;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Filters.Content.Conditions.Customers;
using ShipWorks.Stores;
using ShipWorks.Filters.Content.Conditions.Special;
using Interapptive.Shared.Business;
using ShipWorks.Stores.Platforms;

namespace ShipWorks.Filters.Search
{
    /// <summary>
    /// Helps create FilterDefinitions for the basic search
    /// </summary>
    public static class QuickSearchCriteria
    {
        /// <summary>
        /// Create the basic search definition for the given search and target
        /// </summary>
        public static FilterDefinition CreateDefinition(FilterTarget target, string search)
        {
            FilterDefinition definition = new FilterDefinition(target);

            // Will hold any store-specific conditions
            List<ConditionGroup> storeSpecific = new List<ConditionGroup>();

            switch (target)
            {
                case FilterTarget.Customers:
                    
                    // Apply common customer conditions
                    ApplyCustomerConditions(definition.RootContainer.FirstGroup, search);

                    // Apply store-specific conditions
                    foreach (StoreType storeType in StoreManager.GetUniqueStoreTypes())
                    {
                        storeSpecific.Add(storeType.CreateBasicSearchCustomerConditions(search));
                    }

                    break;

                case FilterTarget.Orders:

                    // Apply common order conditions
                    ApplyOrderConditions(definition.RootContainer.FirstGroup, search);

                    // Apply store-specific conditions
                    foreach (StoreType storeType in StoreManager.GetUniqueStoreTypes())
                    {
                        storeSpecific.Add(storeType.CreateBasicSearchOrderConditions(search));
                    }

                    break;

                default:
                    throw new InvalidOperationException(string.Format("Invalid FilterTarget in BasicSearch {0}", target));
            }

            ConditionGroup parentGroup = null;

            // Get all the groups that are not null
            foreach (ConditionGroup group in storeSpecific.Where(g => g != null))
            {
                // Lazy create the container for them.
                if (parentGroup == null)
                {
                    parentGroup = new ConditionGroup();
                    parentGroup.JoinType = ConditionJoinType.Any;

                    definition.RootContainer.SecondGroup = new ConditionGroupContainer(parentGroup);
                    definition.RootContainer.JoinType = ConditionGroupJoinType.Or;
                }

                // Create a combined result to hold the group of conditions for this store
                CombinedResultCondition combinedResult = new CombinedResultCondition();
                combinedResult.Container.FirstGroup = group;

                // Add the combined result to the parent, which joins them all together with an Any
                parentGroup.Conditions.Add(combinedResult);
            }

            return definition;
        }

        /// <summary>
        /// Add all the basic search order conditions to the search
        /// </summary>
        private static void ApplyOrderConditions(ConditionGroup group, string search)
        {
            // The OrderNumber condition
            group.Conditions.Add(new OrderNumberCondition { IsNumeric = false, StringOperator = StringOperator.BeginsWith, StringValue = search });

            // Only add the address stuff if its not just a number
            long orderNumber;
            if (!long.TryParse(search, out orderNumber))
            {
                // Either the OrderNumber matches, or the Name matches
                group.JoinType = ConditionJoinType.Any;

                // The name stuff will have to be in a CombinedResult to keep its result as a single unit
                CombinedResultCondition combinedResult = new CombinedResultCondition();

                // Add the combined result to the group of conditions
                group.Conditions.Add(combinedResult);

                // Add the name conditions to the first group of the combined result
                ApplyNameConditions(combinedResult.Container.FirstGroup, search, typeof(OrderFirstNameCondition), typeof(OrderLastNameCondition), typeof(OrderEmailAddressCondition));
            }
        }

        /// <summary>
        /// Add all the basic search customer conditions to the search
        /// </summary>
        private static void ApplyCustomerConditions(ConditionGroup group, string search)
        {
            long customerID;
            if (long.TryParse(search, out customerID))
            {
                // Either the CustomerID matches, or the Name matches
                group.JoinType = ConditionJoinType.Any;

                // The CustomerID condition
                group.Conditions.Add(new CustomerIDCondition { Value1 = customerID, Operator = NumericOperator.Equal });

                // The name stuff will have to be in a CombinedResult to keep its result as a single unit
                CombinedResultCondition combinedResult = new CombinedResultCondition();

                // Add the combined result to the group of conditions
                group.Conditions.Add(combinedResult);

                // The combined result group is now what will actually be searched
                group = combinedResult.Container.FirstGroup;
            }

            ApplyNameConditions(group, search, typeof(CustomerFirstNameCondition), typeof(CustomerLastNameCondition), typeof(CustomerEmailAddressCondition));
        }

        /// <summary>
        /// Apply the conditions based on name
        /// </summary>
        private static void ApplyNameConditions(ConditionGroup group, string search, Type firstNameType, Type lastNameType, Type emailType)
        {
            // If there are no spaces, look for first and last individually
            if (!search.Contains(' '))
            {
                group.JoinType = ConditionJoinType.Any;

                group.Conditions.Add(CreateAddressCondition(firstNameType, BillShipAddressOperator.ShipOrBill, StringOperator.BeginsWith, search));
                group.Conditions.Add(CreateAddressCondition(lastNameType, BillShipAddressOperator.ShipOrBill, StringOperator.BeginsWith, search));

                group.Conditions.Add(CreateAddressCondition(emailType, BillShipAddressOperator.ShipOrBill, StringOperator.BeginsWith, search));
            }
            else
            {
                PersonName name = PersonName.Parse(search);

                group.JoinType = ConditionJoinType.All;

                group.Conditions.Add(CreateAddressCondition(firstNameType, BillShipAddressOperator.ShipOrBill, StringOperator.BeginsWith, name.First));
                group.Conditions.Add(CreateAddressCondition(lastNameType, BillShipAddressOperator.ShipOrBill, StringOperator.BeginsWith, name.Last));
            }
        }

        /// <summary>
        /// Create an AddressCondition of the specified type with the given properties
        /// </summary>
        private static Condition CreateAddressCondition(Type type, BillShipAddressOperator addressOperator, StringOperator stringOperator, string targetValue)
        {
            BillShipAddressStringCondition condition = (BillShipAddressStringCondition) Activator.CreateInstance(type);

            condition.AddressOperator = addressOperator;
            condition.Operator = stringOperator;
            condition.TargetValue = targetValue;

            return condition;
        }
    }
}
