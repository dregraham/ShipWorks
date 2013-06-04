using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Stores;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data;
using ShipWorks.Stores.Platforms;
using ShipWorks.Data.Model;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Condition based on the type of store the order is contained
    /// </summary>
    [ConditionElement("Store Type", "Order.StoreType")]
    public class StoreTypeCondition : ValueChoiceCondition<StoreTypeCode>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StoreTypeCondition()
        {
            Value = StoreTypeCode.Invalid;
        }

        /// <summary>
        /// Provides the choices for the user to choose from  This is a list of all store-types that currently
        /// exist in the system.
        /// </summary>
        public override ICollection<ValueChoice<StoreTypeCode>> ValueChoices
        {
            get 
            {
                List<ValueChoice<StoreTypeCode>> choices = new List<ValueChoice<StoreTypeCode>>();

                // Get all the store types in use
                List<StoreTypeCode> storeTypes = GetUtilizedStoreTypes();

                // If it doesn't contain the one already configured, add it.
                if (Value != StoreTypeCode.Invalid)
                {
                    if (!storeTypes.Contains(Value))
                    {
                        storeTypes.Add(Value);
                    }
                }
                else
                {
                    // If currently not set, initialize to the first (if there is one)
                    if (storeTypes.Count > 0)
                    {
                        Value = storeTypes[0];
                    }
                }

                // Create the list of pairs
                foreach (StoreTypeCode typeCode in storeTypes)
                {
                    choices.Add(new ValueChoice<StoreTypeCode>(StoreTypeManager.GetType(typeCode).StoreTypeName, typeCode));
                }

                // If there are none, there are no stores, but we need to show that
                if (choices.Count == 0)
                {
                    choices.Add(new ValueChoice<StoreTypeCode>("(No stores exist)", StoreTypeCode.Invalid));
                }

                // Now they have to be sorted
                choices.Sort(new Comparison<ValueChoice<StoreTypeCode>>(
                    delegate(ValueChoice<StoreTypeCode> left, ValueChoice<StoreTypeCode> right)
                    {
                        return left.Name.CompareTo(right.Name);
                    }));

                return choices;
            }
        }

        /// <summary>
        /// Get a list of all StoreTypeCode's that are currently in use by one or more stores.
        /// </summary>
        private List<StoreTypeCode> GetUtilizedStoreTypes()
        {
            List<StoreTypeCode> storeTypes = new List<StoreTypeCode>();

            foreach (StoreEntity store in StoreManager.GetAllStores())
            {
                StoreTypeCode storeType = (StoreTypeCode) store.TypeCode;

                if (!storeTypes.Contains(storeType))
                {
                    storeTypes.Add(storeType);
                }
            }

            return storeTypes;
        }

        /// <summary>
        /// Generate the SQL for the condition
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(EntityType.StoreEntity, SqlGenerationScopeType.Parent))
            {
                return scope.Adorn(GenerateSql(context.GetColumnReference(StoreFields.TypeCode), context));
            }
        }
    }
}
