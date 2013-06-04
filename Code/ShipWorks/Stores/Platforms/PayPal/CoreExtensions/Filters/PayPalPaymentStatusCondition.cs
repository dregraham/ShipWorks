using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.PayPal.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the PayPal payment status
    /// </summary>
    [ConditionElement("PayPal Payment Status", "PayPalOrder.PaymentStatus")]
    [ConditionStoreType(StoreTypeCode.PayPal)]
    public class PayPalPaymentStatusCondition : EnumCondition<PayPalPaymentStatus>
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            // First we have to get from Order -> PayPalOrder           
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, PayPalOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(GenerateSql(context.GetColumnReference(PayPalOrderFields.PaymentStatus), context));
            }
        }
    }
}
