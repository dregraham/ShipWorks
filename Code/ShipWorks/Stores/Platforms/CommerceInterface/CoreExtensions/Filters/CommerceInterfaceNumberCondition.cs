using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.CommerceInterface.CoreExtensions.Filters
{
    [ConditionElement("CommerceInterface #", "CommerceInterfaceOrder.CommerceInterfaceOrderNumber")]
    [ConditionStoreType(StoreTypeCode.CommerceInterface)]
    public class CommerceInterfaceNumberCondition : StringCondition
    {
        public override string GenerateSql(SqlGenerationContext context)
        {
            // first get from Order -> CommerceInterfaceOrder
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, CommerceInterfaceOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(GenerateSql(context.GetColumnReference(CommerceInterfaceOrderFields.CommerceInterfaceOrderNumber), context));
            }
        }
    }
}
