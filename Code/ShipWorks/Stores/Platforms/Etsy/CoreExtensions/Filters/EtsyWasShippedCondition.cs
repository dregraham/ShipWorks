using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Etsy.Enums;

namespace ShipWorks.Stores.Platforms.Etsy.CoreExtensions.Filters
{
    /// <summary>
    /// Filter for Etsy Shipped Status
    /// </summary>
    [ConditionElement("Etsy Shipment Status", "EtsyOrder.WasShipped")]
    [ConditionStoreType(StoreTypeCode.Etsy)]
    public class EtsyWasShippedCondition : BooleanCondition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EtsyWasShippedCondition()
            : base("Shipped", "Not Shipped")
        {
            Value = false;
        }

        /// <summary>
        /// Generate Sql for Shipped filter
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context", "context required");
            }

            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, EtsyOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(EtsyOrderFields.WasShipped), context));
            }
        }
    }
}