using System;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.ProStores.CoreExtensions.Filters
{
    [ConditionElement("ProStores Confirmation #", "ProStoresOrder.Confirmation")]
    [ConditionStoreType(StoreTypeCode.ProStores)]
    public class ProStoresConfirmationCondition : StringCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string orderSql = String.Empty;
            string orderSearchSql = String.Empty;

            // First we have to get from Order -> ProStoresOrder
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, ProStoresOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSql = scope.Adorn(GenerateSql(context.GetColumnReference(ProStoresOrderFields.ConfirmationNumber), context));
            }

            using (SqlGenerationScope scope = context.PushScope(OrderSearchFields.OrderID, ProStoresOrderSearchFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSearchSql = scope.Adorn(GenerateSql(context.GetColumnReference(ProStoresOrderSearchFields.ConfirmationNumber), context));
            }

            return $"{orderSql} OR {orderSearchSql}";
        }
    }
}
