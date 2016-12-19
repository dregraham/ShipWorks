using System;
using System.Linq;
using Interapptive.Shared.Business;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Customers;
using ShipWorks.Filters.Content.Conditions.Customers.PersonName;
using ShipWorks.Filters.Content.Conditions.Special;

namespace ShipWorks.Filters.Search
{
    public class CustomerDefinitionProvider : IFilterDefinitionProvider
    {
        public FilterDefinition GetDefinition(string quickSearchString)
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Customers);

            ConditionGroup group = definition.RootContainer.FirstGroup;

            long customerID;
            if (long.TryParse(quickSearchString, out customerID))
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

            ApplyNameConditions(group, quickSearchString);

            return definition;
        }

        /// <summary>
        /// Apply the conditions based on name
        /// </summary>
        private void ApplyNameConditions(ConditionGroup group, string search)
        {
            // If there are no spaces, look for first and last individually
            if (!search.Contains(' '))
            {
                group.JoinType = ConditionJoinType.Any;

                group.Conditions.Add(CreateAddressCondition(typeof(CustomerFirstNameCondition), BillShipAddressOperator.ShipOrBill, StringOperator.BeginsWith, search));
                group.Conditions.Add(CreateAddressCondition(typeof(CustomerLastNameCondition), BillShipAddressOperator.ShipOrBill, StringOperator.BeginsWith, search));

                group.Conditions.Add(CreateAddressCondition(typeof(CustomerEmailAddressCondition), BillShipAddressOperator.ShipOrBill, StringOperator.BeginsWith, search));
            }
            else
            {
                PersonName name = PersonName.Parse(search);

                group.JoinType = ConditionJoinType.All;

                group.Conditions.Add(CreateAddressCondition(typeof(CustomerFirstNameCondition), BillShipAddressOperator.ShipOrBill, StringOperator.BeginsWith, name.First));
                group.Conditions.Add(CreateAddressCondition(typeof(CustomerLastNameCondition), BillShipAddressOperator.ShipOrBill, StringOperator.BeginsWith, name.Last));
            }
        }

        /// <summary>
        /// Create an AddressCondition of the specified type with the given properties
        /// </summary>
        private Condition CreateAddressCondition(Type type, BillShipAddressOperator addressOperator, StringOperator stringOperator, string targetValue)
        {
            BillShipAddressStringCondition condition = (BillShipAddressStringCondition)Activator.CreateInstance(type);

            condition.AddressOperator = addressOperator;
            condition.Operator = stringOperator;
            condition.TargetValue = targetValue;

            return condition;
        }
    }
}