using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.Sears.CoreExtensions.Filters
{
    /// <summary>
    /// Sears customer pickup condition
    /// </summary>
    [ConditionElement("Sears Customer Pickup", "SearsOrder.CustomerPickup")]
    [ConditionStoreType(StoreTypeCode.Sears)]
    public class SearsCustomerPickupCondition : BooleanCondition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SearsCustomerPickupCondition()
            : base("Yes", "No")
        {
            Value = false;
        }

        /// <summary>
        /// Generate the SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, SearsOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(SearsOrderFields.CustomerPickup), context));
            }
        }
    }
}