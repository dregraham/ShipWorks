using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Condition that compares against the online status of an order
    /// </summary>
    [ConditionElement("Store Status", "Order.OnlineStatus", ApplicableTest = "IsApplicable") ]
    public class OnlineStatusCondition : StringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.OnlineStatus), context);
        }

        /// <summary>
        /// Provide a list of standard charge types for the user to choose from
        /// </summary>
        public override ICollection<string> GetStandardValues()
        {
            List<string> values = new List<string>();

            // Add all the store-specifics
            foreach (StoreEntity store in StoreManager.GetAllStores())
            {
                // Get the store type
                StoreType storeType = StoreTypeManager.GetType(store);

                // Add all the online status option for this store
                values.AddRange(storeType.GetOnlineStatusChoices());
            }

            // Sort them
            values.Sort();

            // Remove dupes
            return values.Distinct().ToArray();
        }

        /// <summary>
        /// Determines if the condition is applicable given the specified loaded store types
        /// </summary>
        public static bool IsApplicable(List<StoreType> storeTypes)
        {
            // If you can't see it in the grid, i'd assume you wouldn't be able to filter on it
            return storeTypes.Any(st => st.GridOnlineColumnSupported(OnlineGridColumnSupport.OnlineStatus));
        }
    }
}
