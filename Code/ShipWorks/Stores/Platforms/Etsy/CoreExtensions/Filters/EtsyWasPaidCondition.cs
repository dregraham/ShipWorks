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
    /// Etsy Paid Filter
    /// </summary>
    [ConditionElement("Etsy Payment Status", "EtsyOrder.WasPaid")]
    [ConditionStoreType(StoreTypeCode.Etsy)]
    public class EtsyWasPaidCondition : BooleanCondition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EtsyWasPaidCondition()
            : base("Paid", "Not Paid")
        {
            Value = false;
        }

        /// <summary>
        /// Generate Sql for Paid Filter
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context", "context required");
            }

            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, EtsyOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(EtsyOrderFields.WasPaid), context));
            }
        }
    }
}