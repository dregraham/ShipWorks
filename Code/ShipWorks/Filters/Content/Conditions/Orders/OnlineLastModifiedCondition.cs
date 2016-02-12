using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Condition that compares against the online last modified date of an order
    /// </summary>
    [ConditionElement("Last Modified (Online)", "Order.OnlineLastModified", "IsApplicable")]
    public class OnlineLastModifiedCondition : DateCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.OnlineLastModified), context);
        }

        /// <summary>
        /// Determines if the condition is applicable given the specified loaded store types
        /// </summary>
        public static bool IsApplicable(List<StoreType> storeTypes)
        {
            // If you can't see it in the grid, i'd assume you wouldn't be able to filter on it
            return storeTypes.Any(st => st.GridOnlineColumnSupported(OnlineGridColumnSupport.LastModified));
        }
    }
}