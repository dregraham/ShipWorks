using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Filters.Content.Conditions.Orders.PersonName;
using ShipWorks.Filters.Content.Conditions.Special;
using ShipWorks.Stores;

namespace ShipWorks.Filters.Search
{
    /// <summary>
    /// Creates Order Filter Definitions for Quick Search
    /// </summary>
    public class OrderQuickSearchDefinitionProvider : ISearchDefinitionProvider
    {
        private readonly IStoreManager storeManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderQuickSearchDefinitionProvider"/> class.
        /// </summary>
        /// <param name="storeManager">The store manager.</param>
        public OrderQuickSearchDefinitionProvider(IStoreManager storeManager)
        {
            this.storeManager = storeManager;
        }

        /// <summary>
        /// Gets a filter definition based on the provided quick search string.
        /// </summary>
        public FilterDefinition GetDefinition(string quickSearchString)
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders, FilterDefinitionSourceType.Search);

            // Apply common order conditions
            ApplyOrderConditions(definition.RootContainer.FirstGroup, quickSearchString);

            ApplyStoreSpecificCriteria(definition.RootContainer, quickSearchString);

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
                ApplyNameConditions(combinedResult.Container.FirstGroup, search);
            }
        }

        /// <summary>
        /// Apply the conditions based on name
        /// </summary>
        private static void ApplyNameConditions(ConditionGroup group, string search)
        {
            // If there are no spaces, look for first and last individually
            if (!search.Contains(' '))
            {
                group.JoinType = ConditionJoinType.Any;

                group.Conditions.Add(CreateAddressCondition(typeof(OrderFirstNameCondition), BillShipAddressOperator.ShipOrBill, StringOperator.BeginsWith, search));
                group.Conditions.Add(CreateAddressCondition(typeof(OrderLastNameCondition), BillShipAddressOperator.ShipOrBill, StringOperator.BeginsWith, search));

                group.Conditions.Add(CreateAddressCondition(typeof(OrderEmailAddressCondition), BillShipAddressOperator.ShipOrBill, StringOperator.BeginsWith, search));
            }
            else
            {
                PersonName name = PersonName.Parse(search);

                group.JoinType = ConditionJoinType.All;

                group.Conditions.Add(CreateAddressCondition(typeof(OrderFirstNameCondition), BillShipAddressOperator.ShipOrBill, StringOperator.BeginsWith, name.First));
                group.Conditions.Add(CreateAddressCondition(typeof(OrderLastNameCondition), BillShipAddressOperator.ShipOrBill, StringOperator.BeginsWith, name.Last));
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

        /// <summary>
        /// Merges the store criteria.
        /// </summary>
        private void ApplyStoreSpecificCriteria(ConditionGroupContainer rootContainer, string quickSearchString)
        {
            // Will hold any store-specific conditions
            List<ConditionGroup> storeSpecific = new List<ConditionGroup>();

            // Apply store-specific conditions
            foreach (StoreType storeType in storeManager.GetUniqueStoreTypes())
            {
                storeSpecific.Add(storeType.CreateBasicSearchOrderConditions(quickSearchString));
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

                    rootContainer.SecondGroup = new ConditionGroupContainer(parentGroup);
                    rootContainer.JoinType = ConditionGroupJoinType.Or;
                }

                // Create a combined result to hold the group of conditions for this store
                CombinedResultCondition combinedResult = new CombinedResultCondition();
                combinedResult.Container.FirstGroup = group;

                // Add the combined result to the parent, which joins them all together with an Any
                parentGroup.Conditions.Add(combinedResult);
            }
        }
    }
}
