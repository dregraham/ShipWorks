using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Stores;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Condition based on the store of the order
    /// </summary>
    [ConditionElement("Store", "Order.Store")]
    public class StoreCondition : ValueChoiceCondition<long>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StoreCondition()
        {

        }

        /// <summary>
        /// Provides the choices for the user to choose from  This is a list of all store-types that currently
        /// exist in the system.
        /// </summary>
        public override ICollection<ValueChoice<long>> ValueChoices
        {
            get 
            {
                List<ValueChoice<long>> choices = new List<ValueChoice<long>>();

                // Add in all the stores
                foreach (StoreEntity store in StoreManager.GetAllStores())
                {
                    choices.Add(new ValueChoice<long>(store.StoreName, store.StoreID));
                }

                if (Value == 0)
                {
                    Value = choices[0].Value;
                }
                else
                {
                    // Ensure the current value is a choice
                    EnsureCurrentValue(choices);
                }

                // Now they have to be sorted
                choices.Sort(new Comparison<ValueChoice<long>>(
                    delegate(ValueChoice<long> left, ValueChoice<long> right)
                    {
                        return left.Name.CompareTo(right.Name);
                    }));

                return choices;
            }
        }

        /// <summary>
        /// Ensure that the current value is in the choices list.  This is necessary if a store is deleted.
        /// </summary>
        private void EnsureCurrentValue(List<ValueChoice<long>> choices)
        {
            foreach (ValueChoice<long> choice in choices)
            {
                if (choice.Value == Value)
                {
                    return;
                }
            }

            choices.Add(new ValueChoice<long>("(Store Deleted)", Value));
        }

        /// <summary>
        /// Generate the SQL for the condition
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.StoreID), context);
        }
    }
}
