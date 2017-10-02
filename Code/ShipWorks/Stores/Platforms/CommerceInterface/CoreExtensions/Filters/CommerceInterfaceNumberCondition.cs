using System;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.CommerceInterface.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the CommerceInterface OrderNumber field
    /// </summary>
    [ConditionElement("CommerceInterface #", "CommerceInterfaceOrder.CommerceInterfaceOrderNumber")]
    [ConditionStoreType(StoreTypeCode.CommerceInterface)]
    public class CommerceInterfaceNumberCondition : StringCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string orderSql = String.Empty;
            string orderSearchSql = String.Empty;

            // first get from Order -> CommerceInterfaceOrder
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, CommerceInterfaceOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSql = scope.Adorn(GenerateSql(context.GetColumnReference(CommerceInterfaceOrderFields.CommerceInterfaceOrderNumber), context));
            }

            // Combined order search
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, CommerceInterfaceOrderSearchFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSearchSql = scope.Adorn(GenerateSql(context.GetColumnReference(CommerceInterfaceOrderSearchFields.CommerceInterfaceOrderNumber), context));
            }

            return $"{orderSql} OR {orderSearchSql}";
        }
    }
}
